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
			var oObj = CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_POPUP_UI);
			var oPopupUI = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_POPUP_UI, oObj, null);
				
			CSetupSceneManager.m_oPopupUI = oPopupUI;
			CSceneManager.ScreenPopupUIRoot = oPopupUI.ExFindChild(KCDefine.U_OBJ_N_SCREEN_POPUP_UI_ROOT);

			DontDestroyOnLoad(oPopupUI);
			CFunc.SetupScreenUI(oPopupUI, KCDefine.U_SORTING_O_SCREEN_POPUP_UI);
		}
	}

	//! 최상위 UI 를 설정한다
	private void SetupTopmostUI() {
		// 최상위 UI 가 없을 경우
		if(CSetupSceneManager.m_oTopmostUI == null) {
			var oObj = CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_TOPMOST_UI);
			var oTopmostUI = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_TOPMOST_UI, oObj, null);

			CSetupSceneManager.m_oTopmostUI = oTopmostUI;
			CSceneManager.ScreenTopmostUIRoot = oTopmostUI.ExFindChild(KCDefine.U_OBJ_N_SCREEN_TOPMOST_UI_ROOT);

			DontDestroyOnLoad(oTopmostUI);
			CFunc.SetupScreenUI(oTopmostUI, KCDefine.U_SORTING_O_SCREEN_TOPMOST_UI);
		}
	}

	//! 절대 UI 를 설정한다
	private void SetupAbsUI() {
		// 절대 UI 가 없을 경우
		if(CSetupSceneManager.m_oAbsUI == null) {
			var oObj = CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_ABS_UI);
			var oAbsUI = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_ABS_UI, oObj, null);

			CSetupSceneManager.m_oAbsUI = oAbsUI;
			CSceneManager.ScreenAbsUIRoot = oAbsUI.ExFindChild(KCDefine.U_OBJ_N_SCREEN_ABS_UI_ROOT);

			DontDestroyOnLoad(oAbsUI);
			CFunc.SetupScreenUI(oAbsUI, KCDefine.U_SORTING_O_SCREEN_ABS_UI);
		}
	}

	//! 타이머 관리자를 설정한다
	private void SetupTimerManager() {
		// 타이머 관리자가 없을 경우
		if(CSetupSceneManager.m_oTimerManager == null) {
			var oObj = CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_TIMER_MANAGER);
			var oTimerManager = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_TIMER_MANAGER, oObj, null);

			CSetupSceneManager.m_oTimerManager = oTimerManager;
		}
	}

	//! 디버그 UI 를 설정한다
	private void SetupDebugUI() {
#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		// 디버그 UI 가 없을 경우
		if(CSetupSceneManager.m_oDebugUI == null) {
			var oObj = CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_DEBUG_UI);
			var oDebugUI = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_DEBUG_UI, oObj, null);

			CSetupSceneManager.m_oDebugUI = oDebugUI;
			CSceneManager.ScreenDebugUIRoot = oDebugUI.ExFindChild(KCDefine.U_OBJ_N_SCREEN_DEBUG_UI_ROOT);

			CSceneManager.ScreenDebugTextRoot = oDebugUI.ExFindChild(KCDefine.U_OBJ_N_SCREEN_DEBUG_TEXT_ROOT);
			CSceneManager.ScreenDebugTextRoot.SetActive(false);

			CSceneManager.ScreenFPSBtn = oDebugUI.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_FPS_BTN);
			CSceneManager.ScreenFPSBtn.gameObject.SetActive(false);

			CSceneManager.ScreenDebugBtn = oDebugUI.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_DEBUG_BTN);
			CSceneManager.ScreenDebugBtn.gameObject.SetActive(false);

			CSceneManager.ScreenStaticDebugText = oDebugUI.ExFindComponent<Text>(KCDefine.U_OBJ_N_SCREEN_STATIC_DEBUG_TEXT);
			CSceneManager.ScreenStaticDebugText.raycastTarget = false;

			CSceneManager.ScreenDynamicDebugText = oDebugUI.ExFindComponent<Text>(KCDefine.U_OBJ_N_SCREEN_DYNAMIC_DEBUG_TEXT);
			CSceneManager.ScreenDynamicDebugText.raycastTarget = false;

			DontDestroyOnLoad(oDebugUI);
			CFunc.SetupScreenUI(oDebugUI, KCDefine.U_SORTING_O_SCREEN_DEBUG_UI);
		}
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	}

	//! 디버그 콘솔을 설정한다
	private void SetupDebugConsole() {
#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		// 디버그 콘솔이 없을 경우
		if(CSetupSceneManager.m_oDebugConsole == null) {
			var oObj = CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_DEBUG_CONSOLE);
			var oDebugConsole = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_DEBUG_CONSOLE, oObj, null);

			CSetupSceneManager.m_oDebugConsole = oDebugConsole;

			CSceneManager.ScreenDebugConsole = oDebugConsole;
			CSceneManager.ScreenDebugConsole.SetActive(false);

			DontDestroyOnLoad(oDebugConsole);
			CFunc.SetupScreenUI(oDebugConsole, KCDefine.U_SORTING_O_DEBUG_CONSOLE);
		}
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	}

	//! FPS 카운터를 설정한다
	private void SetupFPSCounter() {
#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		// FPS 카운터가 없을 경우
		if(CSetupSceneManager.m_oFPSCounter == null) {
			var oObj = CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_FPS_COUNTER);
			var oFPSCounter = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_FPS_COUNTER, oObj, null);

			CSetupSceneManager.m_oFPSCounter = oFPSCounter;

			CSceneManager.ScreenStaticFPSText = oFPSCounter.ExFindComponent<Text>(KCDefine.U_OBJ_N_FPS_C_STATIC_TEXT);
			CSceneManager.ScreenStaticFPSText.enabled = false;
			CSceneManager.ScreenStaticFPSText.raycastTarget = false;

			CSceneManager.ScreenDynamicFPSText = oFPSCounter.ExFindComponent<Text>(KCDefine.U_OBJ_N_FPS_C_DYNAMIC_TEXT);
			CSceneManager.ScreenDynamicFPSText.enabled = false;
			CSceneManager.ScreenDynamicFPSText.raycastTarget = false;

			DontDestroyOnLoad(oFPSCounter);
			CFunc.SetupScreenUI(oFPSCounter, KCDefine.U_SORTING_O_FPS_COUNTER);
		}
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	}
	#endregion			// 함수
}
