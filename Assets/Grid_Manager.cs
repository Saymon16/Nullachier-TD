using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid_Manager : MonoBehaviour
{


	public GameObject lastHitObj;
	public LayerMask PlacementLayerMask;

	public List<Enemy> allEnemies;
	public List<Turret_Basic> allTurrets;
	public GameObject turret;


	public Material normal;
	public Material highlight;


	// Use this for initialization
	void Start ()
	{
		lastHitObj = null;
	}
	
	// Update is called once per frame

	public void RefreshBoard ()
	{
		if (allEnemies.Count > 0 && allTurrets.Count > 0) {
			foreach (Enemy e in allEnemies) {	
				if (e == null) {
					continue;
				}
				foreach (Turret_Basic t in allTurrets) {
					if (t == null)
						continue;
					if (e.turrets.Contains (t)) {
						continue;
					}
					e.turrets.Add (t);
				}
			}
		}
	}

	void Update ()
	{

		if (Input.GetMouseButtonDown (0) && lastHitObj != null && lastHitObj.tag == "Placement_Plane_Open") {
			lastHitObj.tag = "Placement_Plane_Closed";
			GameObject g = Instantiate (turret, lastHitObj.transform.position, Quaternion.identity) as GameObject;
			Turret_Basic t = g.GetComponent<Turret_Basic> ();
			t.mode = Turret_Basic.Sort.Last;
			allTurrets.Add (t);
			RefreshBoard ();
			t.transform.GetChild (0).transform.rotation = Quaternion.Euler (0, Random.Range (0, 359), 0);
		}
		if (Input.GetMouseButtonDown (0) && lastHitObj != null && lastHitObj.tag == "Placement_Plane_Closed") {
			Collider[] c = Physics.OverlapSphere (lastHitObj.transform.position, 0.02f);
			foreach (Collider cs in c) {				
				if (cs.gameObject.tag == "Turret") {					
					Turret_Basic t = cs.transform.parent.GetComponent<Turret_Basic> ();

					t.ScrollMode ();
				}
			}
		}

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 1000, PlacementLayerMask)) {
			if (lastHitObj != null) {
				lastHitObj.GetComponentInChildren<Renderer> ().material = normal;
			}
			lastHitObj = hit.collider.gameObject;
			lastHitObj.GetComponentInChildren<Renderer> ().material = highlight;
		} else {
			if (lastHitObj != null) {
				lastHitObj.GetComponentInChildren<Renderer> ().material = normal;
				lastHitObj = null;
			}
		}
	}
}
