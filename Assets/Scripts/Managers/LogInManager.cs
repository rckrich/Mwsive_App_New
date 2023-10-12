using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public delegate void LogInCallback(object[] _value);

public class LogInManager : Manager
{
    private static LogInManager _instance;

    public static LogInManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LogInManager>();
            }
            return _instance;
        }
    }

    private string email;
    private string profileid;
    private string playlistid;
    private string[] itemIDs;
    private ProfileRoot profile;
    private MwsiveUser mwsiveUser;

    public Sprite LogInErrorSprite;

    private LogInCallback previousAction;

    public void StartLogInProcess(LogInCallback _callback = null)
    {
        previousAction = _callback;

        NewScreenManager.instance.GetCurrentView().StartSearch();

        SpotifyConnectionManager.instance.StartConnection(Callback_StartSpotifyConnection);
    }

    private void Callback_StartSpotifyConnection(object[] _value)
    {
        if(_value != null)
        {
            if(_value.Length > 0 && (((string)_value[0]).Equals("AuthError") || ((string)_value[0]).Equals("RefreshError")))
            {
                DebugLogManager.instance.DebugLog((string)_value[0]);
                NewScreenManager.instance.GetCurrentView().EndSearch();
                NewScreenManager.instance.ChangeToMainView(ViewID.PopUpViewModel, true);
                PopUpViewModel popUpViewModel = (PopUpViewModel)NewScreenManager.instance.GetMainView(ViewID.PopUpViewModel);
                popUpViewModel.Initialize(PopUpViewModelTypes.MessageOnly, "Advertencia", "Ocurrió un error en el proceso de Inicio de Sesión. Volver a intentar.", "Aceptar", LogInErrorSprite);
                popUpViewModel.SetPopUpAction(() => { NewScreenManager.instance.BackToPreviousView(); });
                return;
            }
        }

        MwsiveTokenLogInProcess();
    }

    public void MwsiveTokenLogInProcess()
    {
        if (ProgressManager.instance.progress.userDataPersistance.userTokenSetted)
        {
            DebugLogManager.instance.DebugLog(ProgressManager.instance.progress.userDataPersistance.access_token);
            if (HasMwsiveTokenExpired())
            {
                SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
            }
            else
            {
                if (IsCurrentPlaylistEmpty())
                {
                    SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_GetCurrentUserPlaylists);
                }
                else
                {
                    NewScreenManager.instance.GetCurrentView().EndSearch();
                    SceneManager.LoadScene("MainScene");
                }
            }
        }
        else
        {
            SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
        }
    }

    private void Callback_GetCurrentUserPlaylists(object[] _value)
    {
        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];

        itemIDs = new string[playlistRoot.items.Count];

        for (int i = 0; i < playlistRoot.items.Count; i++)
        {
            itemIDs[i] = playlistRoot.items[i].id;
        }

        if (mwsiveUser != null) {
            if(mwsiveUser.last_selected_playlist != null && !mwsiveUser.last_selected_playlist.Equals(""))
            {
                SetCurrentPlaylist(mwsiveUser.last_selected_playlist);
            }
            else
            {
                PlaylistException(playlistRoot, itemIDs);
                return;
            }
        }
        else
        {
            PlaylistException(playlistRoot, itemIDs);
            return;
        }

        EndProcess();
    }

    private void PlaylistException(PlaylistRoot _playlistRoot, string[] _itemIDs)
    {
        if (_playlistRoot.items.Count <= 0)
        {
            SpotifyConnectionManager.instance.CreatePlaylist(profileid, Callback_CreatePlaylistException);
        }
        else
        {
            SetCurrentPlaylist(itemIDs[0]);
            EndProcess();
        }
    }

    private void Callback_CreatePlaylistException(object[] _value)
    {
        SpotifyPlaylistRoot spotifyPlaylistRoot = (SpotifyPlaylistRoot)_value[1];

        playlistid = spotifyPlaylistRoot.id;

        SetCurrentPlaylist(playlistid);

        SpotifyConnectionManager.instance.PutChangePlaylistCoverImage(playlistid, WebCallsUtils.MWSIVE_COVER_BASECODE64);

        EndProcess();
    }

    private void Callback_GetUserProfile(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        profile = profileRoot;
        profileid = profileRoot.id;
        email = profileRoot.email;

        MwsiveConnectionManager.instance.PostLogin(email, profileid, Callback_PostLogin);
    }

    private void Callback_PostLogin(object[] _value)
    {
        
        string webcode = ((long)_value[0]).ToString();
        if (webcode == "204" || webcode == "200")
        {
            MwsiveLoginRoot mwsiveLoginRoot = (MwsiveLoginRoot)_value[1];

            mwsiveUser = mwsiveLoginRoot.user;

            DebugLogManager.instance.DebugLog(mwsiveLoginRoot);

            SetMwsiveToken(mwsiveLoginRoot.access_token, DateTime.Now.AddHours(1));

            if (IsCurrentPlaylistEmpty())
            {
                SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_GetCurrentUserPlaylists);
            }
            else
            {
                EndProcess();
            }
        }
        else if (webcode == "404")
        {
            DebugLogManager.instance.DebugLog(ProgressManager.instance.progress.userDataPersistance.access_token);
            SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_GetCurrentUserPlaylistsNewUser);
        }
        else
        {
            //TODO Pop Up Error
        }
    }

    private void Callback_GetCurrentUserPlaylistsNewUser(object[] _value)
    {
        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];

        itemIDs = new string[playlistRoot.items.Count];

        for (int i = 0; i < playlistRoot.items.Count; i++)
        {
            itemIDs[i] = playlistRoot.items[i].id;
        }

        MwsiveConnectionManager.instance.PostCreateUser(email, "null", 0, profile, itemIDs, Callback_PostCreateUser);
    }

    private void Callback_PostCreateUser(object[] _value)
    {
        MwsiveCreatedRoot mwsiveCreatedRoot = (MwsiveCreatedRoot)_value[1];
        SpotifyConnectionManager.instance.CreatePlaylist(profileid, Callback_CreatePlaylist);
    }

    private void Callback_CreatePlaylist(object[] _value)
    {
        SpotifyPlaylistRoot spotifyPlaylistRoot = (SpotifyPlaylistRoot)_value[1];

        playlistid = spotifyPlaylistRoot.id;

        SetCurrentPlaylist(playlistid);

        SpotifyConnectionManager.instance.PutChangePlaylistCoverImage(playlistid, WebCallsUtils.MWSIVE_COVER_BASECODE64);

        MwsiveConnectionManager.instance.PostLogin(email, profileid, Callback_PostLogInAfterCreatingUser);
    }

    private void Callback_PostLogInAfterCreatingUser(object[] _value)
    {
        MwsiveLoginRoot mwsiveLoginRoot = (MwsiveLoginRoot)_value[1];

        SetMwsiveToken(mwsiveLoginRoot.access_token, DateTime.Now.AddHours(1));

        EndProcess();
    }

    private void SetCurrentPlaylist(string _value)
    {
        ProgressManager.instance.progress.userDataPersistance.current_playlist = _value;
        ProgressManager.instance.save();

        MwsiveConnectionManager.instance.PutLastSavedPlaylist(_value);
    }

    private void SetMwsiveToken(string _value, DateTime _expire_date)
    {
        ProgressManager.instance.progress.userDataPersistance.access_token = _value;
        DebugLogManager.instance.DebugLog(ProgressManager.instance.progress.userDataPersistance.access_token);
        ProgressManager.instance.progress.userDataPersistance.expires_at = _expire_date;
        ProgressManager.instance.progress.userDataPersistance.userTokenSetted = true;
        ProgressManager.instance.save();
    }

    private bool IsCurrentPlaylistEmpty()
    {
        return ProgressManager.instance.progress.userDataPersistance.current_playlist.Equals("");
    }

    private bool HasMwsiveTokenExpired()
    {
        return ProgressManager.instance.progress.userDataPersistance.expires_at.CompareTo(DateTime.Now) < 0;
    }

    private void OpenView(ViewID _value)
    {
        NewScreenManager.instance.ChangeToMainView(_value, false);
        DebugLogManager.instance.DebugLog(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    private void EndProcess()
    {
        NewScreenManager.instance.GetCurrentView().EndSearch();

        if (SceneManager.GetActiveScene().name.Equals("LogInScene"))
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            if (previousAction != null)
                previousAction(null);
        }
    }
}