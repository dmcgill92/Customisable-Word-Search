using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private UIManager uiManager;

	[SerializeField]
	private LineManager lineManager;

	[SerializeField]
	private SoundManager soundManager;

	[SerializeField]
	private LetterGrid letterGrid;

	[SerializeField]
	private HintSystem hintSystem;

	[SerializeField]
	private ConnectionManager connectionManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PlayWordSound(bool isCorrect)
	{
		soundManager.PlayWordSound(isCorrect);
	}

	public void GridFinished()
	{
		uiManager.PauseTimer();
		uiManager.UpdateCoinDisplay(true);
		uiManager.ChangeUI(3);
	}

	public void GoToMainMenu()
	{
		uiManager.ChangeUI(0);
		uiManager.ChangeMenu(0);
		uiManager.ResetTheme();
		uiManager.ResetDifficulty();
		hintSystem.Reset();
		letterGrid.ClearGrid();
	}

	public void SendLocationsToHintSystem(Dictionary<Word, Vector2> locations, WordList wordList)
	{
		hintSystem.SetLetterLocations(locations, wordList);
	}

	public void CheckConnection()
	{
		connectionManager.StartConnectionCheck();
	}
}
