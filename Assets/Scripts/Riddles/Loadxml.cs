using UnityEngine;
using System.Collections;
using System.Xml;

public class Loadxml : MonoBehaviour {

	private TextAsset textAsset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	XmlDocument loadLocalXml(string name){
		textAsset = Resources.Load(name) as TextAsset;  
		
		if (textAsset != null) {
			XmlDocument xmldoc = new XmlDocument();
			xmldoc.LoadXml ( textAsset.text );
			return xmldoc;
		}else {
			print("Error: are you sure that the specified xml file exists?");
			return null;
		}
	}
}