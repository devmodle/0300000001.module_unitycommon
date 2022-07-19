using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 플레이어 객체 */
	public partial class CEPlayerObj : CEObj {
		/** 매개 변수 */
		public new partial struct STParams {
			public CEObj.STParams m_stBaseParams;
			public CUserObjInfo m_oUserObjInfo;
		}

		#region 변수
		private STParams m_stParams;
		#endregion			// 변수

		#region 함수
		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			m_stParams = a_stParams;
		}
		#endregion			// 함수
	}

	/** 서브 플레이어 객체 */
	public partial class CEPlayerObj : CEObj {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 추가 변수

		#endregion			// 추가 변수

		#region 추가 함수
		/** 어빌리티 값을 설정한다 */
		public override void SetupAbilityVals() {
			base.SetupAbilityVals();
		}
		#endregion			// 추가 함수
	}	
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
