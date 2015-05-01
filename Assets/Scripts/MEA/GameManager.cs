using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//References
//www.dotnetperls.com/singleton
//http://msdn.microsoft.com/en-us/library/ff650316.aspx

public sealed class GameManager : MonoBehaviour {

	#region FSM
	
	// ---------------- ManagerState ---------------- //
	private State currentState = State.None; 
	private State prevState;
	
	public enum State {
		None = -1,
		SplashScreen = 0,
		GameStart = 1,
		Monster = 2,
		AR = 3,
		GameOver = 4,
		Test
	}
	
	public State CurrentState {
		get { return currentState; }
		set {
			prevState = currentState;
			currentState = value; 
		}
	}
	
	public State PrevState {
		get { return prevState; }
	}

	// Used to set state and level
	public void SetState(State newState){
		prevState = currentState;
		currentState = newState;
		Application.LoadLevel((int)currentState);
	}

	// ---------------- GameState ---------------- //
	private GameState currentGameState = GameState.None;
	
	public enum GameState {
		None,
		CameraOff,
		CameraOn,
		NotFound,
		ArtworkFound,
		ArtworkRecognized,
		Restart, //transition to gameStart
		Quit //transition to GameOver
	}
	
	public GameState CurrentGameState {
		get { return currentGameState; }
		set { 
			PrevGameState = currentGameState;
			currentGameState = value;
		}
	}
	
	public GameState PrevGameState { get; set; }
	
	#endregion

	#region Auto-implemented properties

	public string PlayerName { get; set; }
	public int PlayerID { get; set; }
	public string SampleDate { get; set; }
	public bool TrackableFound { get; set; } //Auto-implemented properties
	public string TrackableName { get; set; }
	public int TrackableID { get; set; }
	public int Achievement { get; set; }
	public bool InitializingCamera { get; set; }
	public bool InitializingTrackableEventHandler { get; set; }
	public bool ButtonPress { get; set; }
	public bool Relational;
	public bool isTesting;

	#endregion

	#region public lists
	[HideInInspector]
	public List<string> artworksVisited; //trackable name from customTrackableEventHandler
	[HideInInspector]
	public List<string> artworkID; //trackable id from customTrackableEventHandler
	[HideInInspector]
	public List<int> trackableIDList;
	[HideInInspector]
	public List<string> role; //will be a enum converted. From gameController script
	[HideInInspector]
	public List<string> continueQuest; //logged at startup and at GUI button press
	[HideInInspector]
	public List<string> artworkRecognized; //datetime from customTrackableEventHandler
	[HideInInspector]
	public List<ArtworkToBeFound> artworksToBeFound; //Could get list from web db
	[HideInInspector]
	public List<string> secondaryArtworksToBeFound;
	[HideInInspector]
	public List<string> FoundBodyPart;
	#endregion

	public enum Priority { Primary, Secondary, None }
	
	#region SINGLETON desing pattern

	private static GameManager instance;

	public static GameManager Instance {
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<GameManager>();
				
				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(instance.gameObject);
			}
			
			return instance;
		}
	}

	void Awake()
	{
		if(instance == null)
		{
			//If I am the first instance, make me the Singleton
			instance = this;
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

	#region FSM

	// ---------------- ManagerState ---------------- //


	// ---------------- GameState ---------------- //


	#endregion


	#region METHODS

//	public void OnApplicationQuit(){
//		instance = null;
//	}

	public void Initialize(){
		Debug.Log("Initialize Game");

		PrimaryArtworks();
		FindSecondaryArtworks(); // Should be implemented as primaryArtworks but with associations to other artworks
		artworksVisited = new List<string>();
		artworkID = new List<string>();
		role = new List<string>();
		continueQuest = new List<string>();
		artworkRecognized = new List<string>();
		trackableIDList = new List<int>();
		FoundBodyPart = new List<string>();
		SampleDate = DateTime.Today.ToString(Utilities.DateFormat);
	}

	private void PrimaryArtworks(){
		
		artworksToBeFound = new List<ArtworkToBeFound>();

		// Primary artworks to be found
		artworksToBeFound.Add( new ArtworkToBeFound() { ArtworkName = "RudeOlaf_SanktGeorgOgDragen", BodyPart = "Arm_Upper_Right" });
		artworksToBeFound.Add( new ArtworkToBeFound() { ArtworkName = "MortensenRichard_BomberOverLande", BodyPart = "Arm_Lower_Right" });
		artworksToBeFound.Add( new ArtworkToBeFound() { ArtworkName = "MortensenRichard_JegMyrder", BodyPart = "Arm_Upper_Left" });
		artworksToBeFound.Add( new ArtworkToBeFound() { ArtworkName = "MagnelliAlberto_RepercussionAigue", BodyPart = "Arm_Lower_Left" });
		artworksToBeFound.Add( new ArtworkToBeFound() { ArtworkName = "PedersenCarlHenning_DetRoedeSkib", BodyPart = "Body" });
		
		artworksToBeFound.Sort();
	}

	private void FindSecondaryArtworks(){
		secondaryArtworksToBeFound = new List<string>();
		//NOT INCLUDED IN THIS PROTOTYPE! 
		//which artworks can be amoung the secondary artworks should be chosen on the website backend,
		//this is due to the exhibition constantly changing so, 
		//the staff should make shure that this list only contains artworks on exhibit.
		//the logic here should be based on the metadata of the primary artworks i.e. period and/or theme

		//INCLUDED IN THIS PROTOTYPE!
		//manual entry based on period and/or theme => learning
		secondaryArtworksToBeFound.Add("DewasneJean_Aurora");
//		secondaryArtworksToBeFound.Add("BrinchAnders_LoveInSpace");
//		secondaryArtworksToBeFound.Add("BrinchAnders_PlayingAllDay");
//		secondaryArtworksToBeFound.Add("JacobsenEgill_Graeshoppedans");
//		secondaryArtworksToBeFound.Add("CarlssonHarry_EtGeniKomTilVerden");
//		secondaryArtworksToBeFound.Add("JornAsger_DidaskaII");
	}

	public void InitializePlayer(int playerID){
		Debug.Log("<color=magenta>Initializing Player</color>");
		PlayerName = "n/a";
		Debug.Log("Player name: " + PlayerName);
		PlayerID = playerID + 1;
		Debug.Log("Player ID: " + PlayerID);
	}

	//not implemented but could retrieve name, age and other user info in beginning of game
	public void InitializePlayer(string playerName, int playerID){
		Debug.Log("<color=magenta>Initializing Player</color>");
		PlayerName = playerName;
		Debug.Log("Player name: " + PlayerName);
		PlayerID = playerID + 1;
		Debug.Log("Player ID: " + PlayerID);
	}

	public void StartState(){

		Debug.Log("<color=magenta><b>Creating the start game state</b></color>");

		// Delete previous entries in list if there are any
		artworksVisited.Clear();
		artworkID.Clear();
		role.Clear();
		continueQuest.Clear();
		artworkRecognized.Clear();
		trackableIDList.Clear();
		Achievement = 0;
		FoundBodyPart.Clear();
//		CurrentBodyPartFound = string.Empty;
	}

	#endregion
}
