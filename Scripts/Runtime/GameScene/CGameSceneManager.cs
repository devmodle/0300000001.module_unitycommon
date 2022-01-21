using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** 게임 씬 관리자 */
public class CGameSceneManager : CSceneManager {
	#region 변수
	/** =====> 객체 <===== */
	protected GameObject m_oBlockObjs = null;
	protected GameObject m_oBGTouchResponder = null;
	#endregion			// 변수

	#region 프로퍼티
	public override bool IsRealtimeFadeInAni => true;
	public override bool IsRealtimeFadeOutAni => true;

	public override string SceneName => KCDefine.B_SCENE_N_GAME;
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		Time.timeScale = KCDefine.B_VAL_1_FLT;

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			// 블럭 객체를 설정한다
			var oBlockObjs = this.ObjsBase.ExFindChild(KCDefine.GS_OBJ_N_BLOCKS);
			m_oBlockObjs = oBlockObjs ?? CFactory.CreateObj(KCDefine.GS_OBJ_N_BLOCKS, this.Objs);

			// 터치 전달자를 설정한다 {
			var oBGTouchResponder = this.UIsBase.ExFindChild(KCDefine.U_OBJ_N_BG_TOUCH_RESPONDER);

			m_oBGTouchResponder = oBGTouchResponder ?? CFactory.CreateTouchResponder(KCDefine.U_OBJ_N_BG_TOUCH_RESPONDER, KCDefine.U_OBJ_P_G_TOUCH_RESPONDER, this.UIs, CSceneManager.CanvasSize, Vector3.zero, KCDefine.U_COLOR_TRANSPARENT);
			m_oBGTouchResponder?.ExSetRaycastTarget<Image>(true, false);
			m_oBGTouchResponder?.transform.SetAsFirstSibling();
			// 터치 전달자를 설정한다 }
		}
	}
	#endregion			// 함수
}
