using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class LoadingText : MonoBehaviour 
{
	public TextMeshProUGUI m_loadingText;
	string loadingLocalization;

	void Awake()
	{
		m_loadingText = gameObject.GetComponent<TextMeshProUGUI> ();
//		loadingLocalization = I2.Loc.ScriptLocalization.Get("Loading");
		loadingLocalization = "Cargando";
	}

	void OnEnable () 
	{
		StartCoroutine (LoadingMovement ());
	}
		
	IEnumerator LoadingMovement()
	{
		int iterations = 0;

		while (iterations < 500)
		{
			iterations += 1;
			m_loadingText.text = loadingLocalization + "";
			yield return new WaitForSeconds (0.3f);
			m_loadingText.text = loadingLocalization + ".";
			yield return new WaitForSeconds (0.3f);
			m_loadingText.text = loadingLocalization + "..";
			yield return new WaitForSeconds (0.3f);
			m_loadingText.text = loadingLocalization + "...";
			yield return new WaitForSeconds (0.3f);

			yield return new WaitForSeconds (0.5f);
		}
	}
}

