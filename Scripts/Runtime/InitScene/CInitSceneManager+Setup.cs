using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! 초기화 씬 관리자 - 설정
public abstract partial class CInitSceneManager : CSceneManager {
	#region 함수
	//! 블라인드 UI 를 설정한다
	private void SetupBlindUI() {
		if(CInitSceneManager.m_oBlindUI == null) {
			var oBlindUI = Func.CreateCloneObj(KDefine.IS_NAME_BLIND_UI,
				CResManager.Instance.GetPrefab(KDefine.IS_PATH_SCREEN_BLIND_UI), null);

			CInitSceneManager.m_oBlindUI = oBlindUI;
			CSceneManager.ScreenBlindUIRoot = oBlindUI.ExFindChild(KDefine.U_OBJ_NAME_SCREEN_BLIND_UI_ROOT);

			// 블라인드 이미지를 생성한다 {
			var oImgs = new Image[] {
				this.CreateBlindImg(KDefine.U_OBJ_NAME_LEFT_BLIND_IMG, CSceneManager.ScreenBlindUIRoot),
				this.CreateBlindImg(KDefine.U_OBJ_NAME_RIGHT_BLIND_IMG, CSceneManager.ScreenBlindUIRoot),
				this.CreateBlindImg(KDefine.U_OBJ_NAME_TOP_BLIND_IMG, CSceneManager.ScreenBlindUIRoot),
				this.CreateBlindImg(KDefine.U_OBJ_NAME_BOTTOM_BLIND_IMG, CSceneManager.ScreenBlindUIRoot)
			};

			for(int i = 0; i < oImgs.Length; ++i) {
				oImgs[i].color = KDefine.U_DEF_COLOR_TRANSPARENT;
				oImgs[i].raycastTarget = false;
			}
			// 블라인드 이미지를 생성한다 }

			DontDestroyOnLoad(oBlindUI);
			Func.SetupScreenUI(oBlindUI, KDefine.U_SORTING_ORDER_SCREEN_BLIND_UI);
		}
	}
	#endregion			// 함수
}
