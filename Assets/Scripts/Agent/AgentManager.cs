using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AgentManager : MonoBehaviour {

	#region STATE

	public enum State { //if tap and prevState instructing/teaching and curState is idle go to prevState
		None,
		Idle,
		WalkingForward, //not implemented
		WalkingBackward, //not implementer
		WalkingSideways, //could be fun to implementer
		Instructing,
		Teaching,
		// Below only applies to Relational Behavior
		Excited, //could be fun to implementer
		Commenting,
		SelfDisclosing,
		Greating,
		FareWell,
		Sleeping, //revesover
		Waiting,
		Thinking,
	}

	private State currentState = State.None;

	public State CurrentState
	{ 
		get { return currentState; }
		set { 
			PrevState = currentState;
			currentState = value;
		}
	}

	public State PrevState { get; set; }

	#endregion

//	#region MOOD STATE
//
//	// Only applies to Relational Behavior
//	public enum MoodState {
//		Neutral,
//		Humorous,
//		Interested,
//		Borred,
//		Impatient,
//		Eager,
//		Disappointed,
//		Understanding
//	}
//
//	private MoodState currentMood;
//	public MoodState CurrentMood
//	{
//		get { return currentMood; }
//		set {
//			PrevMood = currentMood;
//			currentMood = value;
//		}
//	}
//	public MoodState PrevMood { get; set; }
//
//	#endregion

//	public bool DoInstruction { get; set; }
	public bool isTeaching { get; set; }
	
	public List<Utterance> Greating = new List<Utterance>();
	public List<Utterance> Instruction = new List<Utterance>();
	public List<Utterance> UI_Instruction = new List<Utterance>();
	public List<Utterance> Thinking = new List<Utterance>();
	public List<Utterance> Waiting = new List<Utterance>();
	public List<Utterance> SelfDisclosing = new List<Utterance>();
	public List<Utterance> FareWell = new List<Utterance>();
	//Depend on knowledge base
	public List<Utterance> FoundSelfDisclosing = new List<Utterance>();
	public List<Utterance> FoundPraising = new List<Utterance>();
	public List<Utterance> Recognized = new List<Utterance>();
	public List<Utterance> Commenting = new List<Utterance>();
	public List<Utterance> Teaching = new List<Utterance>(); 
	public List<Utterance> CommentingAge = new List<Utterance>();

	#region SINGLETON PATTERN

	private static AgentManager instance;
	
	public static AgentManager Instance {
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<AgentManager>();
				
				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(instance.gameObject);
			}
			
			return instance;
		}
	}
	
	void Awake() {
		if(instance == null)
		{
			//If I am the first instance, make me the Singleton
			instance = this;

			DontDestroyOnLoad(this);
			
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != instance)
				Destroy(this.gameObject);
		}
	}

	#endregion

	void Start()
	{
		XmlManager.Instance.LoadXml();


		// ========================================== UTTERANCE ========================================== //
		// Utterance could be retrieved from an XML file
		// http://pub.uvm.dk/2011/fagliglaesning/vigtige_komponenter_i_tekstforstaaelse.html
		// http://uvm.dk/Service/Publikationer/Publikationer/Folkeskolen/2009/Faelles-Maal-2009-Billedkunst?Mode=full
		
		// Could say the users name if asking for it before game start. Likewise reading speed and LIX of the sentences could be adjusted by age
		
		Greating.Add(new Utterance() { Sentence = "Hej" }); //Silence = 1.44f
		Greating.Add(new Utterance() { Sentence = "Rart at møde dig" }); //Silence = 3f
		
		Instruction.Add(new Utterance() { Sentence = "Er du en ægte detektiv?" }); //Silence = 0.85f
		Instruction.Add(new Utterance() { Sentence = "så find de 5 malerier som jeg har ædt!" }); //Silence = 1.26f
		Instruction.Add(new Utterance() { Sentence = "Tryk på START for at gå på kunstjagt" }); //Silence = 0.1f

		UI_Instruction.Add(new Utterance() { Sentence = "Når du finder et værk jeg har ædt"});
		UI_Instruction.Add(new Utterance() { Sentence = "Kan du trykke på øjet!" });
		UI_Instruction.Add(new Utterance() { Sentence = "og vise mig det med kameraet" });
		
		Thinking.Add(new Utterance() { Sentence = "Mmmmm"});
		Thinking.Add(new Utterance() { Sentence = "Gammel maling"});
		
		Waiting.Add(new Utterance() { Sentence = "Bum" , Duration = 1.2f, Silence = 1f });
		Waiting.Add(new Utterance() { Sentence = "Bum" , Duration = 0.8f, Silence = 0.5f });
		Waiting.Add(new Utterance() { Sentence = "Bum" , Duration = 0.4f, Silence = 4f });
		
		SelfDisclosing.Add(new Utterance() { Sentence = "Blå er min favorit smag" });
		SelfDisclosing.Add(new Utterance() { Sentence = "Oliemaling er min livret" });
		SelfDisclosing.Add(new Utterance() { Sentence = "I går drømte jeg om Abstrakt kunst" });

//		List<string> AllThemes = new List<string>();
//		foreach(Artwork art in XmlManager.Instance.Artwork)
//		{
//			foreach(string theme in art.ArtworkThemes)
//			{
//				AllThemes.Add (theme);
//			}
//		}

		SelfDisclosing.Add(new Utterance() { Sentence = "I nat skal jeg æde KoBra kunst" });
		SelfDisclosing.Add(new Utterance() { Sentence = "Jeg er monster sulten" });
		SelfDisclosing.Add(new Utterance() { Sentence = "Jeg ville være sulten i en verden uden kunst" });
		SelfDisclosing.Add(new Utterance() { Sentence = "Museet er mit køleskab" });
		SelfDisclosing.Add(new Utterance() { Sentence = "Det er dejligt at bo på kunsten når man er et kunst monster" });
		
		FareWell.Add(new Utterance() { Sentence = "Du fandt alle værkerne", Silence = 0.4f });
		FareWell.Add(new Utterance() { Sentence = "jeg havde ædt" });
		FareWell.Add(new Utterance() { Sentence = "Nu knurre min mave" });
		FareWell.Add(new Utterance() { Sentence = "Den står på kunst til morgenmad", Silence = 1.2f });
		FareWell.Add(new Utterance() { Sentence = "Mmmmm" });

		if(DateTime.Now.Hour > 13)
		{
			Recognized.Add(new Utterance() { Sentence = "Måske spiser jeg det til morgenmad", Silence = 0.01f });
		} 
		else
		{
			Recognized.Add(new Utterance() { Sentence = "Måske spiser jeg det til natmad", Silence = 0.01f });
		}

		Recognized.Add(new Utterance() { Sentence = "Man bliver hvad man spiser", Silence = 0.01f });
		Recognized.Add(new Utterance() { Sentence = "Gode farver! men det har jeg ikke ædt", Silence = 0.01f });
		
		FoundSelfDisclosing.Add(new Utterance() { Sentence = "Det smagte så godt", Silence = 0.01f });
		FoundSelfDisclosing.Add(new Utterance() { Sentence = "Jeg spiste det med pensel til", Silence = 0.01f });
		FoundSelfDisclosing.Add(new Utterance() { Sentence = "Det gøre mig monster sulten", Silence = 0.01f });
		FoundSelfDisclosing.Add(new Utterance() { Sentence = "Mums", Silence = 0.01f });

		FoundPraising.Add(new Utterance() { Sentence = "WAUW du er en ægte kunst kender", Silence = 0.01f });
		FoundPraising.Add(new Utterance() { Sentence = "Monster godt gået", Silence = 0.01f });
		FoundPraising.Add(new Utterance() { Sentence = "Super godt gået", Silence = 0.01f });


	}
	// http://videnskab.dk/kultur-samfund/kunst-gor-det-let-laere-grammatik
	// http://vbn.aau.dk/en/persons/tatiana-chemi%28b2127dfc-3066-443e-bffc-7c7bd88f4127%29/publications.html
	public void KnowledgeBase()
	{	
		//If a trackable has been found search the artworks and get the index of the artwork
		int index = XmlManager.Instance.SearchArtworks(GameManager.Instance.TrackableName);
		
		if(GameManager.Instance.Relational)
		{
			if(GameManager.Instance.CurrentGameState == GameManager.GameState.ArtworkFound)
			{
				Teaching.Add(FoundPraising[Utilities.RandomNumber.Next(FoundPraising.Count)]);
			}

			if(GameManager.Instance.CurrentGameState == GameManager.GameState.ArtworkRecognized)
			{
				Teaching.Add(Recognized[Utilities.RandomNumber.Next(Recognized.Count)]);
			}

			Teaching.Add(new Utterance() { Sentence = "Dette maleri hedder"});
			Teaching.Add(new Utterance() { Sentence = XmlManager.Instance.Artwork[index].ArtworkTitle}); //Duration = 1.7f

			if(GameManager.Instance.CurrentGameState == GameManager.GameState.ArtworkFound)
			{
				//Could have some simple analysis of titles, description, theme
				//if artworkTitle contains a name of a color: All that red color tasted so well
				//if artist desctiption contains words like mad, excentric etc: I especially like mad artists
				//Depending on artworkTheme say something about stroke, artist type etc
				//Would be nice to have the gender of the artist
				Teaching.Add(FoundSelfDisclosing[Utilities.RandomNumber.Next(FoundSelfDisclosing.Count)]);
			}

			if(GameManager.Instance.CurrentGameState == GameManager.GameState.ArtworkFound && XmlManager.Instance.Artwork[index].ArtworkThemes[0] != "")
			{
				int countThemes = XmlManager.Instance.Artwork[index].ArtworkThemes.Count;
				string sentence = XmlManager.Instance.Artwork[index].ArtworkThemes[Utilities.RandomNumber.Next(countThemes)];
				if(sentence.Contains("kunst"))
					Teaching.Add(new Utterance() { Sentence = sentence + "\ner en af mine livretter"});
				else
					Teaching.Add(new Utterance() { Sentence = sentence + "\ngenren\ner en af mine livretter"});

//				Teaching.Add(new Utterance() { Sentence = XmlManager.Instance.Artwork[index].ArtworkThemes[Utilities.RandomNumber.Next(countThemes)] 
//					+ "\ngenren\ner en af mine livretter"}); //Silence = 0.01f
			}
			else if(GameManager.Instance.CurrentGameState == GameManager.GameState.ArtworkRecognized && XmlManager.Instance.Artwork[index].ArtworkThemes[0] != "")
				Teaching.Add(new Utterance() { Sentence = "Værket er genren\n" + XmlManager.Instance.Artwork[index].ArtworkThemes[0]});
			else if(XmlManager.Instance.Artwork[index].ArtworkThemes[0] == "" && XmlManager.Instance.Artwork[index].ArtworkPeriod != "")
				Teaching.Add(new Utterance() { Sentence = "Værket er fra\n" + XmlManager.Instance.Artwork[index].ArtworkPeriod});
			else
				Teaching.Add(new Utterance() { Sentence = "Værket er fra " + XmlManager.Instance.Artwork[index].ArtworkYear }); //Duration = 2

			Teaching.Add(new Utterance() { Sentence = "Det er malet af"});
			Teaching.Add(new Utterance() { Sentence = "den\n" + XmlManager.Instance.Artwork[index].ArtistNationality + "e\nkunstner"});
			Teaching.Add(new Utterance() { Sentence = XmlManager.Instance.Artwork[index].ArtistFullname }); //Duration = 2
			Teaching.Add(new Utterance() { Sentence = "som er født i " + XmlManager.Instance.Artwork[index].ArtistYearOfBirth});

			if(XmlManager.Instance.Artwork[index].ArtistYearOfDeath != "" && DateTime.Now.Year - Convert.ToInt32(XmlManager.Instance.Artwork[index].ArtistYearOfBirth) > 80)
			{
				int yearsAgo = DateTime.Now.Year - Convert.ToInt32(XmlManager.Instance.Artwork[index].ArtistYearOfBirth);

				CommentingAge.Add(new Utterance() { Sentence = "Den kunstner må være tudsegammel"});
				CommentingAge.Add(new Utterance() { Sentence = "Det er godt nok lang tid siden"});
				CommentingAge.Add(new Utterance() { Sentence = "WAUW\n Det er " + yearsAgo + " år siden"});
				
				Teaching.Add(CommentingAge[Utilities.RandomNumber.Next(CommentingAge.Count)]);
			}
//				Teaching.Add(new Utterance() { Sentence = "Den kunstner må være tudsegammel" , Silence = 0.01f});

		}
		else
		{
			Teaching.Add(new Utterance() { Sentence = "Dette maleri hedder"});
			Teaching.Add(new Utterance() { Sentence = XmlManager.Instance.Artwork[index].ArtworkTitle }); //Duration = 1.7f

			if(XmlManager.Instance.Artwork[index].ArtworkThemes[0] != "")
				Teaching.Add(new Utterance() { Sentence = "Værket er genren\n" + XmlManager.Instance.Artwork[index].ArtworkThemes[0]});
			else if(XmlManager.Instance.Artwork[index].ArtworkThemes[0] == "" && XmlManager.Instance.Artwork[index].ArtworkPeriod != "")
				Teaching.Add(new Utterance() { Sentence = "Værket er fra\n" + XmlManager.Instance.Artwork[index].ArtworkPeriod});
			else
				Teaching.Add(new Utterance() { Sentence = "Værket er fra " + XmlManager.Instance.Artwork[index].ArtworkYear, Duration = 2});
			
			Teaching.Add(new Utterance() { Sentence = "Det er malet af"});
			Teaching.Add(new Utterance() { Sentence = "den\n" + XmlManager.Instance.Artwork[index].ArtistNationality + "e\nkunstner"});
			Teaching.Add(new Utterance() { Sentence = XmlManager.Instance.Artwork[index].ArtistFullname, Duration = 2});
			Teaching.Add(new Utterance() { Sentence = "som er født i " + XmlManager.Instance.Artwork[index].ArtistYearOfBirth});

			if(XmlManager.Instance.Artwork[index].ArtistYearOfDeath != "" && DateTime.Now.Year - Convert.ToInt32(XmlManager.Instance.Artwork[index].ArtistYearOfBirth) > 80)
			{
				int yearsAgo = DateTime.Now.Year - Convert.ToInt32(XmlManager.Instance.Artwork[index].ArtistYearOfBirth);

				CommentingAge.Add(new Utterance() { Sentence = "Det er " + yearsAgo + " år siden"});
				
				Teaching.Add(CommentingAge[0]);
			}
		}

	}

}
