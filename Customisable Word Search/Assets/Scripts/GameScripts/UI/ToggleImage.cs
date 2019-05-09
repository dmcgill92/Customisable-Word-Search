using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
	[SerializeField]
	private Sprite onSprite;
	[SerializeField]
	private Sprite offSprite;
	[HideInInspector]
	public bool toggle = true;
	private Image image;
	[SerializeField]
	private BoolVariable condition;

    // Start is called before the first frame update
    void Start()
    {
		if (image == null)
		{
			image = GetComponent<Image>();
		}
	}

	public void Toggle()
	{
		if(image == null)
		{
			image = GetComponent<Image>();
		}
		toggle = condition.State;
		if (toggle)
			image.sprite = onSprite;
		else
			image.sprite = offSprite;
	}
}
