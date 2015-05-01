using UnityEngine;
using System.Collections;

public class CompareClassValue : IComparer {

	public int Compare(object x, object y){
		if(x == null) return -1;
		
		return string.Compare((x as Artwork).TrackableName, (string)y);
	}
}
