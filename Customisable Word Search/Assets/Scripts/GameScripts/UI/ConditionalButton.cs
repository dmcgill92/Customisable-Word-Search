using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class ConditionalButton : MonoBehaviour
{
	[SerializeField]
	private BoolVariable condition;
	private Button button;
    // Start is called before the first frame update
    
	void Start()
	{
		button = GetComponent<Button>();
		var listener = GameEventListener.AddGameEventListener(gameObject, condition.GetToggleEvent());
		if(listener != null)
		{
			listener.Response = new UnityEvent();
			listener.Response.AddListener(delegate { ToggleButtonState(); });
		}
		ToggleButtonState();
	}

	public void ToggleButtonState()
	{
		button.interactable = condition.State;
	}
}
