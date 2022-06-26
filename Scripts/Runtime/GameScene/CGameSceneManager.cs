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
				// 객체를 설정한다 {
				var oObjKeyInfoList = new List<(EKey, GameObject)>() {
					(EKey.OBJ_ROOT, this.Objs),
					(EKey.FX_OBJ_ROOT, this.Objs),
					(EKey.SKILL_OBJ_ROOT, this.Objs)
				};

				for(int i = 0; i < oObjKeyInfoList.Count; ++i) {
					var oObj = oObjKeyInfoList[i].Item2.ExFindChild($"{oObjKeyInfoList[i].Item1}");
					m_oObjDict[oObjKeyInfoList[i].Item1] = oObj ?? CFactory.CreateObj($"{oObjKeyInfoList[i].Item1}", oObjKeyInfoList[i].Item2);
				}
				// 객체를 설정한다 }

				// 터치 전달자를 설정한다 {
				var oTouchResponderKeyInfoList = new List<(EKey, GameObject)>() {
					(EKey.BG_TOUCH_RESPONDER, this.UIsBase)
				};

				for(int i = 0; i < oTouchResponderKeyInfoList.Count; ++i) {
					var oTouchResponder = oTouchResponderKeyInfoList[i].Item2.ExFindChild($"{oTouchResponderKeyInfoList[i].Item1}");
					m_oUIsDict[oTouchResponderKeyInfoList[i].Item1] = oTouchResponder ?? CFactory.CreateTouchResponder($"{oTouchResponderKeyInfoList[i].Item1}", KCDefine.U_OBJ_P_G_TOUCH_RESPONDER, oTouchResponderKeyInfoList[i].Item2, CSceneManager.CanvasSize, Vector3.zero, KCDefine.U_COLOR_TRANSPARENT);
					m_oUIsDict[oTouchResponderKeyInfoList[i].Item1]?.ExSetRaycastTarget<Image>(true, false);
					m_oUIsDict[oTouchResponderKeyInfoList[i].Item1]?.transform.SetAsFirstSibling();	
				}
				// 터치 전달자를 설정한다 }
			}
		}
		#endregion			// 함수
	}
}
