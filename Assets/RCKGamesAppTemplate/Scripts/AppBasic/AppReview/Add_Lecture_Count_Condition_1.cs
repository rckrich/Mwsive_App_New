using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add_Lecture_Count_Condition_1 : MonoBehaviour
{

    private void OnEnable()
    {
        Add_Lecture_Count();
    }


    void Add_Lecture_Count()
    {
        if (Progress_Manager.Instance.progress.volver_a_mostrar_InAppReview)
        {
            Progress_Manager.Instance.progress.numero_de_lecturas_para_mostrar_InAppReview += 1;
            Progress_Manager.Instance.save();

#if UNITY_IOS

            iOS_App_Review.Instance.Show_InAppReview();

#endif

#if UNITY_ANDROID

            if (Application.platform == RuntimePlatform.Android)
            {
                Android_App_Review.Instance.Show_InAppReview();
            }

#endif

        }
    }

}
