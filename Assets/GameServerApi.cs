using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameServerApi : MonoBehaviour
{
    private System.Random random;
    public void Start()
    {
        random = new System.Random();
    }

    //imitates request send to server
    public async Task<List<int>> getBallPathFromServer(int pinsCount)
    {
        List<int> plinkoPath = new List<int>();

        int dotNum = 1;
        for (int i = pinsCount - 2; i >= 0; i--)
        {
            plinkoPath.Add(dotNum);
            PlinkoBallMove ballMove = RandomCalculations.plinkoMoveCalculation(random);
            if (ballMove == PlinkoBallMove.Right) dotNum++;
        }
        await Task.Delay(150);
        return plinkoPath;
    }

    //imitates request send to server
    public async Task<float> getBallRunRevenueFromServer(int pinsCount, int reachedPinId)
    {
        await Task.Delay(150);
        return (float)System.Math.Round(Mathf.Abs(reachedPinId - (pinsCount/2f)),1) + 0.5f;
    }
}
