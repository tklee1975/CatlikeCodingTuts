using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonSpawner : MonoBehaviour {

	public Transform nucleonParent;
	public float timeBetweenSpawns;
	public float spawnDistance;
	public Nucleon[] nucleonPrefabs;
	[Range(2, 10000)] public int capacity;

	[SerializeField] 
	private int mSpawnedCount = 0;

	// Use this for initialization
	void Start () {
		mSpawnedCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnNucleon() {
		Nucleon prefab = nucleonPrefabs[Random.Range(0, nucleonPrefabs.Length)];
		Nucleon newNuclon = Instantiate<Nucleon>(prefab);
		newNuclon.transform.localPosition = Random.onUnitSphere * spawnDistance;

		newNuclon.transform.SetParent(nucleonParent, false);

		mSpawnedCount++;
	}

	float mTimeSinceLastSpawn;

	void FixedUpdate () {
		if(mSpawnedCount >= capacity) {
			return;
		}

		mTimeSinceLastSpawn += Time.deltaTime;
		if (mTimeSinceLastSpawn >= timeBetweenSpawns) {
			mTimeSinceLastSpawn = 0;
			SpawnNucleon();
		}
	}
}
