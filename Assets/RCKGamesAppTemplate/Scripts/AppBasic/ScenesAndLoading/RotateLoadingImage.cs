using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLoadingImage : MonoBehaviour 
{
	// Hace que una imagen este dando vueltas constantemente, se usa para el simbolo circular de loading
	void Update () 
	{
		if (gameObject.activeInHierarchy) 
		{
			gameObject.transform.Rotate (0f, 0f, 60f * Time.deltaTime);
		}
	}
		
	void OnDisable()
	{
		gameObject.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
	}

}
