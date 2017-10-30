using System;
using UnityEngine;
public class LKMaterialColor1CurveAnimation : ILKCurveAnimationImp
{
	private GameObject mGameObject;
	private string mColorName;
	private Material mMaterial;
	private Color mOriginalColor;
	private string[] mChildColorNames;
	private Material[] mChildMaterials;
	private Color[] mChildOriginalColors;
	public LKMaterialColor1CurveAnimation(LKCurveAnimation lkCurveAnimation)
	{
        this.mGameObject = lkCurveAnimation.gameObject;
	}
	public void Init(LKCurveInfo lkCurveInfo)
	{
		if (lkCurveInfo.bRecursive)
		{
			if (this.mChildColorNames == null)
			{
				Renderer[] renderers = this.mGameObject.GetComponentsInChildren<Renderer>(true);
				this.mChildColorNames = new string[renderers.Length];
				this.mChildMaterials = new Material[renderers.Length];
				this.mChildOriginalColors = new Color[renderers.Length];
				int i = 0;
				int nmax = renderers.Length;
				while (i < nmax)
				{
					Renderer ren = renderers[i];
                    this.mChildMaterials[i] = ren.material;
					this.mChildColorNames[i] = LKMaterialUtils.GetMaterialColorName(this.mChildMaterials[i]);
					this.mChildOriginalColors[i] = this.mChildMaterials[i].GetColor(this.mChildColorNames[i]);
					i++;
				}
			}
		}
		else
		{
			if (this.mMaterial == null)
			{
				Renderer renderer = this.mGameObject.GetComponent<Renderer>();
				if (renderer != null)
				{
                    this.mMaterial = renderer.material;
					this.mColorName = LKMaterialUtils.GetMaterialColorName(this.mMaterial);
					this.mOriginalColor = this.mMaterial.GetColor(this.mColorName);
				}
				else
				{
					Debug.LogError("Material Color Animation, Renderer or Material is missing!");
				}
			}
		}
	}
	public void Reset()
	{
		if (this.mChildColorNames != null && this.mChildMaterials != null)
		{
			for (int i = 0; i < this.mChildMaterials.Length; i++)
			{
				this.mChildMaterials[i].SetColor(this.mChildColorNames[i], this.mChildOriginalColors[i]);
			}
		}
		if (this.mColorName != null && this.mMaterial != null)
		{
			this.mMaterial.SetColor(this.mColorName, this.mOriginalColor);
		}
	}
	public void ApplyCurve(LKCurveInfo lkCurveInfo, float normalizedTime)
	{
		float deltaCurveValue = lkCurveInfo.GetDeltaCurveValue(normalizedTime);
		if (lkCurveInfo.bRecursive)
		{
			if (this.mChildColorNames != null && this.mChildColorNames.Length > 0)
			{
				int i = 0;
				int max = this.mChildColorNames.Length;
				while (i < max)
				{
					if (this.mChildColorNames[i] != null && this.mChildMaterials[i] != null)
					{
						Color colorDelta = lkCurveInfo.toColor - this.mChildOriginalColors[i];
						Color currColor = this.mChildMaterials[i].GetColor(this.mChildColorNames[i]);
						if (lkCurveInfo.rApply)
						{
							currColor.r += colorDelta.r * deltaCurveValue;
						}
						if (lkCurveInfo.gApply)
						{
							currColor.g += colorDelta.g * deltaCurveValue;
						}
						if (lkCurveInfo.bApply)
						{
							currColor.b += colorDelta.b * deltaCurveValue;
						}
						if (lkCurveInfo.aApply)
						{
							currColor.a += colorDelta.a * deltaCurveValue;
						}
						this.mChildMaterials[i].SetColor(this.mChildColorNames[i], currColor);
					}
					i++;
				}
			}
		}
		else
		{
			if (this.mColorName != null && this.mMaterial != null)
			{
				Color colorDelta = lkCurveInfo.toColor - this.mOriginalColor;
				Color currColor = this.mMaterial.GetColor(this.mColorName);
				if (lkCurveInfo.rApply)
				{
					currColor.r += colorDelta.r * deltaCurveValue;
				}
				if (lkCurveInfo.gApply)
				{
					currColor.g += colorDelta.g * deltaCurveValue;
				}
				if (lkCurveInfo.bApply)
				{
					currColor.b += colorDelta.b * deltaCurveValue;
				}
				if (lkCurveInfo.aApply)
				{
					currColor.a += colorDelta.a * deltaCurveValue;
				}
				this.mMaterial.SetColor(this.mColorName, currColor);
			}
		}
	}
}
