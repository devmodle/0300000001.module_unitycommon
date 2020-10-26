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
			CCommonAppInfoStorage.Instance.SetupDeviceType();
			CCommonAppInfoStorage.Instance.SetupAdsID();
			CCommonAppInfoStorage.Instance.SetupStoreVersion();
		}
	}

	//! 초기화
	public sealed override void Start() {
		base.Start();
		StartCoroutine(this.OnStart());
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		// Do Nothing
	}

	//! 초기화
	private IEnumerator OnStart() {
		CAccess.Assert(CSceneManager.IsInit);

		CSceneLoader.Instance.UnloadSceneAsync(KCDefine.B_SCENE_NAME_PERMISSION, null);
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

		this.Setup();
	}
	#endregion			// 함수
}
