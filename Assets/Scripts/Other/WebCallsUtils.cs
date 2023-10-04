using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WebCallsUtils
{
    public static string MWSIVE_COVER_BASECODE64 { get { return "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"; } }
    public static long SUCCESS_RESPONSE_CODE { get { return 200; } }
    public static long ERROR_RESPONSE_CODE { get { return 400; } }
    public static long AUTHORIZATION_FAILED_RESPONSE_CODE { get { return 401; } }
    public static long ITEM_NOT_AVAILABLE_RESPONSE_CODE { get { return 403; } }
    public static long NOT_FOUND_RESPONSE_CODE { get { return 404; } }
    public static long SERVICE_NOT_AVAILABLE_RESPONSE_CODE { get { return 503; } }
    public static long GATEWAY_TIMEOUT { get { return 504; } }

    public static string AddParametersToURI(string _uri, Dictionary<string, string> _parameters)
    {
        string url = _uri;

        foreach (KeyValuePair<string, string> kvp in _parameters)
        {
            url = url + kvp.Key + "=" + kvp.Value + "&";
        }

        url = url.TrimEnd('&');

        Debug.Log("Complete url is: " + url);

        return url;
    }

    public static string AddMultipleParameterToUri(string _uri, string _key, string[] _parameters)
    {
        string url = _uri + _key + "=";

        foreach (string track_id in _parameters)
        {
            url = url + track_id + "%2C";
        }

        url = url.Remove(url.Length - 3);

        Debug.Log("Complete url with new multiple param is: " + url);

        return url;
    }

    public static string AddSpotifyUrisToUri(string _uri, List<string> _spotifyUris)
    {
        string _modified_uris = "uris=";

        foreach (string uri in _spotifyUris)
        {
            string modifiedSpotifyUri = "";
            string[] separatedUri = uri.Split(':');

            foreach (string part in separatedUri)
            {
                modifiedSpotifyUri = modifiedSpotifyUri + part + "%3A";
            }

            modifiedSpotifyUri = modifiedSpotifyUri.Remove(modifiedSpotifyUri.Length - 3);

            _modified_uris = _modified_uris + modifiedSpotifyUri + "%2C";
        }

        _modified_uris = _modified_uris.Remove(_modified_uris.Length - 3);

        Debug.Log("Complete url with spotify uris param is: " + _uri + _modified_uris);

        return _uri + _modified_uris;
    }

    public static void ReauthenticateUser(SpotifyWebCallback _callback)
    {
        Debug.Log("Bad or expired token. This can happen if the user revoked a token or the access token has expired. Will try yo re-authenticate the user.");
        _callback(new object[] { AUTHORIZATION_FAILED_RESPONSE_CODE });
    }

    public static Texture2D GetTextureCopy(Texture2D _source)
    {
        RenderTexture rt = RenderTexture.GetTemporary(_source.width, _source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(_source, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D readableTexture = new Texture2D(_source.width, _source.height);
        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readableTexture;
    }

    public static bool CheckIfServerServiceIsAvailable(long _responseCode)
    {
        if (_responseCode.Equals(WebCallsUtils.SERVICE_NOT_AVAILABLE_RESPONSE_CODE))
        {
            NewScreenManager.instance.GetCurrentView().EndSearch();

            NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
            PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
            popUpViewModel.Initialize(PopUpViewModelTypes.MessageOnly, "Servicio no disponible", "El servidor de Spotify no puede responder en estos momentos. Volver a intetnar en un rato.", "Aceptar");
            popUpViewModel.SetPopUpAction(() => { NewScreenManager.instance.BackToPreviousView(); });

            return true;
        }

        return false;
    }

    public static bool IsResponseAnyError(long _responseCode)
    {
        if (_responseCode.Equals(WebCallsUtils.ERROR_RESPONSE_CODE)) return true;
        if (_responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE)) return true;
        if (_responseCode.Equals(WebCallsUtils.ITEM_NOT_AVAILABLE_RESPONSE_CODE)) return true;
        if (_responseCode.Equals(WebCallsUtils.NOT_FOUND_RESPONSE_CODE)) return true;
        if (_responseCode.Equals(WebCallsUtils.SERVICE_NOT_AVAILABLE_RESPONSE_CODE)) return true;

        return false;
    }

    public static bool IsResponseItemNotFound(long _responseCode)
    {
        if (_responseCode.Equals(WebCallsUtils.ITEM_NOT_AVAILABLE_RESPONSE_CODE)) return true;
        if (_responseCode.Equals(WebCallsUtils.NOT_FOUND_RESPONSE_CODE)) return true;

        return false;
    }
}
