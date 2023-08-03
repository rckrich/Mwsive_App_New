using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpotifySearchForItemViewModel : MonoBehaviour
{
    private string[] types = new string[] { "album", "artist", "playlist", "track", "show", "episode", "audiobook" };

    public TMP_InputField queryInputField;

    [Header("Instance References")]
    public GameObject trackHolderPrefab;
    public Transform instanceParent;
    public int objectsToNotDestroyIndex;

    public void OnClick_SearchForItem()
    {
        if (!queryInputField.text.Equals(""))
            SpotifyConnectionManager.instance.SearchForItem(queryInputField.text, types, Callback_OnCLick_SearchForItem);
    }

    private void Callback_OnCLick_SearchForItem(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SearchRoot searchRoot = (SearchRoot)_value[1];

        ResetScrollView();

        if (searchRoot.albums != null)
            InstanceItemObjects(searchRoot.albums.items);

        if (searchRoot.artists != null)
            InstanceItemObjects(searchRoot.artists.items);

        if (searchRoot.playlists != null)
            InstanceItemObjects(searchRoot.playlists.items);

        if (searchRoot.tracks != null)
            InstanceTrackObjects(searchRoot.tracks.items);

        if (searchRoot.shows != null)
            InstanceItemObjects(searchRoot.shows.items);

        if (searchRoot.episodes != null)
            InstanceItemObjects(searchRoot.episodes.items);

        if (searchRoot.audiobooks != null)
            InstanceItemObjects(searchRoot.audiobooks.items);
    }

    private void InstanceTrackObjects(List<Item> _items)
    {
        foreach (Item item in _items)
        {
            SpotifyConnectionDemoArtistHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<SpotifyConnectionDemoArtistHolder>();
            instance.Initialize(item.name);

            if (item.album.images != null && item.album.images.Count > 0)
                instance.SetImage(item.album.images[0].url);
        }
    }

    private void InstanceItemObjects(List<Item> _items)
    {
        foreach (Item item in _items)
        {
            SpotifyConnectionDemoArtistHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<SpotifyConnectionDemoArtistHolder>();
            instance.Initialize(item.name);

            if (item.images != null && item.images.Count > 0)
                instance.SetImage(item.images[0].url);
        }
    }

    private void ResetScrollView()
    {
        for (int i = (objectsToNotDestroyIndex); i < instanceParent.childCount; i++)
        {
            Destroy(instanceParent.GetChild(i).gameObject);
        }
    }

    public void OnValueChanged(int _value)
    {
        switch (_value)
        {
            case 0:
                types = new string[] { "album", "artist", "playlist", "track", "show", "episode", "audiobook" };
                break;
            case 1:
                types = new string[] { "album" };
                break;
            case 2:
                types = new string[] { "artist" };
                break;
            case 3:
                types = new string[] { "playlist" };
                break;
            case 4:
                types = new string[] { "track" };
                break;
            case 5:
                types = new string[] { "show" };
                break;
            case 6:
                types = new string[] { "episode" };
                break;
            case 7:
                types = new string[] { "audiobook" };
                break;
        }
    }
}
