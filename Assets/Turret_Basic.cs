using UnityEngine;
using System.Collections;

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
		if (target) {
			if (Time.time >= nextMoveTime) {
				CalculateAimPos (target.position);
				ball.rotation = Quaternion.Lerp (ball.rotation, desiredRot, Time.deltaTime * turnSpeed);
			}
			if (Time.time >= nextFireTime) {
				FireProjectile ();
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Enemy") {
			nextFireTime = Time.time + (reloadTime * 0.5f);
			target = other.gameObject.transform;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.transform == target) {
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

	void FireProjectile ()
	{
		//Audio.Play();
		nextFireTime = Time.time + reloadTime;
		nextMoveTime = Time.time + firePauseTime;
		CalculateAimError ();
		foreach (Transform m in muzzle) {
			Instantiate (projectile, m.position, m.rotation);
			//Instantiate (muzzleFlash, m.position, m.rotation);
		}
	}




}
