using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

#if NEVER_USE_THIS
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 전역 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수
	/** 효과를 재생한다 */
	public static void ExPlay(this ParticleSystem a_oSender, System.Action<CEventDispatcher> a_oCallback, bool a_bIsRemoveChildren = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);
		var oEventDispatcher = a_oSender?.GetComponentInChildren<CEventDispatcher>();

		// 이벤트 전달자가 존재 할 경우
		if(oEventDispatcher != null) {
			oEventDispatcher.ParticleEventCallback = a_oCallback;
		}

		a_oSender?.ExPlay(a_bIsRemoveChildren, a_bIsEnableAssert);
	}

	/** 게이지 애니메이션을 시작한다 */
	public static Tween ExStartGaugeAni(this CGaugeHandler a_oSender, float a_fStartVal, float a_fEndVal, float a_fDuration, Ease a_eEase = KCDefine.U_EASE_ANI, bool a_bIsRealtime = false) {
		CAccess.Assert(a_oSender != null);
		a_oSender.Percent = a_fStartVal;

		return DOTween.To(() => a_oSender.Percent, (a_fVal) => a_oSender.Percent = a_fVal, a_fEndVal, a_fDuration).SetAutoKill().SetEase(a_eEase).SetUpdate(a_bIsRealtime);
	}

	/** 게이지 애니메이션을 시작한다 */
	public static Sequence ExStartGaugeAni(this CGaugeHandler a_oSender, float a_fStartVal, float a_fEndVal, float a_fDuration, System.Action<CGaugeHandler, Sequence> a_oCallback, Ease a_eEase = KCDefine.U_EASE_ANI, bool a_bIsRealtime = false, float a_fDelay = KCDefine.B_VAL_0_FLT) {
		CAccess.Assert(a_oSender != null);
		return CFactory.MakeSequence(a_oSender.ExStartGaugeAni(a_fStartVal, a_fEndVal, a_fDuration, a_eEase, a_bIsRealtime), (a_oSequenceSender) => a_oCallback?.Invoke(a_oSender, a_oSequenceSender), a_fDelay, a_eEase, false, a_bIsRealtime);
	}
	#endregion			// 클래스 함수

	#region 추가 클래스 함수

	#endregion			// 추가 클래스 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
