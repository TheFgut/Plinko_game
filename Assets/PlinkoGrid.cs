using System;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoGrid
{
    private GameObject plinoDotPrefab;
    private PlinkoDot[][] rows;

    private const int minPlinkoDotsRowCount = 3;

    public RectTransform gridFieldTransform { get; private set; }
    public int pinsCount { get; private set; }

    public PlinkoGrid(int pinsCount, GameObject plinoDotPrefab, RectTransform gridParent)
    {
        this.plinoDotPrefab = plinoDotPrefab;
        this.pinsCount = pinsCount;
        gridFieldTransform = GenerateField(pinsCount, gridParent);
    }

    #region plinko grid generation
    private RectTransform GenerateField(int pinsCount, RectTransform gridParent)
    {
        RectTransform gridField = CreateGridFieldObject(gridParent);
        rows = generatePinsRows(pinsCount, gridParent, gridField);
        return gridField;
    }

    private PlinkoDot[][] generatePinsRows(int pinsCount, RectTransform gridParent, RectTransform gridField)
    {
        if (gridParent.rect.width > gridParent.rect.height) throw new Exception("GenerateField failed. Feature for generating" +
            " horizontal fields is not supported");
        float distBetweenPlinko = gridParent.rect.width / (pinsCount + 1);
        int rowsCount = pinsCount - minPlinkoDotsRowCount + 1;
        if (rowsCount - minPlinkoDotsRowCount <= 0) throw new Exception("GenerateField failed. Invalid pinsCount. pinsCount" +
            " shouldn't be <= than minPlinkoDotsRowCount");

        PlinkoDot[][] rows = new PlinkoDot[rowsCount][];
        float XOffset = distBetweenPlinko - (gridParent.rect.width / 2f);
        float YPos = distBetweenPlinko;
        for (int i = 0; i < rowsCount; i++)
        {

            int rowPinsCount = pinsCount - i;
            rows[i] = GenerateRow(rowPinsCount, distBetweenPlinko, YPos, XOffset, gridField);
            YPos += distBetweenPlinko;
            XOffset += distBetweenPlinko * 0.5f;
        }
        return rows;
    }

    private RectTransform CreateGridFieldObject(RectTransform gridParent)
    {
        RectTransform gridField = new GameObject("grid", typeof(RectTransform)).GetComponent<RectTransform>();
        gridField.anchorMin = new Vector2(0.5f, 0);
        gridField.anchorMax = new Vector2(0.5f, 0);
        gridField.pivot = new Vector2(0.5f, 0);
        gridField.anchoredPosition = Vector2.zero;
        gridField.transform.SetParent(gridParent, false);
        return gridField;
    }

    private PlinkoDot[] GenerateRow(int rowPinsCount, float distBetweenPlinko, float YPos, float XOffset, RectTransform gridField)
    {
        PlinkoDot[] plinkoRow = new PlinkoDot[rowPinsCount];
        float XPos = XOffset;
        for (int i = 0; i < rowPinsCount; i++)
        {
            RectTransform dotPos = UnityEngine.Object.Instantiate(plinoDotPrefab, gridField).GetComponent<RectTransform>();
            dotPos.anchoredPosition = new Vector2(XPos, YPos);

            XPos += distBetweenPlinko;

            plinkoRow[i] = new PlinkoDot(dotPos);
        }
        return plinkoRow;
    }
    #endregion



    public List<PlinkoDot> CalculatePhysicalPathForBall(List<int> getPathByDotsIds)
    {
        List<PlinkoDot> plinkoPath = new List<PlinkoDot>();

        for (int i = rows.Length - 1; i >= 0; i--)
        {
            plinkoPath.Add(rows[i][getPathByDotsIds[getPathByDotsIds.Count - i - 1]]);
        }
        return plinkoPath;
    }

    public enum DotType {StartDot, MiddleDot, EndDot }
}
