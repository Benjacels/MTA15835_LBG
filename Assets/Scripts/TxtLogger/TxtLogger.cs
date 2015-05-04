using UnityEngine;
using System.Collections;
using System.IO;

public class TxtLogger : MonoBehaviour {
	public string _filename;
	private string path;
	private bool canLog;
	private string fileContent;

	// Use this for initialization
	void Start () {
		path = Path.Combine (Application.persistentDataPath, _filename);

		if (System.IO.File.Exists (path)) {
			fileContent = loadFileContent(path);
			canLog = true;
		} else {
			canLog = false;
			print ("_____ TXT no Text file to write to, needs InitTxtLog to run first");
		}
	}

	public void log(string logThis){
		if (canLog == true) {
			print ("_____ TXT logs " + logThis);
			string time = System.DateTime.Now.ToString();
			fileContent = fileContent + time + " - " + logThis + "\n";
			saveData(fileContent);
		}
	}

	private void saveData(string dataToSave){
		StreamWriter fileWriter = File.CreateText(path);
		fileWriter.Write(dataToSave);
		fileWriter.Close(); 
		Debug.Log("______ TXT File saved"); 
	}

	private string loadFileContent(string p){
		StreamReader r = File.OpenText(p); 
		string _info = r.ReadToEnd(); 
		r.Close(); 
		Debug.Log("______ TXT File Read"); 
		return _info;
	}
}
