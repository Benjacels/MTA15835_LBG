using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour {

	#region SINGLETON PATTERN
	
	private static DialogManager instance;
	
	public static DialogManager Instance {
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<DialogManager>();
				
				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(instance.gameObject);
			}
			
			return instance;
		}
	}
	
	void Awake() {
		if(instance == null)
		{
			//If I am the first instance, make me the Singleton
			instance = this;
			//			monsterObj =  GameObject.FindGameObjectsWithTag ("Monster"); //could be problem move to start
			DontDestroyOnLoad(this);
			
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != instance)
				Destroy(this.gameObject);
		}
	}
	
	#endregion
	
}
