using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MessagePack;

#if NEVER_USE_THIS
//! 앱 정보
[MessagePackObject]
[System.Serializable]
public sealed class CAppInfo : CBaseInfo {
	#region 함수
	//! 생성자
	public CAppInfo() : base(KDefine.B_VERSION_APP_INFO) {
		// Do Nothing
	}
	#endregion			// 함수
}

//! 앱 정보 저장소
public class CAppInfoStorage : CSingleton<CAppInfoStorage> {
	#region 프로퍼티
	public CAppInfo AppInfo { get; private set; } = new CAppInfo();
	#endregion			// 프로퍼티

	#region 함수
	//! 앱 정보를 저장한다
	public void SaveAppInfo() {
		this.SaveAppInfo(KDefine.B_DATA_P_APP_INFO);
	}

	//! 앱 정보를 저장한다
	public void SaveAppInfo(string a_oFilePath) {
		CFunc.WriteMsgPackObj(a_oFilePath, this.AppInfo);
	}

	//! 앱 정보를 로드한다
	public void LoadAppInfo() {
		this.LoadAppInfo(KDefine.B_DATA_P_APP_INFO);
	}

	//! 앱 정보를 로드한다
	public void LoadAppInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
			this.AppInfo = CFunc.ReadMsgPackObj<CAppInfo>(a_oFilePath);
			CAccess.Assert(this.AppInfo != null);
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
