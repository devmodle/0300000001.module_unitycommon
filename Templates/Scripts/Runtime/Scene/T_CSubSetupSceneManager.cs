using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if SCENE_TEMPLATES_MODULE_ENABLE
/** 서브 설정 씬 관리자 */
public class CSubSetupSceneManager : CSetupSceneManager {
	#region 변수
	[SerializeField] private SystemLanguage m_eSystemLanguage = SystemLanguage.Unknown;
	#endregion			// 변수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 씬을 설정한다 */
	protected override void Setup() {
		base.Setup();

#if RUNTIME_TEMPLATES_MODULE_ENABLE
		// 저장소를 로드한다
		CAppInfoStorage.Inst.LoadAppInfo();
		CUserInfoStorage.Inst.LoadUserInfo();
		CGameInfoStorage.Inst.LoadGameInfo();
		
		// 테이블을 로드한다
		CLevelInfoTable.Inst.LoadLevelInfos();
		CSaleItemInfoTable.Inst.LoadSaleItemInfos();
		CSaleProductInfoTable.Inst.LoadSaleProductInfos();
		CMissionInfoTable.Inst.LoadMissionInfos();
		CRewardInfoTable.Inst.LoadRewardInfos();
		CEpisodeInfoTable.Inst.LoadEpisodeInfos();
		CTutorialInfoTable.Inst.LoadTutorialInfos();
		CBlockInfoTable.Inst.LoadBlockInfos();
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE

		// 공용 앱 정보를 설정한다 {
		CCommonAppInfoStorage.Inst.StoreURL = Access.StoreURL;
		
#if LOCALIZE_TEST_ENABLE
		CCommonAppInfoStorage.Inst.SystemLanguage = m_eSystemLanguage;
#else
		CCommonAppInfoStorage.Inst.SystemLanguage = Application.systemLanguage;
#endif			// #if LOCALIZE_TEST_ENABLE
		// 공용 앱 정보를 설정한다 }
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
