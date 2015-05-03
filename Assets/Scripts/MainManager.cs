﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    private State currentChoice = State.None;
    private State prevState;
    
    public delegate void ChoiceEvent(Choices choice);
    public event ChoiceEvent OnChoiceEvent;

    public delegate void Ask();
    public event Ask OnAskEvent;

    public float Points;

    public bool riddlesFirst;

    [HideInInspector]
    public bool hasSeenBear = false;

    [HideInInspector]
    public bool _fromBeginning;

    private List<Choices> _choices = new List<Choices>();

    private int _dialogueClicks = 4;
    private int _dialogueClickCount = 0;
    private int _nextScene = 0;

    public Canvas currentCanvas;

    private bool _finalScene;

    private Button _choiceFriends;
    private Button _choiceFuel;
    private Button _wayFinder;
    
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

    public enum Choices
    {
        Fuel,
        Friends
    }

    public State CurrentChoice
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
            _choices.Add(Choices.Fuel);
        }
            
        else if (choice == "Friends")
        {
            OnChoiceEvent(Choices.Friends);
            _choices.Add(Choices.Friends);
        }
            

        _choiceFuel.active = false;
        _choiceFriends.active = false;

        _wayFinder.active = true;
    }

    public void OnAskCharacter(Button buttonClicked)
    {
        OnAskEvent();
        
        buttonClicked.active = false;

        _choiceFuel.active = true;
        _choiceFriends.active = true;
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

                hasSeenBear = true;

                break;
            case 4 :
                currentState = State.Riddles;

                if (riddlesFirst)
                    _nextScene = 6;
                else
                    _nextScene = 7;

                break;
            case 5 :
                currentState = State.Map;

                if (riddlesFirst)
                    _nextScene = 7;
                else
                    _nextScene = 6;

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
                _finalScene = true;

                break;
        }
    }

    public void LoadNextScene()
    {
        if (_finalScene)
            Application.Quit();
        else
        {
            Application.LoadLevel(_nextScene);
        }
            
    }
}
