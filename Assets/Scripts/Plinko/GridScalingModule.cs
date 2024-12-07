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

    private Vector3 defaultGridLocalScale;

    private bool initialized;
    public void Init(RectTransform gridParent, RectTransform gridTransform)
    {
        this.gridParent = gridParent;
        this.gridTransform = gridTransform;

        defaultGridLocalScale = gridTransform.transform.localScale;

        parentDefaultWidth = gridParent.rect.width;
        currentFittedWidth = parentDefaultWidth;

        initialized = true;
    }

    public void Update()
    {
        if (!initialized) return;
        if (gridParent.rect.width != currentFittedWidth)
        {
            ResizeGridToFitInParent();
        }
    }

    private void ResizeGridToFitInParent()
    {
        float currentWidth = gridParent.rect.width;

        float scaleFactor = currentWidth / parentDefaultWidth;
        gridTransform.localScale = defaultGridLocalScale * scaleFactor;
        currentFittedWidth = currentWidth;
    }
}
