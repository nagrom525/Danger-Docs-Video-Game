using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationSystem : MonoBehaviour {
    // mathfx.Berp()
    public GameObject scapleNotificationPrefab;
    public GameObject bucketNotificationPrefab;
    public GameObject sutureNotificationPrefab;
    public GameObject gauzeNotificationPrefab;
    public GameObject anestheticNotificationPrefab;
    public GameObject stickPullOutNotificationPrefab;
    public GameObject bearNotificationPrefab;
    // /////////////////////////////////////////// //
    public float notificationRadius;
    public float notificationPadding = 1.0f;
    public float startYOffset = 100.0f;
    public Vector2 firstNotificationLoc = new Vector2(60.0f, -60.0f);

    enum NotificationSystemState { SHIFTING_RIGHT, SHIFTING_LEFT, ADDING, REMOVING, STATIC}
    public enum NotificationType { SCAPLE, SUTURE, BUCKET, ANESTHETIC, BEAR, GAUZE, STICK_PULL_OUT, NULL}

    private NotificationSystemState current_state = NotificationSystemState.STATIC;

    private List<NotificationType> activeNotifications = new List<NotificationType>(8);
    private Queue<NotificationType> toRemoveBuffer = new Queue<NotificationType>();
    private Queue<NotificationType> toAddBuffer = new Queue<NotificationType>();
    private Hashtable NotificationInstances;
    private Hashtable NotificationStartPositions;

    private NotificationType currentNotification = NotificationType.NULL;
    private int currentNotificationIndex = -1;
    private float stateStartTime = 0.0f;
    private float timeToMoveNotification = 1.0f;

	// Use this for initialization
	void Start () {
        // -- Register main reciepe events -- //
        DoctorEvents.Instance.patientNeedsCutOpen += OnPatientScaple;
        DoctorEvents.Instance.patientDoneCutOpen += OnPatientScapleDone;
        DoctorEvents.Instance.patientNeedsBloodSoak += OnPatientGauze;
        DoctorEvents.Instance.patientDoneBloodSoak += OnPatientGauzeDone;
        DoctorEvents.Instance.patientNeedsStitches += OnPatientSuture;
        DoctorEvents.Instance.patientDoneStitches += OnPatientSutureDone;
        // Register environement events -- //
        DoctorEvents.Instance.onFire += OnFire;
        DoctorEvents.Instance.onFirePutOut += OnFirePutOut;

	}
	
	// Update is called once per frame
	void Update () {
        switch (current_state) {
            case NotificationSystemState.ADDING:
                AddingUpdate();
                break;
            case NotificationSystemState.REMOVING:
                RemovingUpdate();
                break;
            case NotificationSystemState.SHIFTING_LEFT:
                ShiftingLeftUpdate();
                break;
            case NotificationSystemState.SHIFTING_RIGHT:
                ShiftingRightUpdate();
                break;
            case NotificationSystemState.STATIC:
                StaticUpdate();
                break;
        }
	}

    // -- Update functions -- //
    private void ShiftingRightUpdate() {
        float t = tForState();
        if(t > 1.0f) {
            // then we are ready to add object
            current_state = NotificationSystemState.ADDING;
            stateStartTime = Time.time;
        } else {
            moveAllToRightOfIndex(currentNotificationIndex, notificationRadius * 2.0f, t);
        }
    }

    private void ShiftingLeftUpdate() {
        float t = tForState();
        if (t > 1.0f) {
            // then we have finished removing the object
            current_state = NotificationSystemState.STATIC;
            activeNotifications.Remove(currentNotification);
            stateStartTime = Time.time;
        } else {
            moveAllToRightOfIndex(currentNotificationIndex, - (2.0f * notificationRadius), t);
        }
    }

    private void StaticUpdate() {
        if(toAddBuffer.Count > 0) {
            addNotification(toAddBuffer.Dequeue());
        } else if(toRemoveBuffer.Count > 0) {
            removeNotification(toRemoveBuffer.Dequeue());
        }
    }

    private void AddingUpdate() {
        float t = tForState();
        if(t > 1.0f) {
            current_state = NotificationSystemState.STATIC;
            stateStartTime = Time.time;
            activeNotifications.Insert(currentNotificationIndex, currentNotification);
        } else {
            moveNotificationByYOffset(NotificationInstances[currentNotification] as GameObject, -startYOffset, t);
        }
    }

    private void RemovingUpdate() {
        float t = tForState();
        if (t > 1.0f) {
            if (currentNotificationIndex == (activeNotifications.Count - 1)) {
                // then we are in the rightmost position (dont have to worry about shifting)
                current_state = NotificationSystemState.STATIC;
                activeNotifications.Remove(currentNotification);
                stateStartTime = Time.time;
            } else {
                // then we have to worry about shifting
                current_state = NotificationSystemState.SHIFTING_LEFT;
                stateStartTime = Time.time;
            }
        } else {
            moveNotificationByYOffset(NotificationInstances[currentNotification] as GameObject, startYOffset, t);
        }
    }

    // -- MAIN SURGERY RECIEPE -- //
    private void OnPatientScaple(float duration) {
        addNotification(NotificationType.SCAPLE);
    }

    private void OnPatientScapleDone(float duration) {
        removeNotification(NotificationType.SCAPLE);
    }

    private void OnPatientSuture(float duration) {
        addNotification(NotificationType.SUTURE);
    }

    private void OnPatientSutureDone(float duration) {
        removeNotification(NotificationType.SUTURE);
    }

    private void OnPatientGauze(float duration) {
        addNotification(NotificationType.GAUZE);
    }

    private void OnPatientGauzeDone(float duration) {
        removeNotification(NotificationType.GAUZE);
    }


    // -- ENVIRONMENT EVENTS -- //
    private void OnFire(float duration) {
        addNotification(NotificationType.BUCKET);
    }

    private void OnFirePutOut(float duration) {
        removeNotification(NotificationType.BUCKET);
    }

    private GameObject retriveNotificatioinPrefab(NotificationType notificationType) {
        switch (notificationType) {
            case NotificationType.SCAPLE:
                return scapleNotificationPrefab;
            case NotificationType.SUTURE:
                return sutureNotificationPrefab;
            case NotificationType.GAUZE:
                return gauzeNotificationPrefab;
            case NotificationType.STICK_PULL_OUT:
                return stickPullOutNotificationPrefab;
            case NotificationType.ANESTHETIC:
                return anestheticNotificationPrefab;
            case NotificationType.BUCKET:
                return bucketNotificationPrefab;
            case NotificationType.BEAR:
                return bearNotificationPrefab;
            default:
                return null;
        }
    }

    private bool isMainRecipeType(NotificationType notificationType) {
        // should only return true if it is of type main recipe
        switch (notificationType) {
            case NotificationType.SUTURE:
                return true;
            case NotificationType.GAUZE:
                return true;
            case NotificationType.SCAPLE:
                return true;
            case NotificationType.STICK_PULL_OUT:
                return true;
            default:
                return false;
        }
    }

    // -- Actual logic -- //

    private void addNotification(NotificationType typeToAdd) {
        if(current_state == NotificationSystemState.STATIC) {
            GameObject prefabToAdd = retriveNotificatioinPrefab(typeToAdd);
            bool mainRecipeType = isMainRecipeType(typeToAdd);
            currentNotification = typeToAdd;
            stateStartTime = Time.time;
            
            if (mainRecipeType) {
                // then we have to insert to the front of the notifications
                currentNotificationIndex = 0;
                if(activeNotifications.Count > 0) {
                    current_state = NotificationSystemState.SHIFTING_RIGHT;
                } else {
                    current_state = NotificationSystemState.ADDING;
                }
            } else {
                // then we are simply adding it to the back of the notifications
                current_state = NotificationSystemState.ADDING;
                currentNotificationIndex = activeNotifications.Count;
            }

            // instantiate object at currentNotificationIndex, and off screen
            Vector3 rectTransformPos = gameObject.GetComponent<RectTransform>().position;
            GameObject instanceToAdd =  Instantiate(prefabToAdd, this.transform) as GameObject;
            //// set up rectTransform
            RectTransform instanceRectTransform = instanceToAdd.GetComponent<RectTransform>();
            instanceRectTransform.anchorMin = new Vector2(0.0f, 1.0f);
            instanceRectTransform.anchorMax = new Vector2(0.0f, 1.0f);
            instanceRectTransform.pivot = new Vector2(0.5f, 0.5f);
            instanceRectTransform.anchoredPosition = new Vector2(getStartXFromIndex(currentNotificationIndex), firstNotificationLoc.y);

            NotificationInstances[typeToAdd] = instanceToAdd;


        } else {
            toAddBuffer.Enqueue(typeToAdd);
        }
    }

    private void removeNotification(NotificationType typeToRemove) {
        if (current_state == NotificationSystemState.STATIC) {
            currentNotification = typeToRemove;
            int index = findIndexFromList(activeNotifications, typeToRemove);
            currentNotificationIndex = index;
            stateStartTime = Time.time;
            current_state = NotificationSystemState.REMOVING;
        } else {
            toRemoveBuffer.Enqueue(typeToRemove);
        }
    }
    
    // returns -1 if the element is not in the list
    private int findIndexFromList(List<NotificationType> list, NotificationType type) {
        for(int i = 0; i < list.Count; ++i) {
            if(list[i] == type) {
                return i;
            }
        }
        return -1;
    }

    // moves all the notification instances to the right (inclusive) of index
    // in the active notification list by xToMove offset
    private void moveAllToRightOfIndex(int index, float xToMoveTotal, float t) {
        for (int i = index; i < activeNotifications.Count; ++i) {
            GameObject instance = NotificationInstances[activeNotifications[i]] as GameObject;
            Vector3 currPos = instance.GetComponent<RectTransform>().anchoredPosition;
            float startXPos = getStartXFromIndex(i);
            instance.GetComponent<RectTransform>().anchoredPosition = new Vector3(Mathfx.Hermite(startXPos, startXPos + xToMoveTotal, t), currPos.y, currPos.z);
        }
    }

    private void moveNotificationByYOffset(GameObject notification, float yToMoveTotal, float t) {
        Vector3 currPos = notification.GetComponent<RectTransform>().anchoredPosition;
        float startPositionY = GetComponent<RectTransform>().anchoredPosition.y + startYOffset;
        notification.GetComponent<RectTransform>().anchoredPosition = new Vector3(currPos.x, Mathfx.Berp(startPositionY, startPositionY + yToMoveTotal, t), currPos.z);
    }

    // gets the original start position based on the index
    private float getStartXFromIndex(int i) {
        float paddingComponent = notificationPadding * i;
        float notificationComponent = (notificationRadius * 2) * i;
        return firstNotificationLoc.x + paddingComponent + notificationComponent;
    }

    private float tForState() {
        return (Time.time - stateStartTime) / (timeToMoveNotification / 2.0f);
    }
}
