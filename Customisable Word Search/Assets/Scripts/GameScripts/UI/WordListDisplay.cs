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
	private TMPro.TextMeshProUGUI displayLeft;
	[SerializeField]
	private TMPro.TextMeshProUGUI displayRight;

	public void SetDisplay(WordList words)
	{
		Component[] columns = GetComponentsInChildren<TMPro.TextMeshProUGUI>();
		foreach (TMPro.TextMeshProUGUI column in columns)
		{
			if (column.rectTransform.anchoredPosition.x < 0.0f)
			{
				displayLeft = column;
			}
			else
			{
				displayRight = column;
			}
		}
		wordList = words;
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		displayLeft.text = "";
		displayRight.text = "";
		for (int i = 0; i < wordList.words.Count; i++)
		{
			TMPro.TextMeshProUGUI display;
			if(i % 2 == 0)
			{
				display = displayLeft;
			}
			else
			{
				display = displayRight;
			}

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
