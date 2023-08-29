using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackViewModel : ViewModel
{
    public string[] seed_artists;
    public string[] seed_genres;
    public string[] seed_tracks;

    // Start is called before the first frame update
    public string trackID;
    public string genre;
    public string artistId;
    public TextMeshProUGUI displayName;
    //public TextMeshProUGUI spotifyID;
    //public TextMeshProUGUI albumName;
    public TextMeshProUGUI artistName;
    public Image trackPicture;
    public GameObject trackHolderPrefab;
    public Transform instanceParent;
    public int objectsToNotDestroyIndex;
    public string artista;
    public string stringUrl;

    private RecommendationsRoot recommendations;

    void Start()
    {
        
        GetTrack();
        GetRecommendations();
    }

    public void GetTrack()
    {
        if (!trackID.Equals(""))
            SpotifyConnectionManager.instance.GetTrack(trackID, Callback_GetTrack);
    }
    private void Callback_GetTrack(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        TrackRoot trackRoot = (TrackRoot)_value[1];
        displayName.text = trackRoot.name;
        trackID = trackRoot.external_urls.spotify;

        foreach(Artist artist in trackRoot.artists) {
            artistName.text += artist.name + ", ";
        }
        if (artistName.text.Length > 27)
        {
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {

                _text2 = _text2 + artistName.text[k];

            }
            _text2 = _text2 + "...";
            artistName.text = _text2;
        }

        seed_artists = new string[trackRoot.artists.Count];

        for(int i = 0; i < seed_artists.Length; i++)
        {
            seed_artists[i] = trackRoot.artists[i].id;
        }

     

        ImageManager.instance.GetImage(trackRoot.album.images[0].url, trackPicture, (RectTransform)this.transform);
    }

    public void GetRecommendations()
    {
        seed_tracks[0] = trackID;
        Debug.Log("aa");
        //seed_genres[0] = genre;
        //seed_artists[0] = artistId;
        if ((seed_artists.Length + /*seed_genres.Length +*/ seed_tracks.Length) > 5)
        {
            Debug.Log("The seeds should be no more than 5 from either artists, genres or tracks");
            return;
        }

        SpotifyConnectionManager.instance.GetRecommendations(seed_artists, /*seed_genres,*/ seed_tracks, Callback_GetRecommendations);
    }

    private void Callback_GetRecommendations(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;
        
        RecommendationsRoot recommendationsRoot = (RecommendationsRoot)_value[1];
        recommendations = recommendationsRoot;
        InstanceTrackObjects(recommendationsRoot.tracks);
    }

    private void InstanceTrackObjects(List<Track> _tracks)
    {

        foreach (Track track in _tracks)
        {
            TrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<TrackHolder>();
            artista = "";
            foreach(Artist artist in track.artists) { artista += artist.name + ", "; }

            if (artista.Length > 27)
            {
                string _text2 = "";
                for (int k = 0; k < 27; k++)
                {

                    _text2 = _text2 + artista[k];

                }
                _text2 = _text2 + "...";
                artista = _text2;
            }

            instance.Initialize(track.name, artista, track.id, track.artists[0].id, track.uri, track.preview_url, track.external_urls);

            if (track.album.images != null && track.album.images.Count > 0)
                instance.SetImage(track.album.images[0].url);
        }
    }

    public void OnClickListenInSpotify()
    {               
        Application.OpenURL(trackID);
    }

    public void OnClick_ButtonBack()
    {
        NewScreenManager.instance.BackToPreviousView();
        SpotifyPreviewAudioManager.instance.StopTrack();
    }

    public void OnClick_Surf()
    {
        

        if(recommendations.tracks.Count == 0){
            UIMessage.instance.UIMessageInstanciate("No hay recomendaciones");
        }else{

            NewScreenManager.instance.ChangeToSpawnedView("surf");
            NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().DynamicPrefabSpawnerSong(new object[] { recommendations });
        }
        
    }
    
}
