using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BettingSystem : MonoBehaviour
{
    [SerializeField] private float betChangeAmount = 5;
    [SerializeField] private float tensBetChangeAmount = 5;
    [SerializeField] private TextMeshProUGUI valueDisplay;
    private float betValue;
    public float GetBetAmount() => betValue;

    public void Start()
    {
        betValue = betChangeAmount;
        UpdateBetDisp(betValue);
    }

    public void IncreaseBet()
    {
        if(betValue >= 10) betValue += tensBetChangeAmount;
        else betValue += betChangeAmount;

        valueDisplay.text = betValue.ToString();
        UpdateBetDisp(betValue);
    }

    public void DecreaseBet()
    {
        betValue -= betChangeAmount;
        if(betValue <= 0) betValue = betChangeAmount;
        UpdateBetDisp(betValue);
    }

    private void UpdateBetDisp(float betValue)
    {
        valueDisplay.text = betValue.ToString();
    }
}
