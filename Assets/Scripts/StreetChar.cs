using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StreetChar : MonoBehaviour {

    public Sprite[] initialSpeech;

    public Sprite questionSpeechBubble;

    public Sprite fuelSpeechBubble;
    public Sprite friendsSpeechBubble;

    private Image _speechSprite;

    private string _prevAnimState;

    private float timeBetweenInitAnim = 5;

    private IEnumerator coroutine;

    void OnEnable()
    {
        if (MainManager.instance != null)
        {
            MainManager.instance.OnChoiceEvent += OnChoiceClick;
            MainManager.instance.OnAskEvent += OnAsk;
        }
    }

    void OnDisable()
    {
        if (MainManager.instance != null)
        {
            MainManager.instance.OnChoiceEvent -= OnChoiceClick;
            MainManager.instance.OnAskEvent -= OnAsk;
        }
    }

	// Use this for initialization
	void Start () 
    {
        if (GameObject.Find("StreetCharSpeechBubble").GetComponent<Image>() != null)
            _speechSprite = GameObject.Find("StreetCharSpeechBubble").GetComponent<Image>();
        else
            print("YOU NEED TO ADD A SPEECHBUBBLE");

        coroutine = LoopSprites();
	    StartCoroutine(coroutine);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //TODO: Add animations

    void OnAsk()
    {
        StopCoroutine(coroutine);
        _speechSprite.sprite = questionSpeechBubble;
    }

    void OnChoiceClick(MainManager.Choices choice)
    {
        if (choice == MainManager.Choices.Fuel)
            _speechSprite.sprite = fuelSpeechBubble;
        else if (choice == MainManager.Choices.Friends)
            _speechSprite.sprite = friendsSpeechBubble;
    }

    IEnumerator LoopSprites()
    {
        int spriteLoopCount = 0;

        while (spriteLoopCount < initialSpeech.Length)
        {
            _speechSprite.sprite = initialSpeech[spriteLoopCount];
            spriteLoopCount++;
            yield return new WaitForSeconds(timeBetweenInitAnim);
        }
        StartCoroutine(LoopSprites());
    }
}
