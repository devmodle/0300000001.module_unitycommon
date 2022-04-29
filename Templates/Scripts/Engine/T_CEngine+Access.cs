using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 블럭 정보를 반환한다 */
		public (EBlockKinds, CEBlock) FindBlockInfo(EBlockType a_eBlockType, EBlockKinds a_eBlockKinds, Vector3Int a_stIdx) {
			bool bIsValid = this.TryFindBlockInfo(a_eBlockType, a_eBlockKinds, a_stIdx, out (EBlockKinds, CEBlock) stBlockInfo);
			CAccess.Assert(bIsValid);

			return stBlockInfo;
		}

		/** 블럭 정보를 반환한다 */
		public List<(EBlockKinds, CEBlock)> FindBlockInfos(EBlockType a_eBlockType, Vector3Int a_stIdx) {
			return m_oBlockInfoDictContainers.ExIsValidIdx(a_stIdx) ? m_oBlockInfoDictContainers[a_stIdx.y, a_stIdx.x].GetValueOrDefault(a_eBlockType) : null;
		}

		/** 최상단 블럭 정보를 반환한다 */
		public (EBlockKinds, CEBlock) FindTopBlockInfo(EBlockKinds a_eBlockKinds, Vector3Int a_stIdx) {
			for(int i = (int)EBlockType.MAX_VAL - KCDefine.B_VAL_1_INT; i > (int)EBlockType.NONE; --i) {
				var oBlockInfo = this.FindBlockInfo((EBlockType)i, a_eBlockKinds, a_stIdx);

				// 블럭 정보가 존재 할 경우
				if(!oBlockInfo.Equals(KDefine.E_INVALID_BLOCK_INFO)) {
					return oBlockInfo;
				}
			}

			return KDefine.E_INVALID_BLOCK_INFO;
		}

		/** 최상단 블럭 정보를 반환한다 */
		public List<(EBlockKinds, CEBlock)> FindTopBlockInfos(Vector3Int a_stIdx) {
			for(int i = (int)EBlockType.MAX_VAL - KCDefine.B_VAL_1_INT; i > (int)EBlockType.NONE; --i) {
				var oBlockInfoList = this.FindBlockInfos((EBlockType)i, a_stIdx);

				// 블럭 정보가 존재 할 경우
				if(oBlockInfoList != null) {
					return oBlockInfoList;
				}
			}

			return null;
		}

		/** 블럭 정보를 반환한다 */
		public bool TryFindBlockInfo(EBlockType a_eBlockType, EBlockKinds a_eBlockKinds, Vector3Int a_stIdx, out (EBlockKinds, CEBlock) a_oOutBlockInfo) {
			// 블럭 정보가 존재 할 경우
			if(this.TryFindBlockInfos(a_eBlockType, a_stIdx, out List<(EBlockKinds, CEBlock)> oBlockInfoList)) {
				a_oOutBlockInfo = oBlockInfoList.ExGetVal((a_oBlockInfo) => a_oBlockInfo.Item1 == a_eBlockKinds, KDefine.E_INVALID_BLOCK_INFO);
				return true;
			}

			a_oOutBlockInfo = KDefine.E_INVALID_BLOCK_INFO;
			return false;
		}

		/** 블럭 정보를 반환한다 */
		public bool TryFindBlockInfos(EBlockType a_eBlockType, Vector3Int a_stIdx, out List<(EBlockKinds, CEBlock)> a_oOutBlockInfoList) {
			a_oOutBlockInfoList = m_oBlockInfoDictContainers.ExGetVal(a_stIdx, null)?.GetValueOrDefault(a_eBlockType);
			return a_oOutBlockInfoList != null;
		}

		/** 최상단 블럭 정보를 반환한다 */
		public bool TryFindTopBlockInfo(EBlockKinds a_eBlockKinds, Vector3Int a_stIdx, out (EBlockKinds, CEBlock) a_oOutTopBlockInfo) {
			for(int i = (int)EBlockType.MAX_VAL - KCDefine.B_VAL_1_INT; i > (int)EBlockType.NONE; --i) {
				// 블럭 정보가 존재 할 경우
				if(this.TryFindBlockInfo((EBlockType)i, a_eBlockKinds, a_stIdx, out a_oOutTopBlockInfo)) {
					return true;
				}
			}

			a_oOutTopBlockInfo = KDefine.E_INVALID_BLOCK_INFO;
			return false;
		}

		/** 블럭 정보를 반환한다 */
		public bool TryFindTopBlockInfos(Vector3Int a_stIdx, out List<(EBlockKinds, CEBlock)> a_oOutTopBlockInfoList) {
			for(int i = (int)EBlockType.MAX_VAL - KCDefine.B_VAL_1_INT; i > (int)EBlockType.NONE; --i) {
				// 블럭 정보가 존재 할 경우
				if(this.TryFindBlockInfos((EBlockType)i, a_stIdx, out a_oOutTopBlockInfoList)) {
					return true;
				}
			}

			a_oOutTopBlockInfoList = null;
			return false;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
