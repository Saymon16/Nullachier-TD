using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public GameObject enemy;
	public RectTransform spawnArea;
	Grid_Manager gm;
	
	// Update is called once per frame
	void Start ()
	{
		gm = GameObject.FindObjectOfType<Grid_Manager> ();
	}

	void SpawnWave (int number)
	{
		for (int i = 0; i < number; i++) {			
			Vector3 pos;
			pos.x = Random.Range (spawnArea.position.x + (spawnArea.rect.center.x - spawnArea.rect.width / 2), spawnArea.position.x + (spawnArea.rect.center.x + spawnArea.rect.width / 2));
			pos.z = Random.Range (spawnArea.position.z + (spawnArea.rect.center.y - spawnArea.rect.height / 2), spawnArea.position.z + (spawnArea.rect.center.y + spawnArea.rect.height / 2));
			pos.y = 0.25f;
			Enemy e = Instantiate (enemy, pos, Quaternion.identity)as Enemy;
			gm.allEnemies.Add (e);
			gm.RefreshBoard ();
		}
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (1)) {
			SpawnWave (5);
		}
	}
}
