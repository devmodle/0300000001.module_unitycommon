using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if UNITY_EDITOR
using UnityEditor;

/** 에디터 기본 팩토리 */
public static partial class EditorFactory {
	#region 클래스 함수
	
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if RUNTIME_TEMPLATES_MODULE_ENABLE
	/** 판매 아이템 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_SUB_CREATE_BASE + "SaleItemInfoTable", false, KCEditorDefine.B_SORTING_O_SUB_CREATE_MENU + 1)]
	public static void CreateSaleItemInfoTable() {
		CEditorFactory.CreateScriptableObj<CSaleItemInfoTable>(KCEditorDefine.B_ASSET_P_SALE_ITEM_INFO_TABLE);
	}

	/** 판매 상품 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_SUB_CREATE_BASE + "SaleProductInfoTable", false, KCEditorDefine.B_SORTING_O_SUB_CREATE_MENU + 1)]
	public static void CreateSaleProductInfoTable() {
		CEditorFactory.CreateScriptableObj<CSaleProductInfoTable>(KCEditorDefine.B_ASSET_P_SALE_PRODUCT_INFO_TABLE);
	}

	/** 미션 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_SUB_CREATE_BASE + "MissionInfoTable", false, KCEditorDefine.B_SORTING_O_SUB_CREATE_MENU + 1)]
	public static void CreateMissionInfoTable() {
		CEditorFactory.CreateScriptableObj<CMissionInfoTable>(KCEditorDefine.B_ASSET_P_MISSION_INFO_TABLE);
	}

	/** 보상 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_SUB_CREATE_BASE + "RewardInfoTable", false, KCEditorDefine.B_SORTING_O_SUB_CREATE_MENU + 1)]
	public static void CreateRewardInfoTable() {
		CEditorFactory.CreateScriptableObj<CRewardInfoTable>(KCEditorDefine.B_ASSET_P_REWARD_INFO_TABLE);
	}

	/** 에피소드 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_SUB_CREATE_BASE + "EpisodeInfoTable", false, KCEditorDefine.B_SORTING_O_SUB_CREATE_MENU + 1)]
	public static void CreateEpisodeInfoTable() {
		CEditorFactory.CreateScriptableObj<CEpisodeInfoTable>(KCEditorDefine.B_ASSET_P_EPISODE_INFO_TABLE);
	}

	/** 튜토리얼 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_SUB_CREATE_BASE + "TutorialInfoTable", false, KCEditorDefine.B_SORTING_O_SUB_CREATE_MENU + 1)]
	public static void CreateTutorialInfoTable() {
		CEditorFactory.CreateScriptableObj<CTutorialInfoTable>(KCEditorDefine.B_ASSET_P_TUTORIAL_INFO_TABLE);
	}

	/** 효과 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_SUB_CREATE_BASE + "FXInfoTable", false, KCEditorDefine.B_SORTING_O_SUB_CREATE_MENU + 1)]
	public static void CreateFXInfoTable() {
		CEditorFactory.CreateScriptableObj<CFXInfoTable>(KCEditorDefine.B_ASSET_P_FX_INFO_TABLE);
	}
	
	/** 블럭 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_SUB_CREATE_BASE + "BlockInfoTable", false, KCEditorDefine.B_SORTING_O_SUB_CREATE_MENU + 1)]
	public static void CreateBlockInfoTable() {
		CEditorFactory.CreateScriptableObj<CBlockInfoTable>(KCEditorDefine.B_ASSET_P_BLOCK_INFO_TABLE);
	}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
	#endregion			// 조건부 클래스 함수

	#region 추가 클래스 함수

	#endregion			// 추가 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if SCRIPT_TEMPLATE_ONLY
