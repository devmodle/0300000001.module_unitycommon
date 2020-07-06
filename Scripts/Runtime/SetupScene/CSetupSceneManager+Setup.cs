using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! 설정 씬 관리자 - 설정
public abstract partial class CSetupSceneManager : CSceneManager {
	#region 함수
	//! 팝업 UI 를 설정한다
	private void SetupPopupUI() {
		if(CSetupSceneManager.m_oPopupUI == null) {
			var oPopupUI = Func.CreateCloneObj(KDefine.SS_NAME_POPUP_UI,
				CResourceManager.Instance.GetPrefab(KDefine.IS_PATH_SCREEN_POPUP_UI), null);

			CSetupSceneManager.m_oPopupUI = oPopupUI;
			CSceneManager.ScreenPopupUIRoot = oPopupUI.ExFindChild(KDefine.U_OBJ_NAME_SCREEN_POPUP_UI_ROOT);

			DontDestroyOnLoad(oPopupUI);
			Func.SetupScreenUI(oPopupUI, KDefine.U_SORTING_ORDER_SCREEN_POPUP_UI);
		}
	}

	//! 최상위 UI 를 설정한다
	private void SetupTopmostUI() {
		if(CSetupSceneManager.m_oTopmostUI == null) {
			var oTopmostUI = Func.CreateCloneObj(KDefine.SS_NAME_TOPMOST_UI,
				CResourceManager.Instance.GetPrefab(KDefine.IS_PATH_SCREEN_TOPMOST_UI), null);

			CSetupSceneManager.m_oTopmostUI = oTopmostUI;
			CSceneManager.ScreenTopmostUIRoot = oTopmostUI.ExFindChild(KDefine.U_OBJ_NAME_SCREEN_TOPMOST_UI_ROOT);

			DontDestroyOnLoad(oTopmostUI);
			Func.SetupScreenUI(oTopmostUI, KDefine.U_SORTING_ORDER_SCREEN_TOPMOST_UI);
		}
	}

	//! 절대 UI 를 설정한다
	private void SetupAbsoluteUI() {
		if(CSetupSceneManager.m_oAbsoluteUI == null) {
			var oAbsoluteUI = Func.CreateCloneObj(KDefine.SS_NAME_ABSOLUTE_UI,
				CResourceManager.Instance.GetPrefab(KDefine.IS_PATH_SCREEN_ABSOLUTE_UI), null);

			CSetupSceneManager.m_oAbsoluteUI = oAbsoluteUI;
			CSceneManager.ScreenAbsoluteUIRoot = oAbsoluteUI.ExFindChild(KDefine.U_OBJ_NAME_SCREEN_ABSOLUTE_UI_ROOT);

			DontDestroyOnLoad(oAbsoluteUI);
			Func.SetupScreenUI(oAbsoluteUI, KDefine.U_SORTING_ORDER_SCREEN_ABSOLUTE_UI);
		}
	}

	//! 타이머 관리자를 설정한다
	private void SetupTimerManager() {
		if(CSetupSceneManager.m_oTimerManager == null) {
			var oTimerManager = Func.CreateCloneObj(KDefine.SS_NAME_TIMER_MANAGER,
				CResourceManager.Instance.GetPrefab(KDefine.U_OBJ_PATH_SS_TIMER_MANAGER), null);

			CSetupSceneManager.m_oTimerManager = oTimerManager;
		}
	}
	#endregion			// 함수

		#region 조건부 함수
#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	//! 디버그 UI 를 설정한다
	private void SetupDebugUI() {
		if(CSetupSceneManager.m_oDebugUI == null) {
			var oDebugUI = Func.CreateCloneObj(KDefine.SS_NAME_DEBUG_UI,
				CResourceManager.Instance.GetPrefab(KDefine.IS_PATH_SCREEN_DEBUG_UI), null);

			CSetupSceneManager.m_oDebugUI = oDebugUI;

			CSceneManager.ScreenDebugUIRoot = oDebugUI.ExFindChild(KDefine.U_NAME_SCREEN_DEBUG_UI_ROOT);
			CSceneManager.ScreenDebugTextRoot = oDebugUI.ExFindChild(KDefine.U_NAME_SCREEN_DEBUG_TEXT_ROOT);

			CSceneManager.ScreenFPSBtn = oDebugUI.ExFindComponent<Button>(KDefine.U_NAME_SCREEN_FPS_BTN);
			CSceneManager.ScreenDebugBtn = oDebugUI.ExFindComponent<Button>(KDefine.U_NAME_SCREEN_DEBUG_BTN);

			CSceneManager.ScreenStaticDebugText = oDebugUI.ExFindComponent<Text>(KDefine.U_NAME_SCREEN_STATIC_DEBUG_TEXT);
			CSceneManager.ScreenStaticDebugText.raycastTarget = false;

			CSceneManager.ScreenDynamicDebugText = oDebugUI.ExFindComponent<Text>(KDefine.U_NAME_SCREEN_DYNAMIC_DEBUG_TEXT);
			CSceneManager.ScreenDynamicDebugText.raycastTarget = false;

			DontDestroyOnLoad(oDebugUI);

			CSceneManager.ScreenDebugTextRoot.SetActive(false);
			Func.SetupScreenUI(oDebugUI, KDefine.U_SORTING_ORDER_SCREEN_DEBUG_UI);
		}
	}
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	//! FPS 카운터를 설정한다
	private void SetupFPSCounter() {
		if(CSetupSceneManager.m_oFPSCounter == null) {
			var oFPSCounter = Func.CreateCloneObj(KDefine.SS_NAME_FPS_COUNTER,
				CResourceManager.Instance.GetPrefab(KDefine.U_OBJ_PATH_SS_FPS_COUNTER), null);

			CSetupSceneManager.m_oFPSCounter = oFPSCounter;

			CSetupSceneManager.ScreenStaticFPSText = oFPSCounter.ExFindComponent<Text>(KDefine.U_NAME_SCREEN_STATIC_FPS_TEXT);
			CSetupSceneManager.ScreenStaticFPSText.enabled = false;
			CSetupSceneManager.ScreenStaticFPSText.raycastTarget = false;

			CSetupSceneManager.ScreenDynamicFPSText = oFPSCounter.ExFindComponent<Text>(KDefine.U_NAME_SCREEN_DYNAMIC_FPS_TEXT);
			CSetupSceneManager.ScreenDynamicFPSText.enabled = false;
			CSetupSceneManager.ScreenDynamicFPSText.raycastTarget = false;

			DontDestroyOnLoad(oFPSCounter);
			Func.SetupScreenUI(oFPSCounter, KDefine.U_SORTING_ORDER_FPS_COUNTER);
		}
	}
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 조건부 함수
}
