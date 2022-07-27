#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
		}

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
		#endregion			// 함수
	}

	/** 서브 엔진 */
	public partial class CEngine : CComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		
		#endregion			// 변수

		#region 프로퍼티

		#endregion			// 프로퍼티

		#region 함수
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

		/** 구동 상태를 처리한다 */
		private void HandleRunState() {
			// Do Something
		}

		/** 중지 상태를 처리한다 */
		private void HandleStopState() {
			// Do Something
		}

		/** 터치 시작 상태를 처리한다 */
		private void HandleTouchBeginState(Vector3 a_stTouchPos) {
			// 그리드 영역 일 경우
			if(m_oGridInfoDict[EKey.SEL_GRID_INFO].m_stBounds.Contains(a_stTouchPos)) {
				var stIdx = a_stTouchPos.ExToIdx(m_oGridInfoDict[EKey.SEL_GRID_INFO].m_stPivotPos, KDefine.E_SIZE_CELL);
			}
		}

		/** 터치 이동 상태를 처리한다 */
		private void HandleTouchMoveState(Vector3 a_stTouchPos) {
			// 그리드 영역 일 경우
			if(m_oGridInfoDict[EKey.SEL_GRID_INFO].m_stBounds.Contains(a_stTouchPos)) {
				var stIdx = a_stTouchPos.ExToIdx(m_oGridInfoDict[EKey.SEL_GRID_INFO].m_stPivotPos, KDefine.E_SIZE_CELL);
			}
		}

		/** 터치 종료 상태를 처리한다 */
		private void HandleTouchEndState(Vector3 a_stTouchPos) {
			// 그리드 영역 일 경우
			if(m_oGridInfoDict[EKey.SEL_GRID_INFO].m_stBounds.Contains(a_stTouchPos)) {
				var stIdx = a_stTouchPos.ExToIdx(m_oGridInfoDict[EKey.SEL_GRID_INFO].m_stPivotPos, KDefine.E_SIZE_CELL);
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
