using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! 초기화 씬 관리자 - 팩토리
public abstract partial class CInitSceneManager : CSceneManager {
	#region 함수
	//! 블라인드 이미지를 생성한다
	protected virtual Image CreateBlindImg(string a_oName, GameObject a_oParent) {
		var oObj = CFactory.CreateCloneObj(a_oName,
			CResManager.Instance.GetRes<GameObject>(KCDefine.IS_OBJ_PATH_SCREEN_BLIND_IMG), a_oParent);

		return oObj.GetComponentInChildren<Image>();
	}
	#endregion			// 함수
}
