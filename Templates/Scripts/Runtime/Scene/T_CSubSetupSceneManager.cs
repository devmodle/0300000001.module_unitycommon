using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 서브 설정 씬 관리자
public class CSubSetupSceneManager : CSetupSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		if(CSceneManager.IsInit) {
			this.IsAutoLoadTable = true;
			this.IsAutoInitManager = true;
		}
	}

	//! 씬을 설정한다
	protected override void Setup() {
		base.Setup();

#if MSG_PACK_ENABLE
		// 저장소를 로드한다
		CAppInfoStorage.Instance.LoadAppInfo();
		CUserInfoStorage.Instance.LoadUserInfo();
		CGameInfoStorage.Instance.LoadGameInfo();
#endif			// #if MSG_PACK_ENABLE
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
