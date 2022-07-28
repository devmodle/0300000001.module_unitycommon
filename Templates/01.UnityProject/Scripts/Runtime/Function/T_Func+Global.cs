#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

/** 전역 함수 */
public static partial class Func {
	#region 클래스 함수
	/** 어빌리티 값을 설정한다 */
	public static void SetupAbilityVals(STTargetInfo a_stTargetInfo, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_stTargetInfo.m_eTargetKinds == ETargetKinds.ABILITY && a_oOutAbilityValDict != null));

		// 어빌리티 값 설정이 가능 할 경우
		if(a_stTargetInfo.m_eTargetKinds == ETargetKinds.ABILITY && a_oOutAbilityValDict != null) {
			var stAbilityInfo = CAbilityInfoTable.Inst.GetAbilityInfo((EAbilityKinds)a_stTargetInfo.Kinds);
			var eAbilityKinds = (EAbilityKinds)a_stTargetInfo.m_nKinds.ExKindsToSubKindsType();

			decimal dmAbilityVal = (stAbilityInfo.m_stValInfo.m_eValType == EValType.INT) ? stAbilityInfo.m_stValInfo.m_nVal : (decimal)stAbilityInfo.m_stValInfo.m_dblVal / KCDefine.B_UNIT_NORM_VAL_TO_PERCENT;
			a_oOutAbilityValDict?.ExReplaceVal(eAbilityKinds, System.Math.Clamp(a_oOutAbilityValDict.GetValueOrDefault(eAbilityKinds) + (dmAbilityVal * a_stTargetInfo.m_stValInfo01.m_nVal), decimal.MinValue, decimal.MaxValue), a_bIsEnableAssert);

			foreach(var stKeyVal in stAbilityInfo.m_oExtraAbilityTargetInfoDict) {
				Func.SetupAbilityVals(stKeyVal.Value, a_oOutAbilityValDict, a_bIsEnableAssert);
			}
		}
	}

	/** 지불한다 */
	public static void Pay(STTargetInfo a_stTargetInfo, bool a_bIsEnableAssert = true) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: Func.PayItemTarget(null, a_stTargetInfo, a_bIsEnableAssert); break;
			case ETargetType.SKILL: Func.PaySkillTarget(null, a_stTargetInfo, a_bIsEnableAssert); break;
			case ETargetType.OBJ: Func.PayObjTarget(null, a_stTargetInfo, a_bIsEnableAssert); break;
		}
	}

	/** 지불한다 */
	public static void Pay(CTargetInfo a_oTargetInfo, STTargetInfo a_stTargetInfo, bool a_bIsEnableAssert = true) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: Func.PayItemTarget(a_oTargetInfo as CItemInfo, a_stTargetInfo, a_bIsEnableAssert); break;
			case ETargetType.SKILL: Func.PaySkillTarget(a_oTargetInfo as CSkillInfo, a_stTargetInfo, a_bIsEnableAssert); break;
			case ETargetType.OBJ: Func.PayObjTarget(a_oTargetInfo as CObjInfo, a_stTargetInfo, a_bIsEnableAssert); break;
		}
	}

	/** 지불한다 */
	public static void Pay(List<(CTargetInfo, STTargetInfo)> a_oPayInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oPayInfoList != null);

		// 지불 정보가 존재 할 경우
		if(a_oPayInfoList != null) {
			for(int i = 0; i < a_oPayInfoList.Count; ++i) {
				Func.Pay(a_oPayInfoList[i].Item1, a_oPayInfoList[i].Item2, a_bIsEnableAssert);
			}
		}
	}

	/** 지불한다 */
	public static void Pay(Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oTargetInfoDict != null);

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfoDict != null) {
			foreach(var stKeyVal in a_oTargetInfoDict) {
				Func.Pay(stKeyVal.Value, a_bIsEnableAssert);
			}
		}
	}

	/** 획득한다 */
	public static void Acquire(STTargetInfo a_stTargetInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: Func.AcquireItemTarget(null, a_stTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert); break;
			case ETargetType.SKILL: Func.AcquireSkillTarget(null, a_stTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert); break;
			case ETargetType.OBJ: Func.AcquireObjTarget(null, a_stTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert); break;
		}
	}

	/** 획득한다 */
	public static void Acquire(CTargetInfo a_oTargetInfo, STTargetInfo a_stTargetInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: Func.AcquireItemTarget(a_oTargetInfo as CItemInfo, a_stTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert); break;
			case ETargetType.SKILL: Func.AcquireSkillTarget(a_oTargetInfo as CSkillInfo, a_stTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert); break;
			case ETargetType.OBJ: Func.AcquireObjTarget(a_oTargetInfo as CObjInfo, a_stTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert); break;
		}
	}

	/** 획득한다 */
	public static void Acquire(List<(CTargetInfo, STTargetInfo)> a_oAcquireInfoList, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oAcquireInfoList != null);

		// 획득 정보가 존재 할 경우
		if(a_oAcquireInfoList != null) {
			for(int i = 0; i < a_oAcquireInfoList.Count; ++i) {
				Func.Acquire(a_oAcquireInfoList[i].Item1, a_oAcquireInfoList[i].Item2, a_bIsAutoCreate, a_bIsEnableAssert);
			}
		}
	}

	/** 획득한다 */
	public static void Acquire(Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oTargetInfoDict != null);

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfoDict != null) {
			foreach(var stKeyVal in a_oTargetInfoDict) {
				Func.Acquire(stKeyVal.Value, a_bIsAutoCreate, a_bIsEnableAssert);
			}
		}	
	}

	/** 구입한다 */
	public static void Buy(STItemTradeInfo a_stItemTradeInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		Func.Pay(a_stItemTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
		Func.Acquire(a_stItemTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
	}

	/** 구입한다 */
	public static void Buy(STSkillTradeInfo a_stSkillTradeInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		Func.Pay(a_stSkillTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
		Func.Acquire(a_stSkillTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
	}

	/** 구입한다 */
	public static void Buy(STObjTradeInfo a_stObjTradeInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		Func.Pay(a_stObjTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
		Func.Acquire(a_stObjTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
	}

	/** 구입한다 */
	public static void Buy(STProductTradeInfo a_stProductTradeInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		Func.Pay(a_stProductTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
		Func.Acquire(a_stProductTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
	}

	/** 판매한다 */
	public static void Sale(STItemTradeInfo a_stItemTradeInfo, bool a_bIsEnableAssert = true) {
		Func.Buy(a_stItemTradeInfo, false, a_bIsEnableAssert);
	}

	/** 판매한다 */
	public static void Sale(STSkillTradeInfo a_stSkillTradeInfo, bool a_bIsEnableAssert = true) {
		Func.Buy(a_stSkillTradeInfo, false, a_bIsEnableAssert);
	}

	/** 판매한다 */
	public static void Sale(STObjTradeInfo a_stObjTradeInfo, bool a_bIsEnableAssert = true) {
		Func.Buy(a_stObjTradeInfo, false, a_bIsEnableAssert);
	}

	/** 상점 팝업을 출력한다 */
	public static void ShowStorePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CStorePopup>(KDefine.G_OBJ_N_STORE_POPUP, KCDefine.U_OBJ_P_G_STORE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 설정 팝업을 출력한다 */
	public static void ShowSettingsPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CSettingsPopup>(KDefine.G_OBJ_N_SETTINGS_POPUP, KCDefine.U_OBJ_P_G_SETTINGS_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 동기화 팝업을 출력한다 */
	public static void ShowSyncPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CSyncPopup>(KDefine.G_OBJ_N_SYNC_POPUP, KCDefine.U_OBJ_P_G_SYNC_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 일일 미션 팝업을 출력한다 */
	public static void ShowDailyMissionPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CDailyMissionPopup>(KDefine.G_OBJ_N_DAILY_MISSION_POPUP, KCDefine.U_OBJ_P_G_DAILY_MISSION_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 무료 보상 팝업을 출력한다 */
	public static void ShowFreeRewardPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CFreeRewardPopup>(KDefine.G_OBJ_N_FREE_REWARD_POPUP, KCDefine.U_OBJ_P_G_FREE_REWARD_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 일일 보상 팝업을 출력한다 */
	public static void ShowDailyRewardPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CDailyRewardPopup>(KDefine.G_OBJ_N_DAILY_REWARD_POPUP, KCDefine.U_OBJ_P_G_DAILY_REWARD_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 코인 상자 팝업을 출력한다 */
	public static void ShowCoinsBoxPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CCoinsBoxPopup>(KDefine.G_OBJ_N_COINS_BOX_POPUP, KCDefine.U_OBJ_P_G_COINS_BOX_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 보상 획득 팝업을 출력한다 */
	public static void ShowRewardAcquirePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CRewardAcquirePopup>(KDefine.G_OBJ_N_REWARD_ACQUIRE_POPUP, KCDefine.U_OBJ_P_G_REWARD_ACQUIRE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 코인 상자 획득 팝업을 출력한다 */
	public static void ShowCoinsBoxAcquirePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CCoinsBoxAcquirePopup>(KDefine.G_OBJ_N_COINS_BOX_ACQUIRE_POPUP, KCDefine.U_OBJ_P_G_COINS_BOX_ACQUIRE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 이어하기 팝업을 출력한다 */
	public static void ShowContinuePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CContinuePopup>(KDefine.G_OBJ_N_CONTINUE_POPUP, KCDefine.U_OBJ_P_G_CONTINUE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 결과 팝업을 출력한다 */
	public static void ShowResultPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CResultPopup>(KDefine.G_OBJ_N_RESULT_POPUP, KCDefine.U_OBJ_P_G_RESULT_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 재개 팝업을 출력한다 */
	public static void ShowResumePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CResumePopup>(KDefine.G_OBJ_N_RESUME_POPUP, KCDefine.U_OBJ_P_G_RESUME_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 정지 팝업을 출력한다 */
	public static void ShowPausePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CPausePopup>(KDefine.G_OBJ_N_PAUSE_POPUP, KCDefine.U_OBJ_P_G_PAUSE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 상품 교환 팝업을 출력한다 */
	public static void ShowProductTradePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CProductTradePopup>(KDefine.G_OBJ_N_PRODUCT_TRADE_POPUP, KCDefine.U_OBJ_P_G_PRODUCT_TRADE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 포커스 팝업을 출력한다 */
	public static void ShowFocusPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CFocusPopup>(KDefine.G_OBJ_N_FOCUS_POPUP, KCDefine.U_OBJ_P_G_FOCUS_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 튜토리얼 팝업을 출력한다 */
	public static void ShowTutorialPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CTutorialPopup>(KDefine.G_OBJ_N_TUTORIAL_POPUP, KCDefine.U_OBJ_P_G_TUTORIAL_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 지불한다 */
	private static void DoPay(CTargetInfo a_oTargetInfo, STTargetInfo a_stTargetInfo, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oTargetInfo != null && !a_stTargetInfo.Equals(STTargetInfo.INVALID)));

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfo != null && !a_stTargetInfo.Equals(STTargetInfo.INVALID)) {
			switch(((int)a_stTargetInfo.m_eTargetKinds).ExKindsToSubKindsTypeVal()) {
				case KEnumVal.LV_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrAbilityTargetVal(EAbilityKinds.STAT_LV, -a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.EXP_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrAbilityTargetVal(EAbilityKinds.STAT_EXP, -a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.NUMS_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrAbilityTargetVal(EAbilityKinds.STAT_NUMS, -a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.ENHANCE_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrAbilityTargetVal(EAbilityKinds.STAT_ENHANCE, -a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
			}
		}
	}

	/** 아이템 타겟을 지불한다 */
	private static void PayItemTarget(CItemInfo a_oItemInfo, STTargetInfo a_stTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CItemInfoTable.Inst.TryGetItemInfo((EItemKinds)a_stTargetInfo.Kinds, out STItemInfo stItemInfo);

		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && !a_stTargetInfo.Equals(STTargetInfo.INVALID)));
		CAccess.Assert(!a_bIsEnableAssert || (a_oItemInfo != null || !stItemInfo.m_stCommonInfo.m_bIsRepeat));

		// 아이템 정보가 존재 할 경우
		if(bIsValid && !a_stTargetInfo.Equals(STTargetInfo.INVALID) && (a_oItemInfo != null || !stItemInfo.m_stCommonInfo.m_bIsRepeat)) {
			Func.DoPay(a_oItemInfo ?? CUserInfoStorage.Inst.GetItemInfo((EItemKinds)a_stTargetInfo.Kinds), a_stTargetInfo, a_bIsEnableAssert);
		}
	}

	/** 스킬 타겟을 지불한다 */
	private static void PaySkillTarget(CSkillInfo a_oSkillInfo, STTargetInfo a_stTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CSkillInfoTable.Inst.TryGetSkillInfo((ESkillKinds)a_stTargetInfo.Kinds, out STSkillInfo stSkillInfo);

		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && !a_stTargetInfo.Equals(STTargetInfo.INVALID)));
		CAccess.Assert(!a_bIsEnableAssert || (a_oSkillInfo != null || !stSkillInfo.m_stCommonInfo.m_bIsRepeat));

		// 스킬 정보가 존재 할 경우
		if(bIsValid && !a_stTargetInfo.Equals(STTargetInfo.INVALID) && (a_oSkillInfo != null || !stSkillInfo.m_stCommonInfo.m_bIsRepeat)) {
			Func.DoPay(a_oSkillInfo ?? CUserInfoStorage.Inst.GetSkillInfo((ESkillKinds)a_stTargetInfo.Kinds), a_stTargetInfo, a_bIsEnableAssert);
		}
	}

	/** 객체 타겟을 지불한다 */
	private static void PayObjTarget(CObjInfo a_oObjInfo, STTargetInfo a_stTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CObjInfoTable.Inst.TryGetObjInfo((EObjKinds)a_stTargetInfo.Kinds, out STObjInfo stObjInfo);

		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && !a_stTargetInfo.Equals(STTargetInfo.INVALID)));
		CAccess.Assert(!a_bIsEnableAssert || (a_oObjInfo != null || !stObjInfo.m_stCommonInfo.m_bIsRepeat));

		// 객체 정보가 존재 할 경우
		if(bIsValid && !a_stTargetInfo.Equals(STTargetInfo.INVALID) && (a_oObjInfo != null || !stObjInfo.m_stCommonInfo.m_bIsRepeat)) {
			Func.DoPay(a_oObjInfo ?? CUserInfoStorage.Inst.GetObjInfo((EObjKinds)a_stTargetInfo.Kinds), a_stTargetInfo, a_bIsEnableAssert);
		}
	}

	/** 획득한다 */
	private static void DoAcquire(CTargetInfo a_oTargetInfo, STTargetInfo a_stTargetInfo, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oTargetInfo != null && !a_stTargetInfo.Equals(STTargetInfo.INVALID)));

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfo != null && !a_stTargetInfo.Equals(STTargetInfo.INVALID)) {
			switch(((int)a_stTargetInfo.m_eTargetKinds).ExKindsToSubKindsTypeVal()) {
				case KEnumVal.LV_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrAbilityTargetVal(EAbilityKinds.STAT_LV, a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.EXP_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrAbilityTargetVal(EAbilityKinds.STAT_EXP, a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.NUMS_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrAbilityTargetVal(EAbilityKinds.STAT_NUMS, a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.ENHANCE_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrAbilityTargetVal(EAbilityKinds.STAT_ENHANCE, a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
			}

			a_oTargetInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, out STTargetInfo stLVAbilityTargetInfo);
			a_oTargetInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, out STTargetInfo stNumsAbilityTargetInfo);

			a_oTargetInfo.m_oAbilityTargetInfoDict.ExReplaceAbilityTargetVal(EAbilityKinds.STAT_LV, System.Math.Clamp(stLVAbilityTargetInfo.m_stValInfo01.m_nVal, KCDefine.B_VAL_1_INT, long.MaxValue), a_bIsEnableAssert);
			a_oTargetInfo.m_oAbilityTargetInfoDict.ExReplaceAbilityTargetVal(EAbilityKinds.STAT_NUMS, System.Math.Clamp(stNumsAbilityTargetInfo.m_stValInfo01.m_nVal, KCDefine.B_VAL_1_INT, long.MaxValue), a_bIsEnableAssert);
		}
	}

	/** 아이템 타겟을 획득한다 */
	private static void AcquireItemTarget(CItemInfo a_oItemInfo, STTargetInfo a_stTargetInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || ((a_bIsAutoCreate || a_oItemInfo != null) && !a_stTargetInfo.Equals(STTargetInfo.INVALID)));

		// 아이템 정보가 존재 할 경우
		if((a_bIsAutoCreate || a_oItemInfo != null) && !a_stTargetInfo.Equals(STTargetInfo.INVALID)) {
			Func.DoAcquire(a_oItemInfo ?? Access.GetItemInfo((EItemKinds)a_stTargetInfo.Kinds, KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_bIsAutoCreate), a_stTargetInfo, a_bIsEnableAssert);

			// 광고 제거 아이템 일 경우
			if(a_stTargetInfo.m_eTargetKinds == ETargetKinds.ITEM_NUMS && (EItemKinds)a_stTargetInfo.Kinds == EItemKinds.NON_CONSUMABLE_REMOVE_ADS) {
#if ADS_MODULE_ENABLE
				CAdsManager.Inst.CloseBannerAds(CPluginInfoTable.Inst.AdsPlatform);

				CAdsManager.Inst.IsEnableBannerAds = false;
				CAdsManager.Inst.IsEnableFullscreenAds = false;
#endif			// #if ADS_MODULE_ENABLE
			}
		}
	}

	/** 스킬 타겟을 획득한다 */
	private static void AcquireSkillTarget(CSkillInfo a_oSkillInfo, STTargetInfo a_stTargetInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || ((a_bIsAutoCreate || a_oSkillInfo != null) && !a_stTargetInfo.Equals(STTargetInfo.INVALID)));

		// 스킬 정보가 존재 할 경우
		if((a_bIsAutoCreate || a_oSkillInfo != null) && !a_stTargetInfo.Equals(STTargetInfo.INVALID)) {
			Func.DoAcquire(a_oSkillInfo ?? Access.GetSkillInfo((ESkillKinds)a_stTargetInfo.Kinds, KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_bIsAutoCreate), a_stTargetInfo, a_bIsEnableAssert);
		}
	}

	/** 객체 타겟을 획득한다 */
	private static void AcquireObjTarget(CObjInfo a_oObjInfo, STTargetInfo a_stTargetInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || ((a_bIsAutoCreate || a_oObjInfo != null) && !a_stTargetInfo.Equals(STTargetInfo.INVALID)));

		// 객체 정보가 존재 할 경우
		if((a_bIsAutoCreate || a_oObjInfo != null) && !a_stTargetInfo.Equals(STTargetInfo.INVALID)) {
			Func.DoAcquire(a_oObjInfo ?? Access.GetObjInfo((EObjKinds)a_stTargetInfo.Kinds, KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_bIsAutoCreate), a_stTargetInfo, a_bIsEnableAssert);
		}
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if FIREBASE_MODULE_ENABLE
	/** 로그인 되었을 경우 */
	public static void OnLogin(CFirebaseManager a_oSender, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 로그아웃 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowLoginSuccessPopup(a_oCallback);
		} else {
			Func.ShowLoginFailPopup(a_oCallback);
		}
	}

	/** 로그아웃 되었을 경우 */
	public static void OnLogout(CFirebaseManager a_oSender, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 로그아웃 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowLogoutSuccessPopup(a_oCallback);
		} else {
			Func.ShowLogoutFailPopup(a_oCallback);
		}
	}

	/** 유저 정보가 로드 되었을 경우 */
	public static void OnLoadUserInfo(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 로드 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowLoadSuccessPopup(a_oCallback);
		} else {
			Func.ShowLoadFailPopup(a_oCallback);
		}
	}

	/** 유저 정보가 저장 되었을 경우 */
	public static void OnSaveUserInfo(CFirebaseManager a_oSender, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 저장 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowSaveSuccessPopup(a_oCallback);
		} else {
			Func.ShowSaveFailPopup(a_oCallback);
		}
	}
#endif			// #if FIREBASE_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	/** 상품이 결제 되었을 경우 */
	public static void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowPurchaseSuccessPopup(a_oCallback);
		} else {
			Func.ShowPurchaseFailPopup(a_oCallback);
		}
	}

	/** 상품이 복원 되었을 경우 */
	public static void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 복원 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowRestoreSuccessPopup(a_oCallback);
		} else {
			Func.ShowRestoreFailPopup(a_oCallback);
		}
	}

	/** 상품을 획득한다 */
	public static void AcquireProduct(string a_oProductID, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oProductID.ExIsValid());

		// 상품이 존재 할 경우
		if(a_oProductID.ExIsValid()) {
			int nIdx = CProductInfoTable.Inst.GetProductInfoIdx(a_oProductID);
			var oProduct = CPurchaseManager.Inst.GetProduct(a_oProductID);
			var stProductTradeInfo = CProductTradeInfoTable.Inst.GetBuyProductTradeTradeInfo(nIdx);

			Func.Acquire(stProductTradeInfo.m_oAcquireTargetInfoDict);

#if NEWTON_SOFT_JSON_MODULE_ENABLE
			// 비소모 상품 일 경우
			if(oProduct != null && oProduct.definition.type == ProductType.NonConsumable && !CCommonUserInfoStorage.Inst.IsRestoreProduct(a_oProductID)) {
				CCommonUserInfoStorage.Inst.AddRestoreProductID(a_oProductID);
				CCommonUserInfoStorage.Inst.SaveUserInfo();
			}
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}

	/** 복원 상품을 획득한다 */
	public static void AcquireRestoreProducts(List<Product> a_oProductList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oProductList != null);

#if NEWTON_SOFT_JSON_MODULE_ENABLE
		// 상품이 존재 할 경우
		if(a_oProductList != null) {
			for(int i = 0; i < a_oProductList.Count; ++i) {
				// 상품 복원이 가능 할 경우
				if(!CCommonUserInfoStorage.Inst.IsRestoreProduct(a_oProductList[i].definition.id)) {
					int nIdx = CProductInfoTable.Inst.GetProductInfoIdx(a_oProductList[i].definition.id);
					var stProductTradeInfo = CProductTradeInfoTable.Inst.GetBuyProductTradeTradeInfo(nIdx);
					
					Func.Acquire(stProductTradeInfo.m_oAcquireTargetInfoDict);
					CCommonUserInfoStorage.Inst.AddRestoreProductID(a_oProductList[i].definition.id);
				}				
			}

			CCommonUserInfoStorage.Inst.SaveUserInfo();
		}
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
	}
#endif			// #if PURCHASE_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수
}

/** 스플래시 씬 함수 */
public static partial class Func {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}

/** 시작 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 설정 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 약관 동의 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 지연 설정 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 타이틀 씬 함수 */
public static partial class Func {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}

/** 메인 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 게임 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 로딩 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion			// 클래스 함수
}

/** 중첩 씬 함수 */
public static partial class Func {
	#region 클래스 함수
	
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
