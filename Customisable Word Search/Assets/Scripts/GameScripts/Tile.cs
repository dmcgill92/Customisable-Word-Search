using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Tile : MonoBehaviour
{
	private string letter;
	private bool isSelected;
	private TMPro.TextMeshProUGUI textMesh;
	private Collider2D col;
	[SerializeField]
	private Image image;
	private Word word;
	private bool isHighlighted;

	//private int xCoord;
	//private int yCoord;
	public float width;

	// Update is called once per frame
	void Update()
	{
		if(isHighlighted)
		{
			if(word.isFound.State)
			{
				StopHighlight();
			}
		}
	}

	// Assign letter to display
	public void SetTile (string ltr)
	{
		if (image == null || textMesh == null)
		{
			foreach(Transform child in transform)
			{
				if(child.GetComponent<Image>())
				{
					image = child.GetComponent<Image>();
				}
				else
				if(child.GetComponent<TMPro.TextMeshProUGUI>())
				{
					textMesh = child.GetComponent<TMPro.TextMeshProUGUI>();
				}
			}
		}
		letter = ltr;
		image.enabled = false;
		textMesh.text = letter;
	}

	public void Select()
	{
		isSelected = !isSelected;
	}

	public void Select(bool state)
	{
		isSelected = state;
	}

	public void SetHighlighted(Word word)
	{
		if(isHighlighted)
		{
			return;
		}

		this.word = word;
		textMesh.color = Color.green;
		isHighlighted = true;
		InvokeRepeating("Highlight", 0.05f, 0.5f);
	}

	public void Highlight()
	{
		image.enabled = !image.enabled;
	}

	public void StopHighlight()
	{
		CancelInvoke("Highlight");
		isHighlighted = false;
		word = null;
		textMesh.color = Color.white;
		image.enabled = false;
	}

	public string GetLetter()
	{
		return letter;
	}
}