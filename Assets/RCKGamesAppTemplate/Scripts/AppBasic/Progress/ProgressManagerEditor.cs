using UnityEngine;
using UnityEditor;
using System.Collections;

#if UNITY_EDITOR
[CustomEditor(typeof(Progress_Manager))]
public class ProgressManagerEditor : Editor 
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		Progress_Manager manager = (Progress_Manager)target;

		if (GUILayout.Button ("Save Preset")) 
		{
			manager.save ();
		}
	}

}
#endif

