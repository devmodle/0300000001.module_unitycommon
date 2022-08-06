#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 접근자 */
	public static partial class Access {
		#region 클래스 함수
		/** 객체 스프라이트를 반환한다 */
		public static Sprite GetObjSprite(EObjKinds a_eObjKinds) {
			bool bIsValid = KDefine.E_IMG_P_OBJ_DICT.TryGetValue((EObjKinds)((int)a_eObjKinds).ExKindsToSubKindsType(), out string oImgPath);
			return (bIsValid && oImgPath.ExIsValid()) ? CResManager.Inst.GetRes<Sprite>(oImgPath) : null;
		}
		
		/** 정렬 순서 정보를 반환한다 */
		public static STSortingOrderInfo GetSortingOrderInfo(EObjKinds a_eObjKinds) {
			bool bIsValid = Access.TryGetSortingOrderInfo(a_eObjKinds, out STSortingOrderInfo stOrderInfo);
			CAccess.Assert(bIsValid);

			return stOrderInfo;
		}

		/** 정렬 순서 정보를 반환한다 */
		public static bool TryGetSortingOrderInfo(EObjKinds a_eObjKinds, out STSortingOrderInfo a_stOutOrderInfo) {
			a_stOutOrderInfo = KDefine.E_SORTING_OI_OBJ_DICT.GetValueOrDefault((EObjKinds)((int)a_eObjKinds).ExKindsToSubKindsType(), STSortingOrderInfo.INVALID);
			return KDefine.E_SORTING_OI_OBJ_DICT.ContainsKey(a_eObjKinds);
		}
		#endregion			// 클래스 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
