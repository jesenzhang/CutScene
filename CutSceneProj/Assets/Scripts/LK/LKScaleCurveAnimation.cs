using System;
using UnityEngine;
public class LKScaleCurveAnimation : ILKCurveAnimationImp
{
	private Transform mTransform;
	private Vector3 mOriginalLocalScale;
	public LKScaleCurveAnimation(LKCurveAnimation lkCurveAnimation)
	{
        this.mTransform = lkCurveAnimation.transform;
        this.mOriginalLocalScale = this.mTransform.localScale;
	}
	public void Init(LKCurveInfo lkCurveInfo)
	{
	}
	public void Reset()
	{
		this.mTransform.localScale = this.mOriginalLocalScale;
	}
	public void ApplyCurve(LKCurveInfo lkCurveInfo, float normalizedTime)
	{
		float deltaCurveValue = lkCurveInfo.GetDeltaCurveValue(normalizedTime) * lkCurveInfo.scaleValue;
		Vector3 scaleTransform = Vector3.zero;
		if (lkCurveInfo.xAxis)
		{
			scaleTransform.x += deltaCurveValue;
		}
		if (lkCurveInfo.yAxis)
		{
			scaleTransform.y += deltaCurveValue;
		}
		if (lkCurveInfo.zAxis)
		{
			scaleTransform.z += deltaCurveValue;
		}
		Transform expr_70 = this.mTransform;
		expr_70.localScale = expr_70.localScale + scaleTransform;
	}
}
