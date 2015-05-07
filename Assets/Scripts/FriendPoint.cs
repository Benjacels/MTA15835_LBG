using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FriendPoint : MonoBehaviour
{
    public GameObject friendObject;
    private GameObject _friendObject;
    public Sprite[] friendSprites;

    public Transform upperBounds;
    public Transform lowerBounds;
    public GameObject newFriendObject;
    private GameObject _newFriend;
    private GameObject _pointCanvas;

    void OnEnable()
    {
        if (RiddleManager.instance != null)
            RiddleManager.instance.OnFriendEvent += IncreaseFriends;
    }

    private void OnDisable()
    {
        if (RiddleManager.instance != null)
            RiddleManager.instance.OnFriendEvent -= IncreaseFriends;
    }

	// Use this for initialization
	void Start () {
        if (MainManager.instance.CurrentState == MainManager.State.Riddles)
            _pointCanvas = GameObject.Find("PointCanvas");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void IncreaseFriends()
    {
        _newFriend = Instantiate(newFriendObject, transform.position, Quaternion.identity) as GameObject;
        _newFriend.transform.parent = _pointCanvas.transform;
        var friendPos = new Vector3(Random.Range(upperBounds.position.x, lowerBounds.position.x),
            Random.Range(upperBounds.position.y, lowerBounds.position.y), 0);
        _friendObject = Instantiate(friendObject, friendPos, Quaternion.identity) as GameObject;
        _friendObject.transform.parent = transform;
        _friendObject.GetComponent<Image>().sprite = friendSprites[Random.Range(0, friendSprites.Length - 1)];
    }
}
