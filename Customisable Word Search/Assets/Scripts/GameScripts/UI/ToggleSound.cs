using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleSound : MonoBehaviour
{
	private SoundManager soundManager;
	[SerializeField]
	private AudioClip onSound;
	[SerializeField]
	private AudioClip offSound;

	private Toggle toggleScript;

	void Awake()
	{
		soundManager = Camera.main.GetComponent<SoundManager>();
		toggleScript = GetComponent<Toggle>();
		toggleScript.onValueChanged.AddListener(delegate { PlayToggleSound(toggleScript.isOn); });
	}

	public void PlayToggleSound(bool isOn)
	{
		if(isOn)
		{
			soundManager.PlaySound(onSound);
			
		}
		else
		{
			soundManager.PlaySound(offSound);
		}
	}
}