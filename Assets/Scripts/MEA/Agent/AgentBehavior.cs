using UnityEngine;
using System.Collections;

public class AgentBehavior : MonoBehaviour {

	private Tween2D tween;
	private SwapSprites sprite;

	private GameObject speechOutputObj;
	private SpeechRenderer speechRenderer;
	private Shadow shadow;

	private Animator mouthAnim;
	private Animator agentAnim;
	private GameObject agentBody;
	private Transform agentTransform;
	private Vector3 agentScale;
//	private Rigidbody2D agentRigidBody;
	
	private int tapCount;
//	private bool singleTap;
//	private bool doubleTap;

	private bool groundHit;
	private Collider2D agentCollider;

	private int idleHash = Animator.StringToHash("Idle");
	private int thinkingHash = Animator.StringToHash("Thinking");
	private int sleepingHash = Animator.StringToHash("Sleeping");
	private int greatingHash = Animator.StringToHash("Greating");
	private int instructingHash = Animator.StringToHash("Instructing");

//	[Range (0, 1000)]
//	public float ForceScale = 450f; //should be in monster behavior script and get state from monster manager
//
//	public bool Relational;

	//	public AnimationClip[] SpriteAnimations = new AnimationClip[3];

	private bool fireOnce;
	private float agentRotation = 338.0f;

	private int countIdle;
	private int countInstructing;
	private int countGreating;
	private int countFarewell;
	private int countThinking;
	private int countSleeping;
	private int countWaiting;
	private int countSelfDisclosing;
	private int countTeaching;
	private int countCommenting;

	void Awake() 
	{
		tween = GetComponent<Tween2D>();
		sprite = GetComponent<SwapSprites>();

		agentBody = GameObject.Find ("Body");
		agentTransform = this.transform;
		//		agentRigidBody = GetComponent<Rigidbody2D>();
		agentCollider = agentBody.GetComponent<Collider2D>();

		speechOutputObj = GameObject.FindGameObjectWithTag("SpeechBubble");
		speechRenderer = speechOutputObj.GetComponent<SpeechRenderer>();
		speechRenderer.AgentTransform = agentBody.transform;

		shadow = GameObject.FindGameObjectWithTag("Shadow").GetComponent<Shadow>();
		shadow.AgentTransform = agentBody.transform;

		// Find objects with tag
		mouthAnim = GameObject.FindGameObjectWithTag ("Mouth").GetComponent<Animator>();
		agentAnim = GameObject.FindGameObjectWithTag("MonsterContainer").GetComponent<Animator>();

	}

//	private void FixedUpdate() //fixed update is used because of the use of physics
//	{
//		switch(GameManager.Instance.CurrentState)
//		{
//		case GameManager.State.SplashScreen:
//			break;
//		case GameManager.State.GameStart:
//			if(groundHit == false)
//			{
//				//if collision with ground detected stop updating
//				//			Debug.Log("<color=red>AddFORCE</color>");
//				// force = mass x accelelation
//				// the larger an object's mass, the more force it requires to accelerate it to a given velocity
//				this.rigidbody2D.AddForce(-Vector2.up * ForceScale * 10);
//			}
//			break;
//		case GameManager.State.Monster:
//			break;
//		case GameManager.State.AR:
//			break;
//		case GameManager.State.GameOver:
//			break;
//			
//		}
//	}
//	
//	void OnCollisionEnter2D(Collision2D coll)
//	{
//		if (coll.gameObject.tag == "Ground") {
//			groundHit = true;
//		}
//	}

	// Update is called once per frame
	void Update () {
		
		if(Input.touchCount > 0){
			
			// One finger tap
			Touch touch = Input.GetTouch(0);
			
			Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

			if(touch.phase == TouchPhase.Began && agentCollider.collider2D.OverlapPoint(touchPosition))
			{
				// One finger single tap
				if(touch.tapCount == 2)
				{
//					doubleTap = true;
					Debug.Log("Tap twice");
				}
				// One finger double tap
				else if(touch.tapCount == 1)
				{
//					singleTap = true;

					if(AgentManager.Instance.PrevState == AgentManager.State.Instructing && AgentManager.Instance.CurrentState == AgentManager.State.Idle)
					{
						Debug.Log("Tap Instructing");
//						StopCoroutine("AgentIntroduction");
						AgentManager.Instance.CurrentState = AgentManager.State.Instructing;
//						StartCoroutine("AgentIntroduction");
					}
					else
					{
						Debug.Log("Tap else " + AgentManager.Instance.PrevState);
						StartCoroutine(TapBehavior(2f));
					}

					Debug.Log("Tap once");
				}
			}
		}
	}

	void ResetPosScale()
	{
		agentTransform.position = new Vector2(0.72f,1.33f);
		agentTransform.localScale = new Vector2(0.4f,0.4f);
	}

	private Vector2 targetPosFound = new Vector2(-3.2f,-2.93f);
	private Vector2 targetPosLost = new Vector2(-6.4f,-7f);
	private bool triggerOnce = true;

	// MAIN FSM
	IEnumerator Start ()
	{
        GameManager.Instance.CurrentState = GameManager.State.GameStart;
       
        while(true)
		{
			switch(GameManager.Instance.CurrentState)
			{
			case GameManager.State.SplashScreen:

				// First time in the state
				if(GameManager.Instance.CurrentState != GameManager.Instance.PrevState)
				{
					sprite.InitializeBlackSprites();
					GameManager.Instance.CurrentState = GameManager.State.SplashScreen;
				}

				break;

			case GameManager.State.GameStart:

                // First time in the state
				if(GameManager.Instance.CurrentState != GameManager.Instance.PrevState)
				{
					AgentManager.Instance.CurrentState = AgentManager.State.None;
					sprite.InitializeBlackSprites();
					ResetPosScale();
//					agentRigidBody.gravityScale = 1;
					
					// Initial State
				    if (GameManager.Instance.Relational)
				    {
                        AgentManager.Instance.CurrentState = AgentManager.State.Greating;
				    }
					else
						AgentManager.Instance.CurrentState = AgentManager.State.Instructing;

					//GameManager.Instance.CurrentState = GameManager.State.GameStart;
					//Debug.Log(" ++++++ cur != prev => First time in the state");
				}
				// In the state
				else if(GameManager.Instance.CurrentState == GameManager.Instance.PrevState)
				{
					yield return StartCoroutine("Agent"); //Coroutine should be stopped when changing state due to waitForSec affecting FSM

				}

				break;

			case GameManager.State.Monster:

				// First time in the state
				if(GameManager.Instance.CurrentState != GameManager.Instance.PrevState)
				{
					Debug.Log("CUR STATE = " + GameManager.Instance.CurrentState + " - PREV STATE = " + GameManager.Instance.PrevState);

					GameManager.Instance.CurrentGameState = GameManager.GameState.CameraOff;
					Debug.Log(" ############ GameState: " + GameManager.Instance.CurrentGameState);

					if(GameManager.Instance.PrevState == GameManager.State.GameStart)
					{
						ResetPosScale();
						AgentManager.Instance.CurrentState = AgentManager.State.Instructing;
					}
					else if(GameManager.Instance.PrevState == GameManager.State.AR)
					{
						sprite.SwapSprite();
						ResetPosScale();
						//Reset teaching and rotation
						AgentManager.Instance.Teaching.Clear();
						GameManager.Instance.CurrentGameState = GameManager.GameState.None;
						agentTransform.rotation = Quaternion.identity;

						AgentManager.Instance.CurrentState = AgentManager.State.Idle;
					}

					GameManager.Instance.CurrentState = GameManager.State.Monster;

				}
				// In the state
				else if(GameManager.Instance.CurrentState == GameManager.Instance.PrevState)
				{
					yield return StartCoroutine("Agent");
				}



				break;

			case GameManager.State.AR:

				// First time in the state
				if(GameManager.Instance.CurrentState != GameManager.Instance.PrevState)
				{
					GameManager.Instance.CurrentGameState = GameManager.GameState.CameraOn;
					Debug.Log(" ############ GameState: " + GameManager.Instance.CurrentGameState);

					// Initial rotation and pos
					AgentManager.Instance.CurrentState = AgentManager.State.None;
					agentTransform.position = targetPosLost;
					agentTransform.Rotate(0,0,agentRotation,Space.Self);
					sprite.SwapSprite();
					GameManager.Instance.CurrentState = GameManager.State.AR;

				}
				// In the state
				else if(GameManager.Instance.CurrentState == GameManager.Instance.PrevState)
				{
					yield return StartCoroutine("Agent");

					//need same logic here as in achievement such that it only fires one time every time tracked
					if(GameManager.Instance.CurrentGameState == GameManager.GameState.ArtworkFound || 
					   GameManager.Instance.CurrentGameState == GameManager.GameState.ArtworkRecognized)
					{
						if(!fireOnce)
						{
							sprite.SwapSprite();
							yield return StartCoroutine(tween.Move(targetPosLost, targetPosFound, 0.15f, Tween2D.EasingMethod.Berp));

							AgentManager.Instance.CurrentState = AgentManager.State.Teaching;
//							AgentManager.Instance.isTeaching = true;
							fireOnce = true;
							triggerOnce = false;
						}
					} 
					else if(GameManager.Instance.CurrentGameState == GameManager.GameState.NotFound)
					{
						if(!triggerOnce && !Achievement.Show)
						{
							fireOnce = false;
							yield return new WaitForSeconds(2f);
							yield return StartCoroutine(tween.Move(targetPosFound, targetPosLost, 0.15f, Tween2D.EasingMethod.Berp));
		
							AgentManager.Instance.CurrentState = AgentManager.State.None;
							AgentManager.Instance.Teaching.Clear();
							triggerOnce = true;
						}
					}
				}

				break;

			case GameManager.State.GameOver:

				// First time in the state
				if(GameManager.Instance.CurrentState != GameManager.Instance.PrevState)
				{
					sprite.SwapSprite();
					ResetPosScale();

					// Initial State
					if(GameManager.Instance.Relational)
						AgentManager.Instance.CurrentState = AgentManager.State.FareWell;
					else
						AgentManager.Instance.CurrentState = AgentManager.State.Idle;

					GameManager.Instance.CurrentState = GameManager.State.GameOver;
				}
				// In the state
				else if(GameManager.Instance.CurrentState == GameManager.Instance.PrevState)
				{
					yield return StartCoroutine("Agent");
				}

				break;
			
			}
			yield return null;
		}
	}



	// SUB FSM
	// See http://answers.unity3d.com/questions/300864/how-to-stop-a-co-routine-in-c-instantly.html if string version becomes a problem
	IEnumerator Agent()
	{
//		AnimatorStateInfo stateInfo = agentAnim.GetCurrentAnimatorStateInfo(0);

		switch(AgentManager.Instance.CurrentState)
		{
		case AgentManager.State.None:
			break;
		case AgentManager.State.Idle:

			countIdle++;
			Debug.Log("Times in " + AgentManager.Instance.CurrentState + ": " + countIdle);

			if(AgentManager.Instance.PrevState == AgentManager.State.Instructing && GameManager.Instance.CurrentState == GameManager.State.GameStart)
			{
				Debug.Log("Cur state " + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
				agentAnim.SetBool(idleHash, true);
				yield return new WaitForSeconds(6); //wait and repeat
				agentAnim.SetBool(idleHash, false);

				//if first time go to instuction again => else go to greating
				if(countIdle % 2 == 0 && GameManager.Instance.Relational)
					AgentManager.Instance.CurrentState = AgentManager.State.Greating;
				else
					AgentManager.Instance.CurrentState = AgentManager.State.Instructing;

			}

			if(GameManager.Instance.CurrentState == GameManager.State.Monster)
			{
				agentAnim.SetBool(idleHash, true);
				yield return new WaitForSeconds(6); //wait and repeat

				if(GameManager.Instance.Relational)
				{
					if(countIdle % 2 == 0)
					{
						Debug.Log("Cur state " + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
						agentAnim.SetBool(idleHash, false);
						AgentManager.Instance.CurrentState = AgentManager.State.Thinking;
					}
					else if(countIdle % 2 == 1 && AgentManager.Instance.PrevState == AgentManager.State.SelfDisclosing)
					{
						Debug.Log("Cur state " + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
						agentAnim.SetBool(idleHash, false);
						AgentManager.Instance.CurrentState = AgentManager.State.Sleeping;
					}
				}
				else
				{
					AgentManager.Instance.CurrentState = AgentManager.State.Idle;
				}
			}

			if(GameManager.Instance.CurrentState == GameManager.State.GameOver)
			{

				agentAnim.SetBool(idleHash, true);
				yield return new WaitForSeconds(6); //wait and repeat
				
				if(GameManager.Instance.Relational)
				{
					// Stay in idle if prev state was farewell
					if(AgentManager.Instance.PrevState == AgentManager.State.FareWell)
					{
						Debug.Log("Cur state " + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
						AgentManager.Instance.CurrentState = AgentManager.State.Idle;
					}
				}
				else
				{
					AgentManager.Instance.CurrentState = AgentManager.State.Idle;
				}
			}

//			// Stay in idle if prev state was farewell
//			if(AgentManager.Instance.PrevState == AgentManager.State.FareWell)
//			{
//				Debug.Log("Cur state " + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
//				AgentManager.Instance.CurrentState = AgentManager.State.Idle;
//			}

			break;

		case AgentManager.State.Instructing:

			countInstructing++;
			Debug.Log("Times in " + AgentManager.Instance.CurrentState + ": " + countInstructing);

			if(GameManager.Instance.CurrentState == GameManager.State.GameStart)
			{
				Debug.Log("Cur state" + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
				agentAnim.SetBool(instructingHash,true);
				yield return StartCoroutine(speechRenderer.Utterance(AgentManager.Instance.Instruction));
				agentAnim.SetBool(instructingHash,false);

				AgentManager.Instance.CurrentState = AgentManager.State.Idle;
			}

			// If second time here go to greating again
			if(AgentManager.Instance.PrevState == AgentManager.State.Idle && GameManager.Instance.CurrentState == GameManager.State.GameStart)
			{
				Debug.Log("Cur state" + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);	agentAnim.SetBool(instructingHash,true);
				yield return StartCoroutine(speechRenderer.Utterance(AgentManager.Instance.Instruction));
				agentAnim.SetBool(instructingHash,false);

				AgentManager.Instance.CurrentState = AgentManager.State.Idle;
			}

			if(GameManager.Instance.CurrentState == GameManager.State.Monster)
			{
				Debug.Log("Cur state" + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
				agentAnim.SetBool(instructingHash,true);
				yield return StartCoroutine(speechRenderer.Utterance(AgentManager.Instance.UI_Instruction));
				agentAnim.SetBool(instructingHash,false);
				
				AgentManager.Instance.CurrentState = AgentManager.State.Idle;
			}

			break;

		case AgentManager.State.Greating:
			
			countGreating++;
			Debug.Log("Times in " + AgentManager.Instance.CurrentState + ": " + countGreating);
			
			if(AgentManager.Instance.PrevState == AgentManager.State.None || AgentManager.Instance.PrevState == AgentManager.State.Idle)
			{
				Debug.Log("Cur state" + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
				agentAnim.SetTrigger(greatingHash);
				yield return new WaitForSeconds(2);
				mouthAnim.SetBool("Happy", true);
				yield return new WaitForSeconds(1);
				mouthAnim.SetBool("Happy", false);
				yield return StartCoroutine(speechRenderer.Utterance(AgentManager.Instance.Greating));
				AgentManager.Instance.CurrentState = AgentManager.State.Instructing;
			}

			if(AgentManager.Instance.PrevState == AgentManager.State.Instructing)
			{
				Debug.Log("Cur state" + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
				agentAnim.SetTrigger(greatingHash);
				yield return new WaitForSeconds(2);
				mouthAnim.SetBool("Happy", true);
				yield return new WaitForSeconds(1);
				mouthAnim.SetBool("Happy", false);
				yield return StartCoroutine(speechRenderer.Utterance(AgentManager.Instance.Greating));
				AgentManager.Instance.CurrentState = AgentManager.State.Idle;
			}
			
			break;

		case AgentManager.State.FareWell:
			
			countFarewell++;
			Debug.Log("Times in " + AgentManager.Instance.CurrentState + ": " + countFarewell);

			if(AgentManager.Instance.CurrentState != AgentManager.Instance.PrevState)
			{
				Debug.Log("Cur state" + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
				agentAnim.SetTrigger(greatingHash);
				yield return new WaitForSeconds(2);
				mouthAnim.SetBool("Happy", true);
				yield return new WaitForSeconds(1);
				mouthAnim.SetBool("Happy", false);
				yield return StartCoroutine(speechRenderer.Utterance(AgentManager.Instance.FareWell));
				AgentManager.Instance.CurrentState = AgentManager.State.Idle;
			}
			
			break;

		case AgentManager.State.Thinking:

			countThinking++;
			Debug.Log("Times in " + AgentManager.Instance.CurrentState + ": " + countThinking);

			if(AgentManager.Instance.PrevState == AgentManager.State.Idle)
			{
				Debug.Log("Cur state" + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
				
				agentAnim.SetBool(thinkingHash,true);
				yield return new WaitForSeconds(12);
				agentAnim.SetBool(thinkingHash,false);
				AgentManager.Instance.CurrentState = AgentManager.State.SelfDisclosing;
			}
			
			break;

		case AgentManager.State.Waiting:

			countWaiting++;
			Debug.Log("Times in " + AgentManager.Instance.CurrentState + ": " + countWaiting);

			if(AgentManager.Instance.CurrentState != AgentManager.Instance.PrevState)
			{
				Debug.Log("Cur state" + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
				AgentManager.Instance.CurrentState = AgentManager.State.Waiting;
			}

			
			break;

		case AgentManager.State.Sleeping:

			countSleeping++;
			Debug.Log("Times in " + AgentManager.Instance.CurrentState + ": " + countSleeping);

			agentAnim.SetBool(sleepingHash,true);
			yield return new WaitForSeconds(24);
			agentAnim.SetBool(sleepingHash,false);
			AgentManager.Instance.CurrentState = AgentManager.State.Idle;
			
			break;
		
		case AgentManager.State.SelfDisclosing:
			
			countSelfDisclosing++;
			Debug.Log("Times in " + AgentManager.Instance.CurrentState + ": " + countSelfDisclosing);
			
			if(AgentManager.Instance.PrevState == AgentManager.State.Thinking)
			{
				//reset count if all self-disclosing utterances has been rendered
				if(countSelfDisclosing == AgentManager.Instance.SelfDisclosing.Count)
					countSelfDisclosing = 0;
				
				Debug.Log("Cur state" + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
				
				if(countSelfDisclosing % 2 == 1)
					agentAnim.SetBool(thinkingHash,true);
				else
					agentAnim.SetBool(idleHash,true);
				
				yield return StartCoroutine(speechRenderer.RandomUtterance(AgentManager.Instance.SelfDisclosing));
				
				if(countSelfDisclosing % 2 == 1)
					agentAnim.SetBool(thinkingHash,false);
				else
					agentAnim.SetBool(idleHash,true);
				
				AgentManager.Instance.CurrentState = AgentManager.State.Idle;
			}
			
			break;

//		case AgentManager.State.Commenting:
//						
//			countCommenting++;
//			Debug.Log("Times in " + AgentManager.Instance.CurrentState + ": " + countCommenting);
//
//			if(!Achievement.Show && AgentManager.Instance.CurrentState != AgentManager.Instance.PrevState)
//			{
//				Debug.Log("Cur state" + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);
//				sprite.SwapSprite();
//
//				if(GameManager.Instance.CurrentGameState == GameManager.GameState.ArtworkFound)
//				{
//					yield return StartCoroutine(speechRenderer.RandomUtterance(AgentManager.Instance.Found));
//				}
//				else if(GameManager.Instance.CurrentGameState == GameManager.GameState.ArtworkRecognized)
//				{
//					yield return StartCoroutine(speechRenderer.RandomUtterance(AgentManager.Instance.Recognized));
//				}
//
//				AgentManager.Instance.CurrentState = AgentManager.State.Teaching;
//			}
//			
//			break;
		
		case AgentManager.State.Teaching:

			sprite.SwapSprite();

			if(!Achievement.Show && GameManager.Instance.CurrentGameState == GameManager.GameState.ArtworkFound ||
			   !Achievement.Show && GameManager.Instance.CurrentGameState == GameManager.GameState.ArtworkRecognized)
			{
				countTeaching++;

				AgentManager.Instance.KnowledgeBase();

				Debug.Log("Times in " + AgentManager.Instance.CurrentState + ": " + countTeaching);
				Debug.Log("Cur state" + AgentManager.Instance.CurrentState + " prev state " + AgentManager.Instance.PrevState);

				if(GameManager.Instance.Relational)
				{
					mouthAnim.SetBool("Happy", true);
					yield return new WaitForSeconds(1);
					mouthAnim.SetBool("Happy", false);
				}

				yield return StartCoroutine(speechRenderer.Utterance(AgentManager.Instance.Teaching));

				AgentManager.Instance.CurrentState = AgentManager.State.Teaching;
				if(!GameManager.Instance.TrackableFound)
					break;
				yield return new WaitForSeconds(2f);
			} 

			break;
		}

		yield return null;
	}

	IEnumerator IntimateProximity()
	{

		yield return null;
	}

	IEnumerator PersonalProximity()
	{
//		agentRigidBody.gravityScale = 0;
		StartCoroutine(tween.Scale(agentTransform.localScale, new Vector2(0.8f,0.8f), 0.5f, Tween2D.EasingMethod.Sinerp));
		StartCoroutine(tween.Move(agentTransform.position, new Vector2(0.43f,1.24f), 0.5f, Tween2D.EasingMethod.Berp));
		yield return null;

	}

	IEnumerator TapBehavior(float waitTime)
	{
		if(GameManager.Instance.Relational)
		{

			agentAnim.SetTrigger("Tap");
			yield return new WaitForSeconds(0.05f);
			mouthAnim.SetBool("Happy", true);
			yield return new WaitForSeconds(waitTime);
			mouthAnim.SetBool("Happy", false);
		}

		yield return null;
	}
	
	IEnumerator StrokeBehavior()
	{
		if(GameManager.Instance.Relational)
		{
			
		}

		yield return null;
	}
	
}
