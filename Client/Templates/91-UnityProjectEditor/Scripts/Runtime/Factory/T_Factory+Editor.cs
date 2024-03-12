#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
/** 에디터 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion // 클래스 함수

	#region 조건부 클래스 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	/** 에디터 셀 객체 정보를 생성한다 */
	public static STCellObjInfo MakeEditorCellObjInfo(EObjKinds a_eObjKinds, Vector3Int a_stSize, Vector3Int a_stBaseIdx) {
		var stCellObjInfo = new STCellObjInfo(null) {
			ObjKinds = a_eObjKinds, SizeX = a_stSize.x, SizeY = a_stSize.y, SizeZ = a_stSize.z
		};

		stCellObjInfo.OnAfterDeserialize(a_stBaseIdx);
		return stCellObjInfo;
	}

	/** 에디터 셀 정보를 생성한다 */
	public static STCellInfo MakeEditorCellInfo(Vector3Int a_stIdx) {
		var stCellInfo = new STCellInfo(null) {
			m_oCellObjInfoList = new List<STCellObjInfo>()
		};

		stCellInfo.OnAfterDeserialize(a_stIdx);
		return stCellInfo;
	}

	/** 에디터 레벨 정보를 생성한다 */
	public static CLevelInfo MakeEditorLevelInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT, EGridType a_eGridType = EGridType.SCALE) {
		var stLevelInfo = new CLevelInfo() {
			m_stIDInfo = new STIDInfo(a_nLevelID, a_nStageID, a_nChapterID), GridType = a_eGridType
		};

		stLevelInfo.OnAfterDeserialize();
		return stLevelInfo;
	}

	/** 에디터 생성 정보를 생성한다 */
	public static CSubEditorCreateInfo MakeDefEditorCreateInfo() {
		return new CSubEditorCreateInfo() {
			m_nNumLevels = KCDefine.B_VAL_0_INT,
			m_stMinNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS,
			m_stMaxNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS
		};
	}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 클래스 함수
}

/** 레벨 에디터 씬 팩토리 */
public static partial class Factory {
	#region 클래스 함수

	#endregion // 클래스 함수
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endif // #if SCRIPT_TEMPLATE_ONLY
