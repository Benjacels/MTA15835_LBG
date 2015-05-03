using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnDialogueButtonClick()
    {
        MainManager.instance.OnDialogueButtonClick();
    }

    public void OnChoiceButtonClick(string choice)
    {
       MainManager.instance.OnChoiceButtonClick(choice);
    }

    public void OnAskCharacter(Button buttonClicked)
    {
        MainManager.instance.OnAskCharacter(buttonClicked);
    }

    public void LoadNextScene()
    {
        MainManager.instance.LoadNextScene();
    }
}
