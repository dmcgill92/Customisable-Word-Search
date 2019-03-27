using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WordList
{
	public List<Word> words = new List<Word>();

	public WordList(List<string> wordList)
	{

		// Add words to text mesh objects
		for(int i = 0; i < wordList.Count; i++)
		{
			words.Add( new Word(wordList[i]));
		}
	}
}
