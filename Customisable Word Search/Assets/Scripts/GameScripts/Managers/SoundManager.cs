using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	private GameManager gameManager;

	[SerializeField]
	private BoolVariable isSoundOn;

	[SerializeField]
	private List<AudioClip> clips = new List<AudioClip>();

	[SerializeField]
	private List<AudioSource> audioSources = new List<AudioSource>();

	// Start is called before the first frame update
	void Start()
    {
		gameManager = gameObject.GetComponent<GameManager>();
		if (!PlayerPrefs.HasKey("SoundSetting"))
		{
			PlayerPrefs.SetInt("SoundSetting", 1);
		}
		isSoundOn.State = (PlayerPrefs.GetInt("SoundSetting") == 1);
		SetSoundState();
    }

	public void ToggleSoundState()
	{
		isSoundOn.State = !isSoundOn.State;
		SetSoundState();
	}

	public void SetSoundState()
	{
		
		for (int i = 0; i < audioSources.Count; i++)
		{
			audioSources[i].enabled = isSoundOn.State;
		}
		PlayerPrefs.SetInt("SoundSetting", isSoundOn.State == true ? 1 : 0);
	}

	public void PlayWordSound(bool isCorrect)
	{
		if (isCorrect)
		{
			audioSources[1].clip = clips[1];
		}
		else
		{
			audioSources[1].clip = clips[2];
		}
		audioSources[1].volume = 0.2f;
		audioSources[1].Play();
	}

	public void PlaySound(AudioClip clip)
	{
		audioSources[1].clip = clip;
		int idx = clips.IndexOf(clip);
		if(idx == 0 || idx == 5)
		{
			audioSources[1].volume = 0.5f;
		}
		else
		{
			audioSources[1].volume = 0.2f;
		}
		audioSources[1].Play();
	}
}
