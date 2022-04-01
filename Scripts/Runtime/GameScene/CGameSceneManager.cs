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
			FX_OBJS,
			BLOCK_OBJS,
			BG_TOUCH_RESPONDER,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oObjDict = new Dictionary<EKey, GameObject>() {
			[EKey.FX_OBJS] = null,
			[EKey.BLOCK_OBJS] = null,
			[EKey.BG_TOUCH_RESPONDER] = null
		};
		#endregion			// 변수

		#region 프로퍼티
		public override bool IsRealtimeFadeInAni => true;
		public override bool IsRealtimeFadeOutAni => true;

		public override string SceneName => KCDefine.B_SCENE_N_GAME;

		protected GameObject FXObjs => m_oObjDict[EKey.FX_OBJS];
		protected GameObject BlockObjs => m_oObjDict[EKey.BLOCK_OBJS];
		protected GameObject BGTouchResponder => m_oObjDict[EKey.BG_TOUCH_RESPONDER];
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			Time.timeScale = KCDefine.B_VAL_1_FLT;

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 블럭 객체를 설정한다
				var oFXObjs = this.ObjsBase.ExFindChild(KCDefine.GS_OBJ_N_FXS);
				m_oObjDict[EKey.FX_OBJS] = oFXObjs ?? CFactory.CreateObj(KCDefine.GS_OBJ_N_FXS, this.Objs);

				var oBlockObjs = this.ObjsBase.ExFindChild(KCDefine.GS_OBJ_N_BLOCKS);
				m_oObjDict[EKey.BLOCK_OBJS] = oBlockObjs ?? CFactory.CreateObj(KCDefine.GS_OBJ_N_BLOCKS, this.Objs);

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
