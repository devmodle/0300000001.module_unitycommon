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
		
		// 테이블을 생성한다 {
		CLevelInfoTable.Create();

		CSaleItemInfoTable.Create(KCDefine.U_ASSET_P_G_SALE_ITEM_INFO_TABLE);
		CSaleProductInfoTable.Create(KCDefine.U_ASSET_P_G_SALE_PRODUCT_INFO_TABLE);
		CMissionInfoTable.Create(KCDefine.U_ASSET_P_G_MISSION_INFO_TABLE);
		CRewardInfoTable.Create(KCDefine.U_ASSET_P_G_REWARD_INFO_TABLE);
		CEpisodeInfoTable.Create(KCDefine.U_ASSET_P_G_EPISODE_INFO_TABLE);
		// 테이블을 생성한다 }
		
		// 저장소를 생성한다
		CAppInfoStorage.Create();
		CUserInfoStorage.Create();
		CGameInfoStorage.Create();

		// 열거형 문자열을 로드한다 {
		CStrTable.Inst.LoadEnumStrs<EUserType>();

		CStrTable.Inst.LoadEnumStrs<ELevelMode>();
		CStrTable.Inst.LoadEnumStrs<EStageMode>();
		CStrTable.Inst.LoadEnumStrs<EChapterMode>();

		CStrTable.Inst.LoadEnumStrs<ETutorialType>();
		CStrTable.Inst.LoadEnumStrs<ETutorialKinds>();
		// 열거형 문자열을 로드한다 }
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
