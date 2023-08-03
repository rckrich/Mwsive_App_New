using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpotifyGetPlaylistViewModel : MonoBehaviour
{
    public TMP_InputField playlistID;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI creatorName;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI publicText;
    public TextMeshProUGUI spotifyID;
    public Image playListPicture;

    [Header("Instance Referecnes")]
    public GameObject trackHolderPrefab;
    public Transform instanceParent;
    public int objectsToNotDestroyIndex;

    public void OnClick_GetPlaylist()
    {
        if (!playlistID.text.Equals(""))
            SpotifyConnectionManager.instance.GetPlaylist(playlistID.text, Callback_OnCLick_GetPLaylist);
    }

    public void OnClick_CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = spotifyID.text;
    }

    private void Callback_OnCLick_GetPLaylist(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        SearchedPlaylist searchedPlaylist = (SearchedPlaylist)_value[1];
        displayName.text = searchedPlaylist.name;
        spotifyID.text = searchedPlaylist.id;
        creatorName.text = searchedPlaylist.owner.display_name;
        descriptionText.text = searchedPlaylist.description;
        publicText.text = searchedPlaylist.@public.ToString(); ;

        InstanceTrackObjects(searchedPlaylist.tracks);

        if (searchedPlaylist.images != null && searchedPlaylist.images.Count > 0)
            ImageManager.instance.GetImage(searchedPlaylist.images[0].url, playListPicture, (RectTransform)this.transform);
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
