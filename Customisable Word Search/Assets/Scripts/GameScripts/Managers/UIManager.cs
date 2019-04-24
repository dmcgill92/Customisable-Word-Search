using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private TMPro.TextMeshProUGUI themeDisplay;

	[SerializeField]
	private StringVariable theme;

	public GraphicRaycaster raycaster;
	public PointerEventData pointerEventData;
	public EventSystem eventSystem;

	public enum InputState { Start, Hold, End, Null};
	public InputState state = InputState.Null;

	private bool isDrawing;
	private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
		raycaster = GetComponent<GraphicRaycaster>();
		eventSystem = GetComponent<EventSystem>();
		inputManager = Camera.main.GetComponent<InputManager>();
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

		if(state != InputState.Null)
		{
			pointerEventData = new PointerEventData(eventSystem);
			pointerEventData.position = position;
			List<RaycastResult> results = new List<RaycastResult>();

			raycaster.Raycast(pointerEventData, results);

			Transform hit = null;
			foreach(RaycastResult result in results)
			{
				if(result.gameObject.CompareTag("Tile"))
				{
					hit = result.gameObject.transform;
					break;
				}
				else
				if(result.gameObject.CompareTag("Button"))
				{
					if(state == InputState.Start)
					{
						break;
					}
				}
			}

			if(hit != null && state == InputState.Start)
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
		if(Input.GetMouseButton(0))
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

		raycaster.Raycast(pointerEventData, results);
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
		themeDisplay.text = theme.Content;
	}
}
