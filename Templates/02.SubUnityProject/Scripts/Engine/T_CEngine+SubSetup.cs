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
		/** 엔진을 설정한다 */
		private void SetupEngine() {
			m_oObjDictContainers = new Dictionary<EObjType, List<CEObj>>[this.Params.m_oLevelInfo.NumCells.y, this.Params.m_oLevelInfo.NumCells.x];
			m_oGridInfoDict.ExReplaceVal(EKey.SEL_GRID_INFO, Factory.MakeGridInfo(this.Params.m_oLevelInfo, Vector3.zero));

			// 객체 풀을 설정한다 {
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_ITEM_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_ITEM), this.Params.m_oItemRoot, KCDefine.U_SIZE_OBJS_POOL, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_SKILL_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_SKILL), this.Params.m_oSkillRoot, KCDefine.U_SIZE_OBJS_POOL, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_OBJS_POOL, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_FX_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_FX), this.Params.m_oFXRoot, KCDefine.U_SIZE_OBJS_POOL, false);

			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_PLAYER_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_OBJS_POOL, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_ENEMY_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_OBJS_POOL, false);
			// 객체 풀을 설정한다 }
		}
		#endregion			// 함수
	}

	/** 서브 엔진 - 설정 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 엔진을 설정한다 */
		private void SubAwakeSetup() {
			// Do Something
		}
		
		/** 셀을 설정한다 */
		private void SetupCell(STCellInfo a_stCellInfo) {
			var oObjInfoDictContainer = new Dictionary<EObjType, List<CEObj>>();

			foreach(var stKeyVal in a_stCellInfo.m_oObjKindsDictContainer) {
				var oObjInfoList = new List<CEObj>();

				for(int i = 0; i < stKeyVal.Value.Count; ++i) {
					// Do Something
				}

				oObjInfoDictContainer.TryAdd(stKeyVal.Key, oObjInfoList);
			}

			m_oObjDictContainers[a_stCellInfo.m_stIdx.y, a_stCellInfo.m_stIdx.x] = oObjInfoDictContainer;
		}

		/** 그리드 라인을 설정한다 */
		private void SetupGridLine() {
			// Do Something
		}
		#endregion			// 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
