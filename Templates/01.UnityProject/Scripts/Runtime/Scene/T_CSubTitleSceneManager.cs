using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
namespace TitleScene {
	/** 서브 타이틀 씬 관리자 */
	public partial class CSubTitleSceneManager : CTitleSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			IS_LOAD_EDITOR_SCENE,
			PLAY_BTN,
			LOGIN_BTN,
			APPLE_LOGIN_BTN,
			FACEBOOK_LOGIN_BTN,
			[HideInInspector] MAX_VAL
		}

#if DEBUG || DEVELOPMENT_BUILD
		/** 테스트 UI */
		[System.Serializable]
		private partial struct STTestUIs {
			// Do Something
		}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

		#region 변수
		private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
			[EKey.IS_LOAD_EDITOR_SCENE] = false
		};

		/** =====> UI <===== */
		private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

#if DEBUG || DEVELOPMENT_BUILD
		[SerializeField] private STTestUIs m_stTestUIs;
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		#endregion			// 변수

		#region 함수
		/** 내비게이션 스택 이벤트를 수신했을 경우 */
		public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
			base.OnReceiveNavStackEvent(a_eEvent);

			// 백 키 눌림 이벤트 일 경우
			if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
				Func.ShowQuitPopup(this.OnReceiveQuitPopupResult);
			}
		}

		/** 최초 시작 상태를 갱신한다 */
		private void UpdateFirstStartState() {
			LogFunc.SendLaunchLog();
			LogFunc.SendSplashLog();
			
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			CCommonAppInfoStorage.Inst.IsFirstStart = false;
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
			
#if (!UNITY_EDITOR && UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			m_oBoolDict[EKey.IS_LOAD_EDITOR_SCENE] = true;
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
#endif			// #if (!UNITY_EDITOR && UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		}

		/** 최초 플레이 상태를 갱신한다 */
		private void UpdateFirstPlayState() {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			CCommonAppInfoStorage.Inst.AppInfo.IsFirstPlay = false;
			CCommonAppInfoStorage.Inst.SaveAppInfo();
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

			// 약관 동의 팝업이 닫혔을 경우
			if(CAppInfoStorage.Inst.IsCloseAgreePopup) {
				LogFunc.SendAgreeLog();
			}
		}

		/** 종료 팝업 결과를 수신했을 경우 */
		private void OnReceiveQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				a_oSender.IsIgnoreAni = true;
				this.ExLateCallFunc((a_oSender) => this.QuitApp());
			}
		}

		/** 업데이트 팝업 결과를 수신했을 경우 */
		private void OnReceiveUpdatePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				Application.OpenURL(Access.StoreURL);
			}
		}

		/** 플레이 버튼을 눌렀을 경우 */
		private void OnTouchPlayBtn() {
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
		}

		/** 로그인 버튼을 눌렀을 경우 */
		private void OnTouchLoginBtn() {
			// Do Something
		}

		/** 애플 로그인 버튼을 눌렀을 경우 */
		private void OnTouchAppleLoginBtn() {
			// Do Something
		}

		/** 페이스 북 로그인 버튼을 눌렀을 경우 */
		private void OnTouchFacebookLoginBtn() {
			// Do Something
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
