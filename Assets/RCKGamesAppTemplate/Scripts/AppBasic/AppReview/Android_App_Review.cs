
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
//using Google.Play.Review;
#endif

public class Android_App_Review : RCK_Singleton<Android_App_Review>
{

#if UNITY_ANDROID

    //ReviewManager _reviewManager;
    //PlayReviewInfo _playReviewInfo;


    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            //_reviewManager = new ReviewManager();

            Show_InAppReview();
        }
    }


    IEnumerator ReviewOperation()
    {
        yield return new WaitForSeconds(1f);


/*var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            Debug.Log(requestFlowOperation.Error.ToString());
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();

        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            Debug.Log(launchFlowOperation.Error.ToString());
            yield break;
        }
*/
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }

    
    public void Show_InAppReview()
    {
        if (Progress_Manager.Instance.progress.volver_a_mostrar_InAppReview)
        {
            //Condicion #1
            if (Progress_Manager.Instance.progress.numero_de_lecturas_para_mostrar_InAppReview == 5 && Progress_Manager.Instance.progress.veces_que_se_ha_mostrado_InAppReview < 2)
            {

                StartCoroutine(ReviewOperation());      // ESTA LINEA ES LA QUE MUESTRA LA PREGUNTA DEL IN APP REVIEW

                Progress_Manager.Instance.progress.veces_que_se_ha_mostrado_InAppReview += 1;
                Progress_Manager.Instance.save();
            }

            //Condicion #2
            if (Progress_Manager.Instance.progress.numero_de_ARscans_para_mostrar_InAppReview >= 3 && Progress_Manager.Instance.progress.veces_que_se_ha_mostrado_InAppReview < 2)
            {

                StartCoroutine(ReviewOperation());      // ESTA LINEA ES LA QUE MUESTRA LA PREGUNTA DEL IN APP REVIEW

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

