using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if NEVER_USE_THIS
#if MSG_PACK_ENABLE
using MessagePack;

//! 어플리케이션 정보
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

//! 어플리케이션 정보 저장소
public class CAppInfoStorage : CSingleton<CAppInfoStorage> {
	#region 프로퍼티
	public CAppInfo AppInfo { get; private set; } = null;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.Reset();
	}

	//! 상태를 리셋한다
	public virtual void Reset() {
		this.AppInfo = new CAppInfo();
	}

	//! 어플리케이션 정보를 저장한다
	public void SaveAppInfo(string a_oFilepath) {
		var oBytes = MessagePackSerializer.Serialize<CAppInfo>(this.AppInfo);

#if SECURITY_ENABLE
		CFunc.WriteSecurityBytes(a_oFilepath, oBytes);
#else
		CFunc.WriteBytes(a_oFilepath, oBytes);
#endif			// #if SECURITY_ENABLE
	}

	//! 어플리케이션 정보를 로드한다
	public void LoadAppInfo(string a_oFilepath) {
		if(File.Exists(a_oFilepath)) {
			try {
#if SECURITY_ENABLE
				var oBytes = CFunc.ReadSecurityBytes(a_oFilepath);
#else
				var oBytes = CFunc.ReadBytes(a_oFilepath);
#endif			// #if SECURITY_ENABLE

				this.AppInfo = MessagePackSerializer.Deserialize<CAppInfo>(oBytes);
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning("CAppInfoStorage.LoadAppInfo Exception: {0}", oException.Message);

				this.Reset();
				this.SaveAppInfo(a_oFilepath);
			}
		}
	}
	#endregion			// 함수
}
#endif			// #if MSG_PACK_ENABLE
#endif			// #if NEVER_USE_THIS
