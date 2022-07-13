using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 전역 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수
	/** 효과를 재생한다 */
	public static void ExPlay(this ParticleSystem a_oSender, System.Action<CEventDispatcher> a_oCallback, bool a_bIsPlayChildren = true, bool a_bIsStopChildren = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);
		var oEventDispatcher = a_oSender?.GetComponentInChildren<CEventDispatcher>();

		// 이벤트 전달자가 존재 할 경우
		if(oEventDispatcher != null) {
			oEventDispatcher.ParticleEventCallback = a_oCallback;
		}

		a_oSender?.ExPlay(a_bIsPlayChildren, a_bIsStopChildren, a_bIsEnableAssert);
	}

	/** 게이지 애니메이션을 시작한다 */
	public static Sequence ExStartGaugeAni(this CGaugeHandler a_oSender, System.Action<float> a_oCallback, System.Action<CGaugeHandler, Sequence> a_oCompleteCallback, float a_fStartVal, float a_fEndVal, float a_fDuration, Ease a_eEase = KCDefine.U_EASE_DEF, bool a_bIsRealtime = false, float a_fDelay = KCDefine.B_VAL_0_REAL) {
		CAccess.Assert(a_oSender != null);
		return CFactory.MakeSequence(CFactory.MakeAni(() => a_oSender.Percent, (a_fVal) => a_oSender.Percent = a_fVal, () => a_oSender.Percent = a_fStartVal, a_oCallback, a_fEndVal, a_fDuration, a_eEase, a_bIsRealtime), (a_oSequenceSender) => CFunc.Invoke(ref a_oCompleteCallback, a_oSender, a_oSequenceSender), a_fDelay, false, a_bIsRealtime);
	}

	/** JSON 노드 정보 => JSON 노드로 변환한다 */
	public static SimpleJSON.JSONNode ExToJSONNode(this Dictionary<string, (SimpleJSON.JSONNode, bool)> a_oSender) {
		CAccess.Assert(a_oSender != null);
		var oJSONNode = new SimpleJSON.JSONClass();

		foreach(var stKeyVal in a_oSender) {
			oJSONNode.Add(stKeyVal.Key, stKeyVal.Value.Item1);
		}

		return oJSONNode;
	}
	#endregion			// 클래스 함수
}

/** 초기화 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 설정 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 약관 동의 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 지연 설정 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 시작 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 스플래시 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}

/** 타이틀 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}

/** 메인 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 게임 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 로딩 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 중첩 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
