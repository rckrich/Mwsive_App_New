using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SettingsDemoInteractor : Interactor
{
    public override void PerformSearch(params object[] list)
    {
        this._query = (string)list[0];

        string token = (string)list[1];

        parameters = new string[] { token };

        StartCoroutine(CR_PostLogOut(this._query, parameters));
    }

    private IEnumerator CR_PostLogOut(string uri, string[] parameters = null)
    {
        string jsonResult = "";

        string url = this.getCompleteRequestURL(uri);
        WWWForm form = new WWWForm();

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + parameters[0]);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                if (webRequest.responseCode.Equals(401))
                {
                    //Catch response code for multiple requests to the server in a short timespan.
                    tryCatchServerError(webRequest.responseCode);

                    presenter.OnServerError();

                    yield break;
                }
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    if (ErrorRequestManager.AnaliceResponseCode(webRequest.responseCode))
                    {
                        presenter.OnResult();
                        yield break;
                    }
                }

                yield return null;
            }

            jsonResult = webRequest.downloadHandler.text;

            string error = jsonResult.ToString();

            presenter.OnFailedResult(null);

            yield break;

        }
    }
}
