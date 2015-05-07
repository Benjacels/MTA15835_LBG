using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    public GameObject fuelNext;
    public GameObject friendNext;
    public GameObject monsterRocketObj;

    public Image speechBubble;
    public Sprite monsterRocket;

    public Sprite[] fuelSpeech;
    public Sprite[] friendSpeech;

    private int SpeechBubbleCounter;

    void Awake()
    {
        MainManager.instance.FuelPoints = 3;
        if (MainManager.instance.FuelPoints > 0)
        {
            fuelNext.active = true;
            friendNext.active = false;
        }
        else
        {
            fuelNext.active = false;
            friendNext.active = true;
        }
    }

	// Use this for initialization
	void Start ()
	{
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnFuelButton()
    {
        SpeechBubbleCounter++;
        if (SpeechBubbleCounter < fuelSpeech.Length)
            speechBubble.sprite = fuelSpeech[SpeechBubbleCounter];
        else
        {
            GameObject.Find("Alien").active = false;
            speechBubble.active = false;
            GameObject.Find("Friends").active = false;

            monsterRocketObj.GetComponent<SpriteRenderer>().sprite = monsterRocket;
            monsterRocketObj.GetComponent<Animator>().enabled = true;
            LeanTween.scale(GameObject.Find("shadow"), Vector3.zero, 2).setEase(LeanTweenType.easeInCubic).setOnComplete(EndGame);
        }
    }

    public void OnFriendButton()
    {
        RenderSettings.ambientLight = new Color(0, 0, 0, 0);
        SpeechBubbleCounter++;
        if (SpeechBubbleCounter < friendSpeech.Length)
            speechBubble.sprite = friendSpeech[SpeechBubbleCounter];
        else
            EndGame();
    }

    void EndGame()
    {
        MainManager.instance.LoadNextScene();
    }
}
