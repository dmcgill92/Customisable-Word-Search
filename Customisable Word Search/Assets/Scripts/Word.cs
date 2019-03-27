using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Word
{
	public string value;
	public bool isFound;

	public Word (string v)
	{
		value = v;
		isFound = false;
	}
}
