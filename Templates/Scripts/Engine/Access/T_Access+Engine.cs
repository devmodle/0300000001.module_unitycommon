using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 접근자 */
	public static partial class Access {
		#region 클래스 함수
		/** 블럭 스프라이트를 반환한다 */
		public static Sprite GetBlockSprite(EBlockKinds a_eBlockKinds) {
			bool bIsValid = KDefine.E_IMG_P_BLOCK_DICT.TryGetValue((EBlockKinds)((int)a_eBlockKinds).ExKindsToKindsType(), out string oImgPath);
			return (bIsValid && oImgPath.ExIsValid()) ? CResManager.Inst.GetRes<Sprite>(oImgPath) : null;
		}
		
		/** 정렬 순서 정보를 반환한다 */
		public static STSortingOrderInfo GetSortingOrderInfo(EBlockKinds a_eBlockKinds) {
			bool bIsValid = Access.TryGetSortingOrderInfo(a_eBlockKinds, out STSortingOrderInfo stOrderInfo);
			CAccess.Assert(bIsValid);

			return stOrderInfo;
		}

		/** 정렬 순서 정보를 반환한다 */
		public static bool TryGetSortingOrderInfo(EBlockKinds a_eBlockKinds, out STSortingOrderInfo a_stOutOrderInfo) {
			a_stOutOrderInfo = KDefine.E_SORTING_OI_BLOCK_DICT.GetValueOrDefault(a_eBlockKinds, default(STSortingOrderInfo));
			return KDefine.E_SORTING_OI_BLOCK_DICT.ContainsKey(a_eBlockKinds);
		}
		#endregion			// 클래스 함수
	}
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
