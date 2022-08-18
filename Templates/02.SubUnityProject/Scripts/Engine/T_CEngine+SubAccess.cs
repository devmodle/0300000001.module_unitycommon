#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		
		#endregion			// 함수
	}

	/** 서브 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 상태를 변경한다 */
		public void SetState(EState a_eState) {
			this.State = (m_oStateCheckerDict.TryGetValue(a_eState, out System.Func<bool> oStateChecker) && oStateChecker()) ? a_eState : this.State;
		}

		/** 가까운 적 객체를 탐색한다 */
		public CEObj FindNearEnemyObj(Vector3 a_stPos) {
			var oEnemyObj = this.EnemyObjList.ExGetVal(KCDefine.B_VAL_0_INT, null);

			for(int i = 1; i < this.EnemyObjList.Count; ++i) {
				float fDistance = (a_stPos - oEnemyObj.transform.localPosition).sqrMagnitude;
				oEnemyObj = fDistance.ExIsLessEquals((oEnemyObj.transform.localPosition - this.EnemyObjList[i].transform.localPosition).sqrMagnitude) ? oEnemyObj : this.EnemyObjList[i];
			}

			return oEnemyObj;
		}

		/** 가까운 적 객체를 탐색한다 */
		public List<CEObj> FindNearEnemyObjs(Vector3 a_stPos, float a_fDistance, List<CEObj> a_oOutEnemyObjList) {
			a_oOutEnemyObjList = a_oOutEnemyObjList ?? new List<CEObj>();
			
			for(int i = 0; i < this.EnemyObjList.Count; ++i) {
				float fDistance = (a_stPos - this.EnemyObjList[i].transform.localPosition).sqrMagnitude;

				// 범위 안에 존재 할 경우
				if(fDistance.ExIsLessEquals(Mathf.Pow(a_fDistance, KCDefine.B_VAL_2_REAL))) {
					a_oOutEnemyObjList.ExAddVal(this.EnemyObjList[i]);
				}
			}

			return a_oOutEnemyObjList;
		}

		/** 무효 상태 가능 여부를 검사한다 */
		private bool IsEnableNoneState() {
			return true;
		}

		/** 플레이 상태 가능 여부를 검사한다 */
		private bool IsEnablePlayState() {
			return true;
		}

		/** 정지 상태 가능 여부를 검사한다 */
		private bool IsEnablePauseState() {
			return true;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
