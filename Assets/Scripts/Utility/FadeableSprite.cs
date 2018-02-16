using UnityEngine;
using System.Collections;


/// <summary>
/// A simple class that can be inherited to enable FadeIn / FadeOut functionality for a sprite object.
/// </summary>
public class FadeableSprite : MonoBehaviour
{
	protected const float FADE_IN_DUR = 0.3f;
	protected const float FADE_OUT_DUR = 0.2f;

	/// <summary>
	/// Allows the FadeableSprite to tween without being affected by TimeScale.
	/// NOTE: This can potentially cause frames to be skipped.
	/// </summary>
	[System.NonSerialized]
	public bool useUnscaledDeltaTimeForUI = false;
	/// <summary>
	/// A reference to an active fade coroutine.
	/// </summary>
	protected Coroutine fadeCoroutine;
	protected bool isFading = false;

	private SpriteRenderer sr;

	public bool IsVisible { get; protected set; }

	public bool IsFading
	{
		get
		{
			return isFading;
		}
	}


	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
	}


	/// <summary>
	/// Immediately displays the canvasGroup.
	/// </summary>
	public virtual void Show()
	{
		IsVisible = true;
		SetAlpha(1.0f);
		sr.gameObject.SetActive(true);
	}


	/// <summary>
	/// Immediately hides the canvasGroup.
	/// </summary>
	public virtual void Hide()
	{
		IsVisible = false;
		SetAlpha(0.0f);
		sr.gameObject.SetActive(false);
	}


	/// <summary>
	/// Fades in the sprite renderer over the defined time.
	/// Interaction is disabled until the animation has finished.
	/// </summary>
	public virtual IEnumerator FadeIn(float startAlpha = 0, float endAlpha = 1, float dur = FADE_IN_DUR)
	{
		IsVisible = true;
		isFading = true;
		sr.gameObject.SetActive(true);
		SetAlpha(startAlpha);

		float duration = dur;
		float timeElapsed = duration * sr.color.a;

		while (timeElapsed < duration)
		{
			SetAlpha(Mathf.Lerp(startAlpha, endAlpha, timeElapsed / duration));
			yield return null;
			timeElapsed += useUnscaledDeltaTimeForUI ? Time.unscaledDeltaTime : Time.deltaTime;
		}
		SetAlpha(endAlpha);
		isFading = false;
		yield break;
	}


	/// <summary>
	/// Fades out the Canvas group over the defined time.
	/// Interaction is becomes disabled immediately.
	/// </summary>
	public virtual IEnumerator FadeOut(float endAlpha = 0, float dur = FADE_OUT_DUR)
	{
		IsVisible = false;
		isFading = true;
		float duration = dur;
		float timeElapsed = duration * (1f - sr.color.a);
		while (timeElapsed < duration)
		{
			SetAlpha(Mathf.Lerp(1, endAlpha, timeElapsed / duration));
			yield return null;
			timeElapsed += useUnscaledDeltaTimeForUI ? Time.unscaledDeltaTime : Time.deltaTime;
		}
		SetAlpha(endAlpha);
		sr.gameObject.SetActive(sr.color.a != 0);
		isFading = false;
		yield break;
	}


	/// <summary>
	/// Starts the FadeIn coroutine inside this script.
	/// </summary>
	/// <param name="force">If true, any previously running fade animation will be cancelled</param>
	public virtual void SelfFadeIn(bool force = true, float startAlpha = 0, float endAlpha = 1, float dur = FADE_IN_DUR)
	{
		if (!force && isFading)
		{
			return;
		}
		// Make the self active incase disabled, coroutine cant run otherwise.
		this.gameObject.SetActive(true);
		if (fadeCoroutine != null)
		{
			StopCoroutine(fadeCoroutine);
		}
		fadeCoroutine = StartCoroutine(FadeIn(startAlpha, endAlpha, dur));
	}


	/// <summary>
	/// Starts the FadeOut coroutine inside this script. 
	/// </summary>
	/// <param name="force">If true, any previously running fade animation will be cancelled</param>
	public virtual void SelfFadeOut(bool force = true, float endAlpha = 0, float dur = FADE_OUT_DUR)
	{
		if (!force && isFading)
		{
			return;
		}
		// Make the self active incase disabled, coroutine cant run otherwise.
		this.gameObject.SetActive(true);
		if (fadeCoroutine != null)
		{
			StopCoroutine(fadeCoroutine);
		}
		fadeCoroutine = StartCoroutine(FadeOut(endAlpha, dur));
	}


	/// <summary>
	/// Sets the alpha value for the sprite renderer. 
	/// </summary>
	/// <param name="alpha">Alpha.</param>
	private void SetAlpha(float alpha)
	{
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
	}
}