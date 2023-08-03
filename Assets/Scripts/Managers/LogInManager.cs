using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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

    public void StartLogInProcess()
    {
        SpotifyConnectionManager.instance.StartConnection(Callback_StartSpotifyConnection);
    }

    private void Callback_StartSpotifyConnection(object[] _value)
    {
        MwsiveTokenLogInProcess();
    }

    public void MwsiveTokenLogInProcess()
    {
        if (ProgressManager.instance.progress.userDataPersistance.userTokenSetted)
        {
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
                    OpenView(ViewID.SurfViewModel);
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
        for (int i = 0; i <= playlistRoot.items.Count; i++)
        {
            itemIDs[i] = playlistRoot.items[i].id;
        }

        SetCurrentPlaylist(itemIDs[0]);

        OpenView(ViewID.SurfViewModel);
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
        string webcode = (string)_value[0];
        if (webcode == "204")
        {
            MwsiveLoginRoot accessToken = (MwsiveLoginRoot)_value[1];

            SetMwsiveToken(accessToken.mwsive_token);

            if (IsCurrentPlaylistEmpty())
            {
                SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_GetCurrentUserPlaylists);
            }
            else
            {
                OpenView(ViewID.SurfViewModel);
            }
        }
        else if (webcode == "404")
        {
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
        for (int i = 0; i <= playlistRoot.items.Count; i++)
        {
            itemIDs[i] = playlistRoot.items[i].id;
        }

        MwsiveConnectionManager.instance.PostCreateUser(email, "null", 0, profile, itemIDs, Callback_PostCreateUser);
    }

    private void Callback_PostCreateUser(object[] _value)
    {
        MwsiveCreatenRoot mwsiveCreatenRoot = (MwsiveCreatenRoot)_value[1];

        SetMwsiveToken(mwsiveCreatenRoot.mwsive_token);

        SpotifyConnectionManager.instance.CreatePlaylist(profileid, Callback_CreatePlaylist);
    }

    private void Callback_CreatePlaylist(object[] _value)
    {
        CreatedPlaylistRoot createdPlaylistRoot = (CreatedPlaylistRoot)_value[1];
        playlistid = createdPlaylistRoot.id;

        SetCurrentPlaylist(playlistid);

        OpenView(ViewID.SurfViewModel);
    }

    private void SetCurrentPlaylist(string _value)
    {
        ProgressManager.instance.progress.userDataPersistance.current_playlist = _value;
        ProgressManager.instance.save();
    }

    private void SetMwsiveToken(string _value)
    {
        ProgressManager.instance.progress.userDataPersistance.access_token = _value;
        ProgressManager.instance.progress.userDataPersistance.userTokenSetted = true;
        ProgressManager.instance.save();
    }

    private bool IsCurrentPlaylistEmpty()
    {
        return ProgressManager.instance.progress.userDataPersistance.current_playlist.Equals("");
    }

    private bool HasMwsiveTokenExpired()
    {
        return ProgressManager.instance.progress.userDataPersistance.expires_at.CompareTo(DateTime.Now) > 0;
    }

    private void OpenView(ViewID _value)
    {
        NewScreenManager.instance.ChangeToMainView(_value, false);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
}