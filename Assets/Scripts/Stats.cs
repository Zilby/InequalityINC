using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// All of the stats for our game. 
/// </summary>
public class Stats
{
	/// <summary>
	/// Used for incrementing the time when dialogue starts. 
	/// </summary>
	public const int DIALOGUE_START_TIME_INCREMENT = 30;

	/// <summary>
	/// Used for incrementing the time with longer dialogue. 
	/// </summary>
	public const int DIALOGUE_LONG_TIME_INCREMENT = 30;

	/// <summary>
	/// The relationship points for each character. 
	/// </summary>
	public static Dictionary<DialogueManager.Character, int> relationshipPoints = new Dictionary<DialogueManager.Character, int>();

	/// <summary>
	/// The current dialogue index for each character. 
	/// </summary>
	public static Dictionary<DialogueManager.Character, int> dialogueIndex = new Dictionary<DialogueManager.Character, int>();

	/// <summary>
	/// Whether or not the player has gotten information from the given character on the given day
	/// The list of bools is the various pieces of information the player potentially has on the character. 
	/// </summary>
	public static Dictionary<DialogueManager.Character, List<bool>> hasInfoOn = new Dictionary<DialogueManager.Character, List<bool>>();

	/// <summary>
	/// The current time in minutes. 
	/// </summary>
	private static int currentTime;

	/// <summary>
	/// The current time in minutes. 
	/// Also updates the clock when set. 
	/// </summary>
	public static int CurrentTime
	{
		get
		{
			return currentTime;
		}
		set
		{
			currentTime = value;
			UIManager.ClockEvent();
			WindowTint.TintEvent();
		}
	}


	/// <summary>
	/// Resets all in-game stats. 
	/// </summary>
	public static void ResetAll()
	{
		ResetDay();
		foreach (DialogueManager.Character c in Enum.GetValues(typeof(DialogueManager.Character)))
		{
			relationshipPoints[c] = 0;
			dialogueIndex[c] = 0;
			hasInfoOn[c] = Enumerable.Repeat(false, 10).ToList();
			for (int i = 0; i < hasInfoOn[c].Count; ++i)
			{
				hasInfoOn[c][i] = false;
			}
		}
	}


	/// <summary>
	/// Resets all stats of the current day. 
	/// </summary>
	public static void ResetDay()
	{
		CurrentTime = 9 * 60;
	}


	/// <summary>
	/// Gets the time as a string.
	/// </summary>
	/// <returns>The time.</returns>
	public static string GetTime()
	{
		return ((currentTime / 60) < 13 ? (currentTime / 60) : ((currentTime / 60) % 13) + 1) + ":" +
			((currentTime % 60) == 0 ? "00" : (currentTime % 60).ToString()) +
			(currentTime < 12 * 60 ? " AM" : " PM");
	}
}
