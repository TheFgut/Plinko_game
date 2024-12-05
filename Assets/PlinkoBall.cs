using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlinkoBall : MonoBehaviour
{
    private new RectTransform transform;

    [Inject]
    private void Construct()
    {

    }

    public void Init()
    {
        transform = GetComponent<RectTransform>();
    }

    public void MoveBall(List<PlinkoDot> route, Action onBallReachDestination)
    {
        MoveBallByRouteRecursively(route, onBallReachDestination);
    }

    private void MoveBallByRouteRecursively(List<PlinkoDot> route, Action onBallReachDestination)
    {
        if(route.Count == 0)
        {
            onBallReachDestination?.Invoke();
            return;
        }
        PlinkoDot target = route[0];
        route.Remove(target);//can be optimized

        transform.DOJumpAnchorPos(target.dotPos.anchoredPosition,0.3f,1, 0.3f)
            .OnComplete(() => MoveBallByRouteRecursively(route, onBallReachDestination));
    }

    public void Reset()
    {
        this.gameObject.SetActive(false);
    }
}
