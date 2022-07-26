using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 팩토리 */
	public static partial class Factory {
		#region 클래스 함수
		/** 엔진 컴포넌트 매개 변수를 생성한다 */
		public static CEComponent.STParams MakeEComponentParams(CEngine a_oEngine, string a_oObjsPoolKey) {
			return new CEComponent.STParams() {
				m_oObjsPoolKey = a_oObjsPoolKey, m_oEngine = a_oEngine
			};
		}

		/** 아이템 매개 변수를 생성한다 */
		public static CEItem.STParams MakeItemParams(CEngine a_oEngine, STItemInfo a_stTableItemInfo, CItemInfo a_oStorageItemInfo, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new CEItem.STParams() {
				m_stBaseParams = Factory.MakeEComponentParams(a_oEngine, a_oObjsPoolKey), m_stTableItemInfo = a_stTableItemInfo, m_oStorageItemInfo = a_oStorageItemInfo
			};
		}
		
		/** 스킬 매개 변수를 생성한다 */
		public static CESkill.STParams MakeSkillParams(CEngine a_oEngine, STSkillInfo a_stTableSkillInfo, CSkillInfo a_oStorageSkillInfo, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new CESkill.STParams() {
				m_stBaseParams = Factory.MakeEComponentParams(a_oEngine, a_oObjsPoolKey), m_stTableSkillInfo = a_stTableSkillInfo, m_oStorageSkillInfo = a_oStorageSkillInfo
			};
		}

		/** 객체 매개 변수를 생성한다 */
		public static CEObj.STParams MakeObjParams(CEngine a_oEngine, STObjInfo a_stTableObjInfo, CObjInfo a_oStorageObjInfo, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new CEObj.STParams() {
				m_stBaseParams = Factory.MakeEComponentParams(a_oEngine, a_oObjsPoolKey), m_stTableObjInfo = a_stTableObjInfo, m_oStorageObjInfo = a_oStorageObjInfo
			};
		}

		/** 효과 매개 변수를 생성한다 */
		public static CEFX.STParams MakeFXParams(CEngine a_oEngine, STFXInfo a_stTableFXInfo, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new CEFX.STParams() {
				m_stBaseParams = Factory.MakeEComponentParams(a_oEngine, a_oObjsPoolKey), m_stTableFXInfo = a_stTableFXInfo
			};
		}
		#endregion			// 클래스 함수

		#region 조건부 클래스 함수
#if RUNTIME_TEMPLATES_MODULE_ENABLE
		/** 그리드 정보를 생성한다 */
		public static STGridInfo MakeGridInfo(CLevelInfo a_oLevelInfo, Vector3 a_stPos) {
			var stGridInfo = new STGridInfo() {
				m_stSize = new Vector3(a_oLevelInfo.NumCells.x * KDefine.E_SIZE_CELL.x, a_oLevelInfo.NumCells.y * KDefine.E_SIZE_CELL.y, KCDefine.B_VAL_0_REAL)
			};
			
			stGridInfo.m_stBounds = new Bounds(a_stPos, stGridInfo.m_stSize);

			try {
				float fScaleX = (KDefine.E_MAX_SIZE_GRID.x / stGridInfo.m_stBounds.size.x);
				float fScaleY = (KDefine.E_MAX_SIZE_GRID.y / stGridInfo.m_stBounds.size.y);

				stGridInfo.m_stScale = Vector3.one * Mathf.Min(fScaleX, fScaleY);
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"Factory.MakeGridInfo Exception: {oException.Message}");
				stGridInfo.m_stScale = Vector3.one;
			}

			stGridInfo.m_stPivotPos = new Vector3(stGridInfo.m_stBounds.min.x, stGridInfo.m_stBounds.max.y, KCDefine.B_VAL_0_REAL);
			return stGridInfo;
		}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
		#endregion			// 조건부 클래스 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
