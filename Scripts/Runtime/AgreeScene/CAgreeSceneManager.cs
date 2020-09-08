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
	#endregion			// 추상 함수

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();
		StartCoroutine(this.OnStart());
	}

	//! 다음 씬을 로드한다
	protected void LoadNextScene() {
		CFunc.BroadcastMsg(KCDefine.AS_FUNC_NAME_AGREE_SCENE_MANAGER_EVENT, 
			EAgreeSceneManagerEventType.LOAD_NEXT_SCENE);

#if MSG_PACK_ENABLE
		CCommonUserInfoStorage.Instance.UserInfo.IsAgree = true;
		CCommonUserInfoStorage.Instance.SaveUserInfo();
#endif			// #if MSG_PACK_ENABLE

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
			string oServiceFilepath = KCDefine.AS_DATA_PATH_KOREAN_SERVICE_TEXT;
			string oPersonalFilepath = KCDefine.AS_DATA_PATH_KOREAN_PERSONAL_TEXT;

#if MSG_PACK_ENABLE
			// 대한민국이 아닐 경우
			if(!CCommonAppInfoStorage.Instance.CountryCode.ExIsEquals(KCDefine.B_KOREA_COUNTRY_CODE)) {
				oServiceFilepath = KCDefine.AS_DATA_PATH_ENGLISH_SERVICE_TEXT;
				oPersonalFilepath = KCDefine.AS_DATA_PATH_ENGLISH_PERSONAL_TEXT;
			}
#endif			// #if MSG_PACK_ENABLE

			CFunc.BroadcastMsg(KCDefine.AS_FUNC_NAME_AGREE_SCENE_MANAGER_EVENT, 
				EAgreeSceneManagerEventType.SHOW_AGREE_POPUP);
			
			this.ShowAgreePopup(CResManager.Instance.GetTextAsset(oServiceFilepath).text,
				CResManager.Instance.GetTextAsset(oPersonalFilepath).text);

			CFunc.LateCallFunc(this, (a_oComponent, a_oParams) => {
				CResManager.Instance.RemoveTextAsset(oServiceFilepath, true);
				CResManager.Instance.RemoveTextAsset(oPersonalFilepath, true);
			});
		}
#endif			// #if ROBO_TEST_ENABLE
	}
	#endregion			// 함수
}
