using LockMonitor;
using System.Runtime.InteropServices;
using UnityEngine;

public class MonoGUIHelper : MonoBehaviour
{
    private LockMouseMonitor owner;
    private bool isLocked = false;
    private RECT currentRect;
    private RECT? lastRect = null;

    public void SetOwner(LockMouseMonitor monitor)
    {
        owner = monitor;
    }

    public void LockRect(int width, int height)
    {
        isLocked = true;

        // Используем Screen.width/height — размеры текущего окна/экрана Unity
        int screenX = Screen.width / 2 - width / 2;
        int screenY = Screen.height / 2 - height / 2;

        RECT newRect = new RECT
        {
            left = screenX,
            top = screenY,
            right = screenX + width,
            bottom = screenY + height
        };

        if (!lastRect.HasValue || !RectsAreEqual(lastRect.Value, newRect))
        {
            currentRect = newRect;
            bool result = ClipCursor(ref currentRect);
            Debug.Log($"ClipCursor called with rect {newRect.left},{newRect.top},{newRect.right},{newRect.bottom} result={result}");
            lastRect = newRect;
        }
    }

    public void Unlock()
    {
        isLocked = false;
        bool result = ClipCursor(System.IntPtr.Zero);
        Debug.Log($"ClipCursor unlocked, result={result}");
        lastRect = null;
    }

    void Update()
    {
        // Не повторяем ClipCursor в Update — убрали, чтобы не вызывать подвисания.
    }

    void OnGUI()
    {
        owner?.OnGUIHandler();
    }

    private bool RectsAreEqual(RECT a, RECT b)
    {
        return a.left == b.left && a.top == b.top && a.right == b.right && a.bottom == b.bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left, top, right, bottom;
    }

    [DllImport("user32.dll")]
    public static extern bool ClipCursor(ref RECT rect);

    [DllImport("user32.dll")]
    public static extern bool ClipCursor(System.IntPtr rect);
}
