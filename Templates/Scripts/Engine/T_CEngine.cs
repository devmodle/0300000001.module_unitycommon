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
			INT_RECORD,
			REAL_RECORD,
			SEL_GRID_INFO,
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
			public GameObject m_oObjRoot;
			public GameObject m_oFXObjRoot;
			public GameObject m_oSkillObjRoot;

			public Dictionary<ECallback, System.Action<CEngine>> m_oCallbackDict;

#if RUNTIME_TEMPLATES_MODULE_ENABLE
			public CLevelInfo m_oLevelInfo;
			public CClearInfo m_oClearInfo;
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
		}

		#region 변수
		private STParams m_stParams;
		private Dictionary<EKey, string> m_oStrDict = new Dictionary<EKey, string>();

		private Dictionary<EKey, STGridInfo> m_oGridInfoDict = new Dictionary<EKey, STGridInfo>() {
			[EKey.SEL_GRID_INFO] = default(STGridInfo)
		};

		private List<LineRenderer> m_oGridLineList = new List<LineRenderer>();

		/** =====> 객체 <===== */
		private Dictionary<EObjType, List<(EObjKinds, CEObj)>>[,] m_oObjInfoDictContainers = null;
		#endregion			// 변수

		#region 프로퍼티
		public long IntRecord { get { return long.Parse(m_oStrDict.GetValueOrDefault(EKey.INT_RECORD, KCDefine.B_STR_0_INT)); } set { m_oStrDict.ExReplaceVal(EKey.INT_RECORD, $"{value}"); } }
		public double RealRecord { get { return double.Parse(m_oStrDict.GetValueOrDefault(EKey.REAL_RECORD, KCDefine.B_STR_0_REAL)); } set { m_oStrDict.ExReplaceVal(EKey.REAL_RECORD, $"{value}"); } }

		public EState State { get; private set; } = EState.NONE;
		public EEngineState EngineState { get; private set; } = EEngineState.NONE;

		public STGridInfo SelGridInfo => m_oGridInfoDict[EKey.SEL_GRID_INFO];

		public GameObject ObjRoot => m_stParams.m_oObjRoot;
		public GameObject FXObjRoot => m_stParams.m_oFXObjRoot;
		public GameObject SkillObjRoot => m_stParams.m_oSkillObjRoot;
		#endregion			// 프로퍼티
		
		#region 함수
		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			m_stParams = a_stParams;

#if RUNTIME_TEMPLATES_MODULE_ENABLE
			this.SetupEngine();
			this.SetupLevel();
			this.SetupGridLine();
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
		}

		/** 터치를 시작했을 경우 */
		public void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			this.HandleTouchState(a_oSender, a_oEventData, ETouch.BEGIN);
		}

		/** 터치를 움직였을 경우 */
		public void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			this.HandleTouchState(a_oSender, a_oEventData, ETouch.MOVE);
		}

		/** 터치를 종료했을 경우 */
		public void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			this.HandleTouchState(a_oSender, a_oEventData, ETouch.END);
		}
		#endregion			// 함수
	}

	/** 서브 엔진 */
	public partial class CEngine : CComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			MOVE_DIRECTION,
			SEL_PLAYER_OBJ,
			[HideInInspector] MAX_VAL
		}

		#region 추가 변수
		private List<CEEnemyObj> m_oSubEnemyObjList = new List<CEEnemyObj>();
		private List<CEFX> m_oSubFXList = new List<CEFX>();

		private Dictionary<ESubKey, Vector3> m_oSubVec3Dict = new Dictionary<ESubKey, Vector3>() {
			[ESubKey.MOVE_DIRECTION] = Vector3.zero
		};

		private Dictionary<ESubKey, CEPlayerObj> m_oSubPlayerObjList = new Dictionary<ESubKey, CEPlayerObj>() {
			[ESubKey.SEL_PLAYER_OBJ] = null
		};
		#endregion			// 추가 변수

		#region 추가 함수
		/** 상태를 리셋한다 */
		public override void Reset() {
			base.Reset();
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning && this.State == EState.RUN) {
				switch(this.EngineState) {
					// Do Something
				}
			}
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEngine.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 플레이어 객체 이동을 처리한다 */
		public void MovePlayerObj(Vector3 a_stDirection) {
			m_oSubVec3Dict[ESubKey.MOVE_DIRECTION] = a_stDirection;
		}

		/** 구동 상태를 처리한다 */
		private void HandleRunState() {
			// Do Something
		}

		/** 중지 상태를 처리한다 */
		private void HandleStopState() {
			// Do Something
		}

		/** 터치 이벤트를 처리한다 */
		private void HandleTouchState(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouch a_eTouch) {
			// 구동 상태 일 경우
			if(this.State == EState.RUN) {
				var stTouchPos = a_oEventData.ExGetLocalPos(m_stParams.m_oObjRoot);

				// 그리드 영역 일 경우
				if(m_oGridInfoDict[EKey.SEL_GRID_INFO].m_stBounds.Contains(stTouchPos)) {
					var stIdx = stTouchPos.ExToIdx(m_oGridInfoDict[EKey.SEL_GRID_INFO].m_stPivotPos, KDefine.E_SIZE_CELL);
					CAccess.Assert(m_oObjInfoDictContainers.ExIsValidIdx(stIdx));
				}
			}
		}
		#endregion			// 추가 함수

		#region 추가 조건부 함수
#if UNITY_EDITOR
		/** 기즈모를 그린다 */
		public virtual void OnDrawGizmos() {
			// 메인 카메라가 존재 할 경우
			if(CSceneManager.IsExistsMainCamera) {
				// Do Something
			}
		}
#endif			// #if UNITY_EDITOR
		#endregion			// 추가 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
