using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add_ARscan_Count_Condition_2 : MonoBehaviour
{
    private void OnEnable()
    {
        Add_ARscan_Count();
    }

    void Add_ARscan_Count()
    {
        if (Progress_Manager.Instance.progress.volver_a_mostrar_InAppReview)
        {
            Progress_Manager.Instance.progress.numero_de_ARscans_para_mostrar_InAppReview += 1;
            Progress_Manager.Instance.save();
        }
    }
}
