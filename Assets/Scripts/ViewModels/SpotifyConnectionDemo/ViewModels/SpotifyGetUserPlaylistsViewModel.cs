using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpotifyGetUserPlaylistsViewModel : MonoBehaviour
{
    public TMP_InputField userIDInputField;
    public GameObject playlistHolderPrefab;
    public Transform instanceParent;

    public void OnClick_GetCurrentUserPlaylists()
    {
        if(!userIDInputField.text.Equals(""))
            SpotifyConnectionManager.instance.GetUserPlaylists(userIDInputField.text, Callback_OnClick_GetUserPlaylists);
    }

    private void Callback_OnClick_GetUserPlaylists(object[] _value)
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
        for (int i = 0; i < instanceParent.childCount; i++)
        {
            Destroy(instanceParent.GetChild(i).gameObject);
        }
    }
}
