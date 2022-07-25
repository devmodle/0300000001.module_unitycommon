using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
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
		public new partial struct STParams {
			public CEComponent.STParams m_stBaseParams;
			public STObjInfo m_stObjInfo;
			public CObjInfo m_oObjInfo;
		}

		#region 변수
		private STParams m_stParams;
		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>();
		#endregion			// 변수

		#region 프로퍼티
		public Vector3Int Idx { get; set; } = Vector3Int.zero;
		public STObjInfo ObjInfo => m_stParams.m_stObjInfo;
		public CObjInfo ClsObjInfo => m_stParams.m_oObjInfo;
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		public override void SetupAbilityVals() {
			base.SetupAbilityVals();
			var oAbilityTargetInfoDict = (m_stParams.m_oObjInfo != null) ? m_stParams.m_oObjInfo.m_oAbilityTargetInfoDict : m_stParams.m_stObjInfo.m_oAbilityTargetInfoDict;

			foreach(var stKeyVal in oAbilityTargetInfoDict) {
				global::Func.SetupAbilityVals(stKeyVal.Value, this.AbilityValDict);
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
