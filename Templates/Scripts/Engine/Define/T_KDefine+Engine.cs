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

		// 크기
		public static readonly Vector3 E_SIZE_CELL = new Vector3(0.0f, 0.0f, 0.0f);
		public static readonly Vector3 E_MAX_SIZE_GRID = new Vector3(KCDefine.B_SCREEN_WIDTH - 20.0f, KCDefine.B_SCREEN_WIDTH - 20.0f, 0.0f);

		// 간격
		public static readonly Vector3 E_OFFSET_CELL = new Vector3(KDefine.E_SIZE_CELL.x / 2.0f, KDefine.E_SIZE_CELL.y / -2.0f, 0.0f);

		// 개수
		public static readonly Vector3Int E_MIN_NUM_CELLS = new Vector3Int(1, 1, 1);
		public static readonly Vector3Int E_MAX_NUM_CELLS = new Vector3Int(15, 15, 15);

		// 정렬 순서
		public static readonly Dictionary<EObjKinds, STSortingOrderInfo> E_SORTING_OI_OBJ_DICT = new Dictionary<EObjKinds, STSortingOrderInfo>() {
			[EObjKinds.BG_EMPTY] = new STSortingOrderInfo() {
				m_nOrder = sbyte.MaxValue * 0, m_oLayer = KCDefine.U_SORTING_L_DEF
			}
		};

		// 경로 {
		public static readonly string E_OBJ_P_FX = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_FX";
		public static readonly string E_OBJ_P_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_Obj";

		public static readonly Dictionary<EObjKinds, string> E_IMG_P_OBJ_DICT = new Dictionary<EObjKinds, string>() {
			[EObjKinds.BG_EMPTY] = EObjKinds.BG_EMPTY.ToString()
		};
		// 경로 }
		#endregion			// 런타임 상수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
