using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 전역 상수
public static partial class KDefine {
	#region 기본
	// 개수
	public const int G_MAX_NUM_SALE_COINS = 0;
	public const int G_MAX_NUM_SALE_ITEM_INFOS = 0;
	public const int G_MAX_NUM_REWARD_ITEM_INFOS = 0;
	public const int G_MAX_NUM_ACQUIRE_FREE_REWARDS = 0;
	public const int G_MAX_NUM_TUTORIAL_STRS = 0;
	public const int G_MAX_NUM_ADS_SKIP_CLEAR_INFOS = 0;

	// 횟수
	public const int G_MAX_TIMES_ADS_SKIP = 0;

	// 식별자 {
	public const int G_PRODUCT_ID_SALE_COINS = 0;
	public const int G_PRODUCT_ID_REMOVE_ADS = 1;

	public const string G_KEY_FMT_TUTORIAL_MSG = "TUTORIAL_MSG_{0:00}_{1:00}";
	// 식별자 }
	
	// 판매 아이템 정보 테이블 {
	public const string G_KEY_SALE_IIT_PRICE = "Price";
	public const string G_KEY_SALE_IIT_PRICE_KINDS = "PriceKinds";
	public const string G_KEY_SALE_IIT_SALE_ITEM_KINDS = "SaleItemKinds";

	public const string G_KEY_FMT_SALE_IIT_NUM_ITEMS = "NumItems_{0:00}";
	public const string G_KEY_FMT_SALE_IIT_ITEM_KINDS = "ItemKinds_{0:00}";
	// 판매 아이템 정보 테이블 }

	// 판매 상품 정보 테이블 {
	public const string G_KEY_SALE_PIT_PRICE = "Price";
	public const string G_KEY_SALE_PIT_PRICE_KINDS = "PriceKinds";
	public const string G_KEY_SALE_PIT_PRODUCT_KINDS = "ProductKinds";
	public const string G_KEY_SALE_PIT_SALE_PRODUCT_KINDS = "SaleProductKinds";

	public const string G_KEY_FMT_SALE_PIT_NUM_ITEMS = "NumItems_{0:00}";
	public const string G_KEY_FMT_SALE_PIT_ITEM_KINDS = "ItemKinds_{0:00}";
	// 판매 상품 정보 테이블 }

	// 미션 정보 테이블 {
	public const string G_KEY_MISSION_IT_FREE = "Free";
	public const string G_KEY_MISSION_IT_DAILY = "Daily";
	public const string G_KEY_MISSION_IT_EVENT = "Event";

	public const string G_KEY_MISSION_IT_MISSION_KINDS = "MissionKinds";
	public const string G_KEY_MISSION_IT_REWARD_KINDS = "RewardKinds";
	// 미션 정보 테이블 }

	// 보상 정보 테이블 {
	public const string G_KEY_REWARD_IT_FREE = "Free";
	public const string G_KEY_REWARD_IT_DAILY = "Daily";
	public const string G_KEY_REWARD_IT_EVENT = "Event";
	public const string G_KEY_REWARD_IT_CLEAR = "Clear";

	public const string G_KEY_REWARD_IT_REWARD_KINDS = "RewardKinds";
	public const string G_KEY_REWARD_IT_REWARD_QUALITY = "RewardQuality";

	public const string G_KEY_FMT_REWARD_IT_NUM_ITEMS = "NumItems_{0:00}";
	public const string G_KEY_FMT_REWARD_IT_ITEM_KINDS = "ItemKinds_{0:00}";
	// 보상 정보 테이블 }

	// 에피소드 정보 테이블 {
	public const int G_IDX_EPISODE_IT_LEVEL = 0;
	public const int G_IDX_EPISODE_IT_STAGE = 1;
	public const int G_IDX_EPISODE_IT_CHAPTER = 2;

	public const string G_KEY_EPISODE_IT_LEVEL = "Level";
	public const string G_KEY_EPISODE_IT_STAGE = "Stage";
	public const string G_KEY_EPISODE_IT_CHAPTER = "Chapter";
	
	public const string G_KEY_EPISODE_IT_ID = "ID";
	public const string G_KEY_EPISODE_IT_LEVEL_MODE = "LevelMode";
	public const string G_KEY_EPISODE_IT_LEVEL_KINDS = "LevelKinds";
	public const string G_KEY_EPISODE_IT_REWARD_KINDS = "RewardKinds";
	public const string G_KEY_EPISODE_IT_TUTORIAL_KINDS = "TutorialKinds";

	public const string G_KEY_EPISODE_IT_STAGE_ID = "StageID";
	public const string G_KEY_EPISODE_IT_STAGE_KINDS = "StageKinds";

	public const string G_KEY_EPISODE_IT_CHAPTER_ID = "ChapterID";
	public const string G_KEY_EPISODE_IT_CHAPTER_KINDS = "ChapterKinds";

	public const string G_KEY_EPISODE_IT_NUM_TARGETS = "NumTargets";
	public const string G_KEY_EPISODE_IT_UNLOCK_NUM_TARGETS = "UnlockNumTargets";
	// 에피소드 정보 테이블 }

	// 튜토리얼 정보 테이블 {
	public const string G_KEY_TUTORIAL_IT_PLAY = "Play";
	public const string G_KEY_TUTORIAL_IT_HELP = "Help";

	public const string G_KEY_TUTORIAL_IT_REWARD_KINDS = "RewardKinds";
	public const string G_KEY_TUTORIAL_IT_TUTORIAL_KINDS = "TutorialKinds";
	public const string G_KEY_TUTORIAL_IT_NEXT_TUTORIAL_KINDS = "NextTutorialKinds";

	public const string G_KEY_FMT_TUTORIAL_IT_STRS = "Str_{0:00}";
	// 튜토리얼 정보 테이블 }
	
	// 상점 팝업
	public const string G_OBJ_N_STORE_POPUP = "StorePopup";

	// 설정 팝업 {
	public const string G_OBJ_N_SETTINGS_POPUP = "SettingsPopup";

	public const string G_IMG_P_SETTINGS_P_BG_SND_ON = "G_BGSndOn";
	public const string G_IMG_P_SETTINGS_P_BG_SND_OFF = "G_BGSndOff";

	public const string G_IMG_P_SETTINGS_P_FX_SNDS_ON = "G_FXSndsOn";
	public const string G_IMG_P_SETTINGS_P_FX_SNDS_OFF = "G_FXSndsOff";

	public const string G_IMG_P_SETTINGS_P_NOTI_ON = "G_NotiOn";
	public const string G_IMG_P_SETTINGS_P_NOTI_OFF = "G_NotiOff";
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

#if !CAMERA_STACK_ENABLE || UNIVERSAL_PIPELINE_MODULE_ENABLE
	public static readonly STSortingOrderInfo G_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS = new STSortingOrderInfo() {
		m_nOrder = 0,
		m_oLayer = KCDefine.U_SORTING_L_TOP_UIS
	};
#else
	public static readonly STSortingOrderInfo G_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS = new STSortingOrderInfo() {
		m_nOrder = 0,
		m_oLayer = KCDefine.U_SORTING_L_TOP
	};
#endif			// #if !CAMERA_STACK_ENABLE || UNIVERSAL_PIPELINE_MODULE_ENABLE
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

#if UNITY_EDITOR
	public static readonly string G_RUNTIME_DATA_P_FMT_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_DATA_P_FMT_G_LEVEL_INFO}.bytes";

	public static readonly string G_RUNTIME_TABLE_P_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO}.bytes";
	public static readonly string G_RUNTIME_TABLE_P_SALE_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_ITEM_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_SALE_PRODUCT_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_PRODUCT_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_MISSION_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_MISSION_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_REWARD_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_EPISODE_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_EPISODE_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_TUTORIAL_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_TUTORIAL_INFO}.json";
#else
	public static readonly string G_RUNTIME_DATA_P_FMT_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_DATA_P_FMT_G_LEVEL_INFO}.bytes";

	public static readonly string G_RUNTIME_TABLE_P_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO}.bytes";
	public static readonly string G_RUNTIME_TABLE_P_SALE_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_ITEM_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_SALE_PRODUCT_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_PRODUCT_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_MISSION_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_MISSION_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_REWARD_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_EPISODE_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_EPISODE_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_TUTORIAL_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_TUTORIAL_INFO}.json";
#endif			// #if UNITY_EDITOR
	// 경로 }

	// 판매 상품 정보 테이블
	public static readonly ESaleProductKinds[] G_KINDS_SALE_PIT_SALE_PRODUCTS = new ESaleProductKinds[] {
		ESaleProductKinds.SINGLE_SALE_COINS,
		ESaleProductKinds.SINGLE_REMOVE_ADS
	};

	// 보상 정보 테이블
	public static readonly ERewardKinds[] G_REWARDS_KINDS_REWARD_IT_DAILY = new ERewardKinds[] {
		// Do Something
	};

	// 상점 팝업
	public static readonly ESaleProductKinds[] G_KINDS_STORE_POPUP_PRODUCTS = new ESaleProductKinds[] {
		// Do Something
	};
	#endregion			// 런타임 상수

	#region 추가 상수

	#endregion			// 추가 상수
}
#endif			// #if NEVER_USE_THIS
