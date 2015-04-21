using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Achievement : MonoBehaviour {

	private GameManager gameManager;
	private string emptyPlaceHolder = string.Empty;
	
	//Achievement
	private Rect achievementBadgeRect;
	private List <Texture2D> achievementBadgeTextureList;
	private GUIStyle achievementBadgeStyle  = new GUIStyle();
	private Vector2 scale;

	//Movement
	private Vector2 badgeTargetDimension;
	private Vector2 buttonPosition = new Vector2(0.5f,0.5f);
	private Vector2 badgeStartDimension = Vector2.zero;

	[Range (0.0f, 10.0f)]
	public float scaleDuration;

	[Range (0.0f, 10.0f)]
	public float moveDuration;

	private Tween2D tween;
	private Inventory inventory;
//	private SwapSprites swapSprites;
	public static bool Show;
	private float alpha = 1;

	
	private void Awake()
	{	
		gameManager = GameManager.Instance;
		// *********** For testing purposes the game is initialized if playing the scenee directly
//		if(gameManager.Level == 0)
//			gameManager.Initialize();

		tween = GetComponent<Tween2D>();
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
//		swapSprites = GameObject.Find("MonsterOfArt").GetComponent<SwapSprites>();

		scale = DynamicGUI.DynamicScale();
		
		//Achievement badge
		achievementBadgeTextureList = new List<Texture2D>();
		
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadge1@2x") as Texture2D : Resources.Load("achievementBadge1") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadge2@2x") as Texture2D : Resources.Load("achievementBadge2") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadge3@2x") as Texture2D : Resources.Load("achievementBadge3") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadge4@2x") as Texture2D : Resources.Load("achievementBadge4") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadge5@2x") as Texture2D : Resources.Load("achievementBadge5") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadge6@2x") as Texture2D : Resources.Load("achievementBadge6") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadge7@2x") as Texture2D : Resources.Load("achievementBadge7") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadge8@2x") as Texture2D : Resources.Load("achievementBadge8") as Texture2D);
		achievementBadgeTextureList.Add(scale.x > 1 ? Resources.Load("achievementBadge9@2x") as Texture2D : Resources.Load("achievementBadge9") as Texture2D);

		badgeTargetDimension = scale.x > 1 ? new Vector2(achievementBadgeTextureList[0].width,achievementBadgeTextureList[0].height)/2 : new Vector2(achievementBadgeTextureList[0].width,achievementBadgeTextureList[0].height);

		achievementBadgeRect = DynamicGUI.DynamicRect(buttonPosition, badgeStartDimension, scale, 0,0,0,0);

		Show = false;

	}

	void OnGUI()
	{
		GUI.depth = 0;

		DisplayAchievement();
	
	}

	void DisplayAchievement()
	{
		//if trackable has been found and the trackable has not already been found before
		//and the trackable is amoung the artworks to be found
		//then
		//flag that the current artwork/trackable has been found
		//increment achievement
		//start animation of badge award
		if (gameManager.TrackableFound && !gameManager.trackableIDList.Contains(gameManager.TrackableID) && gameManager.artworksToBeFound.Exists(x => x.ArtworkName == gameManager.TrackableName))
		{
//			GameManager.Instance.CurrentGameState = GameManager.GameState.ArtworkFound;
//			Debug.Log(" ############ GameState: " + GameManager.Instance.CurrentGameState);
			// If trackable found add trackable ID to list of trackable IDs
			gameManager.trackableIDList.Add(gameManager.TrackableID);

			// If trackable name was found in the primary artworks list
			// find bodypart associated with the artwork name of the class object in the list
			foreach(ArtworkToBeFound obj in gameManager.artworksToBeFound)
			{
				if(obj.ArtworkName == gameManager.TrackableName)
				{
					gameManager.FoundBodyPart.Add(obj.BodyPart);
				}
				
			}

			if(gameManager.Achievement < 9)
				gameManager.Achievement += 1;
			
			Debug.Log("------- Achievement: " + gameManager.Achievement);
			
			StartCoroutine(NewAnim());
			
			Debug.Log("------- Show badge: " + Show);
		}
		
		GUI.color = new Color(1,1,1,alpha);
		
		//Achievement
		if(achievementBadgeTextureList != null && gameManager != null)
		{
			if(Show && gameManager.Achievement > 0)
			{
				GUI.DrawTexture(achievementBadgeRect,achievementBadgeTextureList[gameManager.Achievement-1]);
				achievementBadgeRect = DynamicGUI.DynamicRect(tween.Position, tween.Dimension, scale, 0,0,0,0);
			}
		}
		
	}

//	void DisplayAchievement()
//	{
//		//if trackable has been found and the trackable has not already been found before
//		//and the trackable is amoung the artworks to be found
//		//then
//		//flag that the current artwork/trackable has been found
//		//increment achievement
//		//start animation of badge award
//		if (gameManager.TrackableFound && !gameManager.trackableIDList.Contains(gameManager.TrackableID) && gameManager.artworksToBeFound.Contains(gameManager.TrackableName))
//		{
//			//if trackable found add trackable ID to list of trackable IDs
//			gameManager.trackableIDList.Add(gameManager.TrackableID);
//			
//			if(gameManager.Achievement < 9)
//				gameManager.Achievement += 1;
//			
//			Debug.Log("------- Achievement: " + gameManager.Achievement);
//
//			StartCoroutine(NewAnim());
//			
//			Debug.Log("------- Show badge: " + Show);
//		}
//
//		GUI.color = new Color(1,1,1,alpha);
//		
//		//Achievement
//		if(achievementBadgeTextureList != null && gameManager != null)
//		{
//			if(Show && gameManager.Achievement > 0)
//			{
//				GUI.DrawTexture(achievementBadgeRect,achievementBadgeTextureList[gameManager.Achievement-1]);
//				achievementBadgeRect = DynamicGUI.DynamicRect(tween.Position, tween.Dimension, scale, 0,0,0,0);
//			}
//		}
//
//	}

	IEnumerator NewAnim()
	{

		Show = true;

		if(gameManager.InitializingCamera)
		{
			yield return new WaitForSeconds(2f);
			Debug.Log("<color=purple>------- Waited before Animating</color>");
		}
		else
		{
			yield return null;
			Debug.Log("<color=purple>------- Did not wait before Animating</color>");
		}

		Debug.Log("------- Scale Up Badge");

//		if(CamManager.Instance.ARCameraState && QCARRenderer.Instance.DrawVideoBackground)
//		{

			//initial pos
			tween.Position = buttonPosition;

			yield return StartCoroutine (tween.Scale (badgeStartDimension, badgeTargetDimension, scaleDuration, Tween2D.EasingMethod.Berp));
			yield return new WaitForSeconds(0.5f);

			Debug.Log("------- Move and Scale Down Badge");
			StartCoroutine (tween.Scale (badgeTargetDimension, inventory.BadgeTextureDimension, moveDuration, Tween2D.EasingMethod.Coserp));
			//Could not use inventory.BadgePosition. Probably due the anchor point by which it is scaled or due to some floating point precision issues
			yield return StartCoroutine (tween.Move (buttonPosition, new Vector2(0.96f,0.04f),moveDuration, Tween2D.EasingMethod.Hermite));
			
			//if not in AR scene
			Show = false;
			Debug.Log("------- Show badge: " + Show);
			inventory.Show = true;
//		}
	}

	void AchievementButton()
	{
		if(achievementBadgeTextureList != null && gameManager != null){
			if(Show && gameManager.Achievement > 0){

				achievementBadgeStyle.normal.background = achievementBadgeTextureList[gameManager.Achievement-1];

				if(GUI.Button(achievementBadgeRect,emptyPlaceHolder, achievementBadgeStyle)){
					//putting the badge award into inventory could be trickered by touch i.e.
					//if button (badge) pressed then start coroutine animation
				}
			}
		}
	}
}
