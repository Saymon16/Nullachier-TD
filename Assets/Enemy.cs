using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{

	public int health = 5;
	public float speed = 1.5f;
	public List<Turret_Basic> turrets;

	public Waypoint target;

	Grid_Manager gm;
	public Color mycolor;
	Renderer myrenderer;
	public float waitBeforeGo;

	// Use this for initialization
	void Start ()
	{
		gm = GameObject.FindObjectOfType<Grid_Manager> ();
		myrenderer = transform.GetComponent<Renderer> ();
		myrenderer.material.color	= mycolor;
		Turret_Basic[] tmp = GameObject.FindObjectsOfType<Turret_Basic> ();
		foreach (Turret_Basic t in tmp) {
			turrets.Add (t);
		}

		Waypoint[] waypoints = GameObject.FindObjectsOfType<Waypoint> ();
		float dist = Mathf.Infinity;
		foreach (Waypoint w in waypoints) {
			if (w.isFirst) {
				float d = Vector3.Distance (w.transform.position, transform.position);
				if (d < dist) {
					target = w;
					dist = d;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > waitBeforeGo) {
			myrenderer.material.color = Color.Lerp (myrenderer.material.color, mycolor, Time.deltaTime * 3);
			if (target != null) {

				Vector3 t = new Vector3 (target.transform.position.x, transform.position.y, target.transform.position.z);
				transform.rotation = Quaternion.LookRotation (t - transform.position);
			}
			transform.Translate (Vector3.forward * Time.deltaTime * speed);


		}
		if (health <= 0) {			
			Die ();
		}
	}

	void Die ()
	{		
		foreach (Turret_Basic t in turrets) {
			t.RemoveEnemy (this.transform);
		}
		gm.allEnemies.Remove (this);
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

	public void HasReachedEnd ()
	{
		Destroy (gameObject);
		//loose lives
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

