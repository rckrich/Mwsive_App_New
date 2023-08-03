using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LogInDemoInteractor : Interactor
{
    public override void PerformSearch(params object[] list)
    {
        _query = (string)list[0];

        string email = (string)list[1];
        string password = (string)list[2];

        parameters = new string[] { email, password };

        StartCoroutine(CR_PostLogIn(this._query, parameters));
    }

    private IEnumerator CR_PostLogIn(string uri, string[] parameters = null)
    {
        string jsonResult = "";

        string url = this.getCompleteRequestURL(uri);

        WWWForm form = new WWWForm();

        form.AddField("email", parameters[0]);
        form.AddField("password", parameters[1]);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.
                tryCatchServerError(webRequest.responseCode);

                //Special Log In Exception Condition
                if (webRequest.responseCode.Equals(401))
                { 
                    presenter.OnFailedResult("Su correo o contraseña son incorrectos. Intente de nuevo.");
                    yield break;
                }

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
                        LogInRoot LogInRoot = JsonConvert.DeserializeObject<LogInRoot>(jsonResult);
                        presenter.OnResult(LogInRoot);
                        yield break;
                    }
                }

                yield return null;
            }

            jsonResult = webRequest.downloadHandler.text;

            string errorMessage = FilterFailedResult(jsonResult.ToString());

            if (errorMessage.Equals(""))
            {
                presenter.OnFailedResult(null);
                yield break;
            }
            else
            {
                presenter.OnFailedResult(errorMessage);
                yield break;
            }
        }
    }

    private string FilterFailedResult(string error)
    {
        if (error.Equals("{\"message\":{\"email\":[\"No es un correo v\\u00e1lido.\"]}}"))
        {
            return "Su correo o contraseña son incorrectos. Intente de nuevo.";
        }

        if (error.Equals("{\"message\":\"api.error.unauthorized\"}"))
        {
            return "Su correo o contraseña son incorrectos. Intente de nuevo.";
        }

        return "";
    }
}
