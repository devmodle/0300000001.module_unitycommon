using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 인트로 씬 관리자
public class CIntroSceneManager : CSceneManager {
	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_NAME_INTRO;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			CCommonAppInfoStorage.Instance.SetupMobileType();
			CCommonAppInfoStorage.Instance.SetupAdsID();
			CCommonAppInfoStorage.Instance.SetupStoreVersion();
		}
	}
	#endregion			// 함수
}
