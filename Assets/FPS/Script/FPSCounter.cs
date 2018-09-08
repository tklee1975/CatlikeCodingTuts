using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour {

	private int mFPS; 

	public virtual int FPS { 
		get {
			return mFPS;
		}
		set {
			mFPS = value;
		}
	}

	void Update () {
		mFPS = (int)(1f / Time.unscaledDeltaTime);
	}
}
