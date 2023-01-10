#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
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
		/** 초기화 */
		private void SubAwake() {
			this.SetCellObjInfo(STCellObjInfo.INVALID);
		}

		/** 초기화 */
		private void SubInit() {
			this.SetupAbilityVals();
		}

		/** 제거 되었을 경우 */
		private void SubOnDestroy() {
			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEObj.SubOnDestroy Exception: {oException.Message}");
			}
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
