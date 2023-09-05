using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiplaylistHolder : MonoBehaviour
{

    public TextMeshProUGUI playlistName;
    public Image playlistPicture;
    public TextMeshProUGUI playlistOwner;
    private string spotifyID;
    public MiPlaylistViewModel miPlaylistViewModel;
    public ChangeImage change;
    public HolderManager holderManager;
    public bool @public;
    public ExternalUrls url;

    public void Initialize(string _playlistName, string _spotifyID, string _owner, bool _public, ExternalUrls _url)
    {

        if (_playlistName != null)
        {
            if (_playlistName.Length > 27)
            {
                string _text2 = "";
                for (int i = 0; i < 27; i++)
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
        url = _url;
    }

    public void Initialize(string _playlistName, string _spotifyID, string _owner, bool _public, ExternalUrls _url, string _pictureURL)
    {
        if (_playlistName != null)
        {
            if (_playlistName.Length > 27)
            {
                string _text2 = "";
                for (int i = 0; i < 27; i++)
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
        url = _url;
        ImageManager.instance.GetImage(_pictureURL, playlistPicture, (RectTransform)this.transform);
    }

    public void PublicTrue()
    {
        change.True();
    }
    public void SetImage(string _pictureURL)
    {
        ImageManager.instance.GetImage(_pictureURL, playlistPicture, (RectTransform)this.transform);
    }


    public void OnClick_SpawnPlaylistButton()
    {

        NewScreenManager.instance.ChangeToSpawnedView("playlist");
        PlaylistViewModel playlistViewModel = NewScreenManager.instance.GetCurrentView().GetComponent<PlaylistViewModel>();
        playlistViewModel.playlistName.text = playlistName.text;
        playlistViewModel.SetPlaylistID(spotifyID);
        playlistViewModel.@public = @public;
        holderManager.playlistExternalUrl = url;
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }


}
