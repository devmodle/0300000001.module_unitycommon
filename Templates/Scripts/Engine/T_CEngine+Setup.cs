using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 - 설정 */
	public partial class CEngine : CComponent {
		#region 함수

		#endregion			// 함수

		#region 조건부 함수
#if RUNTIME_TEMPLATES_MODULE_ENABLE
		/** 엔진을 설정한다 */
		private void SetupEngine() {
			m_oObjInfoDictContainers = new Dictionary<EObjType, List<(EObjKinds, CEObj)>>[m_stParams.m_oLevelInfo.NumCells.y, m_stParams.m_oLevelInfo.NumCells.x];
			m_oGridInfoDict[EKey.GRID_INFO] = Factory.MakeGridInfo(m_stParams.m_oLevelInfo, Vector3.zero);
		}
		
		/** 레벨을 설정한다 */
		private void SetupLevel() {
			for(int i = 0; i < m_stParams.m_oLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
				for(int j = 0; j < m_stParams.m_oLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
					this.SetupCell(m_stParams.m_oLevelInfo.m_oCellInfoDictContainer[i][j]);
				}
			}
		}

		/** 셀을 설정한다 */
		private void SetupCell(STCellInfo a_stCellInfo) {
			var oObjInfoDictContainer = new Dictionary<EObjType, List<(EObjKinds, CEObj)>>();

			foreach(var stKeyVal in a_stCellInfo.m_oObjKindsDictContainer) {
				var oObjInfoList = new List<(EObjKinds, CEObj)>();

				for(int i = 0; i < stKeyVal.Value.Count; ++i) {
					// Do Something
				}

				oObjInfoDictContainer.TryAdd(stKeyVal.Key, oObjInfoList);
			}

			m_oObjInfoDictContainers[a_stCellInfo.m_stIdx.y, a_stCellInfo.m_stIdx.x] = oObjInfoDictContainer;
		}

		/** 그리드 라인을 설정한다 */
		private void SetupGridLine() {
			// Do Something
		}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
