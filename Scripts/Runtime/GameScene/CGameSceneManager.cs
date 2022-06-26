using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameScene {
	/** 게임 씬 관리자 */
	public partial class CGameSceneManager : CSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			BG_TOUCH_RESPONDER,
			OBJ_ROOT,
			FX_OBJ_ROOT,
			SKILL_OBJ_ROOT,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>() {
			[EKey.BG_TOUCH_RESPONDER] = null
		};

		private Dictionary<EKey, GameObject> m_oObjDict = new Dictionary<EKey, GameObject>() {
			[EKey.OBJ_ROOT] = null
		};
		#endregion			// 변수

		#region 프로퍼티
		public override bool IsIgnoreTestUIs => false;
		public override bool IsIgnoreOverlayScene => false;

		public override string SceneName => KCDefine.B_SCENE_N_GAME;
		protected GameObject BGTouchResponder => m_oUIsDict[EKey.BG_TOUCH_RESPONDER];

		protected GameObject ObjRoot => m_oObjDict[EKey.OBJ_ROOT];
		protected GameObject FXObjRoot => m_oObjDict[EKey.FX_OBJ_ROOT];
		protected GameObject SkillObjRoot => m_oObjDict[EKey.SKILL_OBJ_ROOT];
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 객체를 설정한다
				CFunc.SetupObjs(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.OBJ_ROOT, $"{EKey.OBJ_ROOT}", this.Objs, null),
					(EKey.FX_OBJ_ROOT, $"{EKey.FX_OBJ_ROOT}", this.Objs, null),
					(EKey.SKILL_OBJ_ROOT, $"{EKey.SKILL_OBJ_ROOT}", this.Objs, null)
				}, m_oObjDict, false);

				// 터치 전달자를 설정한다 {
				CFunc.SetupTouchResponders(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.BG_TOUCH_RESPONDER, $"{EKey.BG_TOUCH_RESPONDER}", this.UIs, Resources.Load<GameObject>(KCDefine.U_OBJ_P_G_TOUCH_RESPONDER))
				}, CSceneManager.CanvasSize, m_oUIsDict, false);

				m_oUIsDict[EKey.BG_TOUCH_RESPONDER]?.transform.SetAsFirstSibling();
				// 터치 전달자를 설정한다 }
			}
		}
		#endregion			// 함수
	}
}
