using UnityEngine;
using System.Collections;

public class Tweening {
		
	/// <summary>
	/// Hermite - interpolate while easeing in out at the limits i.e. the specified start and end. 
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector2 Hermite(Vector2 start, Vector2 end, float value) {
		return new Vector2(Mathfx.Hermite(start.x, end.x, value),Mathfx.Hermite(start.y, end.y, value));
	}
	/// <summary>
	/// Hermite - interpolate while easeing in out at the limits i.e. the specified start and end. 
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector3 Hermite(Vector3 start, Vector3 end, float value) {
		return new Vector3(Mathfx.Hermite(start.x, end.x, value),Mathfx.Hermite(start.y, end.y, value), Mathfx.Hermite(start.z, end.z, value));
	}
		
	/// <summary>
	/// Sinerp - interpolate while easing around the end, when value is near one. 
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector2 Sinerp(Vector2 start, Vector2 end, float value){
		return new Vector2(Mathfx.Sinerp(start.x, end.x, value), Mathfx.Sinerp(start.y, end.y, value));
	}
	/// <summary>
	/// Sinerp - interpolate while easing around the end, when value is near one. 
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector3 Sinerp(Vector3 start, Vector3 end, float value){
		return new Vector3(Mathfx.Sinerp(start.x, end.x, value), Mathfx.Sinerp(start.y, end.y, value), Mathfx.Sinerp(start.z, end.z, value));
	}

	/// <summary>
	/// Coserp interpolate while easing around the specified startv, when value is near zero.
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector2 Coserp(Vector2 start, Vector2 end, float value)
	{
		return new Vector2(Mathfx.Coserp(start.x, end.x, value),Mathfx.Coserp(start.y, end.y, value));
	}
	/// <summary>
	/// Coserp interpolate while easing around the specified startv, when value is near zero.
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector3 Coserp(Vector3 start, Vector3 end, float value)
	{
		return new Vector3(Mathfx.Coserp(start.x, end.x, value),Mathfx.Coserp(start.y, end.y, value), Mathfx.Coserp(start.z, end.z, value));
	}

	/// <summary>
	/// Short for 'boing-like interpolation', this method will first overshoot, then waver back and forth around the end value before coming to a rest. 
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector2 Berp(Vector2 start, Vector2 end, float value) {
		return new Vector2(Mathfx.Berp(start.x, end.x, value),Mathfx.Berp(start.y, end.y, value));
	}
	/// <summary>
	/// Short for 'boing-like interpolation', this method will first overshoot, then waver back and forth around the end value before coming to a rest. 
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector3 Berp(Vector3 start, Vector3 end, float value) {
		return new Vector3(Mathfx.Berp(start.x, end.x, value),Mathfx.Berp(start.y, end.y, value), Mathfx.Berp(start.z, end.z, value));
	}
	//could implement Bounce for every method and not just linear interpolation i.e. 
	//if !EaseMethod.Bounce return new vector2(Mathfx.Method(start.x, end.x, value),Mathfx.Method(start.y, end.y, value))
	//else return new vector2(Mathfx.Method(start.x, end.x, Mathfx.Bounce(value)),Mathfx.Method(start.y, end.y, Mathfx.Bounce(value)))

	/// <summary>
	/// Bounce - lerp with a bounce around the value.
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector2 Bounce(Vector2 start, Vector2 end, float value){
		return Vector2.Lerp(start,end,Mathfx.Bounce(value));
	}
	/// <summary>
	/// Bounce - lerp with a bounce around the value.
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector3 Bounce(Vector3 start, Vector3 end, float value){
		return Vector3.Lerp(start,end,Mathfx.Bounce(value));
	}

	/// <summary>
	/// Smooths the step.
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector2 SmoothStep(Vector2 start, Vector2 end, float value){
//		return new Vector2(Mathfx.SmoothStep(value,start.x,end.x), Mathfx.SmoothStep(value,start.y,end.y));
		return new Vector2(Mathfx.SmoothStep(value,start.x,end.x), Mathfx.SmoothStep(value,start.y,end.y));
	}
	/// <summary>
	/// Smooths the step.
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="value">Value.</param>
	public static Vector3 SmoothStep(Vector3 start, Vector3 end, float value){
		return new Vector3(Mathfx.SmoothStep(value,start.x,end.x),Mathfx.SmoothStep(value,start.y,end.y),Mathfx.SmoothStep(value,start.z,end.z));
	}

}
