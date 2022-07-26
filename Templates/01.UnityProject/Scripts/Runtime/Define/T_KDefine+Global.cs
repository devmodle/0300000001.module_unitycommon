using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 전역 상수 */
public static partial class KDefine {
	#region 기본
	// 개수 {
	public const int G_MAX_NUM_RECORDS = 9;
	public const int G_MAX_NUM_TUTORIAL_STRS = 9;
	public const int G_MAX_NUM_COINS_BOX_COINS = 0;
	public const int G_MAX_NUM_ADS_SKIP_CLEAR_INFOS = 0;

	public const int G_MAX_NUM_FX_KINDS = 9;
	public const int G_MAX_NUM_RES_KINDS = 9;
	public const int G_MAX_NUM_TARGET_INFOS = 9;
	public const int G_MAX_NUM_REWARD_KINDS = 9;
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
	#endregion			// 기본

	#region 런타임 상수
	// 버전 {
	public static readonly System.Version G_VER_APP_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_GAME_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_USER_INFO = new System.Version(1, 0, 0);

	public static readonly System.Version G_VER_CELL_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_CLEAR_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_LEVEL_INFO = new System.Version(1, 0, 0);

	public static readonly System.Version G_VER_ITEM_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_SKILL_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_OBJ_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_ABILITY_TARGET_INFO = new System.Version(1, 0, 0);
	// 버전 }

	// 미션 정보 테이블
	public static readonly List<string> G_KEY_MISSION_INFOS = new List<string>() {
		KCDefine.U_KEY_MAIN, KCDefine.U_KEY_FREE, KCDefine.U_KEY_DAILY, KCDefine.U_KEY_EVENT
	};

	// 보상 정보 테이블
	public static readonly List<string> G_KEY_REWARD_INFOS = new List<string>() {
		KCDefine.U_KEY_FREE, KCDefine.U_KEY_DAILY, KCDefine.U_KEY_EVENT, KCDefine.U_KEY_CLEAR, KCDefine.U_KEY_MISSION, KCDefine.U_KEY_TUTORIAL
	};

	// 튜토리얼 정보 테이블
	public static readonly List<string> G_KEY_TUTORIAL_INFOS = new List<string>() {
		KCDefine.U_KEY_PLAY, KCDefine.U_KEY_HELP
	};

	// 리소스 정보 테이블
	public static readonly List<string> G_KEY_RES_INFOS = new List<string>() {
		KCDefine.U_KEY_SND, KCDefine.U_KEY_FONT, KCDefine.U_KEY_IMG, KCDefine.U_KEY_SPRITE, KCDefine.U_KEY_TEXTURE
	};

	// 아이템 정보 테이블 {
	public static readonly List<string> G_KEY_ITEM_INFOS = new List<string>() {
		KCDefine.U_KEY_GOODS, KCDefine.U_KEY_CONSUMABLE, KCDefine.U_KEY_NON_CONSUMABLE, KCDefine.U_KEY_WEAPON, KCDefine.U_KEY_ARMOR, KCDefine.U_KEY_ACCESSORY, KCDefine.U_KEY_ATTACH
	};

	public static readonly List<string> G_KEY_ITEM_SALE_INFOS = new List<string>() {
		KCDefine.U_KEY_GOODS_SALE, KCDefine.U_KEY_CONSUMABLE_SALE, KCDefine.U_KEY_NON_CONSUMABLE_SALE, KCDefine.U_KEY_WEAPON_SALE, KCDefine.U_KEY_ARMOR_SALE, KCDefine.U_KEY_ACCESSORY_SALE, KCDefine.U_KEY_ATTACH_SALE
	};

	public static readonly List<string> G_KEY_ITEM_ENHANCE_INFOS = new List<string>() {
		KCDefine.U_KEY_GOODS_ENHANCE, KCDefine.U_KEY_CONSUMABLE_ENHANCE, KCDefine.U_KEY_NON_CONSUMABLE_ENHANCE, KCDefine.U_KEY_WEAPON_ENHANCE, KCDefine.U_KEY_ARMOR_ENHANCE, KCDefine.U_KEY_ACCESSORY_ENHANCE, KCDefine.U_KEY_ATTACH_ENHANCE
	};
	// 아이템 정보 테이블 }

	// 스킬 정보 테이블 {
	public static readonly List<string> G_KEY_SKILL_INFOS = new List<string>() {
		KCDefine.U_KEY_ACTION, KCDefine.U_KEY_ACTIVE, KCDefine.U_KEY_PASSIVE
	};

	public static readonly List<string> G_KEY_SKILL_SALE_INFOS = new List<string>() {
		KCDefine.U_KEY_ACTION_SALE, KCDefine.U_KEY_ACTIVE_SALE, KCDefine.U_KEY_PASSIVE_SALE
	};

	public static readonly List<string> G_KEY_SKILL_ENHANCE_INFOS = new List<string>() {
		KCDefine.U_KEY_ACTION_ENHANCE, KCDefine.U_KEY_ACTIVE_ENHANCE, KCDefine.U_KEY_PASSIVE_ENHANCE
	};
	// 스킬 정보 테이블 }

	// 객체 정보 테이블 {
	public static readonly List<string> G_KEY_OBJ_INFOS = new List<string>() {
		KCDefine.U_KEY_BG, KCDefine.U_KEY_NORM, KCDefine.U_KEY_OVERLAY, KCDefine.U_KEY_PLAYABLE, KCDefine.U_KEY_NON_PLAYABLE, KCDefine.U_KEY_ENEMY
	};

	public static readonly List<string> G_KEY_OBJ_SALE_INFOS = new List<string>() {
		KCDefine.U_KEY_BG_SALE, KCDefine.U_KEY_NORM_SALE, KCDefine.U_KEY_OVERLAY_SALE, KCDefine.U_KEY_PLAYABLE_SALE, KCDefine.U_KEY_NON_PLAYABLE_SALE, KCDefine.U_KEY_ENEMY_SALE
	};

	public static readonly List<string> G_KEY_OBJ_ENHANCE_INFOS = new List<string>() {
		KCDefine.U_KEY_BG_ENHANCE, KCDefine.U_KEY_NORM_ENHANCE, KCDefine.U_KEY_OVERLAY_ENHANCE, KCDefine.U_KEY_PLAYABLE_ENHANCE, KCDefine.U_KEY_NON_PLAYABLE_ENHANCE, KCDefine.U_KEY_ENEMY_ENHANCE
	};
	// 객체 정보 테이블 }

	// 효과 정보 테이블
	public static readonly List<string> G_KEY_FX_INFOS = new List<string>() {
		KCDefine.U_KEY_HIT, KCDefine.U_KEY_BUFF, KCDefine.U_KEY_DEBUFF
	};

	// 어빌리티 정보 테이블 {
	public static readonly List<string> G_KEY_ABILITY_INFOS = new List<string>() {
		KCDefine.U_KEY_STAT, KCDefine.U_KEY_BUFF, KCDefine.U_KEY_DEBUFF
	};

	public static readonly List<string> G_KEY_ABILITY_ENHANCE_INFOS = new List<string>() {
		KCDefine.U_KEY_STAT_ENHANCE, KCDefine.U_KEY_BUFF_ENHANCE, KCDefine.U_KEY_DEBUFF_ENHANCE
	};
	// 어빌리티 정보 테이블 }

	// 상품 판매 정보 테이블
	public static readonly List<string> G_KEY_PRODUCT_SALE_INFOS = new List<string>() {
		KCDefine.U_KEY_PKGS, KCDefine.U_KEY_SINGLE
	};
	// 식별자 }

	// 정렬 순서
	public static readonly STSortingOrderInfo G_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS = new STSortingOrderInfo() {		
		m_nOrder = KCDefine.U_SORTING_O_OVERLAY_UIS, m_oLayer = KCDefine.U_SORTING_L_DEF
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

/** 스플래시 씬 상수 */
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
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
