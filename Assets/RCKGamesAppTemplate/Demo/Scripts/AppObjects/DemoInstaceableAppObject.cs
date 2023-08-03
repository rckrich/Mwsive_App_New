using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemoInstaceableAppObject : InstanceableAppObject
{
    public TextMeshProUGUI idText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public override void Initialize(params object[] list)
    {
        int objID = (int)list[0];
        string objName = (string)list[1];
        string objDescription = (string)list[2];

        idText.text = "ID: " + objID.ToString();
        nameText.text = "NAME: " + objName;
        descriptionText.text = "DESCRIPTION: " + objDescription;
    }
}
