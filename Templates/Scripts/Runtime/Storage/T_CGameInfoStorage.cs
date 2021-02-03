using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MessagePack;

#if NEVER_USE_THIS
//! 게임 정보
[MessagePackObject]
[System.Serializable]
public sealed class CGameInfo : CBaseInfo {
	#region 함수
	//! 생성자
	public CGameInfo() : base(KDefine.B_VERSION_GAME_INFO) {
		// Do Nothing
	}
	#endregion			// 함수
}

//! 게임 정보 저장소
public class CGameInfoStorage : CSingleton<CGameInfoStorage> {
	#region 프로퍼티
	public System.DateTime PrevFullscreenAdsTime { get; set; } = System.DateTime.Now;
	public System.DateTime PrevResumeAdsTime { get; set; } = System.DateTime.Now;

	public CGameInfo GameInfo { get; private set; } = new CGameInfo();
	#endregion			// 프로퍼티

	#region 함수
	//! 게임 정보를 저장한다
	public void SaveGameInfo() {
		this.SaveGameInfo(KDefine.B_DATA_P_GAME_INFO);
	}

	//! 게임 정보를 저장한다
	public void SaveGameInfo(string a_oFilePath) {
		CFunc.WriteMsgPackObj(a_oFilePath, this.GameInfo);
	}

	//! 게임 정보를 로드한다
	public CGameInfo LoadGameInfo() {
		return this.LoadGameInfo(KDefine.B_DATA_P_GAME_INFO);
	}

	//! 게임 정보를 로드한다
	public CGameInfo LoadGameInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
			this.GameInfo = CFunc.ReadMsgPackObj<CGameInfo>(a_oFilePath);
			CAccess.Assert(this.GameInfo != null);
		}

		return this.GameInfo;
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
