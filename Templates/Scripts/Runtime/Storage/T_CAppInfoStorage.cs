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
	#region 상수
	private const string KEY_LAST_FREE_COIN_TIME = "LastFreeCoinTime";
	#endregion			// 상수

	#region 프로퍼티
	[IgnoreMember] public System.DateTime LastFreeCoinTime { get; set; } = System.DateTime.Now;
	[IgnoreMember] private string LastFreeCoinTimeString => m_oStringList.ExGetValue(CAppInfo.KEY_LAST_FREE_COIN_TIME, string.Empty);
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 직렬화 될 경우
	public override void OnBeforeSerialize() {
		m_oStringList.ExReplaceValue(CAppInfo.KEY_LAST_FREE_COIN_TIME, this.LastFreeCoinTime.ExToLongString());
	}

	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		this.LastFreeCoinTime = this.LastFreeCoinTime.ExIsValid() ? this.LastFreeCoinTimeString.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Today.AddDays(-KCDefine.B_VALUE_INT_1);
	}
	#endregion			// 인터페이스

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
	public CAppInfo AppInfo { get; private set; } = new CAppInfo() {
		LastFreeCoinTime = System.DateTime.Today.AddDays(-KCDefine.B_VALUE_INT_1)
	};
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
	public CAppInfo LoadAppInfo() {
		return this.LoadAppInfo(KDefine.B_DATA_P_APP_INFO);
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
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
