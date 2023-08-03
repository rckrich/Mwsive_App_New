using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIAniManager : MonoBehaviour
{
    public GameObject MainCanvas;
    public float MoveTransitionDuration =0.5f;
    public float ScaleTransitionDuration =1f;
    public float FadeTransitionDuration = 1f;
    public float ColorTransitionDuration = 0.5f;
    public float SurfTransitionDuration = 0.5f;
    public enum AnimationTypeCurves {Flash, InBack, InBounce, InCirc, InCubic, InElastic, InExpo, InFlash, InOutBack, InOutBounc, InOutCirc, InOutCubic, InOutExpo, InOutFlash, InOutQuad, InOutQuart, InOutQuint, InOutSine, InQuad, InQuint, InSine, Linear, OutBack, OutBounce, OutCirc, OutCubic, OutElastic, OutExpo, OutFlash, OutQuad, OutQuart, OutQuint, OutSine, UnSet}
    public AnimationTypeCurves AnimationMove;
    public AnimationTypeCurves AnimationScale;
    public AnimationTypeCurves AnimationFade;
    


    private static UIAniManager _instance;
    private Vector2 FinalPosition, RestPositionSide, RestPositionDown, RestPositionUp, RestPositionLeft; 
    private Ease _AnimationMove;
    private Ease _AnimationScale;
    private Ease _AnimationFade;
    private bool ChangeColorIsOn = true;
    public bool IsAddSongSurfDone = true;
    


    public static UIAniManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIAniManager>();
            }
            return _instance;
        }
    }

    void Start()
    {
        DOTween.SetTweensCapacity(1250, 50);
        SetPosition();
        switch(AnimationMove){
            case AnimationTypeCurves.Flash:
                _AnimationMove = Ease.Flash;
                break;
            case AnimationTypeCurves.InBack:
                _AnimationMove = Ease.InBack;
                break;
            case AnimationTypeCurves.InBounce:
                _AnimationMove = Ease.InBounce;
                break;
            case AnimationTypeCurves.InCirc:
                _AnimationMove = Ease.InCirc;
                break;
            case AnimationTypeCurves.InCubic:
                _AnimationMove = Ease.InCubic;
                break;
            case AnimationTypeCurves.InElastic:
                _AnimationMove = Ease.InElastic;
                break;
            case AnimationTypeCurves.InExpo:
                _AnimationMove = Ease.InExpo;
                break;
            case AnimationTypeCurves.InFlash:
                _AnimationMove = Ease.InFlash;
                break;
            case AnimationTypeCurves.InOutBack:
                _AnimationMove = Ease.InOutBack;
                break;
            case AnimationTypeCurves.InOutBounc:
                _AnimationMove = Ease.InOutBounce;
                break;
            case AnimationTypeCurves.InOutCirc:
                _AnimationMove = Ease.InOutCirc;
                break;
            case AnimationTypeCurves.InOutCubic:
                _AnimationMove = Ease.InOutCubic;
                break;
            case AnimationTypeCurves.InOutExpo:
                _AnimationMove = Ease.InOutExpo;
                break;
            case AnimationTypeCurves.InOutFlash:
                _AnimationMove = Ease.InOutFlash;
                break;
            case AnimationTypeCurves.InOutQuad:
                _AnimationMove = Ease.InOutQuad;
                break;
            case AnimationTypeCurves.InOutQuart:
                _AnimationMove = Ease.InOutQuart;
                break;
            case AnimationTypeCurves.OutQuint:
                _AnimationMove = Ease.OutQuint;
                break;
            case AnimationTypeCurves.OutSine:
                _AnimationMove = Ease.OutSine;
                break;
            case AnimationTypeCurves.UnSet:
                _AnimationMove = Ease.Unset;
                break;
            default:
                _AnimationMove = Ease.Unset;
                break;
            
        }
        switch(AnimationScale){
            case AnimationTypeCurves.Flash:
                _AnimationScale = Ease.Flash;
                break;
            case AnimationTypeCurves.InBack:
                _AnimationScale = Ease.InBack;
                break;
            case AnimationTypeCurves.InBounce:
                _AnimationScale = Ease.InBounce;
                break;
            case AnimationTypeCurves.InCirc:
                _AnimationScale = Ease.InCirc;
                break;
            case AnimationTypeCurves.InCubic:
                _AnimationScale = Ease.InCubic;
                break;
            case AnimationTypeCurves.InElastic:
                _AnimationScale = Ease.InElastic;
                break;
            case AnimationTypeCurves.InExpo:
                _AnimationScale = Ease.InExpo;
                break;
            case AnimationTypeCurves.InFlash:
                _AnimationScale = Ease.InFlash;
                break;
            case AnimationTypeCurves.InOutBack:
                _AnimationScale = Ease.InOutBack;
                break;
            case AnimationTypeCurves.InOutBounc:
                _AnimationScale = Ease.InOutBounce;
                break;
            case AnimationTypeCurves.InOutCirc:
                _AnimationScale = Ease.InOutCirc;
                break;
            case AnimationTypeCurves.InOutCubic:
                _AnimationScale = Ease.InOutCubic;
                break;
            case AnimationTypeCurves.InOutExpo:
                _AnimationScale = Ease.InOutExpo;
                break;
            case AnimationTypeCurves.InOutFlash:
                _AnimationScale = Ease.InOutFlash;
                break;
            case AnimationTypeCurves.InOutQuad:
                _AnimationScale = Ease.InOutQuad;
                break;
            case AnimationTypeCurves.InOutQuart:
                _AnimationScale = Ease.InOutQuart;
                break;
            case AnimationTypeCurves.OutQuint:
                _AnimationScale = Ease.OutQuint;
                break;
            case AnimationTypeCurves.OutSine:
                _AnimationScale = Ease.OutSine;
                break;
            case AnimationTypeCurves.UnSet:
                _AnimationScale = Ease.Unset;
                break;
            default:
                _AnimationScale = Ease.Unset;
                break;
            
        }
        switch(AnimationFade){
            case AnimationTypeCurves.Flash:
                _AnimationFade = Ease.Flash;
                break;
            case AnimationTypeCurves.InBack:
                _AnimationFade = Ease.InBack;
                break;
            case AnimationTypeCurves.InBounce:
                _AnimationFade = Ease.InBounce;
                break;
            case AnimationTypeCurves.InCirc:
                _AnimationFade = Ease.InCirc;
                break;
            case AnimationTypeCurves.InCubic:
                _AnimationFade = Ease.InCubic;
                break;
            case AnimationTypeCurves.InElastic:
                _AnimationFade = Ease.InElastic;
                break;
            case AnimationTypeCurves.InExpo:
                _AnimationFade = Ease.InExpo;
                break;
            case AnimationTypeCurves.InFlash:
                _AnimationFade = Ease.InFlash;
                break;
            case AnimationTypeCurves.InOutBack:
                _AnimationFade = Ease.InOutBack;
                break;
            case AnimationTypeCurves.InOutBounc:
                _AnimationFade = Ease.InOutBounce;
                break;
            case AnimationTypeCurves.InOutCirc:
                _AnimationFade = Ease.InOutCirc;
                break;
            case AnimationTypeCurves.InOutCubic:
                _AnimationFade = Ease.InOutCubic;
                break;
            case AnimationTypeCurves.InOutExpo:
                _AnimationFade = Ease.InOutExpo;
                break;
            case AnimationTypeCurves.InOutFlash:
                _AnimationFade = Ease.InOutFlash;
                break;
            case AnimationTypeCurves.InOutQuad:
                _AnimationFade = Ease.InOutQuad;
                break;
            case AnimationTypeCurves.InOutQuart:
                _AnimationFade = Ease.InOutQuart;
                break;
            case AnimationTypeCurves.OutQuint:
                _AnimationFade = Ease.OutQuint;
                break;
            case AnimationTypeCurves.OutSine:
                _AnimationFade = Ease.OutSine;
                break;
            case AnimationTypeCurves.UnSet:
                _AnimationFade = Ease.Unset;
                break;
            default:
                _AnimationFade = Ease.Unset;
                break;
            
        }
    }

    void SetPosition(){
        FinalPosition = MainCanvas.transform.position;
        RestPositionDown = new Vector2(MainCanvas.transform.position.x, -2*MainCanvas.transform.position.y);
        RestPositionUp = new Vector2(MainCanvas.transform.position.x, 2*MainCanvas.transform.position.y);
        RestPositionSide = new Vector2(MainCanvas.transform.position.x*4, MainCanvas.transform.position.y);
        RestPositionLeft = new Vector2(MainCanvas.transform.position.x*-2, MainCanvas.transform.position.y);
    }

    void SetPosition(GameObject GA){
        FinalPosition = GA.transform.position;
        RestPositionDown = new Vector2(GA.transform.position.x, -2*GA.transform.position.y);
        RestPositionUp = new Vector2(GA.transform.position.x, GA.transform.position.y*2);
        RestPositionSide = new Vector2(GA.transform.position.x*4, GA.transform.position.y);
        RestPositionLeft = new Vector2(GA.transform.position.x*-4, GA.transform.position.y);
    }

    void SetPosition(GameObject GA, float OffSet){
        FinalPosition = GA.transform.position;
        RestPositionDown = new Vector2(GA.transform.position.x, GA.transform.position.y-OffSet);
        RestPositionUp = new Vector2(GA.transform.position.x, GA.transform.position.y+OffSet);
        RestPositionSide = new Vector2(GA.transform.position.x+OffSet, GA.transform.position.y);
        RestPositionLeft = new Vector2(GA.transform.position.x-OffSet, GA.transform.position.y);
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeColorRainbow(GameObject GA){
        if(ChangeColorIsOn){
            GA.GetComponent<Image>().DOColor(Random.ColorHSV(), ColorTransitionDuration).OnComplete(() => {ChangeColorRainbow(GA);});
        }else{
            GA.GetComponent<Image>().DOColor(Color.white, ColorTransitionDuration);
            ChangeColorIsOn = true;
        }


 
        //GA.GetComponent<Material>().DOColor(Random.ColorHSV(), ColorTransitionDuration).OnComplete(() => {ChangeColorRainbow(GA);});
    }

    public void KillChangeColor(){
        ChangeColorIsOn = false;
    }
    
    //Animaciones de Pantallas --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void FadeIn(GameObject GA){
        GA.GetComponent<CanvasGroup>().alpha = 0;
        GA.SetActive(true);
        GA.GetComponent<CanvasGroup>().DOFade(1, FadeTransitionDuration).SetEase(_AnimationFade);
        
    }


    public void FadeIn(GameObject GA, float TransitionDuration){
        GA.GetComponent<CanvasGroup>().alpha = 0;
        GA.SetActive(true);
        GA.GetComponent<CanvasGroup>().DOFade(1, TransitionDuration).SetEase(_AnimationFade);
        
    }
    public void FadeOut(GameObject GA, float TransitionDuration){
        GA.GetComponent<CanvasGroup>().DOFade(0, TransitionDuration).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationFade);
    }

    public void FadeOut(GameObject GA){
        GA.GetComponent<CanvasGroup>().DOFade(0, FadeTransitionDuration).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationFade);
    }


    public void SideTransitionExitCenter(GameObject GA){
        SetPosition();
        GA.transform.DOMove(RestPositionSide, MoveTransitionDuration, false).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationMove);
    }

    public void SideLeftTransitionExitCenter(GameObject GA){
        SetPosition();
        GA.transform.DOMove(RestPositionLeft, MoveTransitionDuration, false).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationMove);
    }
    public void SideLeftTransitionExitCenter(GameObject GA, float TransitionDuration){
        SetPosition();
        GA.transform.DOMove(RestPositionLeft, TransitionDuration, false).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationMove);
    }
    public void SideTransitionExitCenter(GameObject GA, float TransitionDuration){
        SetPosition();
        GA.transform.DOMove(RestPositionSide, TransitionDuration, false).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationMove);
    }

    public void SideTransitionExitCustomLocation(GameObject GA){
        SetPosition(GA);
        GA.transform.DOMove(RestPositionSide, MoveTransitionDuration, false).OnComplete(() => {GA.SetActive(false); GA.transform.position = FinalPosition;}).SetEase(_AnimationMove);
    }
    public void SideTransitionExitCustomLocation(GameObject GA, float TransitionDuration){
        SetPosition(GA);
        GA.transform.DOMove(RestPositionSide, TransitionDuration, false).OnComplete(() => {GA.SetActive(false); GA.transform.position = FinalPosition;}).SetEase(_AnimationMove);
    }

    public void SideTransitionEnterCustomLocation(GameObject GA){
        SetPosition(GA);
        GA.transform.position = RestPositionSide;
        GA.SetActive(true);
        GA.transform.DOMove(FinalPosition, MoveTransitionDuration, false).SetEase(_AnimationMove);
    }
    public void SideTransitionEnterCustomLocation(GameObject GA, float TransitionDuration){
        SetPosition(GA);
        GA.transform.position = RestPositionSide;
        GA.SetActive(true);
        GA.transform.DOMove(FinalPosition, TransitionDuration, false).SetEase(_AnimationMove);
    }


    public void SideTransitionEnterCenter(GameObject GA){
        SetPosition();
        GA.transform.position = RestPositionSide;
        GA.SetActive(true);
        GA.transform.DOMove(FinalPosition, MoveTransitionDuration, false).SetEase(_AnimationMove);
    }

    public void SideLeftTransitionEnterCenter(GameObject GA){
        SetPosition();
        GA.transform.position = RestPositionLeft;
        GA.SetActive(true);
        GA.transform.DOMove(FinalPosition, MoveTransitionDuration, false).SetEase(_AnimationMove);
    }
    public void SideLeftTransitionEnterCenter(GameObject GA, float TransitionDuration){
        SetPosition();
        GA.transform.position = RestPositionLeft;
        GA.SetActive(true);
        GA.transform.DOMove(FinalPosition, TransitionDuration, false).SetEase(_AnimationMove);
    }
    public void SideTransitionEnterCenter(GameObject GA, float TransitionDuration){
        SetPosition();
        GA.transform.position = RestPositionSide;
        GA.SetActive(true);
        GA.transform.DOMove(FinalPosition, TransitionDuration, false).SetEase(_AnimationMove);
    }
    public void VerticalTransitionToCustomPosition(GameObject GA, GameObject Position){
        GA.transform.DOMove(Position.transform.position, MoveTransitionDuration, false).SetEase(_AnimationMove);
    }
    public void VerticalTransitionToCustomPosition(GameObject GA, GameObject Position, GameObject CompleteGA, bool IsVisible){
        if(IsVisible ==true){
            GA.transform.DOMove(Position.transform.position, MoveTransitionDuration, false).OnComplete(() => {CompleteGA.SetActive(IsVisible);}).SetEase(_AnimationFade);
        }else{
            CompleteGA.SetActive(IsVisible);
            GA.transform.DOMove(Position.transform.position, MoveTransitionDuration, false).SetEase(_AnimationFade);
        }
        
    }


    public void VerticalFadeTransitionEnterCenter(GameObject GA){
        GA.GetComponent<CanvasGroup>().alpha = 0;
        SetPosition();
        GA.transform.position = RestPositionDown;
        GA.SetActive(true);
        GA.GetComponent<CanvasGroup>().DOFade(1f, FadeTransitionDuration).SetEase(_AnimationFade);
        GA.transform.DOMove(FinalPosition, MoveTransitionDuration, false).SetEase(_AnimationMove);
    }
    public void VerticalFadeTransitionEnterCenter(GameObject GA, float TransitionDuration){
        GA.GetComponent<CanvasGroup>().alpha = 0;
        SetPosition();
        GA.transform.position = RestPositionDown;
        GA.SetActive(true);
        GA.GetComponent<CanvasGroup>().DOFade(1f, TransitionDuration).SetEase(_AnimationFade);
        GA.transform.DOMove(FinalPosition, TransitionDuration, false).SetEase(_AnimationMove);
    }

    public void VerticalFadeTransitionExitCenterDown(GameObject GA){
        SetPosition();
        GA.GetComponent<CanvasGroup>().DOFade(0f, FadeTransitionDuration).SetEase(_AnimationFade);
        
        GA.transform.DOMove(RestPositionDown, MoveTransitionDuration, false).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationFade);    
    }
    public void VerticalFadeTransitionExitCenterDown(GameObject GA, float TransitionDuration){
        SetPosition();
        GA.GetComponent<CanvasGroup>().DOFade(0f, TransitionDuration).SetEase(_AnimationFade);
        
        GA.transform.DOMove(RestPositionDown, TransitionDuration, false).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationFade);    
    }

    public void VerticalFadeTransitionExitCenterUp(GameObject GA){
        SetPosition();
        GA.GetComponent<CanvasGroup>().DOFade(0f, FadeTransitionDuration* 0.2f);
        
        GA.transform.DOMove(RestPositionUp, MoveTransitionDuration, false).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationFade); 
        
    }
    
    public void VerticalFadeTransitionExitCenterUp(GameObject GA, float TransitionDuration){
        SetPosition();
        GA.GetComponent<CanvasGroup>().DOFade(0f, TransitionDuration* 0.2f);
        
        GA.transform.DOMove(RestPositionUp, TransitionDuration, false).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationFade); 
        
    }

    public void VerticalTransitionEnterCenter(GameObject GA){
        SetPosition();
        GA.transform.position = RestPositionDown;
        GA.SetActive(true);
        GA.transform.DOMove(FinalPosition, MoveTransitionDuration, false).SetEase(_AnimationMove);
    }

    public void VerticalTransitionEnterCenter(GameObject GA, float TransitionDuration){
        SetPosition();
        GA.transform.position = RestPositionDown;
        GA.SetActive(true);
        GA.transform.DOMove(FinalPosition, TransitionDuration, false).SetEase(_AnimationMove);
    }

    public void VerticalTransitionExitCenter(GameObject GA){
        SetPosition();
        GA.transform.DOMove(RestPositionDown, MoveTransitionDuration, false).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationMove);
        
    }

    
    public void VerticalTransitionExitCenter(GameObject GA, float TransitionDuration){
        SetPosition();
        GA.transform.DOMove(RestPositionDown, TransitionDuration, false).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationMove);
        
    }

    public void ScaleAnimationEnter(GameObject GA){
        GA.transform.localScale = new Vector3(0,0,0);
        GA.SetActive(true);
        GA.transform.DOScale(new Vector3(1,1,1), ScaleTransitionDuration).SetEase(_AnimationScale);
    }

    public void ScaleAnimationEnter(GameObject GA, float TransitionDuration){
        GA.transform.localScale = new Vector3(0, 0, 0);
        GA.SetActive(true);
        GA.transform.DOScale(new Vector3(1,1,1), TransitionDuration).SetEase(_AnimationScale);
    }
    public void ScaleAnimationExit(GameObject GA){

        GA.transform.DOScale(new Vector3(0,0,0), ScaleTransitionDuration).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationScale);
    }
    public void ScaleAnimationExit(GameObject GA, float TransitionDuration){

        GA.transform.DOScale(new Vector3(0,0,0), TransitionDuration).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationScale);
    }


    public void PopUpScaleEnter(GameObject GA){
        SetPosition();
        GA.transform.position = FinalPosition;
        GA.transform.localScale = new Vector3(0,0,0);
        GA.SetActive(true);
        
        GA.transform.DOScale(new Vector3(1,1,1), ScaleTransitionDuration).SetEase(_AnimationScale);
    }
    public void PopUpScaleEnter(GameObject GA, float TransitionDuration){
        SetPosition();
        GA.transform.position = FinalPosition;
        GA.transform.localScale = new Vector3(0,0,0);
        GA.SetActive(true);
        
        GA.transform.DOScale(new Vector3(1,1,1), TransitionDuration).SetEase(_AnimationScale);
    }

    public void PopUpScaleExit(GameObject GA){
        SetPosition();
        GA.transform.DOScale(new Vector3(0,0,0), ScaleTransitionDuration).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationScale);

    }

    public void PopUpScaleExit(GameObject GA, float TransitionDuration){
        SetPosition();
        GA.transform.DOScale(new Vector3(0,0,0), TransitionDuration).OnComplete(() => {GA.SetActive(false);}).SetEase(_AnimationScale);

    }

    //Surf Animations-------------------------------------------------------------------------------------------------------------------------------------------------

    

    public void SurfSide(GameObject GA,float var, float MaxRotation, float fade, bool IsItFinished){
        SetPosition();
        if(IsItFinished){
            GA.transform.DOMove(new Vector2 (RestPositionSide.x*var, RestPositionSide.y), SurfTransitionDuration, false).OnComplete(() => {GA.SetActive(false); GA.transform.position = RestPositionDown;});
        }else{
            GA.transform.DOMove(new Vector2 (RestPositionSide.x*var, RestPositionSide.y), SurfTransitionDuration, false);
        }
        
        GA.transform.DORotate(new Vector3 (0f,0f ,MaxRotation*var), SurfTransitionDuration);

        GA.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration);
    }
    public void SurfReset(GameObject GA){
        
        SetPosition();
        GA.transform.DOMove(FinalPosition, SurfTransitionDuration, false);
        GA.transform.DORotate(new Vector3 (0f,0f ,0f), SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(1, SurfTransitionDuration);
    }

    public void SurfVerticalUp(GameObject GA,float var, float MaxRotation, float fade, bool IsItFinished){
        SetPosition();
        

        if(IsItFinished){
            GA.transform.DOMove(new Vector2(RestPositionUp.x, RestPositionUp.y*var), SurfTransitionDuration, false).OnComplete(() => {GA.SetActive(false);});
        }else{
            GA.transform.DOMove(new Vector2(RestPositionUp.x, RestPositionUp.y*var), SurfTransitionDuration, false);
        }
        
        
        
        GA.transform.DORotate(new Vector3 (0f,0f ,MaxRotation*var), SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration);
    }




    public void SurfVerticalDown(GameObject GA,float var, float MaxRotation, float fade, bool IsItFinished){
        SetPosition();
        
        if(IsItFinished){
            
            GA.transform.DOMove(new Vector2(RestPositionDown.x, RestPositionDown.y*var), SurfTransitionDuration+.5f, false).OnComplete(() => {GA.SetActive(false);});
        }else{
            
            GA.transform.DOMove(new Vector2(RestPositionDown.x, RestPositionDown.y*var), SurfTransitionDuration, false);
        }

        GA.transform.DORotate(new Vector3 (0f,0f ,MaxRotation*var), SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration);
    }






    public void SurfResetOtherSongs(GameObject GA, GameObject Position, bool Visible){
        
        
        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration).OnComplete(() => {GA.SetActive(Visible);});

    }

    public void SurfTransitionBackSong(GameObject GA, GameObject Position){

        GA.SetActive(true);
        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha, SurfTransitionDuration);
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);
        GA.transform.DORotate(new Vector3(0,0,0), SurfTransitionDuration);

    }


    public void SurfTransitionOtherSongs(GameObject GA, GameObject Position, float var){
        float fade = Mathf.Clamp(var*2, 0, 1);
        GA.SetActive(true);
        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha*fade, SurfTransitionDuration);
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);

        if(GA.transform.rotation != Quaternion.identity){
            GA.transform.DORotate(new Vector3(0,0,0), SurfTransitionDuration);
        }
        
    }

    public void SurfTransitionBackHideSong(GameObject GA, GameObject Position, float var){
        Debug.Log(GA.name);
        float fade = Mathf.Clamp(var*2, 0, 1);
        GA.transform.DOMove(Position.transform.position, SurfTransitionDuration, false);
        GA.GetComponent<CanvasGroup>().DOFade(Position.GetComponent<CanvasGroup>().alpha*fade, SurfTransitionDuration).OnComplete(() => {GA.SetActive(false);});
        GA.transform.DOScale(Position.transform.localScale, SurfTransitionDuration);
    
    }
         
    public void SurfAddSong(GameObject GA, float fade){
        
        
        GA.SetActive(true);
        fade = Mathf.Clamp(fade*1.5f, 0, 1f);
        GA.transform.DOScale(new Vector3(1,1,1)*fade, SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(fade, SurfTransitionDuration);
       
        
    }
    public void SurfAddSongReset(GameObject GA){
        if(IsAddSongSurfDone){
            if(GA.transform.localScale != new Vector3 (0,0,0)){
                DOTween.Kill(GA);
                GA.transform.DOScale(new Vector3(0,0,0), 0.3F).OnComplete(() => {GA.SetActive(false);});
                
            }
            
            
        }

    }

    public void CompleteSurfAddSong(GameObject GA, float fade){
        
        IsAddSongSurfDone = false;
        DOTween.Kill(GA);
        GA.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
        
        GA.transform.DOScale(new Vector3(1,1,1)*fade, SurfTransitionDuration);
        GA.GetComponent<CanvasGroup>().DOFade(0, SurfTransitionDuration*2).OnComplete(() => {IsAddSongSurfDone = true; GA.transform.localScale =new Vector3(0,0,0); });
    }

    public void DoubleClickOla(GameObject GA){
        GA.GetComponent<CanvasGroup>().alpha = 1;
        GA.SetActive(true);
        GA.transform.DOScale(new Vector3(1.5F,1.5F,1.5F), .3f);
        GA.GetComponent<CanvasGroup>().DOFade(0, .3f).OnComplete(() => {GA.SetActive(false); GA.transform.localScale = new Vector3 (0,0,0);});
    }


    



    

}
