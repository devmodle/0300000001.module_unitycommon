using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif			// #if UNITY_ANDROID

//! 권한 씬 관리자
public abstract class CPermissionSceneManager : CSceneManager {
	#region 변수
	private List<string> m_oSceneNameList = new List<string>() {
		KCDefine.B_SCENE_NAME_INIT,
		KCDefine.B_SCENE_NAME_SETUP,
		KCDefine.B_SCENE_NAME_START,
		KCDefine.B_SCENE_NAME_SPLASH,
		KCDefine.B_SCENE_NAME_AGREE,
		KCDefine.B_SCENE_NAME_LATE_SETUP,
		KCDefine.B_SCENE_NAME_PERMISSION,
		KCDefine.B_SCENE_NAME_INTRO
	};
	#endregion			// 변수

	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_NAME_PERMISSION;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_ORDER_PERMISSION_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR

#if UNITY_ANDROID
	public List<string> PermissionList { get; protected set; } = null;
#endif			// #if UNITY_ANDROID
	#endregion			// 프로퍼티

	#region 추상 함수
#if UNITY_ANDROID
	//! 권한을 요청한다
	protected abstract void RequestPermission(string a_oPermission, 
		System.Action<string, bool> a_oCallback);
#endif			// #if UNITY_ANDROID
	#endregion			// 추상 함수

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();
		StartCoroutine(this.OnStart());
	}

	//! 초기화
	private IEnumerator OnStart() {
		CSceneLoader.Instance.UnloadSceneAsync(KCDefine.B_SCENE_NAME_LATE_SETUP, null);
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

#if UNITY_ANDROID
		this.CheckPermission();
#else
		this.LoadNextScene();
#endif			// #if UNITY_ANDROID
	}

	//! 다음 씬을 로드한다
	private void LoadNextScene() {
		CCommonAppInfoStorage.Instance.SetupDeviceType();
		CCommonAppInfoStorage.Instance.SetupAdsID();
		CCommonAppInfoStorage.Instance.SetupStoreVersion();
		
		CCommonUserInfoStorage.Instance.UserInfo.IsAgree = true;
		CCommonUserInfoStorage.Instance.SaveUserInfo();

		CFunc.BroadcastMsg(KCDefine.SS_FUNC_NAME_START_SCENE_EVENT, 
			EStartSceneEvent.LOAD_INTRO_SCENE);
			
		int nIndex = m_oSceneNameList.ExFindValue((a_oSceneName) => 
			CSceneManager.AwakeSceneName.ExIsEquals(a_oSceneName));

		this.ExLateCallFunc(KCDefine.U_DELAY_NEXT_SCENE_LOAD, (a_oSender, a_oParams) => {
			// 인트로 씬 로드가 필요 할 경우
			if(nIndex > KCDefine.B_INDEX_INVALID) {
				CSceneLoader.Instance.LoadAdditiveScene(KCDefine.B_SCENE_NAME_INTRO);
			} else {
				CSceneLoader.Instance.LoadScene(CSceneManager.AwakeSceneName, false, false);
			}
		});
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_ANDROID
	//! 권한을 수신했을 경우
	private void OnReceivePermission(string a_oPermission, bool a_bIsSuccess) {
		CAccess.Assert(a_oPermission.ExIsValid());

		this.PermissionList.ExRemoveValue(a_oPermission);
		this.CheckPermission();
	}

	//! 권한을 검사한다
	private void CheckPermission() {
		// 권한이 필요 할 경우
		if(this.PermissionList.ExIsValid()) {
			string oPermission = this.PermissionList[KCDefine.B_VALUE_INT_0];
			this.RequestPermission(oPermission);
		} else {
			this.LoadNextScene();
		}
	}

	//! 권한을 요청한다
	private void RequestPermission(string a_oPermission) {
		CAccess.Assert(a_oPermission.ExIsValid());

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
