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
			var oPopupUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_SCREEN_POPUP_UIS, KCDefine.SS_OBJ_P_SCREEN_POPUP_UIS, null);
				
			CSetupSceneManager.m_oPopupUIs = oPopupUIs;
			CSceneManager.ScreenPopupUIs = oPopupUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_POPUP_UIS, false);

			DontDestroyOnLoad(oPopupUIs);
			CFunc.SetupScreenUIs(oPopupUIs, KCDefine.U_SORTING_O_SCREEN_POPUP_UIS);
		}
	}

	/** 최상위 UI 를 설정한다 */
	private void SetupTopmostUIs() {
		// 최상위 UI 가 없을 경우
		if(CSetupSceneManager.m_oTopmostUIs == null) {
			var oTopmostUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_SCREEN_TOPMOST_UIS, KCDefine.SS_OBJ_P_SCREEN_TOPMOST_UIS, null);

			CSetupSceneManager.m_oTopmostUIs = oTopmostUIs;
			CSceneManager.ScreenTopmostUIs = oTopmostUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_TOPMOST_UIS, false);

			DontDestroyOnLoad(oTopmostUIs);
			CFunc.SetupScreenUIs(oTopmostUIs, KCDefine.U_SORTING_O_SCREEN_TOPMOST_UIS);
		}
	}

	/** 절대 UI 를 설정한다 */
	private void SetupAbsUIs() {
		// 절대 UI 가 없을 경우
		if(CSetupSceneManager.m_oAbsUIs == null) {
			var oAbsUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_SCREEN_ABS_UIS, KCDefine.SS_OBJ_P_SCREEN_ABS_UIS, null);

			CSetupSceneManager.m_oAbsUIs = oAbsUIs;
			CSceneManager.ScreenAbsUIs = oAbsUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_ABS_UIS, false);

			DontDestroyOnLoad(oAbsUIs);
			CFunc.SetupScreenUIs(oAbsUIs, KCDefine.U_SORTING_O_SCREEN_ABS_UIS);
		}
	}

	/** 타이머 관리자를 설정한다 */
	private void SetupTimerManager() {
		// 타이머 관리자가 없을 경우
		if(CSetupSceneManager.m_oTimerManager == null) {
			var oTimerManager = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_TIMER_MANAGER, KCDefine.U_OBJ_P_TIMER_MANAGER, null);
			CSetupSceneManager.m_oTimerManager = oTimerManager;
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
	/** 디버그 UI 를 설정한다 */
	private void SetupDebugUIs() {
		// 디버그 UI 가 없을 경우
		if(CSetupSceneManager.m_oDebugUIs == null) {
			var oDebugUIs = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_DEBUG_UIS, KCDefine.SS_OBJ_P_SCREEN_DEBUG_UIS, null);

			CSetupSceneManager.m_oDebugUIs = oDebugUIs;
			CSceneManager.ScreenDebugUIs = oDebugUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_DEBUG_UIS);

			CSceneManager.ScreenDebugTextUIs = oDebugUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_DEBUG_TEXT_UIS);
			CSceneManager.ScreenDebugTextUIs.SetActive(false);

			CSceneManager.ScreenStaticDebugText = oDebugUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_SCREEN_STATIC_DEBUG_TEXT);
			CSceneManager.ScreenStaticDebugText.raycastTarget = false;

			CSceneManager.ScreenDynamicDebugText = oDebugUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_SCREEN_DYNAMIC_DEBUG_TEXT);
			CSceneManager.ScreenDynamicDebugText.raycastTarget = false;

			CSceneManager.ScreenFPSBtn = oDebugUIs.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_FPS_BTN);
			CSceneManager.ScreenFPSBtn.gameObject.SetActive(false);

			CSceneManager.ScreenDebugBtn = oDebugUIs.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_DEBUG_BTN);
			CSceneManager.ScreenDebugBtn.gameObject.SetActive(false);

			DontDestroyOnLoad(oDebugUIs);
			CFunc.SetupScreenUIs(oDebugUIs, KCDefine.U_SORTING_O_SCREEN_DEBUG_UIS);
		}
	}

	/** FPS 카운터를 설정한다 */
	private void SetupFPSCounter() {
		// FPS 카운터가 없을 경우
		if(CSetupSceneManager.m_oFPSCounter == null) {
			var oFPSCounter = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_FPS_COUNTER, KCDefine.U_OBJ_P_FPS_COUNTER, null);
			CSetupSceneManager.m_oFPSCounter = oFPSCounter;

			CSceneManager.ScreenStaticFPSText = oFPSCounter.ExFindComponent<Text>(KCDefine.U_OBJ_N_FPS_C_STATIC_TEXT);
			CSceneManager.ScreenStaticFPSText.enabled = false;
			CSceneManager.ScreenStaticFPSText.raycastTarget = false;

			CSceneManager.ScreenDynamicFPSText = oFPSCounter.ExFindComponent<Text>(KCDefine.U_OBJ_N_FPS_C_DYNAMIC_TEXT);
			CSceneManager.ScreenDynamicFPSText.enabled = false;
			CSceneManager.ScreenDynamicFPSText.raycastTarget = false;

			DontDestroyOnLoad(oFPSCounter);
			CFunc.SetupScreenUIs(oFPSCounter, KCDefine.U_SORTING_O_FPS_COUNTER);
		}
	}
#endif			// #if DEBUG || DEVELOPMENT_BUILD
	#endregion			// 조건부 함수
}
