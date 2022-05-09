using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 */
	public partial class CEngine : CComponent {
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
		private enum EEngineState {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public struct STParams {
			public GameObject m_oFXObjs;
			public GameObject m_oBlockObjs;

			public Dictionary<ECallback, System.Action<CEngine>> m_oCallbackDict;

#if RUNTIME_TEMPLATES_MODULE_ENABLE
			public CLevelInfo m_oLevelInfo;
			public CClearInfo m_oClearInfo;
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
		}

		#region 변수
		private STParams m_stParams;
		private EEngineState m_eEngineState = EEngineState.NONE;
		private List<LineRenderer> m_oGridLineList = new List<LineRenderer>();

		/** =====> 객체 <===== */
		private Dictionary<EBlockType, List<(EBlockKinds, CEBlock)>>[,] m_oBlockInfoDictContainers = null;
		#endregion			// 변수

		#region 프로퍼티
		public long IntRecord { get; private set; } = 0;
		public double RealRecord { get; private set; } = 0.0;
		public EState State { get; private set; } = EState.NONE;
		public STGridInfo GridInfo { get; private set; }
		
		public GameObject FXObjs => m_stParams.m_oFXObjs;
		public GameObject BlockObjs => m_stParams.m_oBlockObjs;
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

		/** 상태를 리셋한다 */
		public override void Reset() {
			base.Reset();
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
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

		/** 상태를 변경한다 */
		public void SetState(EState a_eState) {
			this.State = a_eState;

			switch(a_eState) {
				case EState.RUN: this.HandleRunState(); break;
				case EState.STOP: this.HandleStopState(); break;
			}
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
			switch(this.State) {
				case EState.RUN: {
					var stTouchPos = a_oEventData.ExGetLocalPos(m_stParams.m_oBlockObjs);

					// 그리드 영역 일 경우
					if(this.GridInfo.m_stBounds.Contains(stTouchPos)) {
						var stIdx = stTouchPos.ExToIdx(this.GridInfo.m_stPivotPos, KDefine.E_SIZE_CELL);
						CAccess.Assert(m_oBlockInfoDictContainers.ExIsValidIdx(stIdx));
					}
				} break;
			}
		}
		#endregion			// 함수

		#region 조건부 함수
#if UNITY_EDITOR
		/** 기즈모를 그린다 */
		public virtual void OnDrawGizmos() {
			// 메인 카메라가 존재 할 경우
			if(CSceneManager.IsExistsMainCamera) {
				// Do Something
			}
		}
#endif			// #if UNITY_EDITOR
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
