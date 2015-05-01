using UnityEngine;
using System.Collections;

public class MonsterBehaviour : MonoBehaviour {
	
	private Animator mouthAnim;
	private Animator monsterAnim;

	private int tapCount;
	private float startTapTime;
	private float endTapTime;

	private bool groundHit;

	public bool gravity;
	[Range (0, 1000)]
	public float forceScale = 450f; //should be in monster behavior script and get state from monster manager


	public bool Relational;


	void Awake() 
	{
		
		// Find objects with tag
		mouthAnim = GameObject.FindGameObjectWithTag ("Mouth").GetComponent<Animator>();
		monsterAnim = GameObject.FindGameObjectWithTag("MonsterContainer").GetComponent<Animator>();

		GameManager.Instance.Relational = Relational;
	}

	IEnumerator Start () {
		
//		speechBubble.guiTexture.enabled = false;
		
		while(true)
		{
			switch(GameManager.Instance.CurrentState)
			{
			case GameManager.State.GameStart:
				//if first time start linear dialog
				yield return new WaitForSeconds(2f);
//				yield return StartCoroutine(LinearDialog(linearDialogGameStart));
				//else start random dialog
				break;
			case GameManager.State.Monster:
				break;
			case GameManager.State.AR:

				break;
			case GameManager.State.GameOver:
				break;

			}
			yield return null;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.touchCount > 0){
			
			// One finger tap
			Touch touch = Input.GetTouch(0);
			
			Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
//			Vector2 touchDeltaPosition = Camera.main.ScreenToWorldPoint(touch.deltaPosition);


			if(touch.phase == TouchPhase.Began && collider2D.OverlapPoint(touchPosition))
			{
				// One finger single tap
				if(touch.tapCount == 2)
				{
					Debug.Log("Tap twice");
				}
				
				// One finger double tap
				else if(touch.tapCount == 1)
				{
					Debug.Log("Tap once");
					StartCoroutine(TapBehavior(2f));
				}
			}

//			if(touch.phase == TouchPhase.Began && collider2D.OverlapPoint(touchPosition))
//			{
//				//A touch began with touch position overlapping collider
//				tapCount += 1;
//				startTapTime = Time.time;
//
//			} 
//			if(touch.phase == TouchPhase.Ended && collider2D.OverlapPoint(touchPosition))
//			{
//				//A touch ended with touch position overlapping collider
//				endTapTime = Time.time;
//
//				if(tapCount == 1)
//					Debug.Log("Single Tap");
//				if(tapCount == 2)
//					Debug.Log("Double Tap");
//
//				tapCount = 0;
//
//			}
		}

		if(gravity && groundHit)
		{
//			monsterAnimator.SetTrigger("GroundTrigger");
//			Debug.Log("Ground Hit");
		}
			
	}

	private void FixedUpdate() //fixed update is used because of the use of physics
	{
		//Character State Introduction
//		if(GameManager.Instance.CurrentState == GameManager.State.GameStart && MonsterManager.Instance.CurrentObjectState == MonsterManager.ObjectState.Active && groundHit == false) 
		if(gravity && groundHit == false)
		{
			//if collision with ground detected stop updating
			//			Debug.Log("<color=red>AddFORCE</color>");
			gameObject.rigidbody2D.AddForce(-Vector2.up * forceScale);
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Ground") {
			Debug.Log("Collided with Ground");
			groundHit = true;
		}
		
	}

	IEnumerator TapBehavior(float waitTime)
	{
		if(Relational)
		{
			monsterAnim.SetBool("Tap", true);
			yield return new WaitForSeconds(0.05f);
			mouthAnim.SetBool("Tap", true);
			yield return new WaitForSeconds(waitTime);
			monsterAnim.SetBool("Tap", false);
			mouthAnim.SetBool("Tap", false);
			//some dialog
		}
		else
		{
			//some dialog
		}
		yield return null;
	}

	IEnumerator StrokeBehavior()
	{
		if(Relational)
		{
			
		}
		else
		{
			
		}
		yield return null;
	}

}
