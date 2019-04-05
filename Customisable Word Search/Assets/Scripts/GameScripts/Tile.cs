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
	private SpriteRenderer renderer;
	private Color prevColor = Color.white;
	public Vector2 gridPos;
	public List<Tile> adjacentTiles = new List<Tile>();

	// Start is called before the first frame update
	void Start()
	{
		collider = GetComponent<Collider2D>();
		renderer = GetComponent<SpriteRenderer>();
		textMesh = GetComponentInChildren<TMPro.TextMeshPro>();
		letter = textMesh.text; //REMOVE when grid generator is built
		GetAdjacent();
	}

	// Update is called once per frame
	void Update()
	{

	}

	// Assign letter to display
	void SetTile (string ltr)
	{
		letter = ltr;
		textMesh.text = letter;
	}

	public void Select()
	{
		isSelected = !isSelected;
		if (isSelected)
		{
			renderer.color = Color.red;
		}
		else
		{
			renderer.color = Color.white;
		}
	}

	public void Select(bool state)
	{
		isSelected = state;
		if (isSelected)
		{
			renderer.color = Color.red;
		}
		else
		{
			renderer.color = prevColor;
		}
	}

	public void ToggleCorrectState()
	{
		isCorrect = true;
		renderer.color = Color.green;
		prevColor = Color.green;
	}

	void GetAdjacent()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);

		for(int i = 0; i < colliders.Length; i++)
		{
			if(colliders[i].CompareTag("Tile"))
			{
				Tile collider = colliders[i].GetComponent<Tile>();
				if(collider != this)
				{
					adjacentTiles.Add(collider);
				}
			}
		}
	}
}