using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDestroyOnLoad : MonoBehaviour 
{
	public static NoDestroyOnLoad noDestroy;

	void Start () 
	{
		DontDestroy ();
	}

	void DontDestroy()
	{
		if (noDestroy == null) 
		{
			DontDestroyOnLoad (gameObject);
			noDestroy = this;
		} 
		else if (noDestroy != this) 
		{
			Destroy (gameObject);
		}
	}

}
