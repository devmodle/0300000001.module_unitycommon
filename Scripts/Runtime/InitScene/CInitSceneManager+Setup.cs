using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InitScene {
	/** 초기화 씬 관리자 - 설정 */
	public abstract partial class CInitSceneManager : CSceneManager {
		#region 함수
		/** 블라인드 UI 를 설정한다 */
		private void SetupBlindUIs() {
			// 블라인드 UI 가 없을 경우
			if(CInitSceneManager.m_oBlindUIs == null) {
				var oBlindUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_BLIND_UIS, CResManager.Inst.GetRes<GameObject>(KCDefine.IS_OBJ_P_SCREEN_BLIND_UIS), null);

				CInitSceneManager.m_oBlindUIs = oBlindUIs;
				CSceneManager.ScreenBlindUIs = oBlindUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_BLIND_UIS);

				// 블라인드 이미지를 생성한다 {
				var oImgList = new List<Image>() {
					this.CreateBlindImg(KCDefine.U_OBJ_N_UP_BLIND_IMG, CSceneManager.ScreenBlindUIs), this.CreateBlindImg(KCDefine.U_OBJ_N_DOWN_BLIND_IMG, CSceneManager.ScreenBlindUIs), this.CreateBlindImg(KCDefine.U_OBJ_N_LEFT_BLIND_IMG, CSceneManager.ScreenBlindUIs), this.CreateBlindImg(KCDefine.U_OBJ_N_RIGHT_BLIND_IMG, CSceneManager.ScreenBlindUIs)
				};
				
				for(int i = 0; i < oImgList.Count; ++i) {
					oImgList[i].color = KCDefine.U_COLOR_TRANSPARENT;
					oImgList[i].raycastTarget = false;
				}
				// 블라인드 이미지를 생성한다 }

				DontDestroyOnLoad(oBlindUIs);
				CFunc.SetupScreenUIs(oBlindUIs, KCDefine.U_SORTING_O_SCREEN_BLIND_UIS);
			}
		}
		#endregion			// 함수
	}
}
