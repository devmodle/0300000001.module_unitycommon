using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 상수 */
	public static partial class KDefine {
		#region 기본
		// 이름
		public const string E_OBJ_N_FX = "FX";
		public const string E_OBJ_N_OBJ = "Obj";
		#endregion			// 기본

		#region 런타임 상수
		// 기타
		public static readonly (EObjKinds, CEObj) E_INVALID_OBJ_INFO = (EObjKinds.NONE, null);

		// 경로
		public static readonly string E_OBJ_P_FX = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_FX";
		public static readonly string E_OBJ_P_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_Obj";
		#endregion			// 런타임 상수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
