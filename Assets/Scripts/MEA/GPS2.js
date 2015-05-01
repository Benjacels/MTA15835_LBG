@script ExecuteInEditMode()

private var native_width : float = 1080;
private var native_height : float = 1920;

//gps location variables
var status : String;
var textGUIStyle : GUIStyle;
var locationLatitude : float;
var locationLongtitude : float;
var locationAltitude : float;
var locationHorizontalAccuracy : float;
var locationTimestamp : float;

//button variables
var buttonText : String[];
var boxStartLocation : Vector2;
var buttonSize : Vector2;
var myStyle : GUIStyle;

//zoneVariable
//var zoneLatitude : String;
//var zoneLongtitude : String;
var zoneText : String;

function Start () {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            return;
        // Start service before querying location
        Input.location.Start (1,1);
        // Wait until service initializes
        var maxWait : int = 20;
        while (Input.location.status
               == LocationServiceStatus.Initializing && maxWait > 0) {
            yield WaitForSeconds (1);
            maxWait--;
        }
        // Service didn't initialize in 20 seconds
        if (maxWait < 1) {
            status = "Timed out";
            return;
        }
        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed) {
            status = "Unable to determine device location";
            return;
        }
        // Access granted and location value could be retrieved
        else {
        	status = "Connection established";
        }
    }
    
    function Update () {
    
    	locationLatitude = Input.location.lastData.latitude;
        locationLongtitude = Input.location.lastData.longitude;
        locationAltitude = Input.location.lastData.altitude;
        locationHorizontalAccuracy = Input.location.lastData.horizontalAccuracy;
        locationTimestamp = Input.location.lastData.timestamp;
        
        //zone at street art
        if 			((locationLatitude > 57.046690 && locationLatitude < 57.046970) && 
        	(locationLongtitude > 9.928334 && locationLongtitude < 9.929044)) 
        	
        	zoneText  = "You are at the street art";
        
        //zone at pink house	
        else if 	((locationLatitude > 57.046419 && locationLatitude < 57.046539) && 
        	(locationLongtitude > 9.922744  && locationLongtitude < 9.922835)) 
        
        	zoneText  = "You are at the pink house";
        
        //zone at squerrel lamp
        else if 	((locationLatitude > 57.046465 && locationLatitude < 57.046585) && 
        	(locationLongtitude > 9.922557 && locationLongtitude < 9.922692)) 
        	
        	zoneText  = "You are at the lamp";

        //zone at yellow houses	
        else if 	((locationLatitude > 57.046536 && locationLatitude < 57.046656) && 
        	(locationLongtitude > 9.922083 && locationLongtitude < 9.922203)) 
        	
        	zoneText  = "You are at the yellow houses";
        	
        //zone at intersection	
        else if 	((locationLatitude > 57.046555 && locationLatitude < 57.046675) && 
        	(locationLongtitude > 9.921983 && locationLongtitude < 9.922103)) 
        	
        	zoneText  = "you are at the intersection";
        	
        else
        	zoneText  = "You are not in any zone";
  	
    
    }

	function OnGUI () {
		
		var rx : float = Screen.width / native_width;
		var ry : float = Screen.height / native_height;
 		GUI.matrix = Matrix4x4.TRS (Vector3(0, 0, 0), Quaternion.identity, Vector3 (rx, ry, 1)); 
		
		// Make a text field that modifies stringToEdit.
		GUI.TextField (Rect (10, 10, 400, 40), status , 50, textGUIStyle);
		
		GUI.TextField (Rect (10, 175, 400, 20), 
		"Location Latitude: " + locationLatitude.ToString() + "\n" + 
		"Location Longtitude: " + locationLongtitude.ToString() + "\n" + 
		"Location Altitude: " + locationAltitude.ToString() + "\n" + 
		"Location Horizontal Accuracy: " + locationHorizontalAccuracy.ToString() + "\n" + 
		"Location Timestamp: " + locationTimestamp.ToString(), 500, textGUIStyle);
		
		for (var i : int = 0; i < 2; i++) {
			if (GUI.Button(Rect(boxStartLocation.x, boxStartLocation.y + (i*175), buttonSize.x, buttonSize.y), buttonText[i], myStyle)) 
			{
				gpsActive(i);
			}
		}
		
		GUI.TextField (Rect (10, 1075, 400, 40), "Status Zone: \n" + zoneText , 500, textGUIStyle);
	}
	
function gpsActive (choice : int) {

	if (choice == 0) {
		Input.location.Start (1,1);
		
	}
	if (choice == 1){
		Input.location.Stop ();
		
	}
	
}
	
	
	