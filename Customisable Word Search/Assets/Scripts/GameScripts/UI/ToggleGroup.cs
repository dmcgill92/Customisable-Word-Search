using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroup : MonoBehaviour
{
	[SerializeField]
	private List<ToggleButton> buttons = new List<ToggleButton>();
	private List<Image> buttonImages = new List<Image>();

	[SerializeField]
	private ToggleButton currentButton;

	// Start is called before the first frame update
	void Start()
	{
		GetButtons();
	}

	public void GetButtons()
	{
		foreach (var button in buttons)
		{
			buttonImages.Add(button.GetComponent<Image>());
			button.SetMaster(this);
		}
	}

	public void AddButton(ToggleButton button)
	{
		buttons.Add(button);
	}

	public void ToggleButtons(ToggleButton onButton)
	{
		currentButton = onButton;
		for (int i = 0; i < buttons.Count; i++)
		{
			if (buttons[i] != onButton)
			{
				buttonImages[i].color = Color.white;
			}
			else
			{
				buttonImages[i].color = Color.green;
			}
		}
	}

	public void UntoggleAll()
	{
		currentButton = null;
		for (int i = 0; i < buttons.Count; i++)
		{
			buttonImages[i].color = Color.white;
		}
	}
}
