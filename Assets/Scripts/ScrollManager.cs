using UnityEngine;
using UnityEngine.UI;

public class ScrollManager : MonoBehaviour
{
    public ScrollRect scrollRect;  // Reference to the ScrollRect component
    public int itemsVisibleAtOnce = 3;  // Number of items visible at once
    public int totalItems = 6;  // Total number of items in the scroll view

    private float scrollStep;  // Step to scroll by

    void Start()
    {
        // Calculate the scroll step based on the number of items and how many are visible
        scrollStep = 1f / (totalItems - itemsVisibleAtOnce);
    }

    // Scroll to the right (next set of items)
    public void ScrollRight()
    {
        float targetPos = Mathf.Clamp(scrollRect.horizontalNormalizedPosition + scrollStep, 0f, 1f);
        scrollRect.horizontalNormalizedPosition = targetPos;
    }

    // Scroll to the left (previous set of items)
    public void ScrollLeft()
    {
        float targetPos = Mathf.Clamp(scrollRect.horizontalNormalizedPosition - scrollStep, 0f, 1f);
        scrollRect.horizontalNormalizedPosition = targetPos;
    }
}
