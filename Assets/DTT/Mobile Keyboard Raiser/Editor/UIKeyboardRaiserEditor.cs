#if UNITY_EDITOR

using DTT.KeyboardRaiser.Editor.Utils;
using DTT.PublishingTools;
using DTT.Utils.EditorUtilities;
using DTT.Utils.Extensions;
using UnityEditor;
using UnityEngine;

namespace DTT.KeyboardRaiser.Editor
{
    /// <summary>
    /// Contains all the GUIContent for <see cref="UIKeyboardRaiserEditor"/>.
    /// </summary>
    internal class UIKeyboardRaiserGUIContentCache : GUIContentCache
    {
        /// <summary>
        /// The GUIContent for the <see cref="UIKeyboardRaiser._isSmooth"/> toggle.
        /// </summary>
        public GUIContent IsSmoothContent => base[nameof(IsSmoothContent)];

        /// <summary>
        /// The GUIContent for the <see cref="UIKeyboardRaiser._padding"/> field.
        /// </summary>
        public GUIContent PaddingContent => base[nameof(PaddingContent)];

        /// <summary>
        /// The GUIContent for the padding arrow debug toggle.
        /// </summary>
        public GUIContent PaddingArrowContent => base[nameof(PaddingArrowContent)];

        /// <summary>
        /// The GUIContent for the threshold area debug toggle.
        /// </summary>
        public GUIContent DebugThresholdAreaContent => base[nameof(DebugThresholdAreaContent)];

        /// <summary>
        /// The GUIContent for the editor keyboard controls header.
        /// </summary>
        public GUIContent KeyboardControlHeaderContent => base[nameof(KeyboardControlHeaderContent)];

        /// <summary>
        /// The GUIContent for the amount occupied slider.
        /// </summary>
        public GUIContent AmountOccupiedSliderContent => base[nameof(AmountOccupiedSliderContent)];

        /// <summary>
        /// Creates an object that contains all the GUIContent for <see cref="UIKeyboardRaiserEditor"/>.
        /// </summary>
        public UIKeyboardRaiserGUIContentCache()
        {
            Add(nameof(IsSmoothContent), () => new GUIContent("Is Smooth", "If enabled the object being raised will be moved smoothly instead of instantly."));
            Add(nameof(PaddingContent), () => new GUIContent("Padding", "The amount of bottom padding in world units. This is shown in the scene as the red line."));
            Add(nameof(PaddingArrowContent), () => new GUIContent("Padding Arrow", "Whether the padding arrow is visible in the scene view."));
            Add(nameof(DebugThresholdAreaContent), () => new GUIContent("Threshold Area", "Whether the threshold area can be seen in the scene."));
            Add(nameof(KeyboardControlHeaderContent), () => new GUIContent("Debug Keyboard", "Provides controls for testing the component by raising a 'mock' keyboard in the editor."));
            Add(nameof(AmountOccupiedSliderContent), () => new GUIContent("Amount Occupied", "The amount in percentages the mock keyboard will take up screen space. Generally mobile keyboards take up ~40% on screen."));
        }
    }

    /// <summary>
    /// Contains all the serialized properties from <see cref="UIKeyboardRaiser"/>.
    /// </summary>
    internal class UIKeyboardRaiserSerializedPropertyCache : SerializedPropertyCache
    {
        /// <summary>
        /// The property for the <see cref="UIKeyboardRaiser._isSmooth"/> toggle.
        /// </summary>
        public SerializedProperty IsSmoothProperty => base["_isSmooth"];

        /// <summary>
        /// The property for the <see cref="UIKeyboardRaiser._padding"/> field.
        /// </summary>
        public SerializedProperty PaddingProperty => base["_padding"];

        public SerializedProperty HintsProperty => base["_isHints"];

        /// <summary>
        /// Creates on object that contains all the serialized properties from <see cref="UIKeyboardRaiser"/>.
        /// </summary>
        /// <param name="serializedObject">The object to gather the serialized properties from.</param>
        public UIKeyboardRaiserSerializedPropertyCache(SerializedObject serializedObject) : base(serializedObject)
        { }
    }

    /// <summary>
    /// The custom inspector for the <see cref="UIKeyboardRaiser"/> component.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIKeyboardRaiser))]
    [DTTHeader("dtt.keyboardraiser")]
    internal class UIKeyboardRaiserEditor : DTTInspector
    {
        /// <summary>
        /// Contains the GUIContent for the <see cref="UIKeyboardRaiserEditor"/>.
        /// </summary>
        private UIKeyboardRaiserGUIContentCache _contentCache;

        /// <summary>
        /// Contains the Serialized Properties for the <see cref="UIKeyboardRaiserEditor"/>.
        /// </summary>
        private UIKeyboardRaiserSerializedPropertyCache _propertyCache;

        /// <summary>
        /// The foldout to use for the debug information.
        /// </summary>
        private AnimatedFoldout _debugFoldout;

        /// <summary>
        /// The casted target of this editor.
        /// </summary>
        private UIKeyboardRaiser _keyboardRaiser;

        /// <summary>
        /// The canvas the target of this is editor is in.
        /// </summary>
        private Canvas _canvas;

        /// <summary>
        /// References the <see cref="KeyboardRaiserConfigPropertyCache"/> from the <see cref="KeyboardRaiserAssetDatabase"/>.
        /// </summary>
        private KeyboardRaiserConfigPropertyCache configPropertyCache => KeyboardRaiserAssetDatabase.ConfigPropertyCache;

        /// <summary>
        /// Selected option from the editor keyboard toolbar.
        /// </summary>
        private static int _selectedEditorKeyboardOption = 1;

        /// <summary>
        /// Color of the treshold area.
        /// </summary>
        private readonly Color _thresholdAreaColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        /// <summary>
        /// Sets up all the required caches.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            _contentCache = new UIKeyboardRaiserGUIContentCache();
            _propertyCache = new UIKeyboardRaiserSerializedPropertyCache(serializedObject);
            _debugFoldout = new AnimatedFoldout(this, _propertyCache.PaddingProperty.isExpanded, true);
            _keyboardRaiser = (UIKeyboardRaiser)target;
            _canvas = _keyboardRaiser.GetComponentInParent<Canvas>();
        }

        /// <summary>
        /// Draws the required fields.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();

            serializedObject.Update();

            EditorGUILayout.PropertyField(_propertyCache.PaddingProperty, _contentCache.PaddingContent);
            EditorGUILayout.PropertyField(_propertyCache.IsSmoothProperty, _contentCache.IsSmoothContent);
            EditorGUILayout.PropertyField(_propertyCache.HintsProperty);

            // Doesn't draw the debug section if the config is missing.
            if (KeyboardRaiserAssetDatabase.Config == null)
            {
                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();

                return;
            }

            configPropertyCache.UpdateRepresentation();
            _propertyCache.PaddingProperty.isExpanded = _debugFoldout.OnGUI("Debug Options", () =>
            {
                EditorGUILayout.PropertyField(configPropertyCache.enablePaddingArrow, _contentCache.PaddingArrowContent);
                EditorGUILayout.PropertyField(configPropertyCache.enableTresholdArea, _contentCache.DebugThresholdAreaContent);
            });

            EditorGUILayout.Space();

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.LabelField(_contentCache.KeyboardControlHeaderContent, labelStyle);

            EditorGUILayout.PropertyField(configPropertyCache.amountOccupied, _contentCache.AmountOccupiedSliderContent);
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            {
                int newOption = GUILayout.Toolbar(Application.isPlaying ? _selectedEditorKeyboardOption : -1, new[] { "Open", "Close" });
                if (_selectedEditorKeyboardOption != newOption)
                {
                    KeyboardStateManager.openingField.Add(_keyboardRaiser);

                    Debug.Log($"openingF {KeyboardStateManager.openingField}");

                    if (newOption == 0)
                    {
                        EditorKeyboard.Open(KeyboardRaiserAssetDatabase.Config.AmountOccupied);                       
                    }     
                    else if (newOption == 1)
                    {
                        EditorKeyboard.Close();
                    }
                        
                }

                _selectedEditorKeyboardOption = newOption;

                if(_selectedEditorKeyboardOption == 1)
                {
                    KeyboardStateManager.openingField.Remove(_keyboardRaiser);
                    KeyboardStateManager.openingField.RemoveAll(item => item == null);
                }

            }
            EditorGUI.EndDisabledGroup();

            if (EditorGUI.EndChangeCheck())
            {
                configPropertyCache.ApplyChanges();
                serializedObject.ApplyModifiedProperties();

                SceneView.RepaintAll();
            }
        }

        /// <summary>
        /// Draws the scene debug information.
        /// </summary>
        private void OnSceneGUI()
        {
            if (_canvas == null)
                return;
            if (Selection.activeGameObject != _keyboardRaiser.gameObject)
                return;
            if (_canvas.transform.rotation != Quaternion.identity)
                return;

            Color original = Handles.color;

            // Draw starting line keyboard raising.
            Handles.color = Color.red;

            Rect canvasRect = _canvas.GetRectTransform().GetWorldRect();
            Rect rect = _keyboardRaiser.transform.GetRectTransform().GetWorldRect();
            float padding = _propertyCache.PaddingProperty.floatValue * _keyboardRaiser.transform.lossyScale.y;

            DrawStartingLine(canvasRect, rect, padding);

            // Draw padding distance arrows.
            if (KeyboardRaiserAssetDatabase.Config.EnablePaddingArrow)
                DrawPaddingArrow(rect, padding);

            // Draw darkened bottom.
            if (KeyboardRaiserAssetDatabase.Config.EnableTresholdArea)
                DrawThresholdArea(canvasRect, rect, padding);

            Handles.color = original;
        }

        /// <summary>
        /// Draws the starting line for showing the threshold.
        /// </summary>
        /// <param name="canvasRect">The rect of the canvas.</param>
        /// <param name="rect">The rect of the selected transform.</param>
        /// <param name="padding">The amount of padding in world units.</param>
        private void DrawStartingLine(Rect canvasRect, Rect rect, float padding)
        {
            Vector3 pointA = new Vector3(canvasRect.xMin, rect.yMin - padding, 0);
            Vector3 pointB = new Vector3(canvasRect.xMax, rect.yMin - padding, 0);
            Handles.DrawLine(pointA, pointB);
        }

        /// <summary>
        /// Draws the arrows for indicating the padding.
        /// </summary>
        /// <param name="rect">The rect of the selected transform.</param>
        /// <param name="padding">The amount of padding in world units.</param>
        private void DrawPaddingArrow(Rect rect, float padding)
        {
            const float MAX_ARROW_CAP_LENGTH = 15;
            Handles.color = Color.red;

            Vector3 a = new Vector3(rect.x + rect.width / 2, rect.yMin);
            Vector3 b = new Vector3(rect.x + rect.width / 2, rect.yMin - padding);

            float distance = Mathf.Abs(a.y - b.y) * _keyboardRaiser.transform.lossyScale.y;
            float length = MAX_ARROW_CAP_LENGTH;
            const float NORMALIZED_EARLY_THRESHOLD = 0.3f;
            float earlyThreshold = distance * NORMALIZED_EARLY_THRESHOLD;
            if (distance < MAX_ARROW_CAP_LENGTH * 2 + earlyThreshold)
                length = distance / 2 - earlyThreshold / 2;

            HandlesLibrary.DrawArrowLine(a, b, length);
        }

        /// <summary>
        /// Draws the area for indicating the threshold.
        /// </summary>
        /// <param name="canvasRect">The rect of the canvas.</param>
        /// <param name="rect">The rect of the selected transform.</param>
        /// <param name="padding">The amount of padding in world units.</param>
        private void DrawThresholdArea(Rect canvasRect, Rect rect, float padding)
        {
            Handles.color = _thresholdAreaColor;
            Vector3 startingPoint = new Vector3(canvasRect.xMin, rect.yMin - padding, 0);

            // Stops drawing the box if the objects threshold is below the canvas starting point.
            if (startingPoint.y < canvasRect.yMin)
                return;

            Rect bottomRect = new Rect(startingPoint, new Vector2(canvasRect.width, (canvasRect.yMin - rect.yMin) + padding));
            Handles.DrawSolidRectangleWithOutline(bottomRect, Color.black, Color.clear);
        }
    }
}
#endif