#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;

using EnhancedUI.EnhancedScroller;

namespace MainScene {
	/** 서브 메인 씬 관리자 - 서브 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

#if DEBUG || DEVELOPMENT_BUILD
		/** 서브 테스트 UI */
		[System.Serializable]
		private struct STSubTestUIs {
			// Do Something
		}
#endif // #if DEBUG || DEVELOPMENT_BUILD

		#region 변수
#if DEBUG || DEVELOPMENT_BUILD
		[Header("=====> UIs <=====")]
		[SerializeField] private STSubTestUIs m_stSubTestUIs;
#endif // #if DEBUG || DEVELOPMENT_BUILD
		#endregion // 변수

		#region 프로퍼티

		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		private void SubAwake() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SubSetupTestUIs();
#endif // #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 초기화 */
		private void SubStart() {
			// Do Something
		}

		/** 제거 되었을 경우 */
		private void SubOnDestroy() {
			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CSubMainSceneManager.SubOnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fDeltaTime) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SubUpdateTestUIsState();
#endif // #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.Objs, this.ScreenSize);
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.Objs, this.ScreenSize);
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.Objs, this.ScreenSize);
		}
		#endregion // 함수

		#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
		/** 테스트 UI 를 설정한다 */
		private void SubSetupTestUIs() {
			// Do Something
		}

		/** 테스트 UI 상태를 갱신한다 */
		private void SubUpdateTestUIsState() {
			// Do Something
		}
#endif // #if DEBUG || DEVELOPMENT_BUILD
		#endregion // 조건부 함수
	}

	/** 서브 메인 씬 관리자 - 스크롤러 셀 뷰 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
		#region 함수
		/** 선택 콜백을 수신했을 경우 */
		private void OnReceiveSelCallback(CScrollerCellView a_oSender, ulong a_nID) {
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
#if NEVER_USE_THIS
			// 레벨 스크롤러 일 경우
			if(a_oSender.Params.m_oScroller == m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScroller) {
				Func.ShowReadyPopup(this.PopupUIs, (a_oSender) => {
					(a_oSender as CReadyPopup).Init(CReadyPopup.MakeParams(a_nID.ExULevelIDToIDInfo(), new Dictionary<CReadyPopup.ECallback, System.Action<CReadyPopup>>() {
						[CReadyPopup.ECallback.PLAY] = this.OnReceiveReadyPopupCallback
					}));
				});
			}
#endif // #if NEVER_USE_THIS
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
