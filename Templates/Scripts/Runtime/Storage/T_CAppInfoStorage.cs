using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if NEVER_USE_THIS
//! 앱 정보
[MessagePackObject]
[System.Serializable]
public class CAppInfo : CBaseInfo {
	#region 인터페이스
	//! 직렬화 될 경우
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
	}
	#endregion			// 인터페이스
}

//! 앱 정보 저장소
public class CAppInfoStorage : CSingleton<CAppInfoStorage> {
	#region 프로퍼티
	public bool IsIgnoreUpdate { get; set; } = false;
	public bool IsCloseAgreePopup { get; set; } = false;
	
	public CAppInfo AppInfo { get; private set; } = new CAppInfo();
	#endregion			// 프로퍼티

	#region 함수
	//! 앱 정보를 로드한다
	public CAppInfo LoadAppInfo() {
		return this.LoadAppInfo(KDefine.G_DATA_P_APP_INFO);
	}

	//! 앱 정보를 로드한다
	public CAppInfo LoadAppInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
			this.AppInfo = CFunc.ReadMsgPackObj<CAppInfo>(a_oFilePath);
			CAccess.Assert(this.AppInfo != null);
		}

		return this.AppInfo;
	}

	//! 앱 정보를 저장한다
	public void SaveAppInfo() {
		this.SaveAppInfo(KDefine.G_DATA_P_APP_INFO);
	}

	//! 앱 정보를 저장한다
	public void SaveAppInfo(string a_oFilePath) {
		CFunc.WriteMsgPackObj(a_oFilePath, this.AppInfo);
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
