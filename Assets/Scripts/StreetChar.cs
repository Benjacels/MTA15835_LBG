using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StreetChar : MonoBehaviour {

    public string initialAnim;
    public Sprite[] initialSpeech;

    public string questionAnim;
    public Sprite questionSpeechBubble;

    public string choiceAnim;
    public Sprite fuelSpeechBubble;
    public Sprite friendsSpeechBubble;

    private Image _speechSprite;
    private Animator _animator;

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
        GetComponent<Animator>().StartPlayback();
        
        if (MainManager.instance.currentCanvas.transform.FindChild("StreetCharSpeechBubble").GetComponent<Image>() != null)
            _speechSprite = MainManager.instance.currentCanvas.transform.FindChild("StreetCharSpeechBubble").GetComponent<Image>();
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
