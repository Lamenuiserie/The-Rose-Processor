using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	/// <summary>
	/// The only instance of the sound manager.
	/// </summary>
	private static SoundManager _instance;


	public AudioClip moveSound;
	public AudioClip creationSound;
	public AudioClip removingSound;
	public AudioClip switchSound;
	public AudioClip newLevelSound;
	public AudioClip lostLevelSound;


	private AudioSource thisAudio;


	/// <summary>
	/// Retrieve the instance of the sound manager.
	/// </summary>
	/// <value>The game manager.</value>
	public static SoundManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.Find("SoundManager").GetComponent<SoundManager>();
			}
			return _instance;
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		thisAudio = audio;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void playMoveSound ()
	{
		thisAudio.PlayOneShot(moveSound, 0.8f);
	}

	public void playCreationSound ()
	{
		thisAudio.PlayOneShot(creationSound, 0.8f);
	}

	public void playRemovingSound ()
	{
		thisAudio.PlayOneShot(removingSound, 0.6f);
	}

	public void playSwitchSound ()
	{
		thisAudio.PlayOneShot(switchSound);
	}

	public void playNewLevelSound ()
	{
		thisAudio.PlayOneShot(newLevelSound);
	}

	public void playLostLevelSound ()
	{
		thisAudio.PlayOneShot(lostLevelSound);
	}
}
