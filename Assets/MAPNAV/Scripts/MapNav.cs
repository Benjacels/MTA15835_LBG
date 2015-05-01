//MAPNAV Navigation ToolKit v.1.3.4
//Attention: This script uses a custom editor inspector: MAPNAV/Editor/MapNavInspector.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[AddComponentMenu("MAPNAV/MapNav")]

public class MapNav : MonoBehaviour 
{
	public Transform user;									 	//User(Player) transform
	public bool simGPS = true;								 	//True when the GPS Emulator is enabled
	public float userSpeed = 5.0f;							 	//User speed when using the GPS Emulator (keyboard input)
	public bool realSpeed = false;								//If true, the perceived player speed depends on zoom level(realistic behaviour)
	public float fixLat = 42.3627f;					   			//Latitude
	public float fixLon = -71.05686f;							//Longitude
	public float altitude;										//Current GPS altitude
	public float heading;										//Last compass sensor reading (Emulator disabled) or user's eulerAngles.y (Emulator enabled)
	public float accuracy;										//GPS location accuracy (error)
	public int maxZoom = 18;									//Maximum zoom level available. Set according to your maps provider
	public int minZoom = 1;										//Minimum zoom level available
	public int zoom = 17;										//Current zoom level
	private float multiplier; 									//1 for a size=640x640 tile, 2 for size=1280*1280 tile, etc. Automatically set when selecting tile size
	public string key = "Fmjtd%7Cluur29072d%2Cbg%3Do5-908s00";  //AppKey (API key) code obtained from your maps provider (MapQuest, Google, etc.). 
																//Default MapQuest key for demo purposes only (with limitations). Please get your own key before you start yout project.															 

	public float markerStartLat = 57.046350f;					//startMarker latitude position 
	public float markerStartLon = 9.922898f;					//startMarker longitude position 
	public float markerMiddleLat = 57.046788f;					//middleMarker latitude position 
	public float markerMiddleLon = 9.92833f;					//middleMarker longitude position 
	public float markerEndLat = 57.04520f;						//endMarker latitude position 
	public float markerEndLon = 9.922497f;						//endMarker longitude position 

	public List<string> routePointsThere = new List<string>(); 		//Where the values from the text file will be stored 
	public List<string> routePointsBack = new List<string>(); 		//Where the values from the text file will be stored

	public string pathThere;
	public string pathBack;

	//Route points there 
	public string point1 = "57.04635,9.922898";
	public string point1a = "57.046424,9.922914";
	public string point2 = "57.046615,9.922038";
	public string point3 = "57.046120,9.921604";
	public string point4 = "57.045819,9.922283";
	public string point5 = "57.045634,9.923061";
	public string point6 = "57.045693,9.923101";
	public string point7 = "57.045648,9.923938";
	public string point8 = "57.045851,9.924069";
	public string point9 = "57.045692,9.925035";
	public string point10 = "57.046191,9.925448";
	public string point11 = "57.045736,9.927765";
	public string point12 = "57.046486,9.928275";
	public string point13 = "57.046788,9.928326";

	//Route points back  
	public string point14 = "57.046925,9.928268";
	public string point15 = "57.047065,9.928236";
	public string point16 = "57.046481,9.929534";
	public string point17 = "57.046382,9.929427";
	public string point18 = "57.046621,9.925135";
	public string point19 = "57.046290,9.925295";
	public string point20 = "57.046449,9.924533";
	public string point21 = "57.046225,9.924334";
	public string point22 = "57.046231,9.923524";
	public string point23 = "57.045146,9.922865";
	public string point24 = "57.045131,9.922395";
	public string point25 = "57.045207,9.922497";

	public string[] routeThere;
	public string[] routeBack;
	public string pathStringThere;
	public string pathStringBack;

	public string[] maptype;									//Array including available map types
	public int[] mapSize;										//Array including available map sizes(pixels)
	public int index;											//maptype array index. 
	public int indexSize;										//mapsize array index. 
	public float camDist = 15.0f;								//Camera distance(3D) or height(2D) to user
	public int camAngle = 40;									//Camera angle from horizontal plane
	public int initTime = 3;									//Hold time after a successful GPS fix in order to improve location accuracy
	public int maxWait = 30;									//GPS fix timeout
	public bool buttons = true;								 	//Enables GUI sample control buttons 
	public string dmsLat;									 	//Latitude as degrees, minutes and seconds
	public string dmsLon;							 		 	//Longitude as degrees, minutes and seconds
	public float updateRate = 0.1f;								//User's position update rate
	public bool autoCenter = true;							 	//Autocenter and refresh map
	public string status;								     	//GPS and other status messages
	public bool gpsFix;								     		//True after a successful GPS fix 
	public Vector3 iniRef;							         	//First location data retrieved on Start	 
	public bool info;									     	//Used by GPS_Status.cs to enable/disable the GPS information window.
	public bool triDView = false;						     	//2D/3D modes toggle
	public bool ready;								     		//true when the map texture has been successfully loaded
	public bool freeCam = false;							 	//when true, MainCamera follows and looks at Player (3D mode only)
	public bool pinchToZoom = true;							 	//Enables Pinch to Zoom interaction on mobile devices
	public bool dragToPan = true;							 	//Enables Drag to Pan interaction on mobile devices
	public bool mapDisabled;								 	//Disables online maps
	public bool mapping = false;							 	//true while map is being downloaded
	public Transform cam;									 	//Reference to the Main Camera transform
	public float userLon;									 	//Current user position longitude
	public float userLat;									 	//Current user position latitude
	
	private float levelHeight;
	private float smooth = 1.3f;	 						    
	private float yVelocity = 0.0f;  
	private float speed;
	private Camera mycam;
	private float currentOrtoSize;
	private LocationInfo loc;
	private Vector3 currentPosition;
	private Vector3 newUserPos; 
	private Vector3 currentUserPos;
	private float download;
	private WWW www;
	private string url = ""; 
	private double longitude;
	private double latitude;
	private Rect rect;
	private float screenX;
	private float screenY;
	private Renderer maprender;
	private Transform mymap;
	private float initPointerSize;
	private double tempLat;
	private double tempLon;
	private bool touchZoom;
	private string centre;
	private bool centering;
	private Texture centerIcon;
	private Texture topIcon;
	private Texture bottomIcon;
	private Texture leftIcon;
	private Texture rightIcon;
	private GUIStyle arrowIcon;
	private float dot;
	private bool centered = true;
	private int borderTile = 0;
	private bool tileLeft;
	private bool tileRight;
	private bool tileTop;
	private bool tileBottom;
	private Rect topCursorPos;
	private Rect rightCursorPos;
	private Rect bottomCursorPos;
	private Rect leftCursorPos;

	//Touch Screen Variables
	private Vector2 prevDist;
	private float actualDist;
	private Transform target;
	private Touch touch;
	private Touch touch2;
	private Vector2 curDist;
	private float dragSpeed;
	private Rect viewArea;
	private float targetOrtoSize;
	private bool firstTime = true;
	private Vector2 focusScreenPoint;
	private Vector3 focusWorldPoint; 


	void Awake(){
		pathThere = Application.dataPath + "/Raw/routePointsThere.txt";
		pathBack = Application.dataPath + "/Raw/routePointsBack.txt";

		//routeThere = new string[]{point1, point2, point3, point4, point5, point6, point7, point8, 
		//	point9, point10, point11, point12, point13};

		//routeBack = new string[]{point13, point14, point15, point16, point17, point18, point19, 
		//	point20, point21, point22, point23, point24, point25};

		//pathStringThere = string.Join("%7C",routeThere);
		//pathStringBack = string.Join("%7C",routeBack);
	
		//Set the map's tag to GameController
		transform.tag = "GameController";
		
		//References to the Main Camera and Player. 
		//Please make sure your camera is tagged as "MainCamera" and your user visualization/character as "Player"
		cam = Camera.main.transform;
		mycam = Camera.main;
		user = GameObject.FindGameObjectWithTag("Player").transform;
		
		//Store most used components and values into variables for faster access.
		mymap = transform;
		maprender = renderer;
		screenX = Screen.width;
		screenY = Screen.height;	
		/*
		//Add possible values to maptype and mapsize arrays (MAPQUEST)
		maptype = new string[]{"map", "sat", "hyb"};
		mapSize = new int[]{640, 1280, 1920, 2560}; //in pixels
		*/

		//Add possible values to maptype and mapsize arrays (GOOGLE)
		maptype = new string[]{"satellite","roadmap","hybrid","terrain"};
		mapSize = new int[]{1024, 768}; //in pixels

		//Set GUI "center" button label
		if(triDView){
			centre = "refresh";
		}
		//Enable autocenter on 2D-view (default)
		else{
			autoCenter = true;
		}
		
		//Load required interface textures
		centerIcon = Resources.Load("centerIcon") as Texture2D;
		topIcon = Resources.Load("cursorTop") as Texture2D;
		bottomIcon = Resources.Load("cursorBottom") as Texture2D;
		leftIcon = Resources.Load("cursorLeft") as Texture2D;
		rightIcon = Resources.Load("cursorRight") as Texture2D;
		
		//Resize GUI according to screen size/orientation 
		if(screenY >= screenX){
			dot = screenY/800.0f;
		}else{
			dot = screenX/800.0f;
		}
	}


	IEnumerator Start () {

		//Setting variables values on Start
		gpsFix=false;
		rect = new Rect (screenX/10, screenY/10, 8*screenX/10, 8*screenY/10);
		topCursorPos = new Rect(screenX/2-25*dot, 0, 50*dot, 50*dot);
		rightCursorPos = new Rect(screenX-50*dot, screenY/2-25*dot, 50*dot, 50*dot);
		if(!buttons)
			bottomCursorPos = new Rect(screenX/2-25*dot, screenY-50*dot, 50*dot, 50*dot);
		else
			bottomCursorPos = new Rect(screenX/2-25*dot, screenY-50*dot-screenY/12, 50*dot, 50*dot);
		leftCursorPos = new Rect(0, screenY/2-25*dot, 50*dot, 50*dot);
		Vector3 tmp = mymap.eulerAngles;
		tmp.y=180;
		mymap.eulerAngles = tmp;
		initPointerSize = user.localScale.x;
		user.position = new Vector3(0, user.position.y, 0);
		
		//Initial Camera Settings
		//3D 
		if(triDView){
			mycam.orthographic = false;
			pinchToZoom = false;
			dragToPan = false;
			//Set the camera's field of view according to Screen size so map's visible area is maximized.
			if(screenY > screenX){
				mycam.fieldOfView = 72.5f;
			}else{
				mycam.fieldOfView = 95-(28*(screenX/screenY));
			}
		}
		//2D
		else{
			mycam.orthographic = true;
			mycam.nearClipPlane = 0.1f;
			mycam.farClipPlane = cam.position.y+1;	
			if(screenY >= screenX){
				mycam.orthographicSize = mymap.localScale.z*5.0f;
			}else{
				mycam.orthographicSize = (screenY/screenX)*mymap.localScale.x*5.0f;		
			}
		}
		
		//The "ready" variable will be true when the map texture has been successfully loaded.
		ready = false; 
		
		//STARTING LOCATION SERVICES
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser){
			//This message prints to the Editor Console
			print("Please enable location services and restart the App");
			//You can use this "status" variable to show messages in your custom user interface (GUIText, etc.)
			status = "Please enable location services\n and restart the App";
			yield return new WaitForSeconds(4);
			Application.Quit();
		}

		// Start service before querying location
		Input.location.Start (3.0f, 3.0f); 
		Input.compass.enabled = true;
		print("Initializing Location Services..");
		status = "Initializing Location Services..";

		// Wait until service initializes
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds (1);
			maxWait--;
		}

		// Service didn't initialize in 30 seconds
		if (maxWait < 1) {
			print("Unable to initialize location services.\nPlease check your location settings and restart the App");
			status = "Unable to initialize location services.\nPlease check your location settings\n and restart the App";
			yield return new WaitForSeconds(4);
			Application.Quit();
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			print("Unable to determine your location.\nPlease check your location setting and restart this App");
			status = "Unable to determine your location.\nPlease check your location settings\n and restart this App";
			yield return new WaitForSeconds(4);
			Application.Quit();
		}
		
		// Access granted and location value could be retrieved
		else {
			if(!mapDisabled){
				print("GPS Fix established. Setting position..");
				status = "GPS Fix established!\n Setting position ...";
			}
			else{
				print("GPS Fix established.");
				status = "GPS Fix established!";
			}
					
			if(!simGPS){
				//Wait in order to find enough satellites and increase GPS accuracy
				yield return new WaitForSeconds(initTime);
				//Set position
				loc  = Input.location.lastData;          
				iniRef.x = ((loc.longitude*20037508.34f/180)/100);
				iniRef.z = (float)(System.Math.Log(System.Math.Tan((90+loc.latitude)*System.Math.PI/360))/(System.Math.PI/180));
				iniRef.z = ((iniRef.z*20037508.34f/180)/100);  
				iniRef.y = 0;
				fixLon = loc.longitude;
				fixLat = loc.latitude; 
				//Successful GPS fix
				gpsFix = true;
				//Update Map for the current location
				 StartCoroutine(MapPosition());
			}  
			else{
				//Simulate initialization time
				yield return new WaitForSeconds(initTime);
				//Set Position
				iniRef.x = ((fixLon*20037508.34f/180)/100);
				iniRef.z = (float)(System.Math.Log(System.Math.Tan((90+fixLat)*System.Math.PI/360))/(System.Math.PI/180));
				iniRef.z = (iniRef.z*20037508.34f/180)/100;  
				iniRef.y = 0;
				//Simulated successful GPS fix
				gpsFix = true;
				//Update Map for the current location
				StartCoroutine(MapPosition());
			}    
		}

		//Rescale map, set new camera height, and resize user pointer according to new zoom level
		 StartCoroutine(ReScale()); 

		//Set player's position using new location data (every "updateRate" seconds)
		//Default value for updateRate is 0.1. Increase if necessary to improve performance
		InvokeRepeating("MyPosition", 1, updateRate); 

		//Read incoming compass data (every 0.1s)
		InvokeRepeating("Orientate", 1, 0.1f);
		
		//Get altitude and horizontal accuracy readings using new location data (Default: every 2s)
		InvokeRepeating("AccuracyAltitude", 1, 2);
		
		//Auto-Center Map on 2D View Mode 
		InvokeRepeating("Check", 1, 0.2f);
	}


	void MyPosition(){
		if(gpsFix){
			if(!simGPS){
				loc = Input.location.lastData;
				newUserPos.x = ((loc.longitude*20037508.34f/180)/100)-iniRef.x;
				newUserPos.z = (float)(System.Math.Log(System.Math.Tan((90+loc.latitude)*System.Math.PI/360))/(System.Math.PI/180));
				newUserPos.z = ((newUserPos.z*20037508.34f/180)/100)-iniRef.z;   
				dmsLat = convertdmsLat(loc.latitude);
				dmsLon = convertdmsLon(loc.longitude);
				userLon = loc.longitude;
				userLat = loc.latitude;
			}
			else{
				userLon = (18000*(user.position.x+iniRef.x))/20037508.34f;
				userLat = ((360/Mathf.PI)*Mathf.Atan(Mathf.Exp(0.00001567855943f*(user.position.z+iniRef.z))))-90;
				dmsLat = convertdmsLat(userLat);
				dmsLon = convertdmsLon(userLon);
			}
		}	
	} 
	
	void Orientate(){
		if(!simGPS && gpsFix){
			heading = Input.compass.trueHeading;
		}
		else{
			heading = user.eulerAngles.y;
		}
	}
	 
	void AccuracyAltitude(){
		if(gpsFix)
			altitude = loc.altitude;
			accuracy = loc.horizontalAccuracy;
	}
	
	void Check(){
		if(autoCenter && triDView == false){
			if(ready == true && mapping == false && gpsFix){
				if (rect.Contains(Vector2.Scale(mycam.WorldToViewportPoint (user.position), new Vector2(screenX, screenY)))){
					//DoNothing
				}
				else{
					centering=true;
					 StartCoroutine(MapPosition());
					 StartCoroutine(ReScale());	
				}
			}
		}
	}

	//Auto-Center Map on 3D View Mode when exiting map's collider
	void OnTriggerExit(Collider other){
		if(other.tag == "Player" && autoCenter && triDView){
			 StartCoroutine(MapPosition());
			 StartCoroutine(ReScale());
		}
	}

	//Update Map with the corresponding map images for the current location ============================================
	IEnumerator MapPosition(){

		//The mapping variable will only be true while the map is being updated
		mapping = true;
		
		CursorsOff();
		
		//CHECK GPS STATUS AND RESTART IF NEEDED
		
		if (Input.location.status == LocationServiceStatus.Stopped || Input.location.status == LocationServiceStatus.Failed){
			// Start service before querying location
			Input.location.Start (3.0f, 3.0f);

			// Wait until service initializes
			int maxWait = 20;
			while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
				yield return new WaitForSeconds (1);
				maxWait--;
			}

			// Service didn't initialize in 20 seconds
			if (maxWait < 1) {
				print ("Timed out");
				//use the status string variable to print messages to your own user interface (GUIText, etc.)
				status = "Timed out";
			}

			// Connection has failed
			if (Input.location.status == LocationServiceStatus.Failed) {
				print ("Unable to determine device location");
				//use the status string variable to print messages to your own user interface (GUIText, etc.)
				status = "Unable to determine device location";
			}
		
		}
		
	   //------------------------------------------------------------------	//
	   
		www = null; 
		//Get last available location data
		loc = Input.location.lastData;
		//Make player invisible while updating map
		user.gameObject.renderer.enabled = false;
		
		
		//Set target latitude and longitude
		if(triDView){
			if(simGPS){
				fixLon = (18000*(user.position.x+iniRef.x))/20037508.34f;
				fixLat = ((360/Mathf.PI)*Mathf.Atan(Mathf.Exp(0.00001567855943f*(user.position.z+iniRef.z))))-90;	
			}else{
				fixLon = loc.longitude;
				fixLat = loc.latitude;
			}
		}else{
			if(centering){
				if(simGPS){
					fixLon = (18000*(user.position.x+iniRef.x))/20037508.34f;
					fixLat = ((360/Mathf.PI)*Mathf.Atan(Mathf.Exp(0.00001567855943f*(user.position.z+iniRef.z))))-90;	
				}else{
					fixLon = loc.longitude;
					fixLat = loc.latitude;
				}
			}
			else{
				if(borderTile == 0){
					fixLat = ((360/Mathf.PI)*Mathf.Atan(Mathf.Exp(0.00001567855943f*(cam.position.z+iniRef.z))))-90;	
					fixLon = (18000*(cam.position.x+iniRef.x))/20037508.34f;
				}
				//North tile
				if (borderTile == 1){
					fixLat = ((360/Mathf.PI)*Mathf.Atan(Mathf.Exp(0.00001567855943f*(cam.position.z+3*mycam.orthographicSize/2+iniRef.z))))-90;	
					fixLon = (18000 *(cam.position.x+iniRef.x))/20037508.34f;
					borderTile=0;	
					tileTop=false;
				}
				//East Tile
				if (borderTile == 2){
					fixLat = ((360/Mathf.PI)*Mathf.Atan(Mathf.Exp(0.00001567855943f*(cam.position.z+iniRef.z))))-90;	
					fixLon = (18000*(cam.position.x+3*(screenX*mycam.orthographicSize/screenY)/2+iniRef.x))/20037508.34f;
					borderTile = 0;
				}
				//South Tile
				if (borderTile == 3){
					fixLat = ((360/Mathf.PI)*Mathf.Atan(Mathf.Exp(0.00001567855943f*(cam.position.z-3*mycam.orthographicSize/2+iniRef.z))))-90;	
					fixLon = (18000*(cam.position.x+iniRef.x))/20037508.34f;
					borderTile=0;
				}
				//West Tile
				if (borderTile == 4){
					fixLat = ((360/Mathf.PI)*Mathf.Atan(Mathf.Exp(0.00001567855943f*(cam.position.z+iniRef.z))))-90;	
					fixLon = (18000*(cam.position.x-3*(screenX*mycam.orthographicSize/screenY)/2+iniRef.x))/20037508.34f;
					borderTile=0;
				}
			}
		}

		/*
		//MAPQUEST ==============================================================================
		//Build a valid MapQuest OpenMaps tile request for the current location
		multiplier=mapSize[indexSize]/640.0f; //Tile Size= 640*multiplier
		url="http://open.mapquestapi.com/staticmap/v4/getmap?key="+key+"&size="+mapSize[indexSize].ToString()+","
			+mapSize[indexSize].ToString()+"&zoom="+zoom+"&type="+maptype[index]+"¢er="+fixLat+","+fixLon+"&scalebar=false";
		tempLat = fixLat;
		tempLon = fixLon;
		*/ 

		//GOOGLE ================================================================================
		//Build a valid Google Maps tile request for the current location
		multiplier=1;
		ReadThere();
		ReadBack();

		pathStringThere = string.Join("%7C",routePointsThere.ToArray());	
		pathStringBack = string.Join("%7C",routePointsBack.ToArray());	


		url= "http://maps.google.com/maps/api/staticmap?center="+fixLat+","+fixLon+"&zoom="+zoom+"&scale=2&size=1024x768&format=jpg&maptype="+maptype[index]+"&markers=color:red%7Clabel:A%7C"+markerStartLat+","+markerStartLon+"&markers=color:blue%7Clabel:B%7C"+markerMiddleLat+","+markerMiddleLon+"&markers=color:yellow%7Clabel:C%7C"+markerEndLat+","+markerEndLon+"&path=color:0xff0000ff%7Cweight:2%7C"+pathStringThere+"&path=color:blue%7Cweight:2%7C"+pathStringBack+"&sensor=false&key="+key;
	
		tempLat = fixLat;
		tempLon = fixLon;

		//Debug.Log(url);

		//=================================================================================================


		//Proceed with download if a Wireless internet connection is available 
		if(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork){
			StartCoroutine(Online());
		}	
		//Proceed with download if a 3G/4G internet connection is available 
		else if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork){
			 StartCoroutine(Online());
		}
		//No internet connection is available. Switching to Offline mode.	 
		else{
			Offline();
		}	
	}

	//ONLINE MAP DOWNLOAD
	IEnumerator Online(){
		if(!mapDisabled){
			// Start a download of the given URL
			www = new WWW(url); 
			// Wait for download to complete
			download = (www.progress);
			while(!www.isDone){
				print("Updating map "+System.Math.Round(download*100)+" %");
				//use the status string variable to print messages to your own user interface (GUIText, etc.)
				status="Updating map "+System.Math.Round(download*100)+" %";
				yield return null;
			}
			//Show download progress and apply texture
			if(www.error == null){
				print("Updating map 100 %");
				print("Map Ready!");
				//use the status string variable to print messages to your own user interface (GUIText, etc.)
				status = "Updating map 100 %\nMap Ready!";
				yield return new WaitForSeconds (0.5f);
				maprender.material.mainTexture = null;
				Texture2D tmp;
				tmp = new Texture2D(1280, 1280, TextureFormat.RGB24, false);
				maprender.material.mainTexture = tmp;
				www.LoadImageIntoTexture(tmp); 	
			}
			//Download Error. Switching to offline mode
			else{
				print("Map Error:"+www.error);
				//use the status string variable to print messages to your own user interface (GUIText, etc.)
				status = "Map Error:"+www.error;
				yield return new WaitForSeconds (1);
				maprender.material.mainTexture = null;
				Offline();
			}
			maprender.enabled = true;
		}
		ReSet();
		user.gameObject.renderer.enabled = true;
		ready = true;
		mapping = false;
		
	}

	//USING OFFLINE BACKGROUND TEXTURE
	void Offline(){
		if(!mapDisabled){
			maprender.material.mainTexture=Resources.Load("offline") as Texture2D;
			maprender.enabled = true;
		}
		ReSet();
		ready = true;
		mapping = false;
		user.gameObject.renderer.enabled = true;
	}

	//Re-position map and camera using updated data
	void ReSet(){
		Vector3 tmp = transform.position;
		tmp.x = (float)((tempLon*20037508.34f/180)/100)-iniRef.x;
		tmp.z = (float)(System.Math.Log(System.Math.Tan((90+tempLat)*System.Math.PI/360))/(System.Math.PI/180));
		tmp.z = ((tmp.z*20037508.34f/180)/100)-iniRef.z;
		transform.position = tmp;
		if(!freeCam){
			cam.position = new Vector3(transform.position.x, cam.position.y, transform.position.z);
		}
		if(triDView == false && centering){
			centered = true;
			autoCenter = true;
			centering = false;
		}
	}



	//RE-SCALE =========================================================================================================
	IEnumerator ReScale(){
		while(mapping){
			yield return null;
		}

		//Rescale map according to new zoom level to maintain 1:100 scale
		float newScale = multiplier*100532.244f/(Mathf.Pow (2, zoom));
		mymap.localScale = new Vector3(newScale, mymap.localScale.y, newScale);
		
		//3D View. Free/custom camera
		if(triDView && freeCam){
			//Do Nothing
		}
		
		//3D View and Camera follows player. Set camera position
		else if(triDView && !freeCam){
			Vector3 tmp = cam.localPosition;
			tmp.z = -(65536*camDist*Mathf.Cos(camAngle*Mathf.PI/180))/Mathf.Pow(2, zoom);
			tmp.y = 65536*camDist*Mathf.Sin(camAngle*Mathf.PI/180)/Mathf.Pow(2, zoom);
			cam.localPosition = tmp;
		}
		
		//2D View. Set camera position 
		else{
			if(firstTime){
				cam.localEulerAngles = new Vector3(90, 0, 0);
				if(screenY >= screenX){
					mycam.orthographicSize = mymap.localScale.z*5.0f*0.75f;
				}else{
					mycam.orthographicSize = (screenY/screenX)*mymap.localScale.x*5.0f*0.75f;		
				}
			}
			firstTime = false;

			if(screenY >= screenX){
				targetOrtoSize = Mathf.Round(mymap.localScale.z*5.0f*100.0f)/100.0f;
			}else{
				targetOrtoSize = Mathf.Round((screenY/screenX)*mymap.localScale.x*5.0f*100.0f)/100.0f;		
			}
			
			while(Mathf.Abs(mycam.orthographicSize-targetOrtoSize*0.625f) > 0.01f){
			currentOrtoSize = mycam.orthographicSize;
			currentOrtoSize = Mathf.MoveTowards (currentOrtoSize, targetOrtoSize*0.625f, 2.5f*32768*Time.deltaTime/Mathf.Pow(2, zoom));
			mycam.orthographicSize = currentOrtoSize;
			yield return null;
			}
			
			//Drag to pan speed according to zoom level
			dragSpeed = 0.8f/9.594413f*mycam.orthographicSize;
		}
	}

	void Update(){
		//Debug.Log (url);

		//Rename GUI "center" button label
		if(!triDView){
			if(cam.position.x != user.position.x || cam.position.z != user.position.z)
				centre ="center";
			else
				centre ="refresh";
		}
		
		//User pointer speed
		if(realSpeed){
			speed = userSpeed*0.05f;
		}
		else{
			speed = userSpeed*10000/(Mathf.Pow(2, zoom)*1.0f);
		}
		
		//3D-2D View Camera Toggle (use only while game is stopped) 
		if(triDView && !freeCam){
			cam.parent = user;
			if(ready)
				cam.LookAt(user);
		}	
		
		if(ready){	
			if(!simGPS){
				//Smoothly move pointer to updated position
				currentUserPos.x = user.position.x;
				currentUserPos.x = Mathf.Lerp (user.position.x, newUserPos.x, 2.0f*Time.deltaTime);
				currentUserPos.z = user.position.z;
				currentUserPos.z = Mathf.Lerp (user.position.z, newUserPos.z, 2.0f*Time.deltaTime);
				user.position = new Vector3(currentUserPos.x, user.position.y, currentUserPos.z);

				//Update rotation
				if(System.Math.Abs(user.eulerAngles.y-heading) >= 5){
					float newAngle = Mathf.SmoothDampAngle(user.eulerAngles.y, heading, ref yVelocity, smooth);
					user.eulerAngles = new Vector3(user.eulerAngles.x, newAngle, user.eulerAngles.z);
				}
			}
			else{
				//When GPS Emulator is enabled, user position is controlled by keyboard input.
				if(mapping == false){
					//Use keyboard input to move the player
					if (Input.GetKey ("up") || Input.GetKey ("w")){
						user.transform.Translate(Vector3.forward*speed*Time.deltaTime);
					}
					if (Input.GetKey ("down") || Input.GetKey ("s")){
						user.transform.Translate(-Vector3.forward*speed*Time.deltaTime);
					}
					//rotate pointer when pressing Left and Right arrow keys
					user.Rotate(Vector3.up, Input.GetAxis("Horizontal")*80*Time.deltaTime);
				}
			}	
		}
		
		if(mapping && !mapDisabled){
			//get download progress while images are still downloading
			if(www != null)
                download = www.progress;
		}	
		
		//Enable/Disable map renderer 
		if(mapDisabled)
			maprender.enabled = false;
		else
			maprender.enabled = true;
		
		//PINCH TO ZOOM ================================================================================================
		if(pinchToZoom){
			if(Input.touchCount == 2 && mapping == false){
				touch = Input.GetTouch(0);
				touch2 = Input.GetTouch(1);
				
				if(touch.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began){
					focusScreenPoint = (touch.position+touch2.position)/2;
					focusWorldPoint = mycam.ScreenToWorldPoint(new Vector3(focusScreenPoint.x, focusScreenPoint.y, cam.position.y));
				}
				
				if(touch.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved){
					touchZoom = true;
					curDist = touch.position-touch2.position;
					prevDist = (touch.position-touch.deltaPosition)-(touch2.position-touch2.deltaPosition);
					actualDist = prevDist.magnitude-curDist.magnitude;
				}else{
					touchZoom = false;
				}
			}
		}
		if(touchZoom){								
																	
			//Modify camera orthographic size
			mycam.orthographicSize = mycam.orthographicSize+actualDist*Time.deltaTime*mycam.orthographicSize/30;
			mycam.orthographicSize = Mathf.Clamp(mycam.orthographicSize, 3*targetOrtoSize/8, targetOrtoSize);
			
			if(actualDist < 0){
				currentPosition.x = cam.position.x;
				currentPosition.x = Mathf.MoveTowards (currentPosition.x, focusWorldPoint.x, -actualDist*0.7f*32768*Time.deltaTime/Mathf.Pow(2, zoom));
				currentPosition.z = cam.position.z;
				currentPosition.z = Mathf.MoveTowards (currentPosition.z, focusWorldPoint.z, -actualDist*0.7f*32768*Time.deltaTime/Mathf.Pow(2, zoom));
				cam.position = new Vector3(currentPosition.x, cam.position.y, currentPosition.z);
			}
			else if (actualDist == 0){
				//Do nothing
			}
			else{
				currentPosition.x = cam.position.x;
				currentPosition.x = Mathf.MoveTowards (currentPosition.x, mymap.position.x, actualDist*0.7f*32768*Time.deltaTime/Mathf.Pow(2, zoom));
				currentPosition.z = cam.position.z;
				currentPosition.z = Mathf.MoveTowards (currentPosition.z, mymap.position.z, actualDist*0.7f*32768*Time.deltaTime/Mathf.Pow(2, zoom));
                cam.position = new Vector3(currentPosition.x, cam.position.y, currentPosition.z);
			}
			
			//Get touch drag speed for new zoom level
			dragSpeed = 0.8f/9.594413f*mycam.orthographicSize;
			
			//Clamp the camera position to avoid displaying any off the map areas
			ClampCam();
			CursorsOff();
					
			//Decrease zoom level
			if(Mathf.Round(mycam.orthographicSize*1000.0f)/1000.0f >= Mathf.Round(targetOrtoSize*1000.0f)/1000.0f && zoom>minZoom){
				if(!mapping){
					touchZoom = false;
					zoom = zoom-1;
					 StartCoroutine(MapPosition());
					 StartCoroutine(ReScale());
				}
			}
			//Increase zoom level
			if(Mathf.Round(mycam.orthographicSize*1000.0f)/1000.0f <= Mathf.Round((3*targetOrtoSize/8)*1000.0f)/1000.0f && zoom<maxZoom){
				if(!mapping){
					touchZoom = false;
					zoom = zoom+1;
					 StartCoroutine(MapPosition());
					 StartCoroutine(ReScale());
				}
			}
		}
		
		//DRAG TO PAN ==================================================================================================
		if(dragToPan){
			if(!mapping && ready){

				#if (UNITY_EDITOR || UNITY_STANDALONE)
				//mouse drag
				if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0)) {
					autoCenter = false;
					centered = false;
					if(Input.mousePosition.y > screenY/12){

						//Check if any of the tile borders has been reached
						CheckBorders();
						
						//Translate the camera
						if(Mathf.Abs (Input.GetAxis("Mouse X"))>0 || Mathf.Abs (Input.GetAxis("Mouse Y"))>0 )
							cam.Translate(-Input.GetAxis("Mouse X") * dragSpeed * 0.7f, -Input.GetAxis("Mouse Y") * dragSpeed * 0.7f, 0);
						
						//Clamp the camera position to avoid displaying any off the map areas
						ClampCam();
					}
				}
				#endif

				//Touch drag
				if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
					autoCenter = false;
					centered = false;
					if(Input.GetTouch(0).position.y > screenY/12){
						Vector2 touchDeltaPosition  = Input.GetTouch(0).deltaPosition;
						
						//Check if any of the tile borders has been reached
						CheckBorders();
						//Translate the camera
						cam.Translate(-touchDeltaPosition.x*dragSpeed*Time.deltaTime, -touchDeltaPosition.y*dragSpeed*Time.deltaTime, 0);
						
						//Clamp the camera position to avoid displaying any off the map areas
						ClampCam();
					}
				}
			}	
		}																																
	}
	
	//READ TEXT FILE =========================================================================================================
	public void ReadThere(){
         string curlineThere; //current line
         //System.IO.StreamReader fileThere = new System.IO.StreamReader("/Users/stephaniegitha/Desktop/MTA15835_LBG - GPSWORK/Assets/Scenes/routePointsThere.txt");
         
         System.IO.StreamReader fileThere = new System.IO.StreamReader(pathThere);

         while((curlineThere = fileThere.ReadLine()) != null)
         {
             routePointsThere.Add(curlineThere);
         }

     }
     
     public void ReadBack(){
         string curlineBack; //current line
        

         //System.IO.StreamReader fileBack = new System.IO.StreamReader("/Users/stephaniegitha/Desktop/MTA15835_LBG - GPSWORK/Assets/Scenes/routePointsBack.txt");
         
         System.IO.StreamReader fileBack = new System.IO.StreamReader(pathBack);

         while((curlineBack = fileBack.ReadLine()) != null)
         {
             routePointsBack.Add(curlineBack);
         }

     }
	
	void CheckBorders(){
		//Reached left tile border
		if(Mathf.Round((mycam.ScreenToWorldPoint(new Vector3(0, 0.5f, cam.position.y)).x)*100.0f)/100.0f <= Mathf.Round((mymap.position.x-mymap.localScale.x*5)*100.0f)/100.0f){
			//show button for borderTile=4;
			tileLeft = true;
		}else{
			//hide button
			tileLeft = false;
		}
		//Reached right tile border
		if(Mathf.Round((mycam.ScreenToWorldPoint(new Vector3(mycam.pixelWidth, 0.5f, cam.position.y)).x)*100.0f)/100.0f >= Mathf.Round((mymap.position.x+mymap.localScale.x*5)*100.0f)/100.0f){
			//show button for borderTile=2;
			tileRight = true;
		}else{
			//hide button
			tileRight = false;
		}
		//Reached bottom tile border
		if(Mathf.Round((mycam.ScreenToWorldPoint(new Vector3(0.5f, 0, cam.position.y)).z)*100.0f)/100.0f <= Mathf.Round((mymap.position.z-mymap.localScale.z*5)*100.0f)/100.0f){
			//show button for borderTile=3;
			tileBottom = true;
		}else{
			//hide button
			tileBottom = false;
		}
		//Reached top tile border
		if(Mathf.Round((mycam.ScreenToWorldPoint(new Vector3(0.5f, mycam.pixelHeight, cam.position.y)).z)*100.0f)/100.0f >= Mathf.Round((mymap.position.z+mymap.localScale.z*5)*100.0f)/100.0f){
			//show button for borderTile=1;
			tileTop = true;
		}else{
			//hide button
			tileTop = false;
		}
	}

	//Disable surrounding tiles cursors
	void CursorsOff(){
		tileTop = false;
		tileBottom = false;
		tileLeft = false;
		tileRight = false;
	}

	//Clamp the camera position
	void ClampCam(){
		Vector3 tmp = cam.position;
		tmp.x = Mathf.Clamp(cam.position.x, 
		                    mymap.position.x-(mymap.localScale.x*5)+(mycam.ScreenToWorldPoint(new Vector3(mycam.pixelWidth, 0.5f, cam.position.y)).x-mycam.ScreenToWorldPoint(new Vector3(0, 0.5f, cam.position.y)).x)/2, 
		                    mymap.position.x+(mymap.localScale.x*5)-(mycam.ScreenToWorldPoint(new Vector3(mycam.pixelWidth, 0.5f, cam.position.y)).x-mycam.ScreenToWorldPoint(new Vector3(0, 0.5f, cam.position.y)).x)/2 );
		tmp.z = Mathf.Clamp(cam.position.z, 
		                    mymap.position.z-(mymap.localScale.z*5)+(mycam.ScreenToWorldPoint(new Vector3(0.5f, mycam.pixelHeight, cam.position.y)).z-mycam.ScreenToWorldPoint(new Vector3(0.5f, 0, cam.position.y)).z)/2, 
		                    mymap.position.z+(mymap.localScale.z*5)-(mycam.ScreenToWorldPoint(new Vector3(0.5f, mycam.pixelHeight, cam.position.y)).z-mycam.ScreenToWorldPoint(new Vector3(0.5f, 0, cam.position.y)).z)/2 );				
		cam.position = tmp;
	}

	//SAMPLE USER INTERFACE. MODIFY OR EXTEND IF NECESSARY =============================================================
	void OnGUI () {
		GUI.skin.box.alignment = TextAnchor.MiddleCenter;
		GUI.skin.box.font = Resources.Load("Neuropol") as Font;
		GUI.skin.box.normal.background = Resources.Load("grey") as Texture2D;
		if(Screen.width >= Screen.height){
			GUI.skin.button.fontSize = (int)Mathf.Round(10*Screen.width/480);
			GUI.skin.box.fontSize = (int)Mathf.Round(10*Screen.width/320);
		}
		else{	
			GUI.skin.button.fontSize = (int) Mathf.Round(10*Screen.height/480);
			GUI.skin.box.fontSize = (int) Mathf.Round(10*Screen.height/320);
		}	
		
		//Display Updating Map message
		if(ready && mapping){
			GUI.Box (new Rect (0, screenY-screenY/12, screenX, screenY/12), "Updating...");
		}
		
		//Display button to center camera at user position if GUI buttons are not enabled
		if (ready && !mapping && !buttons && !centered){	
			if (GUI.Button(new Rect(10*dot, screenY-74*dot, 64*dot, 64*dot), centerIcon)){
				centering = true;
				 StartCoroutine(MapPosition());
				 StartCoroutine(ReScale());
			}
		}
		
		//Display surrounding tiles buttons 
		if (ready && !mapping){	
			if(tileTop){
				GUI.DrawTexture(topCursorPos, topIcon);
				if (GUI.Button(topCursorPos, "", "label")){
					borderTile = 1;
					 StartCoroutine(MapPosition());
					 StartCoroutine(ReScale());
				}
			}
			if(tileRight){
				GUI.DrawTexture(rightCursorPos, rightIcon);
				if (GUI.Button(rightCursorPos, "", "label")){
					borderTile = 2;
					 StartCoroutine(MapPosition());
					 StartCoroutine(ReScale());
				}
			} 
			if(tileBottom){
				GUI.DrawTexture(bottomCursorPos, bottomIcon);
				if (GUI.Button(bottomCursorPos, "", "label")){
					borderTile = 3;
					 StartCoroutine(MapPosition());
					 StartCoroutine(ReScale());
				}
			} 
			if(tileLeft){
				GUI.DrawTexture(leftCursorPos, leftIcon);
				if (GUI.Button(leftCursorPos, "", "label")){
					borderTile = 4;
					 StartCoroutine(MapPosition());
					 StartCoroutine(ReScale());
				}
			} 
		}
		
		if (ready && !mapping && buttons){
			GUI.BeginGroup (new Rect (0, screenY-screenY/12, screenX, screenY/12));	
			GUI.Box (new Rect (0, 0, screenX, screenY/12), "");
			
			//Map type toggle button
			if (GUI.Button(new Rect(0, 0, screenX/5, screenY/12), maptype[index])){
				if(mapping == false){
					if(index < maptype.Length-1)
						index = index+1;
					else
						index = 0;	
					 StartCoroutine(MapPosition());
					 StartCoroutine(ReScale());
				}    
			}

			//3D Zoom Buttons
			if(triDView){
				//Zoom In button
				if(GUI.Button(new Rect(2*screenX/5, 0, screenX/5, screenY/12), "zoom +")){
					if(zoom < maxZoom){
						zoom = zoom+1;
						 StartCoroutine(MapPosition());
						 StartCoroutine(ReScale());
					}
				}
				//Zoom Out button
				if(GUI.Button(new Rect(screenX/5, 0, screenX/5, screenY/12), "zoom -")){
					if(zoom > minZoom){
						zoom = zoom-1;
						 StartCoroutine(MapPosition());
						 StartCoroutine(ReScale());
					}
				}

			//2D Zoom Buttons
			}else{
				//Zoom In button
				if(GUI.RepeatButton(new Rect(2*screenX/5, 0, screenX/5, screenY/12), "zoom +")){
					if(Input.GetMouseButton(0)){
						currentOrtoSize = mycam.orthographicSize;
						currentOrtoSize = Mathf.MoveTowards (currentOrtoSize, 3*targetOrtoSize/8, 5*32768*Time.deltaTime/Mathf.Pow(2, zoom));
						mycam.orthographicSize = currentOrtoSize;
						
						//Clamp the camera position to avoid displaying any off the map areas
						ClampCam();
						CursorsOff();
						
						//Get touch drag speed for new zoom level
						dragSpeed = 0.8f/9.594413f*mycam.orthographicSize;

						//Increase zoom level
						if(Mathf.Round(mycam.orthographicSize*1000.0f)/1000.0f <= Mathf.Round((3*targetOrtoSize/8)*1000.0f)/1000.0f && zoom<maxZoom){
							if(!mapping){
								zoom = zoom+1;
								 StartCoroutine(MapPosition());
								 StartCoroutine(ReScale());
							}
						}
					}
				}

				//Zoom Out button
				if (GUI.RepeatButton(new Rect(screenX/5, 0, screenX/5, screenY/12), "zoom -")){
					if(Input.GetMouseButton(0)){
						currentOrtoSize = mycam.orthographicSize;
						currentOrtoSize = Mathf.MoveTowards (currentOrtoSize, targetOrtoSize, 5*32768*Time.deltaTime/Mathf.Pow(2, zoom));
						mycam.orthographicSize = currentOrtoSize;
						
						//Center camera on map as we zoom out
						currentPosition.x = cam.position.x;
						currentPosition.x = Mathf.MoveTowards (currentPosition.x, mymap.position.x, 10*32768*Time.deltaTime/Mathf.Pow(2, zoom));
						currentPosition.z = cam.position.z;
						currentPosition.z = Mathf.MoveTowards (currentPosition.z, mymap.position.z, 10*32768*Time.deltaTime/Mathf.Pow(2, zoom));
                        cam.position = new Vector3(currentPosition.x, cam.position.y, currentPosition.z);

						//Clamp the camera position to avoid displaying any off the map areas
						ClampCam();
						CursorsOff();

						//Get touch drag speed for new zoom level
						dragSpeed = 0.8f/9.594413f*mycam.orthographicSize;
							
						//Decrease zoom level
						if(Mathf.Round(mycam.orthographicSize*1000.0f)/1000.0f >= Mathf.Round(targetOrtoSize*1000.0f)/1000.0f && zoom>minZoom){
							if(!mapping){
							    zoom = zoom-1;
							    StartCoroutine(MapPosition());
								StartCoroutine(ReScale());
							}
						}
					}
				}	
			}

			//Update map and center user position 
			if (GUI.Button(new Rect(3*screenX/5, 0, screenX/5, screenY/12), centre)){
				centering = true;
				StartCoroutine(MapPosition());
				StartCoroutine(ReScale());
			}

			//Show GPS Status info. Please make sure the GPS_Status.cs script is attached and enabled in the map object.
			if (GUI.Button(new Rect(4*screenX/5, 0, screenX/5, screenY/12), "info")){
				if(info)
					info = false;
				else
					info = true;
			}
			GUI.EndGroup ();
		}
	}
  
    //Translate decimal latitude to Degrees Minutes and Seconds
    string convertdmsLat(float lat) {
        string result;
        float degrees;
        float minutes;
        float seconds; 
        degrees = Mathf.Floor(Mathf.Abs(lat)); 
        minutes = (float)((Mathf.Abs(lat)-Mathf.Floor(Mathf.Abs(lat)))*60.0);
        seconds = (float)((minutes-Mathf.Floor(minutes))*60.0);
		result = degrees+"\u00B0 "+Mathf.Floor(minutes)+"' "+seconds.ToString("F2")+"\" "+((lat > 0) ? "N" : "S");
	    return result;
    }  
 
    //Translate decimal longitude to Degrees Minutes and Seconds  
    string convertdmsLon(float lon) {
        string result;
        float degrees;
        float minutes;
        float seconds;
        degrees = Mathf.Floor(Mathf.Abs(lon));
        minutes = (float)((Mathf.Abs(lon)-Mathf.Floor(Mathf.Abs(lon)))*60.0);
        seconds = (float)((minutes-Mathf.Floor(minutes))*60.0);
        result = degrees+"\u00B0 "+Mathf.Floor(minutes)+"' "+seconds.ToString("F2")+"\" "+((lon > 0) ? "E" : "W");
        return result;
    }
}