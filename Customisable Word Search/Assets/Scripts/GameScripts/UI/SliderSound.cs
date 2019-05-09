using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderSound : MonoBehaviour
{
	private SoundManager soundManager;
	[SerializeField]
	private AudioClip audioClip;

	void Awake()
	{
		soundManager = Camera.main.GetComponent<SoundManager>();
		GetComponent<Slider>().onValueChanged.AddListener(delegate { soundManager.PlaySound(audioClip); });
	}
}