using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

/// <summary>
/// Manages gameplay aspects of the main game. 
/// </summary>
public class GameManager : MonoBehaviour
{
	private const int TIME_INCREMENT_DELAY = 30;

	/// <summary>
	/// Causes the gameplay to pause. 
	/// </summary>
	public static Action PauseEvent;


	void Awake()
	{
		PauseEvent = Pause;
	}


	void Start()
	{
		Stats.ResetAll();
		StartCoroutine(IncrementTime());
	}


	private void Update()
	{
		NPCController.ArriveLeave();
	}


	private void Pause()
	{
		Time.timeScale = Time.timeScale == 0.0f ? 1.0f : 0.0f;
		Camera.main.GetComponent<BlurOptimized>().enabled = Time.timeScale == 0.0f;
	}


	/// <summary>
	/// Increments the time per minute of in-game time.
	/// </summary>
	private IEnumerator IncrementTime()
	{
		while (Stats.CurrentTime < 17 * 60)
		{
			yield return new WaitForSeconds(TIME_INCREMENT_DELAY);
			if (Stats.CurrentTime < 17 * 60)
			{
				Stats.CurrentTime += 10;
			}
		}
		yield return null;
	}
}
