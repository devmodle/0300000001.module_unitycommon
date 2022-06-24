using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
namespace LevelEditorScene {
	/** 레벨 에디터 씬 관리자 */
	public partial class CLevelEditorSceneManager : CSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,

			MID_EDITOR_UIS,
			LEFT_EDITOR_UIS,
			RIGHT_EDITOR_UIS,

			ME_UIS_MSG_UIS,
			LE_UIS_AB_SET_UIS,

			OBJ_ROOT,
			BG_TOUCH_RESPONDER,

			[HideInInspector] MAX_VAL
		}

		#region 변수
		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>() {
			[EKey.MID_EDITOR_UIS] = null,
			[EKey.LEFT_EDITOR_UIS] = null,
			[EKey.RIGHT_EDITOR_UIS] = null,

			[EKey.ME_UIS_MSG_UIS] = null,
			[EKey.LE_UIS_AB_SET_UIS] = null
		};

		private Dictionary<EKey, GameObject> m_oObjDict = new Dictionary<EKey, GameObject>() {
			[EKey.OBJ_ROOT] = null,
			[EKey.BG_TOUCH_RESPONDER] = null
		};
		#endregion			// 변수

		#region 프로퍼티
		public override bool IsIgnoreBlindV => true;
		public override bool IsIgnoreBlindH => true;

		public override float ScreenWidth => KCDefine.B_PORTRAIT_SCREEN_WIDTH;
		public override float ScreenHeight => KCDefine.B_PORTRAIT_SCREEN_HEIGHT;
		
		public override string SceneName => KCDefine.B_SCENE_N_LEVEL_EDITOR;

		protected GameObject MidEditorUIs => m_oUIsDict[EKey.MID_EDITOR_UIS];
		protected GameObject LeftEditorUIs => m_oUIsDict[EKey.LEFT_EDITOR_UIS];
		protected GameObject RightEditorUIs => m_oUIsDict[EKey.RIGHT_EDITOR_UIS];

		protected GameObject MEUIsMsgUIs => m_oUIsDict[EKey.ME_UIS_MSG_UIS];
		protected GameObject LEUIsABSetUIs => m_oUIsDict[EKey.LE_UIS_AB_SET_UIS];

		protected GameObject ObjRoot => m_oObjDict[EKey.OBJ_ROOT];
		protected GameObject BGTouchResponder => m_oObjDict[EKey.BG_TOUCH_RESPONDER];
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			
			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				CSceneManager.ScreenDebugUIs?.SetActive(false);

				m_oUIsDict[EKey.MID_EDITOR_UIS] = this.UIsBase.ExFindChild($"{EKey.MID_EDITOR_UIS}");
				m_oUIsDict[EKey.LEFT_EDITOR_UIS] = this.UIsBase.ExFindChild($"{EKey.LEFT_EDITOR_UIS}");
				m_oUIsDict[EKey.RIGHT_EDITOR_UIS] = this.UIsBase.ExFindChild($"{EKey.RIGHT_EDITOR_UIS}");

				m_oUIsDict[EKey.ME_UIS_MSG_UIS] = m_oUIsDict[EKey.MID_EDITOR_UIS].ExFindChild($"{EKey.ME_UIS_MSG_UIS}");
				m_oUIsDict[EKey.LE_UIS_AB_SET_UIS] = m_oUIsDict[EKey.LEFT_EDITOR_UIS].ExFindChild($"{EKey.LE_UIS_AB_SET_UIS}");

				m_oUIsDict[EKey.ME_UIS_MSG_UIS]?.SetActive(false);

				// 객체 객체를 설정한다
				var oObjRoot = this.ObjsBase.ExFindChild($"{EKey.OBJ_ROOT}");
				m_oObjDict[EKey.OBJ_ROOT] = oObjRoot ?? CFactory.CreateObj($"{EKey.OBJ_ROOT}", this.Objs);

				// 터치 전달자를 설정한다
				var oBGTouchResponder = this.UIsBase.ExFindChild($"{EKey.BG_TOUCH_RESPONDER}");
				m_oObjDict[EKey.BG_TOUCH_RESPONDER] = oBGTouchResponder ?? CFactory.CreateTouchResponder($"{EKey.BG_TOUCH_RESPONDER}", KCDefine.U_OBJ_P_G_TOUCH_RESPONDER, this.UIs, CSceneManager.CanvasSize, Vector3.zero, KCDefine.U_COLOR_TRANSPARENT);
				m_oObjDict[EKey.BG_TOUCH_RESPONDER]?.ExSetRaycastTarget<Image>(true, false);
				m_oObjDict[EKey.BG_TOUCH_RESPONDER]?.transform.SetAsFirstSibling();
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
