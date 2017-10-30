using System;
using UnityEngine;
public class LKPositionCurveAnimationImp : ILKCurveAnimationImp
{
	private Transform mTransform;
	private Vector3 mOriginalLocalPosition;
	private Vector3 mOriginalWorldPosition;
	private LKCurveInfo mLKCurveInfo;
	public LKPositionCurveAnimationImp(LKCurveAnimation lkCurveAnimation)
	{
		this.mTransform = lkCurveAnimation.transform;
        this.mOriginalLocalPosition = this.mTransform.localPosition;
        this.mOriginalWorldPosition = this.mTransform.position;
	}
	public void Init(LKCurveInfo lkCurveInfo)
	{
		this.mLKCurveInfo = lkCurveInfo;
	}
	public void Reset()
	{
		if (this.mLKCurveInfo != null && this.mLKCurveInfo.isWorld)
		{
			this.mTransform.position = this.mOriginalWorldPosition;
		}
		else
		{
			this.mTransform.localPosition = this.mOriginalLocalPosition;
		}
	}
	public void ApplyCurve(LKCurveInfo lkCurveInfo, float normalizedTime)
	{
		float deltaCurveValue = lkCurveInfo.GetDeltaCurveValue(normalizedTime) * lkCurveInfo.scaleValue;
		Vector3 positionTransform = Vector3.zero;
		if (lkCurveInfo.xAxis)
		{
			positionTransform.x += deltaCurveValue;
		}
		if (lkCurveInfo.yAxis)
		{
			positionTransform.y += deltaCurveValue;
		}
		if (lkCurveInfo.zAxis)
		{
			positionTransform.z += deltaCurveValue;
		}
		if (lkCurveInfo.isWorld)
		{
			Transform expr_7D = this.mTransform;
			expr_7D.position = expr_7D.position + positionTransform;
		}
		else
		{
			Transform expr_97 = this.mTransform;
			expr_97.localPosition = expr_97.localPosition + positionTransform;
		}
	}
}
