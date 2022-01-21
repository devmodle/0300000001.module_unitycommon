using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/** 설정 씬 관리자 - 설정 */
public abstract partial class CSetupSceneManager : CSceneManager {
	#region 함수
	/** 팝업 UI 를 설정한다 */
	private void SetupPopupUIs() {
		// 팝업 UI 가 없을 경우
		if(CSetupSceneManager.m_oPopupUIs == null) {
			CSetupSceneManager.m_oPopupUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_SCREEN_POPUP_UIS, KCDefine.SS_OBJ_P_SCREEN_POPUP_UIS, null);
			CSceneManager.ScreenPopupUIs = CSetupSceneManager.m_oPopupUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_POPUP_UIS, false);

			DontDestroyOnLoad(CSetupSceneManager.m_oPopupUIs);
			CFunc.SetupScreenUIs(CSetupSceneManager.m_oPopupUIs, KCDefine.U_SORTING_O_SCREEN_POPUP_UIS);
		}
	}

	/** 최상위 UI 를 설정한다 */
	private void SetupTopmostUIs() {
		// 최상위 UI 가 없을 경우
		if(CSetupSceneManager.m_oTopmostUIs == null) {
			CSetupSceneManager.m_oTopmostUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_SCREEN_TOPMOST_UIS, KCDefine.SS_OBJ_P_SCREEN_TOPMOST_UIS, null);
			CSceneManager.ScreenTopmostUIs = CSetupSceneManager.m_oTopmostUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_TOPMOST_UIS, false);

			DontDestroyOnLoad(CSetupSceneManager.m_oTopmostUIs);
			CFunc.SetupScreenUIs(CSetupSceneManager.m_oTopmostUIs, KCDefine.U_SORTING_O_SCREEN_TOPMOST_UIS);
		}
	}

	/** 절대 UI 를 설정한다 */
	private void SetupAbsUIs() {
		// 절대 UI 가 없을 경우
		if(CSetupSceneManager.m_oAbsUIs == null) {
			CSetupSceneManager.m_oAbsUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_SCREEN_ABS_UIS, KCDefine.SS_OBJ_P_SCREEN_ABS_UIS, null);
			CSceneManager.ScreenAbsUIs = CSetupSceneManager.m_oAbsUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_ABS_UIS, false);

			DontDestroyOnLoad(CSetupSceneManager.m_oAbsUIs);
			CFunc.SetupScreenUIs(CSetupSceneManager.m_oAbsUIs, KCDefine.U_SORTING_O_SCREEN_ABS_UIS);
		}
	}

	/** 타이머 관리자를 설정한다 */
	private void SetupTimerManager() {
		// 타이머 관리자가 없을 경우
		if(CSetupSceneManager.m_oTimerManager == null) {
			CSetupSceneManager.m_oTimerManager = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_TIMER_MANAGER, KCDefine.U_OBJ_P_TIMER_MANAGER, null);
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
	/** 디버그 UI 를 설정한다 */
	private void SetupDebugUIs() {
		// 디버그 UI 가 없을 경우
		if(CSetupSceneManager.m_oDebugUIs == null) {
			CSetupSceneManager.m_oDebugUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_SCREEN_DEBUG_UIS, KCDefine.SS_OBJ_P_SCREEN_DEBUG_UIS, null);
			CSceneManager.ScreenDebugUIs = CSetupSceneManager.m_oDebugUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_DEBUG_UIS, false);

			CSceneManager.ScreenFPSInfoUIs = CSetupSceneManager.m_oDebugUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_FPS_INFO_UIS);
			CSceneManager.ScreenFPSInfoUIs.SetActive(false);

			CSceneManager.ScreenDebugInfoUIs = CSetupSceneManager.m_oDebugUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_DEBUG_INFO_UIS);
			CSceneManager.ScreenDebugInfoUIs.SetActive(false);

			CSceneManager.ScreenFPSText = CSetupSceneManager.m_oDebugUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_SCREEN_FPS_TEXT);
			CSceneManager.ScreenFPSText.raycastTarget = false;

			CSceneManager.ScreenFrameTimeText = CSetupSceneManager.m_oDebugUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_SCREEN_FRAME_TIME_TEXT);
			CSceneManager.ScreenFrameTimeText.raycastTarget = false;

			CSceneManager.ScreenStaticDebugText = CSetupSceneManager.m_oDebugUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_SCREEN_STATIC_DEBUG_TEXT);
			CSceneManager.ScreenStaticDebugText.raycastTarget = false;

			CSceneManager.ScreenDynamicDebugText = CSetupSceneManager.m_oDebugUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_SCREEN_DYNAMIC_DEBUG_TEXT);
			CSceneManager.ScreenDynamicDebugText.raycastTarget = false;

			CSceneManager.ScreenFPSInfoBtn = CSetupSceneManager.m_oDebugUIs.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_FPS_INFO_BTN);
			CSceneManager.ScreenFPSInfoBtn.gameObject.SetActive(false);

			CSceneManager.ScreenDebugInfoBtn = CSetupSceneManager.m_oDebugUIs.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_DEBUG_INFO_BTN);
			CSceneManager.ScreenDebugInfoBtn.gameObject.SetActive(false);

			CSceneManager.ScreenTimeScaleUpBtn = CSetupSceneManager.m_oDebugUIs.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_TIME_SCALE_UP_BTN);
			CSceneManager.ScreenTimeScaleUpBtn.gameObject.SetActive(false);

			CSceneManager.ScreenTimeScaleDownBtn = CSetupSceneManager.m_oDebugUIs.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_TIME_SCALE_DOWN_BTN);
			CSceneManager.ScreenTimeScaleDownBtn.gameObject.SetActive(false);

			DontDestroyOnLoad(CSetupSceneManager.m_oDebugUIs);
			CFunc.SetupScreenUIs(CSetupSceneManager.m_oDebugUIs, KCDefine.U_SORTING_O_SCREEN_DEBUG_UIS);
		}
	}
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 조건부 함수
}
