using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingViewModel : ViewModel
{
   public GameObject selectTime;
  
    public void OnClick_SelectTime()
    {
        selectTime.SetActive(true);
    }
}
