using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfoscreenController : MonoBehaviour {

	public GameObject infoScreen;
	private bool isInfoscreenActive = false;
	public Button _hjelmerstaldBtn;
	public Button _pyramidBtn;
	public Button _spaceBtn;
	public GameObject hjelmerInfo;
	public GameObject pyramidInfo;
	public GameObject spaceInfo;
	public Image notificationImg;
	private List<string> _streetarts = new List<string>();
	public Sprite firstNotificationImg;
	public Sprite secondNotificationImg;
	public Sprite thridNotificationImg;
	public Sprite pyramidImg;
	public Sprite spaceImg;
	//public Sprite 

	// Use this for initialization
	void Start () {
		infoScreen.SetActive (isInfoscreenActive);

		updateSeenStreetart ();
		updateStreetArtNotification ();

		//newStreetartNotification ();
		//newStreetartNotification (); 
		//newStreetartNotification ();
	}

	public void updateSeenStreetart(){
		_streetarts = MainManager.instance.artsSeen;

		_streetarts.Add("hjelmerstald");
		//_streetarts.Add("pyramide");
		//_streetarts.Add("space");
		foreach(string s in _streetarts){ 
			print ("updated, seen streetart: " + s);
            if (s == "pyramide")
		    {
                print("pyramid unlocked");
                _pyramidBtn.image.sprite = pyramidImg;
		    }
		    if (s == "space")
		    {
		        print ("space unlocked");
			    _spaceBtn.image.sprite = spaceImg;
		    }
		}

	}
    
	// Update is called once per frame
	public void toggleInfoscreen(){
		if (isInfoscreenActive == false) {
			infoScreen.SetActive(true);
			isInfoscreenActive = true;
			notificationImg.gameObject.SetActive (false);
			MainManager.instance.NumberOfArtNotification = 0;
			updateSeenStreetart();
		}else if(isInfoscreenActive == true){
			infoScreen.SetActive(false);
			isInfoscreenActive = false;
		}
	}

	public void streetartToShow (string streetart){
		print ("streetartToShow");
		int indexInArrayToShow = _streetarts.IndexOf(streetart);
		if (indexInArrayToShow != -1) {
			showStreetart (_streetarts[indexInArrayToShow]);
		}
	}

	private void showStreetart(string toShow){
		closeStreetarts ();

		if(toShow == "hjelmerstald"){
			hjelmerInfo.SetActive (true);
		}
		if(toShow == "pyramide"){
			pyramidInfo.SetActive (true);
		}
		if(toShow == "space"){
			spaceInfo.SetActive (true);
		}
	}

	private void closeStreetarts (){
		hjelmerInfo.SetActive (false);
		pyramidInfo.SetActive (false);
		spaceInfo.SetActive (false);
	}

	public void newStreetartNotification(){
		MainManager.instance.NumberOfArtNotification = MainManager.instance.NumberOfArtNotification + 1;
		updateStreetArtNotification ();
	}

	private void updateStreetArtNotification(){
		print ("MainManager.instance.NumberOfArtNotification: " + MainManager.instance.NumberOfArtNotification);
		if(MainManager.instance.NumberOfArtNotification == 1){
			notificationImg.sprite = firstNotificationImg;
		}else if(MainManager.instance.NumberOfArtNotification == 2){
			notificationImg.sprite = secondNotificationImg;
		}else if(MainManager.instance.NumberOfArtNotification == 3){
			notificationImg.sprite = thridNotificationImg;
		}
		if(MainManager.instance.NumberOfArtNotification > 0){
			notificationImg.gameObject.SetActive (true);
		}
	}
}
