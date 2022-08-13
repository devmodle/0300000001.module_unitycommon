#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 객체 */
	public partial class CEObj : CEComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			OBJ_SPRITE,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEComponent.STParams m_stBaseParams;
			public STObjInfo m_stObjInfo;
			public CObjTargetInfo m_oObjTargetInfo;
		}

		#region 변수
		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>();
		#endregion			// 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public Vector3Int Idx { get; set; } = Vector3Int.zero;
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		public override void SetupAbilityVals() {
			base.SetupAbilityVals();

			// 객체 정보가 존재 할 경우
			if(this.Params.m_stObjInfo.m_eObjKinds.ExIsValid()) {
				global::Func.SetupAbilityVals(this.Params.m_stObjInfo, this.Params.m_oObjTargetInfo, this.AbilityValDict);
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
