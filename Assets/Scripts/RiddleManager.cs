using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

public class RiddleManager : MonoBehaviour {

    public static RiddleManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<RiddleManager>();
            }

            return _instance;
        }
    }

    private static RiddleManager _instance;

    public delegate void TutorialClick(string buttonClicked);
    public event TutorialClick OnTutorialEvent;

    private TextAsset textAsset;
    private Canvas _canvas;
    private List<Button> _answers = new List<Button>();
    private XmlDocument _xmlDoc;

    private UnityEngine.UI.Text _riddleText;
    private UnityEngine.UI.Text _controlText;

    public string winText;

    private Button _nextRiddle;
    private Button _nextControl;

    private Image _answerImage;
    private UnityEngine.UI.Text _answerText;

    private GameObject _goalScreen;
    private Image _riddleBackground;

    private Sprite[] _answerPics = new Sprite[40];

    private int _riddleCounter = 0;

    private TxtLogger _txtLogger;

    private float _timeRiddleStarted = 0;

    public bool tutorialMode = false;

    List<string> _currentOptions = new List<string>();

    public Sprite correctAnswerSprite;
    public Sprite wrongAnswerSprite;
    public Sprite neutralSprite;

    private UnityEngine.UI.Text _fuelPointsTxt;
    private UnityEngine.UI.Text _friendPointsTxt;

    // Use this for initialization
    void Start()
    {
        if(MainManager.instance.riddlesFirst)
            _xmlDoc = loadLocalXml("RiddleRoute1");
        else
            _xmlDoc = loadLocalXml("RiddleRoute2");

        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        _riddleText = _canvas.transform.FindChild("RiddleText").GetComponent<UnityEngine.UI.Text>();
        _controlText = _canvas.transform.FindChild("ControlText").GetComponent<UnityEngine.UI.Text>();

        _nextRiddle = _canvas.transform.FindChild("NextRiddle").GetComponent<Button>();
        _nextControl = _canvas.transform.FindChild("NextControl").GetComponent<Button>();

        _riddleBackground = _canvas.transform.FindChild("RiddleBackground").GetComponent<Image>();

        _txtLogger = GameObject.FindObjectOfType<TxtLogger>();

        _answerImage = GameObject.Find("AnswerPic").GetComponent<Image>();
        _answerText = GameObject.Find("AnswerText").GetComponent<UnityEngine.UI.Text>();
        _goalScreen = _canvas.transform.FindChild("GoalScreen").gameObject;

        _fuelPointsTxt = _canvas.transform.FindChild("PointsLeft").transform.Find("PointsNum").GetComponent<UnityEngine.UI.Text>();
        _friendPointsTxt = _canvas.transform.FindChild("PointsRight").transform.Find("PointsNum").GetComponent<UnityEngine.UI.Text>();

        foreach (Transform tran in _canvas.transform)
        if (tran.CompareTag("Button_Answer"))
        {
           _answers.Add(tran.GetComponent<Button>());
           tran.GetComponent<Button>().active = false;
        }

        _nextRiddle.active = false;
        _controlText.active = false;
        _nextControl.active = false;
        _answerImage.active = false;
        _answerText.active = false;
        _riddleText.active = false;
        _riddleBackground.active = false;
        _goalScreen.active = false;

        if (MainManager.instance.InDebug)
            _riddleCounter = MainManager.instance.RiddleDebugIndex;

        if (MainManager.instance.InDebug && _riddleCounter != 0)
        {
            _riddleText.active = true;
            _riddleBackground.active = true;
            _nextControl.active = true;
        }
        else if (_riddleCounter == 0)
        {
            tutorialMode = true;
        }

        _riddleText.text = _xmlDoc.GetElementsByTagName("riddle").Item(_riddleCounter).ChildNodes[0].InnerXml;
        _txtLogger.log("Riddle started: ID - " + _riddleCounter);
        _txtLogger.log("Riddle text: " + _riddleText.text);
        _timeRiddleStarted = Time.time;

        //DEBUG ONLY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!:
        //MainManager.instance.choices.Add(MainManager.Choices.Friends);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void NextControl()
    {
        _riddleText.active = false;
        _nextControl.active = false;
        _controlText.active = true;

        _riddleBackground.active = false;

        foreach (Button but in _answers)
        {
            but.enabled = true;
            but.active = true;
        }

        var riddle = _xmlDoc.GetElementsByTagName("riddle").Item(_riddleCounter);
        
        _controlText.text = riddle.ChildNodes[1].InnerXml;
        
        for (int i = 0; i < 3; i++)
        {
            _currentOptions.Add(riddle.ChildNodes[2].ChildNodes[i].Attributes[0].Value);
            _answers[i].transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = riddle.ChildNodes[2].ChildNodes[i].InnerXml;
        }

        if (tutorialMode)
            OnTutorialEvent("Riddle");
    }
    public void Answer(int answer)
    {
        _controlText.active = false;

        foreach (Button but in _answers)
            but.enabled = false;

        if (_xmlDoc.GetElementsByTagName("riddle").Item(_riddleCounter + 1) != null)
        {
            if (!tutorialMode)
                _nextRiddle.active = true;
        }
        else
        {
            _answerText.text = winText;
            _goalScreen.active = true;
            if (!MainManager.instance.riddlesFirst)
                MainManager.instance.artsSeen.Add("space");
                
        }

        var userCorrect = false;

        for (int i = 0; i < 3; i++)
        {
            if (_currentOptions[i] == "true" && i == answer)
            {
                userCorrect = true;
                GivePoints();
                //TODO: Fade-in
                _answers[answer].image.sprite = correctAnswerSprite;
            }
            else //TODO: Fade-in
            {
                _answers[answer].image.sprite = wrongAnswerSprite;
                _answers[_currentOptions.IndexOf("true")].image.sprite = correctAnswerSprite;
            }
        }
        if (!tutorialMode)
        {
            _answerImage.active = true;
            _answerText.active = true;
        }

        if (MainManager.instance.riddlesFirst)
            _answerImage.sprite = Resources.Load<Sprite>("AnswerPics/"+(_riddleCounter).ToString());
        else
            _answerImage.sprite = Resources.Load<Sprite>("AnswerPics/"+(_riddleCounter+21).ToString());

        if (tutorialMode)
            OnTutorialEvent(userCorrect.ToString());

        _txtLogger.log("Riddle ended: ID - " + _riddleCounter);
        var riddleTime = Time.time - _timeRiddleStarted;
        _txtLogger.log("Riddle time: " + riddleTime);
        _txtLogger.log("Answer correct: " + userCorrect);
        var answerToLog = _xmlDoc.GetElementsByTagName("riddle").Item(_riddleCounter).ChildNodes[2].ChildNodes[answer].InnerXml;
        _txtLogger.log("User answered: " + answerToLog);
        
    }

    public void NextRiddle()
    {
        _riddleCounter++;
        _currentOptions.RemoveRange(0, 3);

        foreach (Button but in _answers)
        {
            but.active = false;
            but.image.sprite = neutralSprite;
        }

        _nextRiddle.active = false;
        _nextControl.active = true;
        _riddleText.active = true;

        _riddleText.text = _xmlDoc.GetElementsByTagName("riddle").Item(_riddleCounter).ChildNodes[0].InnerXml;
        _txtLogger.log("Riddle started: ID - " + _riddleCounter);
        _txtLogger.log("Riddle text: " + _riddleText.text);
        _timeRiddleStarted = Time.time;

        _answerImage.active = false;
        _answerText.active = false;

        _riddleBackground.active = true;
    }
    XmlDocument loadLocalXml(string name)
    {
        textAsset = Resources.Load(name) as TextAsset;

        if (textAsset != null)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(textAsset.text);
            return xmldoc;
        }
        else
        {
            print("Error: are you sure that the specified xml file exists?");
            return null;
        }
    }

    public void GivePoints()
    {
        MainManager mm = MainManager.instance;

		switch (mm.choices[mm.choices.Count-1])
        {
            case MainManager.Choices.Friends:
                mm.FriendPoints++;
                _friendPointsTxt.text = mm.FriendPoints.ToString();
                _txtLogger.log("Friends points: " + mm.FriendPoints);
                break;

            case MainManager.Choices.Fuel:
                mm.FuelPoints++;
                _fuelPointsTxt.text = mm.FuelPoints.ToString();
                _txtLogger.log("Fuel points: " + mm.FuelPoints);
                break;
        }
    }
}