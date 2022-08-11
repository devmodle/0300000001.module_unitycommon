#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace TitleScene {
	/** 서브 타이틀 씬 관리자 */
	public partial class CSubTitleSceneManager : CTitleSceneManager {
		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.AwakeSetup();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.StartSetup();
				this.UpdateUIsState();

				Func.PlayBGSnd(EResKinds.SND_BG_SCENE_TITLE_01);

				// 로그인 되었을 경우
				if(CUserInfoStorage.Inst.UserInfo.LoginType != ELoginType.NONE) {
					this.OnLogin(CUserInfoStorage.Inst.UserInfo.LoginType, true);
				}

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

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					foreach(var stKeyVal in m_oAniDict) {
						stKeyVal.Value?.Kill();
					}
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CSubGameSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 씬을 설정한다 */
		private void AwakeSetup() {
			// 텍스트를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
				(EKey.TOUCH_TEXT, $"{EKey.TOUCH_TEXT}", this.UIsBase)
			}, m_oTextDict, false);

			m_oTextDict.GetValueOrDefault(EKey.TOUCH_TEXT)?.gameObject.SetActive(false);
			// 텍스트를 설정한다 }

			// 버튼을 설정한다
			CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
				(EKey.PLAY_BTN, $"{EKey.PLAY_BTN}", this.UIsBase, this.OnTouchPlayBtn),
				(EKey.GUEST_LOGIN_BTN, $"{EKey.GUEST_LOGIN_BTN}", this.UIsBase, this.OnTouchGuestLoginBtn),
				(EKey.APPLE_LOGIN_BTN, $"{EKey.APPLE_LOGIN_BTN}", this.UIsBase, this.OnTouchAppleLoginBtn),
				(EKey.FACEBOOK_LOGIN_BTN, $"{EKey.FACEBOOK_LOGIN_BTN}", this.UIsBase, this.OnTouchFacebookLoginBtn)
			}, m_oBtnDict, false);

			#region 추가
			this.SubAwakeSetup();
			#endregion			// 추가
		}

		/** 씬을 설정한다 */
		private void StartSetup() {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			// 업데이트가 가능 할 경우
			if(!CAppInfoStorage.Inst.IsIgnoreUpdate && CCommonAppInfoStorage.Inst.IsEnableUpdate()) {
				CAppInfoStorage.Inst.IsIgnoreUpdate = true;
				this.ExLateCallFunc((a_oSender) => Func.ShowUpdatePopup(this.OnReceiveUpdatePopupResult));
			}
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE

			#region 추가
			this.SubStartSetup();
			#endregion			// 추가
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			// 버튼을 갱신한다 {
#if UNITY_IOS && APPLE_LOGIN_ENABLE
			m_oBtnDict.GetValueOrDefault(EKey.APPLE_LOGIN_BTN)?.gameObject.SetActive(true);
#else
			m_oBtnDict.GetValueOrDefault(EKey.APPLE_LOGIN_BTN)?.gameObject.SetActive(false);
#endif			// #if UNITY_IOS && APPLE_LOGIN_ENABLE

			for(int i = 0; i < BTN_KEY_LIST.Count; ++i) {
				// 로그인 되었을 경우
				if(CUserInfoStorage.Inst.UserInfo.LoginType != ELoginType.NONE) {
					m_oBtnDict.GetValueOrDefault(BTN_KEY_LIST[i])?.gameObject.SetActive(false);
				}
			}
			// 버튼을 갱신한다 }

			#region 추가
			this.SubUpdateUIsState();
			#endregion			// 추가
		}
		#endregion			// 함수
	}

	/** 서브 타이틀 씬 관리자 - 서브 */
	public partial class CSubTitleSceneManager : CTitleSceneManager {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

#if DEBUG || DEVELOPMENT_BUILD
		/** 서브 테스트 UI */
		[System.Serializable]
		private partial struct STSubTestUIs {
			// Do Something
		}
#endif			// #if DEBUG || DEVELOPMENT_BUILD

		#region 변수
		/** =====> UI <===== */
#if DEBUG || DEVELOPMENT_BUILD
		[SerializeField] private STSubTestUIs m_stSubTestUIs;
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		#endregion			// 변수

		#region 프로퍼티

		#endregion			// 프로퍼티

		#region 함수
		/** 씬을 설정한다 */
		private void SubAwakeSetup() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SetupSubTestUIs();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 씬을 설정한다 */
		private void SubStartSetup() {
			// Do Something
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
			this.UpdateSubTestUIsState();
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 터치 모드가 아닐 경우
			if(!m_oBoolDict.GetValueOrDefault(EKey.IS_TOUCH) && CUserInfoStorage.Inst.UserInfo.LoginType != ELoginType.NONE) {
				m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, true);
			}
		}
		#endregion			// 함수

		#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
		/** 서브 테스트 UI 를 설정한다 */
		private void SetupSubTestUIs() {
			// Do Something
		}

		/** 서브 테스트 UI 상태를 갱신한다 */
		private void UpdateSubTestUIsState() {
			// Do Something
		}
#endif			// #if DEBUG || DEVELOPMENT_BUILD
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
