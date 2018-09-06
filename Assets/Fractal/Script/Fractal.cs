using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {
	[System.Serializable]
	public struct FractalDir
	{
		public Vector3 position;
		public Vector3 rotation;
	}

	[Header("Rendering Setting")]
	public Mesh[] meshes;
	//public Mesh mesh;
	public Material material;
	public float spawnProbability = 0.8f;
	

	[Header("Rotation Setting")]
	public float maxRotationSpeed;	
	private float mRotationSpeed;

	[Header("Fractal Setting")]
	public float generationRate = 0;
	public int maxDepth;
	public float childScale = 0.5f;
	public FractalDir[] directionList;
	//public Vector3[] rotationList = new Vector3[]{Vector3.zero};


	

	// internal 
	//private Vector3 
	private int mDepth;
	private Material[,] mMaterials;


	// Use this for initialization
	void Start () {
		if (mMaterials == null) {
			InitializeMaterials();
		}

		mRotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);

		gameObject.AddComponent<MeshFilter>().mesh = GetRandomMesh();
		gameObject.AddComponent<MeshRenderer>().material = GetRandomMaterial(mDepth);	// mMaterials[mDepth,2];
		

		if(mDepth < maxDepth) {
			if(generationRate > 0) {
				StartCoroutine(CreateFractalChildProgressive());
			} else {
				CreateFractalChildInstantly();
			}
			
		}
	}

	Mesh GetRandomMesh() {
		int randIndex = Random.Range(0, meshes.Length);
		return meshes[randIndex];
	}

	Material GetRandomMaterial(int depth) {
		int randIndex = Random.Range(0, 2);
		return mMaterials[depth, randIndex];
	}

	void InitializeMaterials() {
		mMaterials = new Material[maxDepth + 1,2];

		for(int i=0; i<= maxDepth; i++) {
			

			float t = i / (maxDepth - 1f);
			t *= t;

			mMaterials[i, 0] = new Material(material);
			mMaterials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
			mMaterials[i, 1] = new Material(material);
			mMaterials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
		}
		mMaterials[maxDepth, 0].color = Color.red;	// the tip is red 
		mMaterials[maxDepth, 1].color = Color.magenta;
	}

	void CreateFractalChildInstantly() {
		foreach(FractalDir dir in directionList) {
			if(CanSpawn()) {
				AddFractalChild(dir.position, dir.rotation);
			}
			
		}
	}

	IEnumerator CreateFractalChildProgressive() {
		foreach(FractalDir dir in directionList) {
			if(CanSpawn()) {
				yield return new WaitForSeconds(generationRate);
				AddFractalChild(dir.position, dir.rotation);
			}
		}
	}

	void AddFractalChild(Vector3 direction, Vector3 rotateEuler) {
		GameObject newObject = new GameObject("Fractal Child");
		Fractal fractal = newObject.AddComponent<Fractal>();
		fractal.Initialize(this, direction, rotateEuler);	// Setup the child properties
	}

	bool CanSpawn() {
		return Random.value < spawnProbability;
	}

	private void Initialize(Fractal parent, Vector3 direction, Vector3 rotateEuler) {
		meshes = parent.meshes;
		material = parent.material;
		mMaterials = parent.mMaterials;
		maxRotationSpeed = parent.maxRotationSpeed;
		maxDepth = parent.maxDepth;
		mDepth = parent.mDepth + 1;
		spawnProbability = parent.spawnProbability;
		generationRate = parent.generationRate;
		childScale = parent.childScale;
		directionList = parent.directionList;
		
		// 
		transform.SetParent(parent.transform, false);

		// Define the position 
		transform.localRotation = Quaternion.Euler(rotateEuler.x, rotateEuler.y, rotateEuler.z);
		transform.localScale = Vector3.one * parent.childScale;
		transform.localPosition = direction * (0.5f + 0.5f * childScale);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0f, mRotationSpeed * Time.deltaTime, 0f);
	}
}
