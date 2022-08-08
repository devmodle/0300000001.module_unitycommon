#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 서브 엔진 상수 */
	public static partial class KDefine {
		#region 기본
		// 간격
		public const float E_OFFSET_BOTTOM = 150.0f;
		public const float E_OFFSET_MAIN_CAMERA = -50.0f;
		#endregion			// 기본

		#region 런타임 상수
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
			[EObjKinds.BG_EMPTY_01] = new STSortingOrderInfo() { m_nOrder = sbyte.MaxValue * 0, m_oLayer = KCDefine.U_SORTING_L_DEF },
			[EObjKinds.PLAYABLE_COMMON_CHARACTER_01] = new STSortingOrderInfo() { m_nOrder = sbyte.MaxValue * 1, m_oLayer = KCDefine.U_SORTING_L_DEF }
		};

		// 경로
		public static readonly Dictionary<EObjKinds, string> E_IMG_P_OBJ_DICT = new Dictionary<EObjKinds, string>() {
			[EObjKinds.BG_EMPTY_01] = EObjKinds.BG_EMPTY_01.ToString(),
			[EObjKinds.PLAYABLE_COMMON_CHARACTER_01] = EObjKinds.PLAYABLE_COMMON_CHARACTER_01.ToString()
		};
		#endregion			// 런타임 상수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
