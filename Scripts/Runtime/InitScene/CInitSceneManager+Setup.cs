using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! 초기화 씬 관리자 - 설정
public abstract partial class CInitSceneManager : CSceneManager {
	#region 함수
	//! 블라인드 UI 를 설정한다
	private void SetupBlindUI() {
		// 블라인드 UI 가 없을 경우
		if(CInitSceneManager.m_oBlindUI == null) {
			var oBlindUI = CFactory.CreateCloneObj(KCDefine.IS_OBJ_N_BLIND_UI, KCDefine.IS_OBJ_P_SCREEN_BLIND_UI, null);

			CInitSceneManager.m_oBlindUI = oBlindUI;
			CSceneManager.ScreenBlindUIRoot = oBlindUI.ExFindChild(KCDefine.U_OBJ_N_SCREEN_BLIND_UI_ROOT);

			// 블라인드 이미지를 생성한다 {
			var oImgs = new Image[] {
				this.CreateBlindImg(KCDefine.U_OBJ_N_LEFT_BLIND_IMG, CSceneManager.ScreenBlindUIRoot),
				this.CreateBlindImg(KCDefine.U_OBJ_N_RIGHT_BLIND_IMG, CSceneManager.ScreenBlindUIRoot),
				this.CreateBlindImg(KCDefine.U_OBJ_N_TOP_BLIND_IMG, CSceneManager.ScreenBlindUIRoot),
				this.CreateBlindImg(KCDefine.U_OBJ_N_BOTTOM_BLIND_IMG, CSceneManager.ScreenBlindUIRoot)
			};
			
			for(int i = 0; i < oImgs.Length; ++i) {
				oImgs[i].color = KCDefine.U_COLOR_TRANSPARENT;
				oImgs[i].raycastTarget = false;
			}
			// 블라인드 이미지를 생성한다 }

			DontDestroyOnLoad(oBlindUI);
			CFunc.SetupScreenUI(oBlindUI, KCDefine.U_SORTING_O_SCREEN_BLIND_UI);
		}
	}
	#endregion			// 함수
}
