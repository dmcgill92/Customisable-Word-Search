using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public string letter;
	public bool isSelected;
	public bool isCorrect;
	public TMPro.TextMeshPro textMesh;

	// Start is called before the first frame update
	void Start()
	{
		textMesh = GetComponentInChildren<TMPro.TextMeshPro>();
		letter = textMesh.text; //REMOVE when grid generator is built
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
}