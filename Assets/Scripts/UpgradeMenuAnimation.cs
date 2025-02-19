using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuAnimation : MonoBehaviour
{
    public RectTransform menuPanel;  // Reference to the Menu Panel's RectTransform.
    public float slideSpeed = 5f;    // Speed at which the menu slides up and down.

    private Vector3 offScreenPosition; // The position off the screen
    private Vector3 onScreenPosition;  // The position on the screen

    private bool isMenuVisible = false;  // Boolean to track the menu state

    void Start()
    {
        // Set the off-screen position and on-screen position based on the current screen height.
        offScreenPosition = new Vector3(0, -Screen.height, 0);  // Off the screen (below).
        onScreenPosition = new Vector3(0, 0, 0);  // On the screen

        // Initially position the menu off-screen.
        menuPanel.anchoredPosition = offScreenPosition;
    }

    void Update()
    {
        // Slide the menu up or down based on whether it's visible or not.
        if (isMenuVisible)
        {
            // Move the menu to the on-screen position (slide up).
            menuPanel.anchoredPosition = Vector3.Lerp(menuPanel.anchoredPosition, onScreenPosition, slideSpeed * Time.deltaTime);
        }
        else
        {
            // Move the menu to the off-screen position (slide down).
            menuPanel.anchoredPosition = Vector3.Lerp(menuPanel.anchoredPosition, offScreenPosition, slideSpeed * Time.deltaTime);
        }
    }

    // Public method to toggle the menu visibility (called by button click).
    public void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible;  // Toggle visibility.
    }
}
