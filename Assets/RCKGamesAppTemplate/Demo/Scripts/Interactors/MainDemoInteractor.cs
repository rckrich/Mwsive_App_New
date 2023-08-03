using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainDemoInteractor : Interactor
{
    public override void PerformSearch(params object[] list)
    {
        this._query = (string)list[0];

        StartCoroutine(CR_GetProducts(this._query));
    }

    private IEnumerator CR_GetProducts(string uri, string[] parameters = null)
    {
        string jsonResult = "";

        string url = uri;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.
                tryCatchServerError(webRequest.responseCode);

                presenter.OnServerError();

                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    if (ErrorRequestManager.AnaliceResponseCode(webRequest.responseCode))
                    {
                        jsonResult = webRequest.downloadHandler.text;
                        InstanceableExampleRoot instanceableExampleRoot = JsonConvert.DeserializeObject<InstanceableExampleRoot>(jsonResult);
                        presenter.OnResult(instanceableExampleRoot);
                        yield break;
                    }
                }
            }

            jsonResult = webRequest.downloadHandler.text;

            string error = jsonResult.ToString();

            presenter.OnFailedResult(null);

            yield break;

        }
    }
}
