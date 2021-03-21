using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 중첩 씬 관리자
public partial class CSubOverlaySceneManager : COverlaySceneManager {
	#region 프로퍼티
	public override STSortingOrderInfo UIsCanvasSortingOrderInfo => KDefine.U_SORTING_OI_OVERLAY_SCENE_UIS_CANVAS;
	public override STSortingOrderInfo ObjsCanvasSortingOrderInfo => KDefine.U_SORTING_OI_OVERLAY_SCENE_OBJS_CANVAS;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		//! 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			// Do Nothing
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS