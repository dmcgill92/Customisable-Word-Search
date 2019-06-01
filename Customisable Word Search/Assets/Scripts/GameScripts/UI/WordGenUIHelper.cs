using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WordGenerator;

public class WordGenUIHelper : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField keywordField;
	[SerializeField] private BoolVariable foundWords;
	private UIManager uiManager;
	[SerializeField] private LoadingSpinner loader;
	[SerializeField] private MessageSystem message;

    private List<string> results = new List<string>();

	void Start()
	{
		uiManager = GetComponent<UIManager>();
	}

    public void Search()
    {
        string keyword = keywordField.text;
		foundWords.State = false;

        if (string.IsNullOrEmpty(keyword))
        {
            Debug.Log("keyword cannot be empty");
			message.SetMessage(4);
			loader.StopLoad();
            return;
        }

		StartCoroutine(SearchForTerms(keyword));
	}

	private IEnumerator SearchForTerms(string keyword)
	{
		int statusCode = -1;
		StartCoroutine(WordListManager.Instance.GetResponseStatusCode(keyword, response => { statusCode = response; }));
		yield return new WaitUntil(() => statusCode != -1);
		if (statusCode != 200)
		{
			message.SetMessage(2);
			loader.StopLoad();
			yield break;
		}
		else
			Debug.Log("Connection established.");

		float startTime = Time.realtimeSinceStartup;
		StartCoroutine(WordListManager.Instance.GetWordList(keyword, res => { results = res; }));
		yield return new WaitUntil(() => (results != null));
		Debug.Log("Search finished");
		float step1Time = Time.realtimeSinceStartup - startTime;
		Debug.Log(string.Format("Term Retrieval: {0}", step1Time));

		if(results[0] != "Word List Generated")
		{
			message.SetMessage(2);
		}
		else
		{
			message.SetMessage(3);
		}

		keywordField.text = results[1];
        results.RemoveAt(0);
		results.RemoveAt(0);
		GenWordList();
		foundWords.State = true;
		yield return null;
	}

	private void GenWordList()
	{
		results = results.GetRange(0, 50);
		ShuffleList(results);
		results = results.GetRange(0, 30);

		WordList wordList = new WordList();
		wordList.theme = keywordField.text;
		for( int i = 0; i < results.Count; i++)
		{
			Word word = new Word();
			word.value = results[i].ToUpper();
			wordList.words.Add(word);
		}
		uiManager.SetTheme(wordList);
	}

	public void ShuffleList(List<string> arr)
	{
		for (int i = 0; i < arr.Count - 1; i++)
		{
			int r = Random.Range(0, i);
			string tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}
}
