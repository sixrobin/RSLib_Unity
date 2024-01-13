namespace RSLib.Data.Editor
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ColorField))]
    public class ColorFieldPropertyDrawer : DataFieldPropertyDrawer
    {
        protected override string DataFieldName => "_dataColor";
        protected override string ValueFieldName => "_valueColor";

        protected override bool AddButton => true;
        protected override string ButtonText => "#";

        protected override void OnButtonClicked(SerializedProperty property)
        {
            bool useDataProperty = property.FindPropertyRelative("_useDataValue").boolValue;
            UnityEngine.Color valueProperty = property.FindPropertyRelative(ValueFieldName).colorValue;
            Color dataFloatProperty = (Color)property.FindPropertyRelative(DataFieldName).boxedValue;

            string hexCode = useDataProperty ? dataFloatProperty.HexCode : UnityEngine.ColorUtility.ToHtmlStringRGBA(valueProperty);

            UnityEngine.GUIUtility.systemCopyBuffer = hexCode;
            UnityEngine.Debug.Log($"Copied {hexCode} to clipboard.");
        }
    }
}