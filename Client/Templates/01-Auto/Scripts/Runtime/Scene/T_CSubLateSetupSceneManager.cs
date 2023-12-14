#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCENE_TEMPLATES_MODULE_ENABLE
using System.Linq;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif // #if UNITY_ANDROID

namespace LateSetupScene {
	/** 서브 지연 설정 씬 관리자 */
	public partial class CSubLateSetupSceneManager : CLateSetupSceneManager {
		/** 콜백 */
		private enum ECallback {
			NONE = -1,
			SHOW_AGREE,
			SHOW_TRACKING_DESC,
			[HideInInspector] MAX_VAL
		}

		/** 팝업 콜백 */
		private enum EPopupCallback {
			NONE = -1,
			AGREE,
			TRACKING_DESC,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		[SerializeField] private EUserType m_eUserType = EUserType.NONE;
		private Dictionary<ECallback, System.Action<CPopup>> m_oCallbackDict = new Dictionary<ECallback, System.Action<CPopup>>();
		#endregion // 변수

		#region 프로퍼티
		public override bool IsAutoInitManager => true;
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
				CFunc.ShowLog($"Country Code: {CCommonAppInfoStorage.Inst.CountryCode}", KCDefine.B_LOG_COLOR_PLATFORM_INFO);
				CFunc.ShowLog($"System Language: {CCommonAppInfoStorage.Inst.SystemLanguage}", KCDefine.B_LOG_COLOR_PLATFORM_INFO);

#if UNITY_EDITOR
				// 유저 타입이 유효 할 경우
				if(m_eUserType.ExIsValid()) {
					CCommonUserInfoStorage.Inst.UserInfo.UserType = m_eUserType;
				} else {
					CCommonUserInfoStorage.Inst.UserInfo.UserType = CCommonUserInfoStorage.Inst.UserInfo.UserType.ExIsValid() ? CCommonUserInfoStorage.Inst.UserInfo.UserType : EUserType.A;
				}
#else
				// 유저 타입이 유효하지 않을 경우
				if(!CCommonUserInfoStorage.Inst.UserInfo.UserType.ExIsValid()) {
#if AB_TEST_ENABLE
					int nSumVal = CCommonAppInfoStorage.Inst.AppInfo.DeviceID.Sum((a_chLetter) => a_chLetter);
					CCommonUserInfoStorage.Inst.UserInfo.UserType = (nSumVal % KCDefine.B_VAL_2_INT <= KCDefine.B_VAL_0_INT) ? EUserType.A : EUserType.B;
#else
					CCommonUserInfoStorage.Inst.UserInfo.UserType = EUserType.A;
#endif // #if AB_TEST_ENABLE
				}
#endif // #if UNITY_EDITOR

#if UNITY_ANDROID && EXTERNAL_STORAGE_ENABLE
				this.UserPermissionList.ExAddVal(Permission.ExternalStorageRead);
				this.UserPermissionList.ExAddVal(Permission.ExternalStorageWrite);
#endif // #if UNITY_ANDROID && EXTERNAL_STORAGE_ENABLE

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				Func.SetupStrTable();
				CLateSetupSceneManager.SetLogDataDict(LogFunc.MakeDefDatas());
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

#if ADS_MODULE_ENABLE
#if(EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
				CLateSetupSceneManager.SetIsPurchaseRemoveAds(CUserInfoStorage.Inst.IsPurchaseRemoveAds);
#endif // #if(EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)

#if(!SAMPLE_PROJ && !EDITOR_DIST_BUILD && !CREATIVE_DIST_BUILD && !RESEARCH_MODULE_ENABLE)
				CLateSetupSceneManager.SetIsAutoLoadBannerAds(true);
				CLateSetupSceneManager.SetIsAutoLoadRewardAds(true);
				CLateSetupSceneManager.SetIsAutoLoadFullscreenAds(true);
#endif // #if(!SAMPLE_PROJ && !EDITOR_DIST_BUILD && !CREATIVE_DIST_BUILD && !RESEARCH_MODULE_ENABLE)
#endif // #if ADS_MODULE_ENABLE
			}
		}

		/** 씬을 설정한다 */
		protected override void Setup() {
			base.Setup();

#if ADS_MODULE_ENABLE && EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			CAdsManager.Inst.SetIsEnableBannerAds(!CUserInfoStorage.Inst.IsPurchaseRemoveAds);
			CAdsManager.Inst.SetIsEnableFullscreenAds(!CUserInfoStorage.Inst.IsPurchaseRemoveAds);
#endif // #if ADS_MODULE_ENABLE && EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}

		/** 한국 약관 동의 팝업을 출력한다 */
		protected override void ShowKRAgreePopup(string a_oPrivacy, string a_oServices, System.Action<CPopup> a_oCallback) {
			this.ShowAgreePopup(a_oPrivacy, a_oServices, EAgreePopup.KR, a_oCallback);
		}

		/** 유럽 연합 약관 동의 팝업을 출력한다 */
		protected override void ShowEUAgreePopup(string a_oPrivacyURL, string a_oServicesURL, System.Action<CPopup> a_oCallback) {
			this.ShowAgreePopup(a_oPrivacyURL, a_oServicesURL, EAgreePopup.EU, a_oCallback);
		}

		/** 추적 설명 팝업을 출력한다 */
		protected override void ShowTrackingDescPopup(System.Action<CPopup> a_oCallback) {
			m_oCallbackDict.ExReplaceVal(ECallback.SHOW_TRACKING_DESC, a_oCallback);
			var oTrackingDescPopup = CPopup.Create<CTrackingDescPopup>(KCDefine.LSS_OBJ_N_TRACKING_DESC_POPUP, KCDefine.LSS_OBJ_P_TRACKING_DESC_POPUP, this.PopupUIs);

			oTrackingDescPopup.Init(CTrackingDescPopup.MakeParams(new Dictionary<CTrackingDescPopup.ECallback, System.Action<CTrackingDescPopup>>() {
				[CTrackingDescPopup.ECallback.NEXT] = (a_oSender) => this.OnReceivePopupCallback(a_oSender, EPopupCallback.TRACKING_DESC)
			}));

			oTrackingDescPopup.Show(null, null);
		}

		/** 팝업 콜백을 수신했을 경우 */
		private void OnReceivePopupCallback(CPopup a_oSender, EPopupCallback a_eCallback) {
			a_oSender?.Close();

			switch(a_eCallback) {
				case EPopupCallback.AGREE: m_oCallbackDict.GetValueOrDefault(ECallback.SHOW_AGREE)?.Invoke(a_oSender); break;
				case EPopupCallback.TRACKING_DESC: m_oCallbackDict.GetValueOrDefault(ECallback.SHOW_TRACKING_DESC)?.Invoke(a_oSender); break;
			}
		}

		/** 약관 동의 팝업을 출력한다 */
		private void ShowAgreePopup(string a_oPrivacy, string a_oServices, EAgreePopup a_eAgreePopup, System.Action<CPopup> a_oCallback) {
#if GOOGLE_SHEET_ENABLE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE) && (DEBUG || DEVELOPMENT_BUILD)
			Func.SetupGoogleSheetInfoValCreators();
#endif // #if GOOGLE_SHEET_ENABLE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE) && (DEBUG || DEVELOPMENT_BUILD)

#if MODE_PORTRAIT_ENABLE
			string oObjPath = KCDefine.LSS_OBJ_P_PORTRAIT_AGREE_POPUP;
#else
			string oObjPath = KCDefine.LSS_OBJ_P_LANDSCAPE_AGREE_POPUP;
#endif // #if MODE_PORTRAIT_ENABLE

			m_oCallbackDict.ExReplaceVal(ECallback.SHOW_AGREE, a_oCallback);
			var oAgreePopup = CPopup.Create<CAgreePopup>(KCDefine.AS_OBJ_N_AGREE_POPUP, oObjPath, this.PopupUIs);

			oAgreePopup.Init(CAgreePopup.MakeParams(a_oPrivacy, a_oServices, a_eAgreePopup, new Dictionary<CAgreePopup.ECallback, System.Action<CAgreePopup>>() {
				[CAgreePopup.ECallback.AGREE] = (a_oSender) => this.OnReceivePopupCallback(a_oSender, EPopupCallback.AGREE)
			}));

			oAgreePopup.Show(null, null);
		}
		#endregion // 함수

		#region 조건부 함수
#if UNITY_ANDROID
		/** 유저 권한을 요청한다 */
		protected override void RequestUserPermission(string a_oPermission, System.Action<string, bool> a_oCallback) {
			this.ExRequestUserPermission(a_oPermission, a_oCallback, true);
		}
#endif // #if UNITY_ANDROID
		#endregion // 조건부 함수
	}
}
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
