#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 아이템 */
	public partial class CEItem : CEObjComponent {
		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STItemInfo m_stItemInfo;
			public CItemTargetInfo m_oItemTargetInfo;
		}

		#region 변수

		#endregion			// 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		public override void SetupAbilityVals() {
			base.SetupAbilityVals();
			this.OriginAbilityValDict.ExCopyTo(this.AbilityValDict, (a_dmAbilityVal) => a_dmAbilityVal);
		}

		/** 어빌리티 값을 갱신한다 */
		public override void UpdateAbilityVals() {
			base.UpdateAbilityVals();

			// 아이템 정보가 존재 할 경우
			if(this.Params.m_stItemInfo.m_eItemKinds.ExIsValid()) {
				global::Func.SetupAbilityVals(this.Params.m_stItemInfo, this.Params.m_oItemTargetInfo, this.OriginAbilityValDict);
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
