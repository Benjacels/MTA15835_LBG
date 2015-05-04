using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlienManager : MonoBehaviour {
    /*
    private int idleHash = Animator.StringToHash("Idle");
    private int thinkingHash = Animator.StringToHash("Thinking");
    private int sleepingHash = Animator.StringToHash("Sleeping");
    private int greatingHash = Animator.StringToHash("Greating");
    private int instructingHash = Animator.StringToHash("Instructing");
    */
    
    public string[] bodyAnimations;
    public string[] mouthAnimations;
    public Sprite[] speechSprites;

    public Sprite[] tapSprites;

    public Sprite foundSpot;
    public Sprite tutorialWrong;
    public Sprite tutorialCorrect;

    public Sprite friends;
    public Sprite fuel;
    public Sprite checkPlaceCorrect;
    public Sprite checkPlaceWrong;

    private Image _speechSprite;
    private Image _answerImage;
    private UnityEngine.UI.Text _answerText;
    private Animator _bodyAnimator;
    private Animator _mouthAnimator;

    private UnityEngine.UI.Text _riddleText;
    private Button _nextControl;
    private Button _nextRiddle;

    private Image _riddleBackground;

    private string prevBodyState;
    private string prevMouthState;

    private int _nextInfo = 0;
    private bool _hasAnsweredTut = false;
    private bool _correctAnswerTut = false;
    private bool _showPlaceText = false;
    private bool _showPlacePic = false;

    void OnEnable()
    {
        if (MainManager.instance != null)
            MainManager.instance.OnDialogueEvent += OnDialogClick;

        if (RiddleManager.instance != null)
            RiddleManager.instance.OnTutorialEvent += OnTutorialClick;
    }

    void OnDisable()
    {
        if (MainManager.instance != null)
            MainManager.instance.OnDialogueEvent -= OnDialogClick;

        if (RiddleManager.instance != null)
            RiddleManager.instance.OnTutorialEvent -= OnTutorialClick;
    }

    void Awake()
    {
        if (GameObject.Find("AnswerPic") != null)
            _answerImage = GameObject.Find("AnswerPic").GetComponent<Image>();
        if (GameObject.Find("AnswerText") != null)
            _answerText = GameObject.Find("AnswerText").GetComponent<UnityEngine.UI.Text>();
    }
    
    // Use this for initialization
	void Start ()
	{
	    if (MainManager.instance.currentCanvas.transform.FindChild("SpeechBubble").GetComponent<Image>() != null)
            _speechSprite = MainManager.instance.currentCanvas.transform.FindChild("SpeechBubble").GetComponent<Image>();
	    else
	        print("YOU NEED TO ADD A SPEECHBUBBLE");

        _bodyAnimator = GameObject.FindGameObjectWithTag("MonsterContainer").GetComponent<Animator>();
        _mouthAnimator = GameObject.FindGameObjectWithTag("Mouth").GetComponent<Animator>();

	    prevBodyState = "Idle";
	    prevMouthState = "Idle";

	    if (MainManager.instance.CurrentState == MainManager.State.Riddles)
	    {
            _speechSprite.sprite = tapSprites[0];

            _nextRiddle = MainManager.instance.currentCanvas.transform.FindChild("NextRiddle").GetComponent<Button>();
            _riddleText = MainManager.instance.currentCanvas.transform.FindChild("RiddleText").GetComponent<UnityEngine.UI.Text>();
            _nextControl = MainManager.instance.currentCanvas.transform.FindChild("NextControl").GetComponent<Button>();
            _riddleBackground = MainManager.instance.currentCanvas.transform.FindChild("RiddleBackground").GetComponent<Image>();
	    }
	        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDialogClick(int clickCount)
    {
        //TODO: Fade this
        if (_speechSprite.color.a < 255)
            _speechSprite.color = new Color(_speechSprite.color.r,_speechSprite.color.g,_speechSprite.color.b, 255);

        _speechSprite.sprite = speechSprites[clickCount];

        _bodyAnimator.SetBool(Animator.StringToHash(prevBodyState), false);
        _bodyAnimator.SetBool(Animator.StringToHash(bodyAnimations[clickCount]), true);
        prevBodyState = bodyAnimations[clickCount];

        _mouthAnimator.SetBool(Animator.StringToHash(prevMouthState), false);
        _mouthAnimator.SetBool(Animator.StringToHash(mouthAnimations[clickCount]), true);
        prevMouthState = mouthAnimations[clickCount];
    }

    public void OnTap()
    {
        switch (MainManager.instance.CurrentState)
        {
            case MainManager.State.Riddles:
                if (RiddleManager.instance.tutorialMode)
                {
                    if (_nextInfo < tapSprites.Length - 1)
                    {
                        _nextInfo++;
                        _speechSprite.sprite = tapSprites[_nextInfo];

                        if (_nextInfo == 1)
                        {
                            _riddleText.active = true;
                            _riddleBackground.active = true;
                        }
                        else if (_nextInfo == tapSprites.Length - 1)
                            _nextControl.active = true;
                    }
                    else if (_hasAnsweredTut)
                    {
                        if (_correctAnswerTut)
                        {
                            if (MainManager.instance.choices.Last() == MainManager.Choices.Fuel && !_showPlaceText)
                            {
                                _speechSprite.sprite = fuel;
                                _showPlaceText = true;
                                break;
                            }
                            if (MainManager.instance.choices.Last() == MainManager.Choices.Friends && !_showPlaceText)
                            {
                                _speechSprite.sprite = friends;
                                _showPlaceText = true;
                                break;
                            }
                            if (_showPlaceText && !_showPlacePic)
                            {
                                _speechSprite.sprite = checkPlaceCorrect;
                                _showPlacePic = true;
                                break;
                            }
                        }
                        else if (!_showPlacePic)
                        {
                            _speechSprite.sprite = checkPlaceWrong;
                            _showPlacePic = true;
                            break;
                        }
                        if (_showPlacePic)
                        {
                            _speechSprite.active = false;
                            _answerImage.active = true;
                            _answerText.active = true;
                            _nextRiddle.active = true;
                            RiddleManager.instance.tutorialMode = false;
                        }
                    }
                }
                break;
        }
    }

    void OnTutorialClick(string buttonClicked)
    {
        switch (buttonClicked)
        {
            case "Riddle":
                _speechSprite.sprite = foundSpot;
                break;
            case "True":
                _speechSprite.sprite = tutorialCorrect;
                _hasAnsweredTut = true;
                _correctAnswerTut = true;
                break;
            case "False":
                _speechSprite.sprite = tutorialWrong;
                _hasAnsweredTut = true;
                _correctAnswerTut = false;
                break;
        }
    }
}


