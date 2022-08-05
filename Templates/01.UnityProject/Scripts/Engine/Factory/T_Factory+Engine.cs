#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 팩토리 */
	public static partial class Factory {
		#region 클래스 함수
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

		/** 엔진 컴포넌트 매개 변수를 생성한다 */
		public static CEComponent.STParams MakeEComponentParams(CEngine a_oEngine, CEComponent a_oOwner, CEController a_oController, string a_oObjsPoolKey) {
			return new CEComponent.STParams() {
				m_oObjsPoolKey = a_oObjsPoolKey, m_oEngine = a_oEngine, m_oOwner = a_oOwner, m_oController = a_oController
			};
		}

		/** 제어자 매개 변수를 생성한다 */
		public static CEController.STParams MakeControllerParams(CEngine a_oEngine, CEComponent a_oOwner) {
			return new CEController.STParams() {
				m_oEngine = a_oEngine, m_oOwner = a_oOwner
			};
		}

		/** 아이템 매개 변수를 생성한다 */
		public static CEItem.STParams MakeItemParams(CEngine a_oEngine, STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo, CEComponent a_oOwner = null, CEController a_oController = null, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new CEItem.STParams() {
				m_stBaseParams = Factory.MakeEComponentParams(a_oEngine, a_oOwner, a_oController, a_oObjsPoolKey), m_stItemInfo = a_stItemInfo, m_oItemTargetInfo = a_oItemTargetInfo
			};
		}
		
		/** 스킬 매개 변수를 생성한다 */
		public static CESkill.STParams MakeSkillParams(CEngine a_oEngine, STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, CEComponent a_oOwner = null, CEController a_oController = null, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new CESkill.STParams() {
				m_stBaseParams = Factory.MakeEComponentParams(a_oEngine, a_oOwner, a_oController, a_oObjsPoolKey), m_stSkillInfo = a_stSkillInfo, m_oSkillTargetInfo = a_oSkillTargetInfo
			};
		}

		/** 객체 매개 변수를 생성한다 */
		public static CEObj.STParams MakeObjParams(CEngine a_oEngine, STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEComponent a_oOwner = null, CEController a_oController = null, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new CEObj.STParams() {
				m_stBaseParams = Factory.MakeEComponentParams(a_oEngine, a_oOwner, a_oController, a_oObjsPoolKey), m_stObjInfo = a_stObjInfo, m_oObjTargetInfo = a_oObjTargetInfo
			};
		}

		/** 효과 매개 변수를 생성한다 */
		public static CEFX.STParams MakeFXParams(CEngine a_oEngine, STFXInfo a_stTableFXInfo, CEComponent a_oOwner = null, CEController a_oController = null, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new CEFX.STParams() {
				m_stBaseParams = Factory.MakeEComponentParams(a_oEngine, a_oOwner, a_oController, a_oObjsPoolKey), m_stTableFXInfo = a_stTableFXInfo
			};
		}

		/** 스킬 제어자 매개 변수를 생성한다 */
		public static CESkillController.STParams MakeSkillControllerParams(CEngine a_oEngine, CEComponent a_oOwner) {
			return new CESkillController.STParams() {
				m_stBaseParams = Factory.MakeControllerParams(a_oEngine, a_oOwner)
			};
		}

		/** 효과 제어자 매개 변수를 생성한다 */
		public static CEFXController.STParams MakeFXControllerParams(CEngine a_oEngine, CEComponent a_oOwner) {
			return new CEFXController.STParams() {
				m_stBaseParams = Factory.MakeControllerParams(a_oEngine, a_oOwner)
			};
		}

		/** 플레이어 객체 제어자 매개 변수를 생성한다 */
		public static CEPlayerObjController.STParams MakePlayerObjControllerParams(CEngine a_oEngine, CEComponent a_oOwner) {
			return new CEPlayerObjController.STParams() {
				m_stBaseParams = Factory.MakeControllerParams(a_oEngine, a_oOwner)
			};
		}

		/** 적 객체 제어자 매개 변수를 생성한다 */
		public static CEEnemyObjController.STParams MakeEnemyObjControllerParams(CEngine a_oEngine, CEComponent a_oOwner) {
			return new CEEnemyObjController.STParams() {
				m_stBaseParams = Factory.MakeControllerParams(a_oEngine, a_oOwner)
			};
		}
		#endregion			// 클래스 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
