using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if UNITY_ANDROID
using UnityEngine.Android;
#endif			// #if UNITY_ANDROID

//! 서브 권한 씬 관리자
public class CSubPermissionSceneManager : CPermissionSceneManager {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

#if UNITY_ANDROID
		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			this.PermissionList = new List<string>() {
				Permission.ExternalStorageRead,
				Permission.ExternalStorageWrite
			};
		}
#endif			// #if UNITY_ANDROID
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_ANDROID
	//! 권한 설명을 출력한다
	protected override void ShowPermissionDesc(string a_oPermission, System.Action<string, bool> a_oCallback) {
		CFunc.RequestPermission(this, a_oPermission, a_oCallback);
	}
#endif			// #if UNITY_ANDROID
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
