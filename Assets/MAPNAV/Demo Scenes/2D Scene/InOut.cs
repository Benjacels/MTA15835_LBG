//MAPNAV Navigation ToolKit v.1.3.2

using UnityEngine;
using System.Collections;

public class InOut : MonoBehaviour 
{
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			//transform.Find("Status").guiText.text = "IN";
			MainManager.instance.LoadNextScene();
		}
	}
	void OnTriggerExit(Collider other){
		if(other.tag == "Player"){
			//transform.Find("Status").guiText.text = "OUT";
		}
	}
}