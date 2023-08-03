using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotifyGetCurrentUserPlaylistsViewModel : MonoBehaviour
{
    public GameObject playlistHolderPrefab;
    public Transform instanceParent;

    public void OnClick_GetCurrentUserPlaylists()
    {
        SpotifyConnectionManager.instance.GetCurrentUserPlaylists(Callback_OnClick_GetCurrentUserPlaylists);
    }

    private void Callback_OnClick_GetCurrentUserPlaylists(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ResetScrollView();

        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];

        foreach (Item item in playlistRoot.items)
        {
            SpotifyConnectionDemoPlaylistsHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<SpotifyConnectionDemoPlaylistsHolder>();
            instance.Initialize(item.name, item.id, item.owner.display_name, item.@public);

            if (item.images != null && item.images.Count > 0)
                instance.SetImage(item.images[0].url);
        }
    }

    private void ResetScrollView()
    {
        for(int i = 0; i < instanceParent.childCount; i++)
        {
            Destroy(instanceParent.GetChild(i).gameObject);
        }
    }
}
