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

	// Use this for initialization
	void Start () {
		_streetarts.Add ("hjelmerstald");
		_streetarts.Add ("pyramid");
		_streetarts.Add ("space");
		infoScreen.SetActive (isInfoscreenActive);

		newStreetartNotification ();
	}
	
	// Update is called once per frame
	public void toggleInfoscreen(){
		if (isInfoscreenActive == false) {
			infoScreen.SetActive(true);
			isInfoscreenActive = true;
			notificationImg.gameObject.SetActive (false);
		}else if(isInfoscreenActive == true){
			infoScreen.SetActive(false);
			isInfoscreenActive = false;
		}
	}

	public void streetartToShow (string streetart){
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
		if(toShow == "pyramid"){
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
		notificationImg.gameObject.SetActive (true);
	}
}
