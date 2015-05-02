using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlienManager : MonoBehaviour {
    /*
    private int idleHash = Animator.StringToHash("Idle");
    private int thinkingHash = Animator.StringToHash("Thinking");
    private int sleepingHash = Animator.StringToHash("Sleeping");
    private int greatingHash = Animator.StringToHash("Greating");
    private int instructingHash = Animator.StringToHash("Instructing");
    */
    
    public string[] bodyAnimations;
    public string[] mouthAnimations;
    public Sprite[] speechSprites;

    private Image _speechSprite;
    private Animator _bodyAnimator;
    private Animator _mouthAnimator;

    private string prevBodyState;
    private string prevMouthState;

    void OnEnable()
    {
        if (MainManager.instance != null)
            MainManager.instance.OnDialogueEvent += OnDialogClick;
    }

    void OnDisable()
    {
        if (MainManager.instance != null)
            MainManager.instance.OnDialogueEvent -= OnDialogClick;
    }
    
    // Use this for initialization
	void Start ()
	{
	    if (MainManager.instance.currentCanvas.transform.FindChild("SpeechBubble").GetComponent<Image>() != null)
            _speechSprite = MainManager.instance.currentCanvas.transform.FindChild("SpeechBubble").GetComponent<Image>();
	    else
	        print("YOU NEED TO ADD A SPEECHBUBBLE");

        _bodyAnimator = GameObject.FindGameObjectWithTag("MonsterContainer").GetComponent<Animator>();
        _mouthAnimator = GameObject.FindGameObjectWithTag("Mouth").GetComponent<Animator>();

	    prevBodyState = "Idle";
	    prevMouthState = "Idle";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDialogClick(int clickCount)
    {
        //TODO: Fade this
        if (_speechSprite.color.a < 255)
            _speechSprite.color = new Color(_speechSprite.color.r,_speechSprite.color.g,_speechSprite.color.b, 255);

        _speechSprite.sprite = speechSprites[clickCount];

        _bodyAnimator.SetBool(Animator.StringToHash(prevBodyState), false);
        _bodyAnimator.SetBool(Animator.StringToHash(bodyAnimations[clickCount]), true);
        prevBodyState = bodyAnimations[clickCount];

        _mouthAnimator.SetBool(Animator.StringToHash(prevMouthState), false);
        _mouthAnimator.SetBool(Animator.StringToHash(mouthAnimations[clickCount]), true);
        prevMouthState = mouthAnimations[clickCount];
    }

    public void OnTap()
    {
        //Add speech bubbles and animations
    }
}


