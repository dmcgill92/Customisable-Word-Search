using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StringVariable : ScriptableObject, ISerializationCallbackReceiver
{
	[SerializeField]
	private GameEvent toggleEvent;

	[SerializeField]
	private string initialValue;

	[NonSerialized]
	private string content;

	public string Content
	{
		get
		{
			return content;
		}

		set
		{
			if (content == value) return;
			content = value;
			toggleEvent.Raise();
		}
	}

	public void OnAfterDeserialize()
	{
		content = initialValue;
	}

	public void OnBeforeSerialize() { }

	public StringVariable Init()
	{
		return this;
	}
}
