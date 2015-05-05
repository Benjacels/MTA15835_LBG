using UnityEngine;
using System.Collections;

public class ShowHide : MonoBehaviour {

public GameObject SktHansGade;
public GameObject Portal;

	// Use this for initialization
	void Start () {
		
			if(MainManager.instance.riddlesFirst == true) {
				SktHansGade.SetActive(false);
				Portal.SetActive(true);
			}

			if(MainManager.instance.riddlesFirst == false) {
				SktHansGade.SetActive(true);
				Portal.SetActive(false);
			}

	}
	
}
