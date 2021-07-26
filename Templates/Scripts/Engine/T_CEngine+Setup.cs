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
			foreach(var stKeyVal in m_stParams.m_oLevelInfo.m_oCellInfoDictContainer) {
				foreach(var stCellInfoKeyVal in stKeyVal.Value) {
					var stIdx = new Vector3Int(stCellInfoKeyVal.Key, stKeyVal.Key, KCDefine.B_IDX_INVALID);
					this.SetupCell(stIdx, stCellInfoKeyVal.Value);
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
