using UnityEngine;
using System;

public class UnlockableObject : MonoBehaviour
{
    [Header("State")]
    public bool IsVisible { get; private set; }
    public bool IsUnlocked { get; private set; }

    [Header("Conditions")]
    public Func<bool> VisibleCondition;
    public Func<bool> UnlockCondition;

    [Header("Settings")]
    public bool HideWhenInvisible = true;

    void Start()
    {
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        // Visible logic
        if (!IsVisible && VisibleCondition != null && VisibleCondition())
        {
            IsVisible = true;

            if (HideWhenInvisible)
                gameObject.SetActive(true);
        }

        // Unlock logic
        if (!IsUnlocked && UnlockCondition != null && UnlockCondition())
        {
            IsUnlocked = true;
        }

        // If not visible hide object
        if (!IsVisible && HideWhenInvisible)
        {
            gameObject.SetActive(false);
        }
    }
}