using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WebCallsUtils
{
    public static long SUCCESS_RESPONSE_CODE { get { return 200; } }
    public static long AUTHORIZATION_FAILED_RESPONSE_CODE { get { return 401; } }

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
}
