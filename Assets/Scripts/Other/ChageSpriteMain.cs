using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChageSpriteMain : ViewModel
{
    public GameObject sprite;
    private void OnEnable()
    {
        AddEventListener<ChangeProfileSpriteAppEvent>(Listener_ChangeProfileSpriteAppEvent);
    }
    private void OnDisable()
    {
        RemoveEventListener<ChangeProfileSpriteAppEvent>(Listener_ChangeProfileSpriteAppEvent);
    }
    private void Listener_ChangeProfileSpriteAppEvent(ChangeProfileSpriteAppEvent _event)
    {
        sprite.GetComponent<Image>().sprite = _event.newSprite;
    }
}
