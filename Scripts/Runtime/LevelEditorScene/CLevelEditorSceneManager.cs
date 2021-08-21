using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR || UNITY_STANDALONE
//! 레벨 에디터 씬 관리자
public class CLevelEditorSceneManager : CSceneManager {
	#region 변수
	// 객체 {
	protected GameObject m_oPortraitUIs = null;
	protected GameObject m_oLandscapeUIs = null;

	protected GameObject m_oLeftEditorUIs = null;
	protected GameObject m_oRightEditorUIs = null;
	protected GameObject m_oMidEditorUIs = null;

	protected GameObject m_oBlockObjs = null;
	protected GameObject m_oBGTouchResponder = null;
	// 객체 }
	#endregion			// 변수

	#region 프로퍼티
	public override bool IsIgnoreBlindV => true;
	public override bool IsIgnoreBlindH => true;

	public override bool IsRealtimeFadeInAni => true;
	public override bool IsRealtimeFadeOutAni => true;
	
	public override string SceneName => KCDefine.B_SCENE_N_LEVEL_EDITOR;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			Time.timeScale = KCDefine.B_VAL_1_FLT;
			
			m_oPortraitUIs = this.SubUIs.ExFindChild(KCDefine.E_OBJ_N_PORTRAIT_UIS);
			m_oLandscapeUIs = this.SubUIs.ExFindChild(KCDefine.E_OBJ_N_LANDSCAPE_UIS);

			// 블럭 객체를 설정한다 {
			var oBlockObjsA = this.SubObjs.ExFindChild(KCDefine.GS_OBJ_N_BLOCKS);
			var oBlockObjsB = this.SubCanvasObjs.ExFindChild(KCDefine.GS_OBJ_N_BLOCKS);

			m_oBlockObjs = oBlockObjsA ?? oBlockObjsB;
			m_oBlockObjs = m_oBlockObjs ?? CFactory.CreateObj(KCDefine.GS_OBJ_N_BLOCKS, this.SubObjs);
			// 블럭 객체를 설정한다 }

			// 터치 전달자를 설정한다 {
			var oBGTouchResponder = this.SubUIs.ExFindChild(KCDefine.GS_OBJ_N_BG_TOUCH_RESPONDER);
			
			m_oBGTouchResponder = oBGTouchResponder ?? CFactory.CreateTouchResponder(KCDefine.GS_OBJ_N_BG_TOUCH_RESPONDER, KCDefine.U_OBJ_P_G_TOUCH_RESPONDER, this.SubUIs, CSceneManager.CanvasSize, Vector3.zero, KCDefine.U_COLOR_TRANSPARENT);
			m_oBGTouchResponder?.ExSetRaycastTarget<Image>(true, false);
			m_oBGTouchResponder?.transform.SetAsFirstSibling();
			// 터치 전달자를 설정한다 }

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
