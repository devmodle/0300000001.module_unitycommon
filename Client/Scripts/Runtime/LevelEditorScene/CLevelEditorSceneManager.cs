using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
namespace LevelEditorScene
{
	/** 레벨 에디터 씬 관리자 */
	public partial class CLevelEditorSceneManager : CSceneManager
	{
		/** 식별자 */
		private enum EKey
		{
			NONE = -1,
			MID_EDITOR_UIS,
			LEFT_EDITOR_UIS,
			RIGHT_EDITOR_UIS,

			ME_UIS_MSG_UIS,
			ME_UIS_INFO_UIS,
			ME_UIS_EDITOR_MODE_UIS,
			LE_UIS_AB_SET_UIS,

			OBJ_ROOT,
			EDITOR_OBJ_ROOT,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		[Header("=====> Game Objects <=====")]
		private Dictionary<EKey, GameObject> m_oUIDict = new Dictionary<EKey, GameObject>();
		private Dictionary<EKey, GameObject> m_oObjDict = new Dictionary<EKey, GameObject>();
		#endregion // 변수

		#region 프로퍼티
		public override bool IsIgnoreBlindH => true;
		public override bool IsIgnoreBlindV => true;
		public override bool IsEnableBGTouchResponder => true;

		public override float ScreenWidth => KCDefine.B_DESIGN_P_SCREEN_WIDTH;
		public override float ScreenHeight => KCDefine.B_DESIGN_P_SCREEN_HEIGHT * KCDefine.ES_SCALE_DESIGN_SCREEN_HEIGHT;

		public override EProjection MainCameraProjection => EProjection._3D;
		public override Vector3 ObjRootPivotPos => (this.UIsBase != null && this.UIsBase.ExFindChild(KCDefine.U_OBJ_N_SCENE_MID_EDITOR_UIS) != null) ? Vector3.zero.ExToWorld(this.UIsBase.ExFindChild(KCDefine.U_OBJ_N_SCENE_MID_EDITOR_UIS)).ExToLocal(this.UIs) : Vector3.zero;

		protected GameObject MidEditorUIs => m_oUIDict[EKey.MID_EDITOR_UIS];
		protected GameObject LeftEditorUIs => m_oUIDict[EKey.LEFT_EDITOR_UIS];
		protected GameObject RightEditorUIs => m_oUIDict[EKey.RIGHT_EDITOR_UIS];

		protected GameObject MEUIsMsgUIs => m_oUIDict[EKey.ME_UIS_MSG_UIS];
		protected GameObject MEUIsInfoUIs => m_oUIDict[EKey.ME_UIS_INFO_UIS];
		protected GameObject MEUIsEditorModeUIs => m_oUIDict[EKey.ME_UIS_EDITOR_MODE_UIS];
		protected GameObject LEUIsABSetUIs => m_oUIDict[EKey.LE_UIS_AB_SET_UIS];

		protected GameObject ObjRoot => m_oObjDict[EKey.OBJ_ROOT];
		protected GameObject EditorObjRoot => m_oObjDict[EKey.EDITOR_OBJ_ROOT];
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake()
		{
			base.Awake();

			// 앱 초기화가 필요 할 경우
			if(!CSceneManager.IsAppInit)
			{
				return;
			}

			// 객체를 설정한다 {
			CFunc.SetupGameObjs(new List<(EKey, string, GameObject)>() {
				(EKey.MID_EDITOR_UIS, $"{EKey.MID_EDITOR_UIS}", this.UIsBase),
				(EKey.LEFT_EDITOR_UIS, $"{EKey.LEFT_EDITOR_UIS}", this.UIsBase),
				(EKey.RIGHT_EDITOR_UIS, $"{EKey.RIGHT_EDITOR_UIS}", this.UIsBase)
			}, m_oUIDict);

			CFunc.SetupGameObjs(new List<(EKey, string, GameObject)>() {
				(EKey.ME_UIS_MSG_UIS, $"{EKey.ME_UIS_MSG_UIS}", m_oUIDict[EKey.MID_EDITOR_UIS]),
				(EKey.ME_UIS_INFO_UIS, $"{EKey.ME_UIS_INFO_UIS}", m_oUIDict[EKey.MID_EDITOR_UIS]),
				(EKey.ME_UIS_EDITOR_MODE_UIS, $"{EKey.ME_UIS_EDITOR_MODE_UIS}", m_oUIDict[EKey.MID_EDITOR_UIS]),
				(EKey.LE_UIS_AB_SET_UIS, $"{EKey.LE_UIS_AB_SET_UIS}", m_oUIDict[EKey.LEFT_EDITOR_UIS])
			}, m_oUIDict);

			CFunc.SetupGameObjs(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.EDITOR_OBJ_ROOT, $"{EKey.EDITOR_OBJ_ROOT}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE))
			}, m_oObjDict);

			CFunc.SetupGameObjs(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.OBJ_ROOT, $"{EKey.OBJ_ROOT}", m_oObjDict[EKey.EDITOR_OBJ_ROOT], null)
			}, m_oObjDict);

			m_oUIDict[EKey.ME_UIS_MSG_UIS]?.SetActive(false);
			m_oObjDict[EKey.EDITOR_OBJ_ROOT]?.ExAddComponent<SpriteRenderer>();
			// 객체를 설정한다 }

			CSceneManager.ScreenDebugUIs?.SetActive(false);
		}
		#endregion // 함수
	}
}
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
