using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Descubrir_ViewModel : ViewModel
{ 
    public DescubrirPaginas Descubrir;
    public List<GameObject> Prefabs = new List<GameObject>();    
    public List<GameObject> LastPosition = new List<GameObject>();   
    public GameObject PrefabsPosition,SpawnArea;
    public TMP_InputField Searchbar;
    public List<ScrollRect> Scrollbar = new List<ScrollRect>(); 
    public List<List<GameObject>> ListOfLists = new List<List<GameObject>>();
    [Header("Challenges Gameobject References")]
    public GameObject challengePrefab;
    public Transform challengeScrollContent;
    [Header("Advertasing Gameobject References")]
    public GameObject advertasingPrefab;
    public Transform advertasingScrollContent;
    [Header("Curators Gameobject References")]
    public GameObject curatorPrefab;
    public Transform curatorScrollContent;
    [Header("Artists Gameobject References")]
    public GameObject artistPrefab;
    public Transform artistScrollContent;
    [Header("Playlists Gameobject References")]
    public GameObject playlistPrefab;
    public Transform playlistScrollContent;
    [Header("Tracks Gameobject References")]
    public GameObject trackPrefab;
    public Transform trackScrollContent;
    [Header("Albums Gameobject References")]
    public GameObject albumPrefab;
    public Transform albumScrollContent;
    [Header("Genres Gameobject References")]
    public GameObject genrePrefab;
    public Transform genreScrollContent;


    private string SearchText;
    private GameObject Instance;
    private int numEnpantalla, PositionInSearch;
    private bool EnableSerach;
    private int MaxPrefabsinScreen = 0;
    private float ScrollbarVerticalPos =-0.001f;
    private bool CheckForSpawnHasEnded = true;
    private string[] types = new string[] {"album", "artist", "playlist", "track"};

    void Start()
    {
        foreach (GameObject item in Prefabs)
        {
            ListOfLists.Add( new List<GameObject>());
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return)){
            EnableSerach = true;
            Search();
        }else{
            EnableSerach = false;
        }
    }

    public override void Initialize(params object[] list)
    {
        StartSearch();
        //MwsiveConnectionManager.instance.GetChallenges(Callback_GetChallenges);
        MwsiveConnectionManager.instance.GetRecommendedCurators(Callback_GetRecommendedCurators);
    }

    private void Callback_GetChallenges(object[] _list) {
        MwsiveConnectionManager.instance.GetAdvertising(Callback_GetAdvertasing);
    }

    private void Callback_GetAdvertasing(object[] _list)
    {
        MwsiveConnectionManager.instance.GetRecommendedCurators(Callback_GetRecommendedCurators);
    }

    private void Callback_GetRecommendedCurators(object[] _list)
    {

        MwsiveRecommendedCuratorsRoot mwsiveRecommendedCuratorsRoot = (MwsiveRecommendedCuratorsRoot)_list[1];

        foreach(Curator curator in mwsiveRecommendedCuratorsRoot.curators){
            GameObject curatorInstance = GameObject.Instantiate(curatorPrefab, curatorScrollContent);
            curatorInstance.GetComponent<CuratorAppObject>().Initialize(curator.mwsive_user.platform_id);
        }

        MwsiveConnectionManager.instance.GetRecommendedArtists(Callback_GetRecommendedArtists);
    }

    private void Callback_GetRecommendedArtists(object[] _list)
    {
        MwsiveRecommendedArtistsRoot mwsiveRecommendedArtistsRoot = (MwsiveRecommendedArtistsRoot)_list[1];

        string[] artists_ids = new string[mwsiveRecommendedArtistsRoot.artists.Count];

        for (int i = 0; i < mwsiveRecommendedArtistsRoot.artists.Count; i++)
        {
            artists_ids[i] = mwsiveRecommendedArtistsRoot.artists[i].spotify_id;
        }

        SpotifyConnectionManager.instance.GetSeveralArtists(artists_ids, Callback_GetSeveralArtists);
    }

    private void Callback_GetSeveralArtists(object[] _list)
    {

        SeveralArtistRoot severalArtistRoot = (SeveralArtistRoot)_list[1];

        foreach (Artist artist in severalArtistRoot.artists)
        {
            GameObject artistInstance = GameObject.Instantiate(curatorPrefab, curatorScrollContent);
            artistInstance.GetComponent<ArtistAppObject>().Initialize(artist);
        }

        //MwsiveConnectionManager.instance.GetRecommendedPlaylists(Callback_GetRecommendedPlaylists);
        MwsiveConnectionManager.instance.GetRecommendedTracks(Callback_GetRecommendedTracks);
    }

    private void Callback_GetRecommendedPlaylists(object[] _list)
    {
        MwsiveConnectionManager.instance.GetRecommendedTracks(Callback_GetRecommendedTracks);
    }

    private void Callback_GetRecommendedTracks(object[] _list)
    {
        MwsiveRecommendedTracksRoot mwsiveRecommendedTracksRoot = (MwsiveRecommendedTracksRoot)_list[1];

        string[] tracks_ids = new string[mwsiveRecommendedTracksRoot.tracks.Count];

        for (int i = 0; i < mwsiveRecommendedTracksRoot.tracks.Count; i++)
        {
            //TODO AVISAR A EDITH QUE LOS ID DE SPOTIFY DE TRACKS SON STRINGS
            tracks_ids[i] = mwsiveRecommendedTracksRoot.tracks[i].id;
        }

        SpotifyConnectionManager.instance.GetSeveralTracks(tracks_ids, Callback_GetSeveralTracks);
    }

    private void Callback_GetSeveralTracks(object[] _list)
    {

        SeveralTrackRoot severalTrackRoot = (SeveralTrackRoot)_list[1];

        foreach (Track track in severalTrackRoot.tracks)
        {
            GameObject trackInstance = GameObject.Instantiate(curatorPrefab, curatorScrollContent);
            trackInstance.GetComponent<TrackAppObject>().Initialize(track);
        }

        MwsiveConnectionManager.instance.GetRecommendedAlbums(Callback_GetRecommendedAlbums);
    }

    private void Callback_GetRecommendedAlbums(object[] _list)
    {

        MwsiveRecommendedAlbumsRoot mwsiveRecommendedAlbumsRoot = (MwsiveRecommendedAlbumsRoot)_list[1];

        string[] albums_ids = new string[mwsiveRecommendedAlbumsRoot.albums.Count];

        for (int i = 0; i < mwsiveRecommendedAlbumsRoot.albums.Count; i++)
        {
            //TODO AVISAR A EDITH QUE LOS ID DE SPOTIFY DE ALBUMS SON STRINGS
            albums_ids[i] = mwsiveRecommendedAlbumsRoot.albums[i].id;
        }

        SpotifyConnectionManager.instance.GetSeveralAlbums(albums_ids, Callback_GetSeveralAlbums);
    }

    private void Callback_GetSeveralAlbums(object[] _list)
    {

        SeveralAlbumRoot severalAlbumRoot = (SeveralAlbumRoot)_list[1];

        foreach (Album album in severalAlbumRoot.albums)
        {
            GameObject trackInstance = GameObject.Instantiate(curatorPrefab, curatorScrollContent);
            trackInstance.GetComponent<AlbumAppObject>().Initialize(album);
        }

        //MwsiveConnectionManager.instance.GetGenres(Callback_GetGenres);

        //TODO DELETE AFTER GENRES IS DONE
        EndSearch();
    }

        private void Callback_GetGenres(object[] _list)
    {
        EndSearch();
    }

    public void Search(){
        int lastnum= numEnpantalla;
        numEnpantalla = Descubrir.GetCurrentEscena();
        SearchText = Searchbar.text;

        if(SearchText.Length >= 3 || EnableSerach){

            if(numEnpantalla != lastnum){
                KillPrefablist(lastnum);
            }

            if(ListOfLists[numEnpantalla].Count == 0){
                CalculateMaxPrefabToCall();
            }

            if(ListOfLists[numEnpantalla].Count != 0){
                KillPrefablist(numEnpantalla);
            }

            SpotifySearch();
        } 
    }


    private void SpotifySearch(){

        switch (numEnpantalla)
        {
            case 0:
                types = new string[] { "album", "artist", "playlist", "track" };
                
                SpotifyConnectionManager.instance.SearchForItem(SearchText, types, Callback_OnCLick_SearchForItem, "ES", Mathf.RoundToInt(MaxPrefabsinScreen/3));
                break;
            case 1:
                
                //MWsive DataBase
                break;
            case 2:
                types = new string[] { "track" };
               SpotifyConnectionManager.instance.SearchForItem(SearchText, types, Callback_OnCLick_SearchForItem, "ES", MaxPrefabsinScreen);
               DynamicPrefabSpawner(MaxPrefabsinScreen);
                break;
            case 3:
                types = new string[] { "artist" };
               SpotifyConnectionManager.instance.SearchForItem(SearchText, types, Callback_OnCLick_SearchForItem, "ES", MaxPrefabsinScreen);
               DynamicPrefabSpawner(MaxPrefabsinScreen);
                break;
            case 4:
                types = new string[] { "album" };
               SpotifyConnectionManager.instance.SearchForItem(SearchText, types, Callback_OnCLick_SearchForItem, "ES", MaxPrefabsinScreen);
               DynamicPrefabSpawner(MaxPrefabsinScreen);
                break;
            case 5:
                types = new string[] { "playlist" };
                SpotifyConnectionManager.instance.SearchForItem(SearchText, types, Callback_OnCLick_SearchForItem, "ES", MaxPrefabsinScreen);
                DynamicPrefabSpawner(MaxPrefabsinScreen);
                break;
            case 6:
                types = new string[] { "album", "artist", "playlist", "track" };
                string GenreText =SearchText +"%20genre:" + SearchText;
                SpotifyConnectionManager.instance.SearchForItem(GenreText, types, Callback_OnCLick_SearchForItem, "ES", Mathf.RoundToInt(MaxPrefabsinScreen/3));
                break;
        }

        
        PositionInSearch = MaxPrefabsinScreen;
        //ScrollBar.verticalNormalizedPosition = 1;
    }

    private void Callback_OnCLick_SearchForItem(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SearchRoot searchRoot = (SearchRoot)_value[1];
        
        switch (numEnpantalla)
        {
            case 0:
                KillPrefablist(0);
                if (searchRoot.tracks != null){
                    
                    foreach (var item in searchRoot.tracks.items)
                    {
                        try
                        {
                            
                            if (item.album.images != null){
                                CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(item.name, item.artists[0].name, item.album.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                        }
                    }
                }

                if (searchRoot.artists != null){
                
                    foreach (var item in searchRoot.artists.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(item.name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                        }
                    }
                }

                if (searchRoot.albums != null){
                 
                    foreach (var item in searchRoot.albums.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(item.name, item.artists[0].name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                    } 
                }

                if (searchRoot.playlists != null){
                
                    foreach (var item in searchRoot.playlists.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(item.name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                        }
                    }
                }   
            }

                LastPosition[numEnpantalla].transform.SetAsLastSibling();
            
                break;

            case 2:
                if (searchRoot.tracks != null){
                    for (int i = 0; i < searchRoot.tracks.items.Count; i++)
                    {
                        try
                        {
                            if (searchRoot.tracks.items[i].album.images != null){
                                ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(searchRoot.tracks.items[i].name, searchRoot.tracks.items[i].artists[0].name, searchRoot.tracks.items[i].album.images[0].url, searchRoot.tracks.items[i].id, searchRoot.tracks.items[i].external_urls.spotify);
                            }else{
                                ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(searchRoot.tracks.items[i].name, searchRoot.tracks.items[i].artists[0].name, searchRoot.tracks.items[i].id, searchRoot.tracks.items[i].external_urls.spotify);
                                }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(searchRoot.tracks.items[i].name, searchRoot.tracks.items[i].artists[0].name, searchRoot.tracks.items[i].id, searchRoot.tracks.items[i].external_urls.spotify);
                        
                        }                           
                    }
                }
                break;

            case 3:
                if (searchRoot.artists != null){
                    for (int i = 0; i < searchRoot.artists.items.Count; i++)
                    {
                        try
                        {
                            if (searchRoot.artists.items[i].images != null){
                            ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(searchRoot.artists.items[i].name, searchRoot.artists.items[i].images[0].url, searchRoot.artists.items[i].id, searchRoot.artists.items[i].external_urls.spotify);
                            }else{
                                ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(searchRoot.artists.items[i].name, searchRoot.artists.items[i].id, searchRoot.artists.items[i].external_urls.spotify);
                                }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(searchRoot.artists.items[i].name, searchRoot.artists.items[i].id, searchRoot.artists.items[i].external_urls.spotify);
                        
                        }                           
                    }
                }
                break;
              

            case 4:
                if (searchRoot.albums != null){
                    for (int i = 0; i < searchRoot.albums.items.Count; i++)
                    {
                        try
                        {
                            if (searchRoot.albums.items[i].images != null){
                            ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(searchRoot.albums.items[i].name, searchRoot.albums.items[i].artists[0].name, searchRoot.albums.items[i].images[0].url, searchRoot.albums.items[i].id, searchRoot.albums.items[i].external_urls.spotify);
                            }else{
                                ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(searchRoot.albums.items[i].name, searchRoot.albums.items[i].artists[0].name, searchRoot.albums.items[i].id, searchRoot.albums.items[i].external_urls.spotify);
                                }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(searchRoot.albums.items[i].name, searchRoot.albums.items[i].artists[0].name, searchRoot.albums.items[i].id, searchRoot.albums.items[i].external_urls.spotify);
                        }                        
                    }
                }
                break; 


            case 5:
                if (searchRoot.playlists != null){
                    for (int i = 0; i < searchRoot.playlists.items.Count; i++)
                    {
                        try
                        {
                            if (searchRoot.playlists.items[i].images != null){
                            ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(searchRoot.playlists.items[i].name, searchRoot.playlists.items[i].images[0].url, searchRoot.playlists.items[i].id, searchRoot.playlists.items[i].external_urls.spotify);
                            }else{
                                ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(searchRoot.playlists.items[i].name, searchRoot.playlists.items[i].id, searchRoot.playlists.items[i].external_urls.spotify);
                                }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            ListOfLists[numEnpantalla][i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(searchRoot.playlists.items[i].name, searchRoot.playlists.items[i].id, searchRoot.playlists.items[i].external_urls.spotify);
                        
                        }                    
                    }
                }
                break;     


            case 6:
                KillPrefablist(6);

                if (searchRoot.tracks != null){
                    
                    foreach (var item in searchRoot.tracks.items)
                    {
                        try
                        {
                            if (item.album.images != null){
                                CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(item.name, item.artists[0].name, item.album.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                        }
                    }
                }

                if (searchRoot.artists != null){
                    foreach (var item in searchRoot.artists.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(item.name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                        }
                    } 
                }

                if (searchRoot.albums != null){
                    foreach (var item in searchRoot.albums.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(item.name, item.artists[0].name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                    }
                }
                    
                if (searchRoot.playlists != null){
                
                    foreach (var item in searchRoot.playlists.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(item.name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                        }
                    }
                }   
            }

                LastPosition[numEnpantalla].transform.SetAsLastSibling();
            
                break;
        }

    }

    private void Callback_OnCLick_CheckForSpawn(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SearchRoot searchRoot = (SearchRoot)_value[1];
        switch (numEnpantalla)
        {
            case 0:
                
                if (searchRoot.tracks != null){
                    
                    foreach (var item in searchRoot.tracks.items)
                    {
                        
                        try
                        {
                            if (item.album.images != null){
                                
                                CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(item.name, item.artists[0].name, item.album.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                
                                CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            
                            CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                        }
                    }
                    
                    
                }
                if (searchRoot.artists != null){
                
                    foreach (var item in searchRoot.artists.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(item.name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                        }
                    }
                    
                }
                if (searchRoot.albums != null){
                 
                    foreach (var item in searchRoot.albums.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(item.name, item.artists[0].name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                        }
                    
                    }    
                }        
                if (searchRoot.playlists != null){
                
                    foreach (var item in searchRoot.playlists.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(item.name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                        }
                    }
                }   
            

            
            
            LastPosition[numEnpantalla].transform.SetAsLastSibling();
            
            break;



            case 2:
            if (searchRoot.tracks != null){
                for (int i = 0; i < MaxPrefabsinScreen; i++)
                {
                    
                    try
                    {
                        if (searchRoot.tracks.items[i].album.images != null){
                            ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(searchRoot.tracks.items[i].name, searchRoot.tracks.items[i].artists[0].name, searchRoot.tracks.items[i].album.images[0].url, searchRoot.tracks.items[i].id, searchRoot.tracks.items[i].external_urls.spotify);
                        }else{
                            ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(searchRoot.tracks.items[i].name, searchRoot.tracks.items[i].artists[0].name, searchRoot.tracks.items[i].id, searchRoot.tracks.items[i].external_urls.spotify);
                            }
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(searchRoot.tracks.items[i].name, searchRoot.tracks.items[i].artists[0].name, searchRoot.tracks.items[i].id, searchRoot.tracks.items[i].external_urls.spotify);
                        
                    }
                            
                }
                
            }
            break;
            case 3:
            if (searchRoot.artists != null){
                for (int i = 0; i < MaxPrefabsinScreen; i++)
                {

                    try
                    {
                        if (searchRoot.artists.items[i].images != null){
                        ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(searchRoot.artists.items[i].name, searchRoot.artists.items[i].images[0].url, searchRoot.artists.items[i].id, searchRoot.artists.items[i].external_urls.spotify);
                        }else{
                            ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(searchRoot.artists.items[i].name, searchRoot.tracks.items[i].id, searchRoot.artists.items[i].external_urls.spotify);
                            }
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(searchRoot.artists.items[i].name, searchRoot.tracks.items[i].id, searchRoot.artists.items[i].external_urls.spotify);
                        
                    }
                
                }
                
            }   
                
            break;

            case 4:
            if (searchRoot.albums != null){
                for (int i = 0; i < MaxPrefabsinScreen; i++)
                {
                    try
                    {
                        if (searchRoot.albums.items[i].images != null){
                        ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(searchRoot.albums.items[i].name, searchRoot.albums.items[i].artists[0].name, searchRoot.albums.items[i].images[0].url, searchRoot.albums.items[i].id, searchRoot.albums.items[i].external_urls.spotify);
                        }else{
                            ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(searchRoot.albums.items[i].name, searchRoot.albums.items[i].artists[0].name, searchRoot.albums.items[i].id, searchRoot.albums.items[i].external_urls.spotify);
                            }
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(searchRoot.albums.items[i].name, searchRoot.albums.items[i].artists[0].name, searchRoot.albums.items[i].id, searchRoot.albums.items[i].external_urls.spotify);
                    }
                        
                }
                
            }
            break; 

            case 5:
            if (searchRoot.playlists != null){
                for (int i = 0; i < MaxPrefabsinScreen; i++)
                {
                    try
                    {
                        if (searchRoot.playlists.items[i].images != null){
                        ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(searchRoot.playlists.items[i].name, searchRoot.playlists.items[i].images[0].url, searchRoot.playlists.items[i].id, searchRoot.playlists.items[i].external_urls.spotify);
                        }else{
                            ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(searchRoot.playlists.items[i].name, searchRoot.playlists.items[i].id, searchRoot.playlists.items[i].external_urls.spotify);
                            }
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        ListOfLists[numEnpantalla][PositionInSearch+i].GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(searchRoot.playlists.items[i].name, searchRoot.playlists.items[i].id, searchRoot.playlists.items[i].external_urls.spotify);
                        
                    }
                            
                }
                
            }
            break; 


            case 6:
                
                if (searchRoot.tracks != null){
                    
                    foreach (var item in searchRoot.tracks.items)
                    {
                        
                        try
                        {
                            if (item.album.images != null){
                                
                                CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(item.name, item.artists[0].name, item.album.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                
                                CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            
                            CustomSpawnPrefab(false, 2).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                        }
                    }
                    
                    
                }
                if (searchRoot.artists != null){
                
                    foreach (var item in searchRoot.artists.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(item.name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 3).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                        }
                    }
                    
                }
                if (searchRoot.albums != null){
                 
                    foreach (var item in searchRoot.albums.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDoubleWithImage(item.name, item.artists[0].name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 4).GetComponent<DynamicSearchPrefabInitializer>().InitializeDouble(item.name, item.artists[0].name, item.id, item.external_urls.spotify);
                        }
                    
                    }    
                }
                
                if (searchRoot.playlists != null){
                
                    foreach (var item in searchRoot.playlists.items)
                    {
                        try
                        {
                            if (item.images != null){
                                CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingleWithImage(item.name, item.images[0].url, item.id, item.external_urls.spotify);
                            }else{
                                CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                            }
                        }
                        catch (System.ArgumentOutOfRangeException)
                        {
                            CustomSpawnPrefab(false, 5).GetComponent<DynamicSearchPrefabInitializer>().InitializeSingle(item.name, item.id, item.external_urls.spotify);
                        }
                    }
                }   
            
                LastPosition[numEnpantalla].transform.SetAsLastSibling();
            
                break;
        }
        Debug.Log("EndCheck");
        CheckForSpawnHasEnded = true;
        PositionInSearch = PositionInSearch + MaxPrefabsinScreen;
    }

    
    public void CheckForSpawn(){
        
        if(ListOfLists[numEnpantalla].Count != 0 && CheckForSpawnHasEnded){
            
            if(Scrollbar[numEnpantalla].verticalNormalizedPosition  <= ScrollbarVerticalPos){
                
                CheckForSpawnHasEnded = false;
                DynamicPrefabSpawner(MaxPrefabsinScreen);
                if(numEnpantalla == 6 || numEnpantalla == 0){
                    if(numEnpantalla == 6){
                        string GenreText =SearchText +"%20genre:" + SearchText;
                        SpotifyConnectionManager.instance.SearchForItem(GenreText, types, Callback_OnCLick_CheckForSpawn, "ES", Mathf.RoundToInt(MaxPrefabsinScreen/3), PositionInSearch);
                    }else{
                        SpotifyConnectionManager.instance.SearchForItem(SearchText, types, Callback_OnCLick_CheckForSpawn, "ES", Mathf.RoundToInt(MaxPrefabsinScreen/3), PositionInSearch);
                    }
                    
                }else{
                    SpotifyConnectionManager.instance.SearchForItem(SearchText, types, Callback_OnCLick_CheckForSpawn, "ES", MaxPrefabsinScreen, PositionInSearch);
                }
                
            }
        }
        
    }
    
    public void DynamicPrefabSpawner(int prefabs){
        numEnpantalla = Descubrir.GetCurrentEscena();
        if(ListOfLists[numEnpantalla].Count == 0 ){
            CalculateMaxPrefabToCall();
        }

        if (numEnpantalla == 0 || numEnpantalla == 6){
            //SpawnAll( prefabs);
        }else{
            for (int i = 0; i <= prefabs-1; i++)
            {
                SpawnPrefab(false);
            }
        }
            
        LastPosition[numEnpantalla].transform.SetAsLastSibling();
        Debug.Log("Done Spawn");
        
    }

    private void CalculateMaxPrefabToCall(){
        if(MaxPrefabsinScreen ==0){
                  
            MaxPrefabsinScreen = (int)Mathf.Round((SpawnArea.GetComponent<RectTransform>().rect.height) / Prefabs[numEnpantalla].GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    public void KillAllPrefabLists(){
        Searchbar.text = null;
        foreach (List<GameObject> ListPrefab in ListOfLists)
        {
            foreach(GameObject Prefab in ListPrefab){
                Destroy(Prefab);
            }
            ListPrefab.Clear(); 
        }
    }

    public void KillPrefablist(int scene){
        foreach (GameObject Prefab in ListOfLists[scene])
        {
            Destroy(Prefab);
        }
        ListOfLists[scene].Clear();
    }

    private GameObject CustomSpawnPrefab(bool IsVisible, int scene){
        switch (numEnpantalla){
            case 0:
                switch (scene){
                case 2:
                    Instance = Instantiate(Prefabs[2],PrefabsPosition.transform.position, Quaternion.identity);
                    Instance.transform.SetParent(GameObject.Find("PF_ResultadosdeBusqueda_Container").transform);
                    Instance.transform.localScale = new Vector3(1,1,1);  
                    Instance.SetActive(IsVisible);
                    ListOfLists[numEnpantalla].Add(Instance);
                    return Instance;
                    
                case 3:
                    Instance = Instantiate(Prefabs[3],PrefabsPosition.transform.position, Quaternion.identity);
                    Instance.transform.SetParent(GameObject.Find("PF_ResultadosdeBusqueda_Container").transform);
                    Instance.transform.localScale = new Vector3(1,1,1);
                    Instance.SetActive(IsVisible);
                    ListOfLists[numEnpantalla].Add(Instance);   
                    return Instance;
                case 4: 
                    Instance = Instantiate(Prefabs[4],PrefabsPosition.transform.position, Quaternion.identity);
                    Instance.transform.SetParent(GameObject.Find("PF_ResultadosdeBusqueda_Container").transform);
                    Instance.transform.localScale = new Vector3(1,1,1);  
                    Instance.SetActive(IsVisible); 
                    ListOfLists[numEnpantalla].Add(Instance);
                    return Instance;
                case 5: 
                    Instance = Instantiate(Prefabs[5],PrefabsPosition.transform.position, Quaternion.identity);
                    Instance.transform.SetParent(GameObject.Find("PF_ResultadosdeBusqueda_Container").transform);
                    Instance.transform.localScale = new Vector3(1,1,1);
                    Instance.SetActive(IsVisible);
                    ListOfLists[numEnpantalla].Add(Instance);  
                    return Instance;
            }
            return null;
            case 6:
                switch (scene){
                case 2:
                    Instance = Instantiate(Prefabs[2],PrefabsPosition.transform.position, Quaternion.identity);
                    Instance.transform.SetParent(GameObject.Find("PF_Genders_Container").transform);
                    Instance.transform.localScale = new Vector3(1,1,1);  
                    Instance.SetActive(IsVisible);
                    ListOfLists[numEnpantalla].Add(Instance);
                    return Instance;
                    
                case 3:
                    Instance = Instantiate(Prefabs[3],PrefabsPosition.transform.position, Quaternion.identity);
                    Instance.transform.SetParent(GameObject.Find("PF_Genders_Container").transform);
                    Instance.transform.localScale = new Vector3(1,1,1);
                    Instance.SetActive(IsVisible);
                    ListOfLists[numEnpantalla].Add(Instance);   
                    return Instance;
                case 4: 
                    Instance = Instantiate(Prefabs[4],PrefabsPosition.transform.position, Quaternion.identity);
                    Instance.transform.SetParent(GameObject.Find("PF_Genders_Container").transform);
                    Instance.transform.localScale = new Vector3(1,1,1);  
                    Instance.SetActive(IsVisible); 
                    ListOfLists[numEnpantalla].Add(Instance);
                    return Instance;
                case 5: 
                    Instance = Instantiate(Prefabs[5],PrefabsPosition.transform.position, Quaternion.identity);
                    Instance.transform.SetParent(GameObject.Find("PF_Genders_Container").transform);
                    Instance.transform.localScale = new Vector3(1,1,1);
                    Instance.SetActive(IsVisible);
                    ListOfLists[numEnpantalla].Add(Instance);  
                    return Instance;
            }
            return null;
            
        }
        return null;
    }
    private void SpawnPrefab(bool IsVisible){
        switch (numEnpantalla){
            case 0:
                break;
            case 1:
                Instance = Instantiate(Prefabs[numEnpantalla],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_Curadores_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);  
                Instance.SetActive(IsVisible);
                ListOfLists[numEnpantalla].Add(Instance);
                break;
            case 2:
                Instance = Instantiate(Prefabs[numEnpantalla],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_Songs_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);
                Instance.SetActive(IsVisible);
                ListOfLists[numEnpantalla].Add(Instance);   
                break;
            case 3: 
                Instance = Instantiate(Prefabs[numEnpantalla],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_Artists_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);  
                Instance.SetActive(IsVisible); 
                ListOfLists[numEnpantalla].Add(Instance);
                break;
            case 4: 
                Instance = Instantiate(Prefabs[numEnpantalla],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_Albums_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);
                Instance.SetActive(IsVisible);
                ListOfLists[numEnpantalla].Add(Instance);  
                break;
            case 5: 
                Instance = Instantiate(Prefabs[numEnpantalla],PrefabsPosition.transform.position, Quaternion.identity);
                Instance.transform.SetParent(GameObject.Find("PF_Playlists_Container").transform);
                Instance.transform.localScale = new Vector3(1,1,1);
                Instance.SetActive(IsVisible);
                ListOfLists[numEnpantalla].Add(Instance);   
                break;

        }

    }
}
