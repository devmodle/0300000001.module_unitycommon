using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! 설정 씬 관리자 - 설정
public abstract partial class CSetupSceneManager : CSceneManager {
	#region 함수
	//! 팝업 UI 를 설정한다
	private void SetupPopupUI() {
		// 팝업 UI 가 없을 경우
		if(CSetupSceneManager.m_oPopupUI == null) {
			var oPopupUI = CFactory.CreateCloneObj(KCDefine.SS_OBJ_NAME_POPUP_UI,
				CResManager.Instance.GetPrefab(KCDefine.SS_OBJ_PATH_SCREEN_POPUP_UI), null);

			CSetupSceneManager.m_oPopupUI = oPopupUI;
			CSceneManager.ScreenPopupUIRoot = oPopupUI.ExFindChild(KCDefine.U_OBJ_NAME_SCREEN_POPUP_UI_ROOT);

			DontDestroyOnLoad(oPopupUI);
			CFunc.SetupScreenUI(oPopupUI, KCDefine.U_SORTING_ORDER_SCREEN_POPUP_UI);
		}
	}

	//! 최상위 UI 를 설정한다
	private void SetupTopmostUI() {
		// 최상위 UI 가 없을 경우
		if(CSetupSceneManager.m_oTopmostUI == null) {
			var oTopmostUI = CFactory.CreateCloneObj(KCDefine.SS_OBJ_NAME_TOPMOST_UI,
				CResManager.Instance.GetPrefab(KCDefine.SS_OBJ_PATH_SCREEN_TOPMOST_UI), null);

			CSetupSceneManager.m_oTopmostUI = oTopmostUI;
			CSceneManager.ScreenTopmostUIRoot = oTopmostUI.ExFindChild(KCDefine.U_OBJ_NAME_SCREEN_TOPMOST_UI_ROOT);

			DontDestroyOnLoad(oTopmostUI);
			CFunc.SetupScreenUI(oTopmostUI, KCDefine.U_SORTING_ORDER_SCREEN_TOPMOST_UI);
		}
	}

	//! 절대 UI 를 설정한다
	private void SetupAbsUI() {
		// 절대 UI 가 없을 경우
		if(CSetupSceneManager.m_oAbsUI == null) {
			var oAbsUI = CFactory.CreateCloneObj(KCDefine.SS_OBJ_NAME_ABS_UI,
				CResManager.Instance.GetPrefab(KCDefine.SS_OBJ_PATH_SCREEN_ABS_UI), null);

			CSetupSceneManager.m_oAbsUI = oAbsUI;
			CSceneManager.ScreenAbsUIRoot = oAbsUI.ExFindChild(KCDefine.U_OBJ_NAME_SCREEN_ABS_UI_ROOT);

			DontDestroyOnLoad(oAbsUI);
			CFunc.SetupScreenUI(oAbsUI, KCDefine.U_SORTING_ORDER_SCREEN_ABS_UI);
		}
	}

	//! 타이머 관리자를 설정한다
	private void SetupTimerManager() {
		// 타이머 관리자가 없을 경우
		if(CSetupSceneManager.m_oTimerManager == null) {
			var oTimerManager = CFactory.CreateCloneObj(KCDefine.SS_OBJ_NAME_TIMER_MANAGER,
				CResManager.Instance.GetPrefab(KCDefine.U_OBJ_PATH_TIMER_MANAGER), null);

			CSetupSceneManager.m_oTimerManager = oTimerManager;
		}
	}
	#endregion			// 함수

		#region 조건부 함수
#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	//! 디버그 UI 를 설정한다
	private void SetupDebugUI() {
		// 디버그 UI 가 없을 경우
		if(CSetupSceneManager.m_oDebugUI == null) {
			var oDebugUI = CFactory.CreateCloneObj(KCDefine.SS_OBJ_NAME_DEBUG_UI,
				CResManager.Instance.GetPrefab(KCDefine.SS_OBJ_PATH_SCREEN_DEBUG_UI), null);

			CSetupSceneManager.m_oDebugUI = oDebugUI;

			CSceneManager.ScreenDebugUIRoot = oDebugUI.ExFindChild(KCDefine.U_NAME_SCREEN_DEBUG_UI_ROOT);
			CSceneManager.ScreenDebugTextRoot = oDebugUI.ExFindChild(KCDefine.U_NAME_SCREEN_DEBUG_TEXT_ROOT);

			CSceneManager.ScreenFPSBtn = oDebugUI.ExFindComponent<Button>(KCDefine.U_NAME_SCREEN_FPS_BTN);
			CSceneManager.ScreenFPSBtn.gameObject.SetActive(false);

			CSceneManager.ScreenDebugBtn = oDebugUI.ExFindComponent<Button>(KCDefine.U_NAME_SCREEN_DEBUG_BTN);
			CSceneManager.ScreenDebugBtn.gameObject.SetActive(false);

			CSceneManager.ScreenStaticDebugText = oDebugUI.ExFindComponent<Text>(KCDefine.U_NAME_SCREEN_STATIC_DEBUG_TEXT);
			CSceneManager.ScreenStaticDebugText.raycastTarget = false;

			CSceneManager.ScreenDynamicDebugText = oDebugUI.ExFindComponent<Text>(KCDefine.U_NAME_SCREEN_DYNAMIC_DEBUG_TEXT);
			CSceneManager.ScreenDynamicDebugText.raycastTarget = false;

			DontDestroyOnLoad(oDebugUI);

			CSceneManager.ScreenDebugTextRoot.SetActive(false);
			CFunc.SetupScreenUI(oDebugUI, KCDefine.U_SORTING_ORDER_SCREEN_DEBUG_UI);
		}
	}
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	//! FPS 카운터를 설정한다
	private void SetupFPSCounter() {
		// FPS 카운터가 없을 경우
		if(CSetupSceneManager.m_oFPSCounter == null) {
			var oFPSCounter = CFactory.CreateCloneObj(KCDefine.SS_OBJ_NAME_FPS_COUNTER,
				CResManager.Instance.GetPrefab(KCDefine.U_OBJ_PATH_FPS_COUNTER), null);

			CSetupSceneManager.m_oFPSCounter = oFPSCounter;

			CSetupSceneManager.ScreenStaticFPSText = oFPSCounter.ExFindComponent<Text>(KCDefine.U_NAME_SCREEN_STATIC_FPS_TEXT);
			CSetupSceneManager.ScreenStaticFPSText.enabled = false;
			CSetupSceneManager.ScreenStaticFPSText.raycastTarget = false;

			CSetupSceneManager.ScreenDynamicFPSText = oFPSCounter.ExFindComponent<Text>(KCDefine.U_NAME_SCREEN_DYNAMIC_FPS_TEXT);
			CSetupSceneManager.ScreenDynamicFPSText.enabled = false;
			CSetupSceneManager.ScreenDynamicFPSText.raycastTarget = false;

			DontDestroyOnLoad(oFPSCounter);
			CFunc.SetupScreenUI(oFPSCounter, KCDefine.U_SORTING_ORDER_FPS_COUNTER);
		}
	}
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 조건부 함수
}
