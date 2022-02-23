using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if SCENE_TEMPLATES_MODULE_ENABLE
/** 서브 초기화 씬 관리자 */
public class CSubInitSceneManager : CInitSceneManager {
	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 씬을 설정한다 */
	protected override void Setup() {
		base.Setup();
		
#if RUNTIME_TEMPLATES_MODULE_ENABLE
		// 테이블을 생성한다 {
		CLevelInfoTable.Create();

		CSaleItemInfoTable.Create(KCDefine.U_ASSET_P_G_SALE_ITEM_INFO_TABLE);
		CSaleProductInfoTable.Create(KCDefine.U_ASSET_P_G_SALE_PRODUCT_INFO_TABLE);
		CMissionInfoTable.Create(KCDefine.U_ASSET_P_G_MISSION_INFO_TABLE);
		CRewardInfoTable.Create(KCDefine.U_ASSET_P_G_REWARD_INFO_TABLE);
		CEpisodeInfoTable.Create(KCDefine.U_ASSET_P_G_EPISODE_INFO_TABLE);
		CTutorialInfoTable.Create(KCDefine.U_ASSET_P_G_TUTORIAL_INFO_TABLE);
		CBlockInfoTable.Create(KCDefine.U_ASSET_P_G_BLOCK_INFO_TABLE);
		// 테이블을 생성한다 }
		
		// 저장소를 생성한다
		CAppInfoStorage.Create();
		CUserInfoStorage.Create();
		CGameInfoStorage.Create();
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
