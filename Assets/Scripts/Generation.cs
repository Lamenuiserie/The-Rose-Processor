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
	/// <summary>
	/// The max number of objects to spawn.
	/// </summary>
	public int maxNumberToSpawn;
	public GameObject object1Prefab;
	public GameObject object2Prefab;

	/// <summary>
	/// The timer spawn.
	/// </summary>
	private float timerSpawn;
	/// <summary>
	/// The starting distance at which the objects spawn.
	/// </summary>
	private float startingDistance;
	private float gapBetweenDistance;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		timerSpawn += Time.deltaTime;

		if (timerSpawn > minTimeBetweenSpawn && timerSpawn < maxTimeBetweenSpawn)
		{
			if (Random.value > 0.5f)
			{
				spawnObjects();
				timerSpawn = 0;
			}
		}
		else if (timerSpawn >= maxTimeBetweenSpawn)
		{
			spawnObjects();
			timerSpawn = 0;
		}
	}

	/// <summary>
	/// Spawn objects.
	/// </summary>
	private void spawnObjects ()
	{
		for (int i = 0; i < maxNumberToSpawn; i++)
		{
			if (Random.value > 0.5f)
			{
				// TODO spawn along circle
				GameObject obj = Instantiate(object1Prefab) as GameObject;
				obj.transform.parent = transform;
			}
		}
	}
}
