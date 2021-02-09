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
	#region 상수
	private const string KEY_NUM_CHANGES = "NumChanges";
	private const string KEY_FREE_REWARD_TIMES = "FreeRewardTimes";
	#endregion			// 상수

	#region 프로퍼티
	[Key(51)] public List<string> m_oNonProductIDList { get; set; } = new List<string>();
	[Key(71)] public Dictionary<EItemKinds, int> m_oNumItemsList { get; set; } = new Dictionary<EItemKinds, int>();

	[IgnoreMember] public int NumChanges {
		get { return m_oIntList.ExGetValue(CUserInfo.KEY_NUM_CHANGES, 0); }
		set { m_oIntList.ExReplaceValue(CUserInfo.KEY_NUM_CHANGES, value); }
	}

	[IgnoreMember] public int FreeRewardTimes {
		get { return m_oIntList.ExGetValue(CUserInfo.KEY_FREE_REWARD_TIMES, 0); }
		set { m_oIntList.ExReplaceValue(CUserInfo.KEY_FREE_REWARD_TIMES, value); }
	}
	#endregion			// 프로퍼티

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
	public CUserInfo UserInfo { get; private set; } = new CUserInfo();
	#endregion			// 프로퍼티

	#region 함수
	//! 아이템 개수를 반환한다
	public int GetNumItems(EItemKinds a_eItemKinds) {
		return this.UserInfo.m_oNumItemsList.ExGetValue(a_eItemKinds, KCDefine.B_VALUE_INT_0);
	}

	//! 잔돈 개수를 추가한다
	public void AddNumChanges(int a_nNumChanges) {
		int nNumChanges = this.UserInfo.NumChanges + a_nNumChanges;
		this.UserInfo.NumChanges = Mathf.Clamp(nNumChanges, KCDefine.B_VALUE_INT_0, KDefine.G_MAX_NUM_CHANGES);
	}

	//! 무료 보상 횟수를 추가한다
	public void AddFreeRewardTimes(int a_nRewardTimes) {
		int nFreeRewardTimes = this.UserInfo.FreeRewardTimes + a_nRewardTimes;
		this.UserInfo.FreeRewardTimes = Mathf.Clamp(nFreeRewardTimes, KCDefine.B_VALUE_INT_0, KDefine.G_MAX_TIMES_FREE_REWARD);
	}
	
	//! 아이템 개수를 추가한다
	public void AddNumItems(EItemKinds a_eItemKinds, int a_nNumItems) {
		int nNumItems = this.GetNumItems(a_eItemKinds) + a_nNumItems;
		nNumItems = Mathf.Clamp(nNumItems, KCDefine.B_VALUE_INT_0, int.MaxValue);

		this.UserInfo.m_oNumItemsList.ExReplaceValue(a_eItemKinds, nNumItems);
	}
	
	//! 유저 정보를 저장한다
	public void SaveUserInfo() {
		this.SaveUserInfo(KDefine.B_DATA_P_USER_INFO);
	}

	//! 유저 정보를 저장한다
	public void SaveUserInfo(string a_oFilePath) {
		CFunc.WriteMsgPackObj(a_oFilePath, this.UserInfo);
	}

	//! 유저 정보를 로드한다
	public CUserInfo LoadUserInfo() {
		return this.LoadUserInfo(KDefine.B_DATA_P_USER_INFO);
	}

	//! 유저 정보를 로드한다
	public CUserInfo LoadUserInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
			this.UserInfo = CFunc.ReadMsgPackObj<CUserInfo>(a_oFilePath);
			CAccess.Assert(this.UserInfo != null);
		}

		return this.UserInfo;
	}
	#endregion			// 함수
}
#endif			// #if NEVER_USE_THIS
