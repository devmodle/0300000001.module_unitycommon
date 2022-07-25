using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 객체 정보를 반환한다 */
		public (EObjKinds, CEObj) FindObjInfo(EObjType a_eObjType, EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			bool bIsValid = this.TryFindObjInfo(a_eObjType, a_eObjKinds, a_stIdx, out (EObjKinds, CEObj) stObjInfo);
			CAccess.Assert(bIsValid);

			return stObjInfo;
		}

		/** 객체 정보를 반환한다 */
		public List<(EObjKinds, CEObj)> FindObjInfos(EObjType a_eObjType, Vector3Int a_stIdx) {
			return m_oObjInfoDictContainers.ExIsValidIdx(a_stIdx) ? m_oObjInfoDictContainers[a_stIdx.y, a_stIdx.x].GetValueOrDefault(a_eObjType) : null;
		}

		/** 최상단 객체 정보를 반환한다 */
		public (EObjKinds, CEObj) FindTopObjInfo(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				var oObjInfo = this.FindObjInfo(eObjType, a_eObjKinds, a_stIdx);

				// 객체 정보가 존재 할 경우
				if(!oObjInfo.Equals(KDefine.E_INVALID_OBJ_INFO)) {
					return oObjInfo;
				}
			}

			return KDefine.E_INVALID_OBJ_INFO;
		}

		/** 최상단 객체 정보를 반환한다 */
		public List<(EObjKinds, CEObj)> FindTopObjInfos(Vector3Int a_stIdx) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				var oObjInfoList = this.FindObjInfos(eObjType, a_stIdx);

				// 객체 정보가 존재 할 경우
				if(oObjInfoList != null) {
					return oObjInfoList;
				}
			}

			return null;
		}

		/** 객체 정보를 반환한다 */
		public bool TryFindObjInfo(EObjType a_eObjType, EObjKinds a_eObjKinds, Vector3Int a_stIdx, out (EObjKinds, CEObj) a_oOutObjInfo) {
			// 객체 정보가 존재 할 경우
			if(this.TryFindObjInfos(a_eObjType, a_stIdx, out List<(EObjKinds, CEObj)> oObjInfoList)) {
				a_oOutObjInfo = oObjInfoList.ExGetVal((a_oObjInfo) => a_oObjInfo.Item1 == a_eObjKinds, KDefine.E_INVALID_OBJ_INFO);
				return true;
			}

			a_oOutObjInfo = KDefine.E_INVALID_OBJ_INFO;
			return false;
		}

		/** 객체 정보를 반환한다 */
		public bool TryFindObjInfos(EObjType a_eObjType, Vector3Int a_stIdx, out List<(EObjKinds, CEObj)> a_oOutObjInfoList) {
			a_oOutObjInfoList = m_oObjInfoDictContainers.ExGetVal(a_stIdx, null)?.GetValueOrDefault(a_eObjType);
			return a_oOutObjInfoList != null;
		}

		/** 최상단 객체 정보를 반환한다 */
		public bool TryFindTopObjInfo(EObjKinds a_eObjKinds, Vector3Int a_stIdx, out (EObjKinds, CEObj) a_oOutTopObjInfo) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				// 객체 정보가 존재 할 경우
				if(this.TryFindObjInfo(eObjType, a_eObjKinds, a_stIdx, out a_oOutTopObjInfo)) {
					return true;
				}
			}

			a_oOutTopObjInfo = KDefine.E_INVALID_OBJ_INFO;
			return false;
		}

		/** 객체 정보를 반환한다 */
		public bool TryFindTopObjInfos(Vector3Int a_stIdx, out List<(EObjKinds, CEObj)> a_oOutTopObjInfoList) {
			for(var eObjType = EObjType.MAX_VAL - KCDefine.B_VAL_1_INT; eObjType > EObjType.NONE; --eObjType) {
				// 객체 정보가 존재 할 경우
				if(this.TryFindObjInfos(eObjType, a_stIdx, out a_oOutTopObjInfoList)) {
					return true;
				}
			}

			a_oOutTopObjInfoList = null;
			return false;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
