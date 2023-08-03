using UnityEngine;
using System.Collections;
using System;

public class Progress_Manager : RCK_Singleton<Progress_Manager>
{
	public static Progress_Manager progressManager;
	public Game_Progress progress;

	void Start () 
	{
		DontDestroyPM ();
		InitGame ();	
	}

	void DontDestroyPM()
	{
		if (progressManager == null) 
		{
			DontDestroyOnLoad (gameObject);
			progressManager = this;
		} 
		else if (progressManager != this) 
		{
			Destroy (gameObject);
		}
	}

	public void save()
	{
		File_Manager.saveGameProgress (this.progress);
	}

	void InitGame()
	{
		Game_Progress progressLoaded = File_Manager.loadGameProgress ();

		if (progressLoaded != null) 
		{
			this.progress = progressLoaded;
		}
		else 
		{
			Debug.Log ("No se ha encontrado los datos del juego, inicializando datos del juego...");

			File_Manager.saveGameProgress (this.progress);

			progress.numero_de_lecturas_para_mostrar_InAppReview = 0;
			progress.veces_que_se_ha_mostrado_InAppReview = 0;
			progress.numero_de_ARscans_para_mostrar_InAppReview = 0;
			progress.volver_a_mostrar_InAppReview = true;
		

			File_Manager.saveGameProgress (this.progress);
		}
	}

}




