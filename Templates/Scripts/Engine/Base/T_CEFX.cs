using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 효과 */
	public partial class CEFX : CEComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			FX_PARTICLE,
			[HideInInspector] MAX_VAL
		}
		
		/** 매개 변수 */
		public new partial struct STParams {
			public CEComponent.STParams m_stBaseParams;
			public STFXInfo m_stFXInfo;
		}

		#region 변수
		private STParams m_stParams;
		private Dictionary<EKey, ParticleSystem> m_oParticleDict = new Dictionary<EKey, ParticleSystem>();
		#endregion			// 변수

		#region 프로퍼티
		public STFXInfo FXInfo => m_stParams.m_stFXInfo;
		#endregion			// 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 파티클을 설정한다
			CFunc.SetupParticles(new List<(EKey, string, GameObject)>() {
				(EKey.FX_PARTICLE, $"{EKey.FX_PARTICLE}", this.gameObject)
			}, m_oParticleDict, false);
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			m_stParams = a_stParams;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
