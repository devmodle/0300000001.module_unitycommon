using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 - 팩토리 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 효과를 생성한다 */
		private CEFX CreateFX(EFXKinds a_eFXKinds, Vector3 a_stPos) {
			var oFX = CFactory.CreateCloneObj<CEFX>(KDefine.E_OBJ_N_FX, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_FX), m_stParams.m_oBlockObjs);
			oFX.transform.localPosition = a_stPos;

			oFX.Init(new CEFX.STParams() {
				m_stFXInfo = CFXInfoTable.Inst.GetFXInfo(a_eFXKinds), m_oEngine = this
			});

			return oFX;
		}

		/** 블럭을 생성한다 */
		private CEBlock CreateBlock(EBlockKinds a_eBlockKinds, Vector3 a_stPos, Vector3Int a_stIdx) {
			var oBlock = CFactory.CreateCloneObj<CEBlock>(KDefine.E_OBJ_N_BLOCK, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_BLOCK), m_stParams.m_oBlockObjs);
			oBlock.Idx = a_stIdx;
			oBlock.transform.localPosition = a_stPos;

			oBlock.Init(new CEBlock.STParams() {
				m_stBlockInfo = CBlockInfoTable.Inst.GetBlockInfo(a_eBlockKinds), m_oEngine = this
			});

			return oBlock;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
