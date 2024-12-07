using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlinkoBall : MonoBehaviour
{
    [SerializeField] private float jumpPower = 5;
    [SerializeField] private float ballRadius;
    public new RectTransform transform { get; private set; }


    public void Init()
    {
        transform = GetComponent<RectTransform>();
    }

    public void MoveBall(List<PlinkoDot> route,PlinkoWinCell winCell, Action onBallReachDestination)
    {
        MoveBallByRouteRecursively(route, winCell, onBallReachDestination);
    }

    private void MoveBallByRouteRecursively(List<PlinkoDot> route, PlinkoWinCell winCell, Action onBallReachDestination)
    {
        if(route.Count == 0)
        {
            transform.DOJumpAnchorPos(winCell.transform.anchoredPosition, 7.5f, 1, 0.3f)
                .OnComplete(() =>
                {
                    winCell.PlayBallReachAnim();
                    onBallReachDestination?.Invoke();
                });         
            return;
        }
        PlinkoDot target = route[0];
        route.Remove(target);//can be optimized

        transform.DOJumpAnchorPos(target.dotPos.anchoredPosition + new Vector2(0, target.radius + ballRadius),5,1, 0.3f * UnityEngine.Random.Range(1, 1.2f))
            .OnComplete(() => MoveBallByRouteRecursively(route, winCell, onBallReachDestination));
    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
