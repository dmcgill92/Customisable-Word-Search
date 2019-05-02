using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
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

	public enum UIScene { Main, Pause, Game, End };
	public UIScene scene = UIScene.Main;

	private Canvas curUI;
	[SerializeField]
	private List<Canvas> uiList = new List<Canvas>();
	[SerializeField]
	private Transform curMenu;
	[SerializeField]
	private List<Transform> menus = new List<Transform>();
	[SerializeField]
	private int gameType;

	[SerializeField]
	private List<Transform> difficultySettings = new List<Transform>();

	[SerializeField]
	private List<Transform> customSettings = new List<Transform>();

	public enum Difficulty { Null, Easy, Medium, Hard, Custom };
	[SerializeField]
	private Difficulty difficulty;

	private bool isDrawing;
	private InputManager inputManager;

	[SerializeField]
	private GameObject themeUI;
	[SerializeField]
	private GameObject themeListParent;
	private List<GameObject> themeButtons = new List<GameObject>();
	public List<Button> submitButtons = new List<Button>();

	[SerializeField]
	private List<WordList> wordSets = new List<WordList>();
	[SerializeField]
	private WordList curTheme;

	[SerializeField]
	private LetterGrid grid;
	private int complexity;
	private int gridSize;
	private bool showWords;

	// Start is called before the first frame update
	void Start()
	{
		ChangeUI((int)scene);
		eventSystem = GetComponent<EventSystem>();
		inputManager = Camera.main.GetComponent<InputManager>();
		SetThemeList();
	}

	// Update is called once per frame
	void Update()
	{
		GetInput();
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
				inputManager.StartDraw(hit.transform.GetComponent<Tile>());
				state = InputState.Null;
				return;
			}
			else
			{
				if (isDrawing)
				{
					if (state == InputState.Hold)
					{
						inputManager.UpdateDraw(position);
					}
					else
					if (state == InputState.End)
					{
						inputManager.EndDraw(position);
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
				curCaster = raycasters[1];
				curUI = uiList[1];
				uiList[0].enabled = false;
				uiList[1].enabled = true;
				uiList[2].enabled = false;
				ChangeMenu(0);
				break;
			case UIScene.Pause:
				curCaster = raycasters[2];
				curUI = uiList[2];
				uiList[0].enabled = true;
				uiList[1].enabled = false;
				uiList[2].enabled = true;
				break;
			case UIScene.Game:
				curCaster = raycasters[0];
				curUI = uiList[0];
				uiList[0].enabled = true;
				uiList[1].enabled = false;
				uiList[2].enabled = false;
				break;
			//case UIScene.End:
			//	curCaster = raycasters[3];
			//	curUI = uiList[3];
			//	uiList[0].enabled = true;
			//	uiList[1].enabled = false;
			//	uiList[2].enabled = false;
			//	break;
			default:
				break;
		}
	}

	public void ChangeMenu(int index)
	{
		curMenu = menus[index];
		for (int i = 0; i < menus.Count; i++)
		{
			if (i != index)
			{
				menus[i].gameObject.SetActive(false);
			}
			else
			{
				menus[i].gameObject.SetActive(true);
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
				menus[i].gameObject.SetActive(false);
			}
			else
			{
				curMenu = menus[i];
				menus[i].gameObject.SetActive(true);
			}
		}
	}

	public void ResetTheme()
	{
		SetTheme(null);
	}

	public void SetThemeList()
	{
		for(int i = 0; i < wordSets.Count; i++)
		{
			GameObject themeButton = Instantiate(themeUI, themeListParent.transform);
			WordList list = wordSets[i];
			themeButton.name = string.Format("{0}Button", list.theme);
			themeButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = list.theme;
			themeButton.GetComponentInChildren<Button>().onClick.AddListener(delegate { SetTheme(list); });
			themeButtons.Add(themeButton);
		}
	}

	public void SetTheme(WordList theme)
	{
		curTheme = theme;
		int idx = 0;
		if( curTheme != null)
		{
			idx = wordSets.IndexOf(curTheme);
			if (!submitButtons[gameType - 1].GetComponent<Button>().interactable)
			{
				submitButtons[gameType - 1].GetComponent<Button>().interactable = true;
			}
		}
		else
		{
			submitButtons[0].GetComponent<Button>().interactable = false;
			submitButtons[1].GetComponent<Button>().interactable = false;
			gameType = 0;
			idx = -1;
		}

		for(int i = 0; i < themeButtons.Count; i++)
		{
			if(i != idx)
			{
				themeButtons[i].GetComponent<Image>().color = Color.white;
			}
			else
			{
				themeButtons[i].GetComponent<Image>().color = Color.green;
			}
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

				foreach (Transform difficultyButton in difficultySettings)
				{
					difficultyButton.GetComponent<Image>().color = Color.white;
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

				if (this.difficulty != difficulty)
				{ 
					foreach (Transform difficultyButton in difficultySettings)
					{
						if (difficultyButton.name == "EasyButton")
						{
							difficultyButton.GetComponent<Image>().color = Color.green;
						}
						else
						{
							difficultyButton.GetComponent<Image>().color = Color.white;
						}
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


				if (this.difficulty != difficulty)
				{
					foreach (Transform difficultyButton in difficultySettings)
					{
						if (difficultyButton.name == "MediumButton")
						{
							difficultyButton.GetComponent<Image>().color = Color.green;
						}
						else
						{
							difficultyButton.GetComponent<Image>().color = Color.white;
						}
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

				if (this.difficulty != difficulty)
				{
					foreach (Transform difficultyButton in difficultySettings)
					{
						if (difficultyButton.name == "HardButton")
						{
							difficultyButton.GetComponent<Image>().color = Color.green;
						}
						else
						{
							difficultyButton.GetComponent<Image>().color = Color.white;
						}
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

					foreach (Transform difficultyButton in difficultySettings)
					{
						if (difficultyButton.name == "CustomButton")
						{
							difficultyButton.GetComponent<Image>().color = Color.green;
						}
						else
						{
							difficultyButton.GetComponent<Image>().color = Color.white;
						}
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
		grid.GenerateGrid(curTheme, complexity, gridSize, showWords);
	}
}
