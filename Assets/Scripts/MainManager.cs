using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class MainManager : MonoBehaviour {

    public static MainManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MainManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
                
            return _instance;
        }
    }

    public delegate void DialogueEvent(int clickCount);
    public event DialogueEvent OnDialogueEvent;

    private static MainManager _instance;

    private State currentState = State.None;
	[HideInInspector]
	public State prevState = State.None;
    private Choices currentChoice;
    
    public delegate void ChoiceEvent(Choices choice);
    public event ChoiceEvent OnChoiceEvent;

    public delegate void ArtEvent();
    public event ArtEvent OnArtEvent;

    public delegate void Ask();
    public event Ask OnAskEvent;

    public float FriendPoints;
    public float FuelPoints;

    public bool riddlesFirst;

    [HideInInspector]
    public bool hasSeenBear = false;

    public bool fromBeginning;

    public List<Choices> choices = new List<Choices>();

    [HideInInspector]
    public List<string> artsSeen = new List<string>();

	private int numberOfArtNotification;

    private int _dialogueClicks = 4;
    private int _dialogueClickCount = 0;
    private int _nextScene = 0;

    private int _choicePoints = 10;

    private TxtLogger _txtLogger;

    public Canvas currentCanvas;

    private bool _finalScene;

    private Button _choiceFriends;
    private Button _choiceFuel;
    private Button _wayFinder;

	private int riddleDebugIndex;
	private bool inDebug;

    public enum State
    {
        None = -1,
        Logging = 0,
        Start = 1,
        AR = 2,
        BearDialogue = 3, //Street art
        Riddles = 4,
        Map = 5,
        KidDialogue = 6, //Street art
        End = 7
    }

    public enum Choices
    {
        Fuel,
        Friends
    }

	//numberOfArtNotification
	public int NumberOfArtNotification{
		get { return numberOfArtNotification; }
		set
		{
			numberOfArtNotification = value;
		}
	}

    public Choices CurrentChoice
    {
        get { return currentChoice; }
        set
        {
            currentChoice = value;
        }
    }

    public State CurrentState
    {
        get { return currentState; }
        set
        {
            prevState = currentState;
            currentState = value;
        }
    }
        
    void Awake()
    {
        if (_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != _instance)
                Destroy(this.gameObject);
        }

        if (GameObject.Find("Canvas") != null)
            currentCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        ArrangeScenes();
    }

    void OnLevelWasLoaded(int level)
    {
        if (GameObject.Find("Canvas") != null)
            currentCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        ArrangeScenes();

        if (MainManager.instance.currentState == State.KidDialogue || MainManager.instance.currentState == State.End)
        {
            if (choices.Any())
            {
                switch (MainManager.instance.choices[choices.Count-1])
                {
                    case Choices.Fuel:
                        FuelPoints += 10;
                        break;
                    case Choices.Friends:
                        FriendPoints += 10;
                        print(FriendPoints);
                        break;
                }
                
            }
        }

        _txtLogger = GameObject.FindObjectOfType<TxtLogger>();
        if (currentState != State.Logging && fromBeginning)
            _txtLogger.log(currentState.ToString());
    }

	/// <summary>
	/// Gets or sets the index of the riddle debug.   Peder ændrede her!!!
	/// </summary>
	/// <value>The index of the riddle debug.</value>
	
	public int RiddleDebugIndex{
		get { return riddleDebugIndex; }
		set
		{
			riddleDebugIndex = value;
		}
	}
	public bool InDebug{
		get { return inDebug; }
		set
		{
			inDebug = value;
		}
	}
	/*public artsVisited{

	}*/
	
	//

    // Use this for initialization
    void Start()
    {
        _txtLogger = GameObject.FindObjectOfType<TxtLogger>();
        _txtLogger.log(currentState.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDialogueButtonClick()
    {
        if (_dialogueClickCount < _dialogueClicks)
        {
            OnDialogueEvent(_dialogueClickCount);
            _dialogueClickCount++;
        }
        else
            LoadNextScene();
    }

    //TODO: FADE THE BUTTONS
    public void OnChoiceButtonClick(string choice)
    {
        if (choice == "Fuel")
        {
            OnChoiceEvent(Choices.Fuel);
            choices.Add(Choices.Fuel);
			CurrentChoice = Choices.Fuel;
        }
            
        else if (choice == "Friends")
        {
            OnChoiceEvent(Choices.Friends);
            choices.Add(Choices.Friends);
			CurrentChoice = Choices.Fuel;
        }

        _txtLogger.log("Choice: " + CurrentChoice + ", " + "Scene: " + CurrentState);

        _choiceFuel.active = false;

        _choiceFriends.active = false;

        _wayFinder.enabled = true;
        _wayFinder.image.enabled = true;
    }

    public void OnAskCharacter(Button buttonClicked)
    {
        OnAskEvent();
        
        buttonClicked.active = false;

        _choiceFuel.enabled = true;
        _choiceFuel.image.enabled = true;

        _choiceFriends.enabled = true;
        _choiceFriends.image.enabled = true;
    }

    void ArrangeScenes()
    {
        switch (Application.loadedLevel)
        {
            case 0 :
                currentState = State.Logging;
                _nextScene = 1;
                break;
            case 1 :
                currentState = State.Start;
                _dialogueClicks = 4;
                _nextScene = 2;

                //TODO: Put this into case 0
                break;
            case 2 :
                currentState = State.AR;
                if (!hasSeenBear)
                    _nextScene = 3;
                else
                    _nextScene = 6;
                break;
            case 3 :
                currentState = State.BearDialogue;
                _dialogueClicks = 5;

                if (riddlesFirst)
                    _nextScene = 4;
                else
                    _nextScene = 5;

                _choiceFriends = currentCanvas.transform.FindChild("Choice_Friends").GetComponent<Button>();
                _choiceFuel = currentCanvas.transform.FindChild("Choice_Fuel").GetComponent<Button>();
                _wayFinder = currentCanvas.transform.FindChild("Wayfinder").GetComponent<Button>();

                break;
            case 4 :
                currentState = State.Riddles;

                if (riddlesFirst)
                    _nextScene = 2;
                else
                    _nextScene = 7;

                break;
            case 5 :
                currentState = State.Map;

                if (riddlesFirst)
                    _nextScene = 7;
                else
                    _nextScene = 2;

                break;
            case 6:
                currentState = State.KidDialogue;
                _dialogueClicks = 7;

                if (riddlesFirst)
                    _nextScene = 5;
                else
                    _nextScene = 4;

                _choiceFriends = currentCanvas.transform.FindChild("Choice_Friends").GetComponent<Button>();
                _choiceFuel = currentCanvas.transform.FindChild("Choice_Fuel").GetComponent<Button>();
                _wayFinder = currentCanvas.transform.FindChild("Wayfinder").GetComponent<Button>();

                break;
            case 7:
                currentState = State.End;
                NewStreetArt();
                _finalScene = true;

                break;
        }
    }

    public void LoadNextScene()
    {
        if (_finalScene)
        {
            _txtLogger.log("Final fuel points: " + FuelPoints);
            _txtLogger.log("Final friend points: " + FriendPoints);
            Application.Quit();
        }
        else
        {
			prevState = CurrentState;
            
            Application.LoadLevel(_nextScene);
        }
            
    }

    public void NewStreetArt()
    {
        OnArtEvent();
    }
}
