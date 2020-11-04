using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 약관 동의 씬 관리자
public abstract class CAgreeSceneManager : CSceneManager {
	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_NAME_AGREE;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_ORDER_AGREE_SCENE_MANAGER;
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
		StartCoroutine(this.OnStart());
	}

	//! 다음 씬을 로드한다
	protected void LoadNextScene() {
		CCommonUserInfoStorage.Instance.UserInfo.IsAgree = true;
		CCommonUserInfoStorage.Instance.SaveUserInfo();

		CFunc.BroadcastMsg(KCDefine.SS_FUNC_NAME_START_SCENE_EVENT, 
			EStartSceneEvent.LOAD_LATE_SETUP_SCENE);

		CSceneLoader.Instance.LoadAdditiveScene(KCDefine.B_SCENE_NAME_LATE_SETUP);
	}

	//! 초기화
	private IEnumerator OnStart() {
		CAccess.Assert(CSceneManager.IsInit);

		CSceneLoader.Instance.UnloadSceneAsync(KCDefine.B_SCENE_NAME_SETUP, null);
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

		this.SetupRootScene();
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

#if ROBO_TEST_ENABLE
		this.LoadNextScene();
#else
		bool bIsAgree = CCommonUserInfoStorage.Instance.UserInfo.IsAgree ||
			!CCommonAppInfoStorage.Instance.IsNeedAgreement(CCommonAppInfoStorage.Instance.CountryCode);
			
		// 약관 동의 상태 일 경우
		if(bIsAgree) {
			this.LoadNextScene();
		} else {
			// 한국 일 경우
			if(CCommonAppInfoStorage.Instance.CountryCode.ExIsEquals(KCDefine.B_KOREA_COUNTRY_CODE)) {
				var oServices = CResManager.Instance.GetTextAsset(KCDefine.AS_DATA_PATH_SERVICES);
				var oPrivacy = CResManager.Instance.GetTextAsset(KCDefine.AS_DATA_PATH_PRIVACY);

				CAccess.Assert(oServices.ExIsValid() && oPrivacy.ExIsValid());
				this.ShowAgreePopup(oServices.text, oPrivacy.text);

				CResManager.Instance.RemoveTextAsset(KCDefine.AS_DATA_PATH_SERVICES, true);
				CResManager.Instance.RemoveTextAsset(KCDefine.AS_DATA_PATH_PRIVACY, true);

			} else {
				this.ShowEUAgreePopup(CProjInfoTable.Instance.ServicesURL, 
					CProjInfoTable.Instance.PrivacyURL);
			}			
		}
#endif			// #if ROBO_TEST_ENABLE
	}
	#endregion			// 함수
}
