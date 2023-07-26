using System;
using DTT.Utils.Extensions;
using UnityEngine;
using Doozy.Runtime.Signals;
using UnityEngine.UI;



namespace DTT.KeyboardRaiser
{

    public class UIKeyboardRaiser : MonoBehaviour
    {
        
        public bool IsSmooth
        {
            get => _isSmooth;
            set => _isSmooth = value;
        }

        /// The amount of padding on the bottom from where the object should start being dragged by the keyboard.
        public float Padding
        {
            get => _padding;
            set => _padding = value;
        }

        /// The keyboard state that's being used to gain keyboard information.
        public IKeyboardState KeyboardState
        {
            get => _keyboardState;
            set => _keyboardState = value ?? new NullKeyboardState();
        }

        /// The amount of padding on the bottom from where the object should start being dragged by the keyboard.
        [SerializeField]
        private float _padding;

        /// If enabled the object being raised will be moved smoothly instead of instantly.
        [SerializeField]
        private bool _isSmooth;
        
        /// The state that's being used to gain keyboard information.
        private IKeyboardState _keyboardState;
        
        /// The position before the raising so we know where to return to after keyboard lowering.
        private Vector3 _originalPosition;
        
        /// The rect before the raising so we know where to return to after keyboard lowering.
        private Rect _originalRect;
        
        /// The canvas this component is being contained in.
        private Canvas _canvas;
        
        /// The position we want to go to when the keyboard raises, so we can move there slowly.
        private Vector3 _targetPos;
        
        /// The rect transform of this game object.
        private RectTransform _rectTransform;

        /// The time when the keyboard was lowered last.
        private float _timeOfLastLowering;
        
        private const float TIMEOUT_DURATION = 0.5f;


        private Canvas _myCanvas;
        private LayoutElement _myLayoutElement;



        private void Start()
        {

            _originalPosition = transform.position;
            _originalRect = _rectTransform.GetWorldRect();

            // создаем и заполняем доп элементы управления
            _canvas = GetComponentInParent<Canvas>(true);

            _myCanvas = gameObject.AddComponent<Canvas>();
            _myCanvas.overrideSorting = true;
            _myCanvas.sortingOrder = _canvas.sortingOrder + 2;
            _myCanvas.overrideSorting = false;

            _myLayoutElement = gameObject.AddComponent<LayoutElement>();
            
            gameObject.AddComponent<GraphicRaycaster>();

        }



        private void OnEnable()
        {

            _rectTransform = transform.GetRectTransform();
            _keyboardState = KeyboardStateManager.Current;
            _originalPosition = transform.position;
            _originalRect = _rectTransform.GetWorldRect();

            _keyboardState.Raised += OnKeyboardRaised;
            _keyboardState.Lowered += OnKeyboardLowered;

        }



        private void OnDisable()
        {
            _keyboardState.Raised -= OnKeyboardRaised;
            _keyboardState.Lowered -= OnKeyboardLowered;
        }



        private void OnKeyboardRaised()
        {
            if (Time.time - _timeOfLastLowering > TIMEOUT_DURATION)
            {
                _originalPosition = transform.position;
                _originalRect = _rectTransform.GetWorldRect();

                if (KeyboardStateManager.openingField != this) return;

                _myLayoutElement.ignoreLayout = true;
                _myCanvas.overrideSorting = true;
                Signal.Send("BG", "KeyboardTask", true);
            }
        }
        


        private void OnKeyboardLowered()
        {
            _timeOfLastLowering = Time.time;

            if (KeyboardStateManager.openingField != this) return;

            _myLayoutElement.ignoreLayout = false;
            _myCanvas.overrideSorting = false;
            Signal.Send("BG", "KeyboardTask", false);
        }



        private void Update()
        {
            if (Time.time - _timeOfLastLowering > TIMEOUT_DURATION && !_keyboardState.IsRaised)
                return;


            if (KeyboardStateManager.openingField != this) return;


            float delta = 0;
            if (_canvas != null && _keyboardState.IsRaised)
            {
                Rect canvasRect = _canvas.GetRectTransform().GetWorldRect();

                delta = _keyboardState.ProportionalHeight * canvasRect.height - (_originalRect.yMin - _padding * transform.lossyScale.y);
            }

            _targetPos = _originalPosition + Vector3.up * delta;
            transform.position = Vector3.Lerp(transform.position, _targetPos, _isSmooth ? Time.deltaTime * 10 : 1);
        
        }

    }

}