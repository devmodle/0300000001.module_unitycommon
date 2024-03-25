#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;

namespace PlayScene {
	/** 서브 플레이 씬 관리자 - 서브 */
	public partial class CSubPlaySceneManager : CPlaySceneManager {
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

		/** 제거되었을 경우 */
		private void SubOnDestroy() {
			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsRunningApp) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CSubPlaySceneManager.SubOnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fDeltaTime) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsRunningApp) {
				// Do Something
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnLateUpdate(float a_fDeltaTime) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsRunningApp) {
				// Do Something
			}
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SubUpdateTestUIsState();
#endif // #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 획득 콜백을 수신했을 경우 */
		private void OnReceiveAcquireCallback(NSEngine.CEngine a_oSender, Dictionary<ulong, STTargetInfo> a_oAcquireTargetInfoDict) {
			this.SetIsDirtySaveInfo(true);
			this.SetIsDirtyUpdateState(true);
		}

		/** 엔진 객체 이벤트 콜백을 수신했을 경우 */
		private void OnReceiveEObjEventCallback(NSEngine.CEngine a_oSender, NSEngine.CEObjComponent a_oEObjComponent, NSEngine.EEngineObjEvent a_eEvent, string a_oParams) {
			// Do Something
		}

		/** 이전 팝업 콜백을 처리한다 */
		private void HandlePrevPopupCallback(CPopup a_oPopup) {
			this.LoadLevel(a_oPopup, Access.GetPrevLevelEpisodeInfo(CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03));
		}

		/** 다음 팝업 콜백을 처리한다 */
		private void HandleNextPopupCallback(CPopup a_oPopup) {
			this.LoadLevel(a_oPopup, Access.GetNextLevelEpisodeInfo(CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03));
		}

		/** 재시도 팝업 콜백을 처리한다 */
		private void HandleRetryPopupCallback(CPopup a_oPopup) {
#if ADS_MODULE_ENABLE
			Func.ShowFullscreenAds(null, KCDefine.B_VAL_0_1_REAL);
#endif // #if ADS_MODULE_ENABLE

			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_PLAY);
		}

		/** 재개 팝업 콜백을 처리한다 */
		private void HandleResumePopupCallback(CPopup a_oPopup) {
			a_oPopup?.Close();
		}

		/** 이어하기 팝업 콜백을 처리한다 */
		private void HandleContinuePopupCallback(CPopup a_oPopup) {
			var oContinuePopup = a_oPopup as CContinuePopup;

			m_nContinueTimes += (oContinuePopup != null && oContinuePopup.IsWatchRewardAds) ? KCDefine.B_VAL_0_INT : KCDefine.B_VAL_1_INT;
			m_nAdsContinueTimes += (oContinuePopup != null && oContinuePopup.IsWatchRewardAds) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
		}

		/** 그만두기 팝업 콜백을 처리한다 */
		private void HandleFinishPopupCallback(CPopup a_oPopup) {
			this.ShowResultPopup(false);
		}

		/** 떠나기 팝업 콜백을 처리한다 */
		private void HandleLeavePopupCallback(CPopup a_oPopup) {
			// 테스트 모드 일 경우
			if(CGameInfoStorage.Inst.PlayMode == EPlayMode.TEST) {
#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_EDITOR_LEVEL);
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
			} else {
#if ADS_MODULE_ENABLE
				Func.ShowFullscreenAds(null, KCDefine.B_VAL_0_1_REAL);
#endif // #if ADS_MODULE_ENABLE

				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
			}
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
		}
		#endregion // 함수

		#region 조건부 함수
#if UNITY_EDITOR
		/** 기즈모를 그린다 */
		public override void OnDrawGizmos() {
			base.OnDrawGizmos();

			// 메인 카메라가 존재 할 경우
			if(CSceneManager.IsExistsMainCamera) {
				var stPrevColor = Gizmos.color;
				var stMainCameraPos = CSceneManager.ActiveSceneMainCamera.transform.position;
				var stPivotPos = stMainCameraPos + new Vector3(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, this.PlaneDistance);

				try {
					var oGridPosList = new List<Vector3>() {
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / -KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / -KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / -KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / -KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale
					};

					for(int i = 0; i < oGridPosList.Count; ++i) {
						int nIdx01 = (i + KCDefine.B_VAL_0_INT) % oGridPosList.Count;
						int nIdx02 = (i + KCDefine.B_VAL_1_INT) % oGridPosList.Count;

						Gizmos.color = Color.magenta;
						Gizmos.DrawLine(oGridPosList[nIdx01], oGridPosList[nIdx02]);
					}
				} finally {
					Gizmos.color = stPrevColor;
				}
			}
		}
#endif // #if UNITY_EDITOR

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

#if ADS_MODULE_ENABLE
		/** 보상 광고가 닫혔을 경우 */
		private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
			// 광고를 시청했을 경우
			if(a_bIsSuccess) {
				// Do Something
			}
		}
#endif // #if ADS_MODULE_ENABLE
#endregion // 조건부 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
