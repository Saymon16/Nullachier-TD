using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Turret_Basic : MonoBehaviour
{

	// Use this for initialization


	public enum Sort
	{
		First,
		Random,
		Last}

	;

	public Text displaymode;
	public Sort mode;
	public GameObject projectile;
	public float reloadTime = 1f;
	public float turnSpeed = 5f;
	public float firePauseTime = .25f;
	//public GameObject muzzleFlash;
	public float errorAmount = .001f;
	public Transform target;
	public Vector3 targetLastPos;
	public List<Transform> enemies;
	public Transform[] muzzle;
	public Transform ball;

	Vector3 anticipatedPos;

	float nextFireTime;
	float nextMoveTime;
	float nextEvaluate;
	Quaternion desiredRot;
	float aimError;

	
	// Update is called once per frame

	void Start ()
	{
		
		nextFireTime = Time.time + reloadTime;
		targetLastPos = Vector3.zero;
		nextEvaluate = Time.time + .1f;
		anticipatedPos = Vector3.zero;
	}

	void Update ()
	{

		displaymode.text = mode.ToString ();
		Vector3 rot = Vector3.zero;	

		if (target != null) {
			
			if (Time.time >= nextMoveTime) {
				
				CalculateAimPos (target.position - transform.position); //+ anticipatedPos / 5f
				ball.rotation = Quaternion.Lerp (ball.rotation, desiredRot, Time.deltaTime * turnSpeed);
			}
			if (Time.time >= nextFireTime) {
				FireProjectile ();
			}
			if (Time.time >= nextEvaluate) {
				nextEvaluate = Time.time + .1f; 
				anticipatedPos = (target.position - targetLastPos) * (.1f * Vector3.Distance (transform.position, target.position));
				targetLastPos = target.position;
			}


		} else {
			ChooseNextTarget ();
		}

	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Enemy") {
			enemies.Add (other.transform);
		}
		if (mode != Sort.Random) {
			ChooseNextTarget ();
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.transform == target) {
			target = null;
		}
		if (enemies.Contains (other.transform)) {
			enemies.Remove (other.transform);
		}
	}

	public void ScrollMode ()
	{
		switch (mode) {
		case Sort.First:
			mode = Sort.Last;
			break;		
		case Sort.Last:
			mode = Sort.Random;
			break;
		case Sort.Random:
			mode = Sort.First;
			break;			
		}
	}

	void ChooseNextTarget ()
	{
		nextFireTime = Time.time + reloadTime * 0.5f;
		if (mode == Sort.Random) {
			if (enemies.Count > 0) {
				int r = Random.Range (0, enemies.Count - 1);
				if (enemies [r] == null) {
					enemies.RemoveAt (r);
					ChooseNextTarget ();
				}
				target = enemies [r];
			} else {
				target = null;
			}
		}
		if (mode == Sort.First) {
			if (enemies.Count > 0) {

				if (enemies [0] == null) {
					enemies.RemoveAt (0);
					ChooseNextTarget ();
				}
				target = enemies [0];
			} else {
				target = null;
			}
		}
		if (mode == Sort.Last) {
			if (enemies.Count > 0) {

				if (enemies [enemies.Count - 1] == null) {
					enemies.RemoveAt (enemies.Count - 1);
					ChooseNextTarget ();
				}
				target = enemies [enemies.Count - 1];
			} else {
				target = null;
			}
		}
		/*if (mode == Sort.MostHealth) {
			if (enemies.Count > 0) {
				int max = enemies [0].GetComponent<Enemy> ().health;
				int index = 0;

				for (int k = 0; k < enemies.Count - 1; k++) {
						
					Enemy en = enemies [k].GetComponent<Enemy> ();

					if (en.health > max) {
						max = en.health;
						index = k;
					}
				}
				target = enemies [index];

			} else {
				target = null;
			}
		}
		if (mode == Sort.LeastHealth) {
			if (enemies.Count > 0) {
				int min = enemies [0].GetComponent<Enemy> ().health;
				int index = 0;

				for (int k = 0; k < enemies.Count - 1; k++) {

					Enemy en = enemies [k].GetComponent<Enemy> ();

					if (en.health < min) {
						min = en.health;
						index = k;
					}
				}
				target = enemies [index];

			} else {
				target = null;
			}
		}*/
	}

	void CalculateAimPos (Vector3 pos)
	{
		Vector3 aimpoint = new Vector3 (pos.x + aimError, pos.y + aimError, pos.z + aimError);
		desiredRot = Quaternion.LookRotation (aimpoint);
	}

	void CalculateAimError ()
	{
		aimError = Random.Range (-errorAmount, errorAmount);
	}

	public void RemoveEnemy (Transform me)
	{
		if (enemies.Contains (me)) {
			enemies.Remove (me);
		}
	}

	void FireProjectile ()
	{
		//Audio.Play();
		nextFireTime = Time.time + reloadTime;
		nextMoveTime = Time.time + firePauseTime;
		CalculateAimError ();
		foreach (Transform m in muzzle) {
			GameObject p =	Instantiate (projectile, m.position, m.rotation)as GameObject;
			p.GetComponent<Projectile_Cannon> ().origin = this.transform;
			//Instantiate (muzzleFlash, m.position, m.rotation);
		}
	}




}
