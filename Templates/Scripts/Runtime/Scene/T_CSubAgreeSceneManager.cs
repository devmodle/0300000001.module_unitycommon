using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if SCENE_TEMPLATES_MODULE_ENABLE
namespace AgreeScene {
	/** 서브 약관 동의 씬 관리자 */
	public partial class CSubAgreeSceneManager : CAgreeSceneManager {
		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
				CFunc.ShowLog($"Language: {CCommonAppInfoStorage.Inst.SystemLanguage}", KCDefine.B_LOG_COLOR_PLATFORM_INFO);
				CFunc.ShowLog($"Country Code: {CCommonAppInfoStorage.Inst.CountryCode}", KCDefine.B_LOG_COLOR_PLATFORM_INFO);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE && (DEBUG || DEVELOPMENT_BUILD)

				this.SetupAwake();
			}
		}

		/** 한국 약관 동의 팝업을 출력한다 */
		protected override void ShowKRAgreePopup(string a_oPrivacy, string a_oServices) {
			this.ShowAgreePopup(a_oPrivacy, a_oServices, EAgreePopup.KR);
		}

		/** 유럽 연합 약관 동의 팝업을 출력한다 */
		protected override void ShowEUAgreePopup(string a_oPrivacyURL, string a_oServicesURL) {
			this.ShowAgreePopup(a_oPrivacyURL, a_oServicesURL, EAgreePopup.EU);
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			Func.SetupLocalizeStrs();
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
		}

		/** 약관 동의 팝업을 출력한다 */
		private void ShowAgreePopup(string a_oPrivacy, string a_oServices, EAgreePopup a_eAgreePopup) {
#if MODE_PORTRAIT_ENABLE
			string oObjPath = KCDefine.AS_OBJ_P_PORTRAIT_AGREE_POPUP;
#else
			string oObjPath = KCDefine.AS_OBJ_P_LANDSCAPE_AGREE_POPUP;
#endif			// #if MODE_PORTRAIT_ENABLE

			var stParams = new CAgreePopup.STParams() {
				m_oPrivacy = a_oPrivacy, m_oServices = a_oServices, m_eAgreePopup = a_eAgreePopup
			};

			var oAgreePopup = CPopup.Create<CAgreePopup>(KCDefine.AS_OBJ_N_AGREE_POPUP, oObjPath, this.PopupUIs);
			oAgreePopup.Init(stParams);
			oAgreePopup.Show(null, this.OnCloseAgreePopup);
		}

		/** 약관 동의 팝업이 닫혔을 경우 */
		private void OnCloseAgreePopup(CPopup a_oSender) {
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
			CAppInfoStorage.Inst.IsCloseAgreePopup = true;
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE

			this.LoadNextScene();
		}
		#endregion			// 함수
	}
}
#endif			// #if SCENE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
