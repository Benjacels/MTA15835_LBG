using System;
using System.Collections;
using System.Collections.Generic;

public class Artwork {

	//Auto-implemented properties - correspond to attributes and child- and subchild-elements in the xml document
	public int ID { get; set; }
	public string TrackableName { get; set; }
	public string ArtistSurname { get; set; }
	public string ArtistFirstname { get; set; }
	public string ArtistFullname { get; set; }
	public string ArtistDescription { get; set; }
	public string ArtistYearOfBirth { get; set; }
	public string ArtistYearOfDeath { get; set; }
	public string ArtistNationality { get; set; }
	public string ArtistCountry { get; set; }
	public string ArtworkTitle { get; set; }
	public string ArtworkDesription { get; set; }
	public string ArtworkYear { get; set; }
	public string ArtworkMaterial { get; set; }
	public string ArtworkUnitOfLength { get; set; }
	public string ArtworkHeight { get; set; }
	public string ArtworkWidth { get; set; }
	public string ArtworkType { get; set; }
	public string ArtworkMood { get; set; }
	public string ArtworkPeriod { get; set; }

	//list
	public List<string> ArtworkThemes;
	public List<string> MediaAudioDescription;
	public List<string> MediaAudioUrl;

	//Private fields
	private string artworkDimension;
	private string mediaAudioDescription;
	private string mediaAudioUrl;
	private float artworkHeightDot;
	private float artworkWidthDot;

	//Get/set properties 
	public string ArtworkDimensions { 
		get{ 
			artworkDimension = ArtworkHeight + " x " + ArtworkWidth + " " + ArtworkUnitOfLength;
			return artworkDimension;
		}
	}

//	public string MediaAudioDescription {
//		get { return this.mediaAudioDescription != "" ? this.mediaAudioDescription : "n/a"; } 
//		set { this.mediaAudioDescription = value; }
//	}
//
//	public string MediaAudioUrl {
//		get { return this.mediaAudioUrl != "" ? this.mediaAudioUrl : "n/a"; }
//		set { this.mediaAudioUrl = value; }
//	}

	//if there are two widths or heights then split at '/'
	public float ArtworkHeightDot {
		get {
			if(ArtworkHeight != "") {
				string tempText = ArtworkHeight.Replace(',','.');
				artworkHeightDot = float.Parse(tempText);
				return artworkHeightDot;
			} else {
				return 0.0f;
			}
		}
	}

	public float ArtworkWidthDot {
		get {
			if(ArtworkHeight != "") {
				string tempText = ArtworkWidth.Replace(',','.');
				artworkWidthDot = float.Parse(tempText);
				return artworkWidthDot;
			} else {
				return 0.0f;
			}
		}
	}

//	//Constructor
//	public Artwork (int id, string trackableName, string artistSurname, string artistFirstname, 
//	                string artistFullname, string artistDescription, string artistYearOfBirth, string artistYearOfDeath, 
//	                string artistNationality, string artworkTitle, string artworkDescription, string artworkYear, 
//	                string artworkMaterial, string artworkUnitOfLength, string artworkHeight, string artworkWidth, 
//	                string artworkType, string mediaAudioDescription, string mediaAudioUrl)
//	{
//		ID = id; //artwork id
//		TrackableName = trackableName; //trackable name as named in Vuforia target manager
//		//artist
//		ArtistSurname = artistSurname;
//		ArtistFirstname = artistFirstname;
//		ArtistFullname = artistFullname;
//		ArtistDescription = artistDescription;
//		ArtistYearOfBirth = artistYearOfBirth;
//		ArtistYearOfDeath = artistYearOfDeath;
//		ArtistNationality = artistNationality;
//		//artwork
//		ArtworkTitle = artworkTitle;
//		ArtworkDesription = artworkDescription;
//		ArtworkYear = artworkYear;
//		ArtworkMaterial = artworkMaterial;
//		ArtworkUnitOfLength = artworkUnitOfLength;
//		ArtworkHeight = artworkHeight;
//		ArtworkWidth = artworkWidth;
//		ArtworkType = artworkType;
//		//audio guide
//		MediaAudioDescription = mediaAudioDescription;
//		MediaAudioUrl = mediaAudioUrl;
//	}

	//Methods
	public string metadata(){
		string returnString;
		returnString = "<b>" + ArtworkTitle + "</b>";
		returnString += "\n" + ArtistFullname;
		returnString += "\n\n" + ArtworkDesription;

		return returnString;
	}

}
