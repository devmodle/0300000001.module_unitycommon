using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 동기화 팝업
public class CSyncPopup : CSubPopup {
	#region 변수
	private bool m_bIsLoadUserInfo = false;

	// =====> 객체 <=====
	private GameObject m_oLoginUIs = null;
	private GameObject m_oLogoutUIs = null;
	#endregion			// 변수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		m_oLoginUIs = m_oContents.ExFindChild(KCDefine.U_OBJ_N_LOGIN_UIS);
		m_oLogoutUIs = m_oContents.ExFindChild(KCDefine.U_OBJ_N_LOGOUT_UIS);

		// 버튼을 설정한다 {
		var oLoginBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_LOGIN_BTN);
		oLoginBtn?.onClick.AddListener(this.OnTouchLoginBtn);

		var oLogoutBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_LOGOUT_BTN);
		oLogoutBtn?.onClick.AddListener(this.OnTouchLogoutBtn);

		var oLoadBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_LOAD_BTN);
		oLoadBtn?.onClick.AddListener(this.OnTouchLoadBtn);

		var oSaveBtn = m_oContents.ExFindComponent<Button>(KCDefine.U_OBJ_N_SAVE_BTN);
		oSaveBtn?.onClick.AddListener(this.OnTouchSaveBtn);
		// 버튼을 설정한다 }
	}

	//! 초기화
	public override void Init() {
		base.Init();
	}

	//! 팝업 컨텐츠를 설정한다
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	//! UI 상태를 갱신한다
	private new void UpdateUIsState() {
		base.UpdateUIsState();
		
#if FIREBASE_MODULE_ENABLE
		m_oLoginUIs?.SetActive(CFirebaseManager.Inst.IsLogin);
		m_oLogoutUIs?.SetActive(!CFirebaseManager.Inst.IsLogin);
#endif			// #if FIREBASE_MODULE_ENABLE
	}

	//! 로그인 버튼을 눌렀을 경우
	private void OnTouchLoginBtn() {
#if FIREBASE_MODULE_ENABLE
		Func.FirebaseLogin(this.OnLogin);
#endif			// #if FIREBASE_MODULE_ENABLE
	}

	//! 로그아웃 버튼을 눌렀을 경우
	private void OnTouchLogoutBtn() {
#if FIREBASE_MODULE_ENABLE
		Func.FirebaseLogout(this.OnLogout);
#endif			// #if FIREBASE_MODULE_ENABLE
	}

	//! 로드 버튼을 눌렀을 경우
	private void OnTouchLoadBtn() {
		Func.ShowLoadPopup((a_oSender, a_bIsOK) => {
#if FIREBASE_MODULE_ENABLE
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				a_oSender.IsIgnoreAni = true;
				Func.LoadUserInfo(this.OnLoadUserInfo);
			}
#endif			// #if FIREBASE_MODULE_ENABLE
		});
	}

	//! 저장 버튼을 눌렀을 경우
	private void OnTouchSaveBtn() {
		Func.ShowSavePopup((a_oSender, a_bIsOK) => {
#if FIREBASE_MODULE_ENABLE
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				a_oSender.IsIgnoreAni = true;
				Func.SaveUserInfo(this.OnSaveUserInfo);
			}
#endif			// #if FIREBASE_MODULE_ENABLE
		});
	}
	#endregion			// 함수

	#region 조건부 함수
#if FIREBASE_MODULE_ENABLE
	//! 로그인 되었을 경우
	private void OnLogin(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		// 로그인 되었을 경우
		if(a_bIsSuccess) {
			// Do Something
		}

		this.UpdateUIsState();
	}

	//! 로그아웃 되었을 경우
	private void OnLogout(CFirebaseManager a_oSender) {
		this.UpdateUIsState();
	}

	//! 유저 정보가 저장 되었을 경우
	private void OnSaveUserInfo(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		// 저장 되었을 경우
		if(a_bIsSuccess) {
			// Do Something
		}

		this.UpdateUIsState();
		Func.OnSaveUserInfo(a_oSender, a_bIsSuccess, null);
	}

	//! 유저 정보가 로드 되었을 경우
	private void OnLoadUserInfo(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		// 로드 되었을 경우
		if(a_bIsSuccess && a_oJSONStr.ExIsValid()) {
			var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr);

			string oUserInfoStr = oJSONNode[KCDefine.B_KEY_JSON_USER_INFO_DATA];
			string oGameInfoStr = oJSONNode[KCDefine.B_KEY_JSON_GAME_INFO_DATA];
			string oCommonUserInfoStr = oJSONNode[KCDefine.B_KEY_JSON_COMMON_USER_INFO_DATA];

			CUserInfoStorage.Inst.ResetUserInfo(oUserInfoStr);
			CUserInfoStorage.Inst.SaveUserInfo();

			CGameInfoStorage.Inst.ResetGameInfo(oGameInfoStr);
			CGameInfoStorage.Inst.SaveGameInfo();

			CCommonUserInfoStorage.Inst.ResetUserInfo(oCommonUserInfoStr);
			CCommonUserInfoStorage.Inst.SaveUserInfo();

#if ADS_MODULE_ENABLE
			// 광고 제거 모드 일 경우
			if(CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds) {
				CAdsManager.Inst.CloseBannerAds(CPluginInfoTable.Inst.DefAdsType, true);

				CAdsManager.Inst.IsEnableBannerAds = false;
				CAdsManager.Inst.IsEnableFullscreenAds = false;
			}
#endif			// #if ADS_MODULE_ENABLE
		}

		m_bIsLoadUserInfo = a_bIsSuccess && a_oJSONStr.ExIsValid();

		Func.OnLoadUserInfo(a_oSender, a_oJSONStr, m_bIsLoadUserInfo, this.OnReceiveLoadSuccessPopupResult);
		CSceneManager.ScreenPopupUIs.ExEnumerateComponents<CAlertPopup>((a_oPopupSender) => { a_oPopupSender.IsIgnoreNavStackEvent = m_bIsLoadUserInfo; return true; });
	}

	//! 로드 성공 팝업 결과를 수신했을 경우
	private void OnReceiveLoadSuccessPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 유저 정보를 로드했을 경우
		if(a_bIsOK && m_bIsLoadUserInfo) {
			this.ExLateCallFunc((a_oSender, a_oParams) => { 
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
				
				CScheduleManager.Inst.Reset();
				CNavStackManager.Inst.Reset();
			});
		}
	}
#endif			// #if FIREBASE_MODULE_ENABLE
	#endregion			// 조건부 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
