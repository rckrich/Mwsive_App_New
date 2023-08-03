using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Scene_Manager : RCK_Singleton<Scene_Manager> 
{
	public GameObject loadingCanvas;
	string m_loadingTransition;
	string[] availableScenes = new string[3] {"Login", "Main", "Game"}; 

	void Start ()
	{
		m_loadingTransition = "LoadingTransition";
	}

	/// <summary>
	/// Btn to load a scene
	/// **0 => Login**,  **1 => Main**,  **2 => Game**
	/// </summary>
	/// <param name="value">Value.</param>
	public void Btn_SceneToLoad_IntValue(int value)
	{
		string sceneToLoad = availableScenes [value];

		StartCoroutine (LoadSceneRoutine (sceneToLoad));
	}

    public void Btn_SceneToLoad_StringValue(string value)
    {
        string sceneToLoad = value;

        StartCoroutine(LoadSceneRoutine(sceneToLoad));
    }

    IEnumerator LoadSceneRoutine(string sceneToLoad)
	{
		// Activamos objeto o animacion del loading
		//loadingCanvas.transform.GetChild (0).gameObject.SetActive (true);
		loadingCanvas.GetComponent<Animator> ().SetBool ("Loading", true);

		yield return new WaitForSeconds (1.8f);

        EventManager.instance.resetEventManager();

        // Activamos la transicion a la escena vacia para liberar memoria
        AsyncOperation asyncTransition = SceneManager.LoadSceneAsync (m_loadingTransition, LoadSceneMode.Single); 		

		while (!asyncTransition.isDone) 												
		{
			yield return null;
		}
			
		yield return new WaitForSeconds (1f);	

		// Activamos la transicion a la escena a la que originalmente queriamos ir
		AsyncOperation async = SceneManager.LoadSceneAsync (sceneToLoad, LoadSceneMode.Single); 		

		while (!async.isDone) 												
		{
			yield return null;
		}
			
		yield return new WaitForSeconds (0.5f);	

		//loadingCanvas.transform.GetChild (0).gameObject.SetActive (false);
		loadingCanvas.GetComponent<Animator> ().SetBool ("Loading", false);
	}

	public void CangePhoneOrientationToLandscape()
    {
		StartCoroutine(CR_CangePhoneOrientationToLandscape());
	}
	IEnumerator CR_CangePhoneOrientationToLandscape()
    {
		yield return new WaitForSeconds(2f);
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

	public void CangePhoneOrientationToPortrait()
	{
		StartCoroutine(CR_CangePhoneOrientationToPorttrait());
	}
	IEnumerator CR_CangePhoneOrientationToPorttrait()
	{
		yield return new WaitForSeconds(2f);
		Screen.orientation = ScreenOrientation.Portrait;
	}
}
