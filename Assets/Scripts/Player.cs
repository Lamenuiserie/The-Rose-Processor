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
	private int level;
	/// <summary>
	/// The level selected by the player.
	/// </summary>
	private int levelSelected;
	/// <summary>
	/// The levels that are completed.
	/// </summary>
	private bool[] levelCompleted;
	/// <summary>
	/// The triangles per levels.
	/// </summary>
	private GameObject[,] triangles;
	/// <summary>
	/// The first index from which triangles are activated.
	/// </summary>
	private bool[,] activatedTriangles;

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
		if (Mathf.Abs(lastActivatedTriangleIndex - firstActivatedTriangleIndex) > 1 || firstActivatedTriangleIndex == lastActivatedTriangleIndex)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0)
			{
				int newLastIndex = lastActivatedTriangleIndex + 1;
				if (newLastIndex >= thisTransform.GetChild(levelSelected).childCount)
				{
					newLastIndex = 0;
				}
				triangles[levelSelected,newLastIndex].SetActive(true);
				activatedTriangles[levelSelected,newLastIndex] = true;
				lastActivatedTriangleIndex = newLastIndex;
				
				triangles[levelSelected,firstActivatedTriangleIndex].SetActive(false);
				activatedTriangles[levelSelected,firstActivatedTriangleIndex] = false;
				int newFirstIndex = firstActivatedTriangleIndex + 1;
				if (newFirstIndex >= thisTransform.GetChild(levelSelected).childCount)
				{
					newFirstIndex = 0;
				}
				firstActivatedTriangleIndex = newFirstIndex;
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.mouseScrollDelta.y < 0)
			{
				int newFirstIndex = firstActivatedTriangleIndex - 1;
				if (newFirstIndex < 0)
				{
					newFirstIndex = thisTransform.GetChild(levelSelected).childCount - 1;
				}
				triangles[levelSelected,newFirstIndex].SetActive(true);
				activatedTriangles[levelSelected,newFirstIndex] = true;
				firstActivatedTriangleIndex = newFirstIndex;
				
				triangles[levelSelected,lastActivatedTriangleIndex].SetActive(false);
				activatedTriangles[levelSelected,lastActivatedTriangleIndex] = false;
				int newLastIndex = lastActivatedTriangleIndex - 1;
				if (newLastIndex < 0)
				{
					newLastIndex = thisTransform.GetChild(levelSelected).childCount - 1;
				}
				lastActivatedTriangleIndex = newLastIndex;
			}
		}

		// Adding a triangle at the level of the player
		if (Input.GetKeyDown(KeyCode.Space) && !levelCompleted[level])
		{
			int newIndex = lastActivatedTriangleIndex + 1;
			if (newIndex >= thisTransform.GetChild(level).childCount)
			{
				newIndex = 0;
			}
			triangles[level,newIndex].SetActive(true);
			activatedTriangles[level,newIndex] = true;
			lastActivatedTriangleIndex = newIndex;

			if (lastActivatedTriangleIndex - firstActivatedTriangleIndex == -1)
			{
				levelCompleted[level] = true;
				//TODO level++;
			}
		}
	}

	/// <summary>
	/// Return the index of the first activated triangle.
	/// </summary>
	/// <returns>The index of the first activated triangle.</returns>
	private int getFirstActivatedTriangleIndex(int level)
	{
		for (int i = 0; i < thisTransform.GetChild(level).childCount; i++)
		{
			if (activatedTriangles[level, i])
			{
				return i;
			}
		}
		return -1;
	}

	/// <summary>
	/// Return the index of the last activated triangle.
	/// </summary>
	/// <returns>The index of the last activated triangle.</returns>
	private int getLastActivatedTriangleIndex(int level)
	{
		bool found = false;
		bool foundOne = false;
		int i = 0;
		while (!found)
		{
			if (foundOne)
			{
				if (i >= thisTransform.GetChild(level).childCount)
				{
					return i - 1;
				}
				else if (!activatedTriangles[level,i])
				{
					return i - 1;
				}
			}
			else if (activatedTriangles[level,i])
			{
				foundOne = true;
			}
			i++;
		}
		return -1;



		/*for (int i = 0; i < thisTransform.GetChild(level).childCount; i++)
		{
			if (activatedTriangles[level, i])
			{
				if (i + 1 >= thisTransform.GetChild(level).childCount)
				{
					return i;
				}
				else if (!activatedTriangles[level, i + 1])
				{
					return i;
				}
			}
		}
		return -1;*/
	}
}
