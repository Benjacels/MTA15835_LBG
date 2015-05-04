using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyDebug : MonoBehaviour {
	TxtLogger txtlogger;
	public InputField riddleID;
	public Toggle playMap;
	public Toggle mapPlay;
	public Button goBtn;
	public string riddleScene;
	public string mapScene;

	// Use this for initialization
	void Start () {
		goBtn.interactable = false;

		txtlogger = GameObject.Find("LogManager").GetComponent<TxtLogger>();
		txtlogger.log ("--- Entered debug scene ---");
	}

	public void inputChange(){
		goBtn.interactable = false;

		if(playMap.isOn == true && mapPlay.isOn == false && riddleID.text.Length > 0){
			goBtn.interactable = true;
		}
		if(playMap.isOn == false && mapPlay.isOn == true){
			goBtn.interactable = true;
		}

	}

	public void goBtnPressed(){
		txtlogger.log("Debug, start btn pressed");
		txtlogger.log("Debug, riddle id chosen: " + riddleID.text);

		/*
		 * 
		 * write  riddle id to singleton
		 * 
		 */

		if(playMap.isOn == true){
			MainManager.instance.riddlesFirst = true;
			txtlogger.log("Debug, navigation type: Play --> Map");
			Application.LoadLevel(riddleScene);
		}else if(mapPlay.isOn == true){
			MainManager.instance.riddlesFirst = false;
			txtlogger.log("Debug, navigation type: Map --> Play");
			Application.LoadLevel(mapScene);
		}
	}
}
