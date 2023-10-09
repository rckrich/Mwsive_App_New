using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIMessage_Ani : MonoBehaviour
{

    private Tweener[] MessageAnimation, RestartAnimation;
    private Vector3 FinalPosition, RestPositionDown;
    private GameObject MainCanvas;
    [Range(0f, 2f)]
    public float AnimationDuration;

    // Start is called before the first frame update
    void Awake()
    {
        if(MainCanvas == null)
        {

            MainCanvas = UIMessage.instance.UIMessageCanvas;
        }
        FinalPosition = MainCanvas.transform.position;
        RestPositionDown = new Vector3(MainCanvas.transform.position.x, -2 * MainCanvas.transform.position.y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play_MessageAnimation()
    {
        gameObject.transform.position = RestPositionDown;
        gameObject.SetActive(true);
        if (MessageAnimation == null)
        {
            SetUp_MessageAnimation();
        }
        else
        {
            Restart_MessageAnimation();
        }

        foreach (Tweener item in MessageAnimation)
        {
            item.SetAutoKill(false);
            item.Play();
        }
    }

    private void SetUp_MessageAnimation()
    {
        MessageAnimation = new Tweener[2];
        int position;
        int.TryParse( gameObject.name.Substring(gameObject.name.Length - 1), out position);

        Vector3 PositionToMove = new Vector2(FinalPosition.x, UIMessage.instance.ReturnLastPosition(position-1).y);


        MessageAnimation[0] = gameObject.GetComponent<CanvasGroup>().DOFade(1f, AnimationDuration).Pause();
        
        MessageAnimation[1] = gameObject.transform.DOMove(PositionToMove, AnimationDuration, false).Pause();
        MessageAnimation[0].OnComplete(() => { StartCoroutine(WaitMessage()); });
    }

    private void Restart_MessageAnimation()
    {
        int position;
        int.TryParse(gameObject.name.Substring(gameObject.name.Length - 1), out position);
        Vector3 PositionToMove = new Vector2(FinalPosition.x, FinalPosition.y + (130 * (position-1)));

        MessageAnimation[0].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, (System.Single)1);
        MessageAnimation[1].ChangeValues(gameObject.transform.position, PositionToMove);
    }

    private void SetUp_RestartAnimation()
    {
        RestartAnimation = new Tweener[2];
        RestartAnimation[0] = gameObject.GetComponent<CanvasGroup>().DOFade(0f, .5f).Pause();
        RestartAnimation[1] = gameObject.transform.DOMove(RestPositionDown, 1f, false).Pause();
        RestartAnimation[0].OnComplete(() => { gameObject.SetActive(false); });

    }

    private void Restart_RestartAnimation()
    {
        RestartAnimation[0].ChangeValues(gameObject.GetComponent<CanvasGroup>().alpha, (System.Single)0);
        RestartAnimation[1].ChangeValues(gameObject.transform.position, RestPositionDown);
    }

    IEnumerator WaitMessage()
    {

        yield return new WaitForSeconds(2f);

        if (RestartAnimation == null)
        {
            SetUp_RestartAnimation();
        }
        else
        {
            Restart_RestartAnimation();
        }

        foreach (Tweener item in RestartAnimation)
        {
            item.SetAutoKill(false);
            item.Play();
        }
    }





}
