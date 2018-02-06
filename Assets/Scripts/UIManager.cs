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
	/// The character portraits for each character
	/// eg: characterPortraits[1][2] is elevator lady's sad portrait
	/// </summary>
	[SerializeField]
	public List<GameObjectListWrapper> leftCharacterPortraits;
	public List<GameObjectListWrapper> rightCharacterPortraits;

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
		overlay.useUnscaledDeltaTimeForUI = true;
		textOverlay.useUnscaledDeltaTimeForUI = true;
	}


	/// <summary>
	/// Begins a dialogue stream. 
	/// </summary>
	/// <param name="i">The scene index for dialogue.</param>
	private void BeginText(int i)
	{
		GameManager.PauseEvent();
		ClearTexts();
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
		List<GameObjectListWrapper> portraits = right ? rightCharacterPortraits : leftCharacterPortraits;
		for (int i = 0; i < portraits[c].Count; ++i)
		{
			portraits[c][i].SetActive(i == e);
		}
	}


	private void ClearTexts() {
		foreach (MoveableText t in characterTexts)
		{
			t.ClearText();
		}
	}


	private IEnumerator Dialogue0()
	{
		SetExpression(0, 0, false);
		SetExpression(Character.elevatorLady, Expression.sad, true);
		yield return CharacterDialogue(0, "Welcome to the team. We’ve got a rocky relationship with the coppers, so you should be aware of what you’re getting into before you start");
		SetExpression(Character.player, Expression.neutral, false);
		SetExpression(1, 1, true);
		yield return CharacterDialogue(1, "Alright, I’ll give you the rundown. As you already know, Resist and Transmit is an underground rebel group that’s trying to bring music back to the radio.");
		/*
		SetExpression(Expression.neutral);
		SetExpression(Expression.listening);
		yield return new WaitForSecondsRealtime(0.5f);
		yield return sempaiText.TypeText("Welcome to the team. We’ve got a rocky relationship with the coppers, so you should be aware of what you’re getting into before you start");
		yield return new WaitUntil(KeyCode.Space);
		yield return new WaitForSecondsRealtime(2.0f);
		sempaiText.ClearText();
		SetExpression(Expression.happy);
		yield return kidText.TypeText("Lay it on me");
		yield return new WaitForSecondsRealtime(1.5f);
		SetExpression(Expression.neutral);
		SetExpression(Expression.listening);
		kidText.ClearText();
		yield return sempaiText.TypeText("Alright, I’ll give you the rundown. As you already know, Resist and Transmit is an underground rebel group that’s trying to bring music back to the radio.");
		yield return new WaitForSecondsRealtime(1.5f);
		yield return sempaiText.TypeText("We started up in 3018, when the radio was sold to corporations and they stopped playing music and started running ads 24/7.");
		yield return new WaitForSecondsRealtime(1.5f);
		yield return sempaiText.TypeText("Bastard corporations. So what we do is take over the radio and play music over their commercials");
		yield return new WaitForSecondsRealtime(1.5f);
		SetExpression(Expression.pointing);
		yield return sempaiText.TypeText("That being said, I think it’s about time to stick it to the man. Here’s how it works");
		yield return new WaitForSecondsRealtime(1.5f);
		SetExpression(Expression.listening);
		yield return sempaiText.TypeText("There’s a visualizer at the top of the board. See it? Doesn’t matter. That’s the wavelength we’re trying to match.");
		yield return new WaitForSecondsRealtime(1.5f);
		yield return sempaiText.TypeText("In order to take over the station, we’ve gotta make sure that our frequency and amplitude line up with it. Use the dials to adjust how wide and how tall our wavelength is");
		yield return new WaitForSecondsRealtime(1.5f);
		sempaiText.ClearText();
		yield return kidText.TypeText("Uh, okay. Got it");
		yield return new WaitForSecondsRealtime(1.5f);
		kidText.ClearText();
		yield return sempaiText.TypeText("And you see that visualizer?");
		yield return new WaitForSecondsRealtime(1.5f);
		SetExpression(Expression.meh);
		yield return kidText.TypeText("Um… Which one?");
		yield return new WaitForSecondsRealtime(1.5f);
		SetExpression(Expression.neutral);
		kidText.ClearText();
		yield return sempaiText.TypeText("Eh, you’ll figure it out. Anyway, similar idea. That controls something called impedance.");
		yield return new WaitForSecondsRealtime(1.5f);
		yield return sempaiText.TypeText("Long story short, what you’ve gotta do is use that slider right there to make sure that the red horizontal line you see there matches up with the white horizontal line.");
		yield return new WaitForSecondsRealtime(1.5f);
		yield return sempaiText.TypeText("That’s gonna ensure that we’re not sending out standing waves so that our systems don’t overheat. Got it?");
		yield return new WaitForSecondsRealtime(1.5f);
		sempaiText.ClearText();
		SetExpression(Expression.meh);
		yield return kidText.TypeText("I… Think so?");
		yield return new WaitForSecondsRealtime(1.5f);
		kidText.ClearText();
		yield return sempaiText.TypeText("And there’s a slider at the bottom. That one controls the radius of our broadcast. So the higher that slider is, the more people we’re reaching.");
		yield return new WaitForSecondsRealtime(1.5f);
		yield return sempaiText.TypeText("It’ll increase our popularity, but that also means that the chances that the coppers will be able to find us increases. So use it wisely. Got any questions?");
		yield return new WaitForSecondsRealtime(1.5f);
		sempaiText.ClearText();
		yield return kidText.TypeText("Uh, yeah, so—");
		yield return new WaitForSecondsRealtime(1.5f);
		kidText.ClearText();
		SetExpression(Expression.pointing);
		yield return sempaiText.TypeText("Yoinkers! I gotta be gettin’ out of here. The husband’ll be wondering where I am. Good luck, kid. You’ll figure this out");
		yield return new WaitForSecondsRealtime(2.0f);
		*/
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
