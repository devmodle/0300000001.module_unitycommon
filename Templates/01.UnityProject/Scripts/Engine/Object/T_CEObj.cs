#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 객체 */
	public partial class CEObj : CEObjComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			CELL_IDX,
			OBJ_SPRITE,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STObjInfo m_stObjInfo;
			public CObjTargetInfo m_oObjTargetInfo;
		}

		#region 변수

		#endregion			// 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public Vector3Int CellIdx => this.Vec3IntDict.GetValueOrDefault(EKey.CELL_IDX);

		/** =====> 기타 <===== */
		private Dictionary<EKey, Vector3Int> Vec3IntDict { get; } = new Dictionary<EKey, Vector3Int>();
		private Dictionary<EKey, SpriteRenderer> SpriteDict { get; } = new Dictionary<EKey, SpriteRenderer>();
		#endregion			// 프로퍼티

		#region 함수
		/** 어빌리티 값을 설정한다 */
		protected override void DoSetupAbilityVals(bool a_bIsReset = true) {
			base.DoSetupAbilityVals(a_bIsReset);

			// 객체 정보가 존재 할 경우
			if(this.Params.m_stObjInfo.m_eObjKinds.ExIsValid()) {
				global::Func.SetupAbilityVals(this.Params.m_stObjInfo, this.Params.m_oObjTargetInfo, this.AbilityValDictWrapper.m_oDict02);
			}
		}

		/** 셀 인덱스를 변경한다 */
		public void SetCellIdx(Vector3Int a_stCellIdx) {
			this.Vec3IntDict.ExReplaceVal(EKey.CELL_IDX, a_stCellIdx);
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
