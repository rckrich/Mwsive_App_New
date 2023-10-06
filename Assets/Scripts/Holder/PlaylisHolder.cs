using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaylisHolder : MonoBehaviour
{
    private const int MAX_PLAYLIST_NAME_LENGHT = 18;
    private const int MAX_PLAYLIST_ARTIST_LENGHT = 27;

    public TextMeshProUGUI playlistName;
    public Image playlistPicture;
    public TextMeshProUGUI playlistOwner;
    private string spotifyID;
    public bool @public;

    public void Initialize(string _playlistName, string _spotifyID, string _owner, bool _public)
    {
        if (_playlistName != null)
        {
            if (_playlistName.Length > MAX_PLAYLIST_NAME_LENGHT)
            {
                string _text2 = "";
                for (int i = 0; i < MAX_PLAYLIST_NAME_LENGHT; i++)
                {
                    _text2 = _text2 + _playlistName[i];
                }
                _text2 = _text2 + "...";
                playlistName.text = _text2;
            }
            else
            {
                playlistName.text = _playlistName;
            }
        }

        spotifyID = _spotifyID;
        playlistOwner.text = _owner;
        @public = _public;
    }

    public void Initialize(string _playlistName, string _spotifyID, string _owner, bool _public, string _pictureURL)
    {
        if (_playlistName != null)
        {
            if (_playlistName.Length > MAX_PLAYLIST_ARTIST_LENGHT)
            {
                string _text2 = "";
                for (int i = 0; i < MAX_PLAYLIST_ARTIST_LENGHT; i++)
                {
                    _text2 = _text2 + _playlistName[i];
                }
                _text2 = _text2 + "...";
                playlistName.text = _text2;
            }
            else
            {
                playlistName.text = _playlistName;
            }
        }

        spotifyID = _spotifyID;
        playlistOwner.text = _owner;
        @public = _public;
        ImageManager.instance.GetImage(_pictureURL, playlistPicture, (RectTransform)this.transform);
    }

    public void SetImage(string _pictureURL)
    {
        ImageManager.instance.GetImage(_pictureURL, playlistPicture, (RectTransform)this.transform);
    }

    public void OnClick_CopyToClipboard()
    {
        if (!spotifyID.Equals(""))
            GUIUtility.systemCopyBuffer = spotifyID;
    }

    public void OnClick_SpawnPlaylistButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("playlist");
        PlaylistViewModel playlistViewModel = NewScreenManager.instance.GetCurrentView().GetComponent<PlaylistViewModel>();
        playlistViewModel.playlistName.text = playlistName.text;
        playlistViewModel.SetPlaylistID(spotifyID);
        playlistViewModel.@public = @public;
        playlistViewModel.GetPlaylist();
        NewScreenManager.instance.GetCurrentView().GetComponent<PlaylistViewModel>().SetAndroidBackAction();
    }
}
