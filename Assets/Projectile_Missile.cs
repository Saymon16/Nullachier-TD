using UnityEngine;
using System.Collections;

public class Projectile_Missile : MonoBehaviour
{

	public float speed = 50f;
	public float range = 12f;
	public int damage = 1;
	public Transform origin;
	public Transform target;

	float distance;
	
	// Update is called once per frame
	void Update ()
	{
		target = origin.GetComponent<Turret_Basic> ().target;

		if (target != null) {
			transform.rotation = Quaternion.LookRotation (target.position - transform.position);
		}
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
		distance += Time.deltaTime * speed;
		if (distance >= range) {
			Destroy (gameObject);
		}
	}
}

