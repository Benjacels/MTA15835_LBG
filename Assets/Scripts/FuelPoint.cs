using UnityEngine;
using System.Collections;

public class FuelPoint : MonoBehaviour
{
    private GameObject fuel;
    public GameObject noFuel;
    public GameObject halfFuel;
    public GameObject fullFuel;

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
	    {
            _pointCanvas = GameObject.Find("PointCanvas");
	        if (MainManager.instance.FuelPoints > 0)
	        {
	            halfFuel.active = true;
	            fuel = halfFuel;
	        }
	        else
	        {
	            noFuel.active = true;
                fuel = noFuel;
	        }
	    }

	    if (MainManager.instance.CurrentState == MainManager.State.End)
	    {
	        if (MainManager.instance.FuelPoints > 0)
	            fullFuel.active = true;
	        else
                fullFuel.active = false;
	    }

        //if (fuel.transform.position != Vector3.zero)
            //fuel.transform.position = MainManager.instance.fuelMeterPos;
	}

    void OnLevelWasLoaded()
    {
        //if (MainManager.instance.CurrentState != MainManager.State.Riddles || MainManager.instance.CurrentState != MainManager.State.End)
            //gameObject.active = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void IncreaseFuel()
    {
        _newFuel = Instantiate(newFuelObject, transform.position, Quaternion.identity) as GameObject;
        //_newFuel.transform.parent = _pointCanvas.transform;
        _newFuel.transform.SetParent(_pointCanvas.transform, false);
        _newFuel.transform.position = transform.position;
        LeanTween.move(fuel, fuel.transform.position + (Vector3.up), 1f).setOnComplete(SavePos);
    }

    void SavePos()
    {
        MainManager.instance.fuelMeterPos = fuel.transform.position;
    }
}
