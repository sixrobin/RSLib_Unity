namespace RSLib.Framework.GameSettings.Example
{
    public class CustomGameSettingsManagerExample : GameSettingsManager
    {
        public CustomFloatSettingExample FloatExample { get; private set; }

        protected override string SavePath => System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, "Save", "TmpSettings.xml");

        protected override void SaveCustomSettings(System.Xml.Linq.XContainer container)
        {
            base.SaveCustomSettings(container);
            container.Add(FloatExample.Save());
        }

        protected override void LoadCustomSettings(System.Xml.Linq.XElement gameSettingsElement)
        {
            base.LoadCustomSettings(gameSettingsElement);
            FloatExample = new CustomFloatSettingExample(gameSettingsElement.Element(FloatExample.SerializationName));
        }

        public override void Init()
        {
            base.Init();
            FloatExample = new CustomFloatSettingExample();
        }
    }
}