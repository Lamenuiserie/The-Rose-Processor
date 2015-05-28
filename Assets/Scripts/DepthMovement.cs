using UnityEngine;
using System.Collections;

public class DepthMovement : MonoBehaviour
{
	/// <summary>
	/// Speed at which the petal to be approaches.
	/// </summary>
	[Range(0,1)]
	public float speed;
	/// <summary>
	/// The index of the petal to be.
	/// </summary>
	public int index;
	/// <summary>
	/// The bud at wich the petal to be is.
	/// </summary>
	public int bud;

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
		thisRenderer = GetComponent<Renderer>() as SpriteRenderer;

		// Init
		depth = 0;
		fadeToDeathTimeElapsed = 0.0f;
	}

	/// <summary>
	/// Whether the petal was created already.
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
			// If no petal present create one
			if (!created && fadeToDeathTimeElapsed == 0)
			{
				if (!player.buds[bud].petals[index].alive)
				{
					player.buds[bud].addPetal(index);
				}
				else
				{
					player.buds[bud].deletePetal(index);
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
