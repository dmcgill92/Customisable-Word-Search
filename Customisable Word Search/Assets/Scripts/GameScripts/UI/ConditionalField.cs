using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(TMPro.TMP_InputField))]
public class ConditionalField : MonoBehaviour
{
	[SerializeField]
	private BoolVariable condition;
	private TMPro.TMP_InputField field;
    // Start is called before the first frame update
    
	void Start()
	{
		field = GetComponent<TMPro.TMP_InputField>();
		var listener = GameEventListener.AddGameEventListener(gameObject, condition.GetToggleEvent());
		if(listener != null)
		{
			listener.Response = new UnityEvent();
			listener.Response.AddListener(delegate { ToggleFieldState(); });
		}
		ToggleFieldState();
	}

	public void ToggleFieldState()
	{
		field.interactable = condition.State;
	}
}
