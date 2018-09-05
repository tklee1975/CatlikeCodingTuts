using UnityEngine;

// This class is based on the following tutorial
//		https://catlikecoding.com/unity/tutorials/basics/building-a-graph/
// 
//		Key Stuff:
//		- Instantiate a new object
//		- 
public class Graph2D : MonoBehaviour {
	[Header("Setting")]
	public Transform pointPrefab;
	public float maxX = 2;
	public float margin = 0.2f;	// 20 percent is the margin
	public bool showAnimation = true;

	[Range(10, 100)] public int resolution = 10;

	protected Transform[] points;
	protected int mLastResolution;	// last known resolution value

	protected float mTimeElaspe;

	void Awake () {

		mTimeElaspe = 0;
		SetupPoints();
	}

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
		float width = maxX * 2;
		float halfWidth = maxX;
		float step = width / resolution;

		Vector3 position = Vector3.zero;
		Vector3 scale = Vector3.one * step * (1 - margin);
		
		points = new Transform[resolution];
		for (int i = 0; i < points.Length; i++) {

			Transform point = Instantiate(pointPrefab);

			// x = N * step + step / 2 - halfWidth
			position.x = (i + 0.5f) * step - halfWidth;

			// 
			point.localPosition = position;
			point.localScale = scale;

			// Set to parent the point array
			point.SetParent(transform, false);	// don't change the position already defined
			points[i] = point;
		}

		mLastResolution = resolution;
	}

	void Update () {
		if(mLastResolution != resolution) {	// isDirty
			SetupPoints();
		}


		if(showAnimation == false) {
			return;
		}

		mTimeElaspe += Time.deltaTime;

		for (int i = 0; i < points.Length; i++) {
			Transform point = points[i];
			Vector3 position = point.localPosition;
			position.y = Mathf.Sin(Mathf.PI * (position.x + mTimeElaspe));
			point.localPosition = position;
		}
	}
}