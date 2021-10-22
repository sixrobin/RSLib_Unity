﻿namespace RSLib.Framework.InputSystem
{
    using UnityEngine;

    /// <summary>
    /// Scriptable object meant to create a complete input mapping from the editor.
    /// Can be used for default input mapping or testing.
    /// </summary>
    [CreateAssetMenu(fileName = "New Input Map", menuName = "RSLib/Input Map")]
    public class InputMapDatas : ScriptableObject
    {
        [System.Serializable]
        public class KeyBinding
        {
            [SerializeField] private string _actionId = string.Empty;
            [SerializeField] private KeyCode _btn = KeyCode.None;
            [SerializeField] private KeyCode _altBtn = KeyCode.None;
            [SerializeField] private bool _userAssignable = true;

            public KeyBinding(string actionId, (KeyCode btn, KeyCode altBtn) btns, bool userAccessible)
            {
                _actionId = actionId;
                SetBtns(btns.btn, btns.altBtn);
                _userAssignable = userAccessible;
            }

            public string ActionId => _actionId;
            public (KeyCode btn, KeyCode altBtn) KeyCodes => (_btn, _altBtn);
            public bool UserAssignable => _userAssignable;

            public void SetBtns(KeyCode btn, KeyCode altBtn)
            {
                SetBtn(btn);
                SetAltBtn(altBtn);
            }

            public void SetBtn(KeyCode keyCode)
            {
                _btn = keyCode;
            }

            public void SetAltBtn(KeyCode keyCode)
            {
                _altBtn = keyCode;
            }

            public void SetUserAssignable(bool value)
            {
                _userAssignable = value;
            }
        }

        [SerializeField] private KeyBinding[] _bindings = null;

        public KeyBinding[] Bindings => _bindings;
    }
}