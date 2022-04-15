using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 전역 상수 */
public static partial class KDefine {
	#region 기본
	// 개수 {
	public const int G_MAX_NUM_SALE_COINS = 0;
	public const int G_MAX_NUM_TUTORIAL_STRS = 9;

	public const int G_MAX_NUM_SALE_ITEM_INFOS = 9;
	public const int G_MAX_NUM_REWARD_ITEM_INFOS = 9;
	public const int G_MAX_NUM_ADS_SKIP_CLEAR_INFOS = 0;

	public const int G_MAX_NUM_LEVEL_CLEAR_MARKS = 9;
	public const int G_MAX_NUM_STAGE_CLEAR_MARKS = 9;
	public const int G_MAX_NUM_CHAPTER_CLEAR_MARKS = 9;

	public const int G_MAX_NUM_LEVEL_TARGET_KINDS = 9;
	public const int G_MAX_NUM_STAGE_TARGET_KINDS = 9;
	public const int G_MAX_NUM_CHAPTER_TARGET_KINDS = 9;

	public const int G_MAX_NUM_LEVEL_UNLOCK_TARGET_KINDS = 9;
	public const int G_MAX_NUM_STAGE_UNLOCK_TARGET_KINDS = 9;
	public const int G_MAX_NUM_CHAPTER_UNLOCK_TARGET_KINDS = 9;
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
	public const int G_PRODUCT_ID_SALE_COINS = 0;
	public const int G_PRODUCT_ID_REMOVE_ADS = 1;

	// 에피소드 정보 테이블
	public const int G_IDX_EPISODE_IT_LEVEL = 0;
	public const int G_IDX_EPISODE_IT_STAGE = 1;
	public const int G_IDX_EPISODE_IT_CHAPTER = 2;
	#endregion			// 기본

	#region 런타임 상수
	// 버전 {
	public static readonly System.Version G_VER_APP_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_GAME_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_USER_INFO = new System.Version(1, 0, 0);

	public static readonly System.Version G_VER_CELL_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_CLEAR_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_LEVEL_INFO = new System.Version(1, 0, 0);
	// 버전 }

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
	
	// 판매 상품 정보 테이블
	public static readonly List<ESaleProductKinds> G_KINDS_SALE_PIT_SALE_PRODUCT_LIST = new List<ESaleProductKinds>() {
		ESaleProductKinds.SINGLE_SALE_COINS, ESaleProductKinds.SINGLE_REMOVE_ADS
	};

	// 판매 상품
	public static readonly List<ESaleProductKinds> G_SALE_PRODUCT_KINDS_PRODUCT_LIST = new List<ESaleProductKinds>() {
		// Do Something
	};

	// 특수 패키지 판매 상품
	public static readonly List<ESaleProductKinds> G_SALE_PRODUCT_KINDS_PRODUCT_SPECIAL_PKGS_LIST = new List<ESaleProductKinds>() {
		ESaleProductKinds.PKGS_BEGINNER, ESaleProductKinds.PKGS_EXPERT, ESaleProductKinds.PKGS_PRO
	};

	// 일일 보상
	public static readonly List<ERewardKinds> G_REWARDS_KINDS_DAILY_REWARD_LIST = new List<ERewardKinds>() {
		// Do Something
	};
	#endregion			// 런타임 상수
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
