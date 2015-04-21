using UnityEngine;
using System.Collections;

public class Text : MonoBehaviour {

//	private GUIStyle style  = new GUIStyle();
//
//	private Rect textRect;
//	private Vector2 scale;
//	private int fontSize;
//	private float textBoxWidth;
//	private float textBoxHeight;
//	private Vector2 textPosition;
//
//	private GameObject speechBubble;
//	private SpriteRenderer speechBubbleRenderer;
//	private Transform speechBubbleTransform;
//	private Vector2 speechBubbleScreen;
//	
//	private string dialog = "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii"; // Figure out how many lines/char there can be in box
//
//	void Awake ()
//	{
//		scale = DynamicGUI.DynamicScale();
//		Font myFontBold = Resources.Load("amatic/Amatic-Bold") as Font;
//
//		speechBubble = GameObject.FindGameObjectWithTag("SpeechBubble");
//		speechBubbleRenderer = speechBubble.GetComponent<SpriteRenderer>();
//		speechBubbleTransform = speechBubble.transform;
//		textBoxWidth = speechBubbleRenderer.sprite.rect.width - 180;
//		textBoxHeight = speechBubbleRenderer.sprite.rect.height - 120;
//
//		Debug.Log(textBoxWidth + " " + textBoxHeight);
//
//		//Header styling
//		style.font = myFontBold;
//		style.wordWrap = true;
////		style.clipping = TextClipping.Clip;
//		style.alignment = TextAnchor.MiddleCenter;
//		//fontsize + lineheight * number of lines
//		Debug.Log(style.lineHeight);
//		style.normal.textColor = ColorTheme.black;
//	}
//
//	void OnGUI()
//	{
//		GUI.depth = 1;
//		//should get position from speechbubble
//		textPosition = Camera.main.WorldToScreenPoint(speechBubble.transform.position);
//		fontSize = DynamicGUI.DynamicFontSize(60,scale.x); //int should depend on number of words
//		//if it changes side it should be + not -
//		textRect = new Rect(textPosition.x - ((textBoxWidth/2 - 50) * scale.x), Screen.height - textPosition.y - (textBoxHeight/2 * scale.y), textBoxWidth * scale.x, textBoxHeight * scale.y);
//		GUI.Label(textRect,"<size=" + fontSize + ">" + dialog + "</size>",style);
//	}

}
