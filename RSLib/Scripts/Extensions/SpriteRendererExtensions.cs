namespace RSLib.Extensions
{
	using UnityEngine;

	public static class SpriteRendererExtensions
	{
		#region FLIP

		/// <summary>
		/// Flips the SpriteRenderer on the X axis.
		/// </summary>
		public static void FlipX(this SpriteRenderer spriteRenderer)
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}

		/// <summary>
		/// Flips the SpriteRenderer on the Y axis.
		/// </summary>
		public static void FlipY(this SpriteRenderer spriteRenderer)
		{
			spriteRenderer.flipY = !spriteRenderer.flipY;
		}

		#endregion FLIP
	}
}