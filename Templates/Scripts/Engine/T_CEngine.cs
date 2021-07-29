using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if NEVER_USE_THIS
namespace SampleEngineName {
	//! 엔진
	public partial class CEngine : CComponent {
		//! 상태
		public enum EState {
			NONE = -1,
			PLAY,
			PAUSE,
			FINISH,
			MAX_VAL
		}

		//! 매개 변수
		public struct STParams {
			public CLevelInfo m_oLevelInfo;
			public CClearInfo m_oClearInfo;

			public GameObject m_oBlockObjs;
		}
		
		#region 변수
		private STParams m_stParams;
		private STGridInfo m_stGridInfo;

		private EState m_eState = EState.NONE;
		private Dictionary<EBlockKinds, GameObject>[,] m_oBlockDicts = null;
		#endregion			// 변수

		#region 프로퍼티
		public EState State => m_eState;
		public STGridInfo GridInfo => m_stGridInfo;
		public GameObject BlockObjs => m_stParams.m_oBlockObjs;
		#endregion			// 프로퍼티

		#region 함수
		//! 초기화
		public virtual void Init(STParams a_stParams) {
			m_stParams = a_stParams;

			this.SetupInit();
			this.SetupLevel();
		}

		//! 상태를 리셋한다
		public virtual void Reset() {
			// Do Something
		}

		//! 상태를 갱신한다
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		//! 제거 되었을 경우
		public override void OnDestroy() {
			base.OnDestroy();

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something	
			}
		}

		//! 터치를 시작했을 경우
		public void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		//! 터치를 움직였을 경우
		public void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		//! 터치를 종료했을 경우
		public void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}
		#endregion			// 함수

		#region 조건부 함수
#if UNITY_EDITOR
		//! GUI 를 그린다
		public virtual void OnGUI() {
			// Do Something
		}
		
		//! 가이드 라인을 그린다
		public virtual void OnDrawGizmos() {
			// Do Something
		}
#endif			// #if UNITY_EDITOR
		#endregion			// 조건부 함수
	}
}
#endif			// #if NEVER_USE_THIS
