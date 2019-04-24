using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LetterGrid : MonoBehaviour
{
	[SerializeField]
	private GameObject tilePrefab;
	[SerializeField]
	private GameObject line;

	[Range(5, 12)]
	[SerializeField]
	private int gridSize = 12;
	[Range(1, 10)]
	[SerializeField]
	private int complexity = 4;
	[SerializeField]
	private BoolVariable isGenerated;
	[SerializeField]
	private List<List<Tile>> tiles = new List<List<Tile>>();
	private List<List<string>> gridLetters = new List<List<string>>();
	private List<List<string>> tempLetters = new List<List<string>>();
	private WordList wordList;

	[SerializeField]
	private WordListDisplay display;
	[SerializeField]
	private GameEvent updateEvent;

	private List<Tile> curTiles = new List<Tile>();
	private string curWord;

	[SerializeField]
	private FloatVariable spacing;
	[SerializeField]
	private FloatVariable diagSpacing;

	private GUIStyle style = new GUIStyle();

	[SerializeField]
	private StringVariable theme;
	[SerializeField]
	private List<WordList> wordSets = new List<WordList>();

	// Start is called before the first frame update
	void Start()
	{
		style.fontSize = 92;
		SetWordList();
		GenerateGrid();
		
	}

	void GenerateGrid()
	{
		float time = Time.realtimeSinceStartup;
		CreateTiles();
		AssignLettersToTiles();
		isGenerated.State = true;
		display.SetDisplay(wordList);
		StartCoroutine(GetSpacing());
		time = Time.realtimeSinceStartup - time;
		Debug.Log("Execution time: " + time);
	}

	void SetWordList()
	{
		int rand = Random.Range(0, wordSets.Count);

		// Pass word list to be displayed
		wordList = Instantiate(wordSets[rand]).Init(updateEvent);
		theme.Content = wordList.theme;
	}

	public void ShuffleList(List<string> arr)
	{
		for (int i = 0; i < arr.Count - 1; i++)
		{
			int r = Random.Range(0, i);
			string tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}

	public void SelectTiles(List<Tile> tiles)
	{
		if (curTiles.Count > 0)
		{
			DeselectTiles();
			curTiles.Clear();
			curWord = string.Empty;
		}

		for (int i = 0; i < tiles.Count; i++)
		{
			Tile tile = tiles[i];
			tile.Select(true);
			curWord += tile.GetLetter();
			curTiles.Add(tile);
		}
		CheckWord();
	}

	public void DeselectTiles()
	{
		for (int i = 0; i < curTiles.Count; i++)
		{
			if (curTiles[i])
				curTiles[i].Select(false);
		}
	}

	public void CheckWord()
	{
		List<Word> words = wordList.words;
		for (int i = 0; i < words.Count; i++)
		{
			Word word = words[i];
			if (!word.isFound.State)
			{
				if (curWord == word.value)
				{
					word.isFound.State = true;
					//for (int j = 0; j < curTiles.Count; j++)
					//{
					//	curTiles[j].ToggleCorrectState();
					//}
					GameObject newLine = Instantiate(line, line.transform.parent, true);
					newLine.name = "Line";
					line.GetComponent<LineRenderer>().positionCount = 0;
					return;
				}
			}
		}
		line.GetComponent<LineRenderer>().positionCount = 0;
	}

	void CreateTiles()
	{
		for (int i = 0; i < gridSize; i++)
		{
			List<Tile> tempList = new List<Tile>();
			var row = new GameObject();
			row.AddComponent<RectTransform>().SetParent(transform, false);
			row.name = string.Format("Row {0}", i+1);
			var layout = row.AddComponent<HorizontalLayoutGroup>();
			layout.childControlHeight = true;
			layout.childControlWidth = true;
			layout.childForceExpandHeight = true;
			layout.childForceExpandWidth = true;
			layout.spacing = 5;

			var element = row.AddComponent<LayoutElement>();
			element.flexibleHeight = 1;
			element.flexibleWidth = 1;

			for (int j = 0; j < gridSize; j++)
			{

				GameObject tileObj = Instantiate(tilePrefab, row.transform, false);
				int xCoord = j + 1;
				int yCoord = gridSize - i;
				tileObj.name = string.Format("Tile [{0},{1}]", xCoord, yCoord);

				Tile tile = tileObj.GetComponent<Tile>();
				//tile.SetCoords(xCoord, yCoord);
				tempList.Add(tile);
			}
			tiles.Add(tempList);
		}
	}

	public IEnumerator GetSpacing()
	{
		yield return new WaitForSeconds(0.3f);
		Vector2 point1 = tiles[0][0].GetComponent<RectTransform>().rect.position;
		Vector2 point2 = tiles[0][1].GetComponent<RectTransform>().rect.position;
		point1 = tiles[0][0].GetComponent<RectTransform>().TransformPoint(point1);
		point2 = tiles[0][1].GetComponent<RectTransform>().TransformPoint(point2);

		float dist = point2.x - point1.x;
		spacing.Number = dist;
		diagSpacing.Number = Mathf.Sqrt(Mathf.Pow(dist, 2) + Mathf.Pow(dist, 2));
	}

	void AssignLettersToTiles()
	{
		List<Word> sortedWords = new List<Word>(wordList.words);
		sortedWords.Sort(SortByLength);
		for (int i = 0; i < gridSize; i++)
		{
			List<string> tempList = new List<string>();
			List<string> temp2List = new List<string>();
			for (int j = 0; j < gridSize; j++)
			{
				tempList.Add(string.Empty);
				temp2List.Add(string.Empty);
			}
			gridLetters.Add(tempList);
			tempLetters.Add(temp2List);
		}

		int totalOverlap = 0;
		int highestOverlap = 0;

		List<Word> initWordList = new List<Word>(wordList.words);
		List<Word> savedWordList = new List<Word>(wordList.words);
		float time = Time.realtimeSinceStartup;
		while (totalOverlap < complexity * ((float)gridSize / 10) && Time.realtimeSinceStartup - time < 1.0f)
		{
			ClearList(tempLetters);
			wordList.words = new List<Word>(initWordList);
			totalOverlap = 0;
			int overlap = 0;
			for (int i = sortedWords.Count - 1; i >= 0; i--)
			{
				Word word = sortedWords[i];
				CheckGrid(word, out overlap);
				totalOverlap += overlap;
			}
			//Debug.Log(string.Format("Complexity: {0}", totalOverlap));
			if (totalOverlap > highestOverlap)
			{
				//Debug.Log(string.Format("Highest Complexity: {0}", totalOverlap));
				for (int i = 0; i < gridSize; i++)
				{
					for (int j = 0; j < gridSize; j++)
					{
						gridLetters[i][j] = tempLetters[i][j];
					}
				}

				savedWordList = new List<Word>(wordList.words);
				ClearList(tempLetters);
				highestOverlap = totalOverlap;
			}
		}

		wordList.words = new List<Word>(savedWordList);

		Debug.Log(string.Format("Complexity Achieved: {0} of {1}", highestOverlap, complexity * ((float)gridSize / 10)));
		for (int i = 0; i < gridSize; i++)
		{
			for (int j = 0; j < gridSize; j++)
			{
				if(string.IsNullOrEmpty(gridLetters[i][j]))
				{
					FillInGrid(i, j);
				}

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
			if (!tempLetters[(int)startPos.x][(int)startPos.y].Equals(word.value[0].ToString()) && !string.IsNullOrEmpty(tempLetters[(int)startPos.x][(int)startPos.y]))
			{
				continue;
			}
			int direction = Random.Range(0, 8);
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
			if (tempVec.x < gridSize && tempVec.x > 0 && tempVec.y < gridSize && tempVec.y > 0)
			{
				bool canFit = true;
				for (int j = 1; j < length; j++)
				{
					Vector2 tilePos = startPos + dir * j;
					if (tempLetters[(int)tilePos.x][(int)tilePos.y].Equals(word.value[j].ToString()) || string.IsNullOrEmpty(tempLetters[(int)tilePos.x][(int)tilePos.y]))
					{
						continue;
					}
					else
					{
						canFit = false;
						break;
					}
				}

				if (canFit)
				{
					for (int j = 0; j < length; j++)
					{
						Vector2 tilePos = startPos + dir * j;
						if (tempLetters[(int)tilePos.x][(int)tilePos.y].Equals(word.value[j].ToString()))
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

		if (i == 100)
		{
			wordList.words.Remove(word);
		}
	}

	void ClearList(List<List<string>> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			for (int j = 0; j < list[i].Count; j++)
			{
				list[i][j] = "";
			}
		}
	}

	void FillInGrid(int x, int y)
	{
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		int rand = Random.Range(0, alphabet.Length);
		string letter = alphabet[rand].ToString();
		gridLetters[x][y] = letter;	
	}
}
