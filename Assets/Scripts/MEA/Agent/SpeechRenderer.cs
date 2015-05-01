using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeechRenderer : MonoBehaviour {

	private Tween2D tween;
	public Transform AgentTransform { get; set; }
	private Vector3 offset;
	private Vector3 offsetLeft = new Vector3(4.2f,3.1f,0);
	private Vector3 offsetRight = new Vector3(-4.2f,3.1f,0);
	private float BoundaryLeft = -4.5f;
	private float BoundaryRight = 4.5f;
	
	private GUIStyle style  = new GUIStyle();
	
	private Rect textRect;
	private Vector2 scale;
	private int fontSize;
	private float textBoxWidth;
	private float textBoxHeight;
	private Vector2 textPosition;
	private float textOffset;
	
	private GameObject speechBubble;
	private SpriteRenderer speechBubbleRenderer;
	private Transform speechBubbleTransform;
	private Vector2 speechBubbleScreen;

	public bool Utter { get; set; }
	
	public string Text { get; set; }

	private float easeOut = 0.1f;
	private float easeIn = 0.1f;
//	private float alpha = 0;

	void Awake()
	{
		tween = GetComponent<Tween2D>();

		scale = DynamicGUI.DynamicScale();
		Font myFontBold = Resources.Load("amatic/Amatic-Bold") as Font;

		speechBubbleTransform = this.transform;
		speechBubbleRenderer = GetComponent<SpriteRenderer>();
		speechBubbleRenderer.color = new Color(0,0,0,0);

		textBoxWidth = speechBubbleRenderer.sprite.rect.width - 180;
		textBoxHeight = speechBubbleRenderer.sprite.rect.height - 120;
		textOffset = -50;

		style.font = myFontBold;
		style.wordWrap = true;
		//		style.clipping = TextClipping.Clip;
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = ColorTheme.black;

		textPosition = Camera.main.WorldToScreenPoint(speechBubbleTransform.position);
	}
	
	void Start()
	{
		if(speechBubbleTransform.position.x > -3.7f)
			offset = offsetLeft;

	}
	
	void Update()
	{
		speechBubbleRenderer.color = new Color(1,1,1,tween.AlphaValue);
//		speechBubbleRenderer.color = new Color(1,1,1,alpha);
		if(!GameManager.Instance.isTesting)
		{
			speechBubbleTransform.position = AgentTransform.position + offset;
			
			if(speechBubbleTransform.position.x > BoundaryRight)
			{
				speechBubbleTransform.localScale = new Vector2(-1, 1);
				offset = offsetRight;
				textOffset = 50;
			} 
			else if(speechBubbleTransform.position.x < BoundaryLeft)
			{
				speechBubbleTransform.localScale = new Vector2(1, 1);
				offset = offsetLeft;
				textOffset = -50;
			}
		} 
		else
		{
			speechBubbleTransform.position = Vector2.zero;
		}

	}

	void OnGUI()
	{
		GUI.depth = 10;

		textPosition = Camera.main.WorldToScreenPoint(speechBubbleTransform.position);
		textRect = new Rect(textPosition.x - ((textBoxWidth/2 + textOffset) * scale.x), Screen.height - textPosition.y - (textBoxHeight/2 * scale.y), textBoxWidth * scale.x, textBoxHeight * scale.y);

		GUI.Label(textRect,"<size=" + fontSize + ">" + Text + "</size>",style);
	}
	

	public IEnumerator Utterance(List<Utterance> utterance)
	{
		Utterance lastItem = utterance[utterance.Count - 1];
		
		yield return StartCoroutine(tween.Alpha(0,1,easeIn, Tween2D.EasingMethod.Sinerp));
		Utter = true;
		foreach(Utterance element in utterance)
		{
			fontSize = DynamicGUI.DynamicFontSize(element.FontSize(),scale.x);
			Debug.Log(" FONT SIZE = " + element.FontSize());
			if(element != lastItem)
			{
				Text = element.Sentence;
				
				if(element.Duration <= 0)
					yield return new WaitForSeconds(element.CalculatedDuration());
				else
					yield return new WaitForSeconds(element.Duration);

			}
			else
			{
				Text = element.Sentence;
				
				if(element.Duration <= 0)
					yield return new WaitForSeconds(element.CalculatedDuration());
				else
					yield return new WaitForSeconds(element.Duration);
				
				Text = string.Empty;
				yield return new WaitForSeconds(0.05f);
				yield return StartCoroutine(tween.Alpha(1,0,easeOut,Tween2D.EasingMethod.Sinerp));
				yield return new WaitForSeconds(element.Silence);
			}
			
		}
		
		Utter = false;
	}
	
	public IEnumerator RandomUtterance(List<Utterance> utterance)
	{
		int index = Random.Range(0,utterance.Count-1); //use algorithm that makes sure it is not drawn again unless all utterances has been used
		
		yield return StartCoroutine(tween.Alpha(0,1,easeIn,Tween2D.EasingMethod.Sinerp));
		Utter = true;
		
		fontSize = DynamicGUI.DynamicFontSize(utterance[index].FontSize(),scale.x);
		
		Text = utterance[index].Sentence;
		
		if(utterance[index].Duration <= 0)
			yield return new WaitForSeconds(utterance[index].CalculatedDuration());
		else
			yield return new WaitForSeconds(utterance[index].Duration);
		
		Text = string.Empty;
		
		yield return new WaitForSeconds(0.01f);
		
		yield return StartCoroutine(tween.Alpha(1,0,easeOut,Tween2D.EasingMethod.Sinerp));
		yield return new WaitForSeconds(utterance[index].Silence);
		
		Utter = false;
	}

	public IEnumerator Utterance(List<Utterance> utterance, int index)
	{	
		yield return StartCoroutine(tween.Alpha(0,1,0.1f,Tween2D.EasingMethod.Sinerp));
		Utter = true;

		fontSize = DynamicGUI.DynamicFontSize(utterance[index].FontSize(),scale.x); //int should depend on number of words
//		Debug.Log("Num of Chars: " + utterance[index].CharCount());
//		Debug.Log("Estimated Duration: " + utterance[index].CalculatedDuration() + " Duration: " + utterance[index].Duration);
//		Debug.Log("Silence: " + utterance[index].Silence);
		Text = utterance[index].Sentence;

		if(utterance[index].Duration <= 0)
			yield return new WaitForSeconds(utterance[index].CalculatedDuration());
		else
			yield return new WaitForSeconds(utterance[index].Duration);

		Text = string.Empty;
		yield return new WaitForSeconds(0.05f);
		yield return StartCoroutine(tween.Alpha(1,0,0.15f,Tween2D.EasingMethod.Sinerp));
		yield return new WaitForSeconds(utterance[index].Silence);
		
		Utter = false;
	}	
}
