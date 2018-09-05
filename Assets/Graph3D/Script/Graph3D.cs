using UnityEngine;

// this script is based on this tutorial: 
//		https://catlikecoding.com/unity/tutorials/basics/mathematical-surfaces/
// 

public class Graph3D : MonoBehaviour {

	[Header("Setting")]
	public Transform pointPrefab;
	public float graphicWidth = 2;		// the width for X and Z
	public float margin = 0.2f;	// 20 percent is the margin
	public bool showAnimation = true;

	[Range(10, 100)] public int resolution = 10;

	public static float pointSpacing = 1.0f;
	public GraphFunctionName function = GraphFunctionName.Test3D;		// enum

	Transform[] points;

	protected float mPointStep = 0;
	protected float mHalfWidth;

	protected int mLastResolution;	// last known resolution value

	protected float mTimeElaspe;

	void Awake () {
		
		SetupPoints();
		UpdatePoints(0);		// t = 0
	}



	// void Update () {
	// 	float t = Time.time;
	// 	GraphFunction f = functions[(int)function];
	// 	float step = 2f / resolution;
	// 	for (int i = 0, z = 0; z < resolution; z++) {
	// 		float v = (z + 0.5f) * step - 1f;
	// 		for (int x = 0; x < resolution; x++, i++) {
	// 			float u = (x + 0.5f) * step - 1f;
	// 			points[i].localPosition = f(u, v, t);
	// 		}
	// 	}
	// }

	#region Graphics Points Logic 
	void ClearPoints() {
		if(points == null) {
			return;
		}
		foreach(Transform point in points) {
			GameObject.Destroy(point.gameObject);
		}
		points = new Transform[0];	// empty
	}

	void SetupPoints() {
		ClearPoints();
		
		mHalfWidth = graphicWidth / 2;		
		mPointStep = graphicWidth / resolution;

		pointSpacing = mPointStep;

		Debug.Log("SetupPoints: pointStep=" + mPointStep);


		Vector3 position = Vector3.zero;
		Vector3 scale = Vector3.one * mPointStep * (1 - margin);
		
		// create the point array
		points = new Transform[resolution * resolution];

		// instantiate the points and all at the zero
		for (int i = 0; i < points.Length; i++) {

			Transform point = Instantiate(pointPrefab);

			// 
			point.localPosition = position;
			point.localScale = scale;

			// Set to parent the point array
			point.SetParent(transform, false);	// don't change the position already defined
			points[i] = point;
		}

		mLastResolution = resolution;
	}

	public void UpdatePoints(float t) {	// t is the timeElapse
		GraphFunction f = functions[(int)function];	


		// //float step = 2f / resolution;
		int pointCount = points.Length; 
		int x = 0;	// => u  : actual coordinate at x
		int z = 0;	// => v  : actual coordinate at z

		float startU = -mHalfWidth + 0.5f * mPointStep;

		// First u, v 
		float v = -mHalfWidth + 0.5f * mPointStep;
		float u = startU;
		for(int i=0; i< pointCount; i++) {
			// 
			points[i].localPosition = f(u, v, t);
			
			// 
			
			// Setting u, v
			x++;
			u += mPointStep;
			if(x >= resolution) {
				x = 0;
				u = startU;
				v += mPointStep;
			}
		}


		// for (int i = 0, z = 0; z < resolution; z++) {
		// 	float v = (z + 0.5f) * step - 1f;
		// 	for (int x = 0; x < resolution; x++, i++) {
		// 		float u = (x + 0.5f) * step - 1f;
		// 		points[i].localPosition = f(u, v, t);
		// 	}
		// }
	}

	#endregion

	#region Run time Logic 
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(mLastResolution != resolution) {	// isDirty
			SetupPoints();
			UpdatePoints(mTimeElaspe);
			return;
		}

		if(showAnimation == false) {
			return;
		}

		mTimeElaspe += Time.deltaTime;
		UpdatePoints(mTimeElaspe);

	}

	#endregion

	#region Graphic Functions

	static GraphFunction[] functions = {
		PlaneY, SineFunction, Sine2DFunction, MultiSineFunction, MultiSine2DFunction,
		StaticRipple, Ripple, Pyramid, Test3D, Matrix4, Circle, 
		Cube, StaticCylinder, Cylinder, Sphere, Torus, TempTest
	};

	const float pi = Mathf.PI;

	static Vector3 PlaneY (float x, float z, float t) {
		Vector3 p;
		p.x = x;
		p.y = 0;
		p.z = z;
		return p;
	}

	static Vector3 SineFunction (float x, float z, float t) {
		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + t));
		p.z = z;
		return p;
	}

	static Vector3 MultiSineFunction (float x, float z, float t) {
		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + t));
		p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
		p.y *= 2f / 3f;
		p.z = z;
		return p;
	}

	static Vector3 Sine2DFunction (float x, float z, float t) {
		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + t));
		p.y += Mathf.Sin(pi * (z + t));
		p.y *= 0.5f;
		p.z = z;
		return p;
	}

	static Vector3 MultiSine2DFunction (float x, float z, float t) {
		Vector3 p;
		p.x = x;
		p.y = 4f * Mathf.Sin(pi * (x + z + t / 2f));
		p.y += Mathf.Sin(pi * (x + t));
		p.y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
		p.y *= 1f / 5.5f;
		p.z = z;
		return p;
	}

	static Vector3 StaticRipple (float x, float z, float t) {
		Vector3 p;
		float d = Mathf.Sqrt(x * x + z * z);
		p.x = x;
		p.y = d;
		p.z = z;
		return p;
	}

	static Vector3 Ripple (float x, float z, float t) {
		Vector3 p;
		float d = Mathf.Sqrt(x * x + z * z);
		p.x = x;
		p.y = Mathf.Sin(pi * (d + t));
		//p.y /= 1f + 10f * d;
		p.z = z;
		return p;
	}

	static Vector3 Pyramid (float x, float z, float t) {
		Vector3 p;
		float h = Mathf.Max(Mathf.Abs(x), Mathf.Abs(z));
		p.x = x;
		p.z = z;
		p.y = 1 - h;
		//p.y /= 1f + 10f * d;
		
		return p;
	}

	static Vector3 RippleX (float x, float z, float t) {
		Vector3 p;
		float d = Mathf.Sqrt(x * x + z * z);
		p.x = x;
		p.y = Mathf.Sin(pi * (4f * d - t));
		p.y /= 1f + 10f * d;		// 1
		p.z = z;
		return p;
	}

	static Vector3 Test3D (float u, float v, float t) {
		Vector3 p;
		// float d = Mathf.Sqrt(x * x + z * z);
		p.x = u + v;
		p.y = u * v;
		p.z = v == 0 ? 0 : u / v;
		return p;
	}

	static Vector3 TempTest (float u, float v, float t) {
		Vector3 p;
		// float d = Mathf.Sqrt(x * x + z * z);
		//p.x = u;	p.y = v*v + u*u;	p.z = v;
		// p.x = Mathf.Sin(u);
		// p.z = Mathf.Cos(u);
		// p.y = 0;
		// p.y = u; p.x = Random.Range(-1.0f, 1.0f); p.z = Random.Range(-1.0f, 1.0f);
		//p = 
		// float ratio =  v / 2.0f;	// 2 = halfWidth


		// p.x = Mathf.Cos(ratio * pi);
		// p.y = 0;
		// p.z = Mathf.Sin(ratio * pi);

		Vector3 v3 = new Vector3(u, v, 0);
		Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 30, 50), Vector3.one);
		
		
		return mat.MultiplyPoint3x4(v3);
	}

	static Vector3 Matrix4(float u, float v, float t) {
		Vector3 p;
		// float d = Mathf.Sqrt(x * x + z * z);
		//p.x = u;	p.y = v*v + u*u;	p.z = v;
		// p.x = Mathf.Sin(u);
		// p.z = Mathf.Cos(u);
		// p.y = 0;
		// p.y = u; p.x = Random.Range(-1.0f, 1.0f); p.z = Random.Range(-1.0f, 1.0f);
		//p = 
		// float ratio =  v / 2.0f;	// 2 = halfWidth


		// p.x = Mathf.Cos(ratio * pi);
		// p.y = 0;
		// p.z = Mathf.Sin(ratio * pi);

		Vector3 v3 = new Vector3(u, v, 0);
		Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 30, 50), Vector3.one);
		
		
		return mat.MultiplyPoint3x4(v3);
	}

	static Vector3 Circle (float u, float v, float t) {
		Vector3 p;
		float ratio =  v / 2.0f;	// 2 = halfWidth : ratio : -1 ~ 1


		p.x = Mathf.Cos(ratio * pi);
		p.y = 0;
		p.z = Mathf.Sin(ratio * pi);
		
		return p;		
	}

	static Vector3 Cube (float u, float v, float t) {
		Vector3 p;
		
		int index = Mathf.FloorToInt( (2 + v + pointSpacing * 0.5f) / pointSpacing);
		//Debug.Log("v=" + v + " index=" + index);
		//index = 5 + index;
		//
		int size = 10;	// ken: only work for resolution=100

		int gx = index / size;
		int gy = index % size;

		p.y = u; p.z = gx * 0.1f; p.x = gy * 0.1f;
		return p;		
	}

	static Vector3 StaticCylinder(float u, float v, float t) {
		Vector3 p;
		//float ratio =  v / 2.0f;	// 2 = halfWidth


		p.x = Mathf.Cos(v * pi);
		p.y = u;
		p.z = Mathf.Sin(v * pi);
		
		return p;
	}

	static Vector3 Cylinder (float u, float v, float t) {
		Vector3 p;
		float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t)) * 0.2f;
		p.x = r * Mathf.Sin(pi * u);
		p.y = v;
		p.z = r * Mathf.Cos(pi * u);
		return p;
	}

	static Vector3 Sphere (float u, float v, float t) {
		Vector3 p;
		float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
		r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
		float s = r * Mathf.Cos(pi * 0.5f * v);
		p.x = s * Mathf.Sin(pi * u);
		p.y = r * Mathf.Sin(pi * 0.5f * v);
		p.z = s * Mathf.Cos(pi * u);
		return p;
	}

	static Vector3 Torus (float u, float v, float t) {
		Vector3 p;
		float r1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
		float r2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.05f;
		float s = r2 * Mathf.Cos(pi * v) + r1;
		p.x = s * Mathf.Sin(pi * u);
		p.y = r2 * Mathf.Sin(pi * v);
		p.z = s * Mathf.Cos(pi * u);
		return p;
	}

	#endregion
}