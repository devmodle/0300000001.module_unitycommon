using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if SCENE_TEMPLATES_MODULE_ENABLE
namespace SetupScene {
	/** 서브 설정 씬 관리자 */
	public partial class CSubSetupSceneManager : CSetupSceneManager {
		#region 변수
		[SerializeField] private SystemLanguage m_eSystemLanguage = SystemLanguage.Unknown;
		#endregion			// 변수

		#region 함수
		/** 씬을 설정한다 */
		protected override void Setup() {
			base.Setup();

#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			// 저장소를 로드한다
			CAppInfoStorage.Inst.LoadAppInfo();
			CUserInfoStorage.Inst.LoadUserInfo();
			CGameInfoStorage.Inst.LoadGameInfo();
			
			// 테이블을 로드한다
			CLevelInfoTable.Inst.LoadLevelInfos();
			CItemInfoTable.Inst.LoadItemInfos();
			CItemSaleInfoTable.Inst.LoadItemSaleInfos();
			CProductSaleInfoTable.Inst.LoadProductSaleInfos();
			CMissionInfoTable.Inst.LoadMissionInfos();
			CRewardInfoTable.Inst.LoadRewardInfos();
			CEpisodeInfoTable.Inst.LoadEpisodeInfos();
			CTutorialInfoTable.Inst.LoadTutorialInfos();
			CFXInfoTable.Inst.LoadFXInfos();
			CSkillInfoTable.Inst.LoadSkillInfos();
			CBlockInfoTable.Inst.LoadBlockInfos();
			CResInfoTable.Inst.LoadResInfos();
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE

			// 공용 앱 정보를 설정한다 {
#if EXTRA_SCRIPT_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
			CCommonAppInfoStorage.Inst.StoreURL = Access.StoreURL;
			
#if LOCALIZE_TEST_ENABLE
			CCommonAppInfoStorage.Inst.SystemLanguage = m_eSystemLanguage;
#else
			CCommonAppInfoStorage.Inst.SystemLanguage = Application.systemLanguage;
#endif			// #if LOCALIZE_TEST_ENABLE
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
			// 공용 앱 정보를 설정한다 }
		}
		#endregion			// 함수
	}
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
