namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    public class InputController
    {
        public const string HORIZONTAL_KEYBOARD = "Horizontal";
        public const string HORIZONTAL_CONTROLLER = "HorizontalLeftStick";
        public const string VERTICAL_KEYBOARD = "Vertical";
        public const string VERTICAL_CONTROLLER = "VerticalLeftStick";
        public const string JUMP = "Jump";
        public const string DASH = "Roll";

        private MonoBehaviour _coroutinesRunner;

        private System.Collections.Generic.Dictionary<ButtonCategory, InputGetterHandler> _inputGetters;
        private System.Collections.Generic.Dictionary<ButtonCategory, System.Collections.IEnumerator> _inputStoreCoroutines;
        private System.Collections.Generic.Dictionary<ButtonCategory, float> _inputDelaysByCategory;
        private ButtonCategory _delayedInputs = ButtonCategory.NONE;

        public InputController(PlayerInputData inputData, MonoBehaviour coroutinesRunner)
        {
            InputData = inputData;
            _coroutinesRunner = coroutinesRunner;
            Init();
        }

        private delegate bool InputGetterHandler();

        [System.Flags]
        public enum ButtonCategory
        {
            NONE = 0,
            JUMP = 1,
            DASH = 2,
            ANY = JUMP | DASH
        }

        public PlayerInputData InputData { get; }

        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }

        public float CurrentHorizontalDirection => Mathf.Sign(Horizontal);
        public float CurrentVerticalDirection => Mathf.Sign(Vertical);

        protected virtual float GetDeadZoneSqr()
        {
            return InputData.DefaultDeadZone * InputData.DefaultDeadZone;
        }
        
        public bool CheckInput(ButtonCategory btnCategory)
        {
            return btnCategory == ButtonCategory.ANY ? _delayedInputs != ButtonCategory.NONE : _delayedInputs.HasFlag(btnCategory);
        }

        public virtual bool CheckJumpInput()
        {
            return Input.GetButton(JUMP);
        }

        public virtual bool CheckJumpInputUp()
        {
            return Input.GetButtonUp(JUMP);
        }

        public virtual float GetHorizontalInput()
        {
            float horizontal = Input.GetAxisRaw(HORIZONTAL_KEYBOARD);
            float leftStickHorizontal = Input.GetAxis(HORIZONTAL_CONTROLLER);
            if (leftStickHorizontal * leftStickHorizontal > GetDeadZoneSqr())
                horizontal = leftStickHorizontal;

            return horizontal;
        }

        public virtual float GetVerticalInput()
        {
            float vertical = Input.GetAxisRaw(VERTICAL_KEYBOARD);
            float leftStickVertical = Input.GetAxis(VERTICAL_CONTROLLER);
            if (leftStickVertical * leftStickVertical > GetDeadZoneSqr())
                vertical = leftStickVertical;

            return vertical;
        }

        public virtual void Update()
        {
            Horizontal = GetHorizontalInput();
            Vertical = GetVerticalInput();

            foreach (System.Collections.Generic.KeyValuePair<ButtonCategory, InputGetterHandler> input in _inputGetters)
            {
                if (input.Value() == true)
                {
                    ResetDelayedInput(input.Key);
                    _delayedInputs |= input.Key;
                    _inputStoreCoroutines[input.Key] = StoreInputCoroutine(input.Key);
                    _coroutinesRunner.StartCoroutine(_inputStoreCoroutines[input.Key]);
                }
            }
        }

        public virtual void Reset()
        {
            Horizontal = 0f;
            Vertical = 0f;
        }

        public void ResetDelayedInput(ButtonCategory btnCategory)
        {
            if (_inputStoreCoroutines.TryGetValue(btnCategory, out System.Collections.IEnumerator storeCoroutine) && storeCoroutine != null)
            {
                _coroutinesRunner.StopCoroutine(storeCoroutine);
                storeCoroutine = null;
            }

            _delayedInputs &= ~btnCategory;
        }

        protected virtual void Init()
        {
            _inputGetters = new System.Collections.Generic.Dictionary<ButtonCategory, InputGetterHandler>(new RSLib.Framework.Comparers.EnumComparer<ButtonCategory>())
            {
                { ButtonCategory.JUMP, () => Input.GetButtonDown(JUMP) },
                { ButtonCategory.DASH, () => Input.GetButtonDown(DASH) }
            };

            _inputDelaysByCategory = new System.Collections.Generic.Dictionary<ButtonCategory, float>(new RSLib.Framework.Comparers.EnumComparer<ButtonCategory>())
            {
                { ButtonCategory.JUMP, InputData.JumpInputDelay },
                { ButtonCategory.DASH, InputData.RollInputDelay }
            };

            _inputStoreCoroutines = new System.Collections.Generic.Dictionary<ButtonCategory, System.Collections.IEnumerator>(new RSLib.Framework.Comparers.EnumComparer<ButtonCategory>())
            {
                { ButtonCategory.JUMP, null },
                { ButtonCategory.DASH, null }
            };
        }

        private System.Collections.IEnumerator StoreInputCoroutine(ButtonCategory btnCategory)
        {
            if (!_inputDelaysByCategory.ContainsKey(btnCategory))
                yield break;

            _delayedInputs &= btnCategory;
            yield return RSLib.Yield.SharedYields.WaitForSeconds(_inputDelaysByCategory[btnCategory]);
            _delayedInputs &= ~btnCategory;
        }
    }
}