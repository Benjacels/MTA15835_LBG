using UnityEngine;
using System.Collections;

public class MainManager : MonoBehaviour {

    public static MainManager instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<MainManager>();
            return _instance;
        }
    }

    private static MainManager _instance;

    private State currentState = State.None;
    private State prevState;

    public delegate void DialogueEvent(int clickCount);
    public event DialogueEvent OnDialogueEvent;

    [HideInInspector]
    public float Points;

    private int _dialogueClicks = 2;
    private int _dialogueClickCount = 0;

    public Canvas currentCanvas;

    public enum State
    {
        None = -1,
        Logging = 0,
        Start = 1,
        AR = 2,
        BearDialogue = 3,
        Riddles = 4,
        Map = 5,
        KidDialogue = 6,
        End = 7
    }

    public State CurrentState
    {
        get { return currentState; }
        set
        {
            prevState = currentState;
            currentState = value;

            switch (currentState)
            {
                case State.Start:
                    _dialogueClicks = 4;
                    break;
                case State.BearDialogue:
                    _dialogueClicks = 5;
                    break;
                case State.KidDialogue:
                    _dialogueClicks = 7;
                    break;
            }
        }
    }

    void OnLevelWasLoaded()
    {
        _dialogueClickCount = 0;

        if (GameObject.Find("Canvas") != null)
            currentCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
        
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDialogueButtonClick()
    {
        if (_dialogueClickCount < _dialogueClicks)
        {
            _dialogueClickCount++;
            OnDialogueEvent(_dialogueClickCount);
        }
        else
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        print("Load Next Scene");
    }
}
