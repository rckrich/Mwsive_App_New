using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotifyGetCurrentUserTopTracksViewModel : MonoBehaviour
{
    public GameObject trackHolderPrefab;
    public Transform instanceParent;

    public void OnClick_GetCurrentUserTopTracks()
    {
        SpotifyConnectionManager.instance.GetCurrentUserTopTracks(Callback_OnCLick_GetUserTopTracks);
    }

    private void Callback_OnCLick_GetUserTopTracks(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ResetScrollView();

        UserTopItemsRoot userTopItemsRoot = (UserTopItemsRoot)_value[1];

        foreach(Item item in userTopItemsRoot.items)
        {
            SpotifyConnectionDemoTrackHolder instance = GameObject.Instantiate(trackHolderPrefab, instanceParent).GetComponent<SpotifyConnectionDemoTrackHolder>();
            instance.Initialize(item.name, item.artists[0].name, item.id);

            if (item.album.images != null && item.album.images.Count > 0)
                instance.SetImage(item.album.images[0].url);
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
