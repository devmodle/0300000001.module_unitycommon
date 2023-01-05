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
#region 함수

#endregion // 함수
	}

	/** 서브 객체 */
	public partial class CEObj : CEObjComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

#region 변수

#endregion // 변수

#region 프로퍼티
		public STCellObjInfo CellObjInfo { get; private set; }
#endregion // 프로퍼티

#region 함수
		/** 컴포넌트를 설정한다 */
		private void SubSetupAwake() {
			this.SetCellObjInfo(STCellObjInfo.INVALID);
		}

		/** 초기화한다 */
		private void SubInit() {
			this.SetupAbilityVals();
		}

		/** 셀 객체 정보를 변경한다 */
		public void SetCellObjInfo(STCellObjInfo a_stCellObjInfo) {
			this.CellObjInfo = a_stCellObjInfo;
		}
#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
