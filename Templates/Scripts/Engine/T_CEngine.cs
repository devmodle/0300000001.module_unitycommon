using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if SCRIPT_TEMPLATE_ONLY
#if ENGINE_TEMPLATES_MODULE_ENABLE
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

		/** 매개 변수 */
		public struct STParams {
			public GameObject m_oBlockObjs;
			public Dictionary<ECallback, System.Action<CEngine>> m_oCallbackDict;

#if RUNTIME_TEMPLATES_MODULE_ENABLE
			public CLevelInfo m_oLevelInfo;
			public CClearInfo m_oClearInfo;
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
		}

		#region 변수
		private STParams m_stParams;
		private List<LineRenderer> m_oGridLineList = new List<LineRenderer>();

		/** =====> 객체 <===== */
		private Dictionary<EBlockKinds, GameObject>[,] m_oBlockDicts = null;
		#endregion			// 변수

		#region 프로퍼티
		public long IntRecord { get; private set; } = 0;
		public double RealRecord { get; private set; } = 0.0;

		public EState State { get; private set; } = EState.NONE;
		public STGridInfo GridInfo { get; private set; }
		
		public GameObject BlockObjs => m_stParams.m_oBlockObjs;
		#endregion			// 프로퍼티

		#region 추가 변수

		#endregion			// 추가 변수

		#region 추가 프로퍼티
		
		#endregion			// 추가 프로퍼티

		#region 함수
		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			m_stParams = a_stParams;

#if RUNTIME_TEMPLATES_MODULE_ENABLE
			this.SetupInit();
			this.SetupLevel();
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
				if(CSceneManager.IsAwake || CSceneManager.IsAppRunning) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEngine.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 터치를 시작했을 경우 */
		public void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 실행 상태 일 경우
			if(this.State == EState.RUN) {
				var stTouchPos = a_oEventData.ExGetLocalPos(m_stParams.m_oBlockObjs);
			}
		}

		/** 터치를 움직였을 경우 */
		public void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 실행 상태 일 경우
			if(this.State == EState.RUN) {
				var stTouchPos = a_oEventData.ExGetLocalPos(m_stParams.m_oBlockObjs);
			}
		}

		/** 터치를 종료했을 경우 */
		public void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 실행 상태 일 경우
			if(this.State == EState.RUN) {
				var stTouchPos = a_oEventData.ExGetLocalPos(m_stParams.m_oBlockObjs);
			}
		}

		/** 엔진을 실행한다 */
		public void Run() {
			this.State = EState.RUN;
		}

		/** 엔진을 중지한다 */
		public void Stop() {
			this.State = EState.STOP;
		}
		#endregion			// 함수

		#region 조건부 함수
#if UNITY_EDITOR
		/** GUI 를 그린다 */
		public virtual void OnGUI() {
			// Do Something
		}
		
		/** 기즈모를 그린다 */
		public virtual void OnDrawGizmos() {
			// 앱 실행 중이 아닐 경우
			if(!Application.isPlaying) {
				// Do Something
			}
		}
#endif			// #if UNITY_EDITOR
		#endregion			// 조건부 함수

		#region 추가 함수

		#endregion			// 추가 함수
	}
}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
