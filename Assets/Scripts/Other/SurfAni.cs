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

    private float var;
    private float maxRotation;
    private float fade;
    private bool isItFinished;
    private GameObject Position;
    private Vector2 FinalPosition, RestPositionSide, RestPositionDown, RestPositionUp;

    private Tween SurfSide, SurfSideLastPosition, SurfSideTransitionBack, SurfReset, VerticalUp, VerticalDown1, VerticalDown2, SurfTransitionBackSongDown, SurfResetOtherSongs, SurfTransitionBackSong;
    private Tween SurfTransitionOtherSongs, SurfTransitionBackHideSong, SurfAddSong, SurfAddSongReset, CompleteAddSurfAddSong, SurfAddSongLastPosition;

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
    private void SetUp_SurfSide()
    {
        SetPosition();
        var sequence = DOTween.Sequence();
        SurfSide = sequence;
        sequence.Append(gameObject.transform.DOMove(new Vector2(RestPositionSide.x * var, RestPositionSide.y), SurfTransitionDuration, false));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration));
        sequence.OnComplete(() => {
        if (isItFinished)
        {
          gameObject.SetActive(false); gameObject.transform.position = RestPositionUp;
        }
             });

        sequence.Pause();
    }

    private void SetUp_SurfSideLastPosition(GameObject GA, GameObject Position, float var, float MaxRotation, float fade, GameObject SurfManager)
    {
        SetPosition();
        var sequence = DOTween.Sequence();
        SurfSideLastPosition = sequence;
        sequence.Append(gameObject.transform.DOMove(new Vector2(RestPositionSide.x * var, RestPositionSide.y), SurfTransitionDuration, false));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration));
        sequence.OnComplete(() => {

            SurfSideTransitionBack(GA, Position, -MaxRotation, SurfManager);

        });
        sequence.Pause();
    }

    private void SetUp_SurfSideTransitionBack()
    {
        SetPosition();
        var sequence = DOTween.Sequence();
        SurfSideTransitionBack = sequence;

        sequence.OnPlay(() => {
            gameObject.transform.eulerAngles = new Vector3(0f, 0f, maxRotation);
            gameObject.transform.position = RestPositionUp;
        });

        sequence.Append(gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration));
        sequence.Join(gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration));

        sequence.OnComplete(() => { SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().HasSideScrollEnded = true; });



        sequence.Pause();
    }

    private void SetUp_SurfReset()
    {

        SetPosition();
        var sequence = DOTween.Sequence();
        SurfReset = sequence;

        sequence.Append(gameObject.transform.DOMove(FinalPosition, SurfTransitionDuration, false));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0f, 0f, 0f), SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(1, SurfTransitionDuration));
        sequence.Pause();
    }

    private void SetUp_SurfVerticalUp()
    {
        SetPosition();
        var sequence = DOTween.Sequence();
        VerticalUp = sequence;

        sequence.Append(gameObject.transform.DOMove(new Vector2(RestPositionUp.x, RestPositionUp.y * var), SurfTransitionDuration, false));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0f, 0f, MaxRotation * var), SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration));

        sequence.OnComplete(() => {
            if (isItFinished)
            {
                gameObject.SetActive(false);
            }
            gameObject.transform.DORotate(new Vector3(0f, 0f, 0f), SurfTransitionDuration / 2);
        });
        sequence.Pause();
    }

    private void SetUp_SurfVerticalDown()
    {
        var sequence = DOTween.Sequence();
        VerticalDown1 = sequence;
        sequence.Append(gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration));

        sequence.Pause();
    }

    private void SetUp_SurfVerticalDown2()
    {
        SetPosition();

        var sequence = DOTween.Sequence();
        VerticalDown2 = sequence;

        sequence.Append(gameObject.transform.DOMove(new Vector2(RestPositionDown.x, RestPositionDown.y * var), SurfTransitionDuration + .5f, false));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration));

        sequence.Pause();
        
    }

    private void SetUp_SurfTransitionBackSongDown()
    {

        //DOTween.Kill(GA);
        var sequence = DOTween.Sequence();
        SurfTransitionBackSongDown = sequence;

        sequence.Append(gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration));
        sequence.Join(gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration));

    }



    private void SetUp_SurfResetOtherSongs(GameObject GA, GameObject Position, bool Visible)
    {


        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration).OnComplete(() => { GA.SetActive(Visible); });

    }

    private void SetUp_SurfTransitionBackSong(GameObject GA, GameObject Position, float Maxrotation)
    {

        GA.SetActive(true);
        GA.transform.eulerAngles = new Vector3(0f, 0f, Maxrotation);
        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration);
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);
        GA.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration);

    }



    private void SetUp_SurfTransitionOtherSongs(GameObject GA, GameObject Position, float var)
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

    private void SetUp_SurfTransitionBackHideSong(GameObject GA, GameObject Position, float var)
    {

        float fade = Mathf.Clamp(var * 2, 0, 1);
        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha * fade, SurfTransitionDuration).OnComplete(() => { GA.SetActive(false); });
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);

    }

    private void SetUp_SurfAddSong(GameObject GA, float fade)
    {


        GA.SetActive(true);
        fade = Mathf.Clamp(fade * 1.5f, 0, 1f);
        GA.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration);


    }
    private void SetUp_SurfAddSongReset(GameObject GA)
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

    private void SetUp_CompleteSurfAddSong(GameObject GA, float fade)
    {

        IsAddSongSurfDone = false;
        DOTween.Complete(GA);
        GA.GetComponent<CanvasGroup>().DOFade(1, 0.1f);

        GA.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(0, SurfTransitionDuration * 2).OnComplete(() => { IsAddSongSurfDone = true; });
    }

    private void SetUp_SurfAddSongLastPosition(GameObject GA, float fade)
    {

        DOTween.Complete(GA);
        GA.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
        GA.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(0, SurfTransitionDuration * 2).OnComplete(() => { GA.transform.localScale = new Vector3(0, 0, 0); GA.GetComponent<CanvasGroup>().alpha = 0; });
    }

    private void SurfShareSpawn(GameObject GA)
    {
        SetPosition();
        GA.transform.position = RestPositionUp;
    }
}
