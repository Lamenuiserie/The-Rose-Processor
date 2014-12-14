using UnityEngine;
using System.Collections;

public class Triangle : MonoBehaviour
{
	public int level;
	public int index;

	public bool aligned { get; set; }

	// Components
	private SpriteRenderer thisRenderer;


	// Use this for initialization
	void Start ()
	{
		aligned = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
