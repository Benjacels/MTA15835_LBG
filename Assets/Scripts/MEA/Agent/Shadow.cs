using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour {

	public float shadowMax = 1.5f;
	public float shadowMin = 0.7f;

//	private GameObject AgentTransform;
	public Transform AgentTransform { get; set; }
	private Vector3 position;
	private Vector3 scale;
	private Vector3 agentBodyPosition;
//	private float initialDist;
	private float prevDist = 0;

	void Awake()
	{
//		DontDestroyOnLoad(this.gameObject);

		//cash position and scale
		position = this.transform.position;
		scale = this.transform.localScale;
		
		if (AgentTransform != null) {
			agentBodyPosition = AgentTransform.position;
//			initialDist = Vector3.Distance(agentBodyPosition,position);
		}
	}

	void Update()
	{
		//position shadow based on the position of the monsters body
		//scale shadow based on distance remapped to min and max shadow scale

		if (AgentTransform != null) {

			agentBodyPosition = AgentTransform.position;
			position.x = agentBodyPosition.x;
			transform.position = position;
			
			float dist = (agentBodyPosition - transform.position).magnitude;

			if(dist < prevDist){
		
				Vector3 shadowScale = scale * dist;

				transform.localScale = new Vector3(shadowScale.x.Remap(0,dist,shadowMin,shadowMax),shadowScale.y.Remap(0,dist,shadowMin,shadowMax),1);

			}

			prevDist = dist;
		}
	}	
}
