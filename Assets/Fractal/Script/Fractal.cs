﻿using System.Collections;
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
	public Mesh mesh;
	public Material material;
	

	[Header("Fractal Setting")]
	public float generationRate = 0;
	public int maxDepth;
	public float childScale = 0.5f;
	public FractalDir[] directionList;
	//public Vector3[] rotationList = new Vector3[]{Vector3.zero};


	

	// internal 
	//private Vector3 
	private int mDepth;


	// Use this for initialization
	void Start () {
		gameObject.AddComponent<MeshFilter>().mesh = mesh;
		gameObject.AddComponent<MeshRenderer>().material = material;
		GetComponent<MeshRenderer>().material.color =
			Color.Lerp(Color.white, Color.yellow, (float)mDepth / maxDepth);
		

		if(mDepth < maxDepth) {
			if(generationRate > 0) {
				StartCoroutine(CreateFractalChildProgressive());
			} else {
				CreateFractalChildInstantly();
			}
			
		}
	}

	void CreateFractalChildInstantly() {
		foreach(FractalDir dir in directionList) {
			AddFractalChild(dir.position, dir.rotation);
		}
	}

	IEnumerator CreateFractalChildProgressive() {
		foreach(FractalDir dir in directionList) {
			yield return new WaitForSeconds(generationRate);
			AddFractalChild(dir.position, dir.rotation);
		}
	}

	void AddFractalChild(Vector3 direction, Vector3 rotateEuler) {
		GameObject newObject = new GameObject("Fractal Child");
		Fractal fractal = newObject.AddComponent<Fractal>();
		fractal.Initialize(this, direction, rotateEuler);	// Setup the child properties
	}

	private void Initialize(Fractal parent, Vector3 direction, Vector3 rotateEuler) {
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
		transform.localRotation = Quaternion.Euler(rotateEuler.x, rotateEuler.y, rotateEuler.z);
		transform.localScale = Vector3.one * parent.childScale;
		transform.localPosition = direction * (0.5f + 0.5f * childScale);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}