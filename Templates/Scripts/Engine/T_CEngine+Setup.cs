using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
namespace SampleEngineName {
	//! 엔진 - 설정
	public partial class CEngine : CComponent {
		#region 함수
		//! 엔진을 설정한다
		private void SetupInit() {
			m_oBlockDicts = new Dictionary<EBlockKinds, GameObject>[m_stParams.m_oLevelInfo.NumCells.y, m_stParams.m_oLevelInfo.NumCells.x];
			m_stGridInfo = Factory.MakeGridInfo(m_stParams.m_oLevelInfo);
		}
		
		//! 레벨을 설정한다
		private void SetupLevel() {
			foreach(var stKeyVal in m_stParams.m_oLevelInfo.m_oCellInfoDictContainer) {
				foreach(var stCellInfoKeyVal in stKeyVal.Value) {
					this.SetupCell(stCellInfoKeyVal.Value);
				}
			}
		}

		//! 셀을 설정한다
		private void SetupCell(CCellInfo a_oCellInfo) {
			var oBlockDict = new Dictionary<EBlockKinds, GameObject>();

			// 셀 정보가 존재 할 경우
			if(a_oCellInfo != null) {
				for(int i = 0; i < a_oCellInfo.m_oBlockKindsList.Count; ++i) {
					// Do Something
				}
			}

			m_oBlockDicts[a_oCellInfo.m_stIdx.y, a_oCellInfo.m_stIdx.x] = oBlockDict;
		}

		//! 엔진을 설정한다
		private void SetupEngine() {
			// Do Something
		}
		#endregion			// 함수
	}
}
#endif			// #if NEVER_USE_THIS
