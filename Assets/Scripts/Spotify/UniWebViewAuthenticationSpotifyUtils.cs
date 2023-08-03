using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UniWebViewAuthenticationSpotifyUtils
{
    internal static Dictionary<string, string> ParseFormUrlEncodedString(string input)
    {
        var result = new Dictionary<string, string>();
        if (input.StartsWith("?") || input.StartsWith("#"))
        {
            input = input.Substring(1);
        }

        var pairs = input.Split('&');
        foreach (var pair in pairs)
        {
            var kv = pair.Split('=');
            result.Add(UnityWebRequest.UnEscapeURL(kv[0]), UnityWebRequest.UnEscapeURL(kv[1]));
        }

        return result;
    }
}
