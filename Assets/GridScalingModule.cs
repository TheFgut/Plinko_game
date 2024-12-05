using System;
using UnityEngine;
/// <summary>
/// updates grid depends on resolution change and ui resize
/// </summary>
[Serializable]
public class GridScalingModule
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
        if (gridParent.rect.width != currentFittedWidth)
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
