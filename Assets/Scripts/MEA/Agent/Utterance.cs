using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;


// https://www.thorsager-skole.dk/Infoweb/indhold/Evaluering/L%C3%B8bende%20evaluering.htm
// http://skolebib.skoleblogs.dk/files/2012/12/Bilag-5-m%C3%A5ling-af-l%C3%A6sehastighed.pdf
// http://uvm.dk/~/media/UVM/Filer/Stat/PDF12/120124%20Metode%20bag%20elevfremskrivningen.ashx
// (2)3 - 5 klasse ((8)9-11)
//LIX mellem (10-15)-(25-30) og med en læsehastighed på mellem (80-120) ord/minut til (180-200) ord/minut 
//LIX er en forkortelse for læsbarhedsindeks. Det er et mål for, hvor let teksten er at læse.
//Udregningen tager udgangspunkt i antallet af lange ord (over seks bogstaver) og længden af sætningerne (antal ord imellem hvert punktum).
//http://en.wikipedia.org/wiki/LIX

public class Utterance{
	
	private float duration;
	private float silence;

	public string Sentence { get; set; }
	public float Duration { get; set; }
	public float Silence { 
		get { 
			if(silence >= 0) 
				return Random.Range(0.1f,1.2f);
			else 
				return silence;
		} 
		set { silence = value; } 
	}

	string vowel;
	string consonant;

	public float CalculatedDuration()
	{
		int LIX = Utilities.LIX(this.Sentence);
		int wordCount = Utilities.CountWords(this.Sentence);
		int percentageOfLongWords = Utilities.PercentageOfLongWords(this.Sentence);

		Debug.Log(" Word Count = " + wordCount + " - % of Long words = " + percentageOfLongWords +  " - LIX = " + LIX);

		// Number of words * 60/(words/min)

		if(LIX == 0)
		{
			if(percentageOfLongWords == 0)
			{
				return (wordCount * 0.75f); // based on 80 words min
			}
			else if(percentageOfLongWords < 10)
			{
				return (wordCount * 0.86f); // based on 70 words min
			}
			else if(percentageOfLongWords < 20)
			{
				return (wordCount * 0.9f); // based on 65 words min
			}
			else if(percentageOfLongWords < 30)
			{
				return (wordCount * 1f); // based on 60 words min
			}
			else if(percentageOfLongWords < 40)
			{
				return (wordCount * 1.1f); // based on 50 words min
			}
			else if(percentageOfLongWords < 50)
			{
				return (wordCount * 1.5f); // based on 40 words min
			}
			else
			{
				if(wordCount < 2)
					return (wordCount * 3f); // based on 40 words min
				else
					return (wordCount * 2f); // based on 30 words min

			}
		}
		else if(LIX < 5)
		{
			return (wordCount * 0.86f); // based on 70 words min
		}
		else if(LIX < 10)
		{
			return (wordCount * 0.9f); // based on 65 words min
		}
		else if(LIX < 15)
		{
			return (wordCount * 1f); // based on 60 words min
		}
		else if(LIX < 20)
		{
			return (wordCount * 1.1f); // based on 50 words min
		}
		else if(LIX < 25)
		{
			return (wordCount * 1.5f); // based on 40 words min
		}
		else if(LIX < 30)
		{
			return (wordCount * 1.7f); // based on 35 words min
		}
		else
		{
			return (wordCount * 2f); // based on 30 words min
		}
	}

	public int FontSize()
	{
		int charCount = Utilities.CountNonSpaceChars(this.Sentence);
		int longWords = Utilities.CountLongWords(this.Sentence,12);
		
		if(charCount < 5)
		{
			if(longWords < 1)
				return 110;
			else
				return 75;
		}
		else if(charCount < 10)
		{
			if(longWords < 1)
				return 80;
			else
				return 45;
		}
		else if(charCount < 15)
		{
			if(longWords < 1)
				return 68;
			else
				return 45;
		}
		else if(charCount < 20)
			if(longWords < 1)
				return 64;
			else
				return 45;
		else if(charCount < 25)
		{
			if(longWords < 1)
				return 61;
			else
				return 45;
		}
		else if(charCount < 40)
		{
			if(longWords < 1)
				return 58;
			else
				return 45;
		}
		else if(charCount < 60)
		{
			if(longWords < 1)
				return 55;
			else
				return 45;
		}
		else
		{
			if(longWords < 1)
				return 50;
			else
				return 45;
		}
	}
}
