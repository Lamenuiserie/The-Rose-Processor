using UnityEngine;
using System.Collections;

public class Triangle : MonoBehaviour
{



	private float timerTimeToLive;
	private float fadeToDeathTimeElapsed;
	private bool startFadingToDeath;


	private SpriteRenderer thisRenderer;


	// Use this for initialization
	void Start ()
	{
		// Components
		thisRenderer = renderer as SpriteRenderer;

		// Init
		timerTimeToLive = 0.0f;
		startFadingToDeath = false;
		fadeToDeathTimeElapsed = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*timerTimeToLive += Time.deltaTime;


		if (timerTimeToLive > 5)
		{
			startFadingToDeath = true;
			timerTimeToLive = 0;
		}

		if (startFadingToDeath)
		{
			fadeToDeathTimeElapsed += Time.deltaTime;
			thisRenderer.color = Color.Lerp(thisRenderer.color, new Color(thisRenderer.color.r, thisRenderer.color.g, thisRenderer.color.b, 0.0f), fadeToDeathTimeElapsed / 1);

			if (thisRenderer.color.a == 0)
			{
				gameObject.SetActive(false);
			}
		}*/
	}
}
