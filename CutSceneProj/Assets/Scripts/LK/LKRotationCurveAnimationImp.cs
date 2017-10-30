using System;
using UnityEngine;
public class LKRotationCurveAnimationImp : ILKCurveAnimationImp
{
	private Transform mTransform;
	private Quaternion mOriginalLocalRotation;
	private Quaternion mOriginalWorldRotation;
	private LKCurveInfo mLKCurveInfo;
	public LKRotationCurveAnimationImp(LKCurveAnimation lkCurveAnimation)
	{
        this.mTransform = lkCurveAnimation.transform;
        this.mOriginalLocalRotation = this.mTransform.localRotation;
        this.mOriginalWorldRotation = this.mTransform.rotation;
	}
	public void Init(LKCurveInfo lkCurveInfo)
	{
		this.mLKCurveInfo = lkCurveInfo;
	}
	public void Reset()
	{
		if (this.mLKCurveInfo != null && this.mLKCurveInfo.isWorld)
		{
			this.mTransform.rotation = this.mOriginalWorldRotation;
		}
		else
		{
			this.mTransform.localRotation = this.mOriginalLocalRotation;
		}
	}
	public void ApplyCurve(LKCurveInfo lkCurveInfo, float normalizedTime)
	{
		float deltaCurveValue = lkCurveInfo.GetDeltaCurveValue(normalizedTime) * lkCurveInfo.scaleValue;
		Vector3 rotationTransform = Vector3.zero;
		if (lkCurveInfo.xAxis)
		{
			rotationTransform.x += deltaCurveValue;
		}
		if (lkCurveInfo.yAxis)
		{
			rotationTransform.y += deltaCurveValue;
		}
		if (lkCurveInfo.zAxis)
		{
			rotationTransform.z += deltaCurveValue;
		}
		if (lkCurveInfo.isWorld)
		{
			//this.mTransform.Rotate(rotationTransform, 0);
			this.mTransform.Rotate(rotationTransform, Space.World);
		}
		else
		{
			//this.mTransform.Rotate(rotationTransform, 1);
			this.mTransform.Rotate(rotationTransform, Space.Self);
		}
	}
}
