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

    private int _riddleCounter = 0;

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

        foreach (Transform tran in _canvas.transform)
        if (tran.CompareTag("Button_Answer"))
        {
           _answers.Add(tran.GetComponent<Button>());
           tran.GetComponent<Button>().active = false;
        }

        _nextRiddle.active = false;
        _controlText.active = false;
        _nextControl.active = true;
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

        _controlText.text = _xmlDoc.GetElementsByTagName("riddle").Item(_riddleCounter).ChildNodes[1].InnerXml;
    }
    public void Answer(int answer)
    {
        _controlText.active = false;

        foreach (Button but in _answers)
            but.active = false;

        _nextRiddle.active = true;

        List<string> options = new List<string>();

        for (int i = 0; i < 3; i++)
        {
            options.Add(_xmlDoc.GetElementsByTagName("riddle").Item(_riddleCounter).ChildNodes[2].ChildNodes[i].Attributes[0].Value);
            if(options[i] == "true" && i == answer)
                print("YES!");
        }
    }

    public void NextRiddle()
    {
        _riddleCounter++;

        _nextRiddle.active = false;
        _nextControl.active = true;
        _riddleText.active = true;

        _riddleText.text = _xmlDoc.GetElementsByTagName("riddle").Item(_riddleCounter).ChildNodes[0].InnerXml;
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