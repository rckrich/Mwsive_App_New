using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;

public class NoLogInConnectionManager : MonoBehaviour
{
    private static NoLogInConnectionManager _instance;

    public static NoLogInConnectionManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (NoLogInConnectionManager)GameObject.FindObjectOfType(typeof(NoLogInConnectionManager));
            }
            return _instance;
        }
    }

    public static NoLogInConnectionManager dontDestroyNoLogInConnectionM;

    void Awake()
    {
        if (dontDestroyNoLogInConnectionM == null)
        {
            DontDestroyOnLoad(gameObject);
            dontDestroyNoLogInConnectionM = this;
        }
        else if (dontDestroyNoLogInConnectionM != this)
        {
            Destroy(gameObject);
        }
    }

    private const string CLIENT_ID = "436c4d7d7bba46669876b992f986df8b";
    private const string CLIENT_SECRET = "b066c7da689c4a759c468ccb9d1ecb43";
    private const string CLIENT_CREDENTIALS_URL = "https://accounts.spotify.com/api/token";

    private string noLogInAccesToken = "";
    private DateTime noLogInExpiredDate;

    public string GetNoLogInAccesToken()
    {
        return noLogInAccesToken;
    }

    public void StartConnection()
    {
        StartCoroutine(CR_PostLogin((object[] _value) => {

            if ((long)_value[0] != 200)
            {
                Debug.Log("Error on Spotify's API Client Credentials Spotify Log In");
                return;
            }

            GameObject.FindAnyObjectByType<OAuthHandler>().SetSpotifyTokenRawValue((string)_value[1]);

            ClientCredentialsRoot clientCredentialsRoot = (ClientCredentialsRoot)_value[2];
            noLogInAccesToken = clientCredentialsRoot.access_token;
            noLogInExpiredDate = DateTime.Now.AddMilliseconds(clientCredentialsRoot.expires_in);

            //TEST
            SpotifyConnectionManager.instance.GetPlaylist("5RlUrtDU7WMGnDu8GiRxGI");
            //END TEST
        }));
    }

    private IEnumerator CR_PostLogin(SpotifyWebCallback _callback = null)
    {
        string jsonResult = "";

        WWWForm form = new WWWForm();

        form.AddField("grant_type", "client_credentials");

        using (UnityWebRequest webRequest = UnityWebRequest.Post(CLIENT_CREDENTIALS_URL, form))
        {
            string credentials = String.Format("{0}:{1}", CLIENT_ID, CLIENT_SECRET);
            webRequest.SetRequestHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                Debug.Log("Protocol Error or Connection Error on log in with client credentials with responde code: " + webRequest.responseCode);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Mwisve login " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    ClientCredentialsRoot clientCredentialsRoot = JsonConvert.DeserializeObject<ClientCredentialsRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, jsonResult, clientCredentialsRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on mwsive log in with client credentials " + jsonResult);
            yield break;
        }
    }

    public bool HasNoLogInTokenExpired()
    {
        if (noLogInExpiredDate != null)
        {
            return noLogInExpiredDate.CompareTo(DateTime.Now) < 0;
        }
        else
        {
            return true;
        }
    }

}
