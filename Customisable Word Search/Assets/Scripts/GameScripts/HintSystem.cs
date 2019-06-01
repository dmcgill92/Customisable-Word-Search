using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintSystem : MonoBehaviour
{
	[SerializeField]
	private IntVariable coins;

	[SerializeField]
	private int cost;

	[SerializeField]
	private Dictionary<Word, Vector2> firstLetterLocations = new Dictionary<Word, Vector2>();

	[SerializeField]
	private WordList wordList;

	[SerializeField]
	private LetterGrid letterGrid;

	private Vector2 currentTileLocation;

	[SerializeField]
	private Image hintIcon;
	[SerializeField]
	private Image coinIcon;
	public int intervals = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private bool CheckForSufficientCoins()
	{
		if(coins.Number >= cost)
		{
			coins.Number -= cost;
			return true;
		}
		else
		{
			intervals = 0;
			InvokeRepeating("InsufficientCoins", 0.0f, 0.25f);
			return false;
		}
	}

	public void UseHint()
	{
		if(CheckForSufficientCoins())
		{
			Word tempWord = null;
			bool foundLetter = false;
			Vector2 location = Vector2.zero;
			int iterations = 0;
			Debug.Log(string.Format("wordlist length: {0}", wordList.words.Count));
			Debug.Log(string.Format("dictionary length: {0}", firstLetterLocations.Count));
			while (!foundLetter)
			{
				int rand = Random.Range(0, wordList.words.Count);
				Debug.Log(string.Format("Word selected is {0}", wordList.words[rand].value));
				if (!wordList.words[rand].isFound.State)
				{
					tempWord = wordList.words[rand];
					firstLetterLocations.TryGetValue(tempWord, out location);
					if( currentTileLocation != location)
					{
						foundLetter = true;
					}
				}
				iterations++;
			}
			letterGrid.HighlightTile(location, tempWord);
			currentTileLocation = location;
		}
	}

	public void SetLetterLocations(Dictionary<Word, Vector2> locations, WordList wordList)
	{
		firstLetterLocations = locations;
		this.wordList = wordList;
	}

	public void Reset()
	{
		currentTileLocation = -Vector2.one;
	}

	void InsufficientCoins()
	{
		if(intervals >= 5)
		{
			hintIcon.color = Color.white;
			coinIcon.color = Color.white;
			CancelInvoke("InsufficientCoins");
			return;
		}
		if(hintIcon.color == Color.red)
		{
			hintIcon.color = Color.white;
			coinIcon.color = Color.white;
		}
		else
		{
			hintIcon.color = Color.red;
			coinIcon.color = Color.red;
		}

		intervals++;
	}
}
