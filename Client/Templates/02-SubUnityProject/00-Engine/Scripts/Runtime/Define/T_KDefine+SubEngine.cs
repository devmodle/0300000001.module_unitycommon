#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 엔진 상수 */
	public static partial class KDefine {
		#region 기본
		// 단위
		public const float E_MIN_TIME_SCALE = 1.0f;
		public const float E_MAX_TIME_SCALE = 1.0f;
		#endregion // 기본

		#region 런타임 상수
		// 색상 {
		public static readonly Dictionary<EItemKinds, Color> E_COLOR_ITEM_DICT = new Dictionary<EItemKinds, Color>() {
			// Do Something
		};

		public static readonly Dictionary<ESkillKinds, Color> E_COLOR_SKILL_DICT = new Dictionary<ESkillKinds, Color>() {
			// Do Something
		};

		public static readonly Dictionary<EObjKinds, Color> E_COLOR_OBJ_DICT = new Dictionary<EObjKinds, Color>() {
			// Do Something
		};
		// 색상 }

		// 정렬 순서 {
		public static readonly STSortingOrderInfo E_SORTING_OI_ITEM_DEF = new STSortingOrderInfo(KCDefine.U_SORTING_OI_DEF.m_nOrder - (sbyte.MaxValue * 1), KCDefine.U_SORTING_OI_DEF.m_oLayer);
		public static readonly STSortingOrderInfo E_SORTING_OI_SKILL_DEF = new STSortingOrderInfo(KCDefine.U_SORTING_OI_FOREGROUND.m_nOrder - (sbyte.MaxValue * 1), KCDefine.U_SORTING_OI_FOREGROUND.m_oLayer);
		public static readonly STSortingOrderInfo E_SORTING_OI_OBJ_DEF = new STSortingOrderInfo(KCDefine.U_SORTING_OI_DEF.m_nOrder, KCDefine.U_SORTING_OI_DEF.m_oLayer);
		public static readonly STSortingOrderInfo E_SORTING_OI_FX_DEF = new STSortingOrderInfo(KCDefine.U_SORTING_OI_FOREGROUND.m_nOrder, KCDefine.U_SORTING_OI_FOREGROUND.m_oLayer);

		public static readonly Dictionary<EItemKinds, STSortingOrderInfo> E_SORTING_OI_ITEM_DICT = new Dictionary<EItemKinds, STSortingOrderInfo>() {
			// Do Something
		};

		public static readonly Dictionary<ESkillKinds, STSortingOrderInfo> E_SORTING_OI_SKILL_DICT = new Dictionary<ESkillKinds, STSortingOrderInfo>() {
			// Do Something
		};

		public static readonly Dictionary<EObjKinds, STSortingOrderInfo> E_SORTING_OI_OBJ_DICT = new Dictionary<EObjKinds, STSortingOrderInfo>() {
			[EObjKinds.BG_OBJ_EMPTY_01] = new STSortingOrderInfo(KCDefine.U_SORTING_OI_BACKGROUND.m_nOrder, KCDefine.U_SORTING_OI_BACKGROUND.m_oLayer),
			[EObjKinds.BG_OBJ_PLACEHOLDER_01] = new STSortingOrderInfo(KCDefine.U_SORTING_OI_UNDERGROUND.m_nOrder, KCDefine.U_SORTING_OI_UNDERGROUND.m_oLayer)
		};

		public static readonly Dictionary<EFXKinds, STSortingOrderInfo> E_SORTING_OI_FX_DICT = new Dictionary<EFXKinds, STSortingOrderInfo>() {
			// Do Something
		};
		// 정렬 순서 }

		// 경로 {
		public static readonly Dictionary<EItemKinds, string> E_IMG_P_ITEM_DICT = new Dictionary<EItemKinds, string>() {
			// Do Something
		};

		public static readonly Dictionary<ESkillKinds, string> E_IMG_P_SKILL_DICT = new Dictionary<ESkillKinds, string>() {
			// Do Something
		};

		public static readonly Dictionary<EObjKinds, string> E_IMG_P_OBJ_DICT = new Dictionary<EObjKinds, string>() {
			[EObjKinds.BG_OBJ_EMPTY_01] = EObjKinds.BG_OBJ_EMPTY_01.ToString(),
			[EObjKinds.BG_OBJ_PLACEHOLDER_01] = EObjKinds.BG_OBJ_PLACEHOLDER_01.ToString()
		};
		// 경로 }
		#endregion // 런타임 상수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
