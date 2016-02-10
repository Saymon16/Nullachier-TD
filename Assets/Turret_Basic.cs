using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret_Basic : MonoBehaviour
{

	// Use this for initialization

	public GameObject projectile;
	public float reloadTime = 1f;
	public float turnSpeed = 5f;
	public float firePauseTime = .25f;
	//public GameObject muzzleFlash;
	public float errorAmount = .001f;
	public Transform target;
	public List<Transform> enemies;
	public Transform[] muzzle;
	public Transform ball;

	float nextFireTime;
	float nextMoveTime;
	Quaternion desiredRot;
	float aimError;

	
	// Update is called once per frame

	void Start ()
	{
		
		nextFireTime = Time.time + reloadTime;
	}

	void Update ()
	{
		if (target != null) {
			if (Time.time >= nextMoveTime) {
				CalculateAimPos (target.position);
				ball.rotation = Quaternion.Lerp (ball.rotation, desiredRot, Time.deltaTime * turnSpeed);
			}
			if (Time.time >= nextFireTime) {
				FireProjectile ();
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

	void ChooseNextTarget ()
	{
		nextFireTime = Time.time + reloadTime * 0.5f;
		if (enemies.Count > 0) {
			target = enemies [0];
		} else {
			target = null;
		}
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
