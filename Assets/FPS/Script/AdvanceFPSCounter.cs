using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceFPSCounter : FPSCounter {
	public int frameRange = 60;

	public int avgFPS { get ; private set; }	// Average FPS
	public int maxFPS { get ; private set; }	// Maximum FPS
	public int minFPS { get ; private set; }	// Minimum FPS
	

	private int[] mFPSBuffer;
	private int mFPSBufferIndex;

	public override int FPS { 
		get {
			return avgFPS;
		}
	}

	void InitializeBuffer () {
		if (frameRange <= 0) {
			frameRange = 1;
		}
		mFPSBuffer = new int[frameRange];
		mFPSBufferIndex = 0;
	}

	private int mFPS; 


	

	

	void Update () {
		// Lazy Creation of FPS buffer
		if (mFPSBuffer == null || mFPSBuffer.Length != frameRange) {
			InitializeBuffer();
		}


		UpdateFPSBuffer();
		CalculateFPS();

		// mFPS = (int)(1f / Time.unscaledDeltaTime);
	}

	void UpdateFPSBuffer() {
		mFPSBuffer[mFPSBufferIndex] = (int) ( 1.0f / Time.unscaledDeltaTime) ;
		mFPSBufferIndex++;
		if(mFPSBufferIndex == frameRange) {
			mFPSBufferIndex = 0;
		}
	}

	void CalculateFPS() {
		int total = 0;
		int min = int.MaxValue;
		int max = 0;
		foreach(int fps in mFPSBuffer) {
			total += fps;
			if(fps < min) { min = fps; }
			if(fps > max) { max = fps; }
		}

		avgFPS = total/frameRange;
		minFPS = min;
		maxFPS = max;

		Debug.Log("avgFPS=" + avgFPS + " minFPS=" + minFPS + " maxFPS=" + maxFPS);
	}
}
