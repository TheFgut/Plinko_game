using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserWallet : MonoBehaviour
{
    [SerializeField] private float money_USD = 3000;

    public bool tryDecreaseMoney(float moneyAmount)
    {
        if(money_USD >= moneyAmount)
        {
            money_USD -= moneyAmount;
            return true;
        }
        return false;
    }
}
