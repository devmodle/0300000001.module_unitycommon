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
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupAwake();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupStart();
				this.UpdateUIsState();

				Func.PlayBGSnd(EResKinds.SND_BG_SCENE_TITLE);

#if NEWTON_SOFT_JSON_MODULE_ENABLE
				// 최초 시작 일 경우
				if(CCommonAppInfoStorage.Inst.IsFirstStart) {
					this.UpdateFirstStartState();
				}

				// 최초 플레이 일 경우
				if(CCommonAppInfoStorage.Inst.AppInfo.IsFirstPlay) {
					this.UpdateFirstPlayState();
				}
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
			}
		}

		/** 내비게이션 스택 이벤트를 수신했을 경우 */
		public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
			base.OnReceiveNavStackEvent(a_eEvent);

			// 백 키 눌림 이벤트 일 경우
			if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
				Func.ShowQuitPopup(this.OnReceiveQuitPopupResult);
			}
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			// 버튼을 설정한다
			CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
				(EKey.PLAY_BTN, $"{EKey.PLAY_BTN}", this.UIsBase, this.OnTouchPlayBtn),
				(EKey.LOGIN_BTN, $"{EKey.LOGIN_BTN}", this.UIsBase, this.OnTouchLoginBtn),
				(EKey.APPLE_LOGIN_BTN, $"{EKey.APPLE_LOGIN_BTN}", this.UIsBase, this.OnTouchAppleLoginBtn),
				(EKey.FACEBOOK_LOGIN_BTN, $"{EKey.FACEBOOK_LOGIN_BTN}", this.UIsBase, this.OnTouchFacebookLoginBtn)
			}, m_oBtnDict, false);

#if DEBUG || DEVELOPMENT_BUILD
			this.SetupTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 씬을 설정한다 */
		private void SetupStart() {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			// 업데이트가 가능 할 경우
			if(!CAppInfoStorage.Inst.IsIgnoreUpdate && CCommonAppInfoStorage.Inst.IsEnableUpdate()) {
				CAppInfoStorage.Inst.IsIgnoreUpdate = true;
				this.ExLateCallFunc((a_oSender) => Func.ShowUpdatePopup(this.OnReceiveUpdatePopupResult));
			}
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
			this.UpdateTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
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

		#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
		/** 테스트 UI 를 설정한다 */
		private void SetupTestUIs() {
			// Do Something
		}

		/** 테스트 UI 상태를 갱신한다 */
		private void UpdateTestUIsState() {
			// Do Something
		}
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
