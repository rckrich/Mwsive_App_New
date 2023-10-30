using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.UIElements;

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
    private Vector3 FinalPosition, RestPositionSide, RestPositionDown, RestPositionUp;

    public bool isAvailable, debug;
    public bool hasSurfResetEnd = true;

    private Tweener[] SurfSide, SurfSideLastPosition, SurfSideTransitionBack, SurfReset, VerticalUp, VerticalDown1, VerticalDown2, SurfTransitionBackSongDown, SurfResetOtherSongs, SurfTransitionBackSong;
    private Tweener[] SurfTransitionOtherSongs, SurfTransitionBackHideSong, SurfAddSong, SurfAddSongReset, CompleteAddSurfAddSong, SurfAddSongLastPosition;

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
        RestPositionDown = new Vector3(MainCanvas.transform.position.x, -2 * MainCanvas.transform.position.y, 0f);
        RestPositionUp = new Vector3(MainCanvas.transform.position.x, 3 * MainCanvas.transform.position.y, 0f);
        RestPositionSide = new Vector3(MainCanvas.transform.position.x * 4, MainCanvas.transform.position.y, 0f);
        //RestPositionLeft = new Vector2(MainCanvas.transform.position.x * -2, MainCanvas.transform.position.y);
    }

    public void ResetRestPosition()
    {
        gameObject.transform.position = RestPositionUp;
    }

    // Update is called once per frame
    private void SetUp_SurfSide()
    {
        SetPosition();
        SurfSide = new Tweener[3];
        SurfSide[0] = gameObject.transform.DOMove(new Vector2(RestPositionSide.x * var, RestPositionSide.y), SurfTransitionDuration, false).Pause();
        SurfSide[1] = gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration).Pause();
        SurfSide[2] = gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration).Pause();
        SurfSide[0].OnComplete(() => {
        if (isItFinished)
        {
          isAvailable = true; gameObject.transform.position = RestPositionUp;
        }
        });

    }

    private void SetUp_SurfSideLastPosition()
    {
        SetPosition();
        
        SurfSideLastPosition = new Tweener[3];
        SurfSideLastPosition[0] = gameObject.transform.DOMove(new Vector2(RestPositionSide.x * var, RestPositionSide.y), SurfTransitionDuration, false).Pause();
        SurfSideLastPosition[1] = gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration).Pause();
        SurfSideLastPosition[2] = gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration).Pause();
        SurfSideLastPosition[0].OnComplete(() => {

            Play_SurfSideTransitionBack();
            SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().OnCallback_ResetSideAnimation();

        });
        
        
    }

    private void SetUp_SurfSideTransitionBack()
    {
        SetPosition();
        
        SurfSideTransitionBack = new Tweener[5];

        SurfSideTransitionBack[0] = gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false).Pause();
        SurfSideTransitionBack[1] = gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration).Pause();
        SurfSideTransitionBack[2] = gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration).Pause();
        SurfSideTransitionBack[3] = gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration).Pause();
        SurfSideTransitionBack[4] =gameObject.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration).Pause();

        SurfSideTransitionBack[0].OnComplete(() => {

            try
            {
                SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().HasSideScrollEnded = true;
                SurfController.instance.ReturnCurrentView().GetComponent<PF_SurfManager>().ResetChallengeAnimation();
            }
            catch (System.NullReferenceException)
            {
                
            }
            });

    }

    

    private void SetUp_SurfReset()
    {

        
        
        SurfReset = new Tweener[3];

        SurfReset[0] = gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false).Pause();
        SurfReset[1] =gameObject.transform.DORotate(new Vector3(0f, 0f, 0f), SurfTransitionDuration).Pause();
        SurfReset[2] =gameObject.GetComponent<CanvasGroup>().DOFade(1, SurfTransitionDuration).Pause();
        SurfReset[0].OnComplete(() => {
            hasSurfResetEnd = true;

        });
    }

    private void SetUp_SurfVerticalUp()
    {
        SetPosition();
        
        VerticalUp = new Tweener[3];

        VerticalUp[0] = gameObject.transform.DOMove(new Vector2(RestPositionUp.x, RestPositionUp.y * var), SurfTransitionDuration*2, false).Pause();
        VerticalUp[1] = gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration).Pause();
        VerticalUp[2] = gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration).Pause();

        VerticalUp[0].OnComplete(() => {
            if (isItFinished)
            {
                isAvailable = true;
                gameObject.transform.DORotate(new Vector3(0f, 0f, 0f), SurfTransitionDuration / 2);
            }
            

        });
    }

    private void SetUp_SurfVerticalDown()
    {
        
        VerticalDown1 = new Tweener[2];
        VerticalDown1[0] = gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration).Pause();
        VerticalDown1[1] = gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration).Pause();

    }

    private void SetUp_SurfVerticalDown2()
    {
        SetPosition();

        
        VerticalDown2 = new Tweener[3];

        VerticalDown2[0] = gameObject.transform.DOMove(new Vector2(RestPositionDown.x, RestPositionDown.y * var), SurfTransitionDuration + .5f, false).Pause();
        VerticalDown2[1] = gameObject.transform.DORotate(new Vector3(0f, 0f, maxRotation * var), SurfTransitionDuration).Pause();
        VerticalDown2[2] = gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration).Pause();


    }

    private void SetUp_SurfTransitionBackSongDown()
    {

        //DOTween.Kill(GA);
        
        SurfTransitionBackSongDown = new Tweener[4];

        SurfTransitionBackSongDown[0] = gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration).Pause();
        SurfTransitionBackSongDown[1] = gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration).Pause();
        SurfTransitionBackSongDown[2] = gameObject.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration).Pause();
        SurfTransitionBackSongDown[3] = gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
    }

    private void SetUp_SurfResetOtherSongs()
    {

        
        SurfResetOtherSongs = new Tweener[3];

        SurfResetOtherSongs[0] = gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false).Pause();
        SurfResetOtherSongs[1] = gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration).Pause();
        SurfResetOtherSongs[2] = gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration).Pause();
        SurfResetOtherSongs[0].OnComplete(() => { if(visible)
            {
                isAvailable = false;

            }
            else
            {
                isAvailable = true;
            }
            


        });
    }

    private void SetUp_SurfTransitionBackSong()
    {
        
        SurfTransitionBackSong = new Tweener[4];
        SurfTransitionBackSong[0] = gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false).Pause();
        SurfTransitionBackSong[1] = gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration).Pause();
        SurfTransitionBackSong[2] = gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration).Pause();
        SurfTransitionBackSong[3] = gameObject.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration).Pause();

    }

    private void SetUp_SurfTransitionOtherSongs()
    {
        
        SurfTransitionOtherSongs = new Tweener[4];

        SurfTransitionOtherSongs[0] = gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false).Pause();
        SurfTransitionOtherSongs[1] = gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha*fade, SurfTransitionDuration).Pause();
        SurfTransitionOtherSongs[2] = gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);
        SurfTransitionOtherSongs[3] = gameObject.transform.DORotate(new Vector3(0, 0, 0), SurfTransitionDuration).Pause();


       
        
    }

    private void SetUp_SurfTransitionBackHideSong()
    {

        
        SurfTransitionBackHideSong = new Tweener[3];
        SurfTransitionBackHideSong[0] = gameObject.transform.DOMove(Position.transform.position, SurfTransitionDuration, false).Pause();
        SurfTransitionBackHideSong[1] = gameObject.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha * fade, SurfTransitionDuration).Pause();
        SurfTransitionBackHideSong[2] = gameObject.transform.DOScale(Position.transform.localScale, SurfTransitionDuration).Pause();
        
    }

    private void SetUp_SurfAddSong()
    {
        SurfAddSong = new Tweener[2];

        SurfAddSong[0] = gameObject.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration).Pause();
        SurfAddSong[1] = gameObject.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration).Pause();
        SurfAddSong[0].OnComplete(() => { IsAddSongSurfDone = true; });
    }

    private void SetUp_SurfAddSongReset()
    {        
        SurfAddSongReset = new Tweener[1];

        SurfAddSongReset[0] = gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.3F).Pause();
        SurfAddSongReset[0].OnComplete(() => { gameObject.SetActive(false); });



    }

    private void SetUp_CompleteSurfAddSong()
    {
        
        CompleteAddSurfAddSong = new Tweener[2];

        
        CompleteAddSurfAddSong[0] = gameObject.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration*2).Pause();
        CompleteAddSurfAddSong[1] = gameObject.GetComponent<CanvasGroup>().DOFade(0, SurfTransitionDuration * 2.5f).Pause();
        CompleteAddSurfAddSong[0].OnComplete(() => { IsAddSongSurfDone = true; gameObject.SetActive(false);
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
            gameObject.transform.localScale = new Vector3(0, 0, 0);
        });
        

    }

    private void SetUp_SurfAddSongLastPosition()
    {

        //DOTween.Complete(GA);
        
        SurfAddSongLastPosition = new Tweener[3];

        SurfAddSongLastPosition[0] = gameObject.transform.DOScale(new Vector3(1, 1, 1) * fade, SurfTransitionDuration).Pause();
        SurfAddSongLastPosition[1] = gameObject.GetComponent<CanvasGroup>().DOFade(0, SurfTransitionDuration * 2).Pause();
        SurfAddSongLastPosition[0].OnComplete(() => { gameObject.transform.localScale = new Vector3(0, 0, 0); gameObject.GetComponent<CanvasGroup>().alpha = 0; });


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

    public void Play_SurfSide()
    {
        if(SurfSide == null)
        {
            SetUp_SurfSide();
        }
        else
        {
            Restart_SurfSide();
        }

        foreach (Tweener item in SurfSide)
        {
            item.SetAutoKill(false);
            item.Play();
        } 
    }

    public void Play_SurfSideLasPosition()
    {
        if (SurfSideLastPosition == null)
        {
            SetUp_SurfSideLastPosition();
        }
        else
        {
            Restart_SurfSideLastPosition();
        }

        foreach (Tweener item in SurfSideLastPosition)
        {
            item.SetAutoKill(false);
            item.Play();
        }
        
    }

    public void Play_SurfSideTransitionBack()
    {
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, maxRotation);
        gameObject.transform.position = RestPositionUp;
        if (SurfSideTransitionBack == null)
        {
            SetUp_SurfSideTransitionBack();
        }
        else
        {
            Restart_SurfSideTransitionBack();
        }
        foreach (Tweener item in SurfSideTransitionBack)
        {
            item.SetAutoKill(false);
            item.Play();
        }
        
    }

    public void Play_SurfReset()
    {
        hasSurfResetEnd = false;
        if (SurfReset == null)
        {
            SetUp_SurfReset();
        }
        else
        {
            Restart_SurfReset();
        }
        
        DOTween.Complete(gameObject);
        foreach (Tweener item in SurfReset)
        {
            item.SetAutoKill(false);
            item.Play();
        }
    }

    public void Play_VerticalUp()
    {
        
        isAvailable = false;
        if (VerticalUp == null)
        {
            
            SetUp_SurfVerticalUp();
        }
        else
        {
            Restart_VerticalUp();
        }
        Debug.Log("UP");
        if (hasSurfResetEnd)
        {
            foreach (Tweener item in VerticalUp)
            {
                item.SetAutoKill(false);
                item.Play();
            }
        }
        
    }

    public void Play_VerticalDown1()
    {
        if (VerticalDown1 == null)
        {
            SetUp_SurfVerticalDown();
        }
        else
        {
            Restart_VerticalDown1();
        }
        foreach (Tweener item in VerticalDown1)
        {
            item.SetAutoKill(false);
            item.Play();
        }
    }

    public void Play_VerticalDown2()
    {
        if (VerticalDown2 == null)
        {
            SetUp_SurfVerticalDown2();
        }
        else
        {
            Restart_VerticalDown2();
        }
        foreach (Tweener item in VerticalDown2)
        {
            item.SetAutoKill(false);
            item.Play();
        }
    }

    public void Play_SurfTransitionBackSongDown()
    {
        if (SurfTransitionBackSongDown == null)
        {
            SetUp_SurfTransitionBackSongDown();
        }
        else
        {
            Restart_SurfTransitionBackSongDown();
        }
        foreach (Tweener item in SurfTransitionBackSongDown)
        {
            item.SetAutoKill(false);
            item.Play();
        }
    }

    public void Play_SurfTransitionBackHideSong()
    {
        fade = Mathf.Clamp(var * 2, 0, 1);
        
        if (SurfTransitionBackHideSong == null)
        {
            SetUp_SurfTransitionBackHideSong();
        }
        else
        {
            Restart_SurfTransitionBackHideSong();
        }
        
        foreach (Tweener item in SurfTransitionBackHideSong)
        {
            item.SetAutoKill(false);
            item.Play();
        }
        
    }

    
    public void Play_SurfResetfOtherSongs()
    {
        if (SurfResetOtherSongs == null)
        {
            SetUp_SurfResetOtherSongs();
        }
        else
        {
            Restart_SurfResetOtherSongs();
        }
        foreach (Tweener item in SurfResetOtherSongs)
        {
            item.SetAutoKill(false);
            item.Play();
        }
    }

    public void Play_SurfBackSong()
    {
        isAvailable = false;
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, maxRotation);
        if (SurfTransitionBackSong == null)
        {
            SetUp_SurfTransitionBackSong();
        }
        else
        {
            Restart_SurfTransitionBackSong();
        }
        
        foreach (Tweener item in SurfTransitionBackSong)
        {
            item.SetAutoKill(false);
            item.Play();
        }
        
    }

    public void Play_SurfTransitionOtherSongs()
    {
        
        fade = Mathf.Clamp(var * 2, 0, 1);
        if (SurfTransitionOtherSongs == null)
        {
            SetUp_SurfTransitionOtherSongs();
        }
        else
        {
            Restart_SurfTransitionOtherSongs();
        }
        foreach (Tweener item in SurfTransitionOtherSongs)
        {
            item.SetAutoKill(false);
            item.Play();
        }
        
    }

    public void Play_SurfAddSong()
    {
        fade = Mathf.Clamp(fade * 1.5f, 0, 1f);
        gameObject.SetActive(true);


        if (IsAddSongSurfDone)
        {
            IsAddSongSurfDone = false;
            if (SurfAddSong == null)
            {
                SetUp_SurfAddSong();
            }
            else
            {
                Restart_SurfAddSong();
            }
            foreach (Tweener item in SurfAddSong)
            {
                item.SetAutoKill(false);
                item.Play();
            }
            
        }
 
        
    }

    public void Play_SurfAddsongReset()
    {
        gameObject.SetActive(true);
        if (SurfAddSongReset == null)
        {
            SetUp_SurfAddSongReset();
        }
        else
        {
            Restart_SurfAddSongReset();
        }
        foreach (Tweener item in SurfAddSongReset)
        {
            item.SetAutoKill(false);
            item.Play();
        }
    }

    public void Play_CompleteAddSurfAddSong()
    {
        IsAddSongSurfDone = false;
        gameObject.SetActive(true);

        fade = 1.5f;
        if (CompleteAddSurfAddSong == null)
        {
            SetUp_CompleteSurfAddSong();
        }
        else
        {
            Restart_CompleteAddSurfAddSong();
        }
        
        foreach (Tweener item in CompleteAddSurfAddSong)
        {
            item.SetAutoKill(false);
            item.Play();
        }
        
    }

    public void Play_SurfAddSongLastPosition()
    {
        if (SurfAddSongLastPosition == null)
        {
            SetUp_SurfAddSongLastPosition();
        }
        else
        {
            Restart_SurfAdSongLastPosition();
        }
        foreach (Tweener item in SurfAddSongLastPosition)
        {
            item.SetAutoKill(false);
            item.Play();
        }
    }

    private void Restart_SurfSide()
    {
        SurfSide[0].ChangeValues(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0f), new Vector3(RestPositionSide.x * var, RestPositionSide.y, 0f));
        SurfSide[1].ChangeValues(gameObject.transform.eulerAngles, new Vector3(0f, 0f, maxRotation * var));
        SurfSide[2].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, fade);

    }

    private void Restart_SurfSideLastPosition()
    {
        SurfSideLastPosition[0].ChangeValues(gameObject.transform.position, new Vector3(RestPositionSide.x * var, RestPositionSide.y, 0f));
        SurfSideLastPosition[1].ChangeValues(gameObject.transform.eulerAngles, new Vector3(0f, 0f, maxRotation * var));
        SurfSideLastPosition[2].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, fade);

    }

    private void Restart_SurfSideTransitionBack()
    {

        
        SurfSideTransitionBack[0].ChangeValues(gameObject.transform.position, Position.transform.position);
        SurfSideTransitionBack[1].ChangeValues(gameObject.transform.eulerAngles, new Vector3(0f, 0f, maxRotation * var));
        SurfSideTransitionBack[2].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, Position.GetComponent<CanvasGroup>().alpha);
        SurfSideTransitionBack[3].ChangeValues(gameObject.transform.localScale, Position.transform.localScale);
        SurfSideTransitionBack[4].ChangeValues(gameObject.transform.eulerAngles, new Vector3(0, 0, 0));
        

    }


    private void Restart_SurfTransitionBackHideSong()
    {

        SurfTransitionBackHideSong[0].ChangeValues(gameObject.transform.position, Position.transform.position);
        SurfTransitionBackHideSong[1].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, Position.GetComponent<CanvasGroup>().alpha);
        SurfTransitionBackHideSong[2].ChangeValues(gameObject.transform.localScale, Position.transform.localScale);
        


    }
    private void Restart_SurfReset()
    {

        
        SurfReset[0].ChangeValues(gameObject.transform.position, Position.transform.position);
        SurfReset[1].ChangeValues(gameObject.transform.eulerAngles, new Vector3(0f, 0f, 0f));
        SurfReset[2].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, (System.Single)1);

    }

    private void Restart_VerticalUp()
    {

        
        VerticalUp[0].ChangeValues(gameObject.transform.position, new Vector3(RestPositionUp.x, RestPositionUp.y * var, 0f));
        VerticalUp[1].ChangeValues(gameObject.transform.eulerAngles, new Vector3(0f, 0f, maxRotation * var));
        VerticalUp[2].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, fade);

    }

    private void Restart_VerticalDown1()
    {
        
        VerticalDown1[0].ChangeValues(gameObject.transform.eulerAngles, new Vector3(0f, 0f, maxRotation * var));
        VerticalDown1[1].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, fade);

    }

    private void Restart_VerticalDown2()
    {

        VerticalDown2[0].ChangeValues(gameObject.transform.position, new Vector2(RestPositionDown.x, RestPositionDown.y * var));
        VerticalDown2[1].ChangeValues(gameObject.transform.eulerAngles, new Vector3(0f, 0f, maxRotation * var));
        VerticalDown2[2].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, fade);


    }

    private void Restart_SurfTransitionBackSongDown()
    {

        
        SurfTransitionBackSongDown[0].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, Position.GetComponent<CanvasGroup>().alpha);
        SurfTransitionBackSongDown[1].ChangeValues(gameObject.transform.localScale, Position.transform.localScale);
        SurfTransitionBackSongDown[2].ChangeValues(gameObject.transform.eulerAngles, new Vector3(0, 0, 0));
        SurfTransitionBackSongDown[3].ChangeValues(gameObject.transform.position, Position.transform.position);

    }

    private void Restart_SurfResetOtherSongs()
    {

        SurfResetOtherSongs[0].ChangeValues(gameObject.transform.position, Position.transform.position);
        SurfResetOtherSongs[1].ChangeValues(gameObject.transform.localScale, Position.transform.localScale);
        SurfResetOtherSongs[2].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, Position.GetComponent<CanvasGroup>().alpha);
    }

    private void Restart_SurfTransitionBackSong()
    {

        SurfTransitionBackSong[0].ChangeValues(gameObject.transform.position, Position.transform.position);
        SurfTransitionBackSong[1].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, Position.GetComponent<CanvasGroup>().alpha);
        SurfTransitionBackSong[2].ChangeValues(gameObject.transform.localScale, Position.transform.localScale);

        SurfTransitionBackSong[3].ChangeValues(gameObject.transform.eulerAngles, new Vector3(0, 0, 0));
        

    }

    private void Restart_SurfTransitionOtherSongs()
    {

        SurfTransitionOtherSongs[0].ChangeValues(gameObject.transform.position, Position.transform.position);
        SurfTransitionOtherSongs[1].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, Position.GetComponent<CanvasGroup>().alpha);
        SurfTransitionOtherSongs[2].ChangeValues(gameObject.transform.localScale, Position.transform.localScale);

        SurfTransitionOtherSongs[3].ChangeValues(gameObject.transform.eulerAngles, new Vector3(0, 0, 0));

    }

    private void Restart_SurfAddSong()
    {

        
        SurfAddSong[0].ChangeValues(gameObject.transform.localScale, new Vector3(1, 1, 1) * fade);
        SurfAddSong[1].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, fade);
        

    }

    private void Restart_SurfAddSongReset()
    {

        SurfAddSongReset[0].ChangeValues(gameObject.transform.localScale, new Vector3(0, 0, 0));

    }

    private void Restart_CompleteAddSurfAddSong()
    {

        CompleteAddSurfAddSong[0].ChangeValues(gameObject.transform.localScale, new Vector3(1, 1, 1) * fade);
        CompleteAddSurfAddSong[1].ChangeValues((System.Single)gameObject.GetComponent<CanvasGroup>().alpha, (System.Single)0);

    }

    private void Restart_SurfAdSongLastPosition()
    {


        SurfAddSongLastPosition[0].ChangeValues(gameObject.transform.localScale, new Vector3(1, 1, 1) * fade);
        SurfAddSongLastPosition[1].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, 0);


    }

}
