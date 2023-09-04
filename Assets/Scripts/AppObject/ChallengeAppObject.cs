using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeAppObject : AppObject
{
    private Challenges challenges;
    public TextMeshProUGUI disk;
    public TextMeshProUGUI challengeName;
    public Image challengeImage;

    public Sprite challengeOnSprite;
    public Sprite challengeOffSprite;


    private SeveralTrackRoot severalTrackRoot;
    private bool PointsPosted = false;



    public override void Initialize(params object[] list)
    {
        challenges = (Challenges)list[0];
        if (challenges.name.Length > 27)
        {
            string _text2 = "";
            for (int k = 0; k < 27; k++)
            {

                _text2 = _text2 + challenges.name[k];

            }
            _text2 = _text2 + "...";
            challengeName.text = _text2;
        }
        else
        {
            challengeName.text = challenges.name;
        }

        if(challengeImage != null)
        {
            bool isCompleted = (bool)list[1];
            challengeImage.sprite = isCompleted ? challengeOffSprite : challengeOnSprite;
        }

        disk.text = challenges.disks.ToString();
    }
    public void Test(){
        SpotifyConnectionManager.instance.GetPlaylist("6Nb0rL2W7QYoQeLRlJi4Cc", TESTCallback);
    }

    public void TESTCallback(object[] _value){
        SpotifyPlaylistRoot ProfilePlaylist = (SpotifyPlaylistRoot)_value[1];
        NewScreenManager.instance.ChangeToSpawnedView("surf");
        
        NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().Challenge = true;
        NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().SetChallengeCallback(gameObject.GetComponent<ChallengeAppObject>());
        NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().DynamicPrefabSpawnerPL(new object[] { ProfilePlaylist});
    }
    public void OnClick_OpenChallenge(){
        
    }

    public void OpenChallengeCallBack(){
        NewScreenManager.instance.ChangeToSpawnedView("surf");
        
        NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().Challenge = true;
        NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().SetChallengeCallback(gameObject.GetComponent<ChallengeAppObject>());
        NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().DynamicPrefabSpawnerSeveralTracks(severalTrackRoot.tracks);
    }


    public void CheckForPoints(){
        Debug.Log("Check Coins");
        List<GameObject> _list = NewScreenManager.instance.GetCurrentView().GetComponentInChildren<PF_SurfManager>().GetInstances();
        float counter = 0;
        for (int i = 0; i < _list.Count-3; i++)
        {
            if(_list[i].GetComponent<ButtonSurfPlaylist>().SuccesfulEnded){
                counter++;
            }
        }

        if(counter >= (_list.Count-3)*.9f && !PointsPosted){
            PointsPosted = true;

            Debug.Log("SUCCESFUL COINS ");
            ////BLA BLA BLA COINS FOR PLAYER
        }
    }
}
