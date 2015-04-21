using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour {
	
	private Transform agent;
	private Transform speechBubble;
	private Vector3 offset;
	private Vector3 offsetLeft = new Vector3(4.2f,3.1f,0);
	private Vector3 offsetRight = new Vector3(-4.2f,3.1f,0);
	private float BoundaryLeft = -4.5f;
	private float BoundaryRight = 4.5f;




	
	void Awake()
	{
		agent = GameObject.Find ("Body").transform;
		speechBubble = this.transform;

	}

	void Start()
	{
		offset = offsetLeft;
	}
	
	void Update()
	{
		speechBubble.position = agent.position + offset;

		if(speechBubble.position.x > BoundaryRight)
		{
			speechBubble.localScale = new Vector2(-1, 1);
			offset = offsetRight;

		} 
		else if(speechBubble.position.x < BoundaryLeft)
		{
			speechBubble.localScale = new Vector2(1, 1);
			offset = offsetLeft;

		}
	}


}
