using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GarbageCollector : MonoBehaviour 
{
	// Recolecta la basura de datos generada
	void OnEnable()
	{
		Resources.UnloadUnusedAssets ();
		GC.Collect ();
	}
}
