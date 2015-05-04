using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class GPSlogger : MonoBehaviour {

	string path;
	bool canLog;
	XmlDocument xmldoc = new XmlDocument();

	// Use this for initialization
	void Awake () {
		string _filename = "gpsLog.xml";
		path = Path.Combine (Application.persistentDataPath, _filename);

		if (System.IO.File.Exists (path)) {
			string fileContent = loadFileContent(path);
			xmldoc.LoadXml(fileContent);
			canLog = true;
		} else {
			canLog = false;
			print ("_____ no file to write to, needs InitGPSlog to run first");
		}
	}

	//demands lon and lat, alt is optional
	public void logCoordinate(float lon, float lat, float alt = 0f){
		if (canLog == true) {
			print ("logs coordinate");
			XmlNodeList coordinates = xmldoc.GetElementsByTagName("coordinates");
			XmlNode lastCoordinate = coordinates.Item(coordinates.Count-1);
			lastCoordinate.InnerXml = lastCoordinate.InnerXml + " " + lon + "," + lat;
			if(alt != 0f){
				lastCoordinate.InnerXml = lastCoordinate.InnerXml + "," + alt;
			}
			saveData(xmldoc.InnerXml);
		}
	}

	void saveData(string dataToSave){
		StreamWriter fileWriter = File.CreateText(path);
		fileWriter.Write(dataToSave);
		fileWriter.Close(); 
		Debug.Log("______ File saved"); 
	}

	string loadFileContent(string p){
		StreamReader r = File.OpenText(p); 
		string _info = r.ReadToEnd(); 
		r.Close(); 
		Debug.Log("______ File Read"); 
		return _info;
	}
}
