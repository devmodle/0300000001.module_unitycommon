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
			var oBlindUI = CFactory.CreateCloneObj(KCDefine.IS_NAME_BLIND_UI,
				CResManager.Instance.GetPrefab(KCDefine.IS_PATH_SCREEN_BLIND_UI), null);

			CInitSceneManager.m_oBlindUI = oBlindUI;
			CSceneManager.ScreenBlindUIRoot = oBlindUI.ExFindChild(KCDefine.OBJ_NAME_SCREEN_BLIND_UI_ROOT);

			// 블라인드 이미지를 생성한다 {
			var oImgs = new Image[] {
				this.CreateBlindImg(KCDefine.OBJ_NAME_LEFT_BLIND_IMG, CSceneManager.ScreenBlindUIRoot),
				this.CreateBlindImg(KCDefine.OBJ_NAME_RIGHT_BLIND_IMG, CSceneManager.ScreenBlindUIRoot),
				this.CreateBlindImg(KCDefine.OBJ_NAME_TOP_BLIND_IMG, CSceneManager.ScreenBlindUIRoot),
				this.CreateBlindImg(KCDefine.OBJ_NAME_BOTTOM_BLIND_IMG, CSceneManager.ScreenBlindUIRoot)
			};

			for(int i = 0; i < oImgs.Length; ++i) {
				oImgs[i].color = KCDefine.DEF_COLOR_TRANSPARENT;
				oImgs[i].raycastTarget = false;
			}
			// 블라인드 이미지를 생성한다 }

			DontDestroyOnLoad(oBlindUI);
			CFunc.SetupScreenUI(oBlindUI, KCDefine.SORTING_ORDER_SCREEN_BLIND_UI);
		}
	}
	#endregion			// 함수
}
