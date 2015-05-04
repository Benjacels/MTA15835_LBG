using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine.UI;

public class InitGPSlog : MonoBehaviour {

	string _filename = "gpsLog.xml";
	string path;
	string docContent;
	XmlDocument xmlDoc = new XmlDocument();
	public InputField inputDescription; 

	// Use this for initialization
	void Start () {
		path = Path.Combine (Application.persistentDataPath, _filename);
		print ("_______ filepath: " + path);

		if (fileExistence () == true) {
			docContent = loadFileContent(path);
		} else {
			createFile();
			if (fileExistence() == true){
				docContent = loadFileContent(path);
			}else{
				print ("_____ error");
			}
		}

		if (docContent != "") { 
			xmlDoc.LoadXml(docContent); 
			print ("________ xmlDoc: " + xmlDoc.InnerXml);
		}

		//createNewPath ();

		//deletes every file in persistentdata, not on iOS
		//DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath);
		//dataDir.Delete(true);
	}

	string loadFileContent(string p){
		StreamReader r = File.OpenText(p); 
		string _info = r.ReadToEnd(); 
		r.Close(); 
		Debug.Log("______ File Read"); 
		return _info;
	}

	bool fileExistence(){
		if (System.IO.File.Exists (path)) {
			print ("_______ file exist");
			return true;
		} else {
			print("_______ file does not exist");
			return false;
		}
	}

	void createFile(){
		StreamWriter fileWriter = File.CreateText(path);

		string initialTxt;
		//loads initial xml template
		#if UNITY_IPHONE
		initialTxt = loadFileContent (Application.dataPath + "/Raw/gpsLogInitial.xml");
		#endif

		#if UNITY_EDITOR
		initialTxt = loadFileContent (Application.dataPath + "/gpsLogInitial.xml");
		#endif

		print ("initialTxt: " + initialTxt);
		fileWriter.WriteLine(initialTxt);
		fileWriter.Close(); 
	}

	public void createNewPath(){
		string template = "<?xml version='1.0' encoding='UTF-8'?> <kml xmlns='http://www.opengis.net/kml/2.2'>" + "<Document xmlns='http://www.opengis.net/kml/2.2'><Style id='style1'> <LineStyle> <colorMode>random</colorMode> <width>4</width> </LineStyle> </Style>  <name>LBG test - 2015</name>"; 

		//saves current name and description from xml
		XmlNodeList pm = xmlDoc.GetElementsByTagName("Placemark"); 
		List<string> name = new List<string>();
		List<string> beskrivelse = new List<string>();
		foreach (XmlNode node in pm) {
			name.Add(node.ChildNodes.Item(0).InnerXml);
			beskrivelse.Add(node.ChildNodes.Item(1).InnerXml);
		}

		/*foreach (string n in name) {
			print (n);
		}*/
		 
		//add old coordinates, name and description to template
		XmlNodeList ls = xmlDoc.GetElementsByTagName("LineString"); //.ChildNodes.Item (0).InnerXml);
		int j = 0;
		foreach (XmlNode node in ls){
			print(node.ChildNodes.Item(1).InnerXml);
			
			template += "<Placemark> <name>" + name[j] + "</name><description>" + beskrivelse[j] + "</description><styleUrl>#style1</styleUrl> <LineString> <tessellate>1</tessellate> <coordinates>" + node.ChildNodes.Item(1).InnerXml + "</coordinates> <!-- longitude, latitude, and altitude, where altitude is optional --> </LineString> </Placemark>";
			
			j++;
		}

		string timeDate = System.DateTime.Now.ToString();
																	// place description here
		//add new placemark
		template += "<Placemark> <name>" + timeDate + "</name><description>" + inputDescription.text + "</description><styleUrl>#style1</styleUrl> <LineString> <tessellate>1</tessellate> <coordinates></coordinates> <!-- longitude, latitude, and altitude, where altitude is optional --> </LineString> </Placemark>";

		//ends document
		template += "</Document> </kml>"; 

		//loades template xml into doc
		XmlDocument newXmlData = new XmlDocument ();
		newXmlData.LoadXml (template);

		saveData (newXmlData.InnerXml);

		print ("newXmlData: " + newXmlData.InnerXml);
	}

	void saveData(string dataToSave){
		StreamWriter fileWriter = File.CreateText(path);
		fileWriter.Write(dataToSave);
		fileWriter.Close(); 
	}
}
