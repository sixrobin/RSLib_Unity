namespace RSLib.Editor
{
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditorInternal;

	/// <summary>
	/// Draws a layer mask field in a custom inspector.
	/// </summary>
	public class LayerMaskFieldEditor
	{
		private static readonly List<int> LAYER_NUMBERS = new List<int>();

		public static LayerMask LayerMaskField(string label, LayerMask layerMask)
		{
			string[] layers = InternalEditorUtility.layers;
			LAYER_NUMBERS.Clear();

			for (int i = 0; i < layers.Length; ++i)
				LAYER_NUMBERS.Add(LayerMask.NameToLayer(layers[i]));

			int maskWithoutEmpty = 0;
			for (int i = 0; i < LAYER_NUMBERS.Count; ++i)
				if (((1 << LAYER_NUMBERS[i]) & layerMask.value) > 0)
					maskWithoutEmpty |= 1 << i;

			maskWithoutEmpty = UnityEditor.EditorGUILayout.MaskField(label, maskWithoutEmpty, layers);

			int mask = 0;
			for (int i = 0; i < LAYER_NUMBERS.Count; ++i)
				if ((maskWithoutEmpty & (1 << i)) > 0)
					mask |= 1 << LAYER_NUMBERS[i];

			layerMask.value = mask;
			return layerMask;
		}
	}
}