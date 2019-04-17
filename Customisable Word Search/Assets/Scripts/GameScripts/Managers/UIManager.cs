using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private TMPro.TextMeshProUGUI themeDisplay;

	[SerializeField]
	private StringVariable theme;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void UpdateTheme()
	{
		themeDisplay.text = theme.Content;
	}
}
