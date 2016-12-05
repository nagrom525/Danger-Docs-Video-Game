using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour {
    public string spriteSheetName;
    public Sprite staticImage;
    public int fps = 60;
    public bool reverseAnimation = false;
    public bool loop = false;
    public bool active = false;
    public int numFrames {
        get {
            if(frameSprites != null) {
                return frameSprites.Length;
            } else {
                return 0;
            }
            
        }
        private set { }
    }

    private Sprite[] frameSprites;
    private Image imageRenderer;
    private float startTime;

    void Awake() {
        imageRenderer = GetComponent<Image>();
        frameSprites = Resources.LoadAll<Sprite>(spriteSheetName);
    }

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        print("update");
        if (active) {
            float t = (Time.time - startTime) / getAnimationTime();
            if (t >= 1.0) {
                if (loop) {
                    startAnimation(loop);
                }
            } else {
                int currFrame = Mathf.FloorToInt(t * frameSprites.Length);
                if (reverseAnimation) {
                    currFrame = frameSprites.Length - currFrame - 1;
                }
                imageRenderer.sprite = frameSprites[currFrame];
            }
        }
	}

    public void startAnimation(bool loop) {
        this.loop = loop;
        active = true;
        startTime = Time.time;
    }

    public void endAnimation() {
        active = false;
        imageRenderer.sprite = staticImage;
    }

    private float getAnimationTime() {
        return ((float)frameSprites.Length) / fps;
    }

    public void updateFrames(string framesName) {
        spriteSheetName = framesName;
        frameSprites = Resources.LoadAll<Sprite>(spriteSheetName);
    }
}
