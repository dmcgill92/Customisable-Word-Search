using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{
	private string letter;
	private bool isSelected;
	private bool isCorrect;
	private TMPro.TextMeshPro textMesh;
	private Collider2D collider;

	private int xCoord;
	private int yCoord;

	// Update is called once per frame
	void Update()
	{

	}

	// Assign letter to display
	public void SetTile (string ltr)
	{
		if(collider == null)
		{
			collider = GetComponent<Collider2D>();
			textMesh = GetComponentInChildren<TMPro.TextMeshPro>();
		}
		letter = ltr;
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

	public void ToggleCorrectState()
	{
		isCorrect = true;
	}

	public void SetCoords(int x, int y)
	{
		xCoord = x;
		yCoord = y;
	}

	public string GetLetter()
	{
		return letter;
	}
}