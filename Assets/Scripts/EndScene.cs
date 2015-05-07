using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    public GameObject fuelNext;
    public GameObject friendNext;
    public Image speechBubble;

    public Sprite[] fuelSpeech;
    public Sprite[] friendSpeech;

    private int SpeechBubbleCounter;

    void Awake()
    {
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
    }

    public void OnFriendButton()
    {
        SpeechBubbleCounter++;
        if (SpeechBubbleCounter < friendSpeech.Length)
            speechBubble.sprite = friendSpeech[SpeechBubbleCounter];
    }
}
