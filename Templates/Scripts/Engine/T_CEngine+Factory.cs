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
			var oFX = CFactory.CreateCloneObj<CEFX>(KDefine.E_OBJ_N_FX, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_FX), m_stParams.m_oObjRoot);
			oFX.transform.localPosition = a_stPos;

			oFX.Init(new CEFX.STParams() {
				m_stBaseParams = new CEComponent.STParams() {
					m_oEngine = this
				},

				m_stFXInfo = CFXInfoTable.Inst.GetFXInfo(a_eFXKinds)
			});

			return oFX;
		}

		/** 객체를 생성한다 */
		private CEObj CreateObj(EObjKinds a_eObjKinds, Vector3 a_stPos, Vector3Int a_stIdx) {
			var oObj = CFactory.CreateCloneObj<CEObj>(KDefine.E_OBJ_N_OBJ, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_OBJ), m_stParams.m_oObjRoot);
			oObj.Idx = a_stIdx;
			oObj.transform.localPosition = a_stPos;

			oObj.Init(new CEObj.STParams() {
				m_stBaseParams = new CEComponent.STParams() {
					m_oEngine = this
				},

				m_stObjInfo = CObjInfoTable.Inst.GetObjInfo(a_eObjKinds)
			});

			return oObj;
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
