using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationSystem : MonoBehaviour {
    public GameObject scapleNotificationPrefab;
    public GameObject bucketNotificationPrefab;
    public GameObject sutureNotificationPrefab;
    public GameObject gauzeNotificationPrefab;
    public GameObject anestheticNotificationPrefab;
    public GameObject stickPullOutNotificationPrefab;
    public GameObject bearNotificationPrefab;
    // /////////////////////////////////////////// //
    public float notificationRadius;

    enum NotificationSystemState { SHIFTING_RIGHT, SHIFTING_LEFT, ADDING, REMOVING, STATIC}

    private LinkedList<Notification> activeNotifications;
    private LinkedList<Notification> toRemoveBuffer;
    private LinkedList<Notification> toAddBuffer;

    private Notification currentlyAdding = null;
    private Notification currentlyRemoving = null;
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
	
	}

    // -- MAIN SURGERY RECIEPE -- //
    private void OnPatientScaple(float duration) {
        addNotification(true, Notification.Type.SCAPLE);

    }

    private void OnPatientScapleDone(float duration) {
        removeNotification(Notification.Type.SCAPLE);
    }

    private void OnPatientSuture(float duration) {

    }

    private void OnPatientSutureDone(float duration) {

    }

    private void OnPatientGauze(float duration) {

    }

    private void OnPatientGauzeDone(float duration) {

    }


    // -- ENVIRONMENT EVENTS -- //
    private void OnFire(float duration) {
        addNotification(false, Notification.Type.BUCKET);
    }

    private void OnFirePutOut(float duration) {
        removeNotification(Notification.Type.BUCKET);
    }

    private GameObject retriveNotificatioinPrefab(Notification.Type notificationType) {
        switch (notificationType) {
            case Notification.Type.SCAPLE:
                return scapleNotificationPrefab;
            case Notification.Type.SUTURE:
                return sutureNotificationPrefab;
            case Notification.Type.GAUZE:
                return gauzeNotificationPrefab;
            case Notification.Type.STICK_PULL_OUT:
                return stickPullOutNotificationPrefab;
            case Notification.Type.ANESTHETIC:
                return anestheticNotificationPrefab;
            case Notification.Type.BUCKET:
                return bucketNotificationPrefab;
            case Notification.Type.BEAR:
                return bearNotificationPrefab;
            default:
                return null;
        }
    }

    // -- Actual logic -- //

    private void addNotification(bool isMainRecipe, Notification.Type typeToAdd) {
        GameObject prefabToAdd = retriveNotificatioinPrefab(typeToAdd);
    }

    private void removeNotification(Notification.Type typeToRemove) {
        
    }


    private class Notification {
        public enum Type { SCAPLE, SUTURE, BUCKET, ANESTHETIC, BEAR, GAUZE, STICK_PULL_OUT}
        public GameObject instance;
        public float radius;
        public Type type;

        public Notification(GameObject instance, float radius, Type type) {
            this.instance = instance;
            this.radius = radius;
            this.type = type;
        }
    }
}
