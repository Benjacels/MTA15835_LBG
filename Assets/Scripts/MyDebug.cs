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
	private Toggle[] otherToggles;
	private Toggle chosenToggle;

	// Use this for initialization
	void Start () {
		goBtn.interactable = false;

		txtlogger = GameObject.Find("LogManager").GetComponent<TxtLogger>();
		txtlogger.log ("--- Entered debug scene ---");

		otherToggles = GameObject.Find("OtherScenes").GetComponentsInChildren<Toggle>();
	}

	public void inputChange(){
		goBtn.interactable = false;

		int numberOfTogglesChosen = togglesChosen ();

		//riddle chosen
		if( (playMap.isOn == true && mapPlay.isOn == false) || (playMap.isOn == false && mapPlay.isOn == true) ){
			if(numberOfTogglesChosen == 0 && riddleID.text.Length > 0){
				goBtn.interactable = true;
			}
			if(riddleID.text.Length == 0 && numberOfTogglesChosen == 1){
				goBtn.interactable = true;
			}
		}
	}

	private int togglesChosen(){
		int isOnNumber = 0;
		foreach (Toggle t in otherToggles) {
			if(t.isOn == true){
				isOnNumber++;
			}
		}
		return isOnNumber;
	}

	public void goBtnPressed(){
		txtlogger.log("Debug, go btn pressed");

		MainManager.instance.InDebug = true;


		if(playMap.isOn == true){
			MainManager.instance.riddlesFirst = true;
			txtlogger.log("Debug, navigation type: Play --> Map");

			if(riddleID.text.Length > 0){
				goToRiddle();
			}else{
				chosenToggle = returnChosenToggle();
				goToChosen(chosenToggle.name);
			}


		}else if(mapPlay.isOn == true){
			MainManager.instance.riddlesFirst = false;
			txtlogger.log("Debug, navigation type: Map --> Play");

			if(riddleID.text.Length > 0){
				goToRiddle();
			}else{
				chosenToggle = returnChosenToggle();
				goToChosen(chosenToggle.name);
			}
		}
	}

	public void goToChosen(string chosen){
		print ("go to: " + chosen);
		txtlogger.log("Debug, goes to scene: " + chosen);
		Application.LoadLevel(chosen);
	}

	private Toggle returnChosenToggle(){
		int idChosenToggle = -1;
		int iterator = 0;
		foreach (Toggle tog in otherToggles) {
			if(tog.isOn == true){
				idChosenToggle = iterator;
			}
			iterator++;
		}
		print ("---- "+otherToggles [idChosenToggle].name);
		return otherToggles[idChosenToggle];
	}

	private void goToRiddle(){
		MainManager.instance.RiddleDebugIndex = int.Parse(riddleID.text);
		txtlogger.log("Debug, riddles scene chosen, with riddle id: " + riddleID.text);
		Application.LoadLevel(riddleScene);
	}
}