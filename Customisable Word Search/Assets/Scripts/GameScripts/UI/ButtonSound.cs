using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
	private SoundManager soundManager;
	[SerializeField]
	private AudioClip audioClip;

	void Awake()
	{
		soundManager = Camera.main.GetComponent<SoundManager>();
		GetComponent<Button>().onClick.AddListener(() => soundManager.PlaySound(audioClip));
	}
}