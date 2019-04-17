using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Word
{
	public string value;
	public BoolVariable isFound;

	public void Init (GameEvent updateEvent)
	{
		isFound = ScriptableObject.CreateInstance<BoolVariable>().Init(updateEvent);
		isFound.State = false;
	}
}
