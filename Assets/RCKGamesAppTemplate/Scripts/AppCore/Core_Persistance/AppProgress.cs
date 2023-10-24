using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserDataPersistance
{
    [SerializeField] public bool userTokenSetted = false;
    [SerializeField] public bool spotify_userTokenSetted = false;
    [SerializeField] public string access_token = "";
    [SerializeField] public string token_type = "";
    [SerializeField] public DateTime expires_at = new DateTime(1990, 01, 01);
    [SerializeField] public DateTime spotify_expires_at = new DateTime(1990, 01, 01);
    [SerializeField] public string raw_value = "";
    [SerializeField] public string current_playlist = "";

    public UserDataPersistance(bool _userTokenSetted, bool _spotify_userTokenSetted, string _access_token, string _token_type, DateTime _expires_at, DateTime _spotify_expires_at, string _raw_value, string _current_playlist)
    {
        this.userTokenSetted = _userTokenSetted;
        this.spotify_userTokenSetted = _spotify_userTokenSetted;
        this.access_token = _access_token;
        this.token_type = _token_type;
        this.expires_at = _expires_at;
        this.spotify_expires_at = _spotify_expires_at;
        this.raw_value = _raw_value;
        this.current_playlist = _current_playlist;
    }
}

[System.Serializable]
public class AppProgress
{
    [SerializeField] public UserDataPersistance userDataPersistance = new UserDataPersistance(false, false, "", "", new DateTime(1990, 01, 01), new DateTime(1990, 01, 01), "", "");
}