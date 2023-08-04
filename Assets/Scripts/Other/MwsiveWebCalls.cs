using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public delegate void MwsiveWebCallback(object[] _value);

public class MwsiveWebCalls : MonoBehaviour
{
    public static IEnumerator CR_PostCreateUser(string _email, string _gender, int _age, ProfileRoot _profile, string[] _playlist_ids, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/register";
        string url = "http://192.241.129.184/api/register";

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

        Debug.Log("Body request for creating a playlist is:" + jsonRaw);

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Create Mwsive user " + jsonResult);
                    MwsiveUserRoot mwsiveUserRoot = JsonConvert.DeserializeObject<MwsiveUserRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveUserRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on create mwsive user " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostLogin(string _email, string _user_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/login";
        string url = "http://192.241.129.184/api/login";

        LogInMwsiveRoot logInMwsiveRoot = new LogInMwsiveRoot
        {
            email = _email,
            user_id = _user_id
        };

        string jsonRaw = JsonConvert.SerializeObject(logInMwsiveRoot);

        Debug.Log("Body request for login is: " + jsonRaw);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonRaw, "application/json"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            //TODO si el error es 404 filtrarlo por que lo necesitamos

            if(webRequest.result == UnityWebRequest.Result.ProtocolError && webRequest.responseCode == 404)
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Response code 404. No Mwsive user found, creating new Mwsive User");
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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
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
                    MwsiveLoginRoot mwsiveLoginRoot = JsonConvert.DeserializeObject<MwsiveLoginRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, mwsiveLoginRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on mwsive log in " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostLogout(string _token, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/logout";
        string url = "http://192.241.129.184/api/logout";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();
            
            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.
                Debug.Log(webRequest.responseCode);

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    //TODO Response when unauthorized
                }

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Mwsive logout " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            Debug.Log("Failed on mwsive log out " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetCurrentMwsiveUser(string _token, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/me";
        string url = "http://192.241.129.184/api/me";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch Mwsive User result: " + jsonResult);
                    MwsiveUserRoot mwsiveUserRoot = JsonConvert.DeserializeObject<MwsiveUserRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveUserRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch mwsive user: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetMwsiveUser(string _token, string _user_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/user";
        string url = "http://192.241.129.184/api/user";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("user_id", _user_id);

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch Mwsive User result: " + jsonResult);
                    MwsiveUserRoot mwsiveUserRoot = JsonConvert.DeserializeObject<MwsiveUserRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveUserRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch mwsive user: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_PostDeleteUser(string _token, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/delete_user";
        string url = "http://192.241.129.184/api/delete_user";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Delete Mwsive user " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            Debug.Log("Failed on delete mwsive user " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostTrackAction(string _token, int _user_id, int _track_id, string _action, float _duration, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/track_action";
        string url = "http://192.241.129.184/api/track/action";

        TrackActionRoot newAction = new TrackActionRoot
        {
            user_id = _user_id,
            track_id = _track_id,
            action = _action,
            duration = _duration
        };

        string jsonRaw = JsonConvert.SerializeObject(newAction);

        Debug.Log("Body request for creating a playlist is:" + jsonRaw);

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Post Track action " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            Debug.Log("Failed on post track action " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetCuratorsThatVoted(string _token, string _track_id, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/track/";
        string url = "http://192.241.129.184/api/track";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("track_id", _track_id);


        url = WebCallsUtils.AddParametersToURI(url + "?", parameters);

        url = url + "/curators/";

        parameters.Add("offset", _offset.ToString());
        parameters.Add("limit", _limit.ToString());

        url = WebCallsUtils.AddParametersToURI(url, parameters);

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch Mwsive Curators result: " + jsonResult);
                    MwsiveCuratorsRoot mwsiveCuratorsRoot = JsonConvert.DeserializeObject<MwsiveCuratorsRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveCuratorsRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch Mwsive Curators result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetFollowingThatVoted(string _token, string _track_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/track_following";
        string url = "http://192.241.129.184/api/track/following";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch following that vote result: " + jsonResult);
                    int following_that_voted = JsonConvert.DeserializeObject<int>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, following_that_voted });
                    yield break;
                }
            }

            Debug.Log("Failed fetch following that vote result result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetFollowers(string _token, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/me/followers";
        string url = "http://192.241.129.184/api/me/followers";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch Mwsive followers result: " + jsonResult);
                    MwsiveFollowersRoot mwsiveFollowersRoot = JsonConvert.DeserializeObject<MwsiveFollowersRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveFollowersRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch Mwsive followers result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetFollowing(string _token, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/me/following";
        string url = "http://192.241.129.184/api/me/following";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch Mwsive followers result: " + jsonResult);
                    MwsiveFollowingRoot mwsiveFollowingRoot = JsonConvert.DeserializeObject<MwsiveFollowingRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveFollowingRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch Mwsive followers result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_PostFollow(string _token, string _user_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        WWWForm form = new WWWForm();

        form.AddField("user_id", _user_id);

        //string url = "https://mwsive.com/me/follow";
        string url = "http://192.241.129.184/api/me/follow";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Post follow " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            Debug.Log("Failed on post follow " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetBadges(string _token, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/me/badges";
        string url = "http://192.241.129.184/api/me/badges";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch Badges result: " + jsonResult);
                    MwsiveBadgesRoot mwsiveBadgesRoot = JsonConvert.DeserializeObject<MwsiveBadgesRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveBadgesRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch badges result: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_PostBadgeComplete(string _token, string _badge_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        WWWForm form = new WWWForm();

        form.AddField("badge_id", _badge_id);

        //string url = "https://mwsive.com/me/badges/complete";
        string url = "http://192.241.129.184/api/me/badges/complete";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Post badge complete user " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            Debug.Log("Failed on post badge complete " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostProfilePicture(string _token, Texture2D _texture, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        WWWForm form = new WWWForm();

        byte[] textureBytes = null;
        Texture2D imageTexture = WebCallsUtils.GetTextureCopy(_texture);
        textureBytes = imageTexture.EncodeToPNG();

        form.AddBinaryData("profile_image", textureBytes);

        //string url = "https://mwsive.com/me/edit_photo";
        string url = "http://192.241.129.184/api/me/edit_photo";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Post profile picture " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            Debug.Log("Failed on post profile picture complete " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostDisplayName(string _token, string _display_name, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        WWWForm form = new WWWForm();

        form.AddField("display_name", _display_name);

        //string url = "https://mwsive.com/me/display_name";
        string url = "http://192.241.129.184/api/me/display_name";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Post display name " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            Debug.Log("Failed on post display name complete " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostUserLink(string _token, string _type, string _url, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        WWWForm form = new WWWForm();

        form.AddField("type", _type);
        form.AddField("url", _url);

        //string url = "https://mwsive.com/me/shared_url";
        string url = "http://192.241.129.184/api/me/shared_url";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Post display name " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            Debug.Log("Failed on post display name complete " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostMusicalADNArtists(string _token, string _type, string[] _artists_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/me/musical_adn_artist";
        string url = "http://192.241.129.184/api/me/musical_adn_artist";

        MusicalDNA musicalDNA = new MusicalDNA
        {
            type = _type
        };

        musicalDNA.track_ids = new List<string>();

        for (int i = 0; i < _artists_id.Length; i++)
        {
            musicalDNA.track_ids.Add(_artists_id[i]);
        }

        string jsonRaw = JsonConvert.SerializeObject(musicalDNA);

        Debug.Log("Body request for creating a playlist is:" + jsonRaw);


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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Post Musical ADN Artists " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            Debug.Log("Failed on post Musical ADN Artists complete " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostMusicalADNTracks(string _token, string _type, string[] _tracks_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/me/musical_adn_tracks";
        string url = "http://192.241.129.184/api/me/musical_adn_tracks";

        MusicalDNA musicalDNA = new MusicalDNA
        {
            type = _type
        };

        musicalDNA.track_ids = new List<string>();

        for (int i = 0; i < _tracks_id.Length; i++)
        {
            musicalDNA.track_ids.Add(_tracks_id[i]);
        }

        string jsonRaw = JsonConvert.SerializeObject(musicalDNA);

        Debug.Log("Body request for creating a playlist is:" + jsonRaw);


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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Post Musical ADN Artists " + jsonResult);
                    _callback(new object[] { webRequest.responseCode, null });
                    yield break;
                }
            }

            Debug.Log("Failed on post Musical ADN Artists complete " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRanking(string _token, string _type, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/ranking";
        string url = "http://192.241.129.184/api/ranking";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("type", _type);
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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch ranking result: " + jsonResult);
                    MwsiveRankingRoot mwsiveRankingRoot = JsonConvert.DeserializeObject<MwsiveRankingRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveRankingRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch ranking result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetSettings(string _token, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/settings";
        string url = "http://192.241.129.184/api/settings";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch settings result: " + jsonResult);
                    MwsiveSettingsRoot mwsiveSettingsRoot = JsonConvert.DeserializeObject<MwsiveSettingsRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveSettingsRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch settings result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetChallenges(string _token, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/challenges";
        string url = "http://192.241.129.184/api/challenges";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch challenges result: " + jsonResult);
                    MwsiveChallengesRoot mwsiveChallengesRoot = JsonConvert.DeserializeObject<MwsiveChallengesRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveChallengesRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch challenges result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetCompleteChallenges(string _token, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/complete_challenges";
        string url = "http://192.241.129.184/api/me/completed_challenges";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch complete challenges result: " + jsonResult);
                    MwsiveChallengesRoot mwsiveChallengesRoot = JsonConvert.DeserializeObject<MwsiveChallengesRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveChallengesRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch complete challenges result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_PostChallengeComplete(string _token, int _challenge_id, MwsiveWebCallback _callback)
    {
        string jsonResult = "";

        WWWForm form = new WWWForm();

        form.AddField("challenge_id", _challenge_id.ToString());

        //string url = "https://mwsive.com/me/set_points";
        string url = "http://192.241.129.184/api/me/set_points";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Post challenge complete " + jsonResult);
                    MwsiveCompleteChallengesRoot mwsiveCompleteChallengesRoot = JsonConvert.DeserializeObject<MwsiveCompleteChallengesRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveCompleteChallengesRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on post challenge complete " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetAdvertising(string _token, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/advertasing";
        string url = "http://192.241.129.184/api/advertasing";

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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch advertasing result: " + jsonResult);
                    MwsiveAdvertisingRoot mwsiveAdvertisingRoot = JsonConvert.DeserializeObject<MwsiveAdvertisingRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveAdvertisingRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch advertasing result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRecommendedCurators(string _token, string _type, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/recommended_curators";
        string url = "http://192.241.129.184/api/recommended_curators";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("type", _type);
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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch recommended curators result: " + jsonResult);
                    MwsiveRecommendedCuratorsRoot mwsiveRecommendedCuratorsRoot = JsonConvert.DeserializeObject<MwsiveRecommendedCuratorsRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveRecommendedCuratorsRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch recommended curators result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRecommendedArtists(string _token, string _type, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/recommended_artists";
        string url = "http://192.241.129.184/api/recommended_artists";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("type", _type);
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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch recommended curators result: " + jsonResult);
                    MwsiveRecommendedArtistsRoot mwsiveRecommendedArtist = JsonConvert.DeserializeObject<MwsiveRecommendedArtistsRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveRecommendedArtist });
                    yield break;
                }
            }

            Debug.Log("Failed fetch recommended curators result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRecommendedPlaylists(string _token, string _type, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/recommended_playlists";
        string url = "http://192.241.129.184/api/recommended_playlists";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("type", _type);
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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch recommended playlists result: " + jsonResult);
                    MwsiveRecommendedPlaylistsRoot mwsiveRecommendedPlaylistRoot = JsonConvert.DeserializeObject<MwsiveRecommendedPlaylistsRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveRecommendedPlaylistRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch recommended playlists result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRecommendedTracks(string _token, string _type, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/recommended_tracks";
        string url = "http://192.241.129.184/api/recommended_tracks";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("type", _type);
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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch recommended tracks result: " + jsonResult);
                    MwsiveRecommendedTracksRoot mwsiveRecommendedTrackRoot = JsonConvert.DeserializeObject<MwsiveRecommendedTracksRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveRecommendedTrackRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch recommended tracks result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetRecommendedAlbums(string _token, string _type, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/recommended_albums";
        string url = "http://192.241.129.184/api/recommended_albums";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("type", _type);
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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch recommended albums result: " + jsonResult);
                    MwsiveRecommendedAlbumsRoot mwsiveRecommendedAlbumRoot = JsonConvert.DeserializeObject<MwsiveRecommendedAlbumsRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveRecommendedAlbumRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch recommended albums result: " + jsonResult);
            yield break;
        }
    }

    public static IEnumerator CR_GetGenres(string _token, string _type, MwsiveWebCallback _callback, int _offset = 0, int _limit = 20)
    {
        string jsonResult = "";

        //string url = "https://mwsive.com/genres";
        string url = "https://mwsive.com/genres";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("type", _type);
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

                Debug.Log("Protocol Error or Connection Error on fetch profile");
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch genres result: " + jsonResult);
                    MwsiveGenresRoot mwsiveGenresRoot = JsonConvert.DeserializeObject<MwsiveGenresRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, mwsiveGenresRoot });
                    yield break;
                }
            }

            Debug.Log("Failed fetch genres result: " + jsonResult);
            yield break;
        }
    }
}