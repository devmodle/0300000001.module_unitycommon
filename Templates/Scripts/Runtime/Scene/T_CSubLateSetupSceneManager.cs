using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if SCENE_TEMPLATES_MODULE_ENABLE
#if UNITY_ANDROID
using UnityEngine.Android;
#endif			// #if UNITY_ANDROID

/** 서브 지연 설정 씬 관리자 */
public class CSubLateSetupSceneManager : CLateSetupSceneManager {
	#region 추가 변수
	[SerializeField] private EUserType m_eUserType = EUserType.NONE;
	#endregion			// 추가 변수

	#region 추가 프로퍼티
	
	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		
		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			this.SetupAwake();
		}
	}

	/** 씬을 설정한다 */
	protected override void Setup() {
		base.Setup();
		
#if ADS_MODULE_ENABLE
		CAdsManager.Inst.IsEnableBannerAds = !CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds;
		CAdsManager.Inst.IsEnableFullscreenAds = !CCommonUserInfoStorage.Inst.UserInfo.IsRemoveAds;
#endif			// #if ADS_MODULE_ENABLE
	}

	/** 추적 설명 팝업을 출력한다 */
	protected override void ShowTrackingDescPopup() {
		// 추적 설명 팝업 출력이 가능 할 경우
		if(CCommonAppInfoStorage.Inst.AppInfo.IsEnableShowTrackingDescPopup) {
			var stCallbackParams = new CTrackingDescPopup.STCallbackParams() {
				m_oCallback = this.OnReceiveTrackingDescPopupResult
			};

			var oTrackingDescPopup = CPopup.Create<CTrackingDescPopup>(KCDefine.LSS_OBJ_N_TRACKING_DESC_POPUP, KCDefine.LSS_OBJ_P_TRACKING_DESC_POPUP, this.PopupUIs);
			oTrackingDescPopup.Init(stCallbackParams);
			oTrackingDescPopup.Show(null, null);
		} else {
			this.OnReceiveTrackingDescPopupResult(null);
		}
	}

	/** 씬을 설정한다 */
	private void SetupAwake() {
		this.IsAutoInitManager = true;

#if UNITY_EDITOR
		CCommonUserInfoStorage.Inst.UserInfo.UserType = m_eUserType.ExIsValid() ? m_eUserType : EUserType.A;
#else
		// 유저 타입이 유효하지 않을 경우
		if(!CCommonUserInfoStorage.Inst.UserInfo.UserType.ExIsValid()) {
#if AB_TEST_ENABLE
			int nSumVal = CCommonAppInfoStorage.Inst.AppInfo.DeviceID.Sum((a_chLetter) => a_chLetter);
			CCommonUserInfoStorage.Inst.UserInfo.UserType = (nSumVal % KCDefine.B_VAL_2_INT != KCDefine.B_VAL_0_INT) ? EUserType.A : EUserType.B;
#else
			CCommonUserInfoStorage.Inst.UserInfo.UserType = EUserType.A;
#endif			// #if AB_TEST_ENABLE
		}
#endif			// #if UNITY_EDITOR

#if UNITY_ANDROID
		m_oPermissionList.ExAddVal(Permission.ExternalStorageRead);
		m_oPermissionList.ExAddVal(Permission.ExternalStorageWrite);
#endif			// #if UNITY_ANDROID

#if ADS_MODULE_ENABLE && (!SAMPLE_PROJ && !CREATIVE_DIST_BUILD && !STUDY_MODULE_ENABLE)
		CLateSetupSceneManager.IsAutoLoadBannerAds = true;
		CLateSetupSceneManager.IsAutoLoadRewardAds = true;
		CLateSetupSceneManager.IsAutoLoadFullscreenAds = true;
#endif			// #if ADS_MODULE_ENABLE && (!SAMPLE_PROJ && !CREATIVE_DIST_BUILD && !STUDY_MODULE_ENABLE)

		CCommonAppInfoStorage.Inst.DeviceConfig = CDeviceInfoTable.Inst.DeviceConfig;
		CCommonUserInfoStorage.Inst.SaveUserInfo();
	}

	/** 추적 설명 팝업 결과를 수신했을 경우 */
	private void OnReceiveTrackingDescPopupResult(CTrackingDescPopup a_oSender) {
		a_oSender?.Close();
		this.ExLateCallFunc((a_oSender) => this.ShowConsentView(), KCDefine.U_DELAY_INIT);
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_ANDROID
	/** 권한을 요청한다 */
	protected override void RequestPermission(string a_oPermission, System.Action<string, bool> a_oCallback) {
		CFunc.RequestPermission(this, a_oPermission, a_oCallback);
	}
#endif			// #if UNITY_ANDROID
	#endregion			// 조건부 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
