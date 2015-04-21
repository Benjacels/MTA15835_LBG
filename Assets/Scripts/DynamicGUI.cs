using UnityEngine;
using System.Collections;

public static class DynamicGUI {

	public static int DynamicFontSize(int fontSize, float scale){
		
		//adjust size of dynamic font according to screen resolution
		return (int)Mathf.Floor(fontSize * scale);
	}
	
	public static Vector2 DynamicScale(float width = 1024, float height = 768){
		//scale based on a native resolution
		Vector2 scale = new Vector2();
		
		scale.x = Screen.width / width;
		scale.y = Screen.height / height;
		
		return scale;
	}
	
	public static Rect DynamicRect(Vector2 rectPosition, Vector2 rectDimension, Vector2 dynamicScale, float marginTop = 0, 
	                        float marginRight = 0, float marginBottom = 0, float marginLeft = 0){
		
		//dynamic rect position, dimension and margin
		//position: top-left (0,0) top-right (1,0) bottom-left(0,1) bottom-right (1,1)
		Rect dynamicRect = new Rect();
		
		marginTop *= dynamicScale.y;
		marginBottom *= dynamicScale.y;
		marginLeft *= dynamicScale.x;
		marginRight *= dynamicScale.x;
		
		dynamicRect.width = rectDimension.x * dynamicScale.x;
		dynamicRect.height = rectDimension.y * dynamicScale.y;
		
		dynamicRect.x = (rectPosition.x * (Screen.width - dynamicRect.width)) - marginRight + marginLeft;
		dynamicRect.y = (rectPosition.y * (Screen.height - dynamicRect.height)) - marginBottom + marginTop;
		
		return dynamicRect;
	}
}
