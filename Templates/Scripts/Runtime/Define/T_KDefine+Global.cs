using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 전역 상수
public static partial class KDefine {
	#region 기본
	// 개수
	public const int G_MAX_NUM_CHANGES = 0;
	public const int G_MAX_NUM_REWARD_IT_ITEM_INFOS = 0;
	public const int G_MAX_NUM_SALE_PIT_ITEM_INFOS = 0;

	// 식별자 {
	public const int G_PRODUCT_ID_CHANGES = 0;
	public const int G_PRODUCT_ID_REMOVE_ADS = 1;

	public const string G_KEY_FMT_TUTORIAL_MSG = "TUTORIAL_MSG_{0}_{1}";
	// 식별자 }
	
	// 보상 정보 테이블 {
	public const string G_KEY_REWARD_IT_NAME = "Name";
	public const string G_KEY_REWARD_IT_DESC = "Desc";

	public const string G_KEY_REWARD_IT_REWARD_TYPE = "RewardType";
	public const string G_KEY_REWARD_IT_REWARD_KINDS = "RewardKinds";

	public const string G_KEY_FMT_REWARD_IT_NUM_ITEMS = "NumItems_{0:00}";
	public const string G_KEY_FMT_REWARD_IT_ITEM_KINDS = "ItemKinds_{0:00}";
	// 보상 정보 테이블 }

	// 판매 아이템 정보 테이블 {
	public const string G_KEY_SALE_IIT_NAME = "Name";
	public const string G_KEY_SALE_IIT_DESC = "Desc";

	public const string G_KEY_SALE_IIT_PRICE = "Price";
	public const string G_KEY_SALE_IIT_PRICE_TYPE = "PriceType";
	public const string G_KEY_SALE_IIT_PRICE_KINDS = "PriceKinds";

	public const string G_KEY_SALE_IIT_NUM_ITEMS = "NumItems";
	public const string G_KEY_SALE_IIT_ITEM_KINDS = "ItemKinds";
	public const string G_KEY_SALE_IIT_SALE_ITEM_KINDS = "SaleItemKinds";
	// 판매 아이템 정보 테이블 }

	// 판매 상품 정보 테이블 {
	public const string G_KEY_SALE_PIT_NAME = "Name";
	public const string G_KEY_SALE_PIT_DESC = "Desc";

	public const string G_KEY_SALE_PIT_PRICE_TYPE = "PriceType";
	public const string G_KEY_SALE_PIT_PRICE_KINDS = "PriceKinds";

	public const string G_KEY_FMT_SALE_PIT_NUM_ITEMS = "NumItems_{0:00}";
	public const string G_KEY_FMT_SALE_PIT_ITEM_KINDS = "ItemKinds_{0:00}";
	// 판매 상품 정보 테이블 }

	// 상점 팝업
	public const string G_OBJ_N_STORE_POPUP = "StorePopup";

	// 보상 팝업
	public const string G_OBJ_N_REWARD_POPUP = "RewardPopup";

	// 무료 보상 팝업
	public const string G_OBJ_N_FREE_REWARD_POPUP = "FreeRewardPopup";

	// 일일 보상 팝업
	public const string G_OBJ_N_DAILY_REWARD_POPUP = "DailyRewardPopup";

	// 잔돈 팝업
	public const string G_OBJ_N_CHANGES_POPUP = "ChangesPopup";
	public const string G_OBJ_N_CHANGES_P_PURCHASE_BTN = "PurchaseBtn";

	// 잔돈 획득 팝업
	public const string G_OBJ_N_CHANGES_GET_POPUP = "ChangesGetPopup";

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

	public static readonly STRewardInfo G_INVALID_REWARD_INFO = new STRewardInfo() {
		m_eRewardType = ERewardType.NONE,
		m_eRewardKinds = ERewardKinds.NONE
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
#if UNITY_EDITOR
	public static readonly string G_RUNTIME_TABLE_P_CLEAR_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_CLEAR_REWARD_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_FREE_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_FREE_REWARD_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_DAILY_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_DAILY_REWARD_INFO}.json";

	public static readonly string G_RUNTIME_TABLE_P_SALE_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_ITEM_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_SALE_PRODUCT_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_PRODUCT_INFO}.json";
#else
	public static readonly string G_RUNTIME_TABLE_P_CLEAR_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_CLEAR_REWARD_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_FREE_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_FREE_REWARD_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_DAILY_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_DAILY_REWARD_INFO}.json";

	public static readonly string G_RUNTIME_TABLE_P_SALE_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_ITEM_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_SALE_PRODUCT_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SALE_PRODUCT_INFO}.json";
#endif			// #if UNITY_EDITOR
	// 경로 }
	#endregion			// 런타임 상수
}
#endif			// #if NEVER_USE_THIS
