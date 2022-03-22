using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 - 설정 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 엔진을 설정한다 */
		private void SetupEngine() {
			// Do Something
		}
		#endregion			// 함수

		#region 조건부 함수
#if RUNTIME_TEMPLATES_MODULE_ENABLE
		/** 엔진을 설정한다 */
		private void SetupInit() {
			m_oBlockInfoDictContainers = new Dictionary<EBlockType, List<(EBlockKinds, CBlock)>>[m_stParams.m_oLevelInfo.NumCells.y, m_stParams.m_oLevelInfo.NumCells.x];
			this.GridInfo = Factory.MakeGridInfo(m_stParams.m_oLevelInfo, Vector3.zero);
		}
		
		/** 레벨을 설정한다 */
		private void SetupLevel() {
			for(int i = 0; i < m_stParams.m_oLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
				for(int j = 0; j < m_stParams.m_oLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
					// 셀 정보가 존재 할 경우
					if(m_stParams.m_oLevelInfo.m_oCellInfoDictContainer[i][j] != null) {
						this.SetupCell(m_stParams.m_oLevelInfo.m_oCellInfoDictContainer[i][j]);
					}
				}
			}
		}

		/** 셀을 설정한다 */
		private void SetupCell(CCellInfo a_oCellInfo) {
			var oBlockInfoDictContainer = new Dictionary<EBlockType, List<(EBlockKinds, CBlock)>>();

			foreach(var stKeyVal in a_oCellInfo.m_oBlockKindsDictContainer) {
				var oBlockInfoList = new List<(EBlockKinds, CBlock)>();

				for(int i = 0; i < stKeyVal.Value.Count; ++i) {
					// Do Something
				}

				oBlockInfoDictContainer.TryAdd(stKeyVal.Key, oBlockInfoList);
			}

			m_oBlockInfoDictContainers[a_oCellInfo.m_stIdx.y, a_oCellInfo.m_stIdx.x] = oBlockInfoDictContainer;
		}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
		#endregion			// 조건부 함수

		#region 추가 함수

		#endregion			// 추가 함수
	}
}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
