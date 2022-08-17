#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 서브 엔진 팩토리 */
	public static partial class Factory {
		#region 클래스 함수
		/** 셀 객체 제어자 매개 변수를 생성한다 */
		public static CECellObjController.STParams MakeCellObjControllerParams(CEngine a_oEngine, CEComponent a_oOwner) {
			return new CECellObjController.STParams() {
				m_stBaseParams = Factory.MakeObjControllerParams(a_oEngine, a_oOwner)
			};
		}

		/** 플레이어 객체 제어자 매개 변수를 생성한다 */
		public static CEPlayerObjController.STParams MakePlayerObjControllerParams(CEngine a_oEngine, CEComponent a_oOwner) {
			return new CEPlayerObjController.STParams() {
				m_stBaseParams = Factory.MakeObjControllerParams(a_oEngine, a_oOwner)
			};
		}

		/** 적 객체 제어자 매개 변수를 생성한다 */
		public static CEEnemyObjController.STParams MakeEnemyObjControllerParams(CEngine a_oEngine, CEComponent a_oOwner) {
			return new CEEnemyObjController.STParams() {
				m_stBaseParams = Factory.MakeObjControllerParams(a_oEngine, a_oOwner)
			};
		}
		#endregion			// 클래스 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
