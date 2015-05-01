using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Xml;
using UnityEngine.UI;
using Text = UnityEngine.UI.Text;

public class RiddleManager : MonoBehaviour {

    private TextAsset textAsset;
    private Canvas _canvas;
    private List<Button> _answers = new List<Button>();
    private XmlDocument _xmlDoc;

    private UnityEngine.UI.Text _riddleText;
    private UnityEngine.UI.Text _controlText;

    private Button _nextRiddle;
    private Button _nextControl;

    private Image _answerImage;

    private Sprite[] _answerPics = new Sprite[40];

    private int _riddleCounter = 0;

    List<string> _currentOptions = new List<string>();

    // Use this for initialization
    void Start()
    {
        _xmlDoc = loadLocalXml("RiddleRoute1");
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        _riddleText = _canvas.transform.FindChild("RiddleText").GetComponent<UnityEngine.UI.Text>();
        _controlText = _canvas.transform.FindChild("ControlText").GetComponent<UnityEngine.UI.Text>();

        _nextRiddle = _canvas.transform.FindChild("NextRiddle").GetComponent<Button>();
        _nextControl = _canvas.transform.FindChild("NextControl").GetComponent<Button>();

        _riddleText.text = _xmlDoc.GetElementsByTagName("riddle").Item(_riddleCounter).ChildNodes[0].InnerXml;

        _answerImage = _canvas.transform.FindChild("AnswerPic").GetComponent<Image>();

        foreach (Transform tran in _canvas.transform)
        if (tran.CompareTag("Button_Answer"))
        {
           _answers.Add(tran.GetComponent<Button>());
           tran.GetComponent<Button>().active = false;
        }
        _nextRiddle.active = false;
        _controlText.active = false;
        _nextControl.active = true;
        _answerImage.active = false;
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

        foreach (Button but in _answers)
            but.active = true;

        var riddle = _xmlDoc.GetElementsByTagName("riddle").Item(_riddleCounter);
        
        _controlText.text = riddle.ChildNodes[1].InnerXml;

        
        for (int i = 0; i < 3; i++)
        {
            _currentOptions.Add(riddle.ChildNodes[2].ChildNodes[i].Attributes[0].Value);
            _answers[i].transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = riddle.ChildNodes[2].ChildNodes[i].InnerXml;
        }
    }
    public void Answer(int answer)
    {
        _controlText.active = false;

        foreach (Button but in _answers)
            but.active = false;

        _nextRiddle.active = true;

        for (int i = 0; i < 3; i++)
            if(_currentOptions[i] == "true" && i == answer)
                print("YES!");

        _answerImage.active = true;

        //TODO: Use a higher riddleCounter for riddles on route 2
        _answerImage.sprite = Resources.Load<Sprite>("AnswerPics/"+(_riddleCounter+1).ToString());
    }

    public void NextRiddle()
    {
        _riddleCounter++;
        _currentOptions.RemoveRange(0, 3);

        _nextRiddle.active = false;
        _nextControl.active = true;
        _riddleText.active = true;

        _riddleText.text = _xmlDoc.GetElementsByTagName("riddle").Item(_riddleCounter).ChildNodes[0].InnerXml;

        _answerImage.active = false;
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
}