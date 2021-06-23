using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if NEVER_USE_THIS
namespace SampleEngineName {
	//! 엔진
	public partial class CEngine : CComponent {
		//! 상태
		public enum EState {
			NONE = -1,
			MAX_VAL
		}

		//! 매개 변수
		public struct STParams {
			public CLevelInfo m_oLevelInfo;
		}
		
		#region 변수
		private STParams m_stParams;
		#endregion			// 변수

		#region 프로퍼티
		public EState State { get; private set; } = EState.NONE;
		#endregion			// 프로퍼티

		#region 함수
		//! 초기화
		public virtual void Init(STParams a_stParams) {
			m_stParams = a_stParams;
			this.SetupLevel();
		}

		//! 상태를 갱신한다
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);
		}

		//! 터치를 시작했을 경우
		public void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Nothing
		}

		//! 터치를 움직였을 경우
		public void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Nothing
		}

		//! 터치를 종료했을 경우
		public void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Nothing
		}
		#endregion			// 함수
	}
}
#endif			// #if NEVER_USE_THIS
