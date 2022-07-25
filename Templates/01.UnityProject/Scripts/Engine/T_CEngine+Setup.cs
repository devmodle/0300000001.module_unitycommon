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
		/** 레벨을 설정한다 */
		private void SetupLevel() {
			for(int i = 0; i < m_stParams.m_oLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
				for(int j = 0; j < m_stParams.m_oLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
					this.SetupCell(m_stParams.m_oLevelInfo.m_oCellInfoDictContainer[i][j]);
				}
			}
		}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
		#endregion			// 조건부 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
