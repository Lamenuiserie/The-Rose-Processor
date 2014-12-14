using UnityEngine;
using System.Collections;

public class DepthMovement : MonoBehaviour
{
	/// <summary>
	/// Speed at which it approaches.
	/// </summary>
	[Range(0,1)]
	public float speed;
	/// <summary>
	/// The index of the object.
	/// </summary>
	public int index;
	/// <summary>
	/// The level.
	/// </summary>
	public int level;
	/// <summary>
	/// The player.
	/// </summary>
	private Player player;
	/// <summary>
	/// Depth at which the object is.
	/// Range from zero (invisible) to 1 (visible at scale 1).
	/// </summary>
	private float depth;
	/// <summary>
	/// Time elapsed since the object started to fade to death.
	/// </summary>
	private float fadeToDeathTimeElapsed;


	/// <summary>
	/// The object's transform.
	/// </summary>
	private Transform thisTransform;
	/// <summary>
	/// The object's sprite renderer.
	/// </summary>
	private SpriteRenderer thisRenderer;


	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find("Player").GetComponent<Player>();

		// Components
		thisTransform = transform;
		thisRenderer = renderer as SpriteRenderer;

		// Init
		depth = 0;
		fadeToDeathTimeElapsed = 0.0f;
		//thisTransform.rotation = Quaternion.FromToRotation(Vector3.down, thisTransform.position);
	}

	/// <summary>
	/// Whether the triangle was created already.
	/// </summary>
	private bool created = false;

	// Update is called once per frame
	void Update ()
	{
		// Update the depth at which the object is
		if (depth <= 1f)
		{
			updateDepth();
		}
		else
		{
			// If no triangle create one
			if (!created && fadeToDeathTimeElapsed == 0)
			{
				if (!player.activatedTriangles[level, index])
				{
					player.createTriangle(level, index);
				}
				else
				{
					player.removeTriangle(level, index);
				}
				created = true;
			}
			fadeToDeath();
		}
	}

	/// <summary>
	/// Updates the depth of the object.
	/// </summary>
	void updateDepth ()
	{
		depth += speed * Time.deltaTime;
		thisTransform.localScale = new Vector3(depth, depth, 1);
	}

	/// <summary>
	/// Fades the object until it disappears and destroys it.
	/// </summary>
	void fadeToDeath ()
	{
		fadeToDeathTimeElapsed += Time.deltaTime;
		thisRenderer.color = Color.Lerp(thisRenderer.color, new Color(thisRenderer.color.r, thisRenderer.color.g, thisRenderer.color.b, 0.0f), fadeToDeathTimeElapsed / 1);
		if (thisRenderer.color.a <= 0)
		{
			Destroy(gameObject);
		}
	}
}
