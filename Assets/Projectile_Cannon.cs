using UnityEngine;
using System.Collections;

public class Projectile_Cannon : MonoBehaviour
{

	public float speed = 10f;
	public float range = 10f;
	public int damage = 1;
	public Transform origin;

	float distance;
	
	// Update is called once per frame
	void Update ()
	{
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
		distance += Time.deltaTime * speed;
		if (distance >= range) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter (Collider other)
	{				
		if (other.gameObject.tag == "Enemy") {
			Enemy e = other.GetComponent<Enemy> ();
			e.health -= damage;
		}
	}


}

