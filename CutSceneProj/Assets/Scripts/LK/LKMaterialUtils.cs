using System;
using UnityEngine;
public static class LKMaterialUtils
{
	private static string[] sMaterialColorPropertyNames = new string[]
	{
		"_Color",
		"_TintColor",
		"_EmisColor"
	};
	public static bool MaterialHasColor(Material mat)
	{
		bool result;
		if (null != mat)
		{
			int i = 0;
			int max = LKMaterialUtils.sMaterialColorPropertyNames.Length;
			while (i < max)
			{
				if (mat.HasProperty(LKMaterialUtils.sMaterialColorPropertyNames[i]))
				{
					result = true;
					return result;
				}
				i++;
			}
		}
		result = false;
		return result;
	}
	public static string GetMaterialColorName(Material mat)
	{
		string result;
		if (null != mat)
		{
			int i = 0;
			int max = LKMaterialUtils.sMaterialColorPropertyNames.Length;
			while (i < max)
			{
				if (mat.HasProperty(LKMaterialUtils.sMaterialColorPropertyNames[i]))
				{
					result = LKMaterialUtils.sMaterialColorPropertyNames[i];
					return result;
				}
				i++;
			}
		}
		result = null;
		return result;
	}
	public static Color GetMaterialColor(Material mat)
	{
		return LKMaterialUtils.GetMaterialColor(mat, Color.white);
	}
	public static Color GetMaterialColor(Material mat, Color defaultColor)
	{
		Color result;
		if (null != mat)
		{
			int i = 0;
			int max = LKMaterialUtils.sMaterialColorPropertyNames.Length;
			while (i < max)
			{
				if (mat.HasProperty(LKMaterialUtils.sMaterialColorPropertyNames[i]))
				{
					result = mat.GetColor(LKMaterialUtils.sMaterialColorPropertyNames[i]);
					return result;
				}
				i++;
			}
		}
		result = defaultColor;
		return result;
	}
	public static bool SetMaterialColor(Material mat, Color color)
	{
		bool result;
		if (null != mat)
		{
			int i = 0;
			int max = LKMaterialUtils.sMaterialColorPropertyNames.Length;
			while (i < max)
			{
				if (mat.HasProperty(LKMaterialUtils.sMaterialColorPropertyNames[i]))
				{
					mat.SetColor(LKMaterialUtils.sMaterialColorPropertyNames[i], color);
					result = true;
					return result;
				}
				i++;
			}
		}
		result = false;
		return result;
	}
}
