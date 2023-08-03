using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpotifyAddItemsToPlaylistViewModel : MonoBehaviour
{
    public TMP_InputField playlistIDInputField;
    public List<string> trackSpotifyUris = new List<string>();

    [Header("Instance References")]
    public GameObject trackHolderPrefab;
    public Transform instanceParent;
    public int objectsToNotDestroyIndex;

    public void OnClick_AddItemsToPlaylist()
    {
        if (!playlistIDInputField.text.Equals(""))
            SpotifyConnectionManager.instance.AddItemsToPlaylist(playlistIDInputField.text, trackSpotifyUris, Callback_OnCLick_AddItemsToPlaylist);
    }

    private void Callback_OnCLick_AddItemsToPlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SpotifyConnectionManager.instance.GetPlaylist(playlistIDInputField.text, Callback_OnCLick_GetPlaylist);
    }

    private void Callback_OnCLick_GetPlaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];

        InstanceTrackObjects(searchedPlaylist.tracks);
    }

    private void InstanceTrackObjects(Tracks _tracks)
    {
        ResetScrollView();

        foreach (Item item in _tracks.items)
        {
            SpotifyConnectionDemoTrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<SpotifyConnectionDemoTrackHolder>();
            instance.Initialize(item.track.name, item.track.artists[0].name, item.track.id);

            if (item.track.album.images != null && item.track.album.images.Count > 0)
                instance.SetImage(item.track.album.images[0].url);
        }
    }

    private void ResetScrollView()
    {
        for (int i = (objectsToNotDestroyIndex); i < instanceParent.childCount; i++)
        {
            Destroy(instanceParent.GetChild(i).gameObject);
        }
    }
}
