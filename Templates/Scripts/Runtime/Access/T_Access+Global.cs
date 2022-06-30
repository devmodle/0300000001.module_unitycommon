using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 전역 접근자 */
public static partial class Access {
	#region 클래스 프로퍼티
	public static string StoreURL {
		get {
#if UNITY_IOS
			return string.Format(KCDefine.U_FMT_STORE_URL, CProjInfoTable.Inst.ProjInfo.m_oStoreAppID);
#else
			return string.Format(KCDefine.U_FMT_STORE_URL, CProjInfoTable.Inst.GetAppID(CProjInfoTable.Inst.ProjInfo));
#endif			// #if UNITY_IOS
		}
	}

	public static string MoreGamesURL => string.Format(KCDefine.U_FMT_MORE_GAMES_URL, CProjInfoTable.Inst.ProjInfo.m_oStoreAppID);
	#endregion			// 클래스 프로퍼티

	#region 클래스 함수
	/** 획득 가능 여부를 검사한다 */
	public static bool IsEnableAcquire(STPriceInfo a_stPriceInfo) {
		switch(a_stPriceInfo.m_ePriceType) {
			case EPriceType.ITEM: return CUserInfoStorage.Inst.TryGetUserItemInfo((EItemKinds)a_stPriceInfo.m_nKinds, out CUserItemInfo oUserItemInfo) ? oUserItemInfo.NumItems >= a_stPriceInfo.IntPrice : false;
			case EPriceType.SKILL: return CUserInfoStorage.Inst.TryGetUserSkillInfo((ESkillKinds)a_stPriceInfo.m_nKinds, out CUserSkillInfo oUserSkillInfo) ? oUserSkillInfo.LV >= a_stPriceInfo.IntPrice : false;
			case EPriceType.OBJ: return CUserInfoStorage.Inst.TryGetUserObjInfo((EObjKinds)a_stPriceInfo.m_nKinds, out CUserObjInfo oUserObjInfo) ? oUserObjInfo.LV >= a_stPriceInfo.IntPrice : false;
		}

		return false;
	}

	/** 획득 가능 여부를 검사한다 */
	public static bool IsEnableAcquire(List<STPriceInfo> a_oPriceInfoList) {
		for(int i = 0; i < a_oPriceInfoList.Count; ++i) {
			// 획득 불가능 할 경우
			if(!Access.IsEnableAcquire(a_oPriceInfoList[i])) {
				return false;
			}
		}

		return true;
	}

	/** 튜토리얼 메세지를 반환한다 */
	public static string GetTutorialMsg(ETutorialKinds a_eTutorialKinds, int a_nIdx = KCDefine.B_VAL_0_INT) {
		string oKey = string.Format(KCDefine.U_KEY_FMT_TUTORIAL_MSG, a_eTutorialKinds, a_nIdx + KCDefine.B_VAL_1_INT);
		return CStrTable.Inst.GetStr(oKey);
	}
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
