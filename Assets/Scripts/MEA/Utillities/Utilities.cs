using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public static class Utilities {

	private static string dateFormat = "dd/MM/yyyy";
	private static string timeFormat = "HH:mm:ss";
	private static string enumValue = "d";
	private static string enumChar = "g";
	
	public static string DateFormat { get { return dateFormat; } }
	public static string TimeFormat { get { return timeFormat; } }
	public static string EnumValue { get { return enumValue; } }
	public static string EnumChar { get {return enumChar; } }

	/// <summary>
	/// Generate System.random number.
	/// </summary>
	public static System.Random RandomNumber = new System.Random();

	/// <summary>
	/// Computes LIX number of the specified string.
	/// LIX = A/B + (C * 100)/A , where
	/// A = Number of words, B = Number of periods (defined by period, colon or capital first letter), C = Number of long words above 6 letters
	/// Source: http://www.readabilityformulas.com/the-LIX-readability-formula.php
	/// </summary>
	/// <param name="s">S.</param>
	public static int LIX(string s)
	{
		int b = CountPeriodsAndColon(s) + CountCapitalLetters(s);
		int c = CountLongWords(s);
		int a = CountWords(s);

		if(b > 0) //cannot devide by 0
			return a/b + (c * 100)/a;
		else
			return 0;
	}

	/// <summary>
	/// Computes word length (percentage of long words).
	/// </summary>
	/// <returns>The percentage of long words.</returns>
	/// <param name="s">S.</param>
	public static int PercentageOfLongWords(string s)
	{
		int c = CountLongWords(s);
		int a = CountWords(s);

		return (c * 100)/a;
	}

	/// <summary>
	/// Count words with Regex.
	/// Source: http://www.dotnetperls.com/word-count
	/// </summary>
	public static int CountWords(string s)
	{
		MatchCollection collection = Regex.Matches(s, @"[\S]+");
		return collection.Count;
	}

	/// <summary>
	/// Counts the periods.
	/// </summary>
	/// <returns>The periods.</returns>
	/// <param name="value">Value.</param>
	public static int CountPeriodsAndColon(string value)
	{
		int result = 0;

		foreach(char c in value)
		{
			if(c == '.')
				result++;
			if(c == ':')
				result++;
		}
		return result;
	}

	//Source: http://www.introprogramming.info/tag/counting-the-uppercase-lowercase-words-in-a-text/
	private static bool IsUpperCase(string word)	
	{
		bool result = word.Equals(word.ToUpper());
		return result;
	}

	//Source: http://www.introprogramming.info/tag/counting-the-uppercase-lowercase-words-in-a-text/
	private static bool IsLowerCase(string word)
	{
		bool result = word.Equals(word.ToLower());
		return result;
	}


	/// <summary>
	/// Counts long words above 6 is used in LIX calc which is used to adjust reading speed 
	/// </summary>
	/// <returns>Number of long words above 6 chars</returns>
	/// <param name="s">S.</param>
	public static int CountLongWords(string s)
	{
		int result = 0;
		string[] words = SplitWords(s);

		foreach (string word in words)
		{
			if(CountNonSpaceChars(word) > 6)
			{
				Debug.Log(word);
				result++;
			}
		}

		return result;
	}

	/// <summary>
	/// Counts the long words.
	/// Count long words is also used to set font-size if words are above some chars and line break if long words.
	/// and can be used to trigger dialog i.e. if words are above some chars: you can ask your parent what it means
	/// </summary>
	/// <returns>Number of long words above specified Char number</returns>
	/// <param name="s">String</param>
	/// <param name="charNum">Char number.</param>
	public static int CountLongWords(string s, int charNum)
	{
		int result = 0;
		string[] words = SplitWords(s);
		
		foreach (string word in words)
		{
			if(CountNonSpaceChars(word) > charNum)
			{
				Debug.Log(word);
				result++;
			}
		}
		
		return result;
	}
	/// <summary>
	/// Counts the capital letters.
	/// </summary>
	/// <returns>Number of capital letters.</returns>
	/// <param name="s">S.</param>
	public static int CountCapitalLetters(string s)
	{
		int result = 0;
		string[] words = SplitWords(s);

		foreach(string word in words)
		{
			if(IsUpperCase(word))
			{
				result++;
			}
		}

		return result;
	}

	/// <summary>
	/// Take all the words in the input string and separate them.
	/// Source: http://www.dotnetperls.com/split
	/// </summary>
	/// <returns>The words.</returns>
	/// <param name="s">S.</param>
	static string[] SplitWords(string s)
	{
		//
		// Split on all non-word characters.
		// ... Returns an array of all the words.
		//
		return Regex.Split(s, @"\W+");
		// @      special verbatim string syntax
		// \W+    one or more non-word characters together
	}

	/// <summary>
	/// Return the number of characters in a string using the same method
	/// as Microsoft Word 2007. Sequential spaces are not counted.
	/// Source: http://www.dotnetperls.com/count-characters
	/// </summary>
	/// <param name="value">String to count chars.</param>
	/// <returns>Number of chars in string.</returns>
	public static int CountChars(string value)
	{
		int result = 0;
		bool lastWasSpace = false;
		
		foreach (char c in value)
		{
			if (char.IsWhiteSpace(c))
			{
				// A.
				// Only count sequential spaces one time.
				if (lastWasSpace == false)
				{
					result++;
				}
				lastWasSpace = true;
			}
			else
			{
				// B.
				// Count other characters every time.
				result++;
				lastWasSpace = false;
			}
		}
		return result;
	}

	/// <summary>
	/// Counts the number of non-whitespace characters.
	/// It closely matches Microsoft Word 2007.
	/// Source: http://www.dotnetperls.com/count-characters
	/// </summary>
	/// <param name="value">String to count non-whitespaces.</param>
	/// <returns>Number of non-whitespace chars.</returns>
	public static int CountNonSpaceChars(string value)
	{
		int result = 0;
		foreach (char c in value)
		{
			if (!char.IsWhiteSpace(c))
			{
				result++;
			}
		}
		return result;
	}
}

