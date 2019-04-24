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
		RectTransform child = GetComponentInChildren<RectTransform>();
		Component[] columns = child.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
		foreach (TMPro.TextMeshProUGUI column in columns)
		{
			if (column.rectTransform.pivot.x == 1.0f)
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
				display.text += string.Format("<s color=#FF0000> {0} </s>\n", wordList.words[i].value);
			}
			else
			{
				if(showWords)
				{
					display.text += string.Format(" {0} \n", wordList.words[i].value);
				}
				else
				{
					display.text += string.Format(" {0} \n", ReplaceAll(wordList.words[i].value, "_ "));
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
