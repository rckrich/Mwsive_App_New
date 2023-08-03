using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpotifyConnectionManager : Manager
{
    private static SpotifyConnectionManager _instance;

    public static SpotifyConnectionManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SpotifyConnectionManager>();
            }
            return _instance;
        }
    }

    public static SpotifyConnectionManager dontDestroySpotifyConnectionM;

#if UNITY_EDITOR_WIN
    [Header("Test Token")]
    [TextAreaAttribute]
    public string testRawValue = "";
#endif

    [Header("UniWebView OAuth Reference")]
    public OAuthHandler oAuthHandler;

    // Use this for initialization
    void Awake()
    {
        if (dontDestroySpotifyConnectionM == null)
        {
            DontDestroyOnLoad(gameObject);
            dontDestroySpotifyConnectionM = this;
        }
        else if (dontDestroySpotifyConnectionM != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartConnection(SpotifyWebCallback _callback = null)
    {
        if (ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted)
        {

#if UNITY_EDITOR_WIN
            string rawValue = !testRawValue.Equals("") ? testRawValue : ProgressManager.instance.progress.userDataPersistance.raw_value;
#else
            string rawValue = ProgressManager.instance.progress.userDataPersistance.raw_value;
#endif
            oAuthHandler.SetSpotifyTokenRawValue(rawValue);

            if (ProgressManager.instance.progress.userDataPersistance.spotify_expires_at.CompareTo(DateTime.Now) < 0)
            {
                Debug.Log("Saved token has expired, starting refresh flow");
                oAuthHandler.SpotifyStartRefreshFlow(_callback);
            }
            else
            {
                Debug.Log("Saved token has not expired, can continue normally");
                _callback(new object[]
                {
                    oAuthHandler.GetSpotifyToken().RawValue
                }) ;
            }
        }
        else
        {
#if UNITY_EDITOR_WIN
            if (!testRawValue.Equals(""))
            {
                oAuthHandler.SetSpotifyTokenRawValue(testRawValue);
                SaveToken(oAuthHandler.GetSpotifyToken().RawValue, oAuthHandler.GetSpotifyToken().ExpiresIn);
                if (_callback != null)
                {
                    _callback(new object[] { oAuthHandler.GetSpotifyToken().RawValue });
                }
            }
            else
            {
                oAuthHandler.SpotifyStartAuthFlow(_callback);
            }
#else
            oAuthHandler.SpotifyStartAuthFlow(_callback);
#endif
        }
    }

    public void StartRefreshRequestOnly(SpotifyWebCallback _callback = null)
    {
#if UNITY_EDITOR_WIN
            string rawValue = !testRawValue.Equals("") ? testRawValue : ProgressManager.instance.progress.userDataPersistance.raw_value;
#else
        string rawValue = ProgressManager.instance.progress.userDataPersistance.raw_value;
#endif
        oAuthHandler.SetSpotifyTokenRawValue(rawValue);

        if (ProgressManager.instance.progress.userDataPersistance.spotify_expires_at.CompareTo(DateTime.Now) < 0)
        {
            Debug.Log("Saved token has expired, starting refresh flow");
            oAuthHandler.SpotifyStartRefreshFlow(_callback);
        }
    }

    public void SaveToken(string _rawValue, long _expiresIn)
    {
        ProgressManager.instance.progress.userDataPersistance.raw_value = _rawValue;
        ProgressManager.instance.progress.userDataPersistance.spotify_expires_at = ConvertExpiresInToDateTime(_expiresIn);
        ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted = true;
        ProgressManager.instance.save();
    }

    public void ResetToken()
    {
        ProgressManager.instance.progress.userDataPersistance.raw_value = "";
        ProgressManager.instance.progress.userDataPersistance.spotify_expires_at = new DateTime(1990, 01, 01);
        ProgressManager.instance.progress.userDataPersistance.spotify_userTokenSetted = false;
        ProgressManager.instance.save();
    }

    public bool CheckReauthenticateUser(long _responseCode)
    {
        return _responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE);
    }

    #region Spotify API Call Methods

    public void GetCurrentUserProfile(SpotifyWebCallback _callback = null)
    {
        _callback += Callback_GetUserProfile;
        StartCoroutine(SpotifyWebCalls.CR_GetCurrentUserProfile(oAuthHandler.GetSpotifyToken().AccessToken, _callback));
    }

    private void Callback_GetUserProfile(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0])) {
            StartReauthentication();
            return;
        }

        Debug.Log(((ProfileRoot)_value[1]).display_name);
    }

    public void GetCurrentUserTopTracks(SpotifyWebCallback _callback = null)
    {
        _callback += Callback_GetCurrentUserTopTracks;
        StartCoroutine(SpotifyWebCalls.CR_GetCurrentUserTopTracks(oAuthHandler.GetSpotifyToken().AccessToken, _callback));
    }

    private void Callback_GetCurrentUserTopTracks(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((UserTopItemsRoot)_value[1]);
    }

    public void GetCurrentUserTopArtists(SpotifyWebCallback _callback = null)
    {
        _callback += Callback_GetCurrentUserTopArtists;
        StartCoroutine(SpotifyWebCalls.CR_GetCurrentUserTopArtists(oAuthHandler.GetSpotifyToken().AccessToken, _callback));
    }

    private void Callback_GetCurrentUserTopArtists(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((UserTopItemsRoot)_value[1]);
    }

    public void GetCurrentUserPlaylists(SpotifyWebCallback _callback = null, int _limit = 20, int _offset = 0)
    {
        _callback += Callback_GetCurrentUserPlaylists;
        StartCoroutine(SpotifyWebCalls.CR_GetCurrentUserPlaylists(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _limit, _offset));
    }

    private void Callback_GetCurrentUserPlaylists(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((PlaylistRoot)_value[1]);
    }

    public void GetUserPlaylists(string _userSpotifyID, SpotifyWebCallback _callback = null, int _limit = 20, int _offset = 0)
    {
        _callback += Callback_GetUserPlaylists;
        StartCoroutine(SpotifyWebCalls.CR_GetUserPlaylists(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _userSpotifyID, _limit, _offset));
    }

    private void Callback_GetUserPlaylists(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((PlaylistRoot)_value[1]);
    }

    public void GetPlaylist(string _playlistID, SpotifyWebCallback _callback = null, string _market = "ES")
    {
        _callback += Callback_GetPlaylist;
        StartCoroutine(SpotifyWebCalls.CR_GetPlaylist(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _playlistID, _market));
    }

    private void Callback_GetPlaylist(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((SearchedPlaylist)_value[1]);
    }

    public void GetPlaylistItems(string _playlistID, SpotifyWebCallback _callback = null, string _market = "ES", int _limit = 20, int _offset = 0)
    {
        _callback += Callback_GetPlaylistItems;
        StartCoroutine(SpotifyWebCalls.CR_GetPlaylistItems(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _playlistID, _market, _limit, _offset));
    }

    private void Callback_GetPlaylistItems(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((PlaylistRoot)_value[1]);
    }

    public void GetPlaylistByURL(string _url, SpotifyWebCallback _callback = null)
    {
        _callback += Callback_GetPlaylistByURL;
        StartCoroutine(SpotifyWebCalls.CR_GetPlaylistByURL(oAuthHandler.GetSpotifyToken().AccessToken, _url, _callback));
    }

    private void Callback_GetPlaylistByURL(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((PlaylistRoot)_value[1]);
    }

    public void GetTrack(string _trackSpotifyID, SpotifyWebCallback _callback = null, string _market = "ES")
    {
        _callback += Callback_GetTrack;
        StartCoroutine(SpotifyWebCalls.CR_GetTrack(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _trackSpotifyID, _market));
    }

    private void Callback_GetTrack(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((TrackRoot)_value[1]);
    }

    public void GetSeveralTracks(string[] _track_ids, SpotifyWebCallback _callback = null, string _market = "ES")
    {
        _callback += Callback_GetSeveralTracks;
        StartCoroutine(SpotifyWebCalls.CR_GetSeveralTracks(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _track_ids, _market));
    }

    private void Callback_GetSeveralTracks(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((SeveralTrackRoot)_value[1]);
    }

    public void CreatePlaylist(string _user_id, SpotifyWebCallback _callback = null, string _playlist_name = "Mwsive Playlist", string _playlist_description = "New Mwsive playlist", bool _public = false)
    {
        _callback += Callback_CreatePlaylist;
        StartCoroutine(SpotifyWebCalls.CR_CreatePlaylist(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _user_id, _playlist_name, _playlist_description, _public));
    }

    private void Callback_CreatePlaylist(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((CreatedPlaylistRoot)_value[1]);
    }

    public void ChangePlaylistDetails(string _playlist_id, SpotifyWebCallback _callback = null, string _playlist_name = "Mwsive Playlist", string _playlist_description = "New Mwsive playlist", bool _public = false)
    {
        _callback += Callback_ChangePlaylistDetails;
        StartCoroutine(SpotifyWebCalls.CR_ChangePlaylistDetails(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _playlist_id, _playlist_name, _playlist_description, _public));
    }

    private void Callback_ChangePlaylistDetails(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }
    }

    public void AddItemsToPlaylist(string _playlist_id, List<string> _uris, SpotifyWebCallback _callback = null, int _position = 0)
    {
        _callback += Callback_AddItemsToPlaylist;
        StartCoroutine(SpotifyWebCalls.CR_AddItemsToPlaylist(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _playlist_id, _uris, _position));
    }

    private void Callback_AddItemsToPlaylist(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((AddItemsToPlaylistRoot)_value[1]);
    }

    public void RemoveItemsFromPlaylist(string _playlist_id, List<string> _uris, SpotifyWebCallback _callback = null)
    {
        _callback += Callback_RemoveItemsFromPlaylist;
        StartCoroutine(SpotifyWebCalls.CR_RemoveItemsFromPlaylist(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _playlist_id, _uris));
    }

    private void Callback_RemoveItemsFromPlaylist(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((RemoveItemsToPlaylistRoot)_value[1]);
    }

    public void GetRecommendations(string[] _seed_artists, /*string[] _seed_genres,*/ string[] _seed_tracks, SpotifyWebCallback _callback = null, int _limit = 20, string _market = "ES")
    {
        _callback += Callback_GetRecommendations;
        StartCoroutine(SpotifyWebCalls.CR_GetRecommendations(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _seed_artists,/* _seed_genres,*/ _seed_tracks, _limit, _market));
    }

    private void Callback_GetRecommendations(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((RecommendationsRoot)_value[1]);
    }

    public void SearchForItem(string _query, string[] _types, SpotifyWebCallback _callback = null, string _market = "ES", int _limit = 20, int _offset = 0, string _include_external = "audio")
    {
        _callback += Callback_SearchForItem;
        StartCoroutine(SpotifyWebCalls.CR_SearchForItem(oAuthHandler.GetSpotifyToken().AccessToken, _callback, _query, _types, _market, _limit, _offset, _include_external));
    }

    private void Callback_SearchForItem(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((SearchRoot)_value[1]);
    }

    public void CR_GetGenres(string _query, string[] _types, SpotifyWebCallback _callback = null)
    {
        _callback += Callback_CR_GetGenres;
        StartCoroutine(SpotifyWebCalls.CR_GetGenres(oAuthHandler.GetSpotifyToken().AccessToken, _callback));
    }

    private void Callback_CR_GetGenres(object[] _value)
    {
        if (CheckReauthenticateUser((long)_value[0]))
        {
            StartReauthentication();
            return;
        }

        Debug.Log((GenresRoot)_value[1]);
    }

    #endregion

    #region Private Methods

    private DateTime ConvertExpiresInToDateTime(long _secondsToAdd)
    {
        DateTime expiresAbsolute = DateTime.Now.AddSeconds(_secondsToAdd);
        return expiresAbsolute;
    }

    private void StartReauthentication()
    {
        StopAllCoroutines();
        ResetToken();
        StartConnection();
    }

    #endregion
}
