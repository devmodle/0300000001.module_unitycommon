using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if UNITY_EDITOR || UNITY_STANDALONE
//! 에디터 함수
public static partial class Func {
	#region 클래스 함수
	//! 에디터 종료 팝업을 출력한다
	public static void ShowEditorQuitPopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_QUIT_P_MSG), a_oCallback);
	}
	
	//! 에디터 레벨 제거 팝업을 출력한다
	public static void ShowEditorLevelRemovePopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_REMOVE_LP_MSG), a_oCallback);
	}

	//! 에디터 스테이지 제거 팝업을 출력한다
	public static void ShowEditorStageRemovePopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_REMOVE_SP_MSG), a_oCallback);
	}

	//! 에디터 챕터 제거 팝업을 출력한다
	public static void ShowEditorChapterRemovePopup(System.Action<CAlertPopup, bool> a_oCallback) {
		Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_EDITOR_REMOVE_CP_MSG), a_oCallback);
	}

	//! 에디터 입력 팝업을 출력한다
	public static void ShowEditorInputPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CEditorInputPopup>(KCDefine.E_OBJ_N_EDITOR_INPUT_POPUP, KCDefine.E_OBJ_P_EDITOR_INPUT_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
#endif			// #if NEVER_USE_THIS
