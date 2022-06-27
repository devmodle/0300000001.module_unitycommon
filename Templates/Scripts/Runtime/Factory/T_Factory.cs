using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 기본 팩토리 */
public static partial class Factory {
	#region 클래스 함수
	/** 클리어 정보를 생성한다 */
	public static CClearInfo MakeClearInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return new CClearInfo() {
			NumMarks = KCDefine.B_VAL_0_INT,
			Record = KCDefine.B_STR_0_INT,
			BestRecord = KCDefine.B_STR_0_INT,

			m_stIDInfo = CFactory.MakeIDInfo(a_nID, a_nStageID, a_nChapterID)
		};
	}
	#endregion			// 클래스 함수

	#region 제네릭 클래스 함수
	/** 키 정보를 생성한다 */
	public static List<(T, GameObject)> MakeKeyInfos<T>(List<(T, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3));
		}

		return oKeyInfoList;
	}

	/** 키 정보를 생성한다 */
	public static List<(T, string, GameObject, GameObject)> MakeKeyInfos<T>(List<(T, string, GameObject, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList) {
		CAccess.Assert(a_oKeyInfoList != null);
		var oKeyInfoList = new List<(T, string, GameObject, GameObject)>();

		for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
			oKeyInfoList.Add((a_oKeyInfoList[i].Item1, a_oKeyInfoList[i].Item2, a_oKeyInfoList[i].Item3, a_oKeyInfoList[i].Item4));
		}

		return oKeyInfoList;
	}
	#endregion			// 제네릭 클래스 함수

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

	/** 획득 아이템 정보 노드를 생성한다 */
	public static List<string> MakeAcquireItemInfoNodes() {
		return CFactory.MakeAcquireItemInfoNodes();
	}
#endif			// #if FIREBASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
