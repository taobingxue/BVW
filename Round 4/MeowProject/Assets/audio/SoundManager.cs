using UnityEngine;
using System.Collections;

	public class SoundManager : MonoBehaviour 
	{
		public AudioSource sfxSource;                   // sfx.
		public AudioSource bgmSource;               //bgm
		public AudioSource moveSource;           //user-interaction sound.
		public AudioSource dialogeSource;             //character's dialoge sound
		public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             

		
		
		void Awake ()
		{
			//Check if there is already an instance of SoundManager
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy (gameObject);

			DontDestroyOnLoad (gameObject);
		}
		
		
		//sfx
		public void PlaySfx(AudioClip clip, bool loop)
		{
			sfxSource.clip = clip;
			sfxSource.loop = loop;
			sfxSource.Play ();
		}

		//bgm
		public void PlayBgm(AudioClip clip, bool loop)
		{
			bgmSource.clip = clip;
			bgmSource.loop = loop;
			bgmSource.Play ();
		}

		//user-interaction
		public void PlayMove(AudioClip clip)
		{
			moveSource.clip = clip;
			moveSource.Play ();
		}

		//
		public void PlayDialoge(AudioClip clip)
		{
			dialogeSource.clip = clip;
			dialogeSource.Play ();
		}


	}
