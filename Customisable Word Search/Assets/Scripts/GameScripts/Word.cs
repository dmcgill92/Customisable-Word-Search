using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Word
{
	public string value;
	public BoolVariable isFound;

	public Word (string v, GameEvent updateEvent)
	{
		isFound = ScriptableObject.CreateInstance<BoolVariable>().Init(updateEvent);
		value = v;
		isFound.State = false;
	}
}
