using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 상수 */
	public static partial class KDefine {
		#region 기본
		// 이름
		public const string E_OBJ_N_FX = "FX";
		public const string E_OBJ_N_BLOCK = "Block";
		public const string E_OBJ_N_BLOCK_SPRITE = "BlockSprite";
		#endregion			// 기본

		#region 런타임 상수
		// 기타
		public static readonly (EBlockKinds, CBlock) E_INVALID_BLOCK_INFO = (EBlockKinds.NONE, null);

		// 크기
		public static readonly Vector3 E_SIZE_CELL = new Vector3(0.0f, 0.0f, 0.0f);
		public static readonly Vector3 E_MAX_SIZE_GRID = new Vector3(KCDefine.B_SCREEN_WIDTH - 20.0f, KCDefine.B_SCREEN_WIDTH - 20.0f, 0.0f);

		// 간격
		public static readonly Vector3 E_OFFSET_CELL = new Vector3(KDefine.E_SIZE_CELL.x / 2.0f, KDefine.E_SIZE_CELL.y / -2.0f, 0.0f);

		// 개수
		public static readonly Vector3Int E_MIN_NUM_CELLS = new Vector3Int(1, 1, 1);
		public static readonly Vector3Int E_MAX_NUM_CELLS = new Vector3Int(15, 15, 15);

		// 정렬 순서
		public static readonly Dictionary<EBlockKinds, STSortingOrderInfo> E_SORTING_OI_BLOCK_DICT = new Dictionary<EBlockKinds, STSortingOrderInfo>() {
			[EBlockKinds.BG_EMPTY] = new STSortingOrderInfo() {
				m_nOrder = sbyte.MaxValue * 0, m_oLayer = KCDefine.U_SORTING_L_DEF
			}
		};

		// 경로 {
		public static readonly string E_OBJ_P_FX = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_FX";
		public static readonly string E_OBJ_P_BLOCK = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}E_Block";

		public static readonly Dictionary<EBlockKinds, string> E_IMG_P_BLOCK_DICT = new Dictionary<EBlockKinds, string>() {
			[EBlockKinds.BG_EMPTY] = EBlockKinds.BG_EMPTY.ToString()
		};
		// 경로 }
		#endregion			// 런타임 상수

		#region 추가 상수

		#endregion			// 추가 상수

		#region 추가 런타임 상수

		#endregion			// 추가 런타임 상수
	}
}
#endif			// #if EXTRA_SCRIPT_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
