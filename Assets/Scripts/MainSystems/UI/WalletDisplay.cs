using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class WalletDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;

    private UserWallet wallet;
    private float previousValue;
    [Inject]
    private void Construct(UserWallet wallet)
    {
        this.wallet = wallet;
    }

    public void Start()
    {
        wallet.onMoneyAmountChanged += MoneyAmountChanged;
        float value = wallet.GetMoney();
        currencyText.text = Math.Round(value, 1).ToString();
        previousValue = value;
    }

    private void MoneyAmountChanged(float currentMoneyValue)
    {
        currencyText.text = Math.Round(currentMoneyValue,1).ToString();
        if(previousValue <= currentMoneyValue)
        {
            transform.DOScale(1.2f, 0.2f)
                .OnComplete(() => transform.DOScale(1f, 0.2f));
        }
        previousValue = currentMoneyValue;
    }

}
