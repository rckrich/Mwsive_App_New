using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SurfAni : MonoBehaviour
{
    public GameObject MainCanvas;
    public float MoveTransitionDuration = 0.5f;
    public float ScaleTransitionDuration = 1f;
    public float FadeTransitionDuration = 1f;
    public float ColorTransitionDuration = 0.5f;
    public float SurfTransitionDuration = 0.5f;


    private static UIAniManager _instance;
    private Vector2 FinalPosition, RestPositionSide, RestPositionDown, RestPositionUp;
    public bool IsAddSongSurfDone = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void SetPosition()
    {
        FinalPosition = MainCanvas.transform.position;
        RestPositionDown = new Vector2(MainCanvas.transform.position.x, -2 * MainCanvas.transform.position.y);
        RestPositionUp = new Vector2(MainCanvas.transform.position.x, 2 * MainCanvas.transform.position.y);
        RestPositionSide = new Vector2(MainCanvas.transform.position.x * 4, MainCanvas.transform.position.y);
        //RestPositionLeft = new Vector2(MainCanvas.transform.position.x * -2, MainCanvas.transform.position.y);
    }
    // Update is called once per frame
    public void SurfSide(GameObject GA, float var, float MaxRotation, float fade, bool IsItFinished)
    {
        SetPosition();
        if (IsItFinished)
        {
            GA.transform.DOMove(new Vector2(RestPositionSide.x * var, RestPositionSide.y), SurfTransitionDuration, false).OnComplete(() => { GA.SetActive(false); GA.transform.position = RestPositionUp; });
        }
        else
        {
            GA.transform.DOMove(new Vector2(RestPositionSide.x * var, RestPositionSide.y), SurfTransitionDuration, false);
        }

        GA.transform.DORotate(new Vector3(0f, 0f, MaxRotation * var), SurfTransitionDuration);

        GA.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration);
    }

    public void SurfSideLastPosition(GameObject GA, GameObject Position, float var, float MaxRotation, float fade, GameObject SurfManager)
    {
        SetPosition();
        GA.transform.DOMove(new Vector2(RestPositionSide.x * var, RestPositionSide.y), SurfTransitionDuration, false).OnComplete(() => { SurfSideTransitionBack(GA, Position, -MaxRotation, SurfManager); });
        GA.transform.DORotate(new Vector3(0f, 0f, MaxRotation * var), SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration);
    }

    public void SurfSideTransitionBack(GameObject GA, GameObject Position, float Maxrotation, GameObject SurfManager)
    {
        SetPosition();
        GA.transform.eulerAngles = new Vector3(0f, 0f, Maxrotation);
        GA.transform.position = RestPositionUp;

        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration);
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);
        GA.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration).OnComplete(() => { SurfManager.GetComponent<PF_SurfManager>().HasSideScrollEnded = true; });


    }

    public void SurfReset(GameObject GA)
    {

        SetPosition();
        GA.transform.DOMove(FinalPosition, SurfTransitionDuration, false);
        GA.transform.DORotate(new Vector3(0f, 0f, 0f), SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(1, SurfTransitionDuration);
    }

    public void SurfVerticalUp(GameObject GA, float var, float MaxRotation, float fade, bool IsItFinished)
    {
        SetPosition();
        if (IsItFinished)
        {
            GA.transform.DOMove(new Vector2(RestPositionUp.x, RestPositionUp.y * var), SurfTransitionDuration, false).OnComplete(() => { GA.SetActive(false); });
        }
        else
        {
            GA.transform.DOMove(new Vector2(RestPositionUp.x, RestPositionUp.y * var), SurfTransitionDuration, false);
        }



        GA.transform.DORotate(new Vector3(0f, 0f, MaxRotation * var), SurfTransitionDuration).OnComplete(() => { GA.transform.DORotate(new Vector3(0f, 0f, 0f), SurfTransitionDuration / 2); });
        GA.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration);
    }




    public void SurfVerticalDown(GameObject GA, float var, float MaxRotation, float fade, bool IsItFinished)
    {
        GA.transform.DORotate(new Vector3(0f, 0f, MaxRotation * var), SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration);
    }
    public void SurfVerticalDown(GameObject GA, float var, float MaxRotation, float fade, bool IsItFinished, GameObject position)
    {
        SetPosition();

        if (IsItFinished)
        {

            GA.transform.DOMove(new Vector2(RestPositionDown.x, RestPositionDown.y * var), SurfTransitionDuration + .5f, false);
        }
        else
        {

            GA.transform.DOMove(new Vector2(RestPositionDown.x, RestPositionDown.y * var), SurfTransitionDuration, false);
        }

        GA.transform.DORotate(new Vector3(0f, 0f, MaxRotation * var), SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration);
    }

    public void SurfTransitionBackSongDown(GameObject GA, GameObject Position)
    {

        DOTween.Kill(GA);
        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration);
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);
        GA.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration);


    }



    public void SurfResetOtherSongs(GameObject GA, GameObject Position, bool Visible)
    {


        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration).OnComplete(() => { GA.SetActive(Visible); });

    }

    public void SurfTransitionBackSong(GameObject GA, GameObject Position, float Maxrotation)
    {

        GA.SetActive(true);
        GA.transform.eulerAngles = new Vector3(0f, 0f, Maxrotation);
        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration);
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);
        GA.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration);

    }



    public void SurfTransitionOtherSongs(GameObject GA, GameObject Position, float var)
    {
        DOTween.Kill(GA);
        float fade = Mathf.Clamp(var * 2, 0, 1);
        GA.SetActive(true);
        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha * fade, SurfTransitionDuration);
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);

        if (GA.transform.rotation != Quaternion.identity)
        {
            GA.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration);
        }

    }

    public void SurfTransitionBackHideSong(GameObject GA, GameObject Position, float var)
    {

        float fade = Mathf.Clamp(var * 2, 0, 1);
        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha * fade, SurfTransitionDuration).OnComplete(() => { GA.SetActive(false); });
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);

    }

    public void SurfAddSong(GameObject GA, float fade)
    {


        GA.SetActive(true);
        fade = Mathf.Clamp(fade * 1.5f, 0, 1f);
        GA.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration);


    }
    public void SurfAddSongReset(GameObject GA)
    {
        if (IsAddSongSurfDone)
        {
            if (GA.transform.localScale != new Vector3(0, 0, 0))
            {
                DOTween.Kill(GA);
                GA.transform.DOScale(new Vector3(0, 0, 0), 0.3F).OnComplete(() => { GA.SetActive(false); });

            }


        }

    }

    public void CompleteSurfAddSong(GameObject GA, float fade)
    {

        IsAddSongSurfDone = false;
        DOTween.Complete(GA);
        GA.GetComponent<CanvasGroup>().DOFade(1, 0.1f);

        GA.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(0, SurfTransitionDuration * 2).OnComplete(() => { IsAddSongSurfDone = true; });
    }

    public void SurfAddSongLastPosition(GameObject GA, float fade)
    {

        DOTween.Complete(GA);
        GA.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
        GA.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(0, SurfTransitionDuration * 2).OnComplete(() => { GA.transform.localScale = new Vector3(0, 0, 0); GA.GetComponent<CanvasGroup>().alpha = 0; });
    }

    public void DoubleClickOla(GameObject GA)
    {
        GA.transform.DOScale(new Vector3(1.5F, 1.5F, 1.5F), .3f);
        GA.GetComponent<CanvasGroup>().DOFade(0, .3f).OnComplete(() => { Destroy(GA); });
    }

    public void SurfShareSpawn(GameObject GA)
    {
        SetPosition();
        GA.transform.position = RestPositionUp;
    }
}
