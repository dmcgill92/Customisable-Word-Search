using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSpinner : MonoBehaviour
{
	private Image image;

	[SerializeField]
	private List<Sprite> frames = new List<Sprite>();

	private int maxFrames;

	private int currentFrame = 0;
	private bool isSpinning;
    // Start is called before the first frame update
    void Start()
    {
		maxFrames = frames.Count - 1;
		image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void StartLoad()
	{
		if(!isSpinning)
		{
			image.enabled = true;
			InvokeRepeating("Loading", 0.0f, 0.1f);
			isSpinning = true;
		}
	}

	void Loading()
	{
		currentFrame++;
		currentFrame = (int)Mathf.Repeat(currentFrame, maxFrames);
		image.sprite = frames[currentFrame];
	}

	public void StopLoad()
	{
		CancelInvoke("Loading");
		image.enabled = false;
		isSpinning = false;
	}
}
