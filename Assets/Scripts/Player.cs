using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	/// <summary>
	/// The max level of the player.
	/// </summary>
	public int maxLevel;

	/// <summary>
	/// The generation script.
	/// </summary>
	private Generation generationScript;
	/// <summary>
	/// The level at which the player currently is.
	/// </summary>
	public int level { get; private set; }
	/// <summary>
	/// The level selected by the player.
	/// </summary>
	public int levelSelected { get; private set; }
	/// <summary>
	/// The levels that are completed.
	/// </summary>
	public bool[] levelCompleted { get; private set; }
	/// <summary>
	/// The triangles per levels.
	/// </summary>
	public GameObject[,] triangles { get; private set; }
	/// <summary>
	/// The first index from which triangles are activated.
	/// </summary>
	public bool[,] activatedTriangles { get; private set; }


	/// <summary>
	/// The player's transform.
	/// </summary>
	private Transform thisTransform;


	// Use this for initialization
	void Start ()
	{
		generationScript = GameObject.Find("DepthMovingObjects").GetComponent<Generation>();

		// Components
		thisTransform = transform;

		// Init
		level = 0;
		levelSelected = 0;
		levelCompleted = new bool[maxLevel];
		activatedTriangles = new bool[maxLevel,thisTransform.GetChild(0).childCount];
		triangles = new GameObject[maxLevel,thisTransform.GetChild(0).childCount];
		for (int i = 0; i < maxLevel; i++)
		{
			for (int j = 0; j < thisTransform.GetChild(i).childCount; j++)
			{
				triangles[i,j] = thisTransform.GetChild(i).GetChild(j).gameObject;
				triangles[i,j].SetActive(false);
				activatedTriangles[i,j] = false;
			}
		}
		triangles[0,0].SetActive(true);
		activatedTriangles[0,0] = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		int numberOfActiveTriangles = 0;
		for (int i = 0; i < thisTransform.GetChild(levelSelected).childCount; i++)
		{
			if (activatedTriangles[levelSelected,i])
			{
				numberOfActiveTriangles++;
			}
		}

		// Activation of the triangles through the mouse wheel
		if (numberOfActiveTriangles < thisTransform.GetChild(levelSelected).childCount)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0)
			{
				bool[] newActivatedTriangles = new bool[thisTransform.GetChild(levelSelected).childCount];
				for (int i = 0; i < thisTransform.GetChild(levelSelected).childCount; i++)
				{
					bool found = false;
					int newIndex = i;
					int currentIndex = i;
					while (!found)
					{
						newIndex = currentIndex - 1;
						if (newIndex < 0)
						{
							newIndex = thisTransform.GetChild(levelSelected).childCount - 1;
						}
						if (currentIndex < 0)
						{
							currentIndex = thisTransform.GetChild(levelSelected).childCount - 1;
						}

						if (!triangles[levelSelected,currentIndex].GetComponent<Triangle>().aligned)
						{
							if (!triangles[levelSelected,newIndex].GetComponent<Triangle>().aligned)
							{
								newActivatedTriangles[i] = activatedTriangles[levelSelected, newIndex];
								found = true;
							}
						}
						else
						{
							newActivatedTriangles[currentIndex] = true;
						}
						currentIndex--;
					}
				}

				bool atLeastOneActive = false;
				for (int i = 0; i < thisTransform.GetChild(levelSelected).childCount; i++)
				{
					activatedTriangles[levelSelected,i] = newActivatedTriangles[i];
					triangles[levelSelected,i].SetActive(activatedTriangles[levelSelected,i]);

					if (activatedTriangles[levelSelected,i])
					{
						atLeastOneActive = true;
					}
				}

				if (atLeastOneActive)
				{
					SoundManager.instance.playMoveSound();
				}
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.mouseScrollDelta.y < 0)
			{
				bool[] newActivatedTriangles = new bool[thisTransform.GetChild(levelSelected).childCount];
				for (int i = thisTransform.GetChild(levelSelected).childCount - 1; i >= 0; i--)
				{
					bool found = false;
					int newIndex = i;
					int currentIndex = i;
					while (!found)
					{
						newIndex = currentIndex + 1;
						if (newIndex > thisTransform.GetChild(levelSelected).childCount - 1)
						{
							newIndex = 0;
						}

						if (currentIndex > thisTransform.GetChild(levelSelected).childCount - 1)
						{
							currentIndex = 0;
						}

						if (!triangles[levelSelected,currentIndex].GetComponent<Triangle>().aligned)
						{
							if (!triangles[levelSelected,newIndex].GetComponent<Triangle>().aligned)
							{
								newActivatedTriangles[i] = activatedTriangles[levelSelected, newIndex];
								found = true;
							}
						}
						else
						{
							newActivatedTriangles[currentIndex] = true;
						}
						currentIndex++;
					}
				}

				bool atLeastOneActive = false;
				for (int i = 0; i < thisTransform.GetChild(levelSelected).childCount; i++)
				{
					activatedTriangles[levelSelected,i] = newActivatedTriangles[i];
					triangles[levelSelected,i].SetActive(activatedTriangles[levelSelected,i]);

					if (activatedTriangles[levelSelected,i])
					{
						atLeastOneActive = true;
					}
				}

				if (atLeastOneActive)
				{
					SoundManager.instance.playMoveSound();
				}
			}
		}

		// Switch level
		if (Input.GetKeyDown(KeyCode.Alpha1) && levelSelected != 0)
		{
			levelSelected = 0;
			SoundManager.instance.playSwitchSound();
		}
		if (level >= 1 && Input.GetKeyDown(KeyCode.Alpha2) && levelSelected != 1)
		{
			levelSelected = 1;
			SoundManager.instance.playSwitchSound();
		}
		if (level >= 2 && Input.GetKeyDown(KeyCode.Alpha3) && levelSelected != 2)
		{
			levelSelected = 2;
			SoundManager.instance.playSwitchSound();
		}
		if (level >= 3 && Input.GetKeyDown(KeyCode.Alpha4) && levelSelected != 3)
		{
			levelSelected = 3;
			SoundManager.instance.playSwitchSound();
		}

		lightTriangles();
	}

	/// <summary>
	/// Creates the triangle.
	/// </summary>
	/// <param name="currentLevel">Current level.</param>
	/// <param name="index">Index at which to create the triangle.</param>
	public void createTriangle (int currentLevel, int index)
	{
		SoundManager.instance.playCreationSound();

		triangles[currentLevel,index].SetActive(true);
		activatedTriangles[currentLevel,index] = true;

		bool aligned = true;
		for (int i = 0; i < maxLevel; i++)
		{
			if (!activatedTriangles[i,index])
			{
				aligned = false;
			}
		}

		/*if (aligned)
		{
			// FIXME It seems like it does not work on index 0
			Debug.Log("ALIGNED");
			for (int i = 0; i < maxLevel; i++)
			{
				generationScript.killObjectsAtIndex(i,index);
				triangles[i,index].GetComponent<Triangle>().aligned = true;
			}
		}*/

		int numberOfActiveTriangles = 0;
		for (int i = 0; i < thisTransform.GetChild(currentLevel).childCount; i++)
		{
			if (activatedTriangles[currentLevel, i])
			{
				numberOfActiveTriangles++;
			}
		}

		if (numberOfActiveTriangles > thisTransform.GetChild(currentLevel).childCount / 2)
		{
			if (!levelCompleted[currentLevel])
			{
				SoundManager.instance.playNewLevelSound();
			}

			levelCompleted[currentLevel] = true;

			if (level < currentLevel + 1)
			{
				level = currentLevel + 1;
				if (level == maxLevel)
				{
					// TODO end game
					level--;
				}
				else
				{
					triangles[level,0].SetActive(true);
					activatedTriangles[level,0] = true;
				}
			}
		}
	}

	/// <summary>
	/// Removes the triangle.
	/// </summary>
	/// <param name="currentLevel">Current level.</param>
	/// <param name="index">Index.</param>
	public void removeTriangle (int currentLevel, int index)
	{
		SoundManager.instance.playRemovingSound();

		triangles[currentLevel,index].SetActive(false);
		activatedTriangles[currentLevel,index] = false;

		int numberOfActiveTriangles = 0;
		for (int i = 0; i < thisTransform.GetChild(currentLevel).childCount; i++)
		{
			if (activatedTriangles[currentLevel, i])
			{
				numberOfActiveTriangles++;
			}
		}
		
		if (levelCompleted[currentLevel] && numberOfActiveTriangles <= thisTransform.GetChild(currentLevel).childCount / 2)
		{
			levelCompleted[currentLevel] = false;

			SoundManager.instance.playLostLevelSound();

			for (int i = currentLevel + 1; i <= level; i++)
			{
				generationScript.killObjects(i);

				for (int j = 0; j < thisTransform.GetChild(i).childCount; j++)
				{
					if (!triangles[i,j].GetComponent<Triangle>().aligned)
					{
						triangles[i,j].SetActive(false);
						activatedTriangles[i,j] = false;
					}
				}
			}

			level = currentLevel;
			if (levelSelected > currentLevel)
			{
				levelSelected = level;
			}
		}
	}

	/// <summary>
	/// Lights the triangles.
	/// </summary>
	private void lightTriangles ()
	{
		// Better light currently selected level triangles
		for (int i = 0; i < thisTransform.GetChild(levelSelected).childCount; i++)
		{
			if (activatedTriangles[levelSelected, i])
			{
				SpriteRenderer triangleRenderer = triangles[levelSelected, i].renderer as SpriteRenderer;
				triangleRenderer.color = new Color(triangleRenderer.color.r, triangleRenderer.color.g, triangleRenderer.color.b, 0.85f);
			}
		}
		for (int i = 0; i < maxLevel; i++)
		{
			if (i != levelSelected)
			{
				for (int j = 0; j < thisTransform.GetChild(i).childCount; j++)
				{
					if (activatedTriangles[i,j])
					{
						SpriteRenderer triangleRenderer = triangles[i,j].renderer as SpriteRenderer;
						triangleRenderer.color = new Color(triangleRenderer.color.r, triangleRenderer.color.g, triangleRenderer.color.b, 0.46f);
					}
				}
			}

			// TODO remove
			/*for (int j = 0; j < thisTransform.GetChild(i).childCount; j++)
			{
				if (triangles[i,j].GetComponent<Triangle>().aligned)
				{
					//SpriteRenderer triangleRenderer = triangles[i,j].renderer as SpriteRenderer;
					//triangleRenderer.color = new Color(0, 0, 1, 0.85f);
				}
			}*/
		}
	}
}
