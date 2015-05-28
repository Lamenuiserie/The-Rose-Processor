using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	/// <summary>
	/// The buds.
	/// </summary>
	public Bud[] buds { get; private set; }
	/// <summary>
	/// The level at which the player currently is.
	/// </summary>
	public int level { get; private set; }
	/// <summary>
	/// The current bud selected.
	/// </summary>
	public int budSelected { get; private set; }
	/// <summary>
	/// Whether the flower has been completed.
	/// </summary>
	public bool flowerCompleted { get; private set; }
	/// <summary>
	/// The generation script.
	/// </summary>
	private Generation generationScript;


	/// <summary>
	/// The player's transform.
	/// </summary>
	private Transform thisTransform;


	// Use this for initialization
	void Start ()
	{
		Input.simulateMouseWithTouches = true;

		// Scripts
		generationScript = GameObject.Find("DepthMovingObjects").GetComponent<Generation>();

		// Components
		thisTransform = transform;

		// Init
		level = 0;
		budSelected = 0;
		buds = new Bud[thisTransform.childCount];
		for (int i = 0; i < thisTransform.childCount; i++)
		{
			buds[i] = thisTransform.GetChild(i).GetComponent<Bud>();
		}
		buds[0].petals[0].setAlive(true);
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Find the number of active petals
		int numberOfActivePetals = 0;
		for (int i = 0; i < buds[budSelected].petals.Length; i++)
		{
			if (buds[budSelected].petals[i].alive)
			{
				numberOfActivePetals++;
			}
		}
		
		// Activation of the triangles through the mouse wheel
		if (numberOfActivePetals < buds[budSelected].petals.Length)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0 || (Input.touchCount > 0 && Input.GetTouch(0).deltaPosition.y > 0))
			{
				buds[budSelected].movePetalsAntiClockwise();
				end ();
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.mouseScrollDelta.y < 0 || (Input.touchCount > 0 && Input.GetTouch(0).deltaPosition.y < 0))
			{
				buds[budSelected].movePetalsClockwise();
				end ();
			}
		}

		// TODO remove
		if (Input.GetKeyDown(KeyCode.R))
		{
			for (int i = 0; i < buds.Length; i++)
			{
				generationScript.killObjects(i);
			}
		}

		// Switch level
		if (Input.GetKeyDown(KeyCode.Alpha1) && budSelected != 0)
		{
			budSelected = 0;
			SoundManager.instance.playSwitchSound();
		}
		if (level >= 1 && Input.GetKeyDown(KeyCode.Alpha2) && budSelected != 1)
		{
			budSelected = 1;
			SoundManager.instance.playSwitchSound();
		}
		if (level >= 2 && Input.GetKeyDown(KeyCode.Alpha3) && budSelected != 2)
		{
			budSelected = 2;
			SoundManager.instance.playSwitchSound();
		}
		if (level >= 3 && Input.GetKeyDown(KeyCode.Alpha4) && budSelected != 3)
		{
			budSelected = 3;
			SoundManager.instance.playSwitchSound();
		}

		// Light the bud selected petals
		for (int i = 0; i < buds.Length; i++)
		{
			if (i == budSelected)
			{
				buds[i].lightPetals();
			}
			else
			{
				buds[i].unlightPetals();
			}
		}
	}

	public void checkProgress (int budIndex)
	{
		if (level < budIndex + 1)
		{
			level = budIndex + 1;
			buds[level].petals[0].setAlive(true);
		}
	}

	public void end ()
	{
		// TODO make buds blink and then full highlight
		bool fullyCompeted = false;
		for (int i = 0; i < buds.Length; i++)
		{
			if (buds[i].fullyCompleted)
			{
				fullyCompeted = true;
			}
		}
		if (fullyCompeted)
		{
			flowerCompleted = true;
		}
	}

	public bool isLastBranch ()
	{
		for (int i = 0; i < buds.Length; i++)
		{
			for (int j = 0; j < buds[i].petals.Length; j++)
			{

			}
		}
		return true;
	}

	public void cancelBuds (int budIndex)
	{
		for (int i = budIndex; i <= level; i++)
		{
			generationScript.killObjects(i);
			
			for (int j = 0; j < buds[i].petals.Length; j++)
			{
				if (!buds[i].petals[j].aligned)
				{
					buds[i].petals[j].setAlive(false);
				}
			}
		}
		
		level = budIndex - 1;
		if (budSelected > budIndex)
		{
			budSelected = level;
		}
	}

	/// <summary>
	/// Checks the alignment of petals.
	/// </summary>
	/// <param name="index">Index of the petals to check.</param>
	public void checkAlignment (int index)
	{
		bool aligned = true;
		for (int i = 0; i < buds.Length; i++)
		{
			if (!buds[i].petals[index].alive)
			{
				aligned = false;
			}
		}
		
		if (aligned)
		{
			for (int i = 0; i < buds.Length; i++)
			{
				generationScript.killObjectsAtIndex(i,index);
				buds[i].petals[index].aligned = true;
			}
		}
	}
}
