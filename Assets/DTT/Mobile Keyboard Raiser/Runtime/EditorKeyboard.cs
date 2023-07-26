#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace DTT.KeyboardRaiser
{
    /// <summary>
    /// Used for displaying a mock keyboard in the editor.
    /// </summary>
    public class EditorKeyboard : MonoBehaviour
    {
        /// <summary>
        /// Whether the keyboard is actively shown.
        /// </summary>
        public static bool IsActive { get; private set; }

        /// <summary>
        /// The area occupied by the keyboard.
        /// </summary>
        public static Rect Area => GetArea(_percentageOccupied);

        /// <summary>
        /// Invoked when the keyboard is opened.
        /// </summary>
        public static event Action Opened;
        
        /// <summary>
        /// Invoked when the keyboard is closed.
        /// </summary>
        public static event Action Closed;
        
        /// <summary>
        /// The active worker instance for the ongoing opening/closing of the keyboard.
        /// </summary>
        private static EditorKeyboard _worker;

        /// <summary>
        /// The amount of percentage occupied by the keyboard.
        /// </summary>
        private static float _percentageOccupied = 0.4f;
        
        /// <summary>
        /// Opens the keyboard.
        /// </summary>
        public static void Open(float percentageOccupied = 0.4f)
        {
            if (IsActive)
                return;

            _percentageOccupied = percentageOccupied;
            _worker = new GameObject().AddComponent<EditorKeyboard>();
            _worker.name = "Editor (Mock) Keyboard";
            DontDestroyOnLoad(_worker);
            IsActive = true;
            Opened?.Invoke();
        }

        /// <summary>
        /// Closes the keyboard.
        /// </summary>
        public static void Close()
        {
            if (!IsActive)
                return;
            
            IsActive = false;
            Destroy(_worker.gameObject);
            Closed?.Invoke();
        }

        /// <summary>
        /// Draws the keyboard if it's active.
        /// </summary>
        private void OnGUI()
        {
            if (!IsActive)
                return;
            Rect area = GetArea(_percentageOccupied);
            GUI.Box(area, String.Empty);
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Screen.width / 15,
                alignment = TextAnchor.MiddleCenter,
                font = AssetDatabase.LoadAssetAtPath<Font>("Packages/dtt.keyboardraiser/Fonts/Montserrat-Bold.ttf"),
                normal =
                {
                    textColor = new Color(1, 1, 1, 0.5f)
                }
            };
            GUIUtility.RotateAroundPivot(-15, area.center);
            GUI.Label(area, "Mock keyboard", labelStyle);
        }

        /// <summary>
        /// Determines the area in the screen in the keyboard is being displayed at.
        /// </summary>
        private static Rect GetArea(float percentageOccupied)
        {
            return new Rect(
                0.0f, 
                IsActive ? Screen.height * (1 - percentageOccupied) : Screen.height, 
                Screen.width,
                IsActive ? Screen.height * percentageOccupied : 0
            );
        }
    }
}
#endif