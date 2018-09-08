using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleTDD;

public class FPSTest : BaseTest {		
	public NucleonSpawner spawner;

	[Test]
	public void SpawnNucleon()
	{
		spawner.SpawnNucleon();
	}

	[Test]
	public void test2()
	{
		Debug.Log("###### TEST 2 ######");
	}
}
