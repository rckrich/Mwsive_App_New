using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackHolder : ViewModel
{
    public TextMeshProUGUI trackName;
    public TextMeshProUGUI artistName;
    public Image trackPicture;
    public TrackViewModel trackViewModel;
    public string uri;
    public AddSongOptions songOptions;
    private string trackSpotifyID;
    private string genre;
    private string artistId;
    private string previewURL;
    private string mp3URL;
    public AddSongOptions addSongOptions;
    public GameObject playSimbol;
    public ExternalUrls url;

    public void Initialize(string _trackName, string _artistName, string _trackSpotifyID, string _artistId, string _uri, string _previewURL, ExternalUrls _url)
    {
        if (_trackName.Length > 27)
        {
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {

                _text2 = _text2 + _trackName[k];

            }
            _text2 = _text2 + "...";
            trackName.text = _text2;
        }
        else
        {
            trackName.text = _trackName;
        }
        trackName.text = _trackName;
        artistName.text = _artistName;
        trackSpotifyID = _trackSpotifyID;
        artistId = _artistId;
        uri = _uri;      
        previewURL = _previewURL;
        url = _url;
    }

    public void Initialize(string _trackName, string _artistName,  string _artistId, string _uri, string _previewURL, string _trackSpotifyID, ExternalUrls _url,string _pictureURL)
    {
        if (_trackName.Length > 27)
        {
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {

                _text2 = _text2 + _trackName[k];

            }
            _text2 = _text2 + "...";
            trackName.text = _text2;
        }
        else
        {
            trackName.text = _trackName;
        }
        artistName.text = _artistName;
        trackSpotifyID = _trackSpotifyID;
        artistId= _artistId;
        uri= _uri;       
        previewURL = _previewURL;
        url= _url;
        ImageManager.instance.GetImage(_pictureURL, trackPicture, (RectTransform)this.transform);
    }

    public void SetImage(string _pictureURL)
    {
        ImageManager.instance.GetImage(_pictureURL, trackPicture, (RectTransform)this.transform);
    }

    public void OnClick_PlayAudioPreview()
    {
        mp3URL = previewURL;
        if (!mp3URL.Equals(""))
        {
            SpotifyPreviewAudioManager.instance.GetTrack(mp3URL);
            Playing();
        }
    }

    public void Playing()
    {
        playSimbol.SetActive(true);
    }
    
    public void OnClickSongOptions()
    {
        addSongOptions.trackID = trackSpotifyID;
        AppManager.instance.GetTrack(trackSpotifyID);
        StartSearch();
     
    }
}
