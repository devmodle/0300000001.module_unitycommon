using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 전역 상수 */
public static partial class KDefine {
	#region 기본
	// 개수 {
	public const int G_MAX_NUM_SALE_COINS = 0;
	public const int G_MAX_NUM_TUTORIAL_STRS = 0;

	public const int G_MAX_NUM_SALE_ITEM_INFOS = 0;
	public const int G_MAX_NUM_REWARD_ITEM_INFOS = 0;
	public const int G_MAX_NUM_ADS_SKIP_CLEAR_INFOS = 0;

	public const int G_MAX_NUM_LEVEL_CLEAR_MARKS = 0;
	public const int G_MAX_NUM_STAGE_CLEAR_MARKS = 0;
	public const int G_MAX_NUM_CHAPTER_CLEAR_MARKS = 0;

	public const int G_MAX_NUM_LEVEL_TARGET_KINDS = 0;
	public const int G_MAX_NUM_STAGE_TARGET_KINDS = 0;
	public const int G_MAX_NUM_CHAPTER_TARGET_KINDS = 0;

	public const int G_MAX_NUM_LEVEL_UNLOCK_TARGET_KINDS = 0;
	public const int G_MAX_NUM_STAGE_UNLOCK_TARGET_KINDS = 0;
	public const int G_MAX_NUM_CHAPTER_UNLOCK_TARGET_KINDS = 0;
	// 개수 }

	// 횟수
	public const int G_MAX_TIMES_ADS_SKIP = 0;
	public const int G_MAX_TIMES_ACQUIRE_FREE_REWARDS = 0;

	// 시간 {
	public const float G_DELAY_SCALE_A = 1.0f;
	public const float G_DELAY_SCALE_B = 1.0f;
	public const float G_DELAY_SCALE_C = 1.0f;
	public const float G_DELAY_SCALE_D = 1.0f;
	public const float G_DELAY_SCALE_E = 1.0f;
	public const float G_DELAY_SCALE_F = 1.0f;
	public const float G_DELAY_SCALE_G = 1.0f;
	public const float G_DELAY_SCALE_H = 1.0f;
	public const float G_DELAY_SCALE_I = 1.0f;

	public const float G_DELTA_T_SCALE_A = 1.0f;
	public const float G_DELTA_T_SCALE_B = 1.0f;
	public const float G_DELTA_T_SCALE_C = 1.0f;
	public const float G_DELTA_T_SCALE_D = 1.0f;
	public const float G_DELTA_T_SCALE_E = 1.0f;
	public const float G_DELTA_T_SCALE_F = 1.0f;
	public const float G_DELTA_T_SCALE_G = 1.0f;
	public const float G_DELTA_T_SCALE_H = 1.0f;
	public const float G_DELTA_T_SCALE_I = 1.0f;

	public const float G_DURATION_SCALE_A = 1.0f;
	public const float G_DURATION_SCALE_B = 1.0f;
	public const float G_DURATION_SCALE_C = 1.0f;
	public const float G_DURATION_SCALE_D = 1.0f;
	public const float G_DURATION_SCALE_E = 1.0f;
	public const float G_DURATION_SCALE_F = 1.0f;
	public const float G_DURATION_SCALE_G = 1.0f;
	public const float G_DURATION_SCALE_H = 1.0f;
	public const float G_DURATION_SCALE_I = 1.0f;
	// 시간 }

	// 식별자
	public const int G_PRODUCT_ID_SALE_COINS = 0;
	public const int G_PRODUCT_ID_REMOVE_ADS = 1;

	// 에피소드 정보 테이블
	public const int G_IDX_EPISODE_IT_LEVEL = 0;
	public const int G_IDX_EPISODE_IT_STAGE = 1;
	public const int G_IDX_EPISODE_IT_CHAPTER = 2;
	
	// 상점 팝업
	public const string G_OBJ_N_STORE_POPUP = "StorePopup";

	// 설정 팝업 {
	public const string G_OBJ_N_SETTINGS_POPUP = "SettingsPopup";

	public const string G_IMG_P_SETTINGS_P_BG_SND_ON = "BGSndOnBtn";
	public const string G_IMG_P_SETTINGS_P_BG_SND_OFF = "BGSndOffBtn";

	public const string G_IMG_P_SETTINGS_P_FX_SNDS_ON = "FXSndsOnBtn";
	public const string G_IMG_P_SETTINGS_P_FX_SNDS_OFF = "FXSndsOffBtn";

	public const string G_IMG_P_SETTINGS_P_NOTI_ON = "NotiOnBtn";
	public const string G_IMG_P_SETTINGS_P_NOTI_OFF = "NotiOffBtn";
	// 설정 팝업 }

	// 동기화 팝업
	public const string G_OBJ_N_SYNC_POPUP = "SyncPopup";

	// 일일 미션 팝업
	public const string G_OBJ_N_DAILY_MISSION_POPUP = "DailyMissionPopup";

	// 무료 보상 팝업
	public const string G_OBJ_N_FREE_REWARD_POPUP = "FreeRewardPopup";

	// 일일 보상 팝업
	public const string G_OBJ_N_DAILY_REWARD_POPUP = "DailyRewardPopup";

	// 판매 코인 팝업
	public const string G_OBJ_N_SALE_COINS_POPUP = "SaleCoinsPopup";

	// 보상 획득 팝업
	public const string G_OBJ_N_REWARD_ACQUIRE_POPUP = "RewardAcquirePopup";

	// 판매 코인 획득 팝업
	public const string G_OBJ_N_SALE_COINS_ACQUIRE_POPUP = "SaleCoinsAcquirePopup";

	// 이어하기 팝업
	public const string G_OBJ_N_CONTINUE_POPUP = "ContinuePopup";

	// 결과 팝업
	public const string G_OBJ_N_RESULT_POPUP = "ResultPopup";

	// 포커스 팝업
	public const string G_OBJ_N_FOCUS_POPUP = "FocusPopup";

	// 튜토리얼 팝업
	public const string G_OBJ_N_TUTORIAL_POPUP = "TutorialPopup";
	#endregion			// 기본

	#region 런타임 상수
	// 기타 {
	public static readonly STItemInfo G_INVALID_ITEM_INFO = new STItemInfo() {
		m_eItemKinds = EItemKinds.NONE
	};

	public static readonly STSaleItemInfo G_INVALID_SALE_ITEM_INFO = new STSaleItemInfo() {
		m_ePriceKinds = EPriceKinds.NONE, m_eSaleItemKinds = ESaleItemKinds.NONE
	};

	public static readonly STSaleProductInfo G_INVALID_SALE_PRODUCT_INFO = new STSaleProductInfo() {
		m_ePriceKinds = EPriceKinds.NONE
	};

	public static readonly STMissionInfo G_INVALID_MISSION_INFO = new STMissionInfo() {
		m_eMissionKinds = EMissionKinds.NONE, m_eRewardKinds = ERewardKinds.NONE
	};

	public static readonly STRewardInfo G_INVALID_REWARD_INFO = new STRewardInfo() {
		m_eRewardKinds = ERewardKinds.NONE, m_eRewardQuality = ERewardQuality.NONE
	};

	public static readonly STLevelInfo G_INVALID_LEVEL_INFO = new STLevelInfo() {
		m_eLevelKinds = ELevelKinds.NONE,
		
		m_stEpisodeInfo = new STCommonEpisodeInfo() {
			m_eDifficulty = EDifficulty.NONE, m_eRewardKinds = ERewardKinds.NONE, m_eTutorialKinds = ETutorialKinds.NONE
		}
	};

	public static readonly STStageInfo G_INVALID_STAGE_INFO = new STStageInfo() {
		m_eStageKinds = EStageKinds.NONE,
		
		m_stEpisodeInfo = new STCommonEpisodeInfo() {
			m_eDifficulty = EDifficulty.NONE, m_eRewardKinds = ERewardKinds.NONE, m_eTutorialKinds = ETutorialKinds.NONE
		}
	};

	public static readonly STChapterInfo G_INVALID_CHAPTER_INFO = new STChapterInfo() {
		m_eChapterKinds = EChapterKinds.NONE,
		
		m_stEpisodeInfo = new STCommonEpisodeInfo() {
			m_eDifficulty = EDifficulty.NONE, m_eRewardKinds = ERewardKinds.NONE, m_eTutorialKinds = ETutorialKinds.NONE
		}
	};

	public static readonly STTutorialInfo G_INVALID_TUTORIAL_INFO = new STTutorialInfo() {
		m_eRewardKinds = ERewardKinds.NONE, m_eTutorialKinds = ETutorialKinds.NONE, m_eNextTutorialKinds = ETutorialKinds.NONE
	};

	public static readonly STBlockInfo G_INVALID_BLOCK_INFO = new STBlockInfo() {
		m_eBlockKinds = EBlockKinds.NONE
	};
	// 기타 }

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
		m_nOrder = 1, m_oLayer = KCDefine.U_SORTING_L_TOP
	};

	// 경로 {
#if MSG_PACK_ENABLE
	public static readonly string G_DATA_P_APP_INFO = $"{KCDefine.B_DIR_P_WRITABLE}AppInfo.bytes";
	public static readonly string G_DATA_P_USER_INFO = $"{KCDefine.B_DIR_P_WRITABLE}UserInfo.bytes";
	public static readonly string G_DATA_P_GAME_INFO = $"{KCDefine.B_DIR_P_WRITABLE}GameInfo.bytes";
#else
	public static readonly string G_DATA_P_APP_INFO = $"{KCDefine.B_DIR_P_WRITABLE}AppInfo.json";
	public static readonly string G_DATA_P_USER_INFO = $"{KCDefine.B_DIR_P_WRITABLE}UserInfo.json";
	public static readonly string G_DATA_P_GAME_INFO = $"{KCDefine.B_DIR_P_WRITABLE}GameInfo.json";
#endif			// #if MSG_PACK_ENABLE
	// 경로 }

	// 분석 {
	public static readonly List<EAnalyticsType> G_ANALYTICS_TYPE_LOG_ENABLE_LIST = new List<EAnalyticsType>() {
		EAnalyticsType.FLURRY, EAnalyticsType.FIREBASE, EAnalyticsType.APPS_FLYER
	};

	public static readonly List<EAnalyticsType> G_ANALYTICS_TYPE_PURCHASE_LOG_ENABLE_LIST = new List<EAnalyticsType>() {
		EAnalyticsType.FLURRY, EAnalyticsType.FIREBASE, EAnalyticsType.APPS_FLYER
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

	// 일일 보상
	public static readonly List<ERewardKinds> G_REWARDS_KINDS_DAILY_REWARD_LIST = new List<ERewardKinds>() {
		// Do Something
	};
	#endregion			// 런타임 상수

	#region 추가 상수
	
	#endregion			// 추가 상수

	#region 추가 런타임 상수

	#endregion			// 추가 런타임 상수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
