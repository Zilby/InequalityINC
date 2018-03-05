using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the main menu class. 
/// </summary>
public class MainMenu : MonoBehaviour
{
	public TMP_InputField nameField;
	public TextMeshProUGUI namePlaceholder;

	public Button playButton;
	public Button controlsButton;
	public Button controlsScreenButton;

	/// <summary>
	/// The controls page. 
	/// </summary>
	public FadeableUI controls;
	private Text logField;
    private Toggle logToggle;


	private string characterName; // Fields for the player name.

	// Initialization boilerplate stuff
	void Start()
	{
		nameField.onEndEdit.AddListener((string arg) => NameUpdated(arg));
		namePlaceholder.text = Stats.PlayerName;

		playButton.onClick.AddListener(ChangeScene);
		controlsButton.onClick.AddListener(ToggleControls);
		controlsScreenButton.onClick.AddListener(ToggleControls);

		characterName = Stats.PlayerName;

		// Catalog and hide the Controls page
		controls.Hide();

		// Set default filepath for log file
		logField = GameObject.Find("LogText").GetComponent<Text>();
		Text logPlaceholder = logField.transform.parent.Find("Placeholder").GetComponent<Text>();
        logPlaceholder.text = Logger.DEFAULT_FILENAME;
        GameObject.Find("OutputField").SetActive(false);

		
        logToggle = GameObject.Find ("LoggingToggle").GetComponent<Toggle> ();
        logToggle.isOn = false;
	}

	// Called when the player name is updated from the default.
	public void NameUpdated(string newName)
	{
		if (newName == "")
			characterName = namePlaceholder.text;
		else
			characterName = newName;
	}

	/// <summary>
	/// Changes the scene when the 'Play' button is pressed.
	/// </summary>
	public void ChangeScene()
	{
		Stats.PlayerName = characterName;
        if (logToggle.isOn) {
            Logger.LogActive = true;
            Logger.CreateLog ((logField.text == "") ? Logger.DEFAULT_FILENAME : logField.text);
        } 

        Logger.Log ("Game started. Player name: " + characterName + "\n");
		UnityEngine.SceneManagement.SceneManager.LoadScene(1); // Loads main game scene
	}

	/// <summary>
	/// Fades in and out the controls when the 'Controls' button is pressed. 
	/// </summary>
	public void ToggleControls()
	{
		if (!controls.IsFading)
		{
			if (controls.IsVisible)
			{
				controls.SelfFadeOut(dur: 0.15f);
			}
			else
			{
				controls.SelfFadeIn(dur: 0.15f);
			}
		}
	}
}
