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
		var oClearInfo = new CClearInfo() {
			m_stIDInfo = CFactory.MakeIDInfo(a_nID, a_nStageID, a_nChapterID)
		};

		oClearInfo.OnAfterDeserialize();
		return oClearInfo;
	}

	/** 유저 아이템 정보를 생성한다 */
	public static CUserItemInfo MakeUserItemInfo(EItemKinds a_eItemKinds, long m_nLV = KCDefine.B_VAL_1_INT, long a_nNumItems = KCDefine.B_VAL_0_INT) {
		var oUserItemInfo = new CUserItemInfo() {
			LV = m_nLV, NumItems = a_nNumItems, ItemKinds = a_eItemKinds
		};

		oUserItemInfo.OnAfterDeserialize();
		return oUserItemInfo;
	}

	/** 유저 스킬 정보를 생성한다 */
	public static CUserSkillInfo MakeUserSkillInfo(ESkillKinds a_eSkillKinds, long m_nLV = KCDefine.B_VAL_1_INT) {
		var oUserSkillInfo = new CUserSkillInfo() {
			LV = m_nLV, SkillKinds = a_eSkillKinds
		};

		oUserSkillInfo.OnAfterDeserialize();
		return oUserSkillInfo;
	}

	/** 유저 객체 정보를 생성한다 */
	public static CUserObjInfo MakeUserObjInfo(EObjKinds a_eObjKinds, long m_nLV = KCDefine.B_VAL_1_INT) {
		var oUserObjInfo = new CUserObjInfo() {
			LV = m_nLV, ObjKinds = a_eObjKinds
		};

		oUserObjInfo.OnAfterDeserialize();
		return oUserObjInfo;
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

	/** 획득 정보 노드를 생성한다 */
	public static List<string> MakeAcquireInfoNodes() {
		return CFactory.MakeAcquireInfoNodes();
	}

	/** 결제 정보 노드를 생성한다 */
	public static List<string> MakePurchaseInfoNodes() {
		return CFactory.MakePurchaseInfoNodes();
	}
#endif			// #if FIREBASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
