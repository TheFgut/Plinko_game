using System;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoGrid
{
    private PlinkoWinCell winCellPrefab;
    private GameObject plinoDotPrefab;

    private PlinkoDot[][] rows;
    private PlinkoWinCell[] winCells;

    private const int minPlinkoDotsRowCount = 3;
    private const float plinkoGridRadius = 3f;

    private const float etalonWidth = 240;

    public RectTransform gridFieldTransform { get; private set; }
    public int pinsCount { get; private set; }

    public PlinkoGrid(int pinsCount, float[] winCoeficients, GameObject plinoDotPrefab, 
        PlinkoWinCell winCellPrefab, RectTransform gridParent)
    {
        this.plinoDotPrefab = plinoDotPrefab;
        this.pinsCount = pinsCount;
        this.winCellPrefab = winCellPrefab;
        gridFieldTransform = GenerateField(pinsCount, winCoeficients, gridParent);
    }

    #region plinko grid generation
    private RectTransform GenerateField(int pinsCount, float[] winCoeficients, RectTransform gridParent)
    {
        RectTransform gridField = CreateGridFieldObject(gridParent);
        rows = generatePinsRows(pinsCount, gridParent, gridField);
        winCells = generateWinCells(pinsCount, winCoeficients, gridParent, gridField);
        return gridField;
    }

    private PlinkoDot[][] generatePinsRows(int pinsCount, RectTransform gridParent, RectTransform gridField)
    {
        if (gridParent.rect.width > gridParent.rect.height) throw new Exception("GenerateField failed. Feature for generating" +
            " horizontal fields is not supported");
        float distBetweenPlinko = etalonWidth / (pinsCount + 1);
        int rowsCount = pinsCount - minPlinkoDotsRowCount + 1;
        if (rowsCount - minPlinkoDotsRowCount <= 0) throw new Exception("GenerateField failed. Invalid pinsCount. pinsCount" +
            " shouldn't be <= than minPlinkoDotsRowCount");

        PlinkoDot[][] rows = new PlinkoDot[rowsCount][];
        float XOffset = distBetweenPlinko - (etalonWidth / 2f);
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
        gridField.transform.localScale = Vector3.one * gridParent.rect.width / etalonWidth;
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

            plinkoRow[i] = new PlinkoDot(dotPos, plinkoGridRadius);
        }
        return plinkoRow;
    }

    private PlinkoWinCell[] generateWinCells(int pinsCount, float[] winCoeficients, RectTransform gridParent, RectTransform gridField)
    {
        if (gridParent.rect.width > gridParent.rect.height) throw new Exception("GenerateField failed. Feature for generating" +
    " horizontal fields is not supported");
        float distBetweenPlinko = etalonWidth / (pinsCount + 1);
        int rowsCount = pinsCount - minPlinkoDotsRowCount + 1;
        if (rowsCount - minPlinkoDotsRowCount <= 0) throw new Exception("GenerateField failed. Invalid pinsCount. pinsCount" +
            " shouldn't be <= than minPlinkoDotsRowCount");

        PlinkoWinCell[] winCells = new PlinkoWinCell[pinsCount];
        float XOffset = (distBetweenPlinko * 1.5f) - (etalonWidth / 2f);
        for (int i = 0; i < pinsCount - 1; i++)
        {
            winCells[i] = UnityEngine.Object.Instantiate(winCellPrefab, gridField);
            winCells[i].Init(winCoeficients[i]);
            winCells[i].transform.anchoredPosition = new Vector2(XOffset, 0);
            XOffset += distBetweenPlinko;
        }
        return winCells;
    }
    #endregion



    public List<PlinkoDot> CalculatePhysicalPathForBall(List<int> getPathByDotsIds, out PlinkoWinCell winCell)
    {
        List<PlinkoDot> plinkoPath = new List<PlinkoDot>();

        for (int i = rows.Length - 1; i >= 0; i--)
        {
            plinkoPath.Add(rows[i][getPathByDotsIds[getPathByDotsIds.Count - i - 1]]);
        }
        winCell = winCells[getPathByDotsIds[getPathByDotsIds.Count - 1]];
        return plinkoPath;
    }

    public enum DotType {StartDot, MiddleDot, EndDot }
}
