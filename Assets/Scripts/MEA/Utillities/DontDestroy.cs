using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

	void Awake(){
		//preserve object and script
		DontDestroyOnLoad(this.gameObject);
	}
}
