using UnityEngine;
using System.Collections;

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


	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Update timer
		timerSpawnLevel1 += Time.deltaTime;
		timerSpawnLevel2 += Time.deltaTime;
		timerSpawnLevel3 += Time.deltaTime;

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
			if (player.level == 1 && timerSpawnLevel2 > minTimeBetweenSpawn && timerSpawnLevel2 < maxTimeBetweenSpawn)
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

		Transform obj = Instantiate(level.transform.GetChild((int)(Random.Range(0, 1.1f) * 10))) as Transform;
		obj.parent = transform;
		// TODO color fade
	}
}
