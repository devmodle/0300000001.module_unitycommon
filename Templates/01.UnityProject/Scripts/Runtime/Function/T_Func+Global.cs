#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_MODULE_ENABLE

/** 전역 함수 */
public static partial class Func {
	#region 클래스 함수
	/** 어빌리티 값을 설정한다 */
	public static void SetupAbilityVals(STTargetInfo a_stTargetInfo, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oOutAbilityValDict != null);

		// 어빌리티 값 설정이 가능 할 경우
		if(a_oOutAbilityValDict != null && a_stTargetInfo.m_eTargetKinds == ETargetKinds.ABILITY && a_stTargetInfo.m_nKinds > KCDefine.B_IDX_INVALID) {
			var stAbilityInfo = CAbilityInfoTable.Inst.GetAbilityInfo((EAbilityKinds)a_stTargetInfo.Kinds.ExKindsToCorrectKinds(a_stTargetInfo.m_eKindsGroupType));
			var eAbilityKinds = (EAbilityKinds)a_stTargetInfo.Kinds.ExKindsToCorrectKinds(a_stTargetInfo.m_eKindsGroupType).ExKindsToSubKindsType();

			decimal dmAbilityVal = (stAbilityInfo.m_stValInfo.m_eValType == EValType.INT) ? stAbilityInfo.m_stValInfo.m_nVal : (decimal)stAbilityInfo.m_stValInfo.m_dblVal / KCDefine.B_UNIT_NORM_VAL_TO_PERCENT;
			a_oOutAbilityValDict.ExReplaceVal(eAbilityKinds, System.Math.Clamp(a_oOutAbilityValDict.GetValueOrDefault(eAbilityKinds) + (dmAbilityVal * a_stTargetInfo.m_stValInfo01.m_nVal), decimal.MinValue, decimal.MaxValue), a_bIsEnableAssert);

			foreach(var stKeyVal in stAbilityInfo.m_oExtraAbilityTargetInfoDict) {
				Func.SetupAbilityVals(stKeyVal.Value, a_oOutAbilityValDict, a_bIsEnableAssert);
			}
		}
	}

	/** 어빌리티 값을 설정한다 */
	public static void SetupAbilityVals(Dictionary<ulong, STTargetInfo> a_oAbilityTargetInfoDict, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oAbilityTargetInfoDict != null && a_oOutAbilityValDict != null));

		// 어빌리티 값 설정이 가능 할 경우
		if(a_oAbilityTargetInfoDict != null && a_oOutAbilityValDict != null) {
			foreach(var stKeyVal in a_oAbilityTargetInfoDict) {
				Func.SetupAbilityVals(stKeyVal.Value, a_oOutAbilityValDict);
			}
		}
	}

	/** 어빌리티 값을 설정한다 */
	public static void SetupAbilityVals(STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (!a_stItemInfo.Equals(STItemInfo.INVALID) && a_oOutAbilityValDict != null));

		// 어빌리티 값 설정이 가능 할 경우
		if(!a_stItemInfo.Equals(STItemInfo.INVALID) && a_oOutAbilityValDict != null) {
			Func.SetupAbilityVals(a_stItemInfo.m_oAbilityTargetInfoDict, a_oOutAbilityValDict);

			// 아이템 타겟 정보가 존재 할 경우
			if(a_oItemTargetInfo != null) {
				Func.SetupAbilityVals(a_oItemTargetInfo.m_oAbilityTargetInfoDict, a_oOutAbilityValDict);
			}
		}
	}

	/** 어빌리티 값을 설정한다 */
	public static void SetupAbilityVals(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (!a_stSkillInfo.Equals(STSkillInfo.INVALID) && a_oOutAbilityValDict != null));

		// 어빌리티 값 설정이 가능 할 경우
		if(!a_stSkillInfo.Equals(STSkillInfo.INVALID) && a_oOutAbilityValDict != null) {
			Func.SetupAbilityVals(a_stSkillInfo.m_oAbilityTargetInfoDict, a_oOutAbilityValDict);

			// 스킬 타겟 정보가 존재 할 경우
			if(a_oSkillTargetInfo != null) {
				Func.SetupAbilityVals(a_oSkillTargetInfo.m_oAbilityTargetInfoDict, a_oOutAbilityValDict);
			}
		}
	}

	/** 어빌리티 값을 설정한다 */
	public static void SetupAbilityVals(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (!a_stObjInfo.Equals(STObjInfo.INVALID) && a_oOutAbilityValDict != null));

		// 어빌리티 값 설정이 가능 할 경우
		if(!a_stObjInfo.Equals(STObjInfo.INVALID) && a_oOutAbilityValDict != null) {
			Func.SetupAbilityVals(a_stObjInfo.m_oAbilityTargetInfoDict, a_oOutAbilityValDict);

			// 객체 타겟 정보가 존재 할 경우
			if(a_oObjTargetInfo != null) {
				Func.SetupAbilityVals(a_oObjTargetInfo.m_oAbilityTargetInfoDict, a_oOutAbilityValDict);
			}
		}
	}

	/** 플레이 레벨 정보를 설정한다 */
	public static void SetupPlayLevelInfo(int a_nLevelID, EPlayMode a_ePlayMode, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.PlayMode = a_ePlayMode;
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		CGameInfoStorage.Inst.PlayLevelInfo = CLevelInfoTable.Inst.GetLevelInfo(a_nLevelID, a_nStageID, a_nChapterID);
#else
		CGameInfoStorage.Inst.PlayLevelInfo = CLevelInfoTable.Inst.LoadLevelInfo(a_nLevelID, a_nStageID, a_nChapterID);
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 다음 일일 보상 식별자를 설정한다 */
	public static void SetupNextDailyRewardID(int a_nCharacterID, bool a_bIsResetDailyRewardTime = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo));

		// 캐릭터 정보가 존재 할 경우
		if(CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out oCharacterGameInfo)) {
			// 일일 보상 시간 리셋 모드 일 경우
			if(a_bIsResetDailyRewardTime) {
				oCharacterGameInfo.PrevDailyRewardTime = System.DateTime.Today;
			}

			oCharacterGameInfo.DailyRewardID = (oCharacterGameInfo.DailyRewardID + KCDefine.B_VAL_1_INT) % KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST.Count;
		}
	}

	/** 지불한다 */
	public static void Pay(int a_nCharacterID, STTargetInfo a_stTargetInfo, bool a_bIsEnableAssert = true) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: Func.PayItemTarget(a_stTargetInfo, Access.GetItemTargetInfo(a_nCharacterID, (EItemKinds)a_stTargetInfo.m_nKinds), a_bIsEnableAssert); break;
			case ETargetType.SKILL: Func.PaySkillTarget(a_stTargetInfo, Access.GetSkillTargetInfo(a_nCharacterID, (ESkillKinds)a_stTargetInfo.m_nKinds), a_bIsEnableAssert); break;
			case ETargetType.OBJ: Func.PayObjTarget(a_stTargetInfo, Access.GetObjTargetInfo(a_nCharacterID, (EObjKinds)a_stTargetInfo.m_nKinds), a_bIsEnableAssert); break;
		}
	}

	/** 지불한다 */
	public static void Pay(STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: Func.PayItemTarget(a_stTargetInfo, a_oTargetInfo as CItemTargetInfo, a_bIsEnableAssert); break;
			case ETargetType.SKILL: Func.PaySkillTarget(a_stTargetInfo, a_oTargetInfo as CSkillTargetInfo, a_bIsEnableAssert); break;
			case ETargetType.OBJ: Func.PayObjTarget(a_stTargetInfo, a_oTargetInfo as CObjTargetInfo, a_bIsEnableAssert); break;
		}
	}

	/** 지불한다 */
	public static void Pay(List<(STTargetInfo, CTargetInfo)> a_oPayInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oPayInfoList != null);

		// 지불 정보가 존재 할 경우
		if(a_oPayInfoList != null) {
			for(int i = 0; i < a_oPayInfoList.Count; ++i) {
				Func.Pay(a_oPayInfoList[i].Item1, a_oPayInfoList[i].Item2, a_bIsEnableAssert);
			}
		}
	}

	/** 지불한다 */
	public static void Pay(int a_nCharacterID, Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oTargetInfoDict != null);

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfoDict != null) {
			foreach(var stKeyVal in a_oTargetInfoDict) {
				Func.Pay(a_nCharacterID, stKeyVal.Value, a_bIsEnableAssert);
			}
		}
	}

	/** 획득한다 */
	public static void Acquire(int a_nCharacterID, STTargetInfo a_stTargetInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: Func.AcquireItemTarget(a_stTargetInfo, Access.GetItemTargetInfo(a_nCharacterID, (EItemKinds)a_stTargetInfo.m_nKinds, a_bIsAutoCreate), a_bIsEnableAssert); break;
			case ETargetType.SKILL: Func.AcquireSkillTarget(a_stTargetInfo, Access.GetSkillTargetInfo(a_nCharacterID, (ESkillKinds)a_stTargetInfo.m_nKinds, a_bIsAutoCreate), a_bIsEnableAssert); break;
			case ETargetType.OBJ: Func.AcquireObjTarget(a_stTargetInfo, Access.GetObjTargetInfo(a_nCharacterID, (EObjKinds)a_stTargetInfo.m_nKinds, a_bIsAutoCreate), a_bIsEnableAssert); break;
		}
	}

	/** 획득한다 */
	public static void Acquire(STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: Func.AcquireItemTarget(a_stTargetInfo, a_oTargetInfo as CItemTargetInfo, a_bIsEnableAssert); break;
			case ETargetType.SKILL: Func.AcquireSkillTarget(a_stTargetInfo, a_oTargetInfo as CSkillTargetInfo, a_bIsEnableAssert); break;
			case ETargetType.OBJ: Func.AcquireObjTarget(a_stTargetInfo, a_oTargetInfo as CObjTargetInfo, a_bIsEnableAssert); break;
		}
	}

	/** 획득한다 */
	public static void Acquire(List<(STTargetInfo, CTargetInfo)> a_oAcquireInfoList, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oAcquireInfoList != null);

		// 획득 정보가 존재 할 경우
		if(a_oAcquireInfoList != null) {
			for(int i = 0; i < a_oAcquireInfoList.Count; ++i) {
				Func.Acquire(a_oAcquireInfoList[i].Item1, a_oAcquireInfoList[i].Item2, a_bIsAutoCreate, a_bIsEnableAssert);
			}
		}
	}

	/** 획득한다 */
	public static void Acquire(int a_nCharacterID, Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oTargetInfoDict != null);

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfoDict != null) {
			foreach(var stKeyVal in a_oTargetInfoDict) {
				Func.Acquire(a_nCharacterID, stKeyVal.Value, a_bIsAutoCreate, a_bIsEnableAssert);
			}
		}	
	}

	/** 구입한다 */
	public static void Buy(int a_nCharacterID, STItemTradeInfo a_stItemTradeInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		Func.Pay(a_nCharacterID, a_stItemTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
		Func.Acquire(a_nCharacterID, a_stItemTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
	}

	/** 구입한다 */
	public static void Buy(int a_nCharacterID, STSkillTradeInfo a_stSkillTradeInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		Func.Pay(a_nCharacterID, a_stSkillTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
		Func.Acquire(a_nCharacterID, a_stSkillTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
	}

	/** 구입한다 */
	public static void Buy(int a_nCharacterID, STObjTradeInfo a_stObjTradeInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		Func.Pay(a_nCharacterID, a_stObjTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
		Func.Acquire(a_nCharacterID, a_stObjTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
	}

	/** 구입한다 */
	public static void Buy(int a_nCharacterID, STProductTradeInfo a_stProductTradeInfo, bool a_bIsAutoCreate = false, bool a_bIsEnableAssert = true) {
		Func.Pay(a_nCharacterID, a_stProductTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
		Func.Acquire(a_nCharacterID, a_stProductTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
	}

	/** 판매한다 */
	public static void Sale(int a_nCharacterID, STItemTradeInfo a_stItemTradeInfo, bool a_bIsEnableAssert = true) {
		Func.Buy(a_nCharacterID, a_stItemTradeInfo, false, a_bIsEnableAssert);
	}

	/** 판매한다 */
	public static void Sale(int a_nCharacterID, STSkillTradeInfo a_stSkillTradeInfo, bool a_bIsEnableAssert = true) {
		Func.Buy(a_nCharacterID, a_stSkillTradeInfo, false, a_bIsEnableAssert);
	}

	/** 판매한다 */
	public static void Sale(int a_nCharacterID, STObjTradeInfo a_stObjTradeInfo, bool a_bIsEnableAssert = true) {
		Func.Buy(a_nCharacterID, a_stObjTradeInfo, false, a_bIsEnableAssert);
	}

	/** 무료 보상 획득 횟수를 증가시킨다 */
	public static void IncrFreeRewardAcquireTimes(int a_nCharacterID, int a_nRewardTimes, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo));

		// 캐릭터 게임 정보가 존재 할 경우
		if(CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out oCharacterGameInfo)) {
			oCharacterGameInfo.FreeRewardAcquireTimes = Mathf.Clamp(oCharacterGameInfo.FreeRewardAcquireTimes + a_nRewardTimes, KCDefine.B_VAL_0_INT, KDefine.G_MAX_TIMES_ACQUIRE_FREE_REWARDS);
		}
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

	/** 아이템 타겟을 지불한다 */
	private static void PayItemTarget(STTargetInfo a_stTargetInfo, CItemTargetInfo a_oItemTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CItemInfoTable.Inst.TryGetItemInfo((EItemKinds)a_stTargetInfo.Kinds, out STItemInfo stItemInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oItemTargetInfo != null));

		// 아이템 타겟 정보가 존재 할 경우
		if(bIsValid && a_oItemTargetInfo != null) {
			Func.DoPay(a_stTargetInfo, a_oItemTargetInfo, a_bIsEnableAssert);
		}
	}

	/** 스킬 타겟을 지불한다 */
	private static void PaySkillTarget(STTargetInfo a_stTargetInfo, CSkillTargetInfo a_oSkillTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CSkillInfoTable.Inst.TryGetSkillInfo((ESkillKinds)a_stTargetInfo.Kinds, out STSkillInfo stSkillInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oSkillTargetInfo != null));

		// 스킬 타겟 정보가 존재 할 경우
		if(bIsValid && a_oSkillTargetInfo != null) {
			Func.DoPay(a_stTargetInfo, a_oSkillTargetInfo, a_bIsEnableAssert);
		}
	}

	/** 객체 타겟을 지불한다 */
	private static void PayObjTarget(STTargetInfo a_stTargetInfo, CObjTargetInfo a_oObjTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CObjInfoTable.Inst.TryGetObjInfo((EObjKinds)a_stTargetInfo.Kinds, out STObjInfo stObjInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oObjTargetInfo != null));

		// 객체 정보가 존재 할 경우
		if(bIsValid && a_oObjTargetInfo != null) {
			Func.DoPay(a_stTargetInfo, a_oObjTargetInfo, a_bIsEnableAssert);
		}
	}

	/** 아이템 타겟을 획득한다 */
	private static void AcquireItemTarget(STTargetInfo a_stTargetInfo, CItemTargetInfo a_oItemTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CItemInfoTable.Inst.TryGetItemInfo((EItemKinds)a_stTargetInfo.m_nKinds, out STItemInfo stItemInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oItemTargetInfo != null));

		// 아이템 타겟 정보가 존재 할 경우
		if(bIsValid && a_oItemTargetInfo != null) {
			Func.DoAcquire(a_stTargetInfo, a_oItemTargetInfo, a_bIsEnableAssert);

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
	private static void AcquireSkillTarget(STTargetInfo a_stTargetInfo, CSkillTargetInfo a_oSkillTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CSkillInfoTable.Inst.TryGetSkillInfo((ESkillKinds)a_stTargetInfo.m_nKinds, out STSkillInfo stSkikllInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oSkillTargetInfo != null));

		// 스킬 타겟 정보가 존재 할 경우
		if(bIsValid && a_oSkillTargetInfo != null) {
			Func.DoAcquire(a_stTargetInfo, a_oSkillTargetInfo, a_bIsEnableAssert);
		}
	}

	/** 객체 타겟을 획득한다 */
	private static void AcquireObjTarget(STTargetInfo a_stTargetInfo, CObjTargetInfo a_oObjTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CObjInfoTable.Inst.TryGetObjInfo((EObjKinds)a_stTargetInfo.m_nKinds, out STObjInfo stObjInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oObjTargetInfo != null));

		// 객체 타겟 정보가 존재 할 경우
		if(bIsValid && a_oObjTargetInfo != null) {
			Func.DoAcquire(a_stTargetInfo, a_oObjTargetInfo, a_bIsEnableAssert);
		}
	}

	/** 지불한다 */
	private static void DoPay(STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oTargetInfo != null && !a_stTargetInfo.Equals(STTargetInfo.INVALID)));

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfo != null && !a_stTargetInfo.Equals(STTargetInfo.INVALID)) {
			switch(((int)a_stTargetInfo.m_eTargetKinds).ExKindsToSubKindsTypeVal()) {
				case KEnumVal.LV_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, -a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.EXP_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_EXP, -a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.NUMS_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, -a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.ENHANCE_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_ENHANCE, -a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
			}
		}
	}

	/** 획득한다 */
	private static void DoAcquire(STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oTargetInfo != null && !a_stTargetInfo.Equals(STTargetInfo.INVALID)));

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfo != null && !a_stTargetInfo.Equals(STTargetInfo.INVALID)) {
			switch(((int)a_stTargetInfo.m_eTargetKinds).ExKindsToSubKindsTypeVal()) {
				case KEnumVal.LV_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.EXP_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_EXP, a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.NUMS_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
				case KEnumVal.ENHANCE_TARGET_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_ENHANCE, a_stTargetInfo.m_stValInfo01.m_nVal, a_bIsEnableAssert); break;
			}
			
			a_oTargetInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, out STTargetInfo stLVAbilityTargetInfo);
			a_oTargetInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, out STTargetInfo stNumsAbilityTargetInfo);

			a_oTargetInfo.m_oAbilityTargetInfoDict.ExReplaceTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, System.Math.Clamp(stLVAbilityTargetInfo.m_stValInfo01.m_nVal, KCDefine.B_VAL_1_INT, long.MaxValue), a_bIsEnableAssert);
			a_oTargetInfo.m_oAbilityTargetInfoDict.ExReplaceTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, System.Math.Clamp(stNumsAbilityTargetInfo.m_stValInfo01.m_nVal, KCDefine.B_VAL_1_INT, long.MaxValue), a_bIsEnableAssert);
		}
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if ADS_MODULE_ENABLE
	/** 광고 누적 횟수를 증가시킨다 */
	public static void IncrAdsSkipTimes(int a_nSkipTimes) {
		// 광고 누적 횟수 갱신이 가능 할 경우
		if(CAppInfoStorage.Inst.IsEnableUpdateAdsSkipTimes) {
			CAppInfoStorage.Inst.AdsSkipTimes = Mathf.Clamp(CAppInfoStorage.Inst.AdsSkipTimes + a_nSkipTimes, KCDefine.B_VAL_0_INT, int.MaxValue);
		}
	}

	/** 보상 광고 시청 횟수를 증가시킨다 */
	public static void IncrRewardAdsWatchTimes(int a_nWatchTimes) {
		CAppInfoStorage.Inst.AppInfo.RewardAdsWatchTimes = Mathf.Clamp(CAppInfoStorage.Inst.AppInfo.RewardAdsWatchTimes + a_nWatchTimes, KCDefine.B_VAL_0_INT, int.MaxValue);
	}

	/** 전면 광고 시청 횟수를 증가시킨다 */
	public static void IncrFullscreenAdsWatchTimes(int a_nWatchTimes) {
		CAppInfoStorage.Inst.AppInfo.FullscreenAdsWatchTimes = Mathf.Clamp(CAppInfoStorage.Inst.AppInfo.FullscreenAdsWatchTimes + a_nWatchTimes, KCDefine.B_VAL_0_INT, int.MaxValue);
	}
#endif			// #if ADS_MODULE_ENABLE

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

			Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, stProductTradeInfo.m_oAcquireTargetInfoDict);

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
					
					Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, stProductTradeInfo.m_oAcquireTargetInfoDict);
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

/** 초기화 씬 함수 */
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
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
