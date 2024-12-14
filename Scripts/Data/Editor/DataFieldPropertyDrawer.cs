namespace RSLib.Data.Editor
{
    using UnityEngine;
    using UnityEditor;
    
    public abstract class DataFieldPropertyDrawer : PropertyDrawer
    {
        protected abstract string DataFieldName { get; }
        protected abstract string ValueFieldName { get; }

        protected virtual bool AddButton => false;
        protected virtual string ButtonText => "";

        protected virtual void OnButtonClicked(SerializedProperty property) { }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProperty = property.FindPropertyRelative(DataFieldName);
            return EditorGUI.GetPropertyHeight(valueProperty);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty useDataProperty = property.FindPropertyRelative("_useDataValue");
            SerializedProperty valueProperty = property.FindPropertyRelative(ValueFieldName);
            SerializedProperty dataProperty = property.FindPropertyRelative(DataFieldName);

            position.width -= AddButton ? 48 : 24;

            EditorGUI.PropertyField(position, useDataProperty.boolValue ? dataProperty : valueProperty, label, true);
            
            position.x += position.width + 24;
            position.width = EditorGUI.GetPropertyHeight(useDataProperty);
            position.height = position.width;
            position.x -= position.width;

            if (AddButton)
            {
                if (GUI.Button(position, ButtonText))
                    this.OnButtonClicked(property);
                
                position.x += position.width + 24;
                position.x -= position.width;
            }

            EditorGUI.PropertyField(position, useDataProperty, GUIContent.none);
        }
    }
}
