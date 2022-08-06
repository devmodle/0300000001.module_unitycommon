#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 컴포넌트 */
	public abstract partial class CEComponent : CComponent {
		/** 매개 변수 */
		public partial struct STParams {
			public string m_oObjsPoolKey;
			public CEngine m_oEngine;
			public CEComponent m_oOwner;
			public CEController m_oController;
		}

		#region 변수
		
		#endregion			// 변수
		
		#region 프로퍼티
		public STParams Params { get; private set; }
		public Dictionary<EAbilityKinds, decimal> AbilityValDict { get; private set; } = new Dictionary<EAbilityKinds, decimal>();
		#endregion			// 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);
			this.Params.m_oController?.OnUpdate(a_fDeltaTime);
		}

		/** 어빌리티 값을 설정한다 */
		public virtual void SetupAbilityVals() {
			this.AbilityValDict.Clear();
		}
		#endregion			// 함수

		#region 제네릭 함수
		/** 소유자를 반환한다 */
		public T GetOwner<T>() where T : CEComponent {
			return this.Params.m_oOwner as T;
		}

		/** 제어자를 반환한다 */
		public T GetController<T>() where T : CEController {
			return this.Params.m_oController as T;
		}
		#endregion			// 제네릭 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
