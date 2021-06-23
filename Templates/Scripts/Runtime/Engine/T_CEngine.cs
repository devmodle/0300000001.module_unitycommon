using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
namespace SampleEngineName {
	//! 엔진
	public partial class CEngine : CComponent {
		//! 매개 변수
		public struct STParams {
			public CLevelInfo m_oLevelInfo;
		}

		#region 변수
		private STParams m_stParams;
		#endregion			// 변수

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
		#endregion			// 함수
	}
}
#endif			// #if NEVER_USE_THIS
