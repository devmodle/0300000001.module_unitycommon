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
		/** 셀 객체를 탐색한다 */
		public CEObj FindCellObj(EObjType a_eObjType, EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			return this.TryFindCellObj(a_eObjType, a_eObjKinds, a_stIdx, out CEObj oCellObj) ? oCellObj : null;
		}

		/** 셀 객체를 탐색한다 */
		public List<CEObj> FindCellObjs(EObjType a_eObjType, Vector3Int a_stIdx) {
			return this.TryFindCellObjs(a_eObjType, a_stIdx, out List<CEObj> oCellObjList) ? oCellObjList : null;
		}

		/** 최상단 셀 객체를 탐색한다 */
		public CEObj FindTopCellObj(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				var oCellObj = this.FindCellObj(eObjType, a_eObjKinds, a_stIdx);

				// 객체가 존재 할 경우
				if(oCellObj != null) {
					return oCellObj;
				}
			}

			return null;
		}

		/** 최상단 셀 객체를 탐색한다 */
		public List<CEObj> FindTopCellObjs(Vector3Int a_stIdx) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				var oCellObjList = this.FindCellObjs(eObjType, a_stIdx);

				// 객체 정보가 존재 할 경우
				if(oCellObjList != null) {
					return oCellObjList;
				}
			}

			return null;
		}

		/** 셀 객체를 탐색한다 */
		public bool TryFindCellObj(EObjType a_eObjType, EObjKinds a_eObjKinds, Vector3Int a_stIdx, out CEObj a_oOutCellObj) {
			a_oOutCellObj = this.TryFindCellObjs(a_eObjType, a_stIdx, out List<CEObj> oCellObjList) ? oCellObjList.ExGetVal((a_oCellObj) => a_oCellObj.Params.m_stObjInfo.m_eObjKinds == a_eObjKinds, null) : null;
			return a_oOutCellObj != null;
		}

		/** 셀 객체를 탐색한다 */
		public bool TryFindCellObjs(EObjType a_eObjType, Vector3Int a_stIdx, out List<CEObj> a_oOutCellObjList) {
			a_oOutCellObjList = this.CellObjDictContainers.ExGetVal(a_stIdx, null)?.GetValueOrDefault(a_eObjType);
			return a_oOutCellObjList != null;
		}

		/** 최상단 셀 객체를 탐색한다 */
		public bool TryFindTopCellObj(EObjKinds a_eObjKinds, Vector3Int a_stIdx, out CEObj a_oOutTopCellObj) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				// 객체 정보가 존재 할 경우
				if(this.TryFindCellObj(eObjType, a_eObjKinds, a_stIdx, out a_oOutTopCellObj)) {
					return true;
				}
			}

			a_oOutTopCellObj = null;
			return false;
		}

		/** 셀 객체를 탐색한다 */
		public bool TryFindTopCellObjs(Vector3Int a_stIdx, out List<CEObj> a_oOutTopCellObjList) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				// 객체 정보가 존재 할 경우
				if(this.TryFindCellObjs(eObjType, a_stIdx, out a_oOutTopCellObjList)) {
					return true;
				}
			}

			a_oOutTopCellObjList = null;
			return false;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
