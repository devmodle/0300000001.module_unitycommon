#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
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
	public const int G_MAX_NUM_TARGET_INFOS = 20;
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

	// 식별자 {
	public const int G_ID_COMMON_CHARACTER = byte.MaxValue;

	public const string G_ID_VER_INFO_GOOGLE_SHEET = "1iZ4BmPSlmeSSYuEgunzjFYZYXjco0TNZRvdzs9KhCR0";
	public const string G_ID_ETC_INFO_GOOGLE_SHEET = "1g-mjTYHZ6nH1F5KRChgvX8nxoYh9qERdl4xEgEYmSm8";
	public const string G_ID_MISSION_INFO_GOOGLE_SHEET = "1hjD_76YkbNTWUXALjvH2g2P5X0m4fRq0giKCiDkfN6U";
	public const string G_ID_REWARD_INFO_GOOGLE_SHEET = "1ja3glbQdaNO7uL_xNbLIozu8KKxAjBZn59BfKLnKAWg";
	public const string G_ID_RES_INFO_GOOGLE_SHEET = "1HKak3yoptv5FcD-RwefGFnjKSGfUk7GO9htH22O_gPA";
	public const string G_ID_ITEM_INFO_GOOGLE_SHEET = "1WU4K0uEnqWYW5egbIf3JJnSkIKF7zqLsh2_WuCqzcJI";
	public const string G_ID_SKILL_INFO_GOOGLE_SHEET = "1mZQ-G92iBKJEo74RfJ-wx-nGGtOTJNAeNspKaQIZmgw";
	public const string G_ID_OBJ_INFO_GOOGLE_SHEET = "17SADyLxjV82T2PImZPTutdhvwLbkaYklboNQhOjIZPo";
	public const string G_ID_ABILITY_INFO_GOOGLE_SHEET = "15YV8CBGiRi5aUIJYk_pu_EA8nJt_7-3MmT2b-v3vRD4";
	public const string G_ID_PRODUCT_INFO_GOOGLE_SHEET = "18OBRLyR88iEFLYoZatnXr-zwsiEAUCSxoFOykjA5xiY";
	// 식별자 }
	#endregion			// 기본

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

	// 테이블 정보
	public static readonly Dictionary<string, (string, Dictionary<System.Type, Dictionary<string, List<string>>>)> G_TABLE_INFO_DICT_CONTAINER = new Dictionary<string, (string, Dictionary<System.Type, Dictionary<string, List<string>>>)>() {
		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ETC_INFO)] = (KDefine.G_ID_ETC_INFO_GOOGLE_SHEET, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CCalcInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.U_KEY_ABILITY_CALC
				}
			},

			[typeof(CEpisodeInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_LEVEL] = new List<string>() {
					KCDefine.U_KEY_LEVEL_EPISODE
				},

				[KCDefine.B_KEY_STAGE] = new List<string>() {
					KCDefine.U_KEY_STAGE_EPISODE
				},

				[KCDefine.B_KEY_CHAPTER] = new List<string>() {
					KCDefine.U_KEY_CHAPTER_EPISODE
				}
			},

			[typeof(CTutorialInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.U_KEY_PLAY_TUTORIAL, KCDefine.U_KEY_HELP_TUTORIAL
				}
			},

			[typeof(CFXInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.U_KEY_HIT_FX, KCDefine.U_KEY_BUFF_FX, KCDefine.U_KEY_DEBUFF_FX
				}
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_MISSION_INFO)] = (KDefine.G_ID_MISSION_INFO_GOOGLE_SHEET, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CMissionInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.U_KEY_MAIN, KCDefine.U_KEY_FREE, KCDefine.U_KEY_DAILY, KCDefine.U_KEY_EVENT
				}
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_REWARD_INFO)] = (KDefine.G_ID_REWARD_INFO_GOOGLE_SHEET, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CRewardInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.U_KEY_FREE, KCDefine.U_KEY_DAILY, KCDefine.U_KEY_EVENT, KCDefine.U_KEY_CLEAR, KCDefine.U_KEY_MISSION, KCDefine.U_KEY_TUTORIAL
				}
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_RES_INFO)] = (KDefine.G_ID_RES_INFO_GOOGLE_SHEET, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CResInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.U_KEY_SND, KCDefine.U_KEY_FONT, KCDefine.U_KEY_IMG, KCDefine.U_KEY_SPRITE, KCDefine.U_KEY_TEXTURE
				}
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ITEM_INFO)] = (KDefine.G_ID_ITEM_INFO_GOOGLE_SHEET, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CItemInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.U_KEY_GOODS, KCDefine.U_KEY_CONSUMABLE, KCDefine.U_KEY_NON_CONSUMABLE, KCDefine.U_KEY_WEAPON, KCDefine.U_KEY_ARMOR, KCDefine.U_KEY_ACCESSORY, KCDefine.U_KEY_ATTACH
				},

				[KCDefine.B_KEY_ENHANCE] = new List<string>() {
					KCDefine.U_KEY_GOODS_ENHANCE, KCDefine.U_KEY_CONSUMABLE_ENHANCE, KCDefine.U_KEY_NON_CONSUMABLE_ENHANCE, KCDefine.U_KEY_WEAPON_ENHANCE, KCDefine.U_KEY_ARMOR_ENHANCE, KCDefine.U_KEY_ACCESSORY_ENHANCE, KCDefine.U_KEY_ATTACH_ENHANCE
				},

				[KCDefine.B_KEY_BUY_TRADE] = new List<string>() {
					KCDefine.U_KEY_GOODS_BUY_TRADE, KCDefine.U_KEY_CONSUMABLE_BUY_TRADE, KCDefine.U_KEY_NON_CONSUMABLE_BUY_TRADE, KCDefine.U_KEY_WEAPON_BUY_TRADE, KCDefine.U_KEY_ARMOR_BUY_TRADE, KCDefine.U_KEY_ACCESSORY_BUY_TRADE, KCDefine.U_KEY_ATTACH_BUY_TRADE
				},

				[KCDefine.B_KEY_SALE_TRADE] = new List<string>() {
					KCDefine.U_KEY_GOODS_SALE_TRADE, KCDefine.U_KEY_CONSUMABLE_SALE_TRADE, KCDefine.U_KEY_NON_CONSUMABLE_SALE_TRADE, KCDefine.U_KEY_WEAPON_SALE_TRADE, KCDefine.U_KEY_ARMOR_SALE_TRADE, KCDefine.U_KEY_ACCESSORY_SALE_TRADE, KCDefine.U_KEY_ATTACH_SALE_TRADE
				}
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_SKILL_INFO)] = (KDefine.G_ID_SKILL_INFO_GOOGLE_SHEET, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CSkillInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.U_KEY_ACTION, KCDefine.U_KEY_ACTIVE, KCDefine.U_KEY_PASSIVE
				},

				[KCDefine.B_KEY_ENHANCE] = new List<string>() {
					KCDefine.U_KEY_ACTION_ENHANCE, KCDefine.U_KEY_ACTIVE_ENHANCE, KCDefine.U_KEY_PASSIVE_ENHANCE
				},

				[KCDefine.B_KEY_BUY_TRADE] = new List<string>() {
					KCDefine.U_KEY_ACTION_BUY_TRADE, KCDefine.U_KEY_ACTIVE_BUY_TRADE, KCDefine.U_KEY_PASSIVE_BUY_TRADE
				},

				[KCDefine.B_KEY_SALE_TRADE] = new List<string>() {
					KCDefine.U_KEY_ACTION_SALE_TRADE, KCDefine.U_KEY_ACTIVE_SALE_TRADE, KCDefine.U_KEY_PASSIVE_SALE_TRADE
				}
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_OBJ_INFO)] = (KDefine.G_ID_OBJ_INFO_GOOGLE_SHEET, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CObjInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.U_KEY_BG, KCDefine.U_KEY_NORM, KCDefine.U_KEY_OVERLAY, KCDefine.U_KEY_PLAYABLE, KCDefine.U_KEY_NON_PLAYABLE, KCDefine.U_KEY_ENEMY
				},

				[KCDefine.B_KEY_ENHANCE] = new List<string>() {
					KCDefine.U_KEY_BG_ENHANCE, KCDefine.U_KEY_NORM_ENHANCE, KCDefine.U_KEY_OVERLAY_ENHANCE, KCDefine.U_KEY_PLAYABLE_ENHANCE, KCDefine.U_KEY_NON_PLAYABLE_ENHANCE, KCDefine.U_KEY_ENEMY_ENHANCE
				},

				[KCDefine.B_KEY_BUY_TRADE] = new List<string>() {
					KCDefine.U_KEY_BG_BUY_TRADE, KCDefine.U_KEY_NORM_BUY_TRADE, KCDefine.U_KEY_OVERLAY_BUY_TRADE, KCDefine.U_KEY_PLAYABLE_BUY_TRADE, KCDefine.U_KEY_NON_PLAYABLE_BUY_TRADE, KCDefine.U_KEY_ENEMY_BUY_TRADE
				},

				[KCDefine.B_KEY_SALE_TRADE] = new List<string>() {
					KCDefine.U_KEY_BG_SALE_TRADE, KCDefine.U_KEY_NORM_SALE_TRADE, KCDefine.U_KEY_OVERLAY_SALE_TRADE, KCDefine.U_KEY_PLAYABLE_SALE_TRADE, KCDefine.U_KEY_NON_PLAYABLE_SALE_TRADE, KCDefine.U_KEY_ENEMY_SALE_TRADE
				}
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_ABILITY_INFO)] = (KDefine.G_ID_ABILITY_INFO_GOOGLE_SHEET, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CAbilityInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.U_KEY_STAT, KCDefine.U_KEY_BUFF, KCDefine.U_KEY_DEBUFF, KCDefine.U_KEY_UPGRADE
				},

				[KCDefine.B_KEY_ENHANCE] = new List<string>() {
					KCDefine.U_KEY_STAT_ENHANCE, KCDefine.U_KEY_BUFF_ENHANCE, KCDefine.U_KEY_DEBUFF_ENHANCE, KCDefine.U_KEY_UPGRADE_ENHANCE
				}
			}
		}),

		[Path.GetFileNameWithoutExtension(KCDefine.U_TABLE_P_G_PRODUCT_INFO)] = (KDefine.G_ID_PRODUCT_INFO_GOOGLE_SHEET, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CProductTradeInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.U_KEY_PKGS_BUY_TRADE, KCDefine.U_KEY_SINGLE_BUY_TRADE
				}
			}
		})
	};
	#endregion			// 런타임 상수
}

/** 초기화 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion			// 기본

	#region 런타임 상수
	// 색상
	public static readonly Color IS_COLOR_BG_IMG = new Color(0x29 / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0x4c / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0x94 / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 1.0f);

	// 위치
	public static readonly Vector3 IS_POS_SPLASH_IMG = new Vector3(0.0f, 25.0f, 0.0f);
	#endregion			// 런타임 상수
}

/** 시작 씬 상수 */
public static partial class KDefine {
	#region 기본
	
	#endregion			// 기본

	#region 런타임 상수
	// 위치
	public static readonly Vector3 SS_POS_LOADING_TEXT = new Vector3(0.0f, 35.0f, 0.0f);
	public static readonly Vector3 SS_POS_LOADING_GAUGE = KDefine.SS_POS_LOADING_TEXT + new Vector3(0.0f, -70.0f, 0.0f);
	#endregion			// 런타임 상수
}

/** 설정 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion			// 기본
}

/** 약관 동의 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion			// 기본
}

/** 지연 설정 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion			// 기본
}

/** 타이틀 씬 상수 */
public static partial class KDefine {
	#region 기본
	
	#endregion			// 기본
}

/** 메인 씬 상수 */
public static partial class KDefine {
	#region 기본
	
	#endregion			// 기본
}

/** 게임 씬 상수 */
public static partial class KDefine {
	#region 기본
	// 이름
	public const string GS_OBJ_N_ENGINE = "Engine";
	#endregion			// 기본
}

/** 로딩 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion			// 기본

	#region 런타임 상수
	// 위치
	public static readonly Vector3 LS_POS_LOADING_TEXT = new Vector3(0.0f, 35.0f, 0.0f);
	public static readonly Vector3 LS_POS_LOADING_GAUGE = KDefine.LS_POS_LOADING_TEXT + new Vector3(0.0f, -70.0f, 0.0f);
	#endregion			// 런타임 상수
}

/** 중첩 씬 상수 */
public static partial class KDefine {
	#region 기본
	
	#endregion			// 기본
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
