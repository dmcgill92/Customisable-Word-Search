﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
	[SerializeField]
	private GameEvent toggleEvent;

	[SerializeField]
	private float initialValue;

	[NonSerialized]
	public float number;

	public float Number
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

	public FloatVariable Init()
	{
		return this;
	}
}
