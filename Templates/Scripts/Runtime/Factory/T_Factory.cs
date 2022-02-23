using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 기본 팩토리 */
public static partial class Factory {
	#region 클래스 함수
	/** 클리어 정보를 생성한다 */
	public static CClearInfo MakeClearInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return new CClearInfo() {
			m_stIDInfo = CFactory.MakeIDInfo(a_nID, a_nStageID, a_nChapterID),
			
			NumClearMarks = KCDefine.B_VAL_0_INT,
			ClearRecord = $"{KCDefine.B_VAL_0_INT}",
			BestClearRecord = $"{KCDefine.B_VAL_0_INT}"
		};
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if FIREBASE_MODULE_ENABLE
	/** 유저 정보 노드를 생성한다 */
	public static List<string> MakeUserInfoNodes() {
		return CFactory.MakeUserInfoNodes();
	}

	/** 결제 정보 노드를 생성한다 */
	public static List<string> MakePurchaseInfoNodes() {
		return CFactory.MakePurchaseInfoNodes();
	}

	/** 지급 아이템 정보 노드를 생성한다 */
	public static List<string> MakePostItemInfoNodes() {
		return CFactory.MakePostItemInfoNodes();
	}
#endif			// #if FIREBASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수

	#region 추가 클래스 함수

	#endregion			// 추가 클래스 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
