﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    public GameObject fuelNext;
    public GameObject friendNext;
    public GameObject monsterRocketObj;

    public Image fadeScreen;

    public Image speechBubble;
    public Sprite monsterRocket;

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

        fadeScreen.active = false;
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
            LeanTween.scale(GameObject.Find("shadow"), Vector3.zero, 2).setEase(LeanTweenType.easeInCubic).setOnComplete(FadeScreen);
        }
    }

    public void OnFriendButton()
    {
        RenderSettings.ambientLight = new Color(0, 0, 0, 0);
        SpeechBubbleCounter++;
        if (SpeechBubbleCounter < friendSpeech.Length)
            speechBubble.sprite = friendSpeech[SpeechBubbleCounter];
        else
            FadeScreen();
    }

    void FadeScreen()
    {
        fadeScreen.active = true;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float alpha = 0;
        while (fadeScreen.color.a < 1)
        {
            alpha += Time.deltaTime/2;
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);

            yield return null;
        }
        EndGame();
    }

    void EndGame()
    {
        MainManager.instance.LoadNextScene();
    }
}
