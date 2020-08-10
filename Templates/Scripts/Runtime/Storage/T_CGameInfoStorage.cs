using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if NEVER_USE_THIS
#if MSG_PACK_ENABLE
using MessagePack;

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
		var oBytes = MessagePackSerializer.Serialize<CGameInfo>(this.GameInfo);

#if SECURITY_ENABLE
		CFunc.WriteSecurityBytes(a_oFilepath, oBytes);
#else
		CFunc.WriteBytes(a_oFilepath, oBytes);
#endif			// #if SECURITY_ENABLE
	}

	//! 게임 정보를 로드한다
	public void LoadGameInfo() {
		this.LoadGameInfo(KDefine.B_DATA_PATH_GAME_INFO);
	}

	//! 게임 정보를 로드한다
	public void LoadGameInfo(string a_oFilepath) {
		if(File.Exists(a_oFilepath)) {
			try {
#if SECURITY_ENABLE
				var oBytes = CFunc.ReadSecurityBytes(a_oFilepath);
#else
				var oBytes = CFunc.ReadBytes(a_oFilepath);
#endif			// #if SECURITY_ENABLE

				this.GameInfo = MessagePackSerializer.Deserialize<CGameInfo>(oBytes);
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning("CGameInfoStorage.LoadGameInfo Exception: {0}", oException.Message);

				this.Reset();
				this.SaveGameInfo(a_oFilepath);
			}
		}
	}
	#endregion			// 함수
}
#endif			// #if MSG_PACK_ENABLE
#endif			// #if NEVER_USE_THIS
