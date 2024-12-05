using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoGame : MonoBehaviour
{
    [SerializeField] private PlinkoBall ball;
    [SerializeField] private PlinkoGrid grid;


    void Start()
    {
        System.Random random = new System.Random();
        ball.Init();
        MoveBall(random);
    }

    private void MoveBall(System.Random random)
    {
        ball.MoveBall(grid.CalculateRandomPathForBall(random), () => MoveBall(random));
    }

}
