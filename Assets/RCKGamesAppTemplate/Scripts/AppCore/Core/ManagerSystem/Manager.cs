using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager : AppObject
{
    private const int NETWORKLOADINGPANEL_ANIMATION_LAYER = 0;
    private const float MINIMUM_SECONDS_FOR_END_SEARCH_PANEL = 1.0f;

    [Header("General View Model Game Object Reference")]
    public GameObject networkLoadingCanvas;

    public virtual void StartSearch() { StartCoroutine(CR_StartSearch()); }

    public virtual void EndSearch() { StartCoroutine(CR_EndSearch()); }

    protected virtual IEnumerator CR_StartSearch()
    {
        if (networkLoadingCanvas == null)
            networkLoadingCanvas = GameObject.FindWithTag("NetworkLoadingCanvas");

        if (networkLoadingCanvas != null)
        {
            networkLoadingCanvas.transform.GetChild(0).gameObject.SetActive(true);
        }
        yield return null;
    }

    protected virtual IEnumerator CR_EndSearch()
    {
        if (networkLoadingCanvas == null)
            networkLoadingCanvas = GameObject.FindWithTag("NetworkLoadingCanvas");

        if (networkLoadingCanvas != null)
        {
            GameObject networkLoadingPanel = networkLoadingCanvas.transform.GetChild(0).gameObject;
            networkLoadingPanel.SetActive(false);
            if (networkLoadingPanel.GetComponent<Animator>().GetCurrentAnimatorClipInfo(NETWORKLOADINGPANEL_ANIMATION_LAYER).Length > 0)
            {
                float animationLenght = networkLoadingPanel.GetComponent<Animator>().GetCurrentAnimatorClipInfo(NETWORKLOADINGPANEL_ANIMATION_LAYER)[0].clip.length;
                yield return new WaitForSeconds(animationLenght);
            }
            else
            {
                yield return new WaitForSeconds(MINIMUM_SECONDS_FOR_END_SEARCH_PANEL);
            }

        }

        yield return null;
    }
}