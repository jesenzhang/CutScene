using System;
using UnityEngine;
public class LKTextureUVCurveAnimationImp : ILKCurveAnimationImp
{
	private GameObject mGameObject;
	private LKUVAnimation mLKUVAnimation = null;
	private Vector2 mOriginalScrollSpeed;
	public LKTextureUVCurveAnimationImp(LKCurveAnimation lkCurveAnimation)
	{
        this.mGameObject = lkCurveAnimation.gameObject;
	}
	public void Init(LKCurveInfo lkCurveInfo)
	{
		if (this.mLKUVAnimation == null)
		{
			if (this.mLKUVAnimation = this.mGameObject.GetComponent<LKUVAnimation>())
			{
				this.mOriginalScrollSpeed = new Vector2(this.mLKUVAnimation.scrollSpeedX, this.mLKUVAnimation.scrollSpeedY);
			}
			else
			{
				Debug.LogError("Texture UV Animation, LKUVAnimation is missing!");
			}
		}
	}
	public void Reset()
	{
		if (this.mLKUVAnimation != null)
		{
			this.mLKUVAnimation.scrollSpeedX = this.mOriginalScrollSpeed.x;
			this.mLKUVAnimation.scrollSpeedY = this.mOriginalScrollSpeed.y;
		}
	}
	public void ApplyCurve(LKCurveInfo lkCurveInfo, float normalizedTime)
	{
		if (this.mLKUVAnimation)
		{
			float deltaCurveValue = lkCurveInfo.GetDeltaCurveValue(normalizedTime) * lkCurveInfo.scaleValue;
			if (lkCurveInfo.xApply)
			{
				this.mLKUVAnimation.scrollSpeedX += deltaCurveValue;
			}
			if (lkCurveInfo.yApply)
			{
				this.mLKUVAnimation.scrollSpeedY += deltaCurveValue;
			}
		}
	}
}
