using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowTint : MonoBehaviour {

	private const float MAX_ALPHA = 120;
	private const float MID_DAY = 12f * 60f;
	private const float END_OF_DAY = 17f * 60f;

	/// <summary>
	/// An event for updating the tints of the windows based on the time of day.
	/// </summary>
	public static Action TintEvent;

	/// <summary>
	/// The sprite renderers of the window tints.
	/// </summary>
	public List<SpriteRenderer> tints;

	private void Awake()
	{
		TintEvent = UpdateTints;
	}

	/// <summary>
	/// Updates the tints of the windows based on the time of day.
	/// </summary>
	void UpdateTints () {
		float currentAlpha = (((Mathf.Abs(MID_DAY - Stats.CurrentTime)) / (END_OF_DAY - MID_DAY)) * (MAX_ALPHA + 20f)) - 20f;
		foreach (SpriteRenderer s in tints) {
			s.color = new Color(s.color.r, s.color.g, s.color.b, currentAlpha > 0f ? currentAlpha / 255f : 0f);
		}
	}
}
