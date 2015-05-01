using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	
	private GameManager gameManager;
	private string emptyPlaceHolder = string.Empty;
	
	//Achievement
	private Rect BadgeRect;
//	public Vector2 position;
	public Vector2 BadgePosition { get; set; }
	public Vector2 BadgeTextureDimension { get; set; }
	private List <Texture2D> achievementBadgeTextureList;
	
	//Inventory
	private Rect InventoryRect;
	public bool Show { get; set; }
	private Texture2D buttonInventoryTexture;
	private Texture2D buttonInventoryTexture2x;
	private GUIStyle buttonInventoryStyle  = new GUIStyle();

	//TODO set to not destroy and load during splachScreen and only display if current level is relationalMonster/neutralMonster || AR || gameover. It will save processing and ram
	private void Awake(){
		
		gameManager = GameManager.Instance;
		
		Vector2 scale = DynamicGUI.DynamicScale();
		
		//Achievement badge
		achievementBadgeTextureList = new List<Texture2D>();
		
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadgeSmall/achievementBadge1small@2x") as Texture2D : Resources.Load("achievementBadgeSmall/achievementBadge1small") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadgeSmall/achievementBadge2small@2x") as Texture2D : Resources.Load("achievementBadgeSmall/achievementBadge2small") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadgeSmall/achievementBadge3small@2x") as Texture2D : Resources.Load("achievementBadgeSmall/achievementBadge3small") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadgeSmall/achievementBadge4small@2x") as Texture2D : Resources.Load("achievementBadgeSmall/achievementBadge4small") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadgeSmall/achievementBadge5small@2x") as Texture2D : Resources.Load("achievementBadgeSmall/achievementBadge5small") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadgeSmall/achievementBadge6small@2x") as Texture2D : Resources.Load("achievementBadgeSmall/achievementBadge6small") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadgeSmall/achievementBadge7small@2x") as Texture2D : Resources.Load("achievementBadgeSmall/achievementBadge7small") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadgeSmall/achievementBadge8small@2x") as Texture2D : Resources.Load("achievementBadgeSmall/achievementBadge8small") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadgeSmall/achievementBadge9small@2x") as Texture2D : Resources.Load("achievementBadgeSmall/achievementBadge9small") as Texture2D);

//		BadgePosition = new Vector2(0.04f,0.943f);
		BadgePosition = new Vector2(0.96f,0.034f);
		BadgeTextureDimension = scale.x > 1 ? new Vector2(achievementBadgeTextureList[0].width,achievementBadgeTextureList[0].height)/2 : new Vector2(achievementBadgeTextureList[0].width,achievementBadgeTextureList[0].height);
		BadgeRect = DynamicGUI.DynamicRect(BadgePosition, BadgeTextureDimension, scale, 0,0,0,0);
		
		//Inventory
		buttonInventoryTexture = Resources.Load("awardsInventory") as Texture2D;
		buttonInventoryTexture2x = Resources.Load("awardsInventory@2x") as Texture2D;
		buttonInventoryStyle.normal.background = scale.x > 1 ? buttonInventoryTexture2x : buttonInventoryTexture;
		
		Vector2 buttonDimension = new Vector2(buttonInventoryTexture.width,buttonInventoryTexture.height);
		InventoryRect = DynamicGUI.DynamicRect(new Vector2(0.96f,0.04f), buttonDimension, scale, 0,0,0,0);
		
	}
	
	void OnGUI(){
		GUI.depth = 10;
		//Inventory
		if(GUI.Button(InventoryRect,emptyPlaceHolder, buttonInventoryStyle)){
			//Show inventory	
			Debug.Log("Show inventory");
		}
		if(achievementBadgeTextureList != null && gameManager != null){
			//Show last achievement until the achievemnt badge is in inventory
			if(!Achievement.Show && gameManager.Achievement > 0){
//				Debug.Log("Badge is not shown");
				GUI.DrawTexture(BadgeRect,achievementBadgeTextureList[gameManager.Achievement-1]);
				
			}
			if(Achievement.Show && gameManager.Achievement > 1){
//				Debug.Log("Badge is shown so previous achievement is displayed");
				GUI.DrawTexture(BadgeRect,achievementBadgeTextureList[gameManager.Achievement-2]);
				
			}
		}
	}
}
