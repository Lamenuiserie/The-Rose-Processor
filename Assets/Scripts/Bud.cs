using UnityEngine;
using System.Collections;

public class Bud : MonoBehaviour
{
	/// <summary>
	/// The index of the bud.
	/// </summary>
	public int index;

	/// <summary>
	/// The player.
	/// </summary>
	private Player player;

	/// <summary>
	/// The petals of the bud.
	/// </summary>
	public Petal[] petals { get; private set; }
	/// <summary>
	/// Whether the bud is completed.
	/// </summary>
	public bool completed { get; private set; }
	/// <summary>
	/// Whether the bud is completed.
	/// </summary>
	public bool fullyCompleted { get; private set; }

	/// <summary>
	/// The transform of the bud.
	/// </summary>
	private Transform thisTransform;


	void Start ()
	{
		// GameObjects
		player = GameObject.Find("Player").GetComponent<Player>();

		// Components
		thisTransform = transform;

		// Init
		completed = false;
		fullyCompleted = false;
		petals = new Petal[thisTransform.childCount];
		for (int i = 0; i < thisTransform.childCount; i++)
		{
			petals[i] = thisTransform.GetChild(i).GetComponent<Petal>();
			petals[i].setAlive(false);
		}
	}

	void Update ()
	{

	}

	public void movePetalsClockwise ()
	{
		bool[] newActivatedTriangles = new bool[petals.Length];
		for (int i = 0; i < petals.Length; i++)
		{
			if (!petals[i].aligned)
			{
				bool found = false;
				int newIndex = i + 1;
				while (!found && newIndex != i)
				{
					if (newIndex > petals.Length - 1)
					{
						newIndex = 0;
					}
					if (!petals[newIndex].aligned)
					{
						newActivatedTriangles[i] = petals[newIndex].alive;
						found = true;
					}
					newIndex++;
				}
				
				if (!found)
				{
					newActivatedTriangles[i] = petals[i].alive;
				}
			}
			else
			{
				newActivatedTriangles[i] = true;
			}
		}
		
		bool atLeastOneActive = false;
		for (int i = 0; i < petals.Length; i++)
		{
			petals[i].setAlive(newActivatedTriangles[i]);
			
			if (petals[i].alive)
			{
				atLeastOneActive = true;
				player.checkAlignment(i);
			}
		}

		checkFullCompletion();

		if (atLeastOneActive)
		{
			SoundManager.instance.playMoveSound();
		}
	}

	public void movePetalsAntiClockwise ()
	{
		bool[] newActivatedTriangles = new bool[petals.Length];
		for (int i = 0; i < petals.Length; i++)
		{
			if (!petals[i].aligned)
			{
				bool found = false;
				int newIndex = i - 1;
				while (!found && newIndex != i)
				{
					if (newIndex < 0)
					{
						newIndex = petals.Length - 1;
					}
					if (!petals[newIndex].aligned)
					{
						newActivatedTriangles[i] = petals[newIndex].alive;
						found = true;
					}
					newIndex--;
				}

				if (!found)
				{
					newActivatedTriangles[i] = petals[i].alive;
				}
			}
			else
			{
				newActivatedTriangles[i] = true;
			}
		}

		bool atLeastOneActive = false;
		for (int i = 0; i < petals.Length; i++)
		{
			petals[i].setAlive(newActivatedTriangles[i]);
			
			if (petals[i].alive)
			{
				atLeastOneActive = true;
				player.checkAlignment(i);
			}
		}

		checkFullCompletion();
		
		if (atLeastOneActive)
		{
			SoundManager.instance.playMoveSound();
		}
	}

	public void addPetal (int petalIndex)
	{
		SoundManager.instance.playCreationSound();

		petals[petalIndex].setAlive(true);
		
		player.checkAlignment(petalIndex);

		int numberOfActivePetals = 0;
		for (int i = 0; i < petals.Length; i++)
		{
			if (petals[i].alive)
			{
				numberOfActivePetals++;
			}
		}
		
		if (numberOfActivePetals > petals.Length / 2)
		{
			if (!completed)
			{
				SoundManager.instance.playNewLevelSound();
			}
			completed = true;

			checkFullCompletion();

			if (index < player.buds.Length - 1)
			{
				player.checkProgress(index);
			}
			player.end();
		}
	}

	public void deletePetal (int petalIndex)
	{
		SoundManager.instance.playRemovingSound();

		petals[petalIndex].setAlive(false);

		int numberOfActivePetals = 0;
		for (int i = 0; i < petals.Length; i++)
		{
			if (petals[i].alive)
			{
				numberOfActivePetals++;
			}
		}
		if (completed && numberOfActivePetals <= petals.Length / 2)
		{
			completed = false;
			
			SoundManager.instance.playLostLevelSound();

			player.cancelBuds(index + 1);
		}
	}

	private void checkFullCompletion ()
	{
		int numberOfAlignedPetals = 0;
		for (int i = 0; i < petals.Length; i++)
		{
			if (petals[i].aligned)
			{
				numberOfAlignedPetals++;
			}
		}
		if (numberOfAlignedPetals >= petals.Length - 1)
		{
			fullyCompleted = true;
		}
	}

	/// <summary>
	/// Lights the petals.
	/// </summary>
	public void lightPetals ()
	{
		// Better light currently selected bud petals
		for (int i = 0; i < petals.Length; i++)
		{
			if (petals[i].alive)
			{
				SpriteRenderer petalRenderer = petals[i].GetComponent<Renderer>() as SpriteRenderer;
				petalRenderer.color = new Color(petalRenderer.color.r, petalRenderer.color.g, petalRenderer.color.b, 0.85f);
			}
		}
	}

	/// <summary>
	/// Unlights the petals.
	/// </summary>
	public void unlightPetals ()
	{
		for (int j = 0; j < petals.Length; j++)
		{
			if (petals[j].alive)
			{
				SpriteRenderer petalRenderer = petals[j].GetComponent<Renderer>() as SpriteRenderer;
				petalRenderer.color = new Color(petalRenderer.color.r, petalRenderer.color.g, petalRenderer.color.b, 0.46f);
			}
		}
	}
}