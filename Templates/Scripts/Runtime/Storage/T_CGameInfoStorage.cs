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

	public CGameInfo GameInfo { get; private set; } = null;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.Reset();
	}

	//! 상태를 리셋한다
	public virtual void Reset() {
		this.GameInfo = new CGameInfo();
	}

	//! 게임 정보를 저장한다
	public void SaveGameInfo() {
		this.SaveGameInfo(KDefine.B_DATA_PATH_GAME_INFO);
	}

	//! 게임 정보를 저장한다
	public void SaveGameInfo(string a_oFilepath) {
		CFunc.WriteMsgPackObj(a_oFilepath, this.GameInfo);
	}

	//! 게임 정보를 로드한다
	public void LoadGameInfo() {
		this.LoadGameInfo(KDefine.B_DATA_PATH_GAME_INFO);
	}

	//! 게임 정보를 로드한다
	public void LoadGameInfo(string a_oFilepath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilepath)) {
			try {
				this.GameInfo = CFunc.ReadMsgPackObj<CGameInfo>(a_oFilepath);
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning("CGameInfoStorage.LoadGameInfo Exception: {0}", oException.Message);

				this.Reset();
				this.SaveGameInfo(a_oFilepath);
			}
		}
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
