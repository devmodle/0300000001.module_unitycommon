using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene {
	/** 게임 씬 관리자 */
	public partial class CGameSceneManager : CSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			FX_ROOT,
			OBJ_ROOT,
			BG_TOUCH_RESPONDER,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oObjDict = new Dictionary<EKey, GameObject>() {
			[EKey.FX_ROOT] = null,
			[EKey.OBJ_ROOT] = null,
			[EKey.BG_TOUCH_RESPONDER] = null
		};
		#endregion			// 변수

		#region 프로퍼티
		public override bool IsIgnoreTestUIs => false;
		public override bool IsIgnoreOverlayScene => false;

		public override string SceneName => KCDefine.B_SCENE_N_GAME;

		protected GameObject FXRoot => m_oObjDict[EKey.FX_ROOT];
		protected GameObject ObjRoot => m_oObjDict[EKey.OBJ_ROOT];
		protected GameObject BGTouchResponder => m_oObjDict[EKey.BG_TOUCH_RESPONDER];
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 객체 객체를 설정한다
				var oFXRoot = this.ObjsBase.ExFindChild(KCDefine.GS_OBJ_N_FX_ROOT);
				m_oObjDict[EKey.FX_ROOT] = oFXRoot ?? CFactory.CreateObj(KCDefine.GS_OBJ_N_FX_ROOT, this.Objs);

				var oObjRoot = this.ObjsBase.ExFindChild(KCDefine.GS_OBJ_N_OBJ_ROOT);
				m_oObjDict[EKey.OBJ_ROOT] = oObjRoot ?? CFactory.CreateObj(KCDefine.GS_OBJ_N_OBJ_ROOT, this.Objs);

				// 터치 전달자를 설정한다 {
				var oBGTouchResponder = this.UIsBase.ExFindChild(KCDefine.U_OBJ_N_BG_TOUCH_RESPONDER);

				m_oObjDict[EKey.BG_TOUCH_RESPONDER] = oBGTouchResponder ?? CFactory.CreateTouchResponder(KCDefine.U_OBJ_N_BG_TOUCH_RESPONDER, KCDefine.U_OBJ_P_G_TOUCH_RESPONDER, this.UIs, CSceneManager.CanvasSize, Vector3.zero, KCDefine.U_COLOR_TRANSPARENT);
				m_oObjDict[EKey.BG_TOUCH_RESPONDER]?.ExSetRaycastTarget<Image>(true, false);
				m_oObjDict[EKey.BG_TOUCH_RESPONDER]?.transform.SetAsFirstSibling();
				// 터치 전달자를 설정한다 }
			}
		}
		#endregion			// 함수
	}
}
