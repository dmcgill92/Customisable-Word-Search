using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LetterGrid : MonoBehaviour
{
	public GameObject tilePrefab;
	public List<List<Tile>> tiles = new List<List<Tile>>();
	[Range(5,12)]
	public int gridSize = 12;
	[Range(1, 10)]
	public int complexity = 4;
	private Vector2 gridCentre = new Vector2(0.0f, -2.2f);
	public BoolVariable isGenerated;
	[SerializeField]
	public List<List<string>> gridLetters = new List<List<string>>();
	private List<List<string>> tempLetters = new List<List<string>>();
	public WordList wordList;
	[SerializeField]
	private WordListDisplay display;
	[SerializeField]
	private GameEvent updateEvent;

	[SerializeField]
	private List<Tile> curTiles;
	public string curWord;

	[SerializeField]
	private GameObject line;

	[SerializeField]
	private FloatVariable spacing;
	[SerializeField]
	private FloatVariable diagSpacing;




	// Start is called before the first frame update
	void Start()
    {
		float time = Time.realtimeSinceStartup;
		SetWordList();
		GenerateGrid();
		time -= Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void GenerateGrid()
	{
		float time = Time.realtimeSinceStartup;
		CreateTiles();
		AssignLettersToTiles();
		FillInGrid();
		isGenerated.State = true;
		display.SetDisplay(wordList);
		time = Time.realtimeSinceStartup - time;
		Debug.Log("Execution time: " + time);
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

		ShuffleList(words);

		// Pass word list to be displayed
		wordList = ScriptableObject.CreateInstance<WordList>().Init(words, updateEvent);
	}

	public void ShuffleList(List<string> arr)
	{
		for(int i = 0; i< arr.Count -1; i++)
		{
			int r = Random.Range(0, i);
			string tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
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
					Instantiate(line, line.transform.parent, true);
					line.GetComponent<LineRenderer>().positionCount = 0;
					return;
				}
			}
		}
		line.GetComponent<LineRenderer>().positionCount = 0;
	}

	void CreateTiles()
	{
		Vector3 screenEdge = Camera.main.ScreenToWorldPoint(Vector3.zero);
		float dist = Mathf.Min(Mathf.Abs(screenEdge.x), Mathf.Abs(screenEdge.y));
		float step = (2 * dist) / (gridSize + 1);
		float offset = step * 0.5f;
		float xMin = gridCentre.x - dist + step;
		float yMin = gridCentre.y - dist + step;
		float scale = 12/(float)gridSize;

		LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
		lineRenderer.startWidth = 0.3f * scale;
		lineRenderer.endWidth = line.GetComponent<LineRenderer>().startWidth;
		lineRenderer.numCapVertices = 7 * (int)scale;

		for (int i = 0; i < gridSize; i++)
		{
			List<Tile> tempList = new List<Tile>();
			for (int j = 0; j < gridSize; j++)
			{
				
				GameObject tileObj = Instantiate(tilePrefab, new Vector2(xMin + j * step, yMin + i * step), Quaternion.identity, transform);
				int xCoord = j + 1;
				int yCoord = gridSize - i;
				tileObj.name = string.Format("Tile [{0},{1}]", xCoord, yCoord);
				tileObj.transform.localScale *= scale;
				Tile tile = tileObj.GetComponent<Tile>();
				tile.SetCoords(xCoord, yCoord);
				tempList.Add(tile);
			}
			tiles.Add(tempList);
		}
		spacing.Number = step;
		diagSpacing.Number = Vector2.Distance(new Vector2(0, step), new Vector2(step, 0));
	}

	void AssignLettersToTiles()
	{
		List<Word> sortedWords = new List<Word>(wordList.words);
		sortedWords.Sort(SortByLength);
		for(int i = 0; i < gridSize; i++)
		{
			List<string> tempList = new List<string>();
			for( int j = 0; j < gridSize; j++)
			{
				tempList.Add(string.Empty);
			}
			gridLetters.Add(tempList);
			tempLetters.Add(tempList);
		}

		int totalOverlap = 0;
		int highestOverlap = 0;

		List<Word> tempWordList = new List<Word>(wordList.words);
		float time = Time.realtimeSinceStartup;
		while(totalOverlap < complexity * ((float)gridSize/10) && Time.realtimeSinceStartup - time < 1.0f)
		{
			ClearList();
			wordList.words = new List<Word>(tempWordList);
			totalOverlap = 0;
			int overlap = 0;
			for(int i = sortedWords.Count - 1; i >= 0; i--)
			{
				Word word = sortedWords[i];
				CheckGrid(word, out overlap);
				totalOverlap += overlap;
			}
			if(totalOverlap > highestOverlap)
			{
				gridLetters = tempLetters;
				highestOverlap = totalOverlap;
			}
		}

		Debug.Log(string.Format("Complexity Achieved: {0} of {1}", highestOverlap, complexity * ((float)gridSize / 10)));
		for (int i = 0; i < gridSize; i++)
		{
			for (int j = 0; j < gridSize; j++)
			{
				tiles[i][j].SetTile(gridLetters[i][j]);
			}
		}
	}

	int SortByLength(Word a, Word b)
	{
		 return a.value.Length.CompareTo(b.value.Length);
	}

	void CheckGrid(Word word, out int overlap)
	{
		overlap = 0;
		bool isPlaced = false;
		int i = 0;
		while (i < 100 && !isPlaced)
		{
			overlap = 0;
			int length = word.value.Length;
			Vector2 startPos = new Vector2(Random.Range(0, gridSize), Random.Range(0, gridSize));
			if (!tempLetters[(int)startPos.x][(int)startPos.y].Equals(word.value[0].ToString()) && !string.IsNullOrEmpty(tempLetters[(int)startPos.x][(int)startPos.y]) )
			{
				continue;
			}
			int direction = Random.RandomRange(0, 8);
			Vector2 dir = Vector2.zero;
			switch (direction)
			{
				case 0:
					dir = new Vector2(1, 0);
					break;
				case 1:
					dir = new Vector2(1, 1);
					break;
				case 2:
					dir = new Vector2(0, 1);
					break;
				case 3:
					dir = new Vector2(-1, 1);
					break;
				case 4:
					dir = new Vector2(-1, 0);
					break;
				case 5:
					dir = new Vector2(-1, -1);
					break;
				case 6:
					dir = new Vector2(0, -1);
					break;
				case 7:
					dir = new Vector2(1, -1);
					break;
				default:
					break;
			}

			Vector2 tempVec = startPos + dir * length;
			if(tempVec.x < gridSize && tempVec.x > 0 && tempVec.y < gridSize && tempVec.y > 0)
			{
				bool canFit = true;
				for(int j = 1; j < length; j++)
				{
					Vector2 tilePos = startPos + dir * j;
					if(tempLetters[(int)tilePos.x][(int)tilePos.y].Equals(word.value[j].ToString()) || string.IsNullOrEmpty(tempLetters[(int)tilePos.x][(int)tilePos.y]))
					{
						continue;
					}
					else
					{
						canFit = false;
						break;
					}
				}

				if(canFit)
				{
					for (int j = 0; j < length; j++)
					{
						Vector2 tilePos = startPos + dir * j;
						if(tempLetters[(int)tilePos.x][(int)tilePos.y].Equals(word.value[j].ToString()))
						{
							overlap++;
						}
						else
						{
							tempLetters[(int)tilePos.x][(int)tilePos.y] = word.value[j].ToString();
						}
					}
					isPlaced = true;
				}
			}
			i++;
		}

		if(i == 100)
		{
			wordList.words.Remove(word);
		}
	}

	void ClearList()
	{
		for (int i = 0; i < tempLetters.Count; i++)
		{
			for (int j = 0; j < tempLetters[i].Count; j++)
			{
				gridLetters[i][j] = "";
			}
		}
	}

	void FillInGrid()
	{
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		for (int i = 0; i < gridSize; i++)
		{
			for (int j = 0; j < gridSize; j++)
			{
				if(string.IsNullOrEmpty(gridLetters[i][j]))
				{
					int rand = Random.Range(0, alphabet.Length);
					string letter = alphabet[rand].ToString();
					gridLetters[i][j] = letter;
					tiles[i][j].SetTile(letter);
				}
			}
		}
	}
}
