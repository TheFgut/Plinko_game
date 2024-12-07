using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlinkoWinCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coeficientDisp;
    [SerializeField] private Image graphics;
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float animationDuration = 0.5f;

    public new RectTransform transform { get; private set; }

    public void Init(float winCoef)
    {
        transform = GetComponent<RectTransform>();
        coeficientDisp.text = winCoef.ToString();
        graphics.color = winCoef < 1 ? Color.Lerp(Color.red, graphics.color, winCoef / 1f)
            : Color.Lerp(graphics.color, Color.green, winCoef / 3f);
    }

    public void PlayBallReachAnim()
    {
        transform.DOScale(scaleFactor, animationDuration / 2)
            .OnComplete(() => transform.DOScale(1f, animationDuration / 2));
    }
}
