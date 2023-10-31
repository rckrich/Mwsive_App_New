using UnityEngine;
using UnityEditor;
using System.Collections;

#if UNITY_EDITOR
[CustomEditor(typeof(ProgressManager))]
public class ProgressManagerEditor : Editor 
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		ProgressManager manager = (ProgressManager)target;

		if (GUILayout.Button ("Save Preset")) 
		{
			manager.save ();
		}
	}

}
#endif

