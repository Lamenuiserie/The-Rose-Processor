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
		// Components
		thisTransform = transform;
		thisRenderer = renderer as SpriteRenderer;

		// Init
		depth = 0;
		fadeToDeathTimeElapsed = 0.0f;
		thisTransform.rotation = Quaternion.FromToRotation(Vector3.down, thisTransform.position);
	}

	// Update is called once per frame
	void Update ()
	{
		// TODO rotate
		// TODO oriented toward center of the screen

		// Update the depth at which the object is
		if (depth <= 1)
		{
			updateDepth();
		}
		else
		{
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
