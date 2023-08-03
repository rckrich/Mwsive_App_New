using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileViewModel : ViewModel
{
    // Start is called before the first frame update
    public TextMeshProUGUI displayName;
    public Image profilePicture;
    private string profileId;
    public GameObject playlistHolderPrefab;
    public Transform instanceParent;
    void OnEnable()
    {
        SpotifyConnectionManager.instance.GetCurrentUserProfile(Callback_GetUserProfile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick_SpawnInsigniasButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("insignias");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void OnClick_SpawnADNButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("adn");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }

    public void OnClick_SpawnMiPlaylistButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("miPlaylist");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
    public void OnClick_SpawnPopUpButton()
    {
        NewScreenManager.instance.ChangeToSpawnedView("popUp");
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);
    }
    

    private void Callback_GetUserProfile(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        ProfileRoot profileRoot = (ProfileRoot)_value[1];
        displayName.text = profileRoot.display_name;
        profileId = profileRoot.id;
        ImageManager.instance.GetImage(profileRoot.images[0].url, profilePicture, (RectTransform)this.transform);
        GetCurrentUserPlaylists();
    }

    public void GetCurrentUserPlaylists()
    {
        if (!profileId.Equals(""))
            SpotifyConnectionManager.instance.GetUserPlaylists(profileId, Callback_OnClick_GetUserPlaylists);
    }

    private void Callback_OnClick_GetUserPlaylists(object[] _value)
    {
        if (SpotifyConnectionManager.instance.CheckReauthenticateUser((long)_value[0])) return;

        PlaylistRoot playlistRoot = (PlaylistRoot)_value[1];
        
        for(int i = 0; i < 6; i++)
        {
            PlaylisHolder instance = GameObject.Instantiate(playlistHolderPrefab, instanceParent).GetComponent<PlaylisHolder>();
            instance.Initialize(playlistRoot.items[i].name, playlistRoot.items[i].id, playlistRoot.items[i].owner.display_name, playlistRoot.items[i].@public);           
            if (playlistRoot.items[i].images != null && playlistRoot.items[i].images.Count > 0)
                instance.SetImage(playlistRoot.items[i].images[0].url);
        }
    }
    public void OnClick_BackButton()
    {
        OpenView(ViewID.SurfViewModel);
    }

    public void OnClick_OptionsButton()
    {
        OpenView(ViewID.OptionsViewModel);
    }

    public void OnClick_FollowFollowers()
    {
        OpenView(ViewID.FollowersViewModel);
    }

    public void OnClick_EditProfile()
    {
        OpenView(ViewID.EditProfileViewModel);
    }

    public void OnClick_Surf()
    {
        OpenView(ViewID.SurfViewModel);
    }
    private void OpenView(ViewID _value)
    {
        NewScreenManager.instance.ChangeToMainView(_value, false);
        Debug.Log(NewScreenManager.instance.GetCurrentView().gameObject.name);

    }
}

