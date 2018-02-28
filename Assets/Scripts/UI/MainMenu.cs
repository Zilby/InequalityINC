using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    // Values to track in PersistentGameData for scene transition
    private bool logActive = false;
    private string logOutput = "log.txt";
    private string characterName = "Alex"; // Default name for the player.
    private Text nameField;
    private Text placeholder;


    private GameObject controls;

    // Initialization boilerplate stuff
	void Awake () {
        nameField = GameObject.Find("NameText").GetComponent<Text>();
        placeholder = nameField.transform.parent.Find ("Placeholder").GetComponent<Text> ();
        placeholder.text = characterName;

        controls = GameObject.Find("Controls");
        controls.SetActive (false);
	}

    // Called when the player name is updated from the default.
    public void NameUpdated(string newName) {
        if (newName == "")
            characterName = placeholder.text;
        else
            characterName = newName;
    }

    // Called when the 'Play' button is pressed
    public void ChangeScene() {
        PersistentGameData.Instance.data.Add ("playerName", characterName);
        PersistentGameData.Instance.data.Add ("logActive", logActive.ToString());
        PersistentGameData.Instance.data.Add ("logOutput", logOutput);
        Debug.Log ("Scene 'changed'"); //TODO
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // Loads main game scene
    }

    // Called when the 'Controls' button is pressed
    public void ViewControls() {
        Debug.Log ("view controls");
        controls.SetActive (true);
    }
}
