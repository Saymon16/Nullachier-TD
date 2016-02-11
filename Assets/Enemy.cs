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

	void OnTriggerEnter (Collider other)
	{			
		if (other.gameObject.tag == "Projectile") {			
			Projectile_Cannon p = other.GetComponent<Projectile_Cannon> ();
			health -= p.damage;
			myrenderer.material.color = Color.red;
			if (health <= 0) {				
				Die ();
			}
			Destroy (p.gameObject);
		}
	}
}

