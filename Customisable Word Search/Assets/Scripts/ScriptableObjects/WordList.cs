using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WordList : ScriptableObject
{
	public List<Word> words = new List<Word>();

	public WordList Init(List<string> wordList, GameEvent updateEvent)
	{
		// Add words to text mesh objects
		for (int i = 0; i < wordList.Count; i++)
		{
			words.Add( new Word(wordList[i], updateEvent));
		}
		return this;
	}
}
