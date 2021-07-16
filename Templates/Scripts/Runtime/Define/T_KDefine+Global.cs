using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 전역 상수
public static partial class KDefine {
	#region 기본
	// 개수
	public const int G_MAX_NUM_CHANGES = 0;

	// 횟수
	public const int G_MAX_TIMES_ADS_SKIP = 0;

	// 식별자 {
	public const int G_PRODUCT_ID_CHANGES = 0;
	public const int G_PRODUCT_ID_REMOVE_ADS = 1;

	public const string G_KEY_FMT_TUTORIAL_MSG = "TUTORIAL_MSG_{0:00}_{1:00}";
	// 식별자 }

	// 버전 {
	public const string G_VER_APP_INFO = "1.0.0";
	public const string G_VER_USER_INFO = "1.0.0";
	public const string G_VER_GAME_INFO = "1.0.0";
	public const string G_VER_CLEAR_INFO = "1.0.0";

	public const string G_VER_LEVEL_INFO = "1.0.0";
	public const string G_VER_CELL_INFO = "1.0.0";
	// 버전 }
	
	// 판매 아이템 정보 테이블 {
	public const string G_KEY_SALE_IIT_NAME = "Name";
	public const string G_KEY_SALE_IIT_DESC = "Desc";

	public const string G_KEY_SALE_IIT_PRICE = "Price";

	public const string G_KEY_SALE_IIT_SALE_ITEM_TYPE = "SaleItemType";
	public const string G_KEY_SALE_IIT_SALE_ITEM_KINDS = "SaleItemKinds";

	public const string G_KEY_SALE_IIT_PRICE_TYPE = "PriceType";
	public const string G_KEY_SALE_IIT_PRICE_KINDS = "PriceKinds";

	public const string G_KEY_SALE_IIT_NUM_ITEMS = "NumItems";
	public const string G_KEY_SALE_IIT_ITEM_KINDS = "ItemKinds";
	// 판매 아이템 정보 테이블 }

	// 판매 상품 정보 테이블 {
	public const int G_MAX_NUM_SALE_PIT_ITEM_INFOS = 0;
	
	public const string G_KEY_SALE_PIT_NAME = "Name";
	public const string G_KEY_SALE_PIT_DESC = "Desc";

	public const string G_KEY_SALE_PIT_SALE_PRODUCT_TYPE = "SaleProductType";
	public const string G_KEY_SALE_PIT_SALE_PRODUCT_KINDS = "SaleProductKinds";

	public const string G_KEY_SALE_PIT_PRICE_TYPE = "PriceType";
	public const string G_KEY_SALE_PIT_PRICE_KINDS = "PriceKinds";

	public const string G_KEY_FMT_SALE_PIT_NUM_ITEMS = "NumItems_{0:00}";
	public const string G_KEY_FMT_SALE_PIT_ITEM_KINDS = "ItemKinds_{0:00}";
	// 판매 상품 정보 테이블 }

	// 미션 정보 테이블 {
	public const int G_IDX_MISSION_IT_FREE = 0;
	public const int G_IDX_MISSION_IT_DAILY = 1;

	public const string G_KEY_MISSION_IT_FREE = "Free";
	public const string G_KEY_MISSION_IT_DAILY = "Daily";

	public const string G_KEY_MISSION_IT_NAME = "Name";
	public const string G_KEY_MISSION_IT_DESC = "Desc";

	public const string G_KEY_MISSION_IT_MISSION_TYPE = "MissionType";
	public const string G_KEY_MISSION_IT_MISSION_KINDS = "MissionKinds";
	// 미션 정보 테이블 }

	// 보상 정보 테이블 {
	public const int G_IDX_REWARD_IT_FREE = 0;
	public const int G_IDX_REWARD_IT_DAILY = 1;
	public const int G_IDX_REWARD_IT_CLEAR = 2;

	public const int G_MAX_NUM_REWARD_IT_ITEM_INFOS = 0;

	public const string G_KEY_REWARD_IT_FREE = "Free";
	public const string G_KEY_REWARD_IT_DAILY = "Daily";
	public const string G_KEY_REWARD_IT_CLEAR = "Clear";

	public const string G_KEY_REWARD_IT_NAME = "Name";
	public const string G_KEY_REWARD_IT_DESC = "Desc";

	public const string G_KEY_REWARD_IT_REWARD_TYPE = "RewardType";
	public const string G_KEY_REWARD_IT_REWARD_KINDS = "RewardKinds";

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

	public const string G_KEY_EPISODE_IT_NAME = "Name";
	public const string G_KEY_EPISODE_IT_DESC = "Desc";

	public const string G_KEY_EPISODE_IT_ID = "ID";
	public const string G_KEY_EPISODE_IT_LEVEL_MODE = "LevelMode";

	public const string G_KEY_EPISODE_IT_STAGE_ID = "StageID";
	public const string G_KEY_EPISODE_IT_STAGE_MODE = "StageMode";

	public const string G_KEY_EPISODE_IT_CHAPTER_ID = "ChapterID";
	public const string G_KEY_EPISODE_IT_CHAPTER_MODE = "ChapterMode";
	// 에피소드 정보 테이블 }

	// 상점 팝업
	public const string G_OBJ_N_STORE_POPUP = "StorePopup";

	// 설정 팝업
	public const string G_OBJ_N_SETTINGS_POPUP = "SettingsPopup";

	// 일일 미션 팝업
	public const string G_OBJ_N_DAILY_MISSION_POPUP = "DailyMissionPopup";

	// 보상 팝업
	public const string G_OBJ_N_REWARD_POPUP = "RewardPopup";
	public const string G_OBJ_N_REWARD_P_ACQUIRE_BTN = "AcquireBtn";

	// 무료 보상 팝업
	public const string G_OBJ_N_FREE_REWARD_POPUP = "FreeRewardPopup";
	public const string G_OBJ_N_FREE_RP_ADS_BTN = "AdsBtn";

	// 일일 보상 팝업 {
	public const string G_OBJ_N_DAILY_REWARD_POPUP = "DailyRewardPopup";

	public const string G_OBJ_N_DAILY_RP_ADS_BTN = "AdsBtn";
	public const string G_OBJ_N_DAILY_RP_ACQUIRE_BTN = "AcquireBtn";
	// 일일 보상 팝업 }

	// 잔돈 팝업 {
	public const string G_OBJ_N_CHANGES_POPUP = "ChangesPopup";

	public const string G_OBJ_N_CHANGES_P_OK_BTN = "OKBtn";
	public const string G_OBJ_N_CHANGES_P_PURCHASE_BTN = "PurchaseBtn";
	// 잔돈 팝업 }

	// 잔돈 획득 팝업
	public const string G_OBJ_N_CHANGES_ACQUIRE_POPUP = "ChangesAcquirePopup";

	// 포커스 팝업
	public const string G_OBJ_N_FOCUS_POPUP = "FocusPopup";
	public const string G_OBJ_N_FOCUS_P_BLIND_IMG = "BlindImg";
	public const string G_OBJ_N_FOCUS_P_FOCUS_UIS = "FocusUIs";

	// 튜토리얼 팝업
	public const string G_OBJ_N_TUTORIAL_POPUP = "TutorialPopup";
	#endregion			// 기본

	#region 런타임 상수
	// 기타 {
	public static readonly STItemInfo G_INVALID_ITEM_INFO = new STItemInfo() {
		m_eItemKinds = EItemKinds.NONE
	};

	public static readonly STSaleItemInfo G_INVALID_SALE_ITEM_INFO = new STSaleItemInfo() {
		m_ePriceType = EPriceType.NONE,
		m_ePriceKinds = EPriceKinds.NONE,
		
		m_eSaleItemKinds = ESaleItemKinds.NONE
	};

	public static readonly STSaleProductInfo G_INVALID_SALE_PRODUCT_INFO = new STSaleProductInfo() {
		m_ePriceType = EPriceType.NONE,
		m_ePriceKinds = EPriceKinds.NONE
	};

	public static readonly STMissionInfo G_INVALID_MISSION_INFO = new STMissionInfo() {
		m_eMissionType = EMissionType.NONE,
		m_eMissionKinds = EMissionKinds.NONE
	};

	public static readonly STRewardInfo G_INVALID_REWARD_INFO = new STRewardInfo() {
		m_eRewardType = ERewardType.NONE,
		m_eRewardKinds = ERewardKinds.NONE
	};

	public static readonly STLevelInfo G_INVALID_LEVEL_INFO = new STLevelInfo() {
		m_eLevelMode = ELevelMode.NONE
	};

	public static readonly STStageInfo G_INVALID_STAGE_INFO = new STStageInfo() {
		m_eStageMode = EStageMode.NONE
	};

	public static readonly STChapterInfo G_INVALID_CHAPTER_INFO = new STChapterInfo() {
		m_eChapterMode = EChapterMode.NONE
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

#if UNITY_EDITOR
	public static readonly string G_RUNTIME_DATA_P_FMT_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_DATA_P_FMT_G_LEVEL_INFO}.bytes";

	public static readonly string G_RUNTIME_TABLE_P_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO}.bytes";
	public static readonly string G_RUNTIME_TABLE_P_SALE_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_ITEM_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_SALE_PRODUCT_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_PRODUCT_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_MISSION_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_MISSION_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_REWARD_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_EPISODE_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_EPISODE_INFO}.json";
#else
	public static readonly string G_RUNTIME_DATA_P_FMT_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_DATA_P_FMT_G_LEVEL_INFO}.bytes";

	public static readonly string G_RUNTIME_TABLE_P_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO}.bytes";
	public static readonly string G_RUNTIME_TABLE_P_SALE_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_ITEM_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_SALE_PRODUCT_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_PRODUCT_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_MISSION_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_MISSION_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_REWARD_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_EPISODE_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_EPISODE_INFO}.json";
#endif			// #if UNITY_EDITOR
	// 경로 }

	// 판매 상품 정보 테이블
	public static readonly ESaleProductKinds[] G_KINDS_SALE_PIT_SALE_PRODUCTS = new ESaleProductKinds[] {
		ESaleProductKinds.SINGLE_CHANGES,
		ESaleProductKinds.SINGLE_REMOVE_ADS
	};

	// 보상 정보 테이블
	public static readonly ERewardKinds[] G_KINDS_REWARD_IT_DAILY_REWARDS = new ERewardKinds[] {
		ERewardKinds.DAILY_REWARD
	};
	#endregion			// 런타임 상수
}
#endif			// #if NEVER_USE_THIS
