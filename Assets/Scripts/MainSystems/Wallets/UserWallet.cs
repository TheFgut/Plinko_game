using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserWallet : MonoBehaviour
{
    [SerializeField] private float money_USD = 3000;

    public Action<float> onMoneyAmountChanged;

    public bool tryDecreaseMoney(float moneyAmount)
    {
        if(money_USD >= moneyAmount)
        {
            money_USD -= moneyAmount;
            onMoneyAmountChanged?.Invoke(money_USD);
            return true;
        }
        return false;
    }

    public bool tryIncreaseMoney(float moneyAmount)
    {
        money_USD += moneyAmount;
        onMoneyAmountChanged?.Invoke(money_USD);
        return true;
    }
    public float GetMoney() => money_USD;
}
