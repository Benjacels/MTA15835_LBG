using UnityEngine;
using System.Collections;

public class AnimCam : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PanDown()
    {
        LeanTween.moveY(gameObject, -130, 3).setEase(LeanTweenType.easeInOutSine).setOnComplete(BeginAnimations);
    }

    void BeginAnimations()
    {
        MainManager.instance.OnDialogueButtonClick();
    }


}
