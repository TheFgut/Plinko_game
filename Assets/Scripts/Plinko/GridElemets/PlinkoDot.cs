using UnityEngine;
using static PlinkoGrid;

public class PlinkoDot
{
    public float radius { get; private set; }
    public RectTransform dotPos { get; private set; }
    public DotType dotType { get; private set; }

    public PlinkoDot leftDir { get; private set; }
    public PlinkoDot rightDir { get; private set; }
    public PlinkoDot(RectTransform dotPos, float radius)
    {
        this.dotPos = dotPos;
        this.radius = radius;
    }

    public void SetAwailableTargets(PlinkoDot leftDir, PlinkoDot rightDir)
    {
        this.leftDir = leftDir;
        this.rightDir = rightDir;
    }
}
