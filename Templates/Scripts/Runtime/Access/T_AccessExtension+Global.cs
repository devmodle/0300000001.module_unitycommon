using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 전역 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수
	/** 타겟 정보를 반환한다 */
	public static STTargetInfo ExGetTargetInfo(this Dictionary<ulong, STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = a_oSender.ExTryGetTargetInfo(a_eTargetKinds, a_nKinds, out STTargetInfo stTargetInfo);
		CAccess.Assert(bIsValid);

		return stTargetInfo;
	}

	/** 타겟 정보를 반환한다 */
	public static bool ExTryGetTargetInfo(this Dictionary<ulong, STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutTargetInfo) {
		a_stOutTargetInfo = a_oSender.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID);
		return !a_stOutTargetInfo.Equals(STTargetInfo.INVALID);
	}
	#endregion			// 클래스 함수
}

/** 초기화 씬 접근자 확장 클래스 */
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

/** 시작 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 스플래시 씬 접근자 확장 클래스 */
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
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
