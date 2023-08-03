using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class File_Manager 
{
	private static string filePath = "/GameData.dat";

	public static void saveGameProgress(Game_Progress data)
	{
		#if UNITY_IPHONE
		System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		#endif

		BinaryFormatter formatter = new BinaryFormatter ();

		FileStream file = File.Create (Application.persistentDataPath + filePath);
		formatter.Serialize (file, data);
		file.Close ();
		Debug.Log ("Saved");
	}

	public static Game_Progress loadGameProgress()
	{
		Game_Progress data = null;
		if (File.Exists (Application.persistentDataPath + filePath)) 
		{
			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + filePath, FileMode.Open);
			data = (Game_Progress)formatter.Deserialize (file);
			file.Close ();
			Debug.Log ("Loaded");
		} 
		else 
		{
			Debug.Log ("No se encuentra el archivo GameData.dat, se creara uno nuevo");
		}

		return data;
	}

}