namespace RSLib.Extensions
{
    using System.Xml.Linq;

    public static class XDocumentExtensions
    {
        #region GENERAL

        /// <summary>Checks if an XAttribute or its value are null.</summary>
        public static bool IsNullOrEmpty(this XAttribute attribute)
        {
            return attribute == null || string.IsNullOrEmpty(attribute.Value);
        }

        /// <summary>Checks if an XElement or its value are null.</summary>
        public static bool IsNullOrEmpty(this XElement element)
        {
            return element == null || string.IsNullOrEmpty(element.Value);
        }

        #endregion

        #region PARSING

        /// <summary>Parses a XElement value to a float value.</summary>
        /// <returns>Element value if parsing succeeded, else 0.</returns>
        public static float ValueToFloat(this XElement element)
        {
            UnityEngine.Assertions.Assert.IsFalse(element.IsNullOrEmpty(), $"XElement is null or empty.");

            if (float.TryParse(element.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float value))
                return value;

            UnityEngine.Debug.LogError($"Could not parse XElement {element.Name.LocalName} Value {element.Value} to a valid float value.");
            return 0f;
        }

        /// <summary>Parses a XAttribute value to a float value.</summary>
        /// <returns>Attribute value if parsing succedded, else 0.</returns>
        public static float ValueToFloat(this XAttribute attribute)
        {
            UnityEngine.Assertions.Assert.IsFalse(attribute.IsNullOrEmpty(), $"XAttribute is null or empty.");

            if (float.TryParse(attribute.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float value))
                return value;

            UnityEngine.Debug.LogError($"Could not parse XAttribute {attribute.Name.LocalName} Value {attribute.Value} to a valid float value.");
            return 0f;
        }

        /// <summary>Parses a XElement value to an integer value.</summary>
        /// <returns>Element value if parsing succeeded, else 0.</returns>
        public static int ValueToInt(this XElement element)
        {
            UnityEngine.Assertions.Assert.IsFalse(element.IsNullOrEmpty(), $"XElement is null or empty.");

            if (int.TryParse(element.Value, out int value))
                return value;

            UnityEngine.Debug.LogError($"Could not parse XElement {element.Name.LocalName} Value {element.Value} to a valid integer value.");
            return 0;
        }

        /// <summary>Parses a XAttribute value to an integer value.</summary>
        /// <returns>Attribute value if parsing succedded, else 0.</returns>
        public static int ValueToInt(this XAttribute attribute)
        {
            UnityEngine.Assertions.Assert.IsFalse(attribute.IsNullOrEmpty(), $"XAttribute is null or empty.");

            if (int.TryParse(attribute.Value, out int value))
                return value;

            UnityEngine.Debug.LogError($"Could not parse XAttribute {attribute.Name.LocalName} Value {attribute.Value} to a valid integer value.");
            return 0;
        }

        /// <summary>Parses a XElement value to a boolean value.</summary>
        /// <returns>Element value if parsing succeeded, else false.</returns>
        public static bool ValueToBool(this XElement element)
        {
            UnityEngine.Assertions.Assert.IsFalse(element.IsNullOrEmpty(), $"XElement is null or empty.");

            if (bool.TryParse(element.Value, out bool value))
                return value;

            UnityEngine.Debug.LogError($"Could not parse XElement {element.Name.LocalName} Value {element.Value} to a valid boolean value.");
            return false;
        }

        /// <summary>Parses a XAttribute value to a boolean value.</summary>
        /// <returns>Attribute value if parsing succedded, else false.</returns>
        public static bool ValueToBool(this XAttribute attribute)
        {
            UnityEngine.Assertions.Assert.IsFalse(attribute.IsNullOrEmpty(), $"XAttribute is null or empty.");

            if (bool.TryParse(attribute.Value, out bool value))
                return value;

            UnityEngine.Debug.LogError($"Could not parse XAttribute {attribute.Name.LocalName} Value {attribute.Value} to a valid boolean value.");
            return false;
        }

        /// <summary>Parses a XElement value to an enum value.</summary>
        /// <returns>Element value if parsing succeeded, else default value.</returns>
        public static T ValueToEnum<T>(this XElement element) where T : System.Enum
        {
            UnityEngine.Assertions.Assert.IsFalse(element.IsNullOrEmpty(), $"XElement is null or empty.");

            if (!System.Enum.IsDefined(typeof(T), element.Value))
            {
                UnityEngine.Debug.LogError($"Could not parse XElement {element.Name.LocalName} Value {element.Value} to a valid enum value.");
                return default;
            }

            return (T)System.Enum.Parse(typeof(T), element.Value);
        }

        /// <summary>Parses a XAttribute value to an enum value.</summary>
        /// <returns>Element value if parsing succeeded, else default value.</returns>
        public static T ValueToEnum<T>(this XAttribute attribute) where T : System.Enum
        {
            UnityEngine.Assertions.Assert.IsFalse(attribute.IsNullOrEmpty(), $"XAttribute is null or empty.");

            if (!System.Enum.IsDefined(typeof(T), attribute.Value))
            {
                UnityEngine.Debug.LogError($"Could not parse XAttribute {attribute.Name.LocalName} Value {attribute.Value} to a valid enum value.");
                return default;
            }

            return (T)System.Enum.Parse(typeof(T), attribute.Value);
        }

        /// <summary>Parses a XElement Min and Max attributes to a float Tuple.</summary>
        /// <returns>Attributes values if parsing succeeded, else, each failed value will be 0.</returns>
        public static (float min, float max) MinMaxAttributesToFloats(this XElement element)
        {
            UnityEngine.Assertions.Assert.IsTrue(element != null, $"XElement is null.");

            XAttribute minAttribute = element.Attribute("Min");
            UnityEngine.Assertions.Assert.IsFalse(minAttribute.IsNullOrEmpty(), $"Min attribute is missing or empty.");

            XAttribute maxAttribute = element.Attribute("Max");
            UnityEngine.Assertions.Assert.IsFalse(maxAttribute.IsNullOrEmpty(), $"Max attribute is missing or empty.");

            float min = minAttribute.ValueToFloat();
            float max = maxAttribute.ValueToFloat();
            return (min, max);
        }

        #endregion
    }
}