using UnityEngine;
using System.Collections;

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

    private SpriteRenderer _speechSprite;
    private Animator _bodyAnimator;
    private Animator _mouthAnimator;

    private string prevState;

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
	    if (transform.FindChild("SpeechBubble").GetComponent<SpriteRenderer>() != null)
            _speechSprite = transform.FindChild("SpeechBubble").GetComponent<SpriteRenderer>();
	    else
	        print("YOU NEED TO ADD A SPEECHBUBBLE");

        _bodyAnimator = GameObject.FindGameObjectWithTag("MonsterContainer").GetComponent<Animator>();
        _mouthAnimator = GameObject.FindGameObjectWithTag("Mouth").GetComponent<Animator>();

        _bodyAnimator.SetBool(Animator.StringToHash(bodyAnimations[0]), true);
	    prevState = bodyAnimations[0];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDialogClick(int clickCount)
    {
        _speechSprite.sprite = speechSprites[clickCount];

        _bodyAnimator.SetBool(Animator.StringToHash(prevState), false);
        _bodyAnimator.SetBool(Animator.StringToHash(bodyAnimations[clickCount]), true);
        prevState = bodyAnimations[clickCount];
        //_mouthAnimator.SetBool(Animator.StringToHash(mouthAnimations[clickCount]), true);
    }
}


