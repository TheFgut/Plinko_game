using System;
using System.Collections.Generic;
using UnityEngine;
using static PlinkoGrid;

public class PlinkoGrid : MonoBehaviour
{
    [SerializeField] private RectTransform gridParent;
    [SerializeField] private GameObject plinoDot;
    [SerializeField] private ScalingModule gridScaling;

    private PlinkoDot[][] rows;


    private const int minPlinkoDotsRowCount = 3;

    public void Awake()
    {
        RectTransform gridField = GenerateField(12, gridParent);
        gridScaling.Init(gridParent, gridField);
        
    }


    #region plinko grid generation
    public RectTransform GenerateField(int pinsCount, RectTransform gridParent)
    {
        RectTransform gridField = CreateGridFieldObject(gridParent);

        rows = generatePinsRows(pinsCount, gridParent, gridField);


        return gridField;
    }

    private PlinkoDot[][] generatePinsRows(int pinsCount, RectTransform gridParent, RectTransform gridField)
    {
        if (gridParent.rect.width > gridParent.rect.height) throw new Exception("GenerateField failed. Feature for generating horizontal fields is not supported");
        float distBetweenPlinko = gridParent.rect.width / (pinsCount + 1);
        int rowsCount = pinsCount - minPlinkoDotsRowCount + 1;
        if (rowsCount - minPlinkoDotsRowCount <= 0) throw new Exception("GenerateField failed. Invalid pinsCount. pinsCount shouldn't be <= than minPlinkoDotsRowCount");

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
            RectTransform dotPos = Instantiate(plinoDot, gridField).GetComponent<RectTransform>();
            dotPos.anchoredPosition = new Vector2(XPos, YPos);

            XPos += distBetweenPlinko;

            plinkoRow[i] = new PlinkoDot(dotPos);
        }
        return plinkoRow;
    }
    #endregion



    public List<PlinkoDot> CalculateRandomPathForBall(System.Random random)
    {
        List<PlinkoDot> plinkoPath = new List<PlinkoDot>();

        int dotNum = 1;
        for (int i = rows.Length - 1; i >= 0; i--)
        {
            plinkoPath.Add(rows[i][dotNum]);
            PlinkoBallMove ballMove = RandomCalculations.plinkoMoveCalculation(random);
            if (ballMove == PlinkoBallMove.Right) dotNum++;
        }
        return plinkoPath;
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

    public enum DotType {StartDot, MiddleDot, EndDot }
}

public class PlinkoDot
{
    public RectTransform dotPos { get; private set; }
    public DotType dotType { get; private set; }

    public PlinkoDot leftDir { get; private set; }
    public PlinkoDot rightDir { get; private set; }
    public PlinkoDot(RectTransform dotPos)
    {
        this.dotPos = dotPos;
    }

    public void SetAwailableTargets(PlinkoDot leftDir, PlinkoDot rightDir)
    {
        this.leftDir = leftDir;
        this.rightDir = rightDir;
    }
}
