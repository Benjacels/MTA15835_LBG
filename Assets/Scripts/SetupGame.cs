﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetupGame : MonoBehaviour {
	private Toggle[] toggles;
	private Toggle playMap;
	private string playMapString = "Toggle Play - Map";
	private Toggle mapPlay;
	private string mapPlayString = "Toggle Map - Play";
	public Button startGameBtn;
	private bool newSessionCreated = false;
	public string startScene;

	// Use this for initialization
	void Start () {
		startGameBtn.interactable = false;
		toggles = Toggle.FindObjectsOfType (typeof(Toggle)) as Toggle[];
		foreach (Toggle t in toggles) {
			if(t.name == playMapString){
				playMap = t;
			}else if(t.name == mapPlayString){
				mapPlay = t;
			}else{
				print ("someone changed the toggles' names");
			}
		}

		print ("mainManager: " + MainManager.instance.riddlesFirst);

	}

	public void navigationTypeValueChanged(Toggle t){
		// writes to singleton what navigation types i chosen
		toggleStartGameButton ();
	}

	private void toggleStartGameButton(){
		if(playMap.isOn == true && mapPlay.isOn == false && newSessionCreated == true){
			startGameBtn.interactable = true;
		}else if(mapPlay.isOn == true && playMap.isOn == false && newSessionCreated == true){
			startGameBtn.interactable = true;
		}else{
			startGameBtn.interactable = false;
		}
	}

	public void setNewSessionCreated(bool value){
		newSessionCreated = value;
		toggleStartGameButton ();
	}


	public void startGame(string sceneName){
		if(playMap.isOn == true){
			MainManager.instance.riddlesFirst = true;
		}else if(mapPlay.isOn == true){
			MainManager.instance.riddlesFirst = false;
		}
		print ("mainManager: " + MainManager.instance.riddlesFirst);
		if (playMap.isOn == true || mapPlay.isOn == true) {
			Application.LoadLevel(startScene);
		}
	}
}