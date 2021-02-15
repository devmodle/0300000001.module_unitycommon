using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 초기화 씬 관리자
public class CSubInitSceneManager : CInitSceneManager {
	#region 함수
	//! 씬을 설정한다
	protected override void Setup() {
		base.Setup();
		
		// 테이블을 생성한다
		CItemInfoTable.Create(KCDefine.U_ASSET_P_G_ITEM_INFO_TABLE);
		CSaleProductInfoTable.Create(KCDefine.U_ASSET_P_G_SALE_PRODUCT_INFO_TABLE);
		
		// 저장소를 생성한다
		CAppInfoStorage.Create();
		CUserInfoStorage.Create();
		CGameInfoStorage.Create();

		// 테이블을 로드한다 {
#if UNITY_EDITOR || UNITY_STANDALONE
		CItemInfoTable.Inst.LoadItemInfosFromFile(KDefine.G_RUNTIME_TABLE_P_ITEM_INFO);
#else
		CItemInfoTable.Inst.LoadItemInfosFromRes(KCDefine.U_TABLE_P_G_ITEM_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
		// 테이블을 로드한다 }
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
