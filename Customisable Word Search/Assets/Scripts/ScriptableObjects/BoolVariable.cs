using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoolVariable : ScriptableObject
{
	[SerializeField]
	private GameEvent toggleEvent;

	[SerializeField]
	private bool state;

	public bool State
	{
		get
		{
			return state;
		}

		set
		{
			if( state == value) return;
			state = value;
			toggleEvent.Raise();
		}
	}

	public BoolVariable Init()
	{
		return this;
	}

	public BoolVariable Init(GameEvent eventToSet)
	{
		toggleEvent = eventToSet;
		return this;
	}

}
