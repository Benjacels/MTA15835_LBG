using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class SwapSprites : MonoBehaviour {
	
//	private Tween2D tween;
	
	private GameObject[] monsterObj;
	private List<SpriteRenderer> monsterSpriteRenderers;
	
	private GameObject[] monsterObj2;
	private List<SpriteRenderer> monsterArtSpriteRenderers;
	
	private Color opaque = new Color(1,1,1,1);
	private Color transparent = new Color(0,0,0,0);
	
//	private float alpha = 1;

	string input;
	
	void Awake() 
	{
//		tween = GetComponent<Tween2D>();
		
		// Find and sort objects with tag by name i.e. because Unity does not order objects in any specific order, 
		// and the order can thus change in different scenes and builds.
		monsterObj = Extensions.FindAndSortObjsWithTag("MonsterBlack");
		monsterObj2 = Extensions.FindAndSortObjsWithTag("MonsterOfArt");
		
		// Instantiate lists with size of object array found with tag
		monsterSpriteRenderers = new List<SpriteRenderer>(monsterObj.Length);
		monsterArtSpriteRenderers = new List<SpriteRenderer>(monsterObj2.Length);
		
		// Cash sprite renderers and sprites
		for(int i = 0; i < monsterObj.Length; i++)
		{
			monsterSpriteRenderers.Add(monsterObj[i].GetComponent<SpriteRenderer>());
			monsterArtSpriteRenderers.Add(monsterObj2[i].GetComponent<SpriteRenderer>());
		}

		InitializeArtSprites();
	}

	public void SwapSprite()
	{
			foreach(string bodyPart in GameManager.Instance.FoundBodyPart)
			{
//				Debug.Log("BODY PARTS: " + bodyPart);
				switch(bodyPart)
				{
				case "Arm_Lower_Left":
//					Debug.Log("Swap " + monsterObj[0].name);
					monsterArtSpriteRenderers[0].color = transparent;
					monsterSpriteRenderers[0].color = opaque;
					break;
				case "Arm_Lower_Right":
//					Debug.Log("Swap " + monsterObj[1].name);
					monsterArtSpriteRenderers[1].color = transparent;
					monsterSpriteRenderers[1].color = opaque;
					break;
				case "Arm_Upper_Left":
//					Debug.Log("Swap " + monsterObj[2].name);
					monsterArtSpriteRenderers[2].color = transparent;
					monsterSpriteRenderers[2].color = opaque;
					break;
				case "Arm_Upper_Right":
//					Debug.Log("Swap " + monsterObj[3].name);
					monsterArtSpriteRenderers[3].color = transparent;
					monsterSpriteRenderers[3].color = opaque;
					break;
				case "Body":
//					Debug.Log("Swap " + monsterObj[4].name);
					monsterArtSpriteRenderers[4].color = transparent;
					monsterSpriteRenderers[4].color = opaque;
					break;
				case "Ear_Left":
//					Debug.Log("Swap " + monsterObj[5].name);
					monsterArtSpriteRenderers[5].color = transparent;
					monsterSpriteRenderers[5].color = opaque;
					break;
				case "Ear_Right":
//					Debug.Log("Swap " + monsterObj[6].name);
					monsterArtSpriteRenderers[6].color = transparent;
					monsterSpriteRenderers[6].color = opaque;
					break;
				}
			}
	}

//	public void SwapSprite()
//	{
//		//if trackable has been found and the trackable has not already been found before
//		//and the trackable is amoung the artworks to be found and achievement has been incremented
//		
//		// if currentBodyPartFound != prevBodyPartFound
//		if(GameManager.Instance.CurrentBodyPartFound != GameManager.Instance.PrevBodyPartFound)
//		{
//			switch(GameManager.Instance.CurrentBodyPartFound)
//			{
//			case "Arm_Lower_Left":
//				Debug.Log("Swap " + monsterObj[0].name);
//				monsterArtSpriteRenderers[0].color = transparent;
//				monsterSpriteRenderers[0].color = opaque;
//				break;
//			case "Arm_Lower_Right":
//				Debug.Log("Swap " + monsterObj[1].name);
//				monsterArtSpriteRenderers[1].color = transparent;
//				monsterSpriteRenderers[1].color = opaque;
//				break;
//			case "Arm_Upper_Left":
//				Debug.Log("Swap " + monsterObj[2].name);
//				monsterArtSpriteRenderers[2].color = transparent;
//				monsterSpriteRenderers[2].color = opaque;
//				break;
//			case "Arm_Upper_Right":
//				Debug.Log("Swap " + monsterObj[3].name);
//				monsterArtSpriteRenderers[3].color = transparent;
//				monsterSpriteRenderers[3].color = opaque;
//				break;
//			case "Body":
//				Debug.Log("Swap " + monsterObj[4].name);
//				monsterArtSpriteRenderers[4].color = transparent;
//				monsterSpriteRenderers[4].color = opaque;
//				break;
//			case "Ear_Left":
//				Debug.Log("Swap " + monsterObj[5].name);
//				monsterArtSpriteRenderers[5].color = transparent;
//				monsterSpriteRenderers[5].color = opaque;
//				break;
//			case "Ear_Right":
//				Debug.Log("Swap " + monsterObj[6].name);
//				monsterArtSpriteRenderers[6].color = transparent;
//				monsterSpriteRenderers[6].color = opaque;
//				break;
//			}
//		}
//	}
	
	public void InitializeArtSprites()
	{
		for(int i = 0; i < monsterObj.Length; i++)
		{
			monsterArtSpriteRenderers[i].color = opaque;
			monsterSpriteRenderers[i].color = transparent;
			//			monsterArtSpriteRenderers[i].enabled = true;
			//			monsterSpriteRenderers[i].enabled = false;
		}
	}
	
	public void InitializeBlackSprites()
	{
		for(int i = 0; i < monsterObj.Length; i++)
		{
			monsterArtSpriteRenderers[i].color = transparent;
			monsterSpriteRenderers[i].color = opaque;
			//			monsterArtSpriteRenderers[i].enabled = false;
			//			monsterSpriteRenderers[i].enabled = true;
		}
	}
	
}
