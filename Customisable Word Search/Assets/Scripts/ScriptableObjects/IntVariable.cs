using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
{
	[SerializeField]
	private GameEvent toggleEvent;

	[SerializeField]
	private int initialValue;

	[NonSerialized]
	private int number;

	public int Number
	{
		get
		{
			return number;
		}

		set
		{
			number = value;
		}
	}

	public void OnAfterDeserialize()
	{
		number = initialValue;
	}

	public void OnBeforeSerialize() { }

	public IntVariable Init()
	{
		return this;
	}
}
