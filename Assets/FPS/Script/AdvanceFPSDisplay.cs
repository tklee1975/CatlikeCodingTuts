using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AdvanceFPSCounter))]
public class AdvanceFPSDisplay : FPSDisplay {
	public Text avgFPSText;
	public Text maxFPSText;
	public Text minFPSText;

	private AdvanceFPSCounter mFPSCounter;


	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		mFPSCounter = GetComponent<AdvanceFPSCounter>();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		avgFPSText.text = GetFastString(mFPSCounter.avgFPS);
		maxFPSText.text = GetFastString(mFPSCounter.maxFPS);
		minFPSText.text = GetFastString(mFPSCounter.minFPS);
	}

	

}
