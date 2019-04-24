using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{
	private string letter;
	private bool isSelected;
	private TMPro.TextMeshProUGUI textMesh;
	private Collider2D col;

	//private int xCoord;
	//private int yCoord;
	public float width;

	// Update is called once per frame
	void Update()
	{

	}

	// Assign letter to display
	public void SetTile (string ltr)
	{
		if(col == null)
		{
			col = GetComponent<Collider2D>();
			textMesh = GetComponentInChildren<TMPro.TextMeshProUGUI>();
		}
		letter = ltr;
		if(textMesh == null)
		{
			Debug.Log("Broken");
		}
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

	//public void ToggleCorrectState()
	//{
	//	isCorrect = true;
	//}

	//public void SetCoords(int x, int y)
	//{
	//	xCoord = x;
	//	yCoord = y;
	//}

	public string GetLetter()
	{
		return letter;
	}
}