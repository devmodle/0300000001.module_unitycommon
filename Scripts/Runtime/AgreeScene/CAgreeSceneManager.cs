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
	//! 약관 동의 팝업을 출력한다
	protected abstract void ShowAgreePopup(string a_oServiceString, string a_oPersonalString);

	//! 유럽 연합 약관 동의 팝업을 출력한다
	protected abstract void ShowEuropeanUnionAgreePopup(string a_oServiceURL, string a_oPersonalURL);
	#endregion			// 추상 함수

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();
		StartCoroutine(this.OnStart());
	}

	//! 다음 씬을 로드한다
	protected void LoadNextScene() {
#if MSG_PACK_ENABLE
		CCommonUserInfoStorage.Instance.UserInfo.IsAgree = true;
		CCommonUserInfoStorage.Instance.SaveUserInfo();
#endif			// #if MSG_PACK_ENABLE

		CFunc.BroadcastMsg(KCDefine.SS_FUNC_NAME_START_SCENE_EVENT, 
			EStartSceneEvent.LOAD_LATE_SETUP_SCENE);

		CSceneLoader.Instance.LoadAdditiveScene(KCDefine.B_SCENE_NAME_LATE_SETUP);
	}

	//! 초기화
	private IEnumerator OnStart() {
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

		this.SetupRootScene();
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

#if ROBO_TEST_ENABLE
		this.LoadNextScene();
#else
		bool bIsAgree = false;

#if MSG_PACK_ENABLE
		bIsAgree = CCommonUserInfoStorage.Instance.UserInfo.IsAgree ||
			!CCommonAppInfoStorage.Instance.IsNeedAgreement(CCommonAppInfoStorage.Instance.CountryCode);
#endif			// #if MSG_PACK_ENABLE

		// 약관 동의 상태 일 경우
		if(bIsAgree) {
			this.LoadNextScene();
		} else {
			// 한국 일 경우
			if(CCommonAppInfoStorage.Instance.CountryCode.ExIsEquals(KCDefine.B_KOREA_COUNTRY_CODE)) {
				this.ShowAgreePopup(CResManager.Instance.GetTextAsset(KCDefine.AS_DATA_PATH_SERVICE_TEXT).text,
					CResManager.Instance.GetTextAsset(KCDefine.AS_DATA_PATH_PERSONAL_TEXT).text);

				CResManager.Instance.RemoveTextAsset(KCDefine.AS_DATA_PATH_SERVICE_TEXT, true);
				CResManager.Instance.RemoveTextAsset(KCDefine.AS_DATA_PATH_PERSONAL_TEXT, true);

			} else {
				this.ShowEuropeanUnionAgreePopup(CProjInfoTable.Instance.ServiceURL, 
					CProjInfoTable.Instance.PersonalURL);
			}			
		}
#endif			// #if ROBO_TEST_ENABLE
	}
	#endregion			// 함수
}
