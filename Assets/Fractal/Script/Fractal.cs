using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {
	[Header("Rendering Setting")]
	public Mesh mesh;
	public Material material;
	

	[Header("Fractal Setting")]
	public float generationRate = 0;
	public int maxDepth;
	public float childScale = 0.5f;
	public Vector3[] directionList = new Vector3[]{Vector3.up};


	// internal 
	//private Vector3 
	private int mDepth;


	// Use this for initialization
	void Start () {
		gameObject.AddComponent<MeshFilter>().mesh = mesh;
		gameObject.AddComponent<MeshRenderer>().material = material;

		if(mDepth < maxDepth) {
			if(generationRate > 0) {
				StartCoroutine(CreateFractalChildProgressive());
			} else {
				CreateFractalChildInstantly();
			}
			
		}
	}

	void CreateFractalChildInstantly() {
		foreach(Vector3 dir in directionList) {
			AddFractalChild(dir);
		}
	}

	IEnumerator CreateFractalChildProgressive() {
		foreach(Vector3 dir in directionList) {
			yield return new WaitForSeconds(generationRate);
			AddFractalChild(dir);
		}
	}

	void AddFractalChild(Vector3 direction) {
		GameObject newObject = new GameObject("Fractal Child");
		Fractal fractal = newObject.AddComponent<Fractal>();
		fractal.Initialize(this, direction);	// Setup the child properties
	}

	private void Initialize(Fractal parent, Vector3 direction) {
		mesh = parent.mesh;
		material = parent.material;
		maxDepth = parent.maxDepth;
		mDepth = parent.mDepth + 1;
		generationRate = parent.generationRate;
		childScale = parent.childScale;
		directionList = parent.directionList;
		
		// 
		transform.SetParent(parent.transform, false);

		// Define the position 
		transform.localScale = Vector3.one * parent.childScale;
		transform.localPosition = direction * (0.5f + 0.5f * childScale);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
