using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IImageDownloaderObject
{
    void SetImage(Texture2D _texture);
    void SetImage(Sprite _sprite);
    GameObject GetImageContainer();
}