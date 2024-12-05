using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BallsPool
{
    [SerializeField] private int ballsPoolSize;
    [SerializeField] private PlinkoBall ballPrefab;

    private Stack<PlinkoBall> pooledBallsStack;
    private RectTransform ballsTransformParent;

    public bool hasFreeBall => pooledBallsStack.TryPeek(out var freeBall);
    public void Init(RectTransform ballsTransformParent)
    {
        //filling up pool with balls
        pooledBallsStack = new Stack<PlinkoBall>();
        for (int i = 0; i < ballsPoolSize; i++)
        {
            PlinkoBall ball = Object.Instantiate(ballPrefab, ballsTransformParent);
            ball.Init();
            pooledBallsStack.Push(ball);
        }
    }

    public bool tryGetBallFromPool(out PlinkoBall ball)
    {
       return pooledBallsStack.TryPop(out ball);
    }

    public void putBallInPool(PlinkoBall ball)
    {
        pooledBallsStack.Push(ball);
    }
}
