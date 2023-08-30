using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChallengeAppObject : AppObject
{
    private Challenges challenges;
    public TextMeshProUGUI disk;
    public TextMeshProUGUI name;


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
            name.text = _text2;
        }
        else
        {
            name.text = challenges.name;
        }

        disk.text = challenges.disks.ToString();
    }
}
