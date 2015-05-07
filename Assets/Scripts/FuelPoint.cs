using UnityEngine;
using System.Collections;

public class FuelPoint : MonoBehaviour
{
    public GameObject fuel;
    public GameObject newFuelObject;
    private GameObject _newFuel;
    private GameObject _pointCanvas;

    void OnEnable()
    {
        if (RiddleManager.instance != null)
            RiddleManager.instance.OnFuelEvent += IncreaseFuel;
    }

    private void OnDisable()
    {
        if (RiddleManager.instance != null)
            RiddleManager.instance.OnFuelEvent -= IncreaseFuel;
    }


    // Use this for initialization
	void Start ()
	{
	    if (MainManager.instance.CurrentState == MainManager.State.Riddles)
	        _pointCanvas = GameObject.Find("PointCanvas");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void IncreaseFuel()
    {
        _newFuel = Instantiate(newFuelObject, transform.position, Quaternion.identity) as GameObject;
        _newFuel.transform.parent = _pointCanvas.transform;
        LeanTween.move(fuel, fuel.transform.position + (Vector3.up*2), 1f);
    }

}
