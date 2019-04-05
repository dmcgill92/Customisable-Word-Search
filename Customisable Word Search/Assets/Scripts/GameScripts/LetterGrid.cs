using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LetterGrid : MonoBehaviour
{
	public Tile tilePrefab;
	public List<Tile> tiles = new List<Tile>();
	public int height = 12;
	public int width = 12;
	public bool isGenerated;
	public List<List<string>> gridLetters = new List<List<string>>();
	public WordList wordList;
	[SerializeField]
	private WordListDisplay display;
	[SerializeField]
	private GameEvent updateEvent;

	[SerializeField]
	private List<Tile> curTiles;
	public string curWord;


    // Start is called before the first frame update
    void Start()
    {
		SetWordList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void GenerateGrid()
	{
		//CreateTiles()
		//PositionTiles()

	}

	void SetWordList()
	{
		string line;

		StreamReader sr = new StreamReader("Assets/Resources/WordList.txt");

		//Read the first line of text
		line = sr.ReadLine();
		List<string> words = new List<string>();
		//Continue to read until you reach end of file
		while (line != null)
		{
			// Add each word to list
			string word = line;
			words.Add(word);


			//Read the next line
			line = sr.ReadLine();
		}

		//close the file
		sr.Close();

		// Pass word list to be displayed
		wordList = ScriptableObject.CreateInstance<WordList>().Init(words, updateEvent);

		// REMOVE
		display.SetDisplay(wordList);
	}

	void SetGridLetters()
	{
		string line;

		StreamReader sr = new StreamReader("Assets/Resources/Grid.txt");

		//Read the first line of text
		line = sr.ReadLine();

		//Continue to read until you reach end of file
		while (line != null)
		{
			width = 0;
			// Split each line to separate letters
			string[] letters = line.Split(null);

			List<string> row = new List<string>(); 
			// Set each letter in grid
			for (int i = 0; i < letters.Length; i++)
			{
				row.Add(letters[i]);
				width++;
			}
			gridLetters.Add(row);

			//Read the next line
			line = sr.ReadLine();
			height++;
		}

		//close the file
		sr.Close();
	}

	public void SelectTiles(List<Tile> tiles)
	{
		if(curTiles.Count>0)
		{
			DeselectTiles();
			curTiles.Clear();
			curWord = string.Empty;
		}

		for (int i = 0; i < tiles.Count; i++)
		{
			Tile tile = tiles[i];
			tile.Select(true);
			curWord += tile.letter;
			curTiles.Add(tile);
		}
		CheckWord();
	}

	public void DeselectTiles()
	{
		for(int i = 0; i < curTiles.Count; i++)
		{
			if(curTiles[i])
			curTiles[i].Select(false);
		}
	}

	public void CheckWord()
	{
		Debug.Log("Checking words");
		List<Word> words = wordList.words;
		for(int i = 0; i < words.Count; i++)
		{
			Word word = words[i];
			if(!word.isFound.State)
			{
				if(curWord == word.value)
				{
					word.isFound.State = true;
					for( int j = 0; j < curTiles.Count; j++)
					{
						curTiles[j].ToggleCorrectState();
					}
					return;
				}
			}
		}
	}
}
