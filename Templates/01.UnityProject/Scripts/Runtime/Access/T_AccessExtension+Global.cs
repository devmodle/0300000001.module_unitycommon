#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 전역 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수
	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EPlayMode a_eSender) {
		return a_eSender > EPlayMode.NONE && a_eSender < EPlayMode.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ELoginType a_eSender) {
		return a_eSender > ELoginType.NONE && a_eSender < ELoginType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EPurchaseType a_eSender) {
		return a_eSender > EPurchaseType.NONE && a_eSender < EPurchaseType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EKindsGroupType a_eSender) {
		return a_eSender > EKindsGroupType.NONE && a_eSender < EKindsGroupType.MAX_VAL;
	}
	
	/** 타겟 값을 반환한다 */
	public static long ExGetTargetVal(this Dictionary<ulong, STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds) {
		return a_oSender.ExTryGetTargetInfo(a_eTargetKinds, a_nKinds, out STTargetInfo stTargetInfo) ? stTargetInfo.m_stValInfo01.m_nVal : KCDefine.B_VAL_0_INT;
	}

	/** 타겟 정보를 반환한다 */
	public static STTargetInfo ExGetTargetInfo(this Dictionary<ulong, STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = a_oSender.ExTryGetTargetInfo(a_eTargetKinds, a_nKinds, out STTargetInfo stTargetInfo);
		CAccess.Assert(bIsValid);

		return stTargetInfo;
	}

	/** 타겟 정보를 반환한다 */
	public static CTargetInfo ExGetTargetInfo(this Dictionary<ETargetType, List<CTargetInfo>> a_oSender, ETargetType a_eTargetType, int a_nKinds) {
		bool bIsValid = a_oSender.ExTryGetTargetInfo(a_eTargetType, a_nKinds, out CTargetInfo oTargetInfo);
		CAccess.Assert(bIsValid);

		return oTargetInfo;
	}
	
	/** 타겟 정보를 반환한다 */
	public static bool ExTryGetTargetInfo(this Dictionary<ulong, STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutTargetInfo) {
		a_stOutTargetInfo = a_oSender.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID);
		return a_oSender.ContainsKey(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds));
	}
	
	/** 타겟 정보를 반환한다 */
	public static bool ExTryGetTargetInfo(this Dictionary<ETargetType, List<CTargetInfo>> a_oSender, ETargetType a_eTargetType, int a_nKinds, out CTargetInfo a_oOutTargetInfo) {
		a_oOutTargetInfo = a_oSender.TryGetValue(a_eTargetType, out List<CTargetInfo> oTargetInfoList) ? oTargetInfoList.ExGetVal((a_oTargetInfo) => a_oTargetInfo.TargetType == a_eTargetType && a_oTargetInfo.Kinds == a_nKinds, null) : null;
		return a_oOutTargetInfo != null;
	}
	#endregion			// 클래스 함수
}

/** 초기화 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}

/** 시작 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 설정 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 약관 동의 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 지연 설정 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 타이틀 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}

/** 메인 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 게임 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 로딩 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 중첩 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
