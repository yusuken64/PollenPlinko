using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections;

public class TransparentWindow : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
    // Import Windows API functions
    [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")] private static extern IntPtr GetActiveWindow();
    [DllImport("Dwmapi.dll")] private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
    [DllImport("user32.dll")] static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("user32.dll")] static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")] static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    const int GWL_STYLE = -16;
    const int GWL_EXSTYLE = -20;
    const int WS_EX_LAYERED = 0x80000;
    const int WS_EX_TRANSPARENT = 0x20;
    const int LWA_COLORKEY = 0x1;
    const int WS_POPUP = unchecked((int)0x80000000);
    private struct MARGINS
    {
        public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight;
    }

    void Awake()
    {
        var cam = Camera.main;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.allowHDR = false;
    }

    void Start()
    {
        StartCoroutine(ApplyTransparency());
    }

    IEnumerator ApplyTransparency()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        IntPtr hwnd = GetForegroundWindow();

        // fallback if needed
        if (hwnd == IntPtr.Zero)
            hwnd = GetActiveWindow();

        // Apply layered style
        int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);

        // Set black as transparent
        SetLayeredWindowAttributes(hwnd, 0x000000, 0, LWA_COLORKEY);
    }
#endif
}