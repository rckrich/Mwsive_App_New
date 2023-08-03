using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public delegate void SpotifyWebCallback(object[] _value);

public static class SpotifyWebCalls
{
    public static IEnumerator CR_GetCurrentUserProfile(string _token, SpotifyWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/me";

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch profile. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch Profile result: " + jsonResult);
                    ProfileRoot profileRoot = JsonConvert.DeserializeObject<ProfileRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, profileRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch profile: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetCurrentUserTopTracks(string _token, SpotifyWebCallback _callback, string _time_range = "medium_term", int _limit = 20, int _offset = 0)
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/me/top/tracks";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("time_range", _time_range);
        parameters.Add("limit", _limit.ToString());
        parameters.Add("offset", _offset.ToString());

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch top tracks. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch user top songs result: " + jsonResult);
                    UserTopItemsRoot userTopItemsRoot = JsonConvert.DeserializeObject<UserTopItemsRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, userTopItemsRoot });

                    /*AudioDownloadManager.instance.SetTotalTracksNumber(userTopItemsRoot.items.Count);

                    foreach (Item item in userTopItemsRoot.items)
                    {
                        if ((!item.preview_url.Equals("")) || (item.preview_url != null))
                            AudioDownloadManager.instance.AddTrackToList(item.name, item.preview_url, item.album.images[0].url, item.album.artists[0].name);
                    }

                    Debug.Log(userTopItemsRoot.ToString());*/
                    yield break;
                }
            }

            Debug.Log("Failed on fetch user top songs: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetCurrentUserTopArtists(string _token, SpotifyWebCallback _callback, string _time_range = "medium_term", int _limit = 20, int _offset = 0)
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/me/top/artists";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("time_range", _time_range);
        parameters.Add("limit", _limit.ToString());
        parameters.Add("offset", _offset.ToString());

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch top artist. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch user top artist result: " + jsonResult);
                    UserTopItemsRoot userTopItemsRoot = JsonConvert.DeserializeObject<UserTopItemsRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, userTopItemsRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch user top artist: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetCurrentUserPlaylists(string _token, SpotifyWebCallback _callback, int _limit = 20, int _offset = 0)
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/me/playlists";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("limit", _limit.ToString());
        parameters.Add("offset", _offset.ToString());

        url = WebCallsUtils.AddParametersToURI(url + "?", parameters);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if(webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch playlists. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch user playlists result: " + jsonResult);
                    PlaylistRoot playlistRoot = JsonConvert.DeserializeObject<PlaylistRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, playlistRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch user playlists: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetUserPlaylists(string _token, SpotifyWebCallback _callback, string _user_id, int _limit = 20, int _offset = 0)
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/users/" + _user_id + "/playlists";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("limit", _limit.ToString());
        parameters.Add("offset", _offset.ToString());

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch playlists. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch user " + _user_id + " playlists result: " + jsonResult);
                    PlaylistRoot playlistRoot = JsonConvert.DeserializeObject<PlaylistRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, playlistRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch user " + _user_id + "playlists: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetPlaylist(string _token, SpotifyWebCallback _callback, string _playlist_id, string _market = "ES")
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/playlists/" + _playlist_id;

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("market", _market);

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch playlist. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch playlist result: " + jsonResult);
                    SearchedPlaylist searchedPlaylist = JsonConvert.DeserializeObject<SearchedPlaylist>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, searchedPlaylist });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch playlist: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetPlaylistItems(string _token, SpotifyWebCallback _callback, string _playlist_id, string _market = "ES", int _limit = 20, int _offset = 0)
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/playlists/" + _playlist_id + "/tracks";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("market", _market);
        parameters.Add("limit", _limit.ToString());
        parameters.Add("offset", _offset.ToString());

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch playlists' items. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch playlist result: " + jsonResult);
                    PlaylistRoot playlistRoot = JsonConvert.DeserializeObject<PlaylistRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, playlistRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch playlist: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetPlaylistByURL(string _token, string _url, SpotifyWebCallback _callback)
    {
        string jsonResult = "";

        string url = _url;

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch playlist by url. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch playlist result: " + jsonResult);
                    PlaylistRoot playlistRoot = JsonConvert.DeserializeObject<PlaylistRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, playlistRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch playlist: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetTrack(string _token, SpotifyWebCallback _callback, string _track_id, string _market = "ES")
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/tracks/" + _track_id;

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("market", _market);

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch track. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch track " + _track_id + " result: " + jsonResult);
                    TrackRoot trackRoot = JsonConvert.DeserializeObject<TrackRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, trackRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch track " + _track_id + ": " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetSeveralTracks(string _token, SpotifyWebCallback _callback, string[] _track_ids, string _market = "ES")
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/tracks";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("market", _market);

        url = WebCallsUtils.AddParametersToURI(url + "?", parameters);

        url = WebCallsUtils.AddMultipleParameterToUri(url + "&", "ids", _track_ids);

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch several tracks. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch several tracks result " + jsonResult);
                    SeveralTrackRoot severalTrackRoot = JsonConvert.DeserializeObject<SeveralTrackRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, severalTrackRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch several tracks: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_CreatePlaylist(string _token, SpotifyWebCallback _callback, string _user_id, string _playlist_name = "Mwsive Playlist", string _playlist_description = "New Mwsive playlist", bool _public = false)
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/users/" + _user_id + "/playlists";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            CreatePlaylistBodyRequestRoot bodyRequest = new CreatePlaylistBodyRequestRoot {
                name = _playlist_name,
                description = _playlist_description,
                @public = _public
            };

            string jsonRaw = JsonConvert.SerializeObject(bodyRequest);

            Debug.Log("Body request for creating a playlist is:" + jsonRaw);

            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRaw);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on create playlist. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Playlist created " + jsonResult);
                    CreatedPlaylistRoot createdPlaylistRoot = JsonConvert.DeserializeObject<CreatedPlaylistRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, createdPlaylistRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on crate playlist " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_ChangePlaylistDetails(string _token, SpotifyWebCallback _callback, string _playlist_id, string _playlist_name = "Mwsive Playlist", string _playlist_description = "New Mwsive playlist", bool _public = false)
    { 
        string url = "https://api.spotify.com/v1/playlists/" + _playlist_id;

        string jsonRaw = "";

        if (_playlist_description.Equals(""))
        {
            EditPlaylistCompleteBodyRequestRoot bodyRequest = new EditPlaylistCompleteBodyRequestRoot
            {
                name = _playlist_name,
                description = _playlist_description,
                @public = _public
            };

            jsonRaw = JsonConvert.SerializeObject(bodyRequest);
        }
        else
        {
            EditPlaylistNoDescriptionBodyRequestRoot bodyRequest = new EditPlaylistNoDescriptionBodyRequestRoot
            {
                name = _playlist_name,
                @public = _public
            };

            jsonRaw = JsonConvert.SerializeObject(bodyRequest);
        }

        Debug.Log("Body request for changing a playlist is:" + jsonRaw);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRaw);

        using (UnityWebRequest webRequest = UnityWebRequest.Put(url, bodyRaw))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on change playlist's details. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    Debug.Log("Playlist updated");
                    _callback(new object[] { WebCallsUtils.SUCCESS_RESPONSE_CODE });
                    yield break;
                }
            }

            Debug.Log("Failed on update playlist");
            yield break;

        }
    }

    public static IEnumerator CR_AddItemsToPlaylist(string _token, SpotifyWebCallback _callback, string _playlist_id, List<string> _uris, int _position = 0)
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/playlists/" + _playlist_id + "/tracks";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("position", _position.ToString());

        url = WebCallsUtils.AddParametersToURI(url + "?", parameters);

        url = WebCallsUtils.AddSpotifyUrisToUri(url + "&", _uris);

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            AddItemsPlaylistBodyRequestRoot bodyRequest = new AddItemsPlaylistBodyRequestRoot
            {
                position = _position,
            };

            bodyRequest.uris = new List<string>();

            foreach (string uri in _uris)
            {
                bodyRequest.uris.Add(uri);
            }

            string jsonRaw = JsonConvert.SerializeObject(bodyRequest);

            Debug.Log("Body request for creating a playlist is:" + jsonRaw);

            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRaw);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on add items to playlist. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Added tracks to playlist " + _playlist_id + " result: " + jsonResult);
                    AddItemsToPlaylistRoot addItemsToPlaylistRoot = JsonConvert.DeserializeObject<AddItemsToPlaylistRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, addItemsToPlaylistRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on add tracks to playlist " + _playlist_id + ": " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_RemoveItemsFromPlaylist(string _token, SpotifyWebCallback _callback, string _playlist_id, List<string> _uris)
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/playlists/" + _playlist_id + "/tracks";

        using (UnityWebRequest webRequest = UnityWebRequest.Delete(url))
        {
            RemoveItemsPlaylistBodyRequestRoot bodyRequest = new RemoveItemsPlaylistBodyRequestRoot();

            bodyRequest.tracks = new List<RemoveTrack>();

            foreach (string spotifyUri in _uris)
            {
                RemoveTrack removeTrack = new RemoveTrack { uri = spotifyUri };
                bodyRequest.tracks.Add(removeTrack);
            }

            string jsonRaw = JsonConvert.SerializeObject(bodyRequest);

            Debug.Log("Body request for creating a playlist is:" + jsonRaw);

            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRaw);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + _token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.

                if (webRequest.responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE))
                {
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on remove items to playlist. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Removed tracks to playlist " + _playlist_id + " result: " + jsonResult);
                    RemoveItemsToPlaylistRoot removeItemsToPlaylistRoot = JsonConvert.DeserializeObject<RemoveItemsToPlaylistRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, removeItemsToPlaylistRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on remove tracks to playlist " + _playlist_id + ": " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetRecommendations(string _token, SpotifyWebCallback _callback, string[] _seed_artists,/* string[] _seed_genres,*/ string[] _seed_tracks, int _limit = 20, string _market = "ES")
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/recommendations";

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("limit", _limit.ToString());
        parameters.Add("market", _market);

        url = WebCallsUtils.AddParametersToURI(url + "?", parameters);

        url = WebCallsUtils.AddMultipleParameterToUri(url + "&", "seed_artists", _seed_artists);
        /*url = WebCallsUtils.AddMultipleParameterToUri(url + "&", "seed_genres", _seed_genres);*/
        url = WebCallsUtils.AddMultipleParameterToUri(url + "&", "seed_tracks", _seed_tracks);

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch recommendations. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch recommendations result: " + jsonResult);
                    RecommendationsRoot recommendationsRoot = JsonConvert.DeserializeObject<RecommendationsRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, recommendationsRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch recommendations: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_SearchForItem(string _token, SpotifyWebCallback _callback, string _query, string[] _types, string _market = "ES", int _limit = 20, int _offset = 0, string _include_external = "audio")
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/search";

        url = url + "?q=" + Uri.EscapeDataString(_query);

        //Accepted strings are: "album", "artist", "playlist", "track", "show", "episode" and "audiobook"
        url = WebCallsUtils.AddMultipleParameterToUri(url + "&", "type", _types);

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("market", _market);
        parameters.Add("limit", _limit.ToString());
        parameters.Add("offset", _offset.ToString());
        parameters.Add("include_external", _include_external);

        url = WebCallsUtils.AddParametersToURI(url + "&", parameters);

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on search for items. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch search result: " + jsonResult);
                    JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    SearchRoot searchRoot = JsonConvert.DeserializeObject<SearchRoot>(jsonResult, settings);
                    _callback(new object[] { webRequest.responseCode, searchRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch search: " + jsonResult);
            yield break;

        }
    }

    public static IEnumerator CR_GetGenres(string _token, SpotifyWebCallback _callback)
    {
        string jsonResult = "";

        string url = "https://api.spotify.com/v1/recommendations/available-genre-seeds";

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
                    WebCallsUtils.ReauthenticateUser(_callback);
                }

                Debug.Log("Protocol Error or Connection Error on fetch genres. Response Code: " + webRequest.responseCode + ". Error: " + webRequest.downloadHandler.text);
                yield break;
            }
            else
            {
                while (!webRequest.isDone) { yield return null; }

                if (webRequest.isDone)
                {
                    jsonResult = webRequest.downloadHandler.text;
                    Debug.Log("Fetch genres result: " + jsonResult);
                    GenresRoot genresRoot = JsonConvert.DeserializeObject<GenresRoot>(jsonResult);
                    _callback(new object[] { webRequest.responseCode, genresRoot });
                    yield break;
                }
            }

            Debug.Log("Failed on fetch genres: " + jsonResult);
            yield break;

        }
    }
}
