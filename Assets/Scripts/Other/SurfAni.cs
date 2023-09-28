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
    private bool visible;
    private GameObject Position;
    private Vector2 FinalPosition, RestPositionSide, RestPositionDown, RestPositionUp;

    public bool isAvailable, debug;

    private Tween SurfSide, SurfSideLastPosition, SurfSideTransitionBack, SurfReset, VerticalUp, VerticalDown1, VerticalDown2, SurfTransitionBackSongDown, SurfResetOtherSongs, SurfTransitionBackSong;
    private Tween SurfTransitionOtherSongs, SurfTransitionBackHideSong, SurfAddSong, SurfAddSongReset, CompleteAddSurfAddSong, SurfAddSongLastPosition;

    private bool IsAddSongSurfDone = true;
    private Vector3 pos, scale;
    private float aplha;
    // Start is called before the first frame update
    void Start()
    {
        
        SetPosition();
    }
    void SetPosition()
    {
        MainCanvas = UIAniManager.instance.MainCanvas;
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
          isAvailable = true; gameObject.transform.position = RestPositionUp;
        }
             });

        sequence.Pause();
        sequence.SetAutoKill(false);
    }

    private void SetUp_SurfSideLastPosition()
    {
        SetPosition();
        var sequence = DOTween.Sequence();
        SurfSideLastPosition = sequence;
        sequence.Append(gameObject.transform.DOMove(new Vector2(RestPositionSide.x * var, RestPositionSide.y), SurfTransitionDuration, false));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration));
        sequence.OnComplete(() => {

            SurfSideTransitionBack.Play();

        });
        sequence.Pause();
        sequence.SetAutoKill(false);
    }

    private void SetUp_SurfSideTransitionBack()
    {
        SetPosition();
        var sequence = DOTween.Sequence();
        SurfSideTransitionBack = sequence;

        sequence.Append(gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration));
        sequence.Join(gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration));

        sequence.OnComplete(() => { SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().HasSideScrollEnded = true; });



        sequence.Pause();
        sequence.SetAutoKill(false);
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
        sequence.SetAutoKill(false);
    }

    private void SetUp_SurfVerticalUp()
    {
        SetPosition();
        var sequence = DOTween.Sequence();
        VerticalUp = sequence;

        sequence.Append(gameObject.transform.DOMove(new Vector2(RestPositionUp.x, RestPositionUp.y * var), SurfTransitionDuration, false));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration));

        sequence.OnComplete(() => {
            if (isItFinished)
            {
                isAvailable = true;
            }
            gameObject.transform.DORotate(new Vector3(0f, 0f, 0f), SurfTransitionDuration / 2);
        });
        sequence.Pause();
        sequence.SetAutoKill(false);
    }

    private void SetUp_SurfVerticalDown()
    {
        var sequence = DOTween.Sequence();
        VerticalDown1 = sequence;
        sequence.Append(gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration));

        sequence.Pause();
        sequence.SetAutoKill(false);
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
        sequence.SetAutoKill(false);

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
        sequence.Pause();
        sequence.SetAutoKill(false);
    }

    private void SetUp_SurfResetOtherSongs()
    {

        var sequence = DOTween.Sequence();
        SurfResetOtherSongs = sequence;

        sequence.Append(gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false));
        sequence.Join(gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration));
        sequence.OnComplete(() => { if(visible)
            {
                isAvailable = false;

            }
            else
            {
                isAvailable = true;
            }
            


        });
        sequence.Pause();
        sequence.SetAutoKill(false);
    }

    private void SetUp_SurfTransitionBackSong()
    {
        var sequence = DOTween.Sequence();
        SurfTransitionBackSong = sequence;
        sequence.Append(gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration));
        sequence.Join(gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration));
        sequence.Pause();
        sequence.SetAutoKill(false);
    }

    private void SetUp_SurfTransitionOtherSongs()
    {
        var sequence = DOTween.Sequence();
        SurfTransitionOtherSongs = sequence;

        
        
        sequence.Append(gameObject.transform.DOMove(pos, SurfTransitionDuration, false));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(aplha, SurfTransitionDuration));
        sequence.Join(gameObject.transform.DOScale(scale, SurfTransitionDuration));
        sequence.Join(gameObject.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration));
        sequence.Pause();
        sequence.SetAutoKill(false);
    }

    private void SetUp_SurfTransitionBackHideSong()
    {

        var sequence = DOTween.Sequence();
        SurfTransitionBackHideSong = sequence;

        sequence.Append(gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha * fade, SurfTransitionDuration));
        sequence.Join(gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration));
        sequence.OnComplete(() => { gameObject.SetActive(false); });
        sequence.Pause();
        sequence.SetAutoKill(false);
    }

    private void SetUp_SurfAddSong()
    {

        var sequence = DOTween.Sequence();
        SurfTransitionBackHideSong = sequence;

        

        sequence.Append(gameObject.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration));
        sequence.Pause();
        sequence.SetAutoKill(false);
    }

    private void SetUp_SurfAddSongReset()
    {
        /*
        if (IsAddSongSurfDone)
        {
            if (GA.transform.localScale != new Vector3(0, 0, 0))
            {
                DOTween.Kill(GA);
                

            }


        }*/

        var sequence = DOTween.Sequence();
        SurfAddSongReset = sequence;

        sequence.Append(gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.3F));
        sequence.OnComplete(() => { gameObject.SetActive(false); });

        sequence.Pause();
        sequence.SetAutoKill(false);


    }

    private void SetUp_CompleteSurfAddSong()
    {


        
        //DOTween.Complete(GA);
        var sequence = DOTween.Sequence();
        CompleteAddSurfAddSong = sequence;

        sequence.Append(gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
        sequence.Join(gameObject.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(0, SurfTransitionDuration * 2));
        sequence.OnComplete(() => { IsAddSongSurfDone = true; });
        sequence.Pause();
        sequence.SetAutoKill(false);
    }

    private void SetUp_SurfAddSongLastPosition()
    {

        //DOTween.Complete(GA);
        var sequence = DOTween.Sequence();
        SurfAddSongLastPosition = sequence;

        sequence.Append(gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
        sequence.Join(gameObject.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration));
        sequence.Join(gameObject.GetComponent<CanvasGroup>().DOFade(0, SurfTransitionDuration * 2));
        sequence.OnComplete(() => { gameObject.transform.localScale = new Vector3(0, 0, 0); gameObject.GetComponent<CanvasGroup>().alpha = 0; });

        sequence.Pause();
        sequence.SetAutoKill(false);

    }

    public void SurfShareSpawn()
    { 
        gameObject.transform.position = RestPositionUp;
    }

    public void SetValues(float? _var = null, float? _maxRotation = null, float? _fade = null, bool? _isItFinished = null, bool? _visible = null, GameObject _position = null) 
    {
        if(_var != null)
        {
            var = (float)_var;
        }
        if (_maxRotation != null)
        {
            maxRotation = (float)_maxRotation;
        }
        if (_fade != null)
        {
            fade = (float)_fade;
        }
        if (_isItFinished != null)
        {
            isItFinished = (bool)_isItFinished;
        }
        if (_visible != null)
        {
            visible = (bool)_visible;
        }
        if (_position != null)
        {
            Position = _position;
        }

    }

    public void Play_SurfSide(bool _value)
    {
        if(SurfSide == null)
        {
            SetUp_SurfSide();
        }

        if (_value)
        {
            SurfSide.Play();
        }
        else
        {
            SurfSide.Pause();
        }
    }

    public void Play_SurfSideLasPosition(bool _value)
    {
        if (SurfSideLastPosition == null)
        {
            SetUp_SurfSideLastPosition();
        }
        if (_value)
        {
            SurfSideLastPosition.Play();
        }
        else
        {
            SurfSideLastPosition.Pause();
        }
    }

    public void Play_SurfSideTransitionBack(bool _value)
    {
        if (SurfSideTransitionBack == null)
        {
            SetUp_SurfSideTransitionBack();
        }
        if (_value)
        {
            gameObject.transform.eulerAngles = new Vector3(0f, 0f, maxRotation);
            gameObject.transform.position = RestPositionUp;
            SurfSideTransitionBack.Play();
        }
        else
        {
            SurfSideTransitionBack.Pause();
        }
    }

    public void Play_SurfReset(bool _value)
    {
        if (SurfReset == null)
        {
            SetUp_SurfReset();
        }
        if (_value)
        {
            SurfReset.Play();
        }
        else
        {
            SurfReset.Pause();
        }
    }

    public void Play_VerticalUp(bool _value)
    {
        if (VerticalUp == null)
        {
            SetUp_SurfVerticalUp();
        }
        if (_value)
        {
            fade = Mathf.Clamp(var * 2, 0, 1);
            gameObject.SetActive(true);
            VerticalUp.Play();
        }
        else
        {
            VerticalUp.Pause();
        }
    }

    public void Play_VerticalDown1(bool _value)
    {
        if (VerticalDown1 == null)
        {
            SetUp_SurfVerticalDown();
        }
        if (_value)
        {
            VerticalDown1.Play();
        }
        else
        {
            VerticalDown1.Pause();
        }
    }

    public void Play_VerticalDown2(bool _value)
    {
        if (VerticalDown2 == null)
        {
            SetUp_SurfVerticalDown2();
        }
        if (_value)
        {
            VerticalDown2.Play();
        }
        else
        {
            VerticalDown2.Pause();
        }
    }

    public void Play_SurfTransitionBackSongDown(bool _value)
    {
        if (SurfTransitionBackSongDown == null)
        {
            SetUp_SurfTransitionBackSongDown();
        }
        if (_value)
        {
            SurfTransitionBackSongDown.Play();
        }
        else
        {
            SurfTransitionBackSongDown.Pause();
        }
    }

    public void Play_SurfTransitionBackHideSong(bool _value)
    {
        if (SurfTransitionBackHideSong == null)
        {
            SetUp_SurfTransitionBackHideSong();
        }
        if (_value)
        {
            fade = Mathf.Clamp(var * 2, 0, 1);
            gameObject.SetActive(true);

            SurfTransitionBackHideSong.Play();
        }
        else
        {
            SurfTransitionBackHideSong.Pause();
        }
    }

    
    public void Play_SurfResetfOtherSongs(bool _value)
    {
        if (SurfResetOtherSongs == null)
        {
            SetUp_SurfResetOtherSongs();
        }
        if (_value)
        {
            SurfResetOtherSongs.Play();
        }
        else
        {
            SurfResetOtherSongs.Pause();
        }
    }

    public void Play_SurfBackSong(bool _value)
    {
        if (SurfTransitionBackSong == null)
        {
            SetUp_SurfTransitionBackSong();
        }
        if (_value)
        {
            gameObject.SetActive(true);
            gameObject.transform.eulerAngles = new Vector3(0f, 0f, maxRotation);
            SurfTransitionBackSong.Play();
        }
        else
        {
            SurfTransitionBackSong.Pause();
        }
    }

    public void Play_SurfTransitionOtherSongs(bool _value)
    {
        if (SurfTransitionOtherSongs == null)
        {
            SetUp_SurfTransitionOtherSongs();
        }
        if (_value)
        {
             pos = new Vector3(Position.transform.position.x, Position.transform.position.y, Position.transform.position.z);
             scale = new Vector3(Position.transform.localScale.x, Position.transform.localScale.y, Position.transform.localScale.z);
            aplha = Position.GetComponent<CanvasGroup>().alpha;
            SurfTransitionOtherSongs.Play();
        }
        else
        {
            SurfTransitionOtherSongs.Pause();
        }
    }

    public void Play_SurfAddSong(bool _value)
    {
        if (SurfAddSong == null)
        {
            SetUp_SurfAddSong();
        }
        if (_value)
        {
            fade = Mathf.Clamp(fade * 1.5f, 0, 1f);
            gameObject.SetActive(true);
            SurfAddSong.Play();
        }
        else
        {
            SurfAddSong.Pause();
        }
    }

    public void Play_SurfAddsongReset(bool _value)
    {
        if (SurfAddSongReset == null)
        {
            SetUp_SurfAddSongReset();
        }
        if (_value)
        {
            SurfAddSongReset.Play();
        }
        else
        {
            SurfAddSongReset.Pause();
        }
    }

    public void Play_CompleteAddSurfAddSong(bool _value)
    {
        if (CompleteAddSurfAddSong == null)
        {
            SetUp_CompleteSurfAddSong();
        }
        if (_value)
        {
            IsAddSongSurfDone = false;
            CompleteAddSurfAddSong.Play();
        }
        else
        {
            CompleteAddSurfAddSong.Pause();
        }
    }

    public void Play_SurfAddSongLastPosition(bool _value)
    {
        if (SurfAddSongLastPosition == null)
        {
            SetUp_SurfAddSongLastPosition();
        }
        if (_value)
        {
            SurfAddSongLastPosition.Play();
        }
        else
        {
            SurfAddSongLastPosition.Pause();
        }
    }

    public void Restart_SurfSide()
    {
        
         SurfSide.Restart();
        
    }

    public void Restart_SurfSideLastPosition()
    {

        SurfSideLastPosition.Restart();

    }

    public void Restart_SurfSideTransitionBack()
    {

        SurfSideTransitionBack.Restart();

    }
    

    public void Restart_SurfTransitionBackHideSong()
    {

        SurfTransitionBackHideSong.Restart();

    }
    public void Restart_SurfReset()
    {

        SurfReset.Restart();

    }

    public void Restart_VerticalUp()
    {

        VerticalUp.Restart();

    }

    public void Restart_VerticalDown1()
    {

        VerticalDown1.Restart();

    }

    public void Restart_VerticalDown2()
    {

        VerticalDown2.Restart();

    }

    public void Restart_SurfTransitionBackSongDown()
    {

        SurfTransitionBackSongDown.Restart();

    }

    public void Restart_SurfResetOtherSongs()
    {

        SurfResetOtherSongs.Restart();

    }

    public void Restart_SurfTransitionBackSong()
    {

        SurfTransitionBackSong.Restart();

    }

    public void Restart_SurfTransitionOtherSongs()
    {

        SurfTransitionOtherSongs.Restart();

    }

    public void Restart_SurfAddSong()
    {

        SurfAddSong.Restart();

    }

    public void Restart_SurfAddSongReset()
    {

        SurfAddSong.Restart();

    }

    public void Restart_CompleteAddSurfAddSong()
    {

        CompleteAddSurfAddSong.Restart();

    }

    public void Restart_SurfAdSongLastPosition()
    {

        SurfAddSongLastPosition.Restart();

    }

}
