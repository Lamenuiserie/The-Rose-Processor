using UnityEngine;
using System.Collections.Generic;

public class Generation : MonoBehaviour
{
	/// <summary>
	/// The maximum time between spawn.
	/// </summary>
	public float maxTimeBetweenSpawn;
	/// <summary>
	/// The minimum time between spawn.
	/// </summary>
	public float minTimeBetweenSpawn;
	public GameObject level1;
	public GameObject level2;
	public GameObject level3;
	public GameObject level4;

	/// <summary>
	/// The player.
	/// </summary>
	private Player player;
	/// <summary>
	/// The timer spawn.
	/// </summary>
	private float timerSpawnLevel1;
	private float timerSpawnLevel2;
	private float timerSpawnLevel3;
	private float timerSpawnLevel4;


	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!player.flowerCompleted)
		{
			// Update timer
			timerSpawnLevel1 += Time.deltaTime;
			timerSpawnLevel2 += Time.deltaTime;
			timerSpawnLevel3 += Time.deltaTime;
			timerSpawnLevel4 += Time.deltaTime;

			// Spawn level 1 objects
			if (timerSpawnLevel1 > minTimeBetweenSpawn && timerSpawnLevel1 < maxTimeBetweenSpawn)
			{
				if (Random.value > 0.5f)
				{
					spawnObjects(0);
					timerSpawnLevel1 = 0;
				}
			}
			else if (timerSpawnLevel1 >= maxTimeBetweenSpawn)
			{
				spawnObjects(0);
				timerSpawnLevel1 = 0;
			}

			// Spawn level 2 objects
			if (player.level >= 1)
			{
				if (timerSpawnLevel2 > minTimeBetweenSpawn && timerSpawnLevel2 < maxTimeBetweenSpawn)
				{
					if (Random.value > 0.5f)
					{
						spawnObjects(1);
						timerSpawnLevel2 = 0;
					}
				}
				else if (timerSpawnLevel2 >= maxTimeBetweenSpawn)
				{
					spawnObjects(1);
					timerSpawnLevel2 = 0;
				}
			}

			// Spawn level 3 objects
			if (player.level >= 2)
			{
				if (timerSpawnLevel3 > minTimeBetweenSpawn && timerSpawnLevel3 < maxTimeBetweenSpawn)
				{
					if (Random.value > 0.5f)
					{
						spawnObjects(2);
						timerSpawnLevel3 = 0;
					}
				}
				else if (timerSpawnLevel3 >= maxTimeBetweenSpawn)
				{
					spawnObjects(2);
					timerSpawnLevel3 = 0;
				}
			}

			// Spawn level 4 objects
			if (player.level >= 3)
			{
				if (timerSpawnLevel4 > minTimeBetweenSpawn && timerSpawnLevel4 < maxTimeBetweenSpawn)
				{
					if (Random.value > 0.5f)
					{
						spawnObjects(3);
						timerSpawnLevel4 = 0;
					}
				}
				else if (timerSpawnLevel4 >= maxTimeBetweenSpawn)
				{
					spawnObjects(3);
					timerSpawnLevel4 = 0;
				}
			}
		}
		else
		{
			for (int i = 0; i < player.buds.Length; i++)
			{
				for (int j = 0; j < player.buds[i].petals.Length; j++)
				{
					if (!player.buds[i].petals[j].alive)
					{
						bool createOne = true;
						for (int x = 0; x < transform.childCount; x++)
						{
							if (transform.GetChild(x).GetComponent<DepthMovement>().index == j)
							{
								createOne = false;
							}
						}
						if (createOne)
						{
							GameObject level = level1;
							if (i == 1)
							{
								level = level2;
							}
							else if (i == 2)
							{
								level = level3;
							}
							else if (i == 3)
							{
								level = level4;
							}
							Transform obj = Instantiate(level.transform.GetChild(j)) as Transform;
							obj.parent = transform;
						}
					}
					else
					{
						killObjectsAtIndex(i, j);
					}
				}
			}
		}
	}

	/// <summary>
	/// Spawn objects.
	/// </summary>
	private void spawnObjects (int levelIndex)
	{
		GameObject level = level1;
		if (levelIndex == 1)
		{
			level = level2;
		}
		else if (levelIndex == 2)
		{
			level = level3;
		}
		else if (levelIndex == 3)
		{
			level = level4;
		}

		// Find the possible indices at which spawning new petals
		List<int> possibleIndices = new List<int>();
		for (int i = 0; i < player.buds[levelIndex].petals.Length; i++)
		{
			if (!player.buds[levelIndex].petals[i].aligned)
			{
				possibleIndices.Add(i);
			}
		}
		for (int i = 0; i < transform.childCount; i++)
		{
			DepthMovement depthObj = transform.GetChild(i).GetComponent<DepthMovement>();
			if (depthObj.bud == levelIndex)
			{
				possibleIndices.Remove(depthObj.index);
			}
		}

		if (possibleIndices.Count > 0)
		{
			int index = Random.Range(0, possibleIndices.Count - 1);
			Transform obj = Instantiate(level.transform.GetChild(possibleIndices[index])) as Transform;
			obj.parent = transform;
		}
	}

	public void killObjects (int bud)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).GetComponent<DepthMovement>().bud == bud)
			{
				Destroy(transform.GetChild(i).gameObject);
			}
		}
	}

	public void killObjectsAtIndex (int bud, int index)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).GetComponent<DepthMovement>().bud == bud && transform.GetChild(i).GetComponent<DepthMovement>().index == index)
			{
				Destroy(transform.GetChild(i).gameObject);
			}
		}
	}
}
