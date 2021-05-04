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
	private const string KEY_NUM_COINS = "NumCoins";
	private const string KEY_NUM_CHANGES = "NumChanges";
	#endregion			// 상수

	#region 변수
	[Key(71)] public Dictionary<EItemKinds, int> m_oNumItemsList = new Dictionary<EItemKinds, int>();
	#endregion			// 변수
	
	#region 프로퍼티
	[IgnoreMember] public int NumCoins {
		get { return m_oIntList.ExGetVal(CUserInfo.KEY_NUM_COINS, KCDefine.B_VAL_0_INT); } 
		set { m_oIntList.ExReplaceVal(CUserInfo.KEY_NUM_COINS, value); }
	}

	[IgnoreMember] public int NumChanges {
		get { return m_oIntList.ExGetVal(CUserInfo.KEY_NUM_CHANGES, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CUserInfo.KEY_NUM_CHANGES, value); }
	}
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		m_oNumItemsList = m_oNumItemsList ?? new Dictionary<EItemKinds, int>();
	}
	#endregion			// 인터페이스

	#region 함수
	//! 생성자
	public CUserInfo() : base(KDefine.B_VER_USER_INFO) {
		// Do Nothing
	}
	#endregion			// 함수
}

//! 유저 정보 저장소
public class CUserInfoStorage : CSingleton<CUserInfoStorage> {
	#region 프로퍼티
	public CUserInfo UserInfo { get; private set; } = new CUserInfo();
	#endregion            // 프로퍼티

	#region 함수
	//! 아이템 개수를 반환한다
	public int GetNumItems(EItemKinds a_eItemKinds) {
		return this.UserInfo.m_oNumItemsList.ExGetVal(a_eItemKinds, KCDefine.B_VAL_0_INT);
	}

	//! 코인 개수를 추가한다
	public void AddNumCoins(int a_nNumCoins) {
		int nNumCoins = this.UserInfo.NumCoins + a_nNumCoins;
		this.UserInfo.NumCoins = Mathf.Clamp(nNumCoins, KCDefine.B_VAL_0_INT, int.MaxValue);
	}

	//! 잔돈 개수를 추가한다
	public void AddNumChanges(int a_nNumChanges) {
		int nNumChanges = this.UserInfo.NumChanges + a_nNumChanges;
		this.UserInfo.NumChanges = Mathf.Clamp(nNumChanges, KCDefine.B_VAL_0_INT, KDefine.G_MAX_NUM_CHANGES);
	}
	
	//! 아이템 개수를 추가한다
	public void AddNumItems(EItemKinds a_eItemKinds, int a_nNumItems) {
		int nNumItems = this.GetNumItems(a_eItemKinds) + a_nNumItems;
		nNumItems = Mathf.Clamp(nNumItems, KCDefine.B_VAL_0_INT, int.MaxValue);

		this.UserInfo.m_oNumItemsList.ExReplaceVal(a_eItemKinds, nNumItems);
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
