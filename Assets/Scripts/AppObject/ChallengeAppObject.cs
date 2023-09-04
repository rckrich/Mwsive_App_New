using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChallengeAppObject : AppObject
{
    private Challenges challenges;
    public TextMeshProUGUI disk;
    public TextMeshProUGUI textname;
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
            textname.text = _text2;
        }
        else
        {
            textname.text = challenges.name;
        }

        disk.text = challenges.disks.ToString();
    }
    public void OnClick_OpenChallenge(){
        List<string> tracks = new List<string>();
        foreach (MwsiveTrack item in challenges.mwsive_tracks)
            {
                tracks.Add(item.spotify_track_id);
            }
        SpotifyConnectionManager.instance.GetSeveralTracks(tracks.ToArray(), OpenChallengeCallBack);
    }

    public void OpenChallengeCallBack(object[] _value){
        SeveralTrackRoot severalTrackRoot = (SeveralTrackRoot)_value[1];

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
            MwsiveConnectionManager.instance.PostChallengeComplete(challenges.id);
            UIMessage.instance.UIMessageInstanciate("Desafio Completado");
            
        }
    }
}
