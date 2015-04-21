using UnityEngine;
using System;
using System.Collections;

public class ArtworkToBeFound : IComparable<ArtworkToBeFound>{
	
	public string ArtworkName { get; set; }
	public string BodyPart { get; set; }

	#region IComparable implementation
	public int CompareTo (ArtworkToBeFound other)
	{
//		return other.BodyPart.CompareTo(this.BodyPart);
		return this.BodyPart.CompareTo(other.BodyPart);
	}
	#endregion
}
