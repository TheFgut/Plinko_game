using UnityEngine;
using static PlinkoGrid;

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
