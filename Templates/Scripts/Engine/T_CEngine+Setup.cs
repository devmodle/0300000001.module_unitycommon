using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
namespace SampleEngineName {
	//! 엔진 - 설정
	public partial class CEngine : CComponent {
		#region 함수
		//! 엔진을 설정한다
		private void SetupInit() {
			// Do Nothing
		}
		
		//! 레벨을 설정한다
		private void SetupLevel() {
			for(int i = 0; i < m_stParams.m_oLevelInfo.m_oCellInfoListContainer.Count; ++i) {
				var oCellInfoList = m_stParams.m_oLevelInfo.m_oCellInfoListContainer[i];

				for(int j = 0; j < oCellInfoList.Count; ++j) {
					var stIdx = new Vector3Int(j, i, KCDefine.B_IDX_INVALID);
					this.SetupCell(stIdx, oCellInfoList[j]);
				}
			}
		}

		//! 셀을 설정한다
		private void SetupCell(Vector3Int a_stIdx, CCellInfo a_oCellInfo) {
			// Do Nothing
		}

		//! 엔진을 설정한다
		private void SetupEngine() {
			// Do Nothing
		}
		#endregion			// 함수
	}
}
#endif			// #if NEVER_USE_THIS
