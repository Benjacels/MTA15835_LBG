using UnityEngine;
using System.Collections;

public class DrawBoundaries : MonoBehaviour {

	void OnDrawGizmos() {

			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(new Vector3(0,4.25f,0), new Vector3(14, 17, 1));

	}
}
