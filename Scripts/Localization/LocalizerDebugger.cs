namespace RSLib.Unity.Localization
{
    using UnityEngine;

    [DisallowMultipleComponent]
    public sealed class LocalizerDebugger : MonoBehaviour
    {
        #if RSLIB
        private void Awake()
        {
            RSLib.Unity.Debug.Console.DebugConsole.OverrideCommand<string>("locKey", "Localizes a given key.", key => Localizer.Instance.Log(Localizer.Get(key), forceVerbose: true));
            RSLib.Unity.Debug.Console.DebugConsole.OverrideCommand<string>("locSetLanguage", "Set language.", Localizer.SetCurrentLanguage);
            RSLib.Unity.Debug.Console.DebugConsole.OverrideCommand<int>("locSetLanguageIndex", "Set language index.", Localizer.SetCurrentLanguage);
            
            RSLib.Unity.Debug.Console.DebugConsole.OverrideCommand("locShowLanguages", "Shows handled languages.",
                () =>
                {
                    for (int i = 0; i < Localizer.Instance.Languages.Length; ++i)
                    {
                        Localizer.Instance.Log(Localizer.Instance.Languages[i], forceVerbose: true);
                        RSLib.Unity.Debug.Console.DebugConsole.LogExternal(Localizer.Instance.Languages[i]);
                    }
                });
        }

        private void Update()
        {
            RSLib.Unity.Debug.ValuesDebugger.DebugValue("localization_language", () => Localizer.Instance.Language);
        }
        #endif
    }
}