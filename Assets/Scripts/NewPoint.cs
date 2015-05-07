using System.Net.Mime;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewPoint : MonoBehaviour
{
    public Sprite friendPointSprite;
    public Sprite fuelPointSprite;

	// Use this for initialization
	void Start ()
	{
        print(MainManager.instance.CurrentChoice);
	    if (MainManager.instance.CurrentChoice == MainManager.Choices.Friends)
            GetComponent<Image>().sprite = friendPointSprite;
        else if (MainManager.instance.CurrentChoice == MainManager.Choices.Fuel)
            GetComponent<Image>().sprite = fuelPointSprite;
	    transform.localScale = Vector3.zero;

	    LeanTween.move(gameObject, transform.position + (Vector3.up*700), 2f);
        LeanTween.scale(gameObject, Vector3.one, 1).setEase(LeanTweenType.easeOutBounce).setOnComplete(DestroyPoint);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void DestroyPoint()
    {
        Destroy(gameObject);
    }
}
