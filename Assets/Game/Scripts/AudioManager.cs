using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private static AudioSource asource;

	private void Start()
	{
		asource = GetComponent<AudioSource>();
	}

	public static void Play(AudioClip clip)
	{
		asource.PlayOneShot(clip);
	}

	public static void PlayLoop(AudioClip clip)
	{
		asource.clip = clip;
		asource.Play();
	}

	public static void StopLoop()
	{
		asource.Stop();
	}
}
