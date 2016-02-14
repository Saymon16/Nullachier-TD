using UnityEngine;
using System.Collections;


[RequireComponent (typeof(Light))]
public class Flash : MonoBehaviour
{

	public Light obj;

	public float delay;
	public float ontime;
	public float offtime;
	public float fadeout;
	public float fadein;

	float a, b, c, d;
	float t;

	float intensityToReach;

	// Use this for initialization
	void Start ()
	{
		intensityToReach = obj.intensity;
		obj.intensity = 0;
		obj.enabled = false;
		a = delay;
		b = a + fadein;
		c = b + ontime;
		d = c + fadeout;
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		t += Time.deltaTime;

		//delay
		if (t > a) {	
			//fadein
			obj.enabled = true;
			Mathf.Lerp (0, intensityToReach, (t - a) / (b - a));
		}

		if (t > b) {
			//ontime
			obj.intensity = intensityToReach;
		}
				
		if (t > c) {
			//fadeout
			Mathf.Lerp (intensityToReach, 0, (t - c) / (d - c));
		}

		if (t > d) {
			//offtime
			obj.enabled = false;
			Destroy (gameObject);
		}		
	}
}
