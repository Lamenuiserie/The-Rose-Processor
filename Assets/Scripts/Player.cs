using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	/// <summary>
	/// The max level of the player.
	/// </summary>
	public int maxLevel;


	/// <summary>
	/// The level at which the player currently is.
	/// </summary>
	public int level { get; private set; }
	/// <summary>
	/// The level selected by the player.
	/// </summary>
	private int levelSelected;
	/// <summary>
	/// The levels that are completed.
	/// </summary>
	public bool[] levelCompleted { get; private set; }
	/// <summary>
	/// The triangles per levels.
	/// </summary>
	private GameObject[,] triangles;
	/// <summary>
	/// The first index from which triangles are activated.
	/// </summary>
	public bool[,] activatedTriangles { get; private set; }

	private int firstActivatedTriangleIndex;
	private int lastActivatedTriangleIndex;


	/// <summary>
	/// The player's transform.
	/// </summary>
	private Transform thisTransform;


	// Use this for initialization
	void Start ()
	{
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
		firstActivatedTriangleIndex = 0;
		lastActivatedTriangleIndex = 0;
		triangles[0,0].SetActive(true);
		activatedTriangles[0,0] = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Activation of the triangles through the mouse wheel
		if (!levelCompleted[levelSelected])
		{
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0)
			{
				Debug.Log(levelSelected);
				bool[] newActivatedTriangles = new bool[thisTransform.GetChild(levelSelected).childCount];
				for (int i = 0; i < thisTransform.GetChild(levelSelected).childCount; i++)
				{
					int newIndex = i - 1;
					if (newIndex < 0)
					{
						newIndex = thisTransform.GetChild(levelSelected).childCount - 1;
					}
					newActivatedTriangles[i] = activatedTriangles[levelSelected, newIndex];
				}

				for (int i = 0; i < thisTransform.GetChild(levelSelected).childCount; i++)
				{
					activatedTriangles[levelSelected,i] = newActivatedTriangles[i];
					triangles[levelSelected,i].SetActive(activatedTriangles[levelSelected,i]);
				}
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.mouseScrollDelta.y < 0)
			{
				bool[] newActivatedTriangles = new bool[thisTransform.GetChild(levelSelected).childCount];
				for (int i = thisTransform.GetChild(levelSelected).childCount - 1; i >= 0; i--)
				{
					int newIndex = i + 1;
					if (newIndex > thisTransform.GetChild(levelSelected).childCount - 1)
					{
						newIndex = 0;
					}
					newActivatedTriangles[i] = activatedTriangles[levelSelected, newIndex];
				}

				for (int i = 0; i < thisTransform.GetChild(levelSelected).childCount; i++)
				{
					activatedTriangles[levelSelected,i] = newActivatedTriangles[i];
					triangles[levelSelected,i].SetActive(activatedTriangles[levelSelected,i]);
				}
			}
		}

		// Switch level
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			levelSelected = 0;
		}
		if (level >= 1 && Input.GetKeyDown(KeyCode.Alpha2))
		{
			levelSelected = 1;
		}
		if (level >= 2 && Input.GetKeyDown(KeyCode.Alpha3))
		{
			levelSelected = 2;
		}

		lightTriangles();

		// TODO remove
		if (Input.GetKeyDown(KeyCode.G))
		{
			level++;
			levelSelected = level;
			triangles[level,0].SetActive(true);
			activatedTriangles[level,0] = true;
		}
	}

	/// <summary>
	/// Creates the triangle.
	/// </summary>
	/// <param name="index">Index at which to create the triangle.</param>
	public void createTriangle (int currentLevel, int index)
	{
		triangles[currentLevel,index].SetActive(true);
		activatedTriangles[currentLevel,index] = true;

		Debug.Log("kjsddkf");

		//levelCompleted[currentLevel] = true;
		for (int i = 0; i < thisTransform.GetChild(currentLevel).childCount; i++)
		{
			if (!activatedTriangles[currentLevel, i])
			{
				//levelCompleted[currentLevel] = false;
			}
		}
		
		if (levelCompleted[currentLevel])
		{
			level = currentLevel + 1;
			triangles[level,0].SetActive(true);
			activatedTriangles[level,0] = true;
			levelSelected = level;
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
				triangleRenderer.color = new Color(triangleRenderer.color.r, triangleRenderer.color.g, triangleRenderer.color.b, 0.8f);
			}
		}
		for (int i = 0; i < maxLevel; i++)
		{
			if (i != levelSelected)
			{
				for (int j = 0; j < thisTransform.GetChild(i).childCount; j++)
				{
					if (activatedTriangles[i, j])
					{
						SpriteRenderer triangleRenderer = triangles[i, j].renderer as SpriteRenderer;
						triangleRenderer.color = new Color(triangleRenderer.color.r, triangleRenderer.color.g, triangleRenderer.color.b, 0.57f);
					}
				}
			}
		}
	}
}
