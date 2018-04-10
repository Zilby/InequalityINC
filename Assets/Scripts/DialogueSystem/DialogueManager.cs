using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.IO;

/// <summary>
/// Manages the main game dialogue. 
/// </summary>
public class DialogueManager : MonoBehaviour
{
	/// <summary>
	/// The list of buttons for dialogue. 
	/// </summary>
	public List<Button> dialogueButtons;

	/// <summary>
	/// The expressions assignable to each character.
	/// </summary>
	public enum Expression
	{
		happy = 0,
		neutral = 1,
		sad = 2,
		anxious = 3,
		excited = 4,
		angry = 5,
	}


	/// <summary>
	/// The characters that have in-game portraits. 
	/// </summary>
	public enum Character
	{
		player = 0,
		dave = 1,
		sharon = 2,
		stan = 3, 
		janet = 4, 
		richard = 5
	}

	/// <summary>
	/// Causes the UI to show the dialogue text display. 
	/// </summary>
	public static Action PauseEvent;

	public delegate void textEvent(int i, bool b, DialogueManager.Character ch, bool trust = true);
	/// <summary>
	/// Starts a given dialogue event (eg: scene 0). 
	/// </summary>
	public static textEvent StartText;

	/// <summary>
	/// Ends a dialogue event
	/// </summary>
	public static Action EndText;

	/// <summary>
	/// The overlay backdrop for the text
	/// </summary>
	public FadeableUI textOverlay;

	/// <summary>
	/// The background for a dialogue scene
	/// </summary>
	public FadeableUI overlay;

	/// <summary>
	/// The moving dialogue text for each character
	/// </summary>
	public List<MoveableText> characterTexts;

	/// <summary>
	/// The question text.
	/// </summary>
	public TextMeshProUGUI questionText;

	/// <summary>
	/// The fadeable UI for a question. 
	/// </summary>
	public FadeableUI question;

	/// <summary>
	/// The containers for the character portraits. 
	/// </summary>
	public List<FadeableUI> leftCharacters;
	public List<FadeableUI> rightCharacters;

	public List<FadeableUI> trustUI;

	/// <summary>
	/// Used to determine if the trust UI is active for this dialogue scene. 
	/// </summary>
	private bool trustUIActive = true;

	/// <summary>
	/// The character portraits for each character
	/// eg: characterPortraits[1][2] is elevator lady's sad portrait
	/// </summary>
	private List<List<GameObject>> leftCharacterPortraits;
	private List<List<GameObject>> rightCharacterPortraits;

	/// <summary>
	/// The dialogue parser for parsing text files. 
	/// </summary>
	private DialogueParser dParser = new DialogueParser();

	/// <summary>
	/// Whether or not we are currently presenting options to the player. 
	/// </summary>
	private bool presentingOptions = false;


	// Use this for initialization
	void Awake()
	{
		PauseEvent = Pause;
		StartText = BeginText;
		EndText = FinishText;
		leftCharacterPortraits = GetChildren(leftCharacters);
		rightCharacterPortraits = GetChildren(rightCharacters);
		textOverlay.Hide();
		overlay.Hide();
		question.Hide();
		foreach (FadeableUI f in leftCharacters)
		{
			f.Hide();
		}
		foreach (FadeableUI f in rightCharacters)
		{
			f.Hide();
		}
	}


	/// <summary>
	/// Gets a list of the children of a given list of fadeable UI.
	/// </summary>
	/// <returns>The children.</returns>
	private List<List<GameObject>> GetChildren(List<FadeableUI> l)
	{
		List<List<GameObject>> output = new List<List<GameObject>>();
		foreach (FadeableUI f in l)
		{
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in f.transform)
			{
				children.Add(child.gameObject);
			}
			output.Add(children);
		}
		return output;
	}


	/// <summary>
	/// Begins a dialogue stream. 
	/// </summary>
	/// <param name="i">The scene index for dialogue.</param>
	/// <param name="c">The number of conversations left for this character.</param>
	/// <param name="b">Whether or not there is available dialogue.</param>
	private void BeginText(int i, bool b, Character ch, bool trust = true)
	{
		trustUIActive = trust;
		GameManager.PauseEvent();
		Pause();
		ClearTexts();
		ClearPortraits();
		ClearTrustUI();
		textOverlay.SelfFadeIn();
		StartCoroutine(RunDialogue(i, !b, ch));
	}


	/// <summary>
	/// Ends a dialogue scene. 
	/// </summary>
	private void FinishText()
	{
		Logger.Log("Ended dialogue.\n");
		GameManager.PauseEvent();
		Pause();
		textOverlay.SelfFadeOut();
	}


	/// <summary>
	/// Waits for the given key to be pressed.
	/// </summary>
	/// <param name="k">The key to be pressed.</param>
	public static IEnumerator WaitForKeypress(KeyCode k)
	{
		while (!Input.GetKeyDown(k))
		{
			yield return null;
		}
	}


	/// <summary>
	/// Displays the dialogue for a given character. 
	/// </summary>
	/// <param name="c">The character given.</param>
	/// <param name="s">The dialogue string. </param>
	private IEnumerator CharacterDialogue(Character c, string s)
	{
		yield return CharacterDialogue((int)c, s);
	}


	/// <summary>
	/// Displays the dialogue for a given character. 
	/// </summary>
	/// <param name="c">The character index given.</param>
	/// <param name="s">The dialogue string. </param>
	private IEnumerator CharacterDialogue(int c, string s)
	{
		Logger.Log(((Character)c).ToString() + " says : " + s);
		ClearTexts();
		yield return characterTexts[c].TypeText(s);
		yield return new WaitForSecondsRealtime(0.1f);
		yield return WaitForKeypress(KeyCode.Space);
	}


	/// <summary>
	/// Sets the expression of the given character. 
	/// </summary>
	/// <param name="c">The character for the expression.</param>
	/// <param name="e">The expression of the character.</param>
	/// <param name="right">Whether the character is on the left or right side.</param>
	private void SetExpression(Character c, Expression e, bool right)
	{
		SetExpression((int)c, (int)e, right);
	}


	/// <summary>
	/// Sets the expression of the given character. 
	/// </summary>
	/// <param name="c">The character index for the expression.</param>
	/// <param name="e">The expression index of the character.</param>
	/// <param name="right">Whether the character is on the left or right side.</param>
	private void SetExpression(int c, int e, bool right)
	{
		int side = right ? 1 : 0;
		ClearPortraits(side);
		List<FadeableUI> characters = right ? rightCharacters : leftCharacters;

		// fade the new portrait in 
		characters[c].SelfFadeIn();

		// Set the current expression
		List<List<GameObject>> portraits = right ? rightCharacterPortraits : leftCharacterPortraits;
		for (int i = 0; i < portraits[c].Count; ++i)
		{
			portraits[c][i].SetActive(i == e);
		}
	}


	/// <summary>
	/// Clears the dialogue texts.
	/// </summary>
	private void ClearTexts()
	{
		foreach (MoveableText t in characterTexts)
		{
			t.ClearText();
		}
	}


	/// <summary>
	/// Clears the character portraits. Optionally has parameter s 
	/// to only clear the left side (s = 0) or right side (s = 1)
	/// </summary>
	/// <param name="s">Optional side parameter</param>
	private void ClearPortraits(int s = -1)
	{
		if (s != 1)
		{
			foreach (FadeableUI f in leftCharacters)
			{
				f.Hide();
			}
		}
		if (s != 0)
		{
			foreach (FadeableUI f in rightCharacters)
			{
				f.Hide();
			}
		}
	}


	private void ClearTrustUI() {
		foreach (FadeableUI f in trustUI)
		{
			f.Hide();
		}
	}


	private void AssignExpression(ref Dictionary<Character, Expression> lastExpression, Character c, Expression e, string position)
	{
		if (!lastExpression.ContainsKey(c) || e != lastExpression[c])
		{
			SetExpression(c, e, position.ToUpper() == "R");
		}
		if (!lastExpression.ContainsKey(c))
		{
			lastExpression.Add(c, e);
		}
		else
		{
			lastExpression[c] = e;
		}
	}


	/// <summary>
	/// Runs the dialogue for the scene loaded in the dialogue parser. 
	/// </summary>
	private IEnumerator RunDialogue(int scene, bool dialogue, Character c)
	{
		Dictionary<Character, Expression> lastExpression = new Dictionary<Character, Expression>();
		if (dialogue)
		{
			yield return dParser.LoadDialogue(Path.Combine(Path.Combine("Dialogue", c.ToString()), "Dialogue" + scene));
			Stats.CurrentTime += Stats.DIALOGUE_START_TIME_INCREMENT;
		}
		else
		{
			yield return dParser.LoadDialogue(Path.Combine(Path.Combine("Snippet", c.ToString()), "Snippet" + scene));
		}
		DialogueParser.DialogueLine l = dParser.Head;
		while (true)
		{
			// Handle things done before dialogue
			string position = l.node.character == Character.player || l.node.forceLeft ? "L" : "R";
			Image e = (position == "R" ? rightCharacterPortraits : leftCharacterPortraits)[(int)l.node.character][(int)l.node.expression].GetComponent<Image>();
			HandleTrustUI(l);
			e.color = Color.white;

			// Handle dialogue
			AssignExpression(ref lastExpression, l.node.character, l.node.expression, position);
			yield return CharacterDialogue(l.node.character, l.node.dialogue);

			// Handle things after dialogue
			e.color = new Color(135 / 255.0f, 135 / 255.0f, 135 / 255.0f, 165 / 255.0f);
			if (l.node.longOption)
			{
				IncrementDialogueTime();
			}
			if (l.node.positive)
			{
				AddRelationshipPoints(l.node.characterAffected, 1);
			}
			if (l.node.negative)
			{
				AddRelationshipPoints(l.node.characterAffected, -1);
			}
			if (l.node.fired)
			{
				FiredCharacter(l.node.characterAffected);
			}
			if (-1 < l.node.infoGathered && l.node.infoGathered < 11)
			{
				GotInfoOnCharacter(l.node.characterAffected, l.node.infoGathered);
			}
			l.RestrictConnections();
			if (l.connections.Count < 2)
			{
				if (l.connections.Count == 1)
				{
					l = l.connections[0];
				}
				else
				{
					break;
				}
			}
			else
			{
				ClearTexts();
				presentingOptions = true;

				for (int j = 0; j < l.connections.Count; ++j)
				{
					dialogueButtons[j].gameObject.SetActive(true);
					dialogueButtons[j].GetComponent<TextMeshProUGUI>().text = l.connections[j].node.dialogue;
					// On click gets called after j is incremented, so we have to save it as a temp value. 
					int temp = j;
					dialogueButtons[j].onClick.AddListener(() => UpdateLine(ref l, l.connections[temp]));
				}

				questionText.text = l.node.dialogue;
				question.SelfFadeIn();

				while (presentingOptions)
				{
					yield return null;
				}

				question.SelfFadeOut();
			}
		}
		FinishText();
	}


	private void HandleTrustUI(DialogueParser.DialogueLine l) {
		int rp = Mathf.Clamp(Stats.relationshipPoints[l.node.character], -1, 1) + 1;
		if (trustUIActive && l.node.character != Character.player)
		{
			for (int i = 0; i < trustUI.Count; ++i)
			{
				if (rp == i)
				{
					if (!trustUI[i].IsVisible)
					{
						trustUI[i].SelfFadeIn();
					}
				}
				else
				{
					if (trustUI[i].IsVisible)
					{
						trustUI[i].SelfFadeOut();
					}
				}
			}
		}
	}


	/// <summary>
	/// Updates the current dialogue line number once the player has clicked on a dialogue option. 
	/// </summary>
	private void UpdateLine(ref DialogueParser.DialogueLine current, DialogueParser.DialogueLine newLine)
	{
		current = newLine;
		presentingOptions = false;
		foreach (Button b in dialogueButtons)
		{
			b.gameObject.SetActive(false);
			b.onClick.RemoveAllListeners();
		}
	}


	/// <summary>
	/// Indicates that the given info id has been gathered on the given character.
	/// </summary>
	private void GotInfoOnCharacter(Character c, int info)
	{
		Stats.hasInfoOn[c][info] = true;
		Logger.Log(c.ToString() + " info #" + info.ToString() + "obtained.");
	}


	/// <summary>
	/// Indicates that the given character was fired. 
	/// </summary>
	private void FiredCharacter(Character c)
	{
		Stats.fired[c] = true;
		Logger.Log(c.ToString() + " fired.");
	}


	/// <summary>
	/// Increments the dialogue time for longer dialogue choices.
	/// </summary>
	private void IncrementDialogueTime()
	{
		Stats.CurrentTime += Stats.DIALOGUE_LONG_TIME_INCREMENT;
	}


	/// <summary>
	/// Adds relationship points to the given character. 
	/// </summary>
	private void AddRelationshipPoints(Character c, int points)
	{
		Stats.relationshipPoints[c] += points;
		Logger.Log(c.ToString() + " bond set to " + Stats.relationshipPoints[c].ToString() + ".");
	}


	/// <summary>
	/// Used when the game pauses. 
	/// </summary>
	private void Pause()
	{
		if (Time.timeScale == 0.0f)
		{
			overlay.SelfFadeIn();
		}
		else
		{
			overlay.SelfFadeOut();
		}
	}
}