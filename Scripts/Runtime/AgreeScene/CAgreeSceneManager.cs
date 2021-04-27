using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 약관 동의 씬 관리자
public abstract class CAgreeSceneManager : CSceneManager {
	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_AGREE;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_O_AGREE_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 추상 함수
	//! 일반 약관 동의 팝업을 출력한다
	protected abstract void ShowNormAgreePopup(string a_oServices, string a_oPrivacy);

	//! 유럽 연합 약관 동의 팝업을 출력한다
	protected abstract void ShowEUAgreePopup(string a_oServicesURL, string a_oPrivacyURL);
	#endregion			// 추상 함수

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			StartCoroutine(this.OnStart());
		}
	}

	//! 다음 씬을 로드한다
	protected void LoadNextScene() {
		CCommonAppInfoStorage.Inst.AppInfo.IsAgree = true;
		CCommonAppInfoStorage.Inst.SaveAppInfo();

		CFunc.BroadcastMsg(KCDefine.SS_FUNC_N_START_SCENE_EVENT, EStartSceneEvent.LOAD_LATE_SETUP_SCENE);
		CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_LATE_SETUP);
	}

	//! 초기화
	private IEnumerator OnStart() {
		CSceneLoader.Inst.UnloadSceneAsync(KCDefine.B_SCENE_N_SETUP, null);
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);

		this.SetupRootScene();
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);

#if ROBO_TEST_ENABLE
		this.LoadNextScene();
#else
		bool bIsAgree = CCommonAppInfoStorage.Inst.AppInfo.IsAgree;
			
		// 약관 동의 상태 일 경우
		if(bIsAgree || !CCommonAppInfoStorage.Inst.IsNeedAgree(CCommonAppInfoStorage.Inst.CountryCode)) {
			this.LoadNextScene();
		} else {
			// 한국 일 경우
			if(CCommonAppInfoStorage.Inst.CountryCode.ExIsEquals(KCDefine.B_KOREA_COUNTRY_CODE)) {
				var oServices = CResManager.Inst.GetRes<TextAsset>(KCDefine.AS_DATA_P_SERVICES);
				var oPrivacy = CResManager.Inst.GetRes<TextAsset>(KCDefine.AS_DATA_P_PRIVACY);
				
				this.ShowNormAgreePopup(oServices.text, oPrivacy.text);

				CResManager.Inst.RemoveRes<TextAsset>(KCDefine.AS_DATA_P_SERVICES, true);
				CResManager.Inst.RemoveRes<TextAsset>(KCDefine.AS_DATA_P_PRIVACY, true);
			} else {
				this.ShowEUAgreePopup(CProjInfoTable.Inst.ServicesURL, CProjInfoTable.Inst.PrivacyURL);
			}			
		}
#endif			// #if ROBO_TEST_ENABLE
	}
	#endregion			// 함수
}
