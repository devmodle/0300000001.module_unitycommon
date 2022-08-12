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

	// 식별자
	public const int G_ID_COMMON_CHARACTER = byte.MaxValue;
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

	// 정렬 순서
	public static readonly STSortingOrderInfo G_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS = new STSortingOrderInfo() {		
		m_nOrder = KCDefine.U_SORTING_O_OVERLAY_UIS, m_oLayer = KCDefine.U_SORTING_L_DEF
	};

	// 기타 정보 테이블
	public static readonly List<string> G_KEY_ETC_IT_CALC_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_ABILITY_CALC
	};

	// 미션 정보 테이블
	public static readonly List<string> G_KEY_MISSION_IT_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_MAIN, KCDefine.U_KEY_FREE, KCDefine.U_KEY_DAILY, KCDefine.U_KEY_EVENT
	};

	// 보상 정보 테이블
	public static readonly List<string> G_KEY_REWARD_IT_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_FREE, KCDefine.U_KEY_DAILY, KCDefine.U_KEY_EVENT, KCDefine.U_KEY_CLEAR, KCDefine.U_KEY_MISSION, KCDefine.U_KEY_TUTORIAL
	};

	// 에피소드 정보 테이블 {
	public static readonly List<string> G_KEY_EPISODE_IT_LEVEL_EPISODE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_LEVEL_EPISODE
	};

	public static readonly List<string> G_KEY_EPISODE_IT_STAGE_EPISODE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_STAGE_EPISODE
	};

	public static readonly List<string> G_KEY_EPISODE_IT_CHAPTER_EPISODE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_CHAPTER_EPISODE
	};
	// 에피소드 정보 테이블 }

	// 튜토리얼 정보 테이블
	public static readonly List<string> G_KEY_TUTORIAL_IT_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_PLAY_TUTORIAL, KCDefine.U_KEY_HELP_TUTORIAL
	};

	// 리소스 정보 테이블
	public static readonly List<string> G_KEY_RES_IT_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_SND, KCDefine.U_KEY_FONT, KCDefine.U_KEY_IMG, KCDefine.U_KEY_SPRITE, KCDefine.U_KEY_TEXTURE
	};

	// 아이템 정보 테이블 {
	public static readonly List<string> G_KEY_ITEM_IT_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_GOODS, KCDefine.U_KEY_CONSUMABLE, KCDefine.U_KEY_NON_CONSUMABLE, KCDefine.U_KEY_WEAPON, KCDefine.U_KEY_ARMOR, KCDefine.U_KEY_ACCESSORY, KCDefine.U_KEY_ATTACH
	};

	public static readonly List<string> G_KEY_ITEM_IT_ENHANCE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_GOODS_ENHANCE, KCDefine.U_KEY_CONSUMABLE_ENHANCE, KCDefine.U_KEY_NON_CONSUMABLE_ENHANCE, KCDefine.U_KEY_WEAPON_ENHANCE, KCDefine.U_KEY_ARMOR_ENHANCE, KCDefine.U_KEY_ACCESSORY_ENHANCE, KCDefine.U_KEY_ATTACH_ENHANCE
	};

	public static readonly List<string> G_KEY_ITEM_IT_BUY_TRADE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_GOODS_BUY_TRADE, KCDefine.U_KEY_CONSUMABLE_BUY_TRADE, KCDefine.U_KEY_NON_CONSUMABLE_BUY_TRADE, KCDefine.U_KEY_WEAPON_BUY_TRADE, KCDefine.U_KEY_ARMOR_BUY_TRADE, KCDefine.U_KEY_ACCESSORY_BUY_TRADE, KCDefine.U_KEY_ATTACH_BUY_TRADE
	};

	public static readonly List<string> G_KEY_ITEM_IT_SALE_TRADE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_GOODS_SALE_TRADE, KCDefine.U_KEY_CONSUMABLE_SALE_TRADE, KCDefine.U_KEY_NON_CONSUMABLE_SALE_TRADE, KCDefine.U_KEY_WEAPON_SALE_TRADE, KCDefine.U_KEY_ARMOR_SALE_TRADE, KCDefine.U_KEY_ACCESSORY_SALE_TRADE, KCDefine.U_KEY_ATTACH_SALE_TRADE
	};
	// 아이템 정보 테이블 }

	// 스킬 정보 테이블 {
	public static readonly List<string> G_KEY_SKILL_IT_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_ACTION, KCDefine.U_KEY_ACTIVE, KCDefine.U_KEY_PASSIVE
	};

	public static readonly List<string> G_KEY_SKILL_IT_ENHANCE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_ACTION_ENHANCE, KCDefine.U_KEY_ACTIVE_ENHANCE, KCDefine.U_KEY_PASSIVE_ENHANCE
	};

	public static readonly List<string> G_KEY_SKILL_IT_BUY_TRADE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_ACTION_BUY_TRADE, KCDefine.U_KEY_ACTIVE_BUY_TRADE, KCDefine.U_KEY_PASSIVE_BUY_TRADE
	};

	public static readonly List<string> G_KEY_SKILL_IT_SALE_TRADE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_ACTION_SALE_TRADE, KCDefine.U_KEY_ACTIVE_SALE_TRADE, KCDefine.U_KEY_PASSIVE_SALE_TRADE
	};
	// 스킬 정보 테이블 }

	// 객체 정보 테이블 {
	public static readonly List<string> G_KEY_OBJ_IT_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_BG, KCDefine.U_KEY_NORM, KCDefine.U_KEY_OVERLAY, KCDefine.U_KEY_PLAYABLE, KCDefine.U_KEY_NON_PLAYABLE, KCDefine.U_KEY_ENEMY
	};

	public static readonly List<string> G_KEY_OBJ_IT_ENHANCE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_BG_ENHANCE, KCDefine.U_KEY_NORM_ENHANCE, KCDefine.U_KEY_OVERLAY_ENHANCE, KCDefine.U_KEY_PLAYABLE_ENHANCE, KCDefine.U_KEY_NON_PLAYABLE_ENHANCE, KCDefine.U_KEY_ENEMY_ENHANCE
	};

	public static readonly List<string> G_KEY_OBJ_IT_BUY_TRADE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_BG_BUY_TRADE, KCDefine.U_KEY_NORM_BUY_TRADE, KCDefine.U_KEY_OVERLAY_BUY_TRADE, KCDefine.U_KEY_PLAYABLE_BUY_TRADE, KCDefine.U_KEY_NON_PLAYABLE_BUY_TRADE, KCDefine.U_KEY_ENEMY_BUY_TRADE
	};

	public static readonly List<string> G_KEY_OBJ_IT_SALE_TRADE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_BG_SALE_TRADE, KCDefine.U_KEY_NORM_SALE_TRADE, KCDefine.U_KEY_OVERLAY_SALE_TRADE, KCDefine.U_KEY_PLAYABLE_SALE_TRADE, KCDefine.U_KEY_NON_PLAYABLE_SALE_TRADE, KCDefine.U_KEY_ENEMY_SALE_TRADE
	};
	// 객체 정보 테이블 }

	// 효과 정보 테이블
	public static readonly List<string> G_KEY_FX_IT_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_HIT_FX, KCDefine.U_KEY_BUFF_FX, KCDefine.U_KEY_DEBUFF_FX
	};

	// 어빌리티 정보 테이블 {
	public static readonly List<string> G_KEY_ABILITY_IT_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_STAT, KCDefine.U_KEY_BUFF, KCDefine.U_KEY_DEBUFF, KCDefine.U_KEY_UPGRADE
	};

	public static readonly List<string> G_KEY_ABILITY_IT_ENHANCE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_STAT_ENHANCE, KCDefine.U_KEY_BUFF_ENHANCE, KCDefine.U_KEY_DEBUFF_ENHANCE, KCDefine.U_KEY_UPGRADE_ENHANCE
	};
	// 어빌리티 정보 테이블 }

	// 상품 교환 정보 테이블
	public static readonly List<string> G_KEY_PRODUCT_TIT_BUY_TRADE_INFOS_LIST = new List<string>() {
		KCDefine.U_KEY_PKGS_BUY_TRADE, KCDefine.U_KEY_SINGLE_BUY_TRADE
	};

	// 경로 {
#if MSG_PACK_ENABLE
	public static readonly string G_DATA_P_APP_INFO = $"{KCDefine.B_DIR_P_WRITABLE}AppInfo.bytes";
	public static readonly string G_DATA_P_USER_INFO = $"{KCDefine.B_DIR_P_WRITABLE}UserInfo.bytes";
	public static readonly string G_DATA_P_GAME_INFO = $"{KCDefine.B_DIR_P_WRITABLE}GameInfo.bytes";
#elif NEWTON_SOFT_JSON_MODULE_ENABLE
	public static readonly string G_DATA_P_APP_INFO = $"{KCDefine.B_DIR_P_WRITABLE}AppInfo.json";
	public static readonly string G_DATA_P_USER_INFO = $"{KCDefine.B_DIR_P_WRITABLE}UserInfo.json";
	public static readonly string G_DATA_P_GAME_INFO = $"{KCDefine.B_DIR_P_WRITABLE}GameInfo.json";
#endif			// #if MSG_PACK_ENABLE
	// 경로 }

	// 분석 {
	public static readonly List<EAnalytics> G_ANALYTICS_LOG_ENABLE_LIST = new List<EAnalytics>() {
		EAnalytics.FLURRY, EAnalytics.FIREBASE, EAnalytics.APPS_FLYER
	};

	public static readonly List<EAnalytics> G_ANALYTICS_PURCHASE_LOG_ENABLE_LIST = new List<EAnalytics>() {
		EAnalytics.FLURRY, EAnalytics.FIREBASE, EAnalytics.APPS_FLYER
	};
	// 분석 }
	#endregion			// 런타임 상수
}

/** 초기화 씬 상수 */
public static partial class KDefine {
	#region 기본
	
	#endregion			// 기본
}

/** 시작 씬 상수 */
public static partial class KDefine {
	#region 기본
	
	#endregion			// 기본
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
}

/** 중첩 씬 상수 */
public static partial class KDefine {
	#region 기본
	
	#endregion			// 기본
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
