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
        for (int i = 0; i < pinsCount - 2; i++)
        {
            plinkoPath.Add(dotNum);
            PlinkoBallMove ballMove = RandomCalculations.plinkoMoveCalculation(random);
            if (ballMove == PlinkoBallMove.Right) dotNum++;
        }
        await Task.Delay(150);
        return plinkoPath;
    }

    //imitates request send to server
    public async Task<float> getBallRunRevenueFromServer(int pinsCount, int reachedPinId, float betAmount)
    {
        await Task.Delay(150);
        return CalcCoef(pinsCount, reachedPinId) * betAmount;
    }

    public async Task<float[]> getBallRunRevenueFieldCoeficients(int pinsCount)
    {
        await Task.Delay(150);
        float[] coeficients = new float[pinsCount];
        for (int i = 0; i < pinsCount; i++)
        {
            coeficients[i] = CalcCoef(pinsCount, i);
        }
        return coeficients;
    }

    private float CalcCoef(int pinsCount, int pinNum)
    {
        pinNum += 1;
        return (float)(System.Math.Round(Mathf.Pow(Mathf.Abs((pinNum - (pinsCount / 2f))/3f),2), 1) + 0.5f);
    }
}
