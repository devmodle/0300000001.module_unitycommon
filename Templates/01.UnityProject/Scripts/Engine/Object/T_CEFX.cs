#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 효과 */
	public partial class CEFX : CEObjComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			FX_PARTICLE,
			[HideInInspector] MAX_VAL
		}
		
		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STFXInfo m_stTableFXInfo;
		}

		#region 프로퍼티
		public new STParams Params { get; private set; }
		private Dictionary<EKey, ParticleSystem> ParticleDict { get; } = new Dictionary<EKey, ParticleSystem>();
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		protected override void DoSetupAbilityVals(bool a_bIsReset = true) {
			base.DoSetupAbilityVals(a_bIsReset);
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
