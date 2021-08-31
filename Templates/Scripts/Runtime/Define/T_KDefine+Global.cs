using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 전역 상수
public static partial class KDefine {
	#region 기본
	// 개수 {
	public const int G_MAX_NUM_SALE_COINS = 0;
	public const int G_MAX_NUM_TUTORIAL_STRS = 0;
	public const int G_MAX_NUM_ACQUIRE_FREE_REWARDS = 0;

	public const int G_MAX_NUM_TARGET_KINDS = 0;
	public const int G_MAX_NUM_UNLOCK_TARGET_KINDS = 0;

	public const int G_MAX_NUM_SALE_ITEM_INFOS = 0;
	public const int G_MAX_NUM_REWARD_ITEM_INFOS = 0;
	public const int G_MAX_NUM_ADS_SKIP_CLEAR_INFOS = 0;
	// 개수 }

	// 횟수
	public const int G_MAX_TIMES_ADS_SKIP = 0;

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

	public const string G_IMG_P_SETTINGS_P_BG_SND_ON = "SampleOnBtn";
	public const string G_IMG_P_SETTINGS_P_BG_SND_OFF = "SampleOffBtn";

	public const string G_IMG_P_SETTINGS_P_FX_SNDS_ON = "SampleOnBtn";
	public const string G_IMG_P_SETTINGS_P_FX_SNDS_OFF = "SampleOffBtn";

	public const string G_IMG_P_SETTINGS_P_NOTI_ON = "SampleOnBtn";
	public const string G_IMG_P_SETTINGS_P_NOTI_OFF = "SampleOffBtn";
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
		m_ePriceKinds = EPriceKinds.NONE,
		m_eSaleItemKinds = ESaleItemKinds.NONE
	};

	public static readonly STSaleProductInfo G_INVALID_SALE_PRODUCT_INFO = new STSaleProductInfo() {
		m_ePriceKinds = EPriceKinds.NONE
	};

	public static readonly STMissionInfo G_INVALID_MISSION_INFO = new STMissionInfo() {
		m_eMissionKinds = EMissionKinds.NONE,
		m_eRewardKinds = ERewardKinds.NONE
	};

	public static readonly STRewardInfo G_INVALID_REWARD_INFO = new STRewardInfo() {
		m_eRewardKinds = ERewardKinds.NONE
	};

	public static readonly STLevelInfo G_INVALID_LEVEL_INFO = new STLevelInfo() {
		m_eLevelMode = ELevelMode.NONE,
		m_eLevelKinds = ELevelKinds.NONE,
		m_eRewardKinds = ERewardKinds.NONE,
		m_eTutorialKinds = ETutorialKinds.NONE
	};

	public static readonly STStageInfo G_INVALID_STAGE_INFO = new STStageInfo() {
		m_eStageKinds = EStageKinds.NONE,
		m_eRewardKinds = ERewardKinds.NONE,
		m_eTutorialKinds = ETutorialKinds.NONE
	};

	public static readonly STChapterInfo G_INVALID_CHAPTER_INFO = new STChapterInfo() {
		m_eChapterKinds = EChapterKinds.NONE,
		m_eRewardKinds = ERewardKinds.NONE,
		m_eTutorialKinds = ETutorialKinds.NONE
	};

	public static readonly STTutorialInfo G_INVALID_TUTORIAL_INFO = new STTutorialInfo() {
		m_eRewardKinds = ERewardKinds.NONE,
		m_eTutorialKinds = ETutorialKinds.NONE,
		m_eNextTutorialKinds = ETutorialKinds.NONE
	};
	// 기타 }

	// 정렬 순서 {
	public static readonly STSortingOrderInfo G_SORTING_OI_OVERLAY_SCENE_OBJS_CANVAS = new STSortingOrderInfo() {
		m_nOrder = 0,
		m_oLayer = KCDefine.U_SORTING_L_TOP
	};

#if CAMERA_STACK_ENABLE
	public static readonly STSortingOrderInfo G_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS = new STSortingOrderInfo() {
		m_nOrder = 1,
		m_oLayer = KCDefine.U_SORTING_L_TOP
	};
#else
	public static readonly STSortingOrderInfo G_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS = new STSortingOrderInfo() {
		m_nOrder = 0,
		m_oLayer = KCDefine.U_SORTING_L_TOP_UIS
	};
#endif			// #if CAMERA_STACK_ENABLE
	// 정렬 순서 }

	// 경로 {
	public static readonly string G_DATA_P_APP_INFO = $"{KCDefine.B_DIR_P_WRITABLE}AppInfo.bytes";
	public static readonly string G_DATA_P_USER_INFO = $"{KCDefine.B_DIR_P_WRITABLE}UserInfo.bytes";
	public static readonly string G_DATA_P_GAME_INFO = $"{KCDefine.B_DIR_P_WRITABLE}GameInfo.bytes";

	public static readonly string G_ASSET_P_SPRITE_ATLAS_02 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_02";
	public static readonly string G_ASSET_P_SPRITE_ATLAS_03 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_03";
	public static readonly string G_ASSET_P_SPRITE_ATLAS_04 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_04";
	public static readonly string G_ASSET_P_SPRITE_ATLAS_05 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_05";
	public static readonly string G_ASSET_P_SPRITE_ATLAS_06 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_06";
	public static readonly string G_ASSET_P_SPRITE_ATLAS_07 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_07";
	public static readonly string G_ASSET_P_SPRITE_ATLAS_08 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_08";
	public static readonly string G_ASSET_P_SPRITE_ATLAS_09 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_09";
	// 경로 }

	// 분석 {
	public static readonly EAnalyticsType[] G_ANALYTICS_TYPE_LOG_ENABLES = new EAnalyticsType[] {
		EAnalyticsType.FLURRY,
		EAnalyticsType.FIREBASE,
		EAnalyticsType.APPS_FLYER,
		EAnalyticsType.GAME_ANALYTICS,
		EAnalyticsType.SINGULAR
	};

	public static readonly EAnalyticsType[] G_ANALYTICS_TYPE_PURCHASE_LOG_ENABLES = new EAnalyticsType[] {
		EAnalyticsType.FLURRY,
		EAnalyticsType.FIREBASE,
		EAnalyticsType.APPS_FLYER,
		EAnalyticsType.GAME_ANALYTICS,
		EAnalyticsType.SINGULAR
	};
	// 분석 }

	// 판매 상품 정보 테이블
	public static readonly ESaleProductKinds[] G_KINDS_SALE_PIT_SALE_PRODUCTS = new ESaleProductKinds[] {
		ESaleProductKinds.SINGLE_SALE_COINS,
		ESaleProductKinds.SINGLE_REMOVE_ADS
	};

	// 판매 상품
	public static readonly ESaleProductKinds[] G_SALE_PRODUCT_KINDS_PRODUCTS = new ESaleProductKinds[] {
		// Do Something
	};

	// 일일 보상
	public static readonly ERewardKinds[] G_REWARDS_KINDS_DAILY_REWARDS = new ERewardKinds[] {
		// Do Something
	};
	#endregion			// 런타임 상수

	#region 추가 상수

	#endregion			// 추가 상수
}
#endif			// #if NEVER_USE_THIS
