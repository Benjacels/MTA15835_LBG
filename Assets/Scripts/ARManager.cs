using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Vuforia;

public class ARManager : MonoBehaviour
{
    private static ARManager _instance;

    public static ARManager instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<ARManager>();
            return _instance;
        }
    }

    public float detectTime = 3;

	public GameObject infoScreen;
    
    private GameObject _augmentBack;
    private Vector3 _backStartPos;
    private GameObject _augmentPic;
    private UnityEngine.UI.Image colChangeImg;
    private UnityEngine.UI.Image alphaChangeImg;

    private GameObject _successScreen;

    private bool picFound = false;

	// Use this for initialization
	void Start ()
	{
	    if (!MainManager.instance.hasSeenBear)
	    {
            MainManager.instance.currentCanvas.transform.Find("Bear").active = true;
            MainManager.instance.currentCanvas.transform.Find("Kid").active = false;
	    }
        else if (MainManager.instance.hasSeenBear)
        {
            MainManager.instance.currentCanvas.transform.Find("Kid").active = true;
            MainManager.instance.currentCanvas.transform.Find("Bear").active = false;
        }
	    _augmentBack = GameObject.FindObjectOfType<Mask>().transform.GetChild(0).gameObject;
	    _augmentPic = MainManager.instance.currentCanvas.transform.Find("AugmentPic").gameObject;
        _successScreen = MainManager.instance.currentCanvas.transform.Find("SuccessScreen").gameObject;

		infoScreen.active = false;

	    _backStartPos = _augmentBack.transform.position;

        //LeanTween.move(_augmentBack, _augmentBack.transform.parent.position, detectTime);
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    public void OnTrackingFound()
    {
        LeanTween.cancel(_augmentBack);
        LeanTween.move(_augmentBack, _augmentBack.transform.parent.position, detectTime).setOnComplete(ChangeLogo);
    }

    public void OnTrackingLost()
    {
        LeanTween.cancel(_augmentBack);
        LeanTween.move(_augmentBack, _backStartPos, detectTime);
    }

    void ChangeLogo()
    {
        LeanTween.cancel(_augmentBack);

        picFound = true;

        colChangeImg = _augmentBack.GetComponent<UnityEngine.UI.Image>();
        LeanTween.value(_augmentBack, ChangeColor, colChangeImg.color, Color.white, 1);

        alphaChangeImg = _augmentPic.GetComponent<UnityEngine.UI.Image>();
        var noAlpha = new Color(alphaChangeImg.color.r, alphaChangeImg.color.b, alphaChangeImg.color.g, 0);
        LeanTween.value(_augmentPic, ChangeAlpha, alphaChangeImg.color, noAlpha, 1).setOnComplete(FadeSuccessScreen);
    }

    void FadeSuccessScreen()
    {
        LeanTween.cancel(_augmentPic);
        LeanTween.value(gameObject, ChangeMultiAlpha, 0, 255, 1).setOnComplete(ActivateSuccessScreen);
    }
    void ActivateSuccessScreen()
    {
        LeanTween.cancel(gameObject);


		if (MainManager.instance.prevState == MainManager.State.Start) {
						MainManager.instance.artsSeen.Add ("hjelmerstald");
						MainManager.instance.hasSeenBear = true;	
				} 
		else {
			MainManager.instance.artsSeen.Add("pyramide");
				}
			
        /*switch (MainManager.instance.prevState)
        {
            case MainManager.State.Start:
                MainManager.instance.artsSeen.Add("hjelmerstald");
                MainManager.instance.hasSeenBear = true;
                break;
            case MainManager.State.KidDialogue:
                MainManager.instance.artsSeen.Add("pyramide");
                break;
        }*/

        var imgT = GameObject.FindObjectsOfType<ImageTargetBehaviour>();
        foreach (ImageTargetBehaviour i in imgT)
            Destroy(i.gameObject);

        foreach (Transform tran in _successScreen.transform)
        {
            if (tran.GetComponent<Button>())
                tran.GetComponent<Button>().enabled = true;
        }

		//infoScreen.active = true;
    }

    void ChangeColor(Color colValue)
    {
        colChangeImg.color = colValue;
    }

    void ChangeAlpha(Color alphaValue)
    {
        alphaChangeImg.color = alphaValue;
    }

    void ChangeMultiAlpha(float alphaValue)
    {
        /*var img = _successScreen.transform.Find("SuccessBackground").GetComponent<UnityEngine.UI.Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, alphaValue);*/

        foreach (Transform tran in _successScreen.transform)
        {
            if (tran.GetComponent<UnityEngine.UI.Image>())
            {
                var img = tran.GetComponent<UnityEngine.UI.Image>();
                img.color = new Color(img.color.r, img.color.g, img.color.b, alphaValue);
            }
            else if (tran.GetComponent<UnityEngine.UI.Text>())
            {
                var txt = tran.GetComponent<UnityEngine.UI.Text>();
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alphaValue);
            }
        }
    }

    
}
