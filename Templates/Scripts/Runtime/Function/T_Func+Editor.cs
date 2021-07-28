using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	//! 에디터 레벨 생성 팝업을 출력한다
	public static void ShowEditorLevelCreatePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CEditorLevelCreatePopup>(KCDefine.E_OBJ_N_EDITOR_LEVEL_CREATE_POPUP, KCDefine.E_OBJ_P_EDITOR_LEVEL_CREATE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	//! 에디터 레벨 정보를 설정한다
	public static void EditorSetupLevelInfo(CLevelInfo a_oLevelInfo, STEditorLevelCreateInfo a_stCreateInfo) {
		int nNumCellsX = Random.Range(a_stCreateInfo.m_nMinNumCellsX, a_stCreateInfo.m_nMaxNumCellsX + KCDefine.B_VAL_1_INT);
		int nNumCellsY = Random.Range(a_stCreateInfo.m_nMinNumCellsY, a_stCreateInfo.m_nMaxNumCellsY + KCDefine.B_VAL_1_INT);

		a_oLevelInfo.m_oCellInfoDictContainer.Clear();

		for(int i = 0; i < nNumCellsY; ++i) {
			var oCellInfoDict = new Dictionary<int, CCellInfo>();

			for(int j = 0; j < nNumCellsY; ++j) {
				var stIdx = new Vector3Int(j, i, KCDefine.B_IDX_INVALID);
				oCellInfoDict.Add(j, Factory.MakeCellInfo(stIdx));
			}

			a_oLevelInfo.m_oCellInfoDictContainer.Add(i, oCellInfoDict);
		}

		a_oLevelInfo.OnAfterDeserialize();
		Func.EditorSetupCellInfos(a_oLevelInfo, a_stCreateInfo);
	}

	//! 에디터 셀 정보를 설정한다
	private static void EditorSetupCellInfos(CLevelInfo a_oLevelInfo, STEditorLevelCreateInfo a_stCreateInfo) {
		foreach(var stKeyVal in a_oLevelInfo.m_oCellInfoDictContainer) {
			foreach(var stCellInfoKeyVal in stKeyVal.Value) {
				stCellInfoKeyVal.Value.m_oBlockKindsList.Add(SampleEngineName.EBlockKinds.BG_EMPTY);
			}
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
#endif			// #if NEVER_USE_THIS
