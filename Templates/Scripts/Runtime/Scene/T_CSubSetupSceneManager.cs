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
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
