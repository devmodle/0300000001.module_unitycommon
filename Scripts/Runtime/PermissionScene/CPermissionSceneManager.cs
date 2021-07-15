using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif			// #if UNITY_ANDROID

//! 권한 씬 관리자
public abstract class CPermissionSceneManager : CSceneManager {
	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_PERMISSION;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_O_PERMISSION_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR

#if UNITY_ANDROID
	public List<string> PermissionList { get; protected set; } = new List<string>();
#endif			// #if UNITY_ANDROID
	#endregion			// 프로퍼티

	#region 추상 함수
#if UNITY_ANDROID
	//! 권한을 요청한다
	protected abstract void RequestPermission(string a_oPermission, System.Action<string, bool> a_oCallback);
#endif			// #if UNITY_ANDROID
	#endregion			// 추상 함수

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			StartCoroutine(this.OnStart());
		}
	}

	//! 초기화
	private IEnumerator OnStart() {
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);
		CSceneLoader.Inst.UnloadSceneAsync(KCDefine.B_SCENE_N_LATE_SETUP, null);

#if UNITY_ANDROID
		this.CheckPermission();
#else
		this.LoadNextScene();
#endif			// #if UNITY_ANDROID
	}

	//! 다음 씬을 로드한다
	private void LoadNextScene() {
		CCommonAppInfoStorage.Inst.SetupDeviceType();
		CCommonAppInfoStorage.Inst.SetupAdsID();
		CCommonAppInfoStorage.Inst.SetupStoreVer();
		
		CFunc.BroadcastMsg(KCDefine.SS_FUNC_N_START_SCENE_EVENT, EStartSceneEvent.LOAD_INTRO_SCENE);
		CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_INTRO);
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_ANDROID
	//! 권한을 수신했을 경우
	private void OnReceivePermission(string a_oPermission, bool a_bIsSuccess) {
		this.PermissionList.ExRemoveVal(a_oPermission);
		this.CheckPermission();
	}

	//! 권한을 검사한다
	private void CheckPermission() {
		// 권한이 필요 할 경우
		if(this.PermissionList.ExIsValid()) {
			string oPermission = this.PermissionList[KCDefine.B_VAL_0_INT];
			this.RequestPermission(oPermission);
		} else {
			this.LoadNextScene();
		}
	}

	//! 권한을 요청한다
	private void RequestPermission(string a_oPermission) {
		// 권한이 유효 할 경우
		if(CAccess.IsEnablePermission(a_oPermission)) {
			this.OnReceivePermission(a_oPermission, true);
		} else {
			this.RequestPermission(a_oPermission, this.OnReceivePermission);
		}
	}
#endif			// #if UNITY_ANDROID
	#endregion			// 조건부 함수
}
