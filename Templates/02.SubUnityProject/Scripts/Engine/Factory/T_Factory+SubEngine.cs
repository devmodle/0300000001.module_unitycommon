#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 서브 엔진 팩토리 */
	public static partial class Factory {
		#region 클래스 함수
		/** 플레이어 객체 매개 변수를 생성한다 */
		public static CEPlayerObj.STParams MakePlayerObjParams(CEngine a_oEngine, STObjInfo a_stObjInfo, CObjInfo a_oObjTargetInfo, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new CEPlayerObj.STParams() {
				m_stBaseParams = Factory.MakeObjParams(a_oEngine, a_stObjInfo, a_oObjTargetInfo, a_oObjsPoolKey)
			};
		}

		/** 적 객체 매개 변수를 생성한다 */
		public static CEEnemyObj.STParams MakeEnemyObjParams(CEngine a_oEngine, STObjInfo a_stObjInfo, CObjInfo a_oObjTargetInfo, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new CEEnemyObj.STParams() {
				m_stBaseParams = Factory.MakeObjParams(a_oEngine, a_stObjInfo, a_oObjTargetInfo, a_oObjsPoolKey)
			};
		}
		#endregion			// 클래스 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
