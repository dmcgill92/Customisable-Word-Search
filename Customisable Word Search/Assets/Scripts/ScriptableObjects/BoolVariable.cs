using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoolVariable : ScriptableObject, ISerializationCallbackReceiver
{
	[SerializeField]
	private GameEvent toggleEvent;

	[SerializeField]
	private bool initialState;

	[NonSerialized]
	public bool state;

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
			if(toggleEvent != null)
			{
				toggleEvent.Raise();
			}
		}
	}

	public void OnAfterDeserialize()
	{
		state = initialState;
	}

	public void OnBeforeSerialize() { }

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
