using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    private bool logActive = false; // Fields for the output log file.
    private string logOutput; 
    private Text logPlaceholder;

    private string characterName; // Fields for the player name.
    private Text namePlaceholder;

    private GameObject controls; // Controls Page gameObject.

    // Initialization boilerplate stuff
	void Awake () {
        // Set default player name in text field
        Text nameField = GameObject.Find("NameText").GetComponent<Text>();
        namePlaceholder = nameField.transform.parent.Find ("Placeholder").GetComponent<Text> ();
        namePlaceholder.text = Stats.PlayerName;
        characterName = Stats.PlayerName;

        // Catalog and hide the Controls page
        controls = GameObject.Find("Controls");
        controls.SetActive (false);

        // Set default filepath for log file
        Text logField = GameObject.Find("LogText").GetComponent<Text>();
        logPlaceholder = logField.transform.parent.Find ("Placeholder").GetComponent<Text> ();
        logPlaceholder.text = Stats.LogFile;

        // Set Logging checkmark to false
        GameObject logToggle = GameObject.Find("OutputField");
        logToggle.SetActive (false);
        GameObject.Find ("LoggingToggle").GetComponent<Toggle> ().isOn = false;
	}

    // Called when the player name is updated from the default.
    public void NameUpdated(string newName) {
        if (newName == "")
            characterName = namePlaceholder.text;
        else
            characterName = newName;
    }

    // Called when the 'Play' button is pressed
    public void ChangeScene() {
        Stats.PlayerName = characterName;
        Stats.LogFile = logOutput;
        Stats.LogActive = logActive;

        Debug.Log ("Scene 'changed'"); //TODO
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // Loads main game scene
    }

    // Called when the 'Controls' button is pressed
    public void ViewControls() {
        Debug.Log ("view controls");
        controls.SetActive (true);
    }
}
