#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 전역 상수 */
public static partial class KDefine {
#region 기본
	// 개수 {
	public const int G_MAX_NUM_RECORDS = 10;
	public const int G_MAX_NUM_TUTORIAL_STRS = 10;
	public const int G_MAX_NUM_COINS_BOX_COINS = 0;
	public const int G_MAX_NUM_ADS_SKIP_CLEAR_INFOS = 0;

	public const int G_MAX_NUM_FX_KINDS = 10;
	public const int G_MAX_NUM_RES_KINDS = 10;
	public const int G_MAX_NUM_REWARD_KINDS = 10;
	public const int G_MAX_NUM_TARGET_INFOS = 30;
	// 개수 }

	// 횟수
	public const int G_MAX_TIMES_ADS_SKIP = 0;
	public const int G_MAX_TIMES_ACQUIRE_FREE_REWARDS = 0;

	// 시간 {
	public const float G_DELAY_SCALE_01 = 1.0f;
	public const float G_DELAY_SCALE_02 = 1.0f;
	public const float G_DELAY_SCALE_03 = 1.0f;
	public const float G_DELAY_SCALE_04 = 1.0f;
	public const float G_DELAY_SCALE_05 = 1.0f;
	public const float G_DELAY_SCALE_06 = 1.0f;
	public const float G_DELAY_SCALE_07 = 1.0f;
	public const float G_DELAY_SCALE_08 = 1.0f;
	public const float G_DELAY_SCALE_09 = 1.0f;

	public const float G_DELTA_T_SCALE_01 = 1.0f;
	public const float G_DELTA_T_SCALE_02 = 1.0f;
	public const float G_DELTA_T_SCALE_03 = 1.0f;
	public const float G_DELTA_T_SCALE_04 = 1.0f;
	public const float G_DELTA_T_SCALE_05 = 1.0f;
	public const float G_DELTA_T_SCALE_06 = 1.0f;
	public const float G_DELTA_T_SCALE_07 = 1.0f;
	public const float G_DELTA_T_SCALE_08 = 1.0f;
	public const float G_DELTA_T_SCALE_09 = 1.0f;

	public const float G_DURATION_SCALE_01 = 1.0f;
	public const float G_DURATION_SCALE_02 = 1.0f;
	public const float G_DURATION_SCALE_03 = 1.0f;
	public const float G_DURATION_SCALE_04 = 1.0f;
	public const float G_DURATION_SCALE_05 = 1.0f;
	public const float G_DURATION_SCALE_06 = 1.0f;
	public const float G_DURATION_SCALE_07 = 1.0f;
	public const float G_DURATION_SCALE_08 = 1.0f;
	public const float G_DURATION_SCALE_09 = 1.0f;
	// 시간 }

	// 식별자
	public const int G_ID_COMMON_CHARACTER = byte.MaxValue;
#endregion          // 기본               

#region 런타임 상수
	// 버전 {
	public static readonly System.Version G_VER_APP_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_GAME_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_USER_INFO = new System.Version(1, 0, 0);

	public static readonly System.Version G_VER_CELL_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_CLEAR_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_LEVEL_INFO = new System.Version(1, 0, 0);

	public static readonly System.Version G_VER_ITEM_TARGET_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_SKILL_TARGET_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_OBJ_TARGET_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_ABILITY_TARGET_INFO = new System.Version(1, 0, 0);

	public static readonly System.Version G_VER_CHARACTER_OBJ_TARGET_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_CHARACTER_GAME_INFO = new System.Version(1, 0, 0);
	// 버전 }

	// 경로
	public static readonly string G_DATA_P_APP_INFO = $"{KCDefine.B_DIR_P_WRITABLE}AppInfo.bytes";
	public static readonly string G_DATA_P_USER_INFO = $"{KCDefine.B_DIR_P_WRITABLE}UserInfo.bytes";
	public static readonly string G_DATA_P_GAME_INFO = $"{KCDefine.B_DIR_P_WRITABLE}GameInfo.bytes";

	// 정렬 순서
	public static readonly STSortingOrderInfo G_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_OVERLAY_UIS, m_oLayer = KCDefine.U_SORTING_L_DEF
	};

	// 분석 {
	public static readonly List<EAnalytics> G_ANALYTICS_LOG_ENABLE_LIST = new List<EAnalytics>() {
		EAnalytics.FLURRY, EAnalytics.FIREBASE, EAnalytics.APPS_FLYER
	};

	public static readonly List<EAnalytics> G_ANALYTICS_PURCHASE_LOG_ENABLE_LIST = new List<EAnalytics>() {
		EAnalytics.FLURRY, EAnalytics.FIREBASE, EAnalytics.APPS_FLYER
	};
	// 분석 }

	// 테이블 정보 {
	public static readonly Dictionary<string, (string, int)> G_TABLE_INFO_GOOGLE_SHEET_DICT = new Dictionary<string, (string, int)>() {
		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_VER_INFO)] = ("11lBz0haEcQDRfOcxW-V-CbtD15Go_7Z22CA3qOTtGps", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS_AT_ONCE),
		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ETC_INFO)] = ("18LvXIUoIet_NT1m5SGyu-fAOoG0UAYVFjlE6rk7rULs", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS_AT_ONCE),
		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_MISSION_INFO)] = ("1dxG0mEGey9eBLC_uR-mu49FWfIXbLCKv3u7L_M0AESM", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS_AT_ONCE),
		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_REWARD_INFO)] = ("1Mp7yIcihpAHvALtGW394NyNF455YF4s41Mo0ZqBi8Ig", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS_AT_ONCE),
		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_RES_INFO)] = ("1OjqxK699MWTcQ0PqQ81EE3HA70aXVMjEJXzhL1ZacJM", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS_AT_ONCE),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ITEM_INFO)] = ("18Fzoyyu6yg_FQY8nOi-DaCGqes37oxYuBLabYKOjpnI", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS_AT_ONCE),
		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_SKILL_INFO)] = ("1m00Mfx_KuxYYHnwYYerSakn3_dszKZHbMCdpVtAzPos", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS_AT_ONCE),
		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_OBJ_INFO)] = ("1qnJkf80sIwdJaTteymNrUDHSJ-p4162LT6_qvUYGYTY", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS_AT_ONCE),
		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ABILITY_INFO)] = ("1cxc5dC57Go_AMUD0crtHMWxDw5GjEASRrCU7ERKQxQE", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS_AT_ONCE),
		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_PRODUCT_INFO)] = ("1uqeDN-ZaAs3_UxWp93Ub_nEX5r1z7MNDHfwXE-IKB_w", KCDefine.U_MAX_NUM_GOOGLE_SHEET_CELLS_AT_ONCE)
	};

	public static readonly Dictionary<string, (string, Dictionary<System.Type, Dictionary<string, List<string>>>)> G_TABLE_INFO_DICT_CONTAINER = new Dictionary<string, (string, Dictionary<System.Type, Dictionary<string, List<string>>>)>() {
		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ETC_INFO)] = (KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ETC_INFO)].Item1, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CCalcInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() { KCDefine.U_KEY_CALC }
			},

			[typeof(CEpisodeInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.U_KEY_LEVEL_EPISODE] = new List<string>() { KCDefine.U_KEY_LEVEL_EPISODE },
				[KCDefine.U_KEY_STAGE_EPISODE] = new List<string>() { KCDefine.U_KEY_STAGE_EPISODE },
				[KCDefine.U_KEY_CHAPTER_EPISODE] = new List<string>() { KCDefine.U_KEY_CHAPTER_EPISODE }
			},

			[typeof(CTutorialInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() { KCDefine.U_KEY_TUTORIAL }
			},

			[typeof(CFXInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() { KCDefine.U_KEY_FX }
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_MISSION_INFO)] = (KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_MISSION_INFO)].Item1, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CMissionInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() { KCDefine.B_KEY_COMMON }
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_REWARD_INFO)] = (KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_REWARD_INFO)].Item1, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CRewardInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() { KCDefine.B_KEY_COMMON }
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_RES_INFO)] = (KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_RES_INFO)].Item1, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CResInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() { KCDefine.B_KEY_COMMON }
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ITEM_INFO)] = (KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ITEM_INFO)].Item1, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CItemInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() { KCDefine.B_KEY_COMMON },
				[KCDefine.B_KEY_ENHANCE] = new List<string>() { KCDefine.B_KEY_ENHANCE },
				[KCDefine.B_KEY_BUY_TRADE] = new List<string>() { KCDefine.B_KEY_BUY_TRADE },
				[KCDefine.B_KEY_SALE_TRADE] = new List<string>() { KCDefine.B_KEY_SALE_TRADE }
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_SKILL_INFO)] = (KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_SKILL_INFO)].Item1, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CSkillInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() { KCDefine.B_KEY_COMMON },
				[KCDefine.B_KEY_ENHANCE] = new List<string>() { KCDefine.B_KEY_ENHANCE },
				[KCDefine.B_KEY_BUY_TRADE] = new List<string>() { KCDefine.B_KEY_BUY_TRADE },
				[KCDefine.B_KEY_SALE_TRADE] = new List<string>() { KCDefine.B_KEY_SALE_TRADE }
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_OBJ_INFO)] = (KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_OBJ_INFO)].Item1, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CObjInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() { KCDefine.B_KEY_COMMON },
				[KCDefine.B_KEY_ENHANCE] = new List<string>() { KCDefine.B_KEY_ENHANCE },
				[KCDefine.B_KEY_BUY_TRADE] = new List<string>() { KCDefine.B_KEY_BUY_TRADE },
				[KCDefine.B_KEY_SALE_TRADE] = new List<string>() { KCDefine.B_KEY_SALE_TRADE }
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ABILITY_INFO)] = (KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ABILITY_INFO)].Item1, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CAbilityInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() { KCDefine.B_KEY_COMMON },
				[KCDefine.B_KEY_ENHANCE] = new List<string>() { KCDefine.B_KEY_ENHANCE }
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_PRODUCT_INFO)] = (KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_PRODUCT_INFO)].Item1, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CProductTradeInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.B_KEY_COMMON, KCDefine.B_PLATFORM_N_IOS_APPLE, KCDefine.B_PLATFORM_N_ANDROID_GOOGLE, KCDefine.B_PLATFORM_N_ANDROID_AMAZON, KCDefine.U_KEY_PKGS_BUY_TRADE, KCDefine.U_KEY_SINGLE_BUY_TRADE
				}
			}
		})
	};
	// 테이블 정보 }
#endregion          // 런타임 상수                   
}

/** 초기화 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion         // 기본               

#region 런타임 상수
	// 색상
	public static readonly Color IS_COLOR_CLEAR = new Color(0x29 / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0x4c / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0x94 / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 1.0f);
#endregion           // 런타임 상수                   
}

/** 시작 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion          // 기본               

#region 런타임 상수
	// 위치
	public static readonly Vector3 SS_POS_LOADING_TEXT = new Vector3(0.0f, 35.0f, 0.0f);
	public static readonly Vector3 SS_POS_LOADING_GAUGE = KDefine.SS_POS_LOADING_TEXT + new Vector3(0.0f, -70.0f, 0.0f);
#endregion          // 런타임 상수                   
}

/** 설정 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion          // 기본               
}

/** 약관 동의 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion          // 기본               
}

/** 지연 설정 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion          // 기본               
}

/** 타이틀 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion          // 기본               
}

/** 메인 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion          // 기본               
}

/** 게임 씬 상수 */
public static partial class KDefine {
#region 기본
	// 이름
	public const string GS_OBJ_N_ENGINE = "Engine";
#endregion          // 기본               
}

/** 로딩 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion          // 기본               

#region 런타임 상수
	// 위치
	public static readonly Vector3 LS_POS_LOADING_TEXT = new Vector3(0.0f, 35.0f, 0.0f);
	public static readonly Vector3 LS_POS_LOADING_GAUGE = KDefine.LS_POS_LOADING_TEXT + new Vector3(0.0f, -70.0f, 0.0f);
#endregion          // 런타임 상수                   
}

/** 중첩 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion          // 기본               
}
#endif          // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE                                                                                     
#endif          // #if SCRIPT_TEMPLATE_ONLY                                     
