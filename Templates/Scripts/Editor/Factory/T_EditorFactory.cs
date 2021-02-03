using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if UNITY_EDITOR
using UnityEditor;

//! 에디터 기본 팩토리
public static partial class EditorFactory {
	#region 클래스 함수
	//! 판매 상품 정보 테이블을 생성한다
	[MenuItem("Tools/Utility/Create/SaleProductInfoTable")]
	public static void CreateSaleProductInfoTable() {
		CEditorFactory.CreateScriptableObj<CSaleProductInfoTable>(KCEditorDefine.B_ASSET_P_SALE_PRODUCT_INFO_TABLE);
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
