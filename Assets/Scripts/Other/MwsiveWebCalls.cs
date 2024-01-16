using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using UnityEngine.XR;

public delegate void MwsiveWebCallback(object[] _value);

public class MwsiveWebCalls : MonoBehaviour
{
    public static IEnumerator CR_PostCreateUser(string _email, string _gender, int _age, ProfileRoot _profile, string[] _playlist_ids, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/register";
        //string url = "http://192.241.129.184/api/register";

        CreateMwsiveUserRoot createMwsiveUserRoot = new CreateMwsiveUserRoot
        {
            user = _profile,
            email = _email,
            gender = _gender,
            age = _age
        };

        createMwsiveUserRoot.playlist_ids = new string[_playlist_ids.Length];

        for (int i = 0; i < _playlist_ids.Length; i++)
        {
            createMwsiveUserRoot.playlist_ids[i] = _playlist_ids[i];
        }

        string jsonRaw = JsonConvert.SerializeObject(createMwsiveUserRoot);

        DebugLogManager.instance.DebugLog("Body request for creating a playlist is:" + jsonRaw);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on create profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Create Mwsive user " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveCreatedRoot mwsiveCreatedRoot = JsonConvert.DeserializeObject<MwsiveCreatedRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveCreatedRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on create mwsive user " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostLogin(string _email, string _user_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/login";
        //string url = "http://192.241.129.184/api/login";

        LogInMwsiveRoot logInMwsiveRoot = new LogInMwsiveRoot
        {
            email = _email,
            user_id = _user_id
        };

        string jsonRaw = JsonConvert.SerializeObject(logInMwsiveRoot);

        DebugLogManager.instance.DebugLog("Body request for login is: " + jsonRaw);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError && webRequest.responseCode == WebCallsUtils.NOT_FOUND_RESPONSE_CODE)
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Response code 404. No Mwsive user found, creating new Mwsive User");
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Mwisve login " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveLoginRoot mwsiveLoginRoot = JsonConvert.DeserializeObject<MwsiveLoginRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveLoginRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on mwsive log in " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostLogout(string _token, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/logout";
        //string url = "http://192.241.129.184/api/logout";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.
                DebugLogManager.instance.DebugLog(webRequest.responseCode);

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Mwsive logout " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on mwsive log out " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetCurrentMwsiveUser(string _token, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me";
        //string url = "http://192.241.129.184/api/me";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError && (webRequest.responseCode == WebCallsUtils.NOT_FOUND_RESPONSE_CODE || webRequest.responseCode == WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Response code 404. No Mwsive user found. Not found in DataBase.");
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch Mwsive User result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveUserRoot mwsiveUserRoot = JsonConvert.DeserializeObject<MwsiveUserRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveUserRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on fetch mwsive user: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetMwsiveUser(string _user_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/users/" + _user_id;
        //string url = "http://192.241.129.184/api/users/" + _user_id;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError && webRequest.responseCode == WebCallsUtils.NOT_FOUND_RESPONSE_CODE)
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Response code 404. No Mwsive user found. Not found in DataBase.");
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch Mwsive User result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveUserRoot mwsiveUserRoot = JsonConvert.DeserializeObject<MwsiveUserRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveUserRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on fetch mwsive user: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_PostDeleteUser(string _token, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/delete";
        //string url = "http://192.241.129.184/api/me/delete";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Delete Mwsive user " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on delete mwsive user " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PutLastSavedPlaylist(string _token, string _playlist_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/last-selected-playlist/" + _playlist_id;
        //string url = "http://192.241.129.184/api/me/last-selected-playlist/" + _playlist_id;

        using (UnityWebRequest webRequest = UnityWebRequest.Put(url, ""))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }
                DebugLogManager.instance.DebugLog(webRequest.responseCode);
                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Put Last Saved Playlist " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, _playlist_id });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on put Last Saved Playlist " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostTrackAction(string _token, string _track_id, string _action, float _duration, string _playlist_id, int _challenge_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/tracks/action";
        //string url = "http://192.241.129.184/api/tracks/action";

        TrackActionRoot newAction = new TrackActionRoot
        {
            track_id = _track_id,
            action = _action,
            duration = _duration,
            playlist_id = _playlist_id,
        };

        newAction.challenge_id = _challenge_id >= 0 ? _challenge_id : null;

        string jsonRaw = JsonConvert.SerializeObject(newAction);

        DebugLogManager.instance.DebugLog("Body request for creating a playlist is:" + jsonRaw);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Post Track action " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    RootTrackAction rootTrackAction = JsonConvert.DeserializeObject<RootTrackAction>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, rootTrackAction });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on post track action " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetCuratorsThatVoted(string _track_id, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/tracks/" + _track_id + "/curators/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/tracks/" + _track_id + "/curators/" + _offset.ToString() + "/" + _limit.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch Mwsive Curators result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveCuratorsRoot mwsiveCuratorsRoot = JsonConvert.DeserializeObject<MwsiveCuratorsRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveCuratorsRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch Mwsive Curators result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetFollowingThatVoted(string _token, string _track_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/track/following";
        //string url = "http://192.241.129.184/api/track/following";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("track_id", _track_id);

        url = WebCallsUtils.AddParametersToURI(url + "?", parameters);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch following that vote result: " + jsonResult);
                    int following_that_voted = JsonConvert.DeserializeObject<int>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, following_that_voted });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch following that vote result result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetFollowers(string _token, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/followers/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/me/followers/" + _offset.ToString() + "/" + _limit.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch Mwsive followers result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveFollowersRoot mwsiveFollowersRoot = JsonConvert.DeserializeObject<MwsiveFollowersRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveFollowersRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch Mwsive followers result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetFollowed(string _token, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/followed/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/me/followed/" + _offset.ToString() + "/" + _limit.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch Mwsive followers result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveFollowedRoot mwsiveFollowedRoot = JsonConvert.DeserializeObject<MwsiveFollowedRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveFollowedRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch Mwsive followers result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetUserFollowers(string _token, string _user_id, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/users/" +_user_id + "/followers/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/users/" + _user_id + "/followers/" + _offset.ToString() + "/" + _limit.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch Mwsive followers result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveFollowersRoot mwsiveFollowersRoot = JsonConvert.DeserializeObject<MwsiveFollowersRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveFollowersRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch Mwsive followers result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetUserFollowed(string _token, string _user_id, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/users/" +_user_id + "/followed/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/users/" + _user_id + "/followed/" + _offset.ToString() + "/" + _limit.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch Mwsive followers result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveFollowedRoot mwsiveFollowedRoot = JsonConvert.DeserializeObject<MwsiveFollowedRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveFollowedRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch Mwsive followers result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_PostFollow(string _token, string _user_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/follow";
        //string url = "http://192.241.129.184/api/me/follow";

        PostFollowRoot postFollowRoot = new PostFollowRoot
        {
            user_id = _user_id
        };

        string jsonRaw = JsonConvert.SerializeObject(postFollowRoot);

        DebugLogManager.instance.DebugLog("Body request for creating a playlist is:" + jsonRaw);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Post follow " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on post follow " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetIsFollowing(string _token, MwsiveWebCallback _callback, string _user_id)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/follow/" + _user_id;
        //string url = "http://192.241.129.184/api/me/follow/" + _user_id;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch is following. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch is following result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    IsFollowingRoot isFollowingRoot = JsonConvert.DeserializeObject<IsFollowingRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, isFollowingRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch is following result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetCuratorsByName(string _token, string _name, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/users/search/" + _name;
        //string url = "http://192.241.129.184/api/users/search/" + _name;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {

                    //TODO Response when unauthorized
                }
                DebugLogManager.instance.DebugLog(webRequest.responseCode);
                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch curators by name. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch Curators by name result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveCuratorsRoot mwsiveCuratorsRoot = JsonConvert.DeserializeObject<MwsiveCuratorsRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveCuratorsRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch curators by name result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetBadges(string _token, string userid, string type, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/users/" + userid + "/badges/" + type + "/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/users/" + userid + "/badges/" + type + "/" + _offset.ToString() + "/" + _limit.ToString();

        DebugLogManager.instance.DebugLog(url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch Badges result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveBadgesRoot mwsiveBadgesRoot = JsonConvert.DeserializeObject<MwsiveBadgesRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveBadgesRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch badges result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_PostBadgeComplete(string _token, string _badge_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        WWWForm form = new WWWForm();

        form.AddField("badge_id", _badge_id);

        string url = "https://wavemanager.mwsive.com/api/me/badges/complete";
        //string url = "http://192.241.129.184/api/me/badges/complete";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Post badge complete user " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on post badge complete " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostProfilePicture(string _token, Texture2D _texture, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        byte[] textureBytes = null;
        Texture2D imageTexture = WebCallsUtils.GetTextureCopy(_texture);
        textureBytes = imageTexture.EncodeToPNG();

        string base64 = "data:image/png;base64," + Convert.ToBase64String(textureBytes);
        DebugLogManager.instance.DebugLog(base64);

        PostUserPhoto postUserPhoto = new PostUserPhoto { image = base64 };

        string jsonRaw = JsonConvert.SerializeObject(postUserPhoto);

        string url = "https://wavemanager.mwsive.com/api/me/image";
        //string url = "http://192.241.129.184/api/me/image";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Post profile picture " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on post profile picture complete " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostDisplayName(string _token, string _display_name, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/display-name";
        //string url = "http://192.241.129.184/api/me/display-name";

        PostDisplayNameRoot postDisplayNameRoot = new PostDisplayNameRoot { display_name = _display_name };

        string jsonRaw = JsonConvert.SerializeObject(postDisplayNameRoot);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Post display name " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on post display name complete " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostUserLink(string _token, string _type, string _url, MwsiveWebCallback _callback )
    {
        //string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/shared-url";
        //string url = "http://192.241.129.184/api/me/shared-url";

        PostUserLinkRoot postUserLinkRoot = new PostUserLinkRoot { type = _type, url = _url };

        string jsonRaw = JsonConvert.SerializeObject(postUserLinkRoot);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on post user link. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Post user link complete");
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on post user link complete");
            yield break;
        }
    }

    public static IEnumerator CR_PostMusicalDNA(string _token, string _type, string[] _items, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/adn";
        //string url = "http://192.241.129.184/api/me/adn";

        MusicalDNA musicalDNA = new MusicalDNA
        {
            type = _type
        };

        musicalDNA.items = new string[_items.Length];

        for (int i = 0; i < _items.Length; i++)
        {
            musicalDNA.items[i] = _items[i];
        }

        string jsonRaw = JsonConvert.SerializeObject(musicalDNA);

        DebugLogManager.instance.DebugLog("Body request for updating musical dna is:" + jsonRaw);


        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on post musical dna. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Post Musical ADN " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on post Musical ADN " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRanking(string _token, string _type, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/ranking/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/ranking/" + _offset.ToString() + "/" + _limit.ToString();

        DebugLogManager.instance.DebugLog(url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch ranking result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveRankingRoot mwsiveRankingRoot = JsonConvert.DeserializeObject<MwsiveRankingRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveRankingRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch ranking result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetSettings(string _token, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/settings";
        //string url = "http://192.241.129.184/api/settings";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch settings result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveSettingsRoot mwsiveSettingsRoot = JsonConvert.DeserializeObject<MwsiveSettingsRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveSettingsRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch settings result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetChallenges(string _token, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/challenges/" + _offset + "/" + _limit;
        //string url = "http://192.241.129.184/api/challenges/" + _offset + "/" + _limit;

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("offset", _offset.ToString());
        parameters.Add("limit", _limit.ToString());

        url = WebCallsUtils.AddParametersToURI(url + "?", parameters);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch challenges result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveChallengesRoot mwsiveChallengesRoot = JsonConvert.DeserializeObject<MwsiveChallengesRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveChallengesRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch challenges result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetCompleteChallenges(string _token, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/challenges/completed/" + _offset + " / " + _limit;
        //string url = "http://192.241.129.184/api/me/challenges/completed/" + _offset + " / " + _limit;


        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch complete challenges result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveChallengesRoot mwsiveChallengesRoot = JsonConvert.DeserializeObject<MwsiveChallengesRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveChallengesRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch complete challenges result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostChallengeStarted(string _token, int _challenge_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/challenges/started";
        //string url = "http://192.241.129.184/api/me/challenges/started";

        ChallengeCompleteRoot challengeCompleteRoot = new ChallengeCompleteRoot
        {
            challenge_id = _challenge_id
        };

        string jsonRaw = JsonConvert.SerializeObject(challengeCompleteRoot);

        DebugLogManager.instance.DebugLog("Body request for creating a playlist is:" + jsonRaw);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Post challenge started " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveStartedChallengeRoot mwsiveStartedChallengeRoot = JsonConvert.DeserializeObject<MwsiveStartedChallengeRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveStartedChallengeRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on post challenge started " + jsonResult);
            yield break;
        }
    }


    public static IEnumerator CR_PostChallengeComplete(string _token, int _challenge_id, int _registered_challenge_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/me/challenges/completed";
        //string url = "http://192.241.129.184/api/me/challenges/completed";

        ChallengeCompleteRoot challengeCompleteRoot = new ChallengeCompleteRoot
        {
            challenge_id = _challenge_id,
            registered_challenge_id = _registered_challenge_id
        };

        string jsonRaw = JsonConvert.SerializeObject(challengeCompleteRoot);

        DebugLogManager.instance.DebugLog("Body request for creating a playlist is:" + jsonRaw);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                if (webRequest.responseCode.Equals(WebCallsUtils.GATEWAY_TIMEOUT_CODE) || webRequest.responseCode.Equals(WebCallsUtils.REQUEST_TIMEOUT_CODE))
                {
                    DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch recommendations. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Post challenge complete " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveCompleteChallengesRoot mwsiveCompleteChallengesRoot = JsonConvert.DeserializeObject<MwsiveCompleteChallengesRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveCompleteChallengesRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on post challenge complete " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetAdvertising(MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/advertisements/" + _offset + "/" + _limit;
        //string url = "http://192.241.129.184/api/advertisements/" + _offset + "/" + _limit;

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("offset", _offset.ToString());
        parameters.Add("limit", _limit.ToString());

        url = WebCallsUtils.AddParametersToURI(url + "?", parameters);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch advertasing result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveAdvertisingRoot mwsiveAdvertisingRoot = JsonConvert.DeserializeObject<MwsiveAdvertisingRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveAdvertisingRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch advertasing result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostSaveAdvertisementClick(string _token, string _user_id, string _advertisement_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/advertisements/click";
        //string url = "http://192.241.129.184/api/advertisements/click";

        AdvertisementClickRoot advertisementClickRoot = new AdvertisementClickRoot
        {
            advertisement_id = _advertisement_id
        };

        advertisementClickRoot.user_id = _user_id.Equals("") ? null : _user_id;

        string jsonRaw = JsonConvert.SerializeObject(advertisementClickRoot);

        DebugLogManager.instance.DebugLog("Body request for login is: " + jsonRaw);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            if (!_token.Equals(""))
                webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.
                DebugLogManager.instance.DebugLog(webRequest.responseCode);

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    DebugLogManager.instance.DebugLog("Mwsive post save advertasing " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed on post click save advertising out " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRecommendedCurators(MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/recommended/curators/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/recommended/curators/" + _offset.ToString() + "/" + _limit.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch recommended curators result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveRecommendedCuratorsRoot mwsiveRecommendedCuratorsRoot = JsonConvert.DeserializeObject<MwsiveRecommendedCuratorsRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveRecommendedCuratorsRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch recommended curators result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRecommendedArtists(MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/recommended/artists/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/recommended/artists/" + _offset.ToString() + "/" + _limit.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch recommended curators result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveRecommendedArtistsRoot mwsiveRecommendedArtist = JsonConvert.DeserializeObject<MwsiveRecommendedArtistsRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveRecommendedArtist });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch recommended curators result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRecommendedPlaylists(MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/recommended/playlists/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/recommended/playlists/" + _offset.ToString() + "/" + _limit.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch recommended playlists result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveRecommendedPlaylistsRoot mwsiveRecommendedPlaylistRoot = JsonConvert.DeserializeObject<MwsiveRecommendedPlaylistsRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveRecommendedPlaylistRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch recommended playlists result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRecommendedTracks(MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/recommended/tracks/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/recommended/tracks/" + _offset.ToString() + " / " + _limit.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch recommended tracks result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveRecommendedTracksRoot mwsiveRecommendedTrackRoot = JsonConvert.DeserializeObject<MwsiveRecommendedTracksRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveRecommendedTrackRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch recommended tracks result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRecommendedAlbums(MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/recommended/albums/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/recommended/albums/" + _offset.ToString() + "/" + _limit.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch recommended albums result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveRecommendedAlbumsRoot mwsiveRecommendedAlbumRoot = JsonConvert.DeserializeObject<MwsiveRecommendedAlbumsRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveRecommendedAlbumRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch recommended albums result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetGenres(MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/recommended/genres/" + _offset.ToString() + "/" + _limit.ToString();
        //string url = "http://192.241.129.184/api/recommended/genres/" + _offset.ToString() + "/" + _limit.ToString();

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch genres result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    MwsiveGenresRoot mwsiveGenresRoot = JsonConvert.DeserializeObject<MwsiveGenresRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveGenresRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch genres result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetTrackInformation_NoAuth(MwsiveWebCallback _callback, string _track_id)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/tracks/" + _track_id;
        //string url = "http://192.241.129.184/api/tracks/" + _track_id;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch track info. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch track info result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    TrackInfoRoot trackInfoRoot = JsonConvert.DeserializeObject<TrackInfoRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, trackInfoRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch track info: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetTrackInformation_Auth(string _token, MwsiveWebCallback _callback, string _track_id, string _playlist_id)
    {
        string jsonResult = "";

        string url = "https://wavemanager.mwsive.com/api/tracks/" + _track_id + "/info/" + _playlist_id;
        //string url = "http://192.241.129.184/api/tracks/" + _track_id + "/info/" + _playlist_id;

        DebugLogManager.instance.DebugLog("Track info url is: " + url);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                DebugLogManager.instance.DebugLog("Protocol Error or Connection Error on fetch track info. Response Code: " + webRequest.responseCode + ". Result: " + webRequest.result.ToString());
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    DebugLogManager.instance.DebugLog("Fetch track info result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    TrackInfoRoot trackInfoRoot = JsonConvert.DeserializeObject<TrackInfoRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, trackInfoRoot });
                    yield break;
                }
            }

            DebugLogManager.instance.DebugLog("Failed fetch track info: " + jsonResult);
            yield break;
        }
    }
}