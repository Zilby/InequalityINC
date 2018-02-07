using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System;
using TMPro;

/// <summary>
/// Manages the main game UI. 
/// </summary>
public class UIManager : MonoBehaviour
{

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
	}


	/// <summary>
	/// The characters that have in-game portraits. 
	/// </summary>
	public enum Character
	{
		options = -1,
		player = 0,
		elevatorLady = 1,
	}


	/// <summary>
	/// Causes the UI to show the pause display. 
	/// </summary>
	public static Action PauseEvent;

	public delegate void textEvent(int i);
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
	/// The containers for the character portraits. 
	/// </summary>
	public List<FadeableUI> leftCharacters;
	public List<FadeableUI> rightCharacters;

	/// <summary>
	/// The character portraits for each character
	/// eg: characterPortraits[1][2] is elevator lady's sad portrait
	/// </summary>
	private List<List<GameObject>> leftCharacterPortraits;
	private List<List<GameObject>> rightCharacterPortraits;

	private DialogueParser dParser = new DialogueParser();

	private bool presentingOptions = false;

	// Use this for initialization
	void Awake()
	{
		PauseEvent = Pause;
		StartText = BeginText;
		EndText = FinishText;
		leftCharacterPortraits = GetChildren(leftCharacters);
		rightCharacterPortraits = GetChildren(rightCharacters);
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
	private void BeginText(int i)
	{
		GameManager.PauseEvent();
		dParser.LoadDialogue(i);
		ClearTexts();
		ClearPortraits();
		textOverlay.SelfFadeIn();
		StartCoroutine(RunDialogue());
	}


	/// <summary>
	/// Ends a dialogue scene. 
	/// </summary>
	private void FinishText()
	{
		GameManager.PauseEvent();
		textOverlay.SelfFadeOut();
	}


	/// <summary>
	/// Waits for the given key to be pressed.
	/// </summary>
	/// <param name="k">The key to be pressed.</param>
	private IEnumerator WaitForKeypress(KeyCode k)
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


	/// <summary>
	/// Runs the dialogue for the scene loaded in the dialogue parser. 
	/// </summary>
	private IEnumerator RunDialogue()
	{
		Dictionary<Character, Expression> lastExpression = new Dictionary<Character, Expression>();
		for (int i = 0; i < dParser.Lines.Count; ++i) 
		{
			DialogueParser.DialogueLine d = dParser.Lines[i];
			if (d.character != Character.options)
			{
				if (!lastExpression.ContainsKey(d.character) || d.expression != lastExpression[d.character])
				{
					SetExpression(d.character, d.expression, d.position == "R");
				}
				if (!lastExpression.ContainsKey(d.character))
				{
					lastExpression.Add(d.character, d.expression);
				}
				else
				{
					lastExpression[d.character] = d.expression;
				}
				yield return CharacterDialogue(d.character, d.content);
			}
			else
			{
				ClearTexts();
				presentingOptions = true;

				for(int j = 0; j < d.options.Length; ++j)
				{
					dialogueButtons[j].gameObject.SetActive(true);
					dialogueButtons[j].GetComponent<TextMeshProUGUI>().text = d.options[j].Split(':')[0];
					// On click gets called after j is incremented, so we have to save it as a temp value. 
					int temp = j;
					dialogueButtons[temp].onClick.AddListener(() => UpdateLine(ref i, int.Parse(d.options[temp].Split(':')[1])));
				}

				while(presentingOptions)
				{
					yield return null;
				}
			}
		}
		FinishText();
	}


	/// <summary>
	/// Updates the current dialogue line number once the player has clicked on a dialogue option. 
	/// </summary>
	private void UpdateLine(ref int index, int line)
	{
		// The line in the file starts at 1 not 0, and i gets 
		// incremented in the for loop again, so subtract 2. 
		index = line - 2;
		presentingOptions = false;
		foreach(Button b in dialogueButtons)
		{
			b.gameObject.SetActive(false);
			b.onClick.RemoveAllListeners();
		}
	}


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
		Camera.main.GetComponent<BlurOptimized>().enabled = Time.timeScale == 0.0f;
	}
}