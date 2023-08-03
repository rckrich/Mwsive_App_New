using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio_Manager : RCK_Singleton<Audio_Manager> 
{
	public AudioMixer audioMixer;
	public AudioSource musicASource;
	public AudioSource soundfxASource;
	public AudioClip[] music;				// ** 0 => Main Theme **, ** 1 => Cutscenes **, ** 2 => Variable Theme from ScriptableObject **
	public AudioClip[] soundFX;				// ** 0 => Ok Btn **, ** 1 => Title_LogoStart **,

	public bool muteMusic = false;
	public bool muteSFX = false;

	bool m_musicIsInTransition = false;

	/// <summary>
	/// Play the music.
	/// ** 0 => Main Theme Title_Map **, ** 1 => Cutscenes **, ** 2 => Variable Theme from ScriptableObject **
	/// </summary>
	/// <param name="value">Value.</param>
	public void PlayMusic(int value)
	{
		if (!muteMusic) 
		{
			AudioClip song = music [value];
			musicASource.clip = song;
			musicASource.Play ();
		}
	}

	public void StopMusic()
	{
		musicASource.Stop ();
	}

	/// <summary>
	/// Pauses and Unpauses music.
	/// ** True to Pause **, ** False to Unpause **
	/// </summary>
	/// <param name="pauseMusic">If set to <c>true</c> pause music.</param>
	public void PauseUnpauseMusic(bool pauseMusic)
	{
		if (pauseMusic)
		{
			musicASource.Pause ();
		}
		else if (!pauseMusic)
		{
			musicASource.UnPause ();
		}
	}
		
	/// <summary>
	/// Music volume.
	/// ** Mute => -80 **, ** Default => -8 **, ** Max => 20 (Not recommended anything above default) **
	/// </summary>
	/// <param name="volume">Volume.</param>
	public void MusicVolume(float volume = -8f)
	{
		audioMixer.SetFloat ("MusicVolume", volume);
	}

	/// <summary>
	/// Music transition from one AudioClip to another AudioClip.
	/// ** 0 => Main Theme Title_Map **, ** 1 => Cutscenes **, ** 2 => Variable Theme from ScriptableObject **
	/// </summary>
	/// <param name="value">Value.</param>
	public void MusicTransition(int value)
	{
		StartCoroutine (MusicTransitionRoutine (value));
	}

	IEnumerator MusicTransitionRoutine(int value)
	{
		if (!m_musicIsInTransition) 
		{
			m_musicIsInTransition = true;

			// Bajo el sonido de la musica
			bool reachedDestination1 = false;
			float elapsedTime1 = 0f;
			float timeToMove1 = 1f;

			while (!reachedDestination1) 
			{
				if (musicASource.volume <= 0f) 
				{
					musicASource.volume = 0f;
					reachedDestination1 = true;
					break;
				}

				elapsedTime1 += Time.deltaTime;
				float t = Mathf.Clamp (elapsedTime1 / timeToMove1, 0f, 1f);
				t = t * t * t * (t * (t * 6 - 15) + 10);

				musicASource.volume = Mathf.Lerp (1f, 0f, t);
				yield return null;
			}

			yield return null;

			// Cambio el AudioClip
			AudioClip song = music [value];
			musicASource.clip = song;
			musicASource.Play ();

			// Subo el volumen de la musica
			bool reachedDestination2 = false;
			float elapsedTime2 = 0f;
			float timeToMove2 = 1f;

			while (!reachedDestination2)
			{
				if (musicASource.volume >= 1f) 
				{
					musicASource.volume = 1f;
					reachedDestination2 = true;
					break;
				}

				elapsedTime2 += Time.deltaTime;
				float t = Mathf.Clamp (elapsedTime2 / timeToMove2, 0f, 1f);
				t = t * t * t * (t * (t * 6 - 15) + 10);

				musicASource.volume = Mathf.Lerp (0f, 1f, t);
				yield return null;
			}
				
			m_musicIsInTransition = false;
		}
	}
		
	/// <summary>
	/// Play the SoundFX.
	/// ** 0 => Ok Btn **, ** 1 => Title_LogoStart **,
	/// </summary>
	/// <param name="value">Value.</param>
	public void PlaySFX(int value)
	{
		if (!muteSFX) 
		{
			AudioClip sfx = soundFX [value];
			soundfxASource.clip = sfx;
			soundfxASource.Play ();
		}
	}

	/// <summary>
	/// SFX volume.
	/// ** Mute => -80 **, ** Default => 0 **, ** Max => 20 (Not recommended anything above default) **
	/// </summary>
	/// <param name="volume">Volume.</param>
	public void SFXVolume(float volume = 0f)
	{
		audioMixer.SetFloat ("SoundFXVolume", volume);
	}


	public void SetGameplayMusic(AudioClip clip)
	{
		music [2] = clip;
	}


}




