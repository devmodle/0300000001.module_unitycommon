using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 초기화 씬 관리자
public class CSubInitSceneManager : CInitSceneManager {
	#region 함수
	//! 씬을 설정한다
	protected override void Setup() {
		base.Setup();
		
		// 테이블을 생성한다
		CRewardInfoTable.Create(KCDefine.U_ASSET_P_G_REWARD_INFO_TABLE);
		CSaleItemInfoTable.Create(KCDefine.U_ASSET_P_G_SALE_ITEM_INFO_TABLE);
		CSaleProductInfoTable.Create(KCDefine.U_ASSET_P_G_SALE_PRODUCT_INFO_TABLE);
		
		// 저장소를 생성한다
		CAppInfoStorage.Create();
		CUserInfoStorage.Create();
		CGameInfoStorage.Create();

		// 테이블을 로드한다 {
		CStrTable.Inst.LoadEnumStrs<EUserType>();
		CStrTable.Inst.LoadEnumStrs<ELevelMode>();

#if UNITY_EDITOR || UNITY_STANDALONE
		CRewardInfoTable.Inst.LoadClearRewardInfosFromFile(KDefine.G_RUNTIME_TABLE_P_CLEAR_REWARD_INFO);
		CRewardInfoTable.Inst.LoadFreeRewardInfosFromFile(KDefine.G_RUNTIME_TABLE_P_FREE_REWARD_INFO);
		CRewardInfoTable.Inst.LoadDailyRewardInfosFromFile(KDefine.G_RUNTIME_TABLE_P_DAILY_REWARD_INFO);

		CSaleItemInfoTable.Inst.LoadSaleItemInfosFromFile(KDefine.G_RUNTIME_TABLE_P_SALE_ITEM_INFO);
		CSaleProductInfoTable.Inst.LoadSaleProductInfosFromFile(KDefine.G_RUNTIME_TABLE_P_SALE_PRODUCT_INFO);
#else
		CRewardInfoTable.Inst.LoadClearRewardInfosFromRes(KCDefine.U_TABLE_P_G_CLEAR_REWARD_INFO);
		CRewardInfoTable.Inst.LoadFreeRewardInfosFromRes(KCDefine.U_TABLE_P_G_FREE_REWARD_INFO);
		CRewardInfoTable.Inst.LoadDailyRewardInfosFromRes(KCDefine.U_TABLE_P_G_DAILY_REWARD_INFO);
		
		CSaleItemInfoTable.Inst.LoadSaleItemInfosFromRes(KCDefine.U_TABLE_P_G_SALE_ITEM_INFO);
		CSaleProductInfoTable.Inst.LoadSaleProductInfosFromRes(KCDefine.U_TABLE_P_G_SALE_PRODUCT_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
		// 테이블을 로드한다 }
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
