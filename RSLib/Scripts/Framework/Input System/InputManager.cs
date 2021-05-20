namespace RSLib.Framework.InputSystem
{
    using System.Xml;
    using System.Xml.Linq;
    using UnityEngine;

    public partial class InputManager : Singleton<InputManager>
    {
        [SerializeField] private InputMapDatas _defaultMapDatas = null;
        [SerializeField] private bool _disableMapLoading = false;
        [SerializeField] private KeyCode _cancelAssignKey = KeyCode.Escape;

        public delegate void KeyAssignedEventHandler(string actionId, KeyCode btn, bool alt);

        private static InputMap s_inputMap;

        private static System.Collections.IEnumerator s_assignKeyCoroutine;
        private static KeyCode[] s_allKeyCodes;

        public static bool IsAssigningKey => s_assignKeyCoroutine != null;

        public static void RestoreMapToDefault(InputMap map)
        {
            map = GetDefaultMapCopy();
        }

        public static InputMap GetDefaultMapCopy()
        {
            return new InputMap(Instance._defaultMapDatas);
        }

        public static void AssignKey(string actionId, bool alt, KeyAssignedEventHandler callback = null)
        {
            AssignKey(s_inputMap, actionId, alt, callback);
        }

        public static void AssignKey(InputMap map, string actionId, bool alt, KeyAssignedEventHandler callback = null)
        {
            if (IsAssigningKey)
                return;

            Instance.StartCoroutine(s_assignKeyCoroutine = AssignKeyCoroutine(map, actionId, alt, callback));
        }

        public static bool GetInput(string actionId)
        {
            (KeyCode btn, KeyCode altBtn) = s_inputMap.GetActionKeyCodes(actionId);
            return Input.GetKey(btn) || Input.GetKey(altBtn);
        }

        public static bool GetAnyInput(params string[] actionsIds)
        {
            for (int i = actionsIds.Length - 1; i >= 0; --i)
                if (GetInput(actionsIds[i]))
                    return true;

            return false;
        }

        public static bool GetInputDown(string actionId)
        {
            (KeyCode btn, KeyCode altBtn) = s_inputMap.GetActionKeyCodes(actionId);
            return Input.GetKeyDown(btn) || Input.GetKeyDown(altBtn);
        }
        
        public static bool GetAnyInputDown(params string[] actionsIds)
        {
            for (int i = actionsIds.Length - 1; i >= 0; --i)
                if (GetInputDown(actionsIds[i]))
                    return true;

            return false;
        }

        public static bool GetInputUp(string actionId)
        {
            (KeyCode btn, KeyCode altBtn) = s_inputMap.GetActionKeyCodes(actionId);
            return Input.GetKeyUp(btn) || Input.GetKeyUp(altBtn);
        }

        public static bool GetAnyInputUp(params string[] actionsIds)
        {
            for (int i = actionsIds.Length - 1; i >= 0; --i)
                if (GetInputUp(actionsIds[i]))
                    return true;

            return false;
        }

        public static System.Collections.Generic.Dictionary<string, (KeyCode btn, KeyCode altBtn)> GetMapCopy()
        {
            return s_inputMap.MapCopy;
        }

        public static void SetBindings(System.Collections.Generic.Dictionary<string, (KeyCode btn, KeyCode altBtn)> bindings)
        {
            s_inputMap = new InputMap(bindings);
        }

        public static void SetMap(InputMap map)
        {
            s_inputMap = new InputMap(map);
        }

        private static void GenerateMissingInputsFromSave()
        {
            for (int i = Instance._defaultMapDatas.Bindings.Length - 1; i >= 0; --i)
            {
                if (s_inputMap.HasAction(Instance._defaultMapDatas.Bindings[i].ActionId))
                    continue;

                Instance.Log($"Loading missing binding from InputSave for action Id {Instance._defaultMapDatas.Bindings[i].ActionId}");
                s_inputMap.CreateAction(Instance._defaultMapDatas.Bindings[i].ActionId, Instance._defaultMapDatas.Bindings[i].KeyCodes);
            }

            s_inputMap.SortActionsBasedOnDatas(Instance._defaultMapDatas);
            SaveCurrentMap();
        }

        private static System.Collections.IEnumerator AssignKeyCoroutine(InputMap map, string actionId, bool alt, KeyAssignedEventHandler callback = null)
        {
            Instance.Log($"Assigning key for action Id {actionId}...");

            for (int i = 0; i < 2; ++i)
                yield return Yield.SharedYields.WaitForEndOfFrame;

            yield return new WaitUntil(() => Input.anyKeyDown);

            if (Instance._cancelAssignKey != KeyCode.None && Input.GetKeyDown(Instance._cancelAssignKey))
            {
                Instance.Log($"Cancelling key assignment for action Id {actionId}.");
                callback?.Invoke(actionId, alt ? map.GetActionKeyCodes(actionId).altBtn : map.GetActionKeyCodes(actionId).btn, alt);
                s_assignKeyCoroutine = null;
                yield break;
            }

            KeyCode key = KeyCode.None;
            for (int i = s_allKeyCodes.Length - 1; i >= 0; --i)
                if (Input.GetKeyDown(s_allKeyCodes[i]))
                    key = s_allKeyCodes[i];

            Instance.Log($"Assigning key {key.ToString()} to {(alt ? "alternate " : "")}button for action Id {actionId}.");
            map.SetActionButton(actionId, key, alt);
            callback?.Invoke(actionId, key, alt);

            s_assignKeyCoroutine = null;
        }

        protected override void Awake()
        {
            base.Awake();

            if (_disableMapLoading || !TryLoadMap())
                RestoreMapToDefault(s_inputMap);
            else
                GenerateMissingInputsFromSave();

            SaveCurrentMap();
            s_allKeyCodes = Helpers.GetEnumValues<KeyCode>();
        }
    }

    public partial class InputManager : Singleton<InputManager>
    {
        private static string SavePath => $"{Application.persistentDataPath}/InputSettings.xml";

        public static void SaveCurrentMap()
        {
            Instance.Log($"Saving input map to {SavePath}...");

            XContainer container = new XElement("InputMap");

            foreach (System.Collections.Generic.KeyValuePair<string, (KeyCode btn, KeyCode altBtn)> binding in s_inputMap.MapCopy)
            {
                if (!s_inputMap.HasAction(binding.Key))
                    continue;

                XElement actionElement = new XElement(binding.Key);
                (KeyCode btn, KeyCode altBtn) = s_inputMap.GetActionKeyCodes(binding.Key);
                actionElement.Add(new XAttribute("Btn", btn.ToString()));
                actionElement.Add(new XAttribute("Alt", altBtn.ToString()));

                container.Add(actionElement);
            }

            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(SavePath);
                if (!fileInfo.Directory.Exists)
                    System.IO.Directory.CreateDirectory(fileInfo.DirectoryName);

                byte[] buffer;
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true, Encoding = System.Text.Encoding.UTF8 }))
                    {
                        XDocument saveDocument = new XDocument();
                        saveDocument.Add(container);
                        saveDocument.Save(xmlWriter);
                    }

                    buffer = ms.ToArray();
                }

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer))
                {
                    using (System.IO.FileStream diskStream = System.IO.File.Open(SavePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        ms.CopyTo(diskStream);
                    }
                }
            }
            catch (System.Exception e)
            {
                Instance.LogError($"Could not save Input map at {SavePath} ! Exception message:\n{e.ToString()}");
            }
        }

        public static bool TryLoadMap()
        {
            if (!System.IO.File.Exists(SavePath))
                return false;

            try
            {
                Instance.Log($"Loading input map from {SavePath}...");

                XContainer container = XDocument.Parse(System.IO.File.ReadAllText(SavePath));
                s_inputMap = new InputMap(container);
                SaveCurrentMap();
            }
            catch (System.Exception e)
            {
                Instance.LogError($"Could not load Input map from {SavePath} ! Exception message:\n{e.ToString()}");
                return false;
            }

            return true;
        }

        [ContextMenu("Save Current Map")]
        private void DebugSaveCurrentMap()
        {
            SaveCurrentMap();
        }
    }
}