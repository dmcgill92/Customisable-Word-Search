using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour
{
	private ToggleGroup master;

	void Toggle()
	{
		master.ToggleButtons(this);
	}

	public void SetMaster(ToggleGroup master)
	{
		this.master = master;
		GetComponent<Button>().onClick.AddListener(delegate { Toggle(); });
	}
}
