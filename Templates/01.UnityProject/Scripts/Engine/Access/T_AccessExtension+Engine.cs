#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 접근자 확장 클래스 */
	public static partial class AccessExtension {
		#region 클래스 함수
		/** 인덱스 유효 여부를 검사한다 */
		public static bool ExIsValidIdx(this Dictionary<EObjType, List<CEObj>>[,] a_oSender, Vector3Int a_stIdx) {
			return a_oSender.ExIsValidIdx<Dictionary<EObjType, List<CEObj>>>(a_stIdx) && a_oSender[a_stIdx.y, a_stIdx.x] != null;
		}

		/** 셀 객체를 반환한다 */
		public static List<CEObj> ExGetCellObjs(this Dictionary<EObjType, List<CEObj>>[,] a_oSender, Vector3Int a_stIdx, EObjType a_eObjType) {
			return a_oSender.ExGetVal(a_stIdx, null)?.GetValueOrDefault(a_eObjType);
		}
		#endregion			// 클래스 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
