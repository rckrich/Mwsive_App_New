using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MwsiveConnectionManager : MonoBehaviour
{
    private static MwsiveConnectionManager _instance;

    public static MwsiveConnectionManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MwsiveConnectionManager>();
            }
            return _instance;
        }
    }

    public static MwsiveConnectionManager dontDestroyMwsiveConnectionM;


    [Header("UniWebView OAuth Reference")]
    public OAuthHandler oAuthHandler;

    // Use this for initialization
    void Awake()
    {
        if (dontDestroyMwsiveConnectionM == null)
        {
            DontDestroyOnLoad(gameObject);
            dontDestroyMwsiveConnectionM = this;
        }
        else if (dontDestroyMwsiveConnectionM != this)
        {
            Destroy(gameObject);
        }
    }

    #region Mwsive API Call Methods

    public void PostCreateUser(string _email, string _gender, int _age, ProfileRoot _profile, string[] _playlist_ids, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostCreateUser;
        StartCoroutine(MwsiveWebCalls.CR_PostCreateUser(_email, _gender, _age, _profile, _playlist_ids, _callback));
    }

    private void Callback_PostCreateUser(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveCreatenRoot)_value[1]));
    }

    public void PostLogin(string _email, string _user_id, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostLogin;
        StartCoroutine(MwsiveWebCalls.CR_PostLogin(_email, _user_id, _callback));
    }

    private void Callback_PostLogin(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveLoginRoot)_value[1]));
    }

    public void PostLogout(MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostLogout;
        StartCoroutine(MwsiveWebCalls.CR_PostLogout(ProgressManager.instance.progress.userDataPersistance.access_token, _callback));
    }

    private void Callback_PostLogout(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }
    }

    public void GetCurrentMwsiveUser(MwsiveWebCallback _callback = null)
    {
        _callback += Callback_GetCurrentMwsiveUser;
        StartCoroutine(MwsiveWebCalls.CR_GetCurrentMwsiveUser(ProgressManager.instance.progress.userDataPersistance.access_token, _callback));
    }

    private void Callback_GetCurrentMwsiveUser(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveUserRoot)_value[1]));
    }

    public void GetMwsiveUser(string _user_id, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_GetMwsiveUser;
        StartCoroutine(MwsiveWebCalls.CR_GetMwsiveUser(ProgressManager.instance.progress.userDataPersistance.access_token, _user_id, _callback));
    }

    private void Callback_GetMwsiveUser(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveUserRoot)_value[1]));
    }

    public void PostDeleteUser(MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostDeleteUser;
        StartCoroutine(MwsiveWebCalls.CR_PostDeleteUser(ProgressManager.instance.progress.userDataPersistance.access_token, _callback));
    }

    private void Callback_PostDeleteUser(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }
    }

    public void PostTrackAction(int _user_id, int _track_id, string _action, float _duration, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostTrackAction;
        StartCoroutine(MwsiveWebCalls.CR_PostTrackAction(ProgressManager.instance.progress.userDataPersistance.access_token, _user_id, _track_id,_action, _duration, _callback));
    }

    private void Callback_PostTrackAction(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }
    }

    public void GetCuratorsThatVoted(string _track_id, MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetCuratorsThatVoted;
        StartCoroutine(MwsiveWebCalls.CR_GetCuratorsThatVoted(ProgressManager.instance.progress.userDataPersistance.access_token, _track_id,_callback, _offset, _limit));
    }

    private void Callback_GetCuratorsThatVoted(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveCuratorsRoot)_value[1]));
    }

    public void GetFollowingThatVoted(string _track_id, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_GetFollowingThatVoted;
        StartCoroutine(MwsiveWebCalls.CR_GetFollowingThatVoted(ProgressManager.instance.progress.userDataPersistance.access_token, _track_id, _callback));
    }

    private void Callback_GetFollowingThatVoted(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(_value[1]);
    }

    public void GetFollowers(MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetFollowers;
        StartCoroutine(MwsiveWebCalls.CR_GetFollowers(ProgressManager.instance.progress.userDataPersistance.access_token, _callback, _offset, _limit));
    }

    private void Callback_GetFollowers(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveFollowersRoot)_value[1]));
    }

    public void GetFollowing(MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetFollowing;
        StartCoroutine(MwsiveWebCalls.CR_GetFollowing(ProgressManager.instance.progress.userDataPersistance.access_token, _callback, _offset, _limit));
    }

    private void Callback_GetFollowing(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveFollowingRoot)_value[1]));
    }

    public void PostFollow(string _user_id, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostFollow;
        StartCoroutine(MwsiveWebCalls.CR_PostFollow(ProgressManager.instance.progress.userDataPersistance.access_token, _user_id, _callback));
    }

    private void Callback_PostFollow(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }
    }

    public void GetBadges(MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetBadges;
        StartCoroutine(MwsiveWebCalls.CR_GetBadges(ProgressManager.instance.progress.userDataPersistance.access_token, _callback, _offset, _limit));
    }

    private void Callback_GetBadges(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveBadgesRoot)_value[1]));
    }

    public void PostBadgeComplete(string _badge_id, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostBadgeComplete;
        StartCoroutine(MwsiveWebCalls.CR_PostBadgeComplete(ProgressManager.instance.progress.userDataPersistance.access_token, _badge_id, _callback));
    }

    private void Callback_PostBadgeComplete(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }
    }

    public void PostProfilePicture(Texture2D _texture, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostProfilePicture;
        StartCoroutine(MwsiveWebCalls.CR_PostProfilePicture(ProgressManager.instance.progress.userDataPersistance.access_token, _texture, _callback));
    }

    private void Callback_PostProfilePicture(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }
    }

    public void PostDisplayName(string _display_name, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostDisplayName;
        StartCoroutine(MwsiveWebCalls.CR_PostDisplayName(ProgressManager.instance.progress.userDataPersistance.access_token, _display_name, _callback));
    }

    private void Callback_PostDisplayName(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }
    }

    public void PostUserLink(string _type, string _url, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostUserLink;
        StartCoroutine(MwsiveWebCalls.CR_PostUserLink(ProgressManager.instance.progress.userDataPersistance.access_token, _type, _url, _callback));
    }

    private void Callback_PostUserLink(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }
    }

    public void PostMusicalADNArtists(string _type, string[] _artists_id, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostMusicalADNArtists;
        StartCoroutine(MwsiveWebCalls.CR_PostMusicalADNArtists(ProgressManager.instance.progress.userDataPersistance.access_token, _type, _artists_id, _callback));
    }

    private void Callback_PostMusicalADNArtists(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }
    }


    public void PostMusicalADNTracks(string _type, string[] _tracks_id, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostMusicalADNTracks;
        StartCoroutine(MwsiveWebCalls.CR_PostMusicalADNTracks(ProgressManager.instance.progress.userDataPersistance.access_token, _type, _tracks_id, _callback));
    }

    private void Callback_PostMusicalADNTracks(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }
    }

    public void GetRanking(string _type, MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetRanking;
        StartCoroutine(MwsiveWebCalls.CR_GetRanking(ProgressManager.instance.progress.userDataPersistance.access_token, _type, _callback, _offset, _limit));
    }

    private void Callback_GetRanking(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveRankingRoot)_value[1]));
    }

    public void GetSettings(MwsiveWebCallback _callback = null)
    {
        _callback += Callback_GetSettings;
        StartCoroutine(MwsiveWebCalls.CR_GetSettings(ProgressManager.instance.progress.userDataPersistance.access_token, _callback));
    }

    private void Callback_GetSettings(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveSettingsRoot)_value[1]));
    }

    public void GetChallenges(MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetChallenges;
        StartCoroutine(MwsiveWebCalls.CR_GetChallenges(ProgressManager.instance.progress.userDataPersistance.access_token, _callback, _offset, _limit));
    }

    private void Callback_GetChallenges(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveChallengesRoot)_value[1]));
    }

    public void GetCompleteChallenges(MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetCompleteChallenges;
        StartCoroutine(MwsiveWebCalls.CR_GetCompleteChallenges(ProgressManager.instance.progress.userDataPersistance.access_token, _callback, _offset, _limit));
    }

    private void Callback_GetCompleteChallenges(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveChallengesRoot)_value[1]));
    }

    public void PostChallengeComplete(int _challenge_id, MwsiveWebCallback _callback = null)
    {
        _callback += Callback_PostChallengeComplete;
        StartCoroutine(MwsiveWebCalls.CR_PostChallengeComplete(ProgressManager.instance.progress.userDataPersistance.access_token, _challenge_id, _callback));
    }

    private void Callback_PostChallengeComplete(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveCompleteChallengesRoot)_value[1]));
    }

    public void GetAdvertising(MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetAdvertising;
        StartCoroutine(MwsiveWebCalls.CR_GetAdvertising(ProgressManager.instance.progress.userDataPersistance.access_token, _callback, _offset, _limit));
    }

    private void Callback_GetAdvertising(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveAdvertisingRoot)_value[1]));
    }

    public void GetRecommendedCurators(string _type, MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetRecommendedCurators;
        StartCoroutine(MwsiveWebCalls.CR_GetRecommendedCurators(ProgressManager.instance.progress.userDataPersistance.access_token, _type, _callback, _offset, _limit));
    }

    private void Callback_GetRecommendedCurators(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveRecommendedCuratorsRoot)_value[1]));
    }

    public void GetRecommendedArtists(string _type, MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetRecommendedArtists;
        StartCoroutine(MwsiveWebCalls.CR_GetRecommendedArtists(ProgressManager.instance.progress.userDataPersistance.access_token, _type, _callback, _offset, _limit));
    }

    private void Callback_GetRecommendedArtists(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveRecommendedArtistsRoot)_value[1]));
    }

    public void GetRecommendedPlaylists(string _type, MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetRecommendedPlaylists;
        StartCoroutine(MwsiveWebCalls.CR_GetRecommendedPlaylists(ProgressManager.instance.progress.userDataPersistance.access_token, _type, _callback, _offset, _limit));
    }

    private void Callback_GetRecommendedPlaylists(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveRecommendedPlaylistsRoot)_value[1]));
    }

    public void GetRecommendedTracks(string _type, MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetRecommendedTracks;
        StartCoroutine(MwsiveWebCalls.CR_GetRecommendedTracks(ProgressManager.instance.progress.userDataPersistance.access_token, _type, _callback, _offset, _limit));
    }

    private void Callback_GetRecommendedTracks(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveRecommendedTracksRoot)_value[1]));
    }

    public void GetRecommendedAlbums(string _type, MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetRecommendedAlbums;
        StartCoroutine(MwsiveWebCalls.CR_GetRecommendedAlbums(ProgressManager.instance.progress.userDataPersistance.access_token, _type, _callback, _offset, _limit));
    }

    private void Callback_GetRecommendedAlbums(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveRecommendedAlbumsRoot)_value[1]));
    }

    public void GetGenres(string _type, MwsiveWebCallback _callback = null, int _offset = 0, int _limit = 20)
    {
        _callback += Callback_GetGenres;
        StartCoroutine(MwsiveWebCalls.CR_GetGenres(ProgressManager.instance.progress.userDataPersistance.access_token, _type, _callback, _offset, _limit));
    }

    private void Callback_GetGenres(object[] _value)
    {
        if (CheckResponse((long)_value[0]))
        {
            return;
        }

        Debug.Log(((MwsiveGenresRoot)_value[1]));
    }

    #endregion

    public bool CheckResponse(long _responseCode)
    {
        return _responseCode.Equals(WebCallsUtils.AUTHORIZATION_FAILED_RESPONSE_CODE);
    }
}
