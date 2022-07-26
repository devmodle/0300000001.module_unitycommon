using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 */
	public partial class CEngine : CComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			SEL_GRID_INFO,
			SEL_PLAYER_OBJ,
			[HideInInspector] MAX_VAL
		}

		/** 상태 */
		public enum EState {
			NONE = -1,
			RUN,
			STOP,
			[HideInInspector] MAX_VAL
		}

		/** 콜백 */
		public enum ECallback {
			NONE = -1,
			CLEAR,
			CLEAR_FAIL,
			[HideInInspector] MAX_VAL
		}

		/** 엔진 상태 */
		public enum EEngineState {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public partial struct STParams {
			public GameObject m_oItemRoot;
			public GameObject m_oSkillRoot;
			public GameObject m_oObjRoot;
			public GameObject m_oFXRoot;

			public Dictionary<ECallback, System.Action<CEngine>> m_oCallbackDict;

#if RUNTIME_TEMPLATES_MODULE_ENABLE
			public CLevelInfo m_oLevelInfo;
			public CClearInfo m_oClearInfo;
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
		}

		#region 변수
		private STParams m_stParams;

		private Dictionary<EKey, STGridInfo> m_oGridInfoDict = new Dictionary<EKey, STGridInfo>() {
			[EKey.SEL_GRID_INFO] = default(STGridInfo)
		};

		private Dictionary<EKey, CEPlayerObj> m_oPlayerObjDict = new Dictionary<EKey, CEPlayerObj>() {
			[EKey.SEL_PLAYER_OBJ] = null
		};

		private List<CEItem> m_oItemList = new List<CEItem>();
		private List<CESkill> m_oSkillList = new List<CESkill>();
		private List<CEFX> m_oFXList = new List<CEFX>();
		private List<CEEnemyObj> m_oEnemyObjList = new List<CEEnemyObj>();
		private List<LineRenderer> m_oGridLineList = new List<LineRenderer>();

		/** =====> 객체 <===== */
		private Dictionary<EObjType, List<(EObjKinds, CEObj)>>[,] m_oObjInfoDictContainers = null;
		#endregion			// 변수

		#region 프로퍼티
		public long IntRecord { get; private set; } = 0;
		public double RealRecord { get; private set; } = 0.0;

		public EState State { get; private set; } = EState.NONE;
		public EEngineState EngineState { get; private set; } = EEngineState.NONE;

		public STGridInfo SelGridInfo => m_oGridInfoDict[EKey.SEL_GRID_INFO];
		public CEPlayerObj SelPlayerObj => m_oPlayerObjDict[EKey.SEL_PLAYER_OBJ];

		public GameObject ItemRoot => m_stParams.m_oItemRoot;
		public GameObject SkillRoot => m_stParams.m_oSkillRoot;
		public GameObject ObjRoot => m_stParams.m_oObjRoot;
		public GameObject FXRoot => m_stParams.m_oFXRoot;

#if RUNTIME_TEMPLATES_MODULE_ENABLE
		public CLevelInfo LevelInfo => m_stParams.m_oLevelInfo;
		public CClearInfo ClearInfo => m_stParams.m_oClearInfo;
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
		#endregion			// 프로퍼티
		
		#region 함수
		/** 터치를 시작했을 경우 */
		public void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			this.HandleTouchState(a_oSender, a_oEventData, ETouch.BEGIN);
		}

		/** 터치를 이동했을 경우 */
		public void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			this.HandleTouchState(a_oSender, a_oEventData, ETouch.MOVE);
		}

		/** 터치를 종료했을 경우 */
		public void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			this.HandleTouchState(a_oSender, a_oEventData, ETouch.END);
		}

		/** 플레이어 객체 이동을 처리한다 */
		public void MovePlayerObj(Vector3 a_stDirection) {
			// Do Something
		}

		/** 플레이어 객체 스킬을 적용한다 */
		public void ApplyPlayerObjSkill(CSkillInfo a_oSkillInfo) {
			// Do Something
		}

		/** 터치 이벤트를 처리한다 */
		private void HandleTouchState(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouch a_eTouch) {
			// 구동 상태 일 경우
			if(this.State == EState.RUN) {
				switch(a_eTouch) {
					case ETouch.BEGIN: this.HandleTouchBeginState(a_oEventData.ExGetLocalPos(m_stParams.m_oObjRoot)); break;
					case ETouch.MOVE: this.HandleTouchMoveState(a_oEventData.ExGetLocalPos(m_stParams.m_oObjRoot)); break;
					case ETouch.END: this.HandleTouchEndState(a_oEventData.ExGetLocalPos(m_stParams.m_oObjRoot)); break;
				}
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
