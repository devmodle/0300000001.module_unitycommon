#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 객체를 반환한다 */
		public CEObj FindObj(EObjType a_eObjType, EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			bool bIsValid = this.TryFindObj(a_eObjType, a_eObjKinds, a_stIdx, out CEObj oObj);
			CAccess.Assert(bIsValid);

			return oObj;
		}

		/** 객체를 반환한다 */
		public List<CEObj> FindObjs(EObjType a_eObjType, Vector3Int a_stIdx) {
			return m_oObjDictContainers.ExIsValidIdx(a_stIdx) ? m_oObjDictContainers[a_stIdx.y, a_stIdx.x].GetValueOrDefault(a_eObjType) : null;
		}

		/** 최상단 객체를 반환한다 */
		public CEObj FindTopObj(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				var oObj = this.FindObj(eObjType, a_eObjKinds, a_stIdx);

				// 객체가 존재 할 경우
				if(oObj != null) {
					return oObj;
				}
			}

			return null;
		}

		/** 최상단 객체를 반환한다 */
		public List<CEObj> FindTopObjs(Vector3Int a_stIdx) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				var oObjList = this.FindObjs(eObjType, a_stIdx);

				// 객체 정보가 존재 할 경우
				if(oObjList != null) {
					return oObjList;
				}
			}

			return null;
		}

		/** 객체를 반환한다 */
		public bool TryFindObj(EObjType a_eObjType, EObjKinds a_eObjKinds, Vector3Int a_stIdx, out CEObj a_oOutObj) {
			a_oOutObj = this.TryFindObjs(a_eObjType, a_stIdx, out List<CEObj> oObjList) ? oObjList.ExGetVal((a_oObj) => a_oObj.Params.m_stObjInfo.m_eObjKinds == a_eObjKinds, null) : null;
			return false;
		}

		/** 객체를 반환한다 */
		public bool TryFindObjs(EObjType a_eObjType, Vector3Int a_stIdx, out List<CEObj> a_oOutObjList) {
			a_oOutObjList = m_oObjDictContainers.ExGetVal(a_stIdx, null)?.GetValueOrDefault(a_eObjType);
			return a_oOutObjList != null;
		}

		/** 최상단 객체를 반환한다 */
		public bool TryFindTopObj(EObjKinds a_eObjKinds, Vector3Int a_stIdx, out CEObj a_oOutTopObj) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				// 객체 정보가 존재 할 경우
				if(this.TryFindObj(eObjType, a_eObjKinds, a_stIdx, out a_oOutTopObj)) {
					return true;
				}
			}

			a_oOutTopObj = null;
			return false;
		}

		/** 객체를 반환한다 */
		public bool TryFindTopObjs(Vector3Int a_stIdx, out List<CEObj> a_oOutTopObjList) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				// 객체 정보가 존재 할 경우
				if(this.TryFindObjs(eObjType, a_stIdx, out a_oOutTopObjList)) {
					return true;
				}
			}

			a_oOutTopObjList = null;
			return false;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
