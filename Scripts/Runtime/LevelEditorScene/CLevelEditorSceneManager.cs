using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
/** 레벨 에디터 씬 관리자 */
public class CLevelEditorSceneManager : CSceneManager {
	#region 변수
	/** =====> 객체 <===== */
	protected GameObject m_oPortraitUIs = null;
	protected GameObject m_oLandscapeUIs = null;

	protected GameObject m_oMidEditorUIs = null;
	protected GameObject m_oLeftEditorUIs = null;
	protected GameObject m_oRightEditorUIs = null;

	protected GameObject m_oMEUIsMsgUIs = null;
	protected GameObject m_oLEUIsSetUIs = null;

	protected GameObject m_oBlockObjs = null;
	protected GameObject m_oBGTouchResponder = null;
	#endregion			// 변수

	#region 프로퍼티
	public override bool IsIgnoreBlindV => true;
	public override bool IsIgnoreBlindH => true;

	public override bool IsRealtimeFadeInAni => true;
	public override bool IsRealtimeFadeOutAni => true;
	
	public override string SceneName => KCDefine.B_SCENE_N_LEVEL_EDITOR;
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		Time.timeScale = KCDefine.B_VAL_1_FLT;
		
		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			CSceneManager.ScreenDebugUIs?.SetActive(false);
			
			m_oPortraitUIs = this.UIsBase.ExFindChild(KCDefine.E_OBJ_N_PORTRAIT_UIS);
			m_oLandscapeUIs = this.UIsBase.ExFindChild(KCDefine.E_OBJ_N_LANDSCAPE_UIS);

			// 블럭 객체를 설정한다
			var oBlockObjs = this.ObjsBase.ExFindChild(KCDefine.GS_OBJ_N_BLOCKS);
			m_oBlockObjs = oBlockObjs ?? CFactory.CreateObj(KCDefine.GS_OBJ_N_BLOCKS, this.Objs);

			// 터치 전달자를 설정한다 {
			var oBGTouchResponder = this.UIsBase.ExFindChild(KCDefine.U_OBJ_N_BG_TOUCH_RESPONDER);
			
			m_oBGTouchResponder = oBGTouchResponder ?? CFactory.CreateTouchResponder(KCDefine.U_OBJ_N_BG_TOUCH_RESPONDER, KCDefine.U_OBJ_P_G_TOUCH_RESPONDER, this.UIs, CSceneManager.CanvasSize, Vector3.zero, KCDefine.U_COLOR_TRANSPARENT);
			m_oBGTouchResponder?.ExSetRaycastTarget<Image>(true, false);
			m_oBGTouchResponder?.transform.SetAsFirstSibling();
			// 터치 전달자를 설정한다 }

#if MODE_PORTRAIT_ENABLE
			m_oPortraitUIs.SetActive(true);
			m_oLandscapeUIs.SetActive(false);

			m_oMidEditorUIs = m_oPortraitUIs.ExFindChild(KCDefine.E_OBJ_N_MID_EDITOR_UIS);
			m_oLeftEditorUIs = m_oPortraitUIs.ExFindChild(KCDefine.E_OBJ_N_LEFT_EDITOR_UIS);
			m_oRightEditorUIs = m_oPortraitUIs.ExFindChild(KCDefine.E_OBJ_N_RIGHT_EDITOR_UIS);
#else
			m_oPortraitUIs.SetActive(false);
			m_oLandscapeUIs.SetActive(true);

			m_oMidEditorUIs = m_oLandscapeUIs.ExFindChild(KCDefine.E_OBJ_N_MID_EDITOR_UIS);
			m_oLeftEditorUIs = m_oLandscapeUIs.ExFindChild(KCDefine.E_OBJ_N_LEFT_EDITOR_UIS);
			m_oRightEditorUIs = m_oLandscapeUIs.ExFindChild(KCDefine.E_OBJ_N_RIGHT_EDITOR_UIS);
#endif			// #if MODE_PORTRAIT_ENABLE

			m_oMEUIsMsgUIs = m_oMidEditorUIs.ExFindChild(KCDefine.E_OBJ_N_ME_UIS_MSG_UIS);
			m_oMEUIsMsgUIs?.SetActive(false);

			m_oLEUIsSetUIs = m_oLeftEditorUIs.ExFindChild(KCDefine.E_OBJ_N_LE_UIS_SET_UIS);
			m_oLEUIsSetUIs?.SetActive(false);
		}
	}
	#endregion			// 함수
}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
