using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomCalculations
{
    public static PlinkoBallMove plinkoMoveCalculation(System.Random random)
    {
        int randomInt = random.Next(0, 2);
        if (randomInt == 1) return PlinkoBallMove.Right;
        else return PlinkoBallMove.Left;
    }
}

public enum PlinkoBallMove { Left, Right }