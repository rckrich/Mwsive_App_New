using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderManager : MonoBehaviour
{
    public string playlistId;
    public string trackID;
    public List<string> uri;
    public ExternalUrls playlistExternalUrl;
    public ExternalUrls trackExternalUrl;
    public string playlistName;
    // Start is called before the first frame update
    void Start()
    {
         playlistId = ProgressManager.instance.progress.userDataPersistance.current_playlist;
         GetPlaylist();
    }
    public void GetPlaylist()
    {
        SpotifyConnectionManager.instance.GetPlaylist(playlistId, Callback_OnCLick_GetPlaylist);
    }
    private void Callback_OnCLick_GetPlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;
        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        playlistName = searchedPlaylist.name;
        Debug.Log(playlistName);

    }
}
