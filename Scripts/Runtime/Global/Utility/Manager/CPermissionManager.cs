using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;

//! 권한 관리자
public class CPermissionManager : CSingleton<CPermissionManager> {
	#region 함수
	//! 권한 유효 여부를 검사한다
	public bool IsEnablePermission(string a_oPermission) {
		CBAccess.Assert(a_oPermission.ExIsValid());
		return Permission.HasUserAuthorizedPermission(a_oPermission);
	}

	//! 권한을 요청한다
	public void RequestPermission(string a_oPermission, System.Action<CPermissionManager, bool> a_oCallback) {
		CBAccess.Assert(a_oPermission.ExIsValid());
		Permission.RequestUserPermission(a_oPermission);

		Func.RepeatCallFunc(this,
			KUDefine.DELTA_TIME_PERMISSION_CHECK, KUDefine.MAX_DELTA_TIME_PERMISSION_CHECK, (a_oComponent, a_oParams, a_bIsComplete) => {
				if(a_bIsComplete) {
					a_oCallback?.Invoke(this, this.IsEnablePermission(a_oPermission));
				}

				return !this.IsEnablePermission(a_oPermission);
			});
	}
	#endregion			// 함수
}
#endif			// #if UNITY_ANDROID
