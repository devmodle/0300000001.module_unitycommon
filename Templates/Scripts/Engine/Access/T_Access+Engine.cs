using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
namespace SampleEngineName {
	//! 엔진 접근자
	public static partial class Access {
		#region 클래스 함수
		//! 정렬 순서를 반환한다
		public static STSortingOrderInfo GetSortingOrder(EBlockKinds a_eBlockKinds) {
			var eBaseKinds = (EBlockKinds)((int)a_eBlockKinds).ExKindsToBaseKinds();
			CAccess.Assert(KDefine.E_SORTING_OI_BLOCKS.ContainsKey(eBaseKinds));

			return KDefine.E_SORTING_OI_BLOCKS[eBaseKinds];
		}

		//! 블럭 스프라이트를 반환한다
		public static Sprite GetBlockSprite(EBlockKinds a_eBlockKinds) {
			var eBaseKinds = (EBlockKinds)((int)a_eBlockKinds).ExKindsToBaseKinds();
			string oImgPath = KDefine.E_IMG_P_BLOCKS.ExGetVal(eBaseKinds, string.Empty);

			return oImgPath.ExIsValid() ? CResManager.Inst.GetRes<Sprite>(oImgPath) : null;
		}
		#endregion			// 클래스 함수
	}
}
#endif			// #if NEVER_USE_THIS
