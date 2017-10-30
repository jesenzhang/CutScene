using System;
using UnityEngine;
public class LKMaterialColor2CurveAnimationImp : ILKCurveAnimationImp
{
	private GameObject mGameObject;
	private string mColorName;
	private Material mMaterial;
	private string[] mChildColorNames;
	private Material[] mChildMaterials;
	private Color mOriginalColor;
	public LKMaterialColor2CurveAnimationImp(LKCurveAnimation lkCurveAnimation)
	{
        this.mGameObject = lkCurveAnimation.gameObject;
	}
	public void Init(LKCurveInfo lkCurveInfo)
	{
		this.mOriginalColor = lkCurveInfo.fromColor;
		if (lkCurveInfo.bRecursive)
		{
			if (this.mChildColorNames == null)
			{
				Renderer[] renderers = this.mGameObject.GetComponentsInChildren<Renderer>(true);
				this.mChildColorNames = new string[renderers.Length];
				this.mChildMaterials = new Material[renderers.Length];
				int i = 0;
				int nmax = renderers.Length;
				while (i < nmax)
				{
					Renderer ren = renderers[i];
                    this.mChildMaterials[i] = ren.material;
					this.mChildColorNames[i] = LKMaterialUtils.GetMaterialColorName(this.mChildMaterials[i]);
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
				this.mChildMaterials[i].SetColor(this.mChildColorNames[i], this.mOriginalColor);
			}
		}
		if (this.mColorName != null && this.mMaterial != null)
		{
			this.mMaterial.SetColor(this.mColorName, this.mOriginalColor);
		}
	}
	public void ApplyCurve(LKCurveInfo lkCurveInfo, float normalizedTime)
	{
		float deltaCurveValue = lkCurveInfo.Evaluate(normalizedTime);
		Color tarColor = lkCurveInfo.fromColor;
		if (lkCurveInfo.rApply)
		{
			tarColor.r = Mathf.Lerp(lkCurveInfo.fromColor.r, lkCurveInfo.toColor.r, deltaCurveValue);
		}
		if (lkCurveInfo.gApply)
		{
			tarColor.g = Mathf.Lerp(lkCurveInfo.fromColor.g, lkCurveInfo.toColor.g, deltaCurveValue);
		}
		if (lkCurveInfo.bApply)
		{
			tarColor.b = Mathf.Lerp(lkCurveInfo.fromColor.b, lkCurveInfo.toColor.b, deltaCurveValue);
		}
		if (lkCurveInfo.aApply)
		{
			tarColor.a = Mathf.Lerp(lkCurveInfo.fromColor.a, lkCurveInfo.toColor.a, deltaCurveValue);
		}
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
						this.mChildMaterials[i].SetColor(this.mChildColorNames[i], tarColor);
					}
					i++;
				}
			}
		}
		else
		{
			if (this.mColorName != null && this.mMaterial != null)
			{
				this.mMaterial.SetColor(this.mColorName, tarColor);
			}
		}
	}
}
