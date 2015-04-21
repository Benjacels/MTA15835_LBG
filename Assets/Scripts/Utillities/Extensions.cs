using UnityEngine;
using System;
using System.Collections;

public static class Extensions {

	/// <summary>
	/// Remap the specified value, from1, to1, from2 and to2.
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="from1">From1.</param>
	/// <param name="to1">To1.</param>
	/// <param name="from2">From2.</param>
	/// <param name="to2">To2.</param>
	public static float Remap (this float value, float from1, float to1, float from2, float to2) {
		
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		
	}

	/// <summary>
	/// Finds objects with tag and sorts them according to object name
	/// </summary>
	/// <returns>Sorted objects with tag.</returns>
	/// <param name="tag">Tag.</param>
	public static GameObject[] FindAndSortObjsWithTag( string tag )
	{
		GameObject[] foundObjs = GameObject.FindGameObjectsWithTag(tag);
		Array.Sort( foundObjs, CompareObjNames );
		return foundObjs;
	}
	
	private static int CompareObjNames( GameObject x, GameObject y )
	{
		return x.name.CompareTo( y.name );
	}

}
