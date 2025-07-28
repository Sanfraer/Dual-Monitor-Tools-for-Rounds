using BepInEx;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Drawing;

namespace LockMonitor
{
    [BepInPlugin("Sanfraer", "MouseLocker", "1.0")]
    public class LockMouseMonitor : BaseUnityPlugin
    {
        private bool menuState = true;
        private string menuMode = "Main";

        private Rect rectMenu = new Rect(10f, 10f, 360f, 336f);
        private Rect dragMenu = new Rect(0f, 0f, 336f, 20f);
        private GUILayoutOption[] buttonSize = new GUILayoutOption[] { GUILayout.Width(105f), GUILayout.Height(25) };

        private bool lock1920x1080 = false;
        private bool customLock = false;
        private string customWidth = "1920";
        private string customHeight = "1080";

        private MonoGUIHelper guiHelper;


        void Start()
        {
            GameObject go = new GameObject("MouseLockerGUI");
            GameObject.DontDestroyOnLoad(go);
            guiHelper = go.AddComponent<MonoGUIHelper>();
            guiHelper.SetOwner(this);
        }

        void Update()
        {   
             if (Input.GetKeyDown(KeyCode.F9))
                {
                    menuState = !menuState;
             }   
        }

        public void OnGUIHandler()
        {
            if (!menuState) return;

            switch (menuMode)
            {
                case "Main":
                    rectMenu = GUI.Window(0, rectMenu, MainWindow, "SanWare - BETA");
                    break;
            }
        }


        private void MainWindow(int id)
        {     
            GUI.color = UnityEngine.Color.cyan;

            GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical();

            if (GUILayout.Button(MakeToggle("1920x1080", lock1920x1080), buttonSize))
            {
                LockMouse(1920, 1080);
                lock1920x1080 = true;
                customLock = false;
            }

            if (GUILayout.Button("Unlock", buttonSize))
            {
                UnlockMouse();
                lock1920x1080 = false;
                customLock = false;
            }

            GUILayout.Space(10);
            GUILayout.Label("Custom Resolution");

            GUILayout.BeginHorizontal();
            GUILayout.Label("W:", GUILayout.Width(20));
            customWidth = GUILayout.TextField(customWidth, GUILayout.Width(60));
            GUILayout.Label("H:", GUILayout.Width(20));
            customHeight = GUILayout.TextField(customHeight, GUILayout.Width(60));
            GUILayout.EndHorizontal();

            if (GUILayout.Button(MakeToggle("Lock custom", customLock), buttonSize))
            {
                if (int.TryParse(customWidth, out int w) && int.TryParse(customHeight, out int h))
                {
                    LockMouse(w, h);
                    lock1920x1080 = false;
                    customLock = true;
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }


        private string MakeToggle(string name, bool toggle)
        {
            string status = toggle ? "<color=green>ON</color>" : "<color=red>OFF</color>";
            return $"{name} {status}";
        }

        private void LockMouse(int width, int height)
        {
            guiHelper?.LockRect(width, height);
        }

        private void UnlockMouse()
        {
            guiHelper?.Unlock();
        }

        [DllImport("user32.dll")]
        static extern bool ClipCursor(ref Rectangle rect);

        [DllImport("user32.dll")]
        static extern bool ClipCursor(System.IntPtr rect);
    }

}
