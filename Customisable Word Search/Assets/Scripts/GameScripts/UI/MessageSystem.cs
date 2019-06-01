using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystem : MonoBehaviour
{
	[SerializeField]
	private string connectionIssueMessage;

	[SerializeField]
	private string listGenerationFailMessage;

	[SerializeField]
	private string listGenerationSuccessMessage;

	[SerializeField]
	private string emptySearchTermMessage;

	private TMPro.TextMeshProUGUI messageText;

	// Start is called before the first frame update
	void Start()
    {
		messageText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetMessage(int num)
	{
		StopAllCoroutines();
		switch(num)
		{
			case 0:
				messageText.text = string.Empty;
				break;
			case 1:
				messageText.text = connectionIssueMessage;
				StartCoroutine("ClearMessage");
				break;
			case 2:
				messageText.text = listGenerationFailMessage;
				StartCoroutine("ClearMessage");
				break;
			case 3:
				messageText.text = listGenerationSuccessMessage;
				StartCoroutine("ClearMessage");
				break;
			case 4:
				messageText.text = emptySearchTermMessage;
				StartCoroutine("ClearMessage");
				break;
			default:
				messageText.text = string.Empty;
				break;
		}
	}

	public IEnumerator ClearMessage()
	{
		float timer = 0.0f;
		while(timer < 2.0f)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		SetMessage(0);
	}
}
