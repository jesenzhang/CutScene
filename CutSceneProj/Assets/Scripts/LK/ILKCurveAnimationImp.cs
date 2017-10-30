using System;
public interface ILKCurveAnimationImp
{
	void Init(LKCurveInfo lkCurveInfo);
	void Reset();
	void ApplyCurve(LKCurveInfo lkCurveInfo, float normalizedTime);
}
