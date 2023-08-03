using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iOS_App_Review : RCK_Singleton<iOS_App_Review>
{

#if UNITY_IOS

    void Start()
    {
        Show_InAppReview();
    }

    public void Btn_Add_ARscan_Count()
    {
        Progress_Manager.Instance.progress.numero_de_ARscans_para_mostrar_InAppReview += 1;
        Progress_Manager.Instance.save();

        Show_InAppReview();
    }

    public void Show_InAppReview()
    {
        if (Progress_Manager.Instance.progress.volver_a_mostrar_InAppReview)
        {
            //Condicion #1
            if (Progress_Manager.Instance.progress.numero_de_lecturas_para_mostrar_InAppReview == 5 && Progress_Manager.Instance.progress.veces_que_se_ha_mostrado_InAppReview < 2)
            {
                UnityEngine.iOS.Device.RequestStoreReview();    // ESTA LINEA ES LA QUE MUESTRA LA PREGUNTA DEL IN APP REVIEW

                Progress_Manager.Instance.progress.veces_que_se_ha_mostrado_InAppReview += 1;
                Progress_Manager.Instance.save();
            }

            //Condicion #2
            if(Progress_Manager.Instance.progress.numero_de_ARscans_para_mostrar_InAppReview >= 3 && Progress_Manager.Instance.progress.veces_que_se_ha_mostrado_InAppReview < 2)
            {
                UnityEngine.iOS.Device.RequestStoreReview();    // ESTA LINEA ES LA QUE MUESTRA LA PREGUNTA DEL IN APP REVIEW

                Progress_Manager.Instance.progress.veces_que_se_ha_mostrado_InAppReview += 1;
                Progress_Manager.Instance.save();
            }


            if (Progress_Manager.Instance.progress.veces_que_se_ha_mostrado_InAppReview >= 2)
            {
                Progress_Manager.Instance.progress.volver_a_mostrar_InAppReview = false;
                Progress_Manager.Instance.save();
            }
        }
    }

#endif

}
