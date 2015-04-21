using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public sealed class XmlManager{

	#region PRIVATE_MEMBER_VARIABLES
	private TextAsset textAsset;
	private int prevIndex;
	private string previousTrackableName;
	#endregion

	#region PUBLIC_MEMBER_VARIABLES
	public Artwork[] Artwork {get; set;}
	#endregion

	#region SINGLETON
	private static readonly XmlManager instance = new XmlManager();

	private XmlManager(){}

	public static XmlManager Instance {
		get{
			return instance;
		}
	}
	#endregion
	
	#region PUBLIC_METHODS

	public void LoadXml(){

		textAsset = Resources.Load("xmlArtworksDB2") as TextAsset;
		
		if(textAsset != null){
			Debug.Log("Sucessfully loaded XML document: " + textAsset.name);
			
			//Create a new XML document out of the loaded data
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(textAsset.text);
			
			//Point to and process root/child node
			ProcessArtworkData(xmlDoc.SelectNodes("artworks/artwork"));
			
		} else {
			Debug.Log("ERROR: XML document not loaded." + "\nCheck if the XML document is assigned!!!");
		}
	}
	
	/// <summary>
	/// Searchs the artworks using binarysearch by comparing sting value with object property trackablename
	/// </summary>
	/// <param name="trackableName">Trackable name.</param>
	
	//target names are used instead of an id in case that some of the targets are deleted from the target DB or artwork metadata is altered.
	public int SearchArtworks(string trackableName) {
		
		if(trackableName != previousTrackableName){
			Debug.Log("binary search is only processed if the search term is not the same as previously");
			int index = Array.BinarySearch(Artwork, trackableName, new CompareClassValue());
			
			previousTrackableName = trackableName;
			prevIndex = index;
			
			if(index < 0){
				Debug.Log("Trackable: " + trackableName + ", does not exist in the artwork database");
			} else {
				Debug.Log("Trackable: " + trackableName + ", exist in the artwork database at index [" + index + "]");
			}
			return index;
		} else {
			return prevIndex;
		}
		
	}
	
	#endregion

	#region PRIVATE_METHODS
	
	private void ProcessArtworkData(XmlNodeList nodes){
		Debug.Log("Process XmlNodeList and assign nodes to Artwork objects");
		
		int numberOfElements = nodes.Count;
		Artwork = new Artwork[numberOfElements]; //create array of artwork objects 
		
		for(int i = 0; i < nodes.Count; i++){
			//instantiate objects and assign node values
			
			Artwork[i] = new Artwork();
			Artwork[i].TrackableName = nodes[i].Attributes.GetNamedItem("trackable_name").InnerText;
			Artwork[i].ArtistSurname = nodes[i].SelectSingleNode("artist/surname").InnerText;
			Artwork[i].ArtistFirstname = nodes[i].SelectSingleNode("artist/firstname").InnerText;
			Artwork[i].ArtistFullname = nodes[i].SelectSingleNode("artist/fullname").InnerText;
			Artwork[i].ArtistDescription = nodes[i].SelectSingleNode("artist/description").InnerText;
			Artwork[i].ArtistYearOfBirth = nodes[i].SelectSingleNode("artist/born").InnerText;
			Artwork[i].ArtistYearOfDeath = nodes[i].SelectSingleNode("artist/dead").InnerText;
			Artwork[i].ArtistNationality = nodes[i].SelectSingleNode("artist/nationality").InnerText;
			Artwork[i].ArtistCountry = nodes[i].SelectSingleNode("artist/country").InnerText;
			Artwork[i].ArtworkTitle = nodes[i].SelectSingleNode("title").InnerText;
			Artwork[i].ArtworkDesription = nodes[i].SelectSingleNode("description").InnerText;
			Artwork[i].ArtworkYear = nodes[i].SelectSingleNode("year").InnerText;
			Artwork[i].ArtworkMaterial = nodes[i].SelectSingleNode("material").InnerText;
			Artwork[i].ArtworkUnitOfLength = nodes[i].SelectSingleNode("dimensions").Attributes.GetNamedItem("unit").Value;
			Artwork[i].ArtworkHeight = nodes[i].SelectSingleNode("dimensions/height").InnerText;
			Artwork[i].ArtworkWidth = nodes[i].SelectSingleNode("dimensions/width").InnerText;
			Artwork[i].ArtworkType = nodes[i].SelectSingleNode("type").InnerText;
			Artwork[i].ArtworkPeriod = nodes[i].SelectSingleNode("period").InnerText;
			Artwork[i].ArtworkMood = nodes[i].SelectSingleNode("mood").InnerText;
			
			//loop through media/audio_description and media/audio_url
			Artwork[i].MediaAudioDescription = new List<string>();
			foreach(XmlNode mediaAudioDescription in nodes[i].SelectNodes("media/audio_description"))
				Artwork[i].MediaAudioDescription.Add(mediaAudioDescription.InnerText);
			
			Artwork[i].MediaAudioUrl = new List<string>();
			foreach(XmlNode mediaAudioUrl in nodes[i].SelectNodes("media/audio_url"))
				Artwork[i].MediaAudioUrl.Add(mediaAudioUrl.InnerText);
			
			//loop through themes/theme
			Artwork[i].ArtworkThemes = new List<string>();
			foreach(XmlNode theme in nodes[i].SelectNodes("themes/theme")) {
				Artwork[i].ArtworkThemes.Add(theme.InnerText);
			}
			
		}
		
		Array.Sort(Artwork, delegate(Artwork x, Artwork y) {
			return x.TrackableName.CompareTo(y.TrackableName);
		});
		
	}
	
	#endregion
}
