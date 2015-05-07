using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StreetChar : MonoBehaviour {

    public Sprite[] initialSpeech;

    public Sprite questionSpeechBubble;

    public Sprite helpSpeechBubble;
    public Sprite wayFinderSpeechBubble;

    private Image _speechSprite;

    private string _prevAnimState;

    private float timeBetweenInitAnim = 5;
    private float timeForLastSpeech = 2;

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
        _speechSprite.sprite = helpSpeechBubble;
        StartCoroutine(WaitAndChangeSpeech());
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
    IEnumerator WaitAndChangeSpeech()
    {
        yield return new WaitForSeconds(timeForLastSpeech);
        _speechSprite.sprite = wayFinderSpeechBubble;
        var wayFinder = MainManager.instance.currentCanvas.transform.FindChild("Wayfinder").GetComponent<Button>();
        wayFinder.enabled = true;
        wayFinder.image.enabled = true;
        yield return null;
    }
}
