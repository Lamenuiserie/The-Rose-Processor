using UnityEngine;
using System.Collections;

public class Petal : MonoBehaviour
{
	/// <summary>
	/// Index of the petal.
	/// </summary>
	public int index;

	/// <summary>
	/// Whether the petal is alive.
	/// </summary>
	public bool alive { get; private set; }
	/// <summary>
	/// Whether the petal is aligned.
	/// </summary>
	public bool aligned { get; set; }

	/// <summary>
	/// The petal renderer.
	/// </summary>
	private SpriteRenderer thisRenderer;


	// Use this for initialization
	void Start ()
	{
		// Components
		thisRenderer = GetComponent<Renderer>() as SpriteRenderer;

		// Init
		aligned = false;
		alive = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	public void setAlive (bool alive)
	{
		this.alive = alive;
		thisRenderer.enabled = alive;
	}
}
