using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordListDisplay : MonoBehaviour
{
	[SerializeField]
	private WordList wordList;
	private TMPro.TextMeshPro display;


	public void SetDisplay(WordList words)
	{
		Component[] columns = GetComponentsInChildren<TMPro.TextMeshPro>();
		foreach (TMPro.TextMeshPro column in columns)
		{
			if (column.transform.position.x < 0.0f)
			{
				display = column;
				break;
			}
		}
		wordList = words;
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		display.text = "";
		for(int i = 0; i < wordList.words.Count; i++)
		{
			if (wordList.words[i].isFound)
			{
				display.text += "<s>" + wordList.words[i].value + "<s>\n";
			}
			else
			{
				display.text += wordList.words[i].value + "\n";
			}
		}
	}
}
