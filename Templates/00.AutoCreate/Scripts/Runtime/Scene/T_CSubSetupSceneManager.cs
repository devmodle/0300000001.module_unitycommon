#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			// 저장소를 로드한다
			CAppInfoStorage.Inst.LoadAppInfo();
			CUserInfoStorage.Inst.LoadUserInfo();
			CGameInfoStorage.Inst.LoadGameInfo();
			
			// 테이블을 로드한다
			CLevelInfoTable.Inst.LoadLevelInfos();

			CEtcInfoTable.Inst.LoadEtcInfos();
			CMissionInfoTable.Inst.LoadMissionInfos();
			CRewardInfoTable.Inst.LoadRewardInfos();
			CEpisodeInfoTable.Inst.LoadEpisodeInfos();
			CTutorialInfoTable.Inst.LoadTutorialInfos();
			CResInfoTable.Inst.LoadResInfos();

			CItemInfoTable.Inst.LoadItemInfos();
			CSkillInfoTable.Inst.LoadSkillInfos();
			CObjInfoTable.Inst.LoadObjInfos();
			CFXInfoTable.Inst.LoadFXInfos();
			CAbilityInfoTable.Inst.LoadAbilityInfos();
			CProductTradeInfoTable.Inst.LoadProductTradeInfos();

			// 기본 캐릭터 유저 정보가 없을 경우
			if(!CUserInfoStorage.Inst.TryGetCharacterUserInfo(KDefine.G_ID_COMMON_CHARACTER, out CCharacterUserInfo oCharacterUserInfo)) {
				CUserInfoStorage.Inst.AddCharacterUserInfo(KDefine.G_ID_COMMON_CHARACTER, Factory.MakeCharacterUserInfo(EObjKinds.PLAYABLE_COMMON_CHARACTER_01, STIdxInfo.INVALID));
				CUserInfoStorage.Inst.SaveUserInfo();
			}

			// 공용 앱 정보를 설정한다 {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			CCommonAppInfoStorage.Inst.StoreURL = Access.StoreURL;
			
#if LOCALIZE_TEST_ENABLE
			CCommonAppInfoStorage.Inst.SystemLanguage = m_eSystemLanguage;
#else
			CCommonAppInfoStorage.Inst.SystemLanguage = Application.systemLanguage;
#endif			// #if LOCALIZE_TEST_ENABLE
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
			// 공용 앱 정보를 설정한다 }
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}
		#endregion			// 함수
	}
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
