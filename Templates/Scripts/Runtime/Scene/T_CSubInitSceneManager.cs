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
		CSaleItemInfoTable.Create(KCDefine.U_ASSET_P_G_SALE_ITEM_INFO_TABLE);
		CSaleProductInfoTable.Create(KCDefine.U_ASSET_P_G_SALE_PRODUCT_INFO_TABLE);
		CMissionInfoTable.Create(KCDefine.U_ASSET_P_G_MISSION_INFO_TABLE);
		CRewardInfoTable.Create(KCDefine.U_ASSET_P_G_REWARD_INFO_TABLE);
		
		// 저장소를 생성한다
		CAppInfoStorage.Create();
		CUserInfoStorage.Create();
		CGameInfoStorage.Create();

		// 테이블을 로드한다 {
		CStrTable.Inst.LoadEnumStrs<EUserType>();
		CStrTable.Inst.LoadEnumStrs<ELevelMode>();

#if UNITY_EDITOR || UNITY_STANDALONE
		CSaleItemInfoTable.Inst.LoadSaleItemInfosFromFile(KDefine.G_RUNTIME_TABLE_P_SALE_ITEM_INFO);
		CSaleProductInfoTable.Inst.LoadSaleProductInfosFromFile(KDefine.G_RUNTIME_TABLE_P_SALE_PRODUCT_INFO);
		CMissionInfoTable.Inst.LoadMissionInfosFromFile(KDefine.G_RUNTIME_TABLE_P_MISSION_INFO);
		CRewardInfoTable.Inst.LoadRewardInfosFromFile(KDefine.G_RUNTIME_TABLE_P_REWARD_INFO);
#else
		CSaleItemInfoTable.Inst.LoadSaleItemInfosFromRes(KCDefine.U_TABLE_P_G_SALE_ITEM_INFO);
		CSaleProductInfoTable.Inst.LoadSaleProductInfosFromRes(KCDefine.U_TABLE_P_G_SALE_PRODUCT_INFO);
		CMissionInfoTable.Inst.LoadMissionInfosFromRes(KCDefine.U_TABLE_P_G_MISSION_INFO);
		CRewardInfoTable.Inst.LoadRewardInfosFromRes(KCDefine.U_TABLE_P_G_REWARD_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
		// 테이블을 로드한다 }
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
