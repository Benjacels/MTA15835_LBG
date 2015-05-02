using UnityEngine;
using System.Collections;
using Vuforia;

public class ARManager : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    if (!MainManager.instance.hasSeenBear)
	        MainManager.instance.currentCanvas.transform.Find("Bear").active = true;
        else if(MainManager.instance.hasSeenBear)
            MainManager.instance.currentCanvas.transform.Find("Kid").active = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTrackingFound()
    {
        
    }
}
