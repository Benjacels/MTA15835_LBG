using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	[Range (0, 1000)]
	public float forceScale = 450f; //should be in monster behavior script and get state from monster manager
	private bool groundHit;

	private void FixedUpdate() //fixed update is used because of the use of physics
	{
		//Character State Introduction
		if(GameManager.Instance.CurrentState == GameManager.State.GameStart && groundHit == false) 
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

}
