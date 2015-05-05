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
	private int numberOfNotifications;
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

		//newStreetartNotification ();
		//newStreetartNotification ();
		//newStreetartNotification ();
		streetartToShow ("hjelmerstald");
	}

	public void updateSeenStreetart(){
		_streetarts = MainManager.instance.artsSeen;
		_streetarts.Add("hjelmerstald");
		_streetarts.Add("pyramide");
		_streetarts.Add("space");
		foreach(string s in _streetarts){
			print ("______ _streetarts: "+ s);
		}
	}
	
	// Update is called once per frame
	public void toggleInfoscreen(){
		if (isInfoscreenActive == false) {
			infoScreen.SetActive(true);
			isInfoscreenActive = true;
			notificationImg.gameObject.SetActive (false);
			numberOfNotifications = 0;
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
			print ("pyramid unlocked");
			_pyramidBtn.image.sprite = pyramidImg;
		}
		if(toShow == "space"){
			spaceInfo.SetActive (true);
			print ("space unlocked");
			_spaceBtn.image.sprite = spaceImg;
		}
	}

	private void closeStreetarts (){
		hjelmerInfo.SetActive (false);
		pyramidInfo.SetActive (false);
		spaceInfo.SetActive (false);
	}

	public void newStreetartNotification(){
		numberOfNotifications++;
		if(numberOfNotifications == 1){
			notificationImg.sprite = firstNotificationImg;
		}else if(numberOfNotifications == 2){
			notificationImg.sprite = secondNotificationImg;
		}else if(numberOfNotifications == 3){
			notificationImg.sprite = thridNotificationImg;
		}
		notificationImg.gameObject.SetActive (true);
	}
}
