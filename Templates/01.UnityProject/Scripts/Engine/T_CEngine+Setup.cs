#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 - 설정 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 레벨을 설정한다 */
		private void SetupLevel() {
			for(int i = 0; i < this.Params.m_oLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
				for(int j = 0; j < this.Params.m_oLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
					this.SetupCell(this.Params.m_oLevelInfo.m_oCellInfoDictContainer[i][j]);
				}
			}
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
