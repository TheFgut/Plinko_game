using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlinkoGrid : MonoBehaviour
{
    [SerializeField] private RectTransform gridParent;
    [SerializeField] private GameObject plinoDot;
    [SerializeField] private ScalingModule gridScaling;

    private const int minPlinkoDotsRowCount = 3;

    public void Start()
    {
        RectTransform gridField = GenerateField(12, gridParent);
        gridScaling.Init(gridParent, gridField);
    }

    public RectTransform GenerateField(int pinsCount, RectTransform gridParent)
    {
        RectTransform gridField = new GameObject("grid",typeof(RectTransform)).GetComponent<RectTransform>();
        gridField.transform.SetParent(gridParent, false);

        if (gridParent.rect.width > gridParent.rect.height) throw new Exception("GenerateField failed. Feature for generating horizontal fields is not supported");
        float distBetweenPlinko = gridParent.rect.width / (pinsCount + 1);
        int rowsCount = pinsCount;
        if (rowsCount - minPlinkoDotsRowCount <= 0) throw new Exception("GenerateField failed. Invalid pinsCount. pinsCount shouldn't be <= than minPlinkoDotsRowCount");
        
        float XOffset = distBetweenPlinko - (gridParent.rect.width / 2f);
        float YPos = distBetweenPlinko;
        for (int i = 0; i <= rowsCount - minPlinkoDotsRowCount; i++)
        {

            int rowPinsCount = rowsCount - i;
            GenerateRow(rowPinsCount, distBetweenPlinko, YPos, XOffset, gridField);
            YPos += distBetweenPlinko;
            XOffset += distBetweenPlinko * 0.5f;
        }
        return gridField;
    }

    private void GenerateRow(int rowPinsCount, float distBetweenPlinko, float YPos, float XOffset, RectTransform gridField)
    {
        float XPos = XOffset;
        for (int i = 0; i < rowPinsCount; i++)
        {
            RectTransform dot = Instantiate(plinoDot, gridField).GetComponent<RectTransform>();
            dot.anchoredPosition = new Vector2(XPos, YPos);

            XPos += distBetweenPlinko;
        }
    }

    void Update()
    {
        gridScaling.Update();
    }

    /// <summary>
    /// updates grid depends on resolution change and ui resize
    /// </summary>
    [Serializable]
    private class ScalingModule
    {
        private RectTransform gridParent;
        private RectTransform gridTransform;

        private float parentDefaultWidth;
        private float currentFittedWidth;
        public void Init(RectTransform gridParent, RectTransform gridTransform)
        {
            this.gridParent = gridParent;
            this.gridTransform = gridTransform;

            parentDefaultWidth = gridParent.rect.width;
            currentFittedWidth = parentDefaultWidth;
        }

        public void Update()
        {
            if(gridParent.rect.width != currentFittedWidth)
            {
                ResizeGridToFitInParent();
            }
        }

        private void ResizeGridToFitInParent()
        {
            float currentWidth = gridParent.rect.width;

            float scaleFactor = currentWidth / parentDefaultWidth;
            gridTransform.localScale = Vector3.one * scaleFactor;
            currentFittedWidth = currentWidth;
        }
    }
}
