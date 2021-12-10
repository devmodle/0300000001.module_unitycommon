using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if ENGINE_TEMPLATES_MODULE_ENABLE
namespace SampleEngineName {
	/** 엔진 접근자 */
	public static partial class Access {
		#region 클래스 함수
		/** 정렬 순서를 반환한다 */
		public static STSortingOrderInfo GetSortingOrder(EBlockKinds a_eBlockKinds) {
			bool bIsValid = KDefine.E_SORTING_OI_BLOCK_DICT.TryGetValue(a_eBlockKinds.ExKindsToKindsType(), out STSortingOrderInfo stOrderInfo);
			CAccess.Assert(bIsValid);

			return stOrderInfo;
		}

		/** 블럭 스프라이트를 반환한다 */
		public static Sprite GetBlockSprite(EBlockKinds a_eBlockKinds) {
			bool bIsValid = KDefine.E_IMG_P_BLOCK_DICT.TryGetValue(a_eBlockKinds.ExKindsToKindsType(), out string oImgPath);
			return (bIsValid && oImgPath.ExIsValid()) ? CResManager.Inst.GetRes<Sprite>(oImgPath) : null;
		}
		#endregion			// 클래스 함수

		#region 추가 클래스 함수

		#endregion			// 추가 클래스 함수
	}
}
#endif			// #if ENGINE_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
