using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
namespace SampleEngineName {
	//! 엔진 상수
	public static partial class KDefine {
		#region 기본

		#endregion			// 기본

		#region 런타임 상수
		// 크기
		public static readonly Vector3 E_SIZE_CELL = new Vector3(0.0f, 0.0f, 0.0f);
		public static readonly Vector3 E_MAX_SIZE_GRID = new Vector3(KCDefine.B_SCREEN_WIDTH - 20.0f, KCDefine.B_SCREEN_WIDTH - 20.0f, 0.0f);

		// 간격
		public static readonly Vector3 E_OFFSET_CELL = new Vector3(KDefine.E_SIZE_CELL.x / 2.0f, KDefine.E_SIZE_CELL.y / -2.0f, 0.0f);

		// 개수
		public static readonly Vector3Int E_MIN_NUM_CELLS = new Vector3Int(0, 0, 0);
		public static readonly Vector3Int E_MAX_NUM_CELLS = new Vector3Int(0, 0, 0);

		// 정렬 순서
		public static readonly Dictionary<EBlockKinds, STSortingOrderInfo> E_SORTING_OI_BLOCKS = new Dictionary<EBlockKinds, STSortingOrderInfo>() {
			[EBlockKinds.BG_EMPTY] = new STSortingOrderInfo() {
				m_nOrder = 1,
				m_oLayer = KCDefine.U_SORTING_L_BACKGROUND	
			}
		};

		// 경로
		public static readonly Dictionary<EBlockKinds, string> E_IMG_P_BLOCKS = new Dictionary<EBlockKinds, string>() {
			[EBlockKinds.BG_EMPTY] = KCDefine.U_IMG_N_UNKNOWN_SPRITE
		};
		#endregion			// 런타임 상수
	}
}
#endif			// #if NEVER_USE_THIS
