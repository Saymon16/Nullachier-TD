using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

	public int health = 5;



	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (health <= 0) {
			
			Die ();
		}
	}

	void Die ()
	{
		Transform t = null;
		Die (t);
	}

	void Die (Transform p)
	{
		if (p != null) {
			p.GetComponent<Turret_Basic> ().RemoveEnemy (this.transform);
		}
		Destroy (gameObject);
	}

	void OnTriggerEnter (Collider other)
	{			
		if (other.gameObject.tag == "Projectile") {			
			Projectile_Cannon p = other.GetComponent<Projectile_Cannon> ();
			health -= p.damage;
			if (health <= 0) {				
				Die (p.origin);
			}
		}
	}
}

