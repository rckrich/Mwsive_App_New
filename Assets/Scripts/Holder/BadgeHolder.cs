using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BadgeHolder : ViewModel
{
    public Image image;
    public TextMeshProUGUI badgeText;

    private Badge badges;
    private string type;
    private string group;
    private MwsiveTrack track;

    [SerializeField]
    List<Sprite> sprites;
    
    public override void Initialize(params object[] list)
    {
        badges = (Badge)list[0];

        type = badges.type;
        group = badges.group;
        typeSwitch();
    }

    public void typeSwitch()
    {
        switch (type)
        {
           case "track":
                track = badges.mwsive_track;
                if (track.name.Length > 20)
                {
                    string _text2 = "";
                    for (int k = 0; k < 27; k++)
                    {

                        _text2 = _text2 + track.name[k];

                    }
                    _text2 = _text2 + "...";
                    badgeText.text = "Top " + "#" + group + "<br>" + _text2;
                }
                else
                {
                    badgeText.text = "Top " + "#" + group + "<br>" + track.name;
                }
                image.sprite = sprites[0];
                
                break;

            case "engagement":

                badgeText.text = group;
                image.sprite = sprites[1];

                break;
        }
    }

    public void OnClick_Badge()
    {
        if(type.Equals("track"))
            CallPopUP(PopUpViewModelTypes.MessageOnly, "Top #" + group + "<br>" + track.name, ("<br> Fuiste de los primeros " + group + " en hacer pik a esta canción"), "Aceptar", image.sprite);
        if (type.Equals("engagement"))
            CallPopUP(PopUpViewModelTypes.MessageOnly, "Insignia " + char.ToUpper(group.ToCharArray()[0]) + group.Substring(1), ("<br> Has hecho " + FilterBadgeEngamentGroup(group) + " piks"), "Aceptar", image.sprite);
    }

    private string FilterBadgeEngamentGroup(string _group)
    {
        switch (_group)
        {
            case "explorador":                
                return "100";
            case "virtuoso":
                return "1K";
            case "innovador":
                return "5K";
            case "song_master":
                return "10K";
            case "wave_master":
                return "100K";
            case "music_master":
                return "1M";
        }

        return "";
    }

}
