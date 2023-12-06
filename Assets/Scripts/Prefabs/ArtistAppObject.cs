using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArtistAppObject : AppObject
{
    private const int IMAGES_NOT_NULL_INDEX = 0;

    public TextMeshProUGUI displayName;
    public Image artistPicture;

    private Artist artist;

    public override void Initialize(params object[] list)
    {
        artist = (Artist)list[0];
        if (artist.name.Length > 27)
        {
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {

                _text2 = _text2 + artist.name[k];

            }
            _text2 = _text2 + "...";
            displayName.text = _text2;
        }
        else
        {
            displayName.text = artist.name;
        }

        if (artist.images != null)
        {
            if(artist.images.Count > IMAGES_NOT_NULL_INDEX)
                ImageManager.instance.GetImage(artist.images[0].url, artistPicture, (RectTransform)this.transform);
        }
    }

    public void OnClick_ArtistAppObject()
    {
        /*if(artist != null && artist.external_urls != null && !artist.external_urls.spotify.Equals(""))
            Application.OpenURL(artist.external_urls.spotify);*/

        if (artist != null)
            SpotifyConnectionManager.instance.GetArtistTopTracks(artist.id, Callback_GetArtistTopTracks);
    }

    private void Callback_GetArtistTopTracks(object[] _value)
    {
        SeveralTrackRoot severalTrackRoot = (SeveralTrackRoot)_value[1];

        if (!ArtistsHasTopSongs(severalTrackRoot))
        {
            UIMessage.instance.UIMessageInstanciate("Este artista aún no tiene un top de canciones disponibles");
            return;
        }

        NewScreenManager.instance.ChangeToSpawnedView("surf");
        NewScreenManager.instance.GetCurrentView().GetComponent<PF_SurfViewModel>().Initialize();
        NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().DynamicPrefabSpawnerSeveralTracks(severalTrackRoot.tracks);
    }

    private bool ArtistsHasTopSongs(SeveralTrackRoot tracks)
    {
        if (tracks == null)
        {
            return false;
        }

        if (tracks.tracks == null)
        {
            return false;
        }

        if (tracks.tracks.Count <= 0)
        {
            return false;
        }

        return true;
    }
}
