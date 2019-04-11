using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
	[SerializeField]
	private GameEvent toggleEvent;

	[SerializeField]
	private float number;

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

	public FloatVariable Init()
	{
		return this;
	}
}
