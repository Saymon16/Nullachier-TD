using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{

	public int health = 5;
	public float speed = 1.5f;
	public List<Turret_Basic> turrets;

	Color mycolor;
	Renderer myrenderer;


	// Use this for initialization
	void Start ()
	{
		myrenderer = transform.GetComponent<Renderer> ();
		mycolor = myrenderer.material.color;
		Turret_Basic[] tmp = GameObject.FindObjectsOfType<Turret_Basic> ();
		foreach (Turret_Basic t in tmp) {
			turrets.Add (t);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		myrenderer.material.color = Color.Lerp (myrenderer.material.color, mycolor, Time.deltaTime * 3);

		transform.Translate (Vector3.forward * Time.deltaTime * speed);

		if (health <= 0) {			
			Die ();
		}
	}

	void Die ()
	{		
		foreach (Turret_Basic t in turrets) {
			t.RemoveEnemy (this.transform);
		}
		Destroy (gameObject);
	}

	void AOE (float radius, float damage)
	{
		Collider[] cols = Physics.OverlapSphere (transform.position, radius + 1);
		foreach (Collider col in cols) {
			if (col.gameObject.tag == "Enemy") {
				if (Vector3.Distance (col.transform.position, transform.position) <= radius) {
					Enemy e = col.GetComponent<Enemy> ();
					e.health -= Mathf.RoundToInt (damage);
					e.myrenderer.material.color = Color.red;
				}
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{			
		if (other.gameObject.tag == "Projectile_Cannon") {			
			Projectile_Cannon p = other.GetComponent<Projectile_Cannon> ();
			health -= p.damage;
			myrenderer.material.color = Color.red;
			if (health <= 0) {				
				Die ();
			}
			Destroy (p.gameObject);
		}
		if (other.gameObject.tag == "Projectile_Missile") {			
			Projectile_Missile p = other.GetComponent<Projectile_Missile> ();
			health -= p.damage;
			AOE (3, p.damage / 2);
			myrenderer.material.color = Color.red;
			if (health <= 0) {				
				Die ();
			}
			Destroy (p.gameObject);
		}
	}
}

