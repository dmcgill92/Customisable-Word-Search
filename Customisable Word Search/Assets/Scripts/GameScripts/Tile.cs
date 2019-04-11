using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{
	public string letter;
	public bool isSelected;
	public bool isCorrect;
	public TMPro.TextMeshPro textMesh;
	private Collider2D collider;

	public int xCoord;
	public int yCoord;
	// Start is called before the first frame update
	void Awake()
	{
		collider = GetComponent<Collider2D>();
		textMesh = GetComponentInChildren<TMPro.TextMeshPro>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	// Assign letter to display
	public void SetTile (string ltr)
	{
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
}