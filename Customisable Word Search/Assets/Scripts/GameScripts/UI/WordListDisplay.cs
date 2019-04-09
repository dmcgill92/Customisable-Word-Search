using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WordListDisplay : MonoBehaviour
{
	[SerializeField]
	private WordList wordList;
	[SerializeField]
	private bool showWords;
	[SerializeField]
	private TMPro.TextMeshProUGUI display;


	public void SetDisplay(WordList words)
	{
		Component[] columns = GetComponentsInChildren<TMPro.TextMeshProUGUI>();
		foreach (TMPro.TextMeshProUGUI column in columns)
		{
			Debug.Log(column.rectTransform.anchoredPosition.x);
			if (column.rectTransform.anchoredPosition.x < 0.0f)
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
		Debug.Log("UpdatingDisplay");
		display.text = "";
		for(int i = 0; i < wordList.words.Count; i++)
		{
			if (wordList.words[i].isFound.State)
			{
				
				display.text += "<s>" + wordList.words[i].value + "</s>\n";
			}
			else
			{
				if(showWords)
				{
					display.text += wordList.words[i].value + "\n";
				}
				else
				{
					display.text += ReplaceAll(wordList.words[i].value, "_ ") + "\n";
				}
			}
		}
	}

	public string ReplaceAll(string input, string target)
	{
		StringBuilder sb = new StringBuilder(input.Length);
		for (int i = 0; i < input.Length; i++)
		{
			sb.Append(target);
		}

		return sb.ToString();
	}
}
