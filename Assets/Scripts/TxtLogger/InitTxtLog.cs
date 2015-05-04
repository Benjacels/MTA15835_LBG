using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class InitTxtLog : MonoBehaviour {
	public string _filename;
	private string path;
	private string docContent;
	public InputField description;
	public SetupGame setupGame;
	public GameObject createfb;

	// Use this for initialization
	void Start () {
		path = Path.Combine (Application.persistentDataPath, _filename);
		print ("_______ TXT filepath: " + path);
		
		if (fileExistence () == true) {
			docContent = loadFileContent(path);
		} else {
			createFile();
			if (fileExistence() == true){
				docContent = loadFileContent(path);
			}else{
				print ("_____ TXT error");
			}
		}
		
		if (docContent != "") { 
			print ("________ TXT doc: " + docContent);
		}
		
		//deletes every file in persistentdata, not on iOS
		//DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath);
		//dataDir.Delete(true);
	
	}
	
	bool fileExistence(){
		if (System.IO.File.Exists (path)) {
			print ("_______ TXT file exist");
			return true;
		} else {
			print("_______ TXT file does not exist");
			return false;
		}
	}

	string loadFileContent(string p){
		StreamReader r = File.OpenText(p); 
		string _info = r.ReadToEnd(); 
		r.Close(); 
		Debug.Log("______ TXT File Read"); 
		return _info;
	}

	void createFile(){
		StreamWriter fileWriter = File.CreateText(path);
		
		string initialTxt = _filename + " created: " + System.DateTime.Now + "\n \n \n";

		fileWriter.WriteLine(initialTxt);
		fileWriter.Close(); 
	}

	public void indicateNewSession(){
		docContent = docContent + "\n \n" + "--------------------------" + "\n" + System.DateTime.Now.ToString() + " - New session, with description: " + description.text + "\n";
		saveData (docContent);

		setupGame.setNewSessionCreated (true);

		createfb.SetActive (true);
	}

	private void saveData(string dataToSave){
		StreamWriter fileWriter = File.CreateText(path);
		fileWriter.Write(dataToSave);
		fileWriter.Close(); 
		Debug.Log("______ TXT File saved"); 
	}
}
