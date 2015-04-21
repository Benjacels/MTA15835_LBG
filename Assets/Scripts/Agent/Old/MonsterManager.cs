using UnityEngine;
using System.Collections;

public class MonsterManager : MonoBehaviour {
	
//	private GameObject[] monsterObj;
	private Transform moa;
	
	private static MonsterManager instance;
	
	public static MonsterManager Instance {
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<MonsterManager>();
				
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
	void Start(){
//		monsterObj =  GameObject.FindGameObjectsWithTag ("Monster");
		moa = GameObject.Find("MonsterOfArt").transform;
//		moa.localScale = new Vector3(0.4f,0.4f,1);
//		moa.transform.position = new Vector3(-0.6f,10,0);
		Debug.Log("monster object " + moa.name);
	}

	// ===================== State of GameObjects related to the Monster ===================== //

//	public enum ObjectState {
//		None,
//		Active,
//		Inactive
//	}
//
//	private ObjectState currentObjectState = ObjectState.None;
//	private ObjectState prevObjectState;
//
//	public ObjectState CurrentObjectState 
//	{
//		get { return currentObjectState; }
//	}
//
//	public ObjectState PrevObjectState
//	{
//		get { return prevObjectState; }
//	}
//
//	public void SetObjectState(ObjectState newState)
//	{
//		prevObjectState = currentObjectState;
//		currentObjectState = newState;
//		Debug.Log("<color=green>********** <b>Monster State:</b>\t" + currentObjectState.ToString() + "\n<b>********** Prev Monster State:</b>\t" + prevObjectState.ToString() + "</color>");
//	}

	//This should maybe not be used because it will always be active. So the stuff in gameStart can be moved to initialize
//	public void ActivateMonster()
//	{
//
//
//		switch(GameManager.Instance.CurrentState)
//		{
//		case GameManager.State.GameStart:
//			Activate();
//			break;
//		case GameManager.State.Monster:
//		case GameManager.State.GameOver:
////			if(GameManager.Instance.PrevState == GameManager.State.AR)
////				Activate();
//			
//			break;
//			
//		case GameManager.State.AR:
////			Deactivate();
//			
//			break;
//		}
//	}
//
//	private void Activate()
//	{
//		Debug.Log("<color=green>********** <b>Monster Activated</b> **********</color>");
//		if(monsterObj != null)
//		{
//			foreach(GameObject obj in monsterObj)
//			{
//				if(obj != null)
//					obj.SetActive(true);
//			}
//
//		}
////		SetObjectState(ObjectState.Active);
//	}
//
//	private void Deactivate()
//	{
//		Debug.Log("<color=green>********** <b>Monster Deactivated</b> **********</color>");
//		if(monsterObj != null)
//		{
//			foreach(GameObject obj in monsterObj)
//			{
//				if(obj != null)
//					obj.SetActive(false);
//			}
//
//		}
////		SetObjectState(ObjectState.Inactive);
//	}

}
