using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MessagePack;

#if NEVER_USE_THIS
//! 유저 정보
[MessagePackObject]
[System.Serializable]
public sealed class CUserInfo : CBaseInfo {
	#region 함수
	//! 생성자
	public CUserInfo() : base(KDefine.B_VERSION_USER_INFO) {
		// Do Nothing
	}
	#endregion			// 함수
}

//! 유저 정보 저장소
public class CUserInfoStorage : CSingleton<CUserInfoStorage> {
	#region 프로퍼티
	public CUserInfo UserInfo { get; private set; } = null;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.Reset();
	}

	//! 상태를 리셋한다
	public virtual void Reset() {
		this.UserInfo = new CUserInfo();
	}

	//! 유저 정보를 저장한다
	public void SaveUserInfo() {
		this.SaveUserInfo(KDefine.B_DATA_PATH_USER_INFO);
	}

	//! 유저 정보를 저장한다
	public void SaveUserInfo(string a_oFilePath) {
		this.SaveUserInfo(KDefine.B_DATA_PATH_USER_INFO);
	}

	//! 유저 정보를 로드한다
	public void LoadUserInfo() {
		this.LoadUserInfo(KDefine.B_DATA_PATH_USER_INFO);
	}

	//! 유저 정보를 로드한다
	public void LoadUserInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
			try {
				this.UserInfo = CFunc.ReadMsgPackObj<CUserInfo>(a_oFilePath);
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning("CUserInfoStorage.LoadUserInfo Exception: {0}", oException.Message);

				this.Reset();
				this.SaveUserInfo(a_oFilePath);
			}
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
