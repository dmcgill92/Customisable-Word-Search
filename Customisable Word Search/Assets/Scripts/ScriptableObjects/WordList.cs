using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class WordList : ScriptableObject
{
	public string theme;
	public List<Word> words = new List<Word>();

	public WordList Init(GameEvent updateEvent)
	{
		// Add words to text mesh objects
		for (int i = 0; i < words.Count; i++)
		{
			words[i].Init(updateEvent);
		}
		return this;
	}
}
