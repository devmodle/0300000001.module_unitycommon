using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 전역 상수
public static partial class KDefine {
	#region 기본
	// 개수
	public const int G_MAX_NUM_CHANGES = 0;
	public const int G_MAX_NUM_DAILY_REWARDS = 0;
	public const int G_MAX_NUM_SALE_ITEM_INFOS = 0;

	// 횟수
	public const int G_MAX_TIMES_FREE_REWARD = 0;

	// 아이템 정보 테이블 {
	public const string G_KEY_ITEM_IT_NAME = "Name";
	public const string G_KEY_ITEM_IT_DESC = "Desc";

	public const string G_KEY_ITEM_IT_PRICE = "Price";
	public const string G_KEY_ITEM_IT_PRICE_TYPE = "PriceType";
	public const string G_KEY_ITEM_IT_PRICE_KINDS = "PriceKinds";

	public const string G_KEY_ITEM_IT_NUM_ITEMS = "NumItems";
	public const string G_KEY_ITEM_IT_ITEM_KINDS = "ItemKinds";
	public const string G_KEY_ITEM_IT_SALE_ITEM_KINDS = "SaleItemKinds";
	// 아이템 정보 테이블 }

	// 판매 상품 정보 테이블 {
	public const string G_KEY_SALE_PIT_NAME = "Name";
	public const string G_KEY_SALE_PIT_DESC = "Desc";

	public const string G_KEY_SALE_PIT_PRICE_TYPE = "PriceType";
	public const string G_KEY_SALE_PIT_PRICE_KINDS = "PriceKinds";

	public const string G_KEY_FMT_SALE_PIT_NUM_ITEMS = "NumItems_{0:00}";
	public const string G_KEY_FMT_SALE_PIT_SALE_ITEM_KINDS = "SaleItemKinds_{0:00}";
	// 판매 상품 정보 테이블 }

	// 상점 팝업
	public const string G_OBJ_N_STORE_POPUP = "StorePopup";

	// 보상 팝업
	public const string G_OBJ_N_REWARD_POPUP = "RewardPopup";

	// 무료 보상 팝업
	public const string G_OBJ_N_FREE_REWARD_POPUP = "FreeRewardPopup";

	// 일일 보상 팝업
	public const string G_OBJ_N_DAILY_REWARD_POPUP = "DailyRewardPopup";
	#endregion			// 기본

	#region 런타임 상수
	// 정렬 순서 {
	public static readonly STSortingOrderInfo U_SORTING_OI_OVERLAY_SCENE_OBJS_CANVAS = new STSortingOrderInfo() {
		m_nOrder = 0,
		m_oLayer = KCDefine.U_SORTING_L_TOP
	};

#if !CAMERA_STACK_ENABLE || UNIVERSAL_PIPELINE_MODULE_ENABLE
	public static readonly STSortingOrderInfo U_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS = new STSortingOrderInfo() {
		m_nOrder = 0,
		m_oLayer = KCDefine.U_SORTING_L_TOP_UIS
	};
#else
	public static readonly STSortingOrderInfo U_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS = new STSortingOrderInfo() {
		m_nOrder = 0,
		m_oLayer = KCDefine.U_SORTING_L_TOP
	};
#endif			// #if !CAMERA_STACK_ENABLE || UNIVERSAL_PIPELINE_MODULE_ENABLE
	// 정렬 순서 }
	
	// 경로 {
#if UNITY_EDITOR
	public static readonly string G_RUNTIME_TABLE_P_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ITEM_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_SALE_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ITEM_INFO}.json";
#else
	public static readonly string G_RUNTIME_TABLE_P_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ITEM_INFO}.json";
	public static readonly string G_RUNTIME_TABLE_P_SALE_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ITEM_INFO}.json";
#endif			// #if UNITY_EDITOR
	// 경로 }
	#endregion			// 런타임 상수
}
#endif			// #if NEVER_USE_THIS
