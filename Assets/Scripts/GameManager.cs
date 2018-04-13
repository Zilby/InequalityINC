using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

/// <summary>
/// Manages gameplay aspects of the main game. 
/// </summary>
public class GameManager : MonoBehaviour
{
	public GameObject characterHolder;
	private NPCController[] characters;

	[SerializeField]
	private float timeIncrementDelay = 30;

	/// <summary>
	/// Causes the gameplay to pause. 
	/// </summary>
	public static Action PauseEvent;


	void Awake()
	{
		PauseEvent = Pause;
		characters = characterHolder.GetComponentsInChildren<NPCController>();
		Stats.ResetAll();
	}


	void Start()
	{
		StartCoroutine(NewDay());
	}


	private void Update()
	{
		#if UNITY_EDITOR
		if(Input.GetKey(KeyCode.L)) 
		{
			Stats.CurrentTime = 17 * 60;
		}
		#endif
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
			float delay = 0.0f;
			while (delay < timeIncrementDelay)
			{
				if (Stats.CurrentTime >= 17 * 60)
				{
					break;
				}
				yield return new WaitForSeconds (0.1f);
				delay += 0.1f;
			}
			Stats.CurrentTime += 10;
		}
		yield return null;
		Stats.Day++;
		StartCoroutine(NewDay());
	}

	public void OnApplicationQuit()
	{
		Logger.Log("Application Quit");
	}

	/// <summary>
	/// Sets up a new day
	/// </summary>
	private IEnumerator NewDay()
	{
		if (Stats.Day == 5)
		{
			SceneManager.LoadScene("Credits");
		}
		else
		{
			Pause();
			if (Stats.Day == 0)
			{
				// Day 1 exclusive content
				SoundManager.SongEvent(0);
			}
			if (Stats.Day == 1)
			{
				// Day 2 exclusive content
				SoundManager.SongEvent(2);
			}
			if (Stats.Day == 2)
			{
				// Day 3 exclusive content
				SoundManager.SongEvent(3);
			}
			if (Stats.Day == 3)
			{
				// Day 4 exclusive content
				SoundManager.SongEvent(4);
			}
			if (Stats.Day == 4)
			{
				// Day 5 exclusive content
				SoundManager.SongEvent(0);
			}
			yield return UIManager.FadeDayInEvent();
			Stats.ResetDay();
			PlayerController.ResetEvent();
			NPCController.Relocate();
			yield return UIManager.FadeDayOutEvent();
			Pause();
			StartCoroutine(IncrementTime());
		}
	}
}
