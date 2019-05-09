using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	private GameManager gameManager;
	[SerializeField]
	private List<TMPro.TextMeshProUGUI> themeDisplays = new List<TMPro.TextMeshProUGUI>();

	[SerializeField]
	private StringVariable theme;

	[SerializeField]
	private List<GraphicRaycaster> raycasters = new List<GraphicRaycaster>();
	private GraphicRaycaster curCaster;
	private PointerEventData pointerEventData;
	private EventSystem eventSystem;

	private enum InputState { Start, Hold, End, Null };
	[SerializeField]
	private InputState state = InputState.Null;

	private enum UIScene { Main, Pause, Game, End };
	private UIScene scene = UIScene.Main;

	private Canvas curUI;
	[SerializeField]
	private List<Canvas> uiList = new List<Canvas>();
	private Canvas curMenu;
	[SerializeField]
	private List<Canvas> menus = new List<Canvas>();
	private int gameType;

	[SerializeField]
	private List<Transform> difficultySettings = new List<Transform>();

	[SerializeField]
	private List<Transform> customSettings = new List<Transform>();

	private enum Difficulty { Null, Easy, Medium, Hard, Custom };
	[SerializeField]
	private Difficulty difficulty;

	private bool isDrawing;
	private LineManager lineManager;

	[SerializeField]
	private GameObject themeUI;
	[SerializeField]
	private GameObject themeListParent;
	private List<GameObject> themeButtons = new List<GameObject>();
	public List<Button> submitButtons = new List<Button>();

	[SerializeField]
	private List<WordList> wordSets = new List<WordList>();
	private WordList curTheme;

	[SerializeField]
	private LetterGrid grid;
	private int complexity;
	private int gridSize;
	private bool showWords;

	public IntVariable coinTotal;
	[SerializeField]
	private List<TMPro.TextMeshProUGUI> coinDisplays = new List<TMPro.TextMeshProUGUI>();
	private float timer;
	public FloatVariable elapsedTime;
	public float time;
	[SerializeField]
	private List<TMPro.TextMeshProUGUI> timeDisplays = new List<TMPro.TextMeshProUGUI>();

	[SerializeField]
	private int gridScore;

	[SerializeField]
	private BoolVariable hasChosenTheme;

	// Start is called before the first frame update
	void Start()
	{
		gameManager = Camera.main.GetComponent<GameManager>();
		ChangeUI((int)scene);
		eventSystem = GetComponent<EventSystem>();
		lineManager = Camera.main.GetComponent<LineManager>();
		UpdateCoinDisplay(false);
		SetThemeList();
	}

	// Update is called once per frame
	void Update()
	{
		GetInput();
	}

	public void StartTimer()
	{
		elapsedTime.Number = 0.0f;
		timer = Time.realtimeSinceStartup;
		time = elapsedTime.Number;
	}

	public void PauseTimer()
	{
		timer = Time.realtimeSinceStartup - timer;
		elapsedTime.Number += timer;
		time = elapsedTime.Number;
		UpdateTimeDisplay();
	}


	public void ContinueTimer()
	{
		timer = Time.realtimeSinceStartup;
		time = elapsedTime.Number;
	}

	public void UpdateTimeDisplay()
	{
		int minutes = Mathf.FloorToInt(elapsedTime.Number / 60.0f);
		int seconds = Mathf.FloorToInt(elapsedTime.Number - minutes * 60);
		string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

		foreach (TMPro.TextMeshProUGUI timeDisplay in timeDisplays)
		{
			timeDisplay.text = niceTime;
		}
	}

	public void UpdateCoinDisplay( bool isEnd)
	{
		if(!PlayerPrefs.HasKey("Coins"))
		{
			PlayerPrefs.SetInt("Coins", 100);
		}
		coinTotal.Number = PlayerPrefs.GetInt("Coins");
		if(isEnd)
		{
			coinTotal.Number += gridScore;
		}
		for( int i = 0; i < coinDisplays.Count; i++)
		{
			if(i != coinDisplays.Count-1)
			{
				coinDisplays[i].text = coinTotal.Number.ToString("n0");
			}
			else
			{
				coinDisplays[i].text = gridScore.ToString("n0");
			}
		}
		PlayerPrefs.SetInt("Coins", coinTotal.Number);
	}

	void GetInput()
	{
		Vector2 position;
#if UNITY_EDITOR
		state = CheckClicks(out position);
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			state = CheckTouches(out position);
#endif

		if (state != InputState.Null)
		{
			pointerEventData = new PointerEventData(eventSystem);
			pointerEventData.position = position;
			List<RaycastResult> results = new List<RaycastResult>();

			curCaster.Raycast(pointerEventData, results);

			Transform hit = null;
			foreach (RaycastResult result in results)
			{
				if (result.gameObject.CompareTag("Tile"))
				{
					hit = result.gameObject.transform;
					break;
				}
				else
				if (result.gameObject.CompareTag("Button"))
				{
					if (state == InputState.Start)
					{
						break;
					}
				}
				else
				if (result.gameObject.CompareTag("Input"))
				{
					if (state == InputState.Start)
					{
						break;
					}
				}
			}

			if (hit != null && state == InputState.Start)
			{
				isDrawing = true;
				lineManager.StartDraw(hit.transform.GetComponent<Tile>());
				state = InputState.Null;
				return;
			}
			else
			{
				if (isDrawing)
				{
					if (state == InputState.Hold)
					{
						lineManager.UpdateDraw(position);
					}
					else
					if (state == InputState.End)
					{
						lineManager.EndDraw(position);
						isDrawing = false;
					}
				}
			}
		}
		else
		{
			state = InputState.Null;
			return;
		}
	}


	InputState CheckClicks(out Vector2 position)
	{
		if (Input.GetMouseButtonDown(0))
		{
			position = Input.mousePosition;
			return InputState.Start;
		}
		else
		if (Input.GetMouseButton(0))
		{
			position = Input.mousePosition;
			return InputState.Hold;
		}
		else
		if (Input.GetMouseButtonUp(0))
		{
			position = Input.mousePosition;
			return InputState.End;
		}
		else
		{
			position = Vector2.zero;
			return InputState.Null;
		}
	}

	InputState CheckTouches(out Vector2 position)
	{
		int nbTouches = Input.touchCount;

		if (nbTouches > 0)
		{
			for (int i = 0; i < nbTouches; i++)
			{
				Touch touch = Input.GetTouch(i);

				if (touch.phase == TouchPhase.Began)
				{
					position = touch.position;
					return InputState.Start;
				}
				else
				if (touch.phase == TouchPhase.Moved)
				{
					position = touch.position;
					return InputState.Hold;
				}
				else
				if (touch.phase == TouchPhase.Ended)
				{
					position = touch.position;
					return InputState.End;
				}
			}
		}
		position = Vector2.zero;
		return InputState.Null;
	}

	public Tile CheckForTile(Vector2 pos)
	{

		pos = Camera.main.WorldToScreenPoint(pos);
		pointerEventData = new PointerEventData(eventSystem);
		pointerEventData.position = pos;
		List<RaycastResult> results = new List<RaycastResult>();

		curCaster.Raycast(pointerEventData, results);
		foreach (RaycastResult result in results)
		{
			if (result.gameObject.CompareTag("Tile"))
			{
				return result.gameObject.GetComponent<Tile>();
			}
		}
		return null;
	}

	public void UpdateTheme()
	{
		foreach (TMPro.TextMeshProUGUI display in themeDisplays)
		{
			display.text = theme.Content;
		}
	}

	public void ChangeUI(int curScene)
	{
		scene = (UIScene)curScene;
		switch (scene)
		{
			case UIScene.Main:
				curCaster = raycasters[3];
				curUI = uiList[1];
				uiList[0].enabled = false;
				uiList[1].enabled = true;
				uiList[2].enabled = false;
				uiList[3].enabled = false;
				ChangeMenu(0);
				break;
			case UIScene.Pause:
				curCaster = raycasters[1];
				curUI = uiList[2];
				uiList[0].enabled = true;
				uiList[1].enabled = false;
				uiList[2].enabled = true;
				uiList[3].enabled = false;
				ChangeMenu(-1);
				break;
			case UIScene.Game:
				curCaster = raycasters[0];
				curUI = uiList[0];
				uiList[0].enabled = true;
				uiList[1].enabled = false;
				uiList[2].enabled = false;
				uiList[3].enabled = false;
				ChangeMenu(-1);
				break;
			case UIScene.End:
				curCaster = raycasters[2];
				curUI = uiList[3];
				uiList[0].enabled = false;
				uiList[1].enabled = false;
				uiList[2].enabled = false;
				uiList[3].enabled = true;
				ChangeMenu(-1);
				break;
			default:
				break;
		}
	}

	public void ChangeMenu(int index)
	{
		if (index != -1)
		{
			curMenu = menus[index];
			for (int i = 0; i < menus.Count; i++)
			{
				if (i != index)
				{
					menus[i].enabled = false;
				}
				else
				{
					menus[i].enabled = true;
					curCaster = raycasters[3 + i];
				}
			}
		}
		else
		{
			curMenu = null;
			for (int i = 0; i < menus.Count; i++)
			{
				menus[i].enabled = false;
			}
		}

		
	}

	public void SetGameType(bool isCustom)
	{
		if(isCustom)
		{
			gameType = 1;
		}
		else
		{
			gameType = 2;
		}
	}

	public void Back()
	{
		int index = menus.IndexOf(curMenu);
		if( index == 3)
		{
			curMenu = menus[gameType];
			index = gameType;
		}
		else
		{
			curMenu = menus[0];
			index = 0;
		}

		for (int i = 0; i < menus.Count; i++)
		{
			if (i != index)
			{
				menus[i].enabled = false;
			}
			else
			{
				curMenu = menus[i];
				menus[i].enabled = true;
				curCaster = raycasters[3 + i];
			}
		}
	}

	public void ResetTheme()
	{
		SetTheme(null);
	}

	public void SetThemeList()
	{
		ToggleGroup group = themeListParent.GetComponent<ToggleGroup>();
		wordSets.Sort(SortByAlphabet);
		for(int i = 0; i <= wordSets.Count; i++)
		{
			GameObject themeButton = Instantiate(themeUI, themeListParent.transform);
			if( i != wordSets.Count)
			{
				WordList list = wordSets[i];
				themeButton.name = string.Format("{0}Button", list.theme);
				themeButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = list.theme;
				themeButton.GetComponent<Button>().onClick.AddListener(delegate { SetTheme(list); });
			}
			else
			{
				themeButton.name = "Random Button";
				themeButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Random";
				themeButton.GetComponent<Button>().onClick.AddListener(delegate { RandomTheme(); });
				themeButton.transform.SetAsFirstSibling();
			}
			themeButtons.Add(themeButton);
			group.AddButton(themeButton.GetComponent<ToggleButton>());
		}
		group.GetButtons();
	}

	int SortByAlphabet(WordList a, WordList b)
	{
		return a.theme.CompareTo(b.theme);
	}

	public void RandomTheme()
	{
		WordList randTheme = wordSets[Random.Range(0, wordSets.Count)];
		SetTheme(randTheme);
	}

	public void SetTheme(WordList theme)
	{
		curTheme = theme;
		if( curTheme != null)
		{
			hasChosenTheme.State = true;
		}
		else
		{
			hasChosenTheme.State = false;
			gameType = 0;
		}		
	}

		public void SetDifficulty(int diff)
	{
		Difficulty difficulty = (Difficulty)diff;
		switch (difficulty)
		{
			case Difficulty.Null:
				foreach (Transform setting in customSettings)
				{
					setting.gameObject.SetActive(false);
				}

				complexity = 0;
				gridSize = 0;
				showWords = false;
				break;
			case Difficulty.Easy:
				if (this.difficulty == Difficulty.Custom)
				{
					foreach (Transform setting in customSettings)
					{
						setting.gameObject.SetActive(false);
					}
				}

				complexity = 2;
				gridSize = 5;
				showWords = true;
				break;
			case Difficulty.Medium:
				if (this.difficulty == Difficulty.Custom)
				{
					foreach (Transform setting in customSettings)
					{
						setting.gameObject.SetActive(false);
					}
				}

				complexity = 8;
				gridSize = 8;
				showWords = true;
				break;
			case Difficulty.Hard:
				if (this.difficulty == Difficulty.Custom)
				{
					foreach (Transform setting in customSettings)
					{
						setting.gameObject.SetActive(false);
					}
				}

				complexity = 10;
				gridSize = 10;
				showWords = false;
				break;
			case Difficulty.Custom:
				if(this.difficulty != difficulty)
				{
					foreach(Transform setting in customSettings)
					{
						setting.gameObject.SetActive(true);
					}
				}
				SetGridSize();
				SetComplexity();
				ToggleShowWords();
				break;
		}
		this.difficulty = difficulty;
	}

	public void ResetDifficulty()
	{
		SetDifficulty(0);
	}


	public void SetGridSize()
	{
		gridSize = (int)customSettings[0].GetComponent<Slider>().value;
		customSettings[0].Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = gridSize.ToString();
	}

	public void SetComplexity()
	{
		complexity = (int)customSettings[1].GetComponent<Slider>().value;
		customSettings[1].Find("Value").GetComponent<TMPro.TextMeshProUGUI>().text = complexity.ToString();
	}

	public void ToggleShowWords()
	{
		showWords = customSettings[2].GetComponent<Toggle>().isOn;
	}

	public void SetGridParams()
	{
		int tempScore = showWords ? 0 : 1;
		gridScore = gridSize + 2 * complexity + 10 * tempScore; 
		grid.GenerateGrid(curTheme, complexity, gridSize, showWords);
	}	
}
