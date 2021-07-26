using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR || UNITY_STANDALONE
//! 레벨 에디터 씬 관리자
public class CLevelEditorSceneManager : CSceneManager {
	#region 객체
	protected GameObject m_oPortraitUIs = null;
	protected GameObject m_oLandscapeUIs = null;

	protected GameObject m_oLeftEditorUIs = null;
	protected GameObject m_oRightEditorUIs = null;
	protected GameObject m_oMidEditorUIs = null;
	#endregion			// 객체

	#region 프로퍼티
	public override bool IsIgnoreBlindV => true;
	public override bool IsIgnoreBlindH => true;
	
	public override string SceneName => KCDefine.B_SCENE_N_LEVEL_EDITOR;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			m_oPortraitUIs = this.SubUIs.ExFindChild(KCDefine.E_OBJ_N_PORTRAIT_UIS);
			m_oLandscapeUIs = this.SubUIs.ExFindChild(KCDefine.E_OBJ_N_LANDSCAPE_UIS);

#if MODE_PORTRAIT_ENABLE
			m_oPortraitUIs.SetActive(true);
			m_oLandscapeUIs.SetActive(false);

			m_oLeftEditorUIs = m_oPortraitUIs.ExFindChild(KCDefine.E_OBJ_N_LEFT_EDITOR_UIS);
			m_oRightEditorUIs = m_oPortraitUIs.ExFindChild(KCDefine.E_OBJ_N_RIGHT_EDITOR_UIS);
			m_oMidEditorUIs = m_oPortraitUIs.ExFindChild(KCDefine.E_OBJ_N_MID_EDITOR_UIS);
#else
			m_oPortraitUIs.SetActive(false);
			m_oLandscapeUIs.SetActive(true);
			
			m_oLeftEditorUIs = m_oLandscapeUIs.ExFindChild(KCDefine.E_OBJ_N_LEFT_EDITOR_UIS);
			m_oRightEditorUIs = m_oLandscapeUIs.ExFindChild(KCDefine.E_OBJ_N_RIGHT_EDITOR_UIS);
			m_oMidEditorUIs = m_oLandscapeUIs.ExFindChild(KCDefine.E_OBJ_N_MID_EDITOR_UIS);
#endif			// #if MODE_PORTRAIT_ENABLE
		}
	}
	#endregion			// 함수
}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
