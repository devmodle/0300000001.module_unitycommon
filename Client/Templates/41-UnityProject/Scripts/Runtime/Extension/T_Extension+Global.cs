#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using DG.Tweening;

/** 전역 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수
	/** 타겟 정보를 추가한다 */
	public static void ExAddTargetInfo(this List<CTargetInfo> a_oSender, CTargetInfo a_oTargetInfo, bool a_bIsCounting = true, bool a_bIsAssert = true) {
		CFunc.Assert(!a_bIsAssert || (a_oSender != null && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(a_oSender != null && a_oTargetInfo != null) {
			int nIdx = a_oSender.FindIndex((a_oCompareTargetInfo) => a_oCompareTargetInfo.TargetType == a_oTargetInfo.TargetType && a_oCompareTargetInfo.Kinds == a_oTargetInfo.Kinds);

			// 타겟 정보 추가가 가능 할 경우
			if(!a_bIsCounting || !a_oSender.ExIsValidIdx(nIdx)) {
				a_oSender.ExAddVal(a_oTargetInfo);
			}
		}
	}

	/** 소유 타겟 정보를 추가한다 */
	public static void ExAddOwnedTargetInfo(this CTargetInfo a_oSender, CTargetInfo a_oTargetInfo, bool a_bIsAssert = true) {
		CFunc.Assert(!a_bIsAssert || (a_oSender != null && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(a_oSender != null && a_oTargetInfo != null) {
			a_oSender.m_oOwnedTargetInfoList.ExAddTargetInfo(a_oTargetInfo, true, a_bIsAssert);
			a_oTargetInfo.ExSetOwnerTargetInfo(a_oSender, a_bIsAssert);
		}
	}

	/** 타겟 정보를 제거한다 */
	public static void ExRemoveTargetInfo(this List<CTargetInfo> a_oSender, CTargetInfo a_oTargetInfo, bool a_bIsAssert = true) {
		CFunc.Assert(!a_bIsAssert || (a_oSender != null && a_oTargetInfo != null && a_oSender.Contains(a_oTargetInfo)));

		// 타겟 정보가 존재 할 경우
		if((a_oSender != null && a_oTargetInfo != null && a_oSender.Contains(a_oTargetInfo))) {
			a_oSender.ExRemoveVal(a_oTargetInfo);
		}
	}

	/** 소유 타겟 정보를 제거한다 */
	public static void ExRemoveOwnedTargetInfo(this CTargetInfo a_oSender, CTargetInfo a_oTargetInfo, bool a_bIsAssert = true) {
		CFunc.Assert(!a_bIsAssert || (a_oSender != null && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(a_oSender != null && a_oTargetInfo != null) {
			a_oSender.m_oOwnedTargetInfoList.ExRemoveTargetInfo(a_oTargetInfo, a_bIsAssert);
			a_oTargetInfo.ExSetOwnerTargetInfo(null, a_bIsAssert);
		}
	}

	/** 어빌리티 값을 증가시킨다 */
	public static void ExIncrAbilityVal(this Dictionary<EAbilityKinds, decimal> a_oSender, EAbilityKinds a_eAbilityKinds, decimal a_dmVal, bool a_bIsAssert = true) {
		CFunc.Assert(!a_bIsAssert || (a_oSender != null && a_eAbilityKinds != EAbilityKinds.NONE));

		// 어빌리티 값 설정이 가능 할 경우
		if(a_oSender != null && a_eAbilityKinds != EAbilityKinds.NONE) {
			decimal dmAbilityVal = a_oSender.GetValueOrDefault(a_eAbilityKinds);
			a_oSender.ExReplaceVal(a_eAbilityKinds, System.Math.Clamp(dmAbilityVal + a_dmVal, KCDefine.B_VAL_0_INT, decimal.MaxValue));
		}
	}

	/** 타겟 값을 증가시킨다 */
	public static void ExIncrTargetVal(this Dictionary<ulong, STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds, decimal a_dmVal, bool a_bIsAssert = true) {
		CFunc.Assert(!a_bIsAssert || (a_oSender != null && a_eTargetKinds.ExIsValid()));
		a_oSender.ExReplaceTargetVal(a_eTargetKinds, a_nKinds, a_oSender.ExGetTargetVal(a_eTargetKinds, a_nKinds) + a_dmVal, a_bIsAssert);
	}

	/** 타겟 값을 대체한다 */
	public static void ExReplaceTargetVal(this Dictionary<ulong, STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds, decimal a_dmVal, bool a_bIsAssert = true) {
		CFunc.Assert(!a_bIsAssert || (a_oSender != null && a_eTargetKinds.ExIsValid()));

		// 타겟 정보가 존재 할 경우
		if(a_oSender != null && a_eTargetKinds.ExIsValid()) {
			a_oSender.ExTryGetTargetInfo(a_eTargetKinds, a_nKinds, out STTargetInfo stTargetInfo);
			stTargetInfo.m_nKinds = a_nKinds;
			stTargetInfo.m_eTargetKinds = a_eTargetKinds;
			stTargetInfo.m_stValInfo01.m_dmVal = System.Math.Clamp(a_dmVal, decimal.MinValue, decimal.MaxValue);
			stTargetInfo.m_stValInfo01.m_eValType = (stTargetInfo.m_stValInfo01.m_eValType == EValType.NONE) ? stTargetInfo.m_stValInfo01.m_dmVal.ExIsInt() ? EValType.INT : EValType.REAL : stTargetInfo.m_stValInfo01.m_eValType;

			a_oSender.ExReplaceVal(Factory.MakeUTargetInfoID(a_eTargetKinds, a_nKinds), stTargetInfo);
		}
	}

	/** 효과를 실행한다 */
	public static void ExPlay(this ParticleSystem a_oSender, System.Action<CEventDispatcher, string> a_oCallback, bool a_bIsPlayChildren = true, bool a_bIsStopChildren = true, bool a_bIsAssert = true) {
		CFunc.Assert(!a_bIsAssert || a_oSender != null);

		a_oSender?.ExPlay(a_bIsPlayChildren, a_bIsStopChildren, a_bIsAssert);
		a_oSender?.GetComponentInChildren<CEventDispatcher>()?.SetParticleFXCallback(a_oCallback);
	}

	/** 게이지 애니메이션을 시작한다 */
	public static Sequence ExStartGaugeAnim(this CGaugeHandler a_oSender, System.Action<float> a_oCallback, System.Action<CGaugeHandler, Sequence> a_oCompleteCallback, float a_fStartVal, float a_fEndVal, float a_fDuration, Ease a_eEase = KCDefine.U_EASE_DEF, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsRealtime = false) {
		CFunc.Assert(a_oSender != null);
		return CFactory.MakeSequence(CFactory.MakeAnim(() => a_oSender.Percent, (a_fVal) => a_oSender.SetPercent(a_fVal), () => a_oSender.SetPercent(a_fStartVal), a_oCallback, a_fEndVal, a_fDuration, a_eEase, a_bIsRealtime), (a_oAnimSender) => CFunc.Invoke(ref a_oCompleteCallback, a_oSender, a_oAnimSender), a_fDelay, a_bIsRealtime: a_bIsRealtime);
	}

	/** JSON 노드 정보 => JSON 노드로 변환한다 */
	public static SimpleJSON.JSONNode ExToJSONNode(this Dictionary<string, SimpleJSON.JSONNode> a_oSender) {
		CFunc.Assert(a_oSender != null);
		var oJSONNode = new SimpleJSON.JSONClass();

		foreach(var stKeyVal in a_oSender) {
			oJSONNode.Add(stKeyVal.Key, stKeyVal.Value);
		}

		return oJSONNode;
	}
	#endregion // 클래스 함수
}

/** 초기화 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 설정 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 지연 설정 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 타이틀 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 메인 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 플레이 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 결과 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 로딩 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 중첩 씬 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수

	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
