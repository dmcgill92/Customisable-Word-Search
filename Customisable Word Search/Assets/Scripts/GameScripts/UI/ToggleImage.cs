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
	private bool toggle = true;
	private Image image;

    // Start is called before the first frame update
    void Start()
    {
		image = GetComponent<Image>();
    }

	public void Toggle()
	{
		toggle = !toggle;
		if (toggle)
			image.sprite = onSprite;
		else
			image.sprite = offSprite;
	}
}
