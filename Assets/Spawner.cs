using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public GameObject enemy;
	public RectTransform spawnArea;
	Grid_Manager gm;

	public Wave[] waves;

	int waveindex = -1;

	public float waitBetweenWaves = 3f;
	float nextWave;

	public float waitBetweenEnnemies = 1f;
	float nextEnemy;

	bool counted = false;
	bool waveSpawnEnd = true;


	
	// Update is called once per frame
	void Start ()
	{
		
		gm = GameObject.FindObjectOfType<Grid_Manager> ();
	}

	void SpawnEnnemy (int number, int _health, float _speed, Color _color, float _size, float order)
	{
		for (int i = 0; i < number; i++) {			
			Vector3 pos;
			pos.x = Random.Range (spawnArea.position.x + (spawnArea.rect.center.x - spawnArea.rect.width / 2), spawnArea.position.x + (spawnArea.rect.center.x + spawnArea.rect.width / 2));
			pos.z = Random.Range (spawnArea.position.z + (spawnArea.rect.center.y - spawnArea.rect.height / 2), spawnArea.position.z + (spawnArea.rect.center.y + spawnArea.rect.height / 2));
			pos.y = 0.25f;
			GameObject g = Instantiate (enemy, pos, Quaternion.identity)as GameObject;
			Enemy e = g.GetComponent<Enemy> ();
			e.waitBeforeGo = Time.time + order + (float)(i * (0.3f + (1.4f * (1 / _speed))));
			e.health = _health;
			e.speed = _speed;
			e.mycolor = _color;
			e.transform.localScale = new Vector3 (_size, _size, _size);

			gm.allEnemies.Add (e);
			gm.RefreshBoard ();
		}
	}

	void SpawnWave (Wave w)
	{		
		for (int i = 0; i < w.waves.Length; i++) {			
			SpawnEnnemy (w.waves [i].number, w.waves [i].health, w.waves [i].speed, w.waves [i].color, w.waves [i].size, (float)(i * 1.2f));
		}
		waveSpawnEnd = true;
	}

	void RaiseDifficulty ()
	{
		foreach (Wave w in waves) {
			for (int i = 0; i < w.waves.Length; i++) {
				w.waves [i].health = Mathf.RoundToInt ((w.waves [i].health * 1.7f) + 1);	
				w.waves [i].number = Mathf.RoundToInt ((w.waves [i].number * 1.2f) + 1);
			}
		}
	}

	void Update ()
	{
		
		if (gm.allEnemies.Count == 0 && !counted && waveSpawnEnd) {			
			counted = true;
			nextWave = Time.time + waitBetweenWaves;
		}
		if (Time.time > nextWave && counted) {
			waveindex++;
			if (waveindex >= waves.Length) {
				waveindex = -1;
				nextWave = Time.time + 3 * waitBetweenWaves;
				RaiseDifficulty ();
				counted = true;
				return;
			}
			waveSpawnEnd = false;
			SpawnWave (waves [waveindex]);	
			counted = false;
		}
	}
}
