using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if SCENE_TEMPLATES_MODULE_ENABLE
namespace InitScene {
	/** 서브 초기화 씬 관리자 */
	public partial class CSubInitSceneManager : CInitSceneManager {
		#region 함수
		/** 씬을 설정한다 */
		protected override void Setup() {
			base.Setup();
			
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			// 테이블을 생성한다 {
			CLevelInfoTable.Create();

			CItemSaleInfoTable.Create(KCDefine.U_ASSET_P_G_ITEM_SALE_INFO_TABLE);
			CProductSaleInfoTable.Create(KCDefine.U_ASSET_P_G_PRODUCT_SALE_INFO_TABLE);
			CMissionInfoTable.Create(KCDefine.U_ASSET_P_G_MISSION_INFO_TABLE);
			CRewardInfoTable.Create(KCDefine.U_ASSET_P_G_REWARD_INFO_TABLE);
			CEpisodeInfoTable.Create(KCDefine.U_ASSET_P_G_EPISODE_INFO_TABLE);
			CTutorialInfoTable.Create(KCDefine.U_ASSET_P_G_TUTORIAL_INFO_TABLE);
			CFXInfoTable.Create(KCDefine.U_ASSET_P_G_FX_INFO_TABLE);
			CBlockInfoTable.Create(KCDefine.U_ASSET_P_G_BLOCK_INFO_TABLE);
			CResInfoTable.Create(KCDefine.U_ASSET_P_G_RES_INFO_TABLE);
			// 테이블을 생성한다 }
			
			// 저장소를 생성한다
			CAppInfoStorage.Create();
			CUserInfoStorage.Create();
			CGameInfoStorage.Create();
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		}
		#endregion			// 함수
	}
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
