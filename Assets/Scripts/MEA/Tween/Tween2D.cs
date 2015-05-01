using UnityEngine;
using System.Collections;

public class Tween2D : MonoBehaviour {


	public enum EasingMethod{
		/// <summary>
		/// Hermite - This method will interpolate while easing in and out at the limits. 
		/// </summary>
		Hermite,
		/// <summary>
		/// Sinerp - Short for 'sinusoidal interpolation', this method will interpolate while easing around the end, when value is near one. 
		/// </summary>
		Sinerp,
		/// <summary>
		/// Coserp - Similar to Sinerp, except it eases in, when value is near zero, instead of easing out (and uses cosine instead of sine). 
		/// </summary>
		Coserp,
		/// <summary>
		/// Linear i.e. Lerp - Short for 'linearly interpolate' - but with some smoothing
		/// </summary>
		Linear,
		/// <summary>
		/// Berp - Short for 'boing-like interpolation', this method will first overshoot, then waver back and forth around the end value before coming to a rest. 
		/// </summary>
		Berp,
		/// <summary>
		/// SmoothStep - Works like Lerp, but has ease-in and ease-out of the values. 
		/// </summary>
		SmoothStep,
		/// <summary>
		/// Bounce - lerp with a bounce around the value.
		/// </summary>
		Bounce
	}

	// Position and Dimension are used for Rect (used in the gui system) instead of transform.position and transform.localScale
	public Vector2 Position { get; set;}
	public Vector2 Dimension { get; set; }

	//Alpha
	public float AlphaValue { get; set; }
	public bool AlphaTransitionFinished 
	{ 
		get{ return alphaTransitionFinished; }
	}
	private bool alphaTransitionFinished = false;

	private Vector2 EaseMethod(Vector2 start, Vector2 end, float duration, float startTime, EasingMethod easingMethod)
	{

		switch(easingMethod)
		{
		case EasingMethod.Linear:
			return Vector2.Lerp(start,end, (Time.time - startTime)/duration);
		case EasingMethod.Hermite:
			return Tweening.Hermite(start,end, (Time.time - startTime)/duration);
		case EasingMethod.Sinerp:
			return Tweening.Sinerp(start,end, (Time.time - startTime)/duration);
		case EasingMethod.Coserp:
			return Tweening.Coserp(start,end, (Time.time - startTime)/duration);
		case EasingMethod.Berp:
			return Tweening.Berp(start, end, (Time.time - startTime)/duration);
		case EasingMethod.SmoothStep:
			return Tweening.SmoothStep(start, end, (Time.time - startTime)/duration);
		case EasingMethod.Bounce:
			return Tweening.Bounce(start, end, (Time.time - startTime)/duration);
		default:
			Debug.Log("no easing method was choosen");
			return Vector2.zero;
		}
	}

	#region IEnumeerators

	public IEnumerator Move(Vector2 start, Vector2 end, float duration, EasingMethod easingMethod)
	{

		Position = start;

		yield return new WaitForSeconds(0.1f);
		
		float startTime = Time.time;

		//while current time is less than initial starttime + the duration
		while(Time.time < startTime + duration){

			Position = EaseMethod(start, end, duration, startTime, easingMethod);
			transform.position = Position;
			
			yield return null;
		}
		
		Debug.Log(" ******** Reached target");
	}

	public IEnumerator Scale(Vector2 start, Vector2 end, float duration, EasingMethod easingMethod)
	{

		Dimension = start;

		yield return new WaitForSeconds(0.1f);
		
		float startTime = Time.time;
		//Most easing methods can be evaluated with distance however bounce and berp cannot function with distance 
		//as these interpolations overshoot thus time is needed to evaluate bounce and berp. 
		//Furthermore floating point inpressision causes evaluation of easing methods to be infinit if distance is evaluated against zero
		while(Time.time < startTime + duration){

			Dimension = EaseMethod(start, end, duration, startTime, easingMethod);
			transform.localScale = Dimension;
			
			yield return null;
		}
		
		Debug.Log(" ******** Scaled");
	}

	//Tween Color
	public IEnumerator Alpha(float start, float end, float duration, EasingMethod easingMethod)
	{

		yield return new WaitForSeconds(0.1f);

		alphaTransitionFinished = false;

		float startTime = Time.time;

		start = Mathf.Clamp01(start);
		end = Mathf.Clamp01(end);

		if(start < 1)
			AlphaValue = 0;
		else if(start >= 1)
			AlphaValue = 1;

		while(Time.time < startTime + duration){

			switch(easingMethod)
			{
			case EasingMethod.Linear:
				AlphaValue = Mathf.Lerp(start, end, (Time.time - startTime)/duration);
				break;
			case EasingMethod.Hermite:
				AlphaValue = Mathfx.Hermite(start, end, (Time.time - startTime)/duration);
				break;
			case EasingMethod.Sinerp:
				AlphaValue = Mathfx.Sinerp(start,end,(Time.time - startTime)/duration);
				break;
			case EasingMethod.Coserp:
				AlphaValue = Mathfx.Coserp(start,end,(Time.time - startTime)/duration);
				break;
			case EasingMethod.Berp:
				AlphaValue = Mathfx.Berp(start,end,(Time.time - startTime)/duration);
				break;
			case EasingMethod.SmoothStep:
				AlphaValue = Mathfx.SmoothStep((Time.time - startTime)/duration,start,end);
				break;
			default:
				Debug.Log("no easing method was choosen");
				break;
			}

			yield return null;
		}

		// Due to floating point imprecision alphavalue is corrected depending on the starting value
		if(start < 1)
			AlphaValue = 1;
		else if(start >= 1)
			AlphaValue = 0;

		alphaTransitionFinished = true;
		Debug.Log("============ Finished transition with Alpha value: " + AlphaValue);

	}

	IEnumerator Color(){
		yield return null;
	}

	#endregion
}
