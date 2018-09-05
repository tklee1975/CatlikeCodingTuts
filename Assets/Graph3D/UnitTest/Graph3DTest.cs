using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleTDD;

public class Graph3DTest : BaseTest {		
	public Dropdown functionDropdown;
	public Graph3D graph3D;


	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		SetupDropdown();
	}	


	void SetupDropdown() {
		GraphFunctionName selected = graph3D.function;

		List<Dropdown.OptionData> optList = new List<Dropdown.OptionData>(); 
		
		int end = (int) GraphFunctionName.Torus;
		int selectedIndex = 0;
		for(int i=0; i<=end; i++) {
			GraphFunctionName thisGraph = (GraphFunctionName) i;

			Dropdown.OptionData data = new Dropdown.OptionData();
			data.text = thisGraph.ToString();
			optList.Add(data);

			if(thisGraph == selected) {
				selectedIndex = i;
			}
		}

		functionDropdown.options = optList;
		functionDropdown.value = selectedIndex;
	}

	public void ChangeGraphFunction() {

		graph3D.function = (GraphFunctionName) functionDropdown.value;
	}


	[Test]
	public void test1()
	{
		Debug.Log("###### TEST 1 ######");
	}

	[Test]
	public void test2()
	{
		Debug.Log("###### TEST 2 ######");
	}
}
