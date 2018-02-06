using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Manages the main game UI. 
/// </summary>
public class UIManager : MonoBehaviour
{

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

	/// <summary>
	/// The moving dialogue text for each character
	/// </summary>
	public List<MoveableText> characterTexts;

	private delegate IEnumerator dialogueEvent();
	private List<dialogueEvent> dialogues;
	private dialogueEvent dialogue0, dialogue1, dialogue2, dialogue3;


	// Use this for initialization
	void Awake()
	{
		PauseEvent = Pause;
		StartText = BeginText;
		EndText = FinishText;
		dialogue0 = Dialogue0;
		dialogues = new List<dialogueEvent>() { dialogue0, dialogue1, dialogue2, dialogue3 };
		leftCharacterPortraits = new List<List<GameObject>>();
		foreach (FadeableUI f in leftCharacters)
		{
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in f.transform)
			{
				children.Add(child.gameObject);
			}
			leftCharacterPortraits.Add(children);
		}
		rightCharacterPortraits = new List<List<GameObject>>();
		foreach (FadeableUI f in rightCharacters)
		{
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in f.transform)
			{
				children.Add(child.gameObject);
			}
			rightCharacterPortraits.Add(children);
		}
	}


	/// <summary>
	/// Begins a dialogue stream. 
	/// </summary>
	/// <param name="i">The scene index for dialogue.</param>
	private void BeginText(int i)
	{
		GameManager.PauseEvent();
		ClearTexts();
		ClearPortraits();
		textOverlay.SelfFadeIn();
		StartCoroutine(dialogues[i]());
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


	private IEnumerator Dialogue0()
	{
		SetExpression(0, 0, false);
		SetExpression(Character.elevatorLady, Expression.sad, true);
		yield return CharacterDialogue(0, "Welcome to the team. We’ve got a rocky relationship with the coppers, " +
									   "so you should be aware of what you’re getting into before you start");
		SetExpression(Character.player, Expression.neutral, false);
		SetExpression(1, 1, true);
		yield return CharacterDialogue(1, "Alright, I’ll give you the rundown. As you already know, Resist and Transmit " +
									   "is an underground rebel group that’s trying to bring music back to the radio.");
		FinishText();
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
