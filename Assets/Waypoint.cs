using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour
{
	public Waypoint nextWaypoint;
	public bool isLast = false;
	public bool isFirst = false;


	void OnTriggerEnter (Collider col)
	{
		if (col.tag == "Enemy") {
			
			Enemy e = col.GetComponent<Enemy> ();
			if (!isLast) {
				e.target = nextWaypoint;

			} else {
				
				e.HasReachedEnd ();
			}
		}
	}
}