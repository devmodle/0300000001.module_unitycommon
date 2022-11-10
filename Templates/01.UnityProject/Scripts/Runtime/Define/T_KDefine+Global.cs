#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 전역 상수 */
public static partial class KDefine {
#region 기본
	// 개수
	public const int G_MAX_NUM_VALS = 10;
	public const int G_MAX_NUM_VAL_INFOS = 10;
	public const int G_MAX_NUM_TARGET_INFOS = 25;

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

	// 이름
	public static readonly string G_TABLE_N_VER_INFO = KCDefine.U_TABLE_P_G_VER_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_ETC_INFO = KCDefine.U_TABLE_P_G_ETC_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_MISSION_INFO = KCDefine.U_TABLE_P_G_MISSION_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_REWARD_INFO = KCDefine.U_TABLE_P_G_REWARD_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_RES_INFO = KCDefine.U_TABLE_P_G_RES_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_ITEM_INFO = KCDefine.U_TABLE_P_G_ITEM_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_SKILL_INFO = KCDefine.U_TABLE_P_G_SKILL_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_OBJ_INFO = KCDefine.U_TABLE_P_G_OBJ_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_ABILITY_INFO = KCDefine.U_TABLE_P_G_ABILITY_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_PRODUCT_INFO = KCDefine.U_TABLE_P_G_PRODUCT_INFO.ExGetFileName(false);


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
	public static readonly List<string> G_TABLE_INFO_COMMON_KEY_LIST = new List<string>() {
		"NOEX_T", "NOEX_ST", "NOEX_KT", "NOEX_SKT", "NOEX_DSKT", KCDefine.U_KEY_REPLACE, string.Format(KCDefine.U_KEY_FMT_FLAGS, KCDefine.B_VAL_1_INT), string.Format(KCDefine.U_KEY_FMT_FLAGS, KCDefine.B_VAL_2_INT), string.Format(KCDefine.U_KEY_FMT_FLAGS, KCDefine.B_VAL_2_INT), KCDefine.U_KEY_NAME, KCDefine.U_KEY_DESC
	};

	public static readonly Dictionary<string, int> G_TABLE_INFO_NUM_ROWS_DICT = new Dictionary<string, int>() {
		[KDefine.G_TABLE_N_VER_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_ETC_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_MISSION_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_REWARD_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_RES_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_ITEM_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_SKILL_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_OBJ_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_ABILITY_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_PRODUCT_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS
	};

	public static readonly Dictionary<string, STGoogleSheetTableInfo> G_TABLE_INFO_GOOGLE_SHEET_DICT = new Dictionary<string, STGoogleSheetTableInfo>() {
		[KDefine.G_TABLE_N_VER_INFO] = new STGoogleSheetTableInfo("11lBz0haEcQDRfOcxW-V-CbtD15Go_7Z22CA3qOTtGps", KDefine.G_TABLE_N_VER_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			// Do Something
		}),

		[KDefine.G_TABLE_N_ETC_INFO] = new STGoogleSheetTableInfo("18LvXIUoIet_NT1m5SGyu-fAOoG0UAYVFjlE6rk7rULs", KDefine.G_TABLE_N_ETC_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CCalcInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.U_KEY_CALC
			},

			[typeof(CEpisodeInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.U_KEY_LEVEL_EPISODE] = KCDefine.U_KEY_LEVEL_EPISODE,
				[KCDefine.U_KEY_STAGE_EPISODE] = KCDefine.U_KEY_STAGE_EPISODE,
				[KCDefine.U_KEY_CHAPTER_EPISODE] = KCDefine.U_KEY_CHAPTER_EPISODE
			},

			[typeof(CTutorialInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.U_KEY_TUTORIAL
			},

			[typeof(CFXInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.U_KEY_FX
			}
		}),

		[KDefine.G_TABLE_N_MISSION_INFO] = new STGoogleSheetTableInfo("1dxG0mEGey9eBLC_uR-mu49FWfIXbLCKv3u7L_M0AESM", KDefine.G_TABLE_N_MISSION_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CMissionInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON
			}
		}),

		[KDefine.G_TABLE_N_REWARD_INFO] = new STGoogleSheetTableInfo("1Mp7yIcihpAHvALtGW394NyNF455YF4s41Mo0ZqBi8Ig", KDefine.G_TABLE_N_REWARD_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CRewardInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON
			}
		}),

		[KDefine.G_TABLE_N_RES_INFO] = new STGoogleSheetTableInfo("1OjqxK699MWTcQ0PqQ81EE3HA70aXVMjEJXzhL1ZacJM", KDefine.G_TABLE_N_RES_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CResInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON
			}
		}),

		[KDefine.G_TABLE_N_ITEM_INFO] = new STGoogleSheetTableInfo("18Fzoyyu6yg_FQY8nOi-DaCGqes37oxYuBLabYKOjpnI", KDefine.G_TABLE_N_ITEM_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CItemInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON,
				[KCDefine.B_KEY_BUY_TRADE] = KCDefine.B_KEY_BUY_TRADE,
				[KCDefine.B_KEY_SALE_TRADE] = KCDefine.B_KEY_SALE_TRADE,
				[KCDefine.B_KEY_ENHANCE_TRADE] = KCDefine.B_KEY_ENHANCE_TRADE
			}
		}),

		[KDefine.G_TABLE_N_SKILL_INFO] = new STGoogleSheetTableInfo("1m00Mfx_KuxYYHnwYYerSakn3_dszKZHbMCdpVtAzPos", KDefine.G_TABLE_N_SKILL_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CSkillInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON,
				[KCDefine.B_KEY_BUY_TRADE] = KCDefine.B_KEY_BUY_TRADE,
				[KCDefine.B_KEY_SALE_TRADE] = KCDefine.B_KEY_SALE_TRADE,
				[KCDefine.B_KEY_ENHANCE_TRADE] = KCDefine.B_KEY_ENHANCE_TRADE
			}
		}),

		[KDefine.G_TABLE_N_OBJ_INFO] = new STGoogleSheetTableInfo("1qnJkf80sIwdJaTteymNrUDHSJ-p4162LT6_qvUYGYTY", KDefine.G_TABLE_N_OBJ_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CObjInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON,
				[KCDefine.B_KEY_BUY_TRADE] = KCDefine.B_KEY_BUY_TRADE,
				[KCDefine.B_KEY_SALE_TRADE] = KCDefine.B_KEY_SALE_TRADE,
				[KCDefine.B_KEY_ENHANCE_TRADE] = KCDefine.B_KEY_ENHANCE_TRADE
			}
		}),

		[KDefine.G_TABLE_N_ABILITY_INFO] = new STGoogleSheetTableInfo("1cxc5dC57Go_AMUD0crtHMWxDw5GjEASRrCU7ERKQxQE", KDefine.G_TABLE_N_ABILITY_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CAbilityInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON,
				[KCDefine.B_KEY_ENHANCE_TRADE] = KCDefine.B_KEY_ENHANCE_TRADE
			}
		}),

		[KDefine.G_TABLE_N_PRODUCT_INFO] = new STGoogleSheetTableInfo("1uqeDN-ZaAs3_UxWp93Ub_nEX5r1z7MNDHfwXE-IKB_w", KDefine.G_TABLE_N_PRODUCT_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CProductTradeInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_BUY_TRADE
			}
		})
	};

	public static readonly Dictionary<string, Dictionary<System.Type, Dictionary<string, List<string>>>> G_TABLE_INFO_GOOGLE_SHEET_KEY_DICT_CONTAINER = new Dictionary<string, Dictionary<System.Type, Dictionary<string, List<string>>>>() {
		[KDefine.G_TABLE_N_VER_INFO] = new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			// Do Something
		},

		[KDefine.G_TABLE_N_ETC_INFO] = new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CCalcInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					// Do Something
				}
			},

			[typeof(CEpisodeInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.U_KEY_LEVEL_EPISODE] = new List<string>() {
					// Do Something
				},

				[KCDefine.U_KEY_STAGE_EPISODE] = new List<string>() {
					// Do Something
				},

				[KCDefine.U_KEY_CHAPTER_EPISODE] = new List<string>() {
					// Do Something
				},
			},

			[typeof(CTutorialInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					// Do Something
				}
			},

			[typeof(CFXInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					// Do Something
				}
			}
		},

		[KDefine.G_TABLE_N_MISSION_INFO] = new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CMissionInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					// Do Something
				}
			}
		},

		[KDefine.G_TABLE_N_REWARD_INFO] = new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CRewardInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					// Do Something
				}
			}
		},

		[KDefine.G_TABLE_N_RES_INFO] = new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CResInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					// Do Something
				}
			}
		},

		[KDefine.G_TABLE_N_ITEM_INFO] = new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CItemInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					// Do Something
				},

				[KCDefine.B_KEY_BUY_TRADE] = new List<string>() {
					// Do Something
				},

				[KCDefine.B_KEY_SALE_TRADE] = new List<string>() {
					// Do Something
				},

				[KCDefine.B_KEY_ENHANCE_TRADE] = new List<string>() {
					// Do Something
				}
			}
		},

		[KDefine.G_TABLE_N_SKILL_INFO] = new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CSkillInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					// Do Something
				},

				[KCDefine.B_KEY_BUY_TRADE] = new List<string>() {
					// Do Something
				},

				[KCDefine.B_KEY_SALE_TRADE] = new List<string>() {
					// Do Something
				},

				[KCDefine.B_KEY_ENHANCE_TRADE] = new List<string>() {
					// Do Something
				}
			}
		},

		[KDefine.G_TABLE_N_OBJ_INFO] = new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CObjInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					// Do Something
				},

				[KCDefine.B_KEY_BUY_TRADE] = new List<string>() {
					// Do Something
				},

				[KCDefine.B_KEY_SALE_TRADE] = new List<string>() {
					// Do Something
				},

				[KCDefine.B_KEY_ENHANCE_TRADE] = new List<string>() {
					// Do Something
				}
			}
		},

		[KDefine.G_TABLE_N_ABILITY_INFO] = new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CAbilityInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					// Do Something
				},

				[KCDefine.B_KEY_ENHANCE_TRADE] = new List<string>() {
					// Do Something
				}
			}
		},

		[KDefine.G_TABLE_N_PRODUCT_INFO] = new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CProductTradeInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					// Do Something
				}
			}
		}
	};

	public static readonly Dictionary<string, Dictionary<System.Type, Dictionary<string, List<string>>>> G_TABLE_INFO_EXTRA_GOOGLE_SHEET_NAME_DICT_CONTAINER = new Dictionary<string, Dictionary<System.Type, Dictionary<string, List<string>>>>() {
		[KDefine.G_TABLE_N_PRODUCT_INFO] = new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CProductTradeInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.B_KEY_COMMON, KCDefine.B_PLATFORM_N_IOS_APPLE, KCDefine.B_PLATFORM_N_ANDROID_GOOGLE, KCDefine.B_PLATFORM_N_ANDROID_AMAZON
				}
			}
		}
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
