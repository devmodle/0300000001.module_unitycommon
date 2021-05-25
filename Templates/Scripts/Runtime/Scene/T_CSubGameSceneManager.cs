using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 서브 게임 씬 관리자
public partial class CSubGameSceneManager : CGameSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		
		//! 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupAwake();
		}
	}
	
	//! 초기화
	public override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			this.SetupStart();
			this.UpdateUIsState();

			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_OVERLAY);
		}
	}

	//! 씬을 설정한다
	private void SetupAwake() {
		// Do Nothing
	}

	//! 씬을 설정한다
	private void SetupStart() {
		// Do Nothing
	}

	//! UI 상태를 갱신한다
	private void UpdateUIsState() {
		// Do Nothing
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
