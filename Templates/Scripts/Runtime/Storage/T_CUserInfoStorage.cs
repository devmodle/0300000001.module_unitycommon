using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if NEVER_USE_THIS
//! 유저 정보
[MessagePackObject]
[System.Serializable]
public class CUserInfo : CBaseInfo {
	#region 상수
	private const string KEY_NUM_COINS = "NumCoins";
	private const string KEY_NUM_SALE_COINS = "NumSaleCoins";
	#endregion			// 상수

	#region 변수
	[Key(111)] public Dictionary<string, int> m_oNumItemsDict = new Dictionary<string, int>();
	#endregion			// 변수
	
	#region 프로퍼티
	[IgnoreMember] public int NumCoins {
		get { return m_oIntDict.ExGetVal(CUserInfo.KEY_NUM_COINS, KCDefine.B_VAL_0_INT); } 
		set { m_oIntDict.ExReplaceVal(CUserInfo.KEY_NUM_COINS, value); }
	}

	[IgnoreMember] public int NumSaleCoins {
		get { return m_oIntDict.ExGetVal(CUserInfo.KEY_NUM_SALE_COINS, KCDefine.B_VAL_0_INT); }
		set { m_oIntDict.ExReplaceVal(CUserInfo.KEY_NUM_SALE_COINS, value); }
	}
	#endregion			// 프로퍼티

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

//! 유저 정보 저장소
public class CUserInfoStorage : CSingleton<CUserInfoStorage> {
	#region 프로퍼티
	public CUserInfo UserInfo { get; private set; } = new CUserInfo();
	#endregion            // 프로퍼티

	#region 함수
	//! 유저 정보를 리셋한다
	public virtual void ResetUserInfo(string a_oJSONStr) {
		CFunc.ShowLog("CUserInfoStorage.ResetUserInfo: {0}", a_oJSONStr);
		CAccess.Assert(a_oJSONStr.ExIsValid());

		this.UserInfo = a_oJSONStr.ExMsgPackJSONStrToObj<CUserInfo>();
		CAccess.Assert(this.UserInfo != null);
	}

	//! 아이템 개수를 반환한다
	public int GetNumItems(EItemKinds a_eItemKinds) {
		string oKey = CStrTable.Inst.GetEnumStr<EItemKinds>(a_eItemKinds);
		return this.UserInfo.m_oNumItemsDict.ExGetVal(oKey, KCDefine.B_VAL_0_INT);
	}

	//! 코인 개수를 추가한다
	public void AddNumCoins(int a_nNumCoins) {
		int nNumCoins = this.UserInfo.NumCoins + a_nNumCoins;
		this.UserInfo.NumCoins = Mathf.Clamp(nNumCoins, KCDefine.B_VAL_0_INT, int.MaxValue);
	}

	//! 잔돈 개수를 추가한다
	public void AddNumSaleCoins(int a_nNumSaleCoins) {
		int nNumSaleCoins = this.UserInfo.NumSaleCoins + a_nNumSaleCoins;
		this.UserInfo.NumSaleCoins = Mathf.Clamp(nNumSaleCoins, KCDefine.B_VAL_0_INT, KDefine.G_MAX_NUM_SALE_COINS);
	}
	
	//! 아이템 개수를 추가한다
	public void AddNumItems(EItemKinds a_eItemKinds, int a_nNumItems) {
		int nNumItems = this.GetNumItems(a_eItemKinds) + a_nNumItems;
		nNumItems = Mathf.Clamp(nNumItems, KCDefine.B_VAL_0_INT, int.MaxValue);

		string oKey = CStrTable.Inst.GetEnumStr<EItemKinds>(a_eItemKinds);
		this.UserInfo.m_oNumItemsDict.ExReplaceVal(oKey, nNumItems);
	}

	//! 유저 정보를 로드한다
	public CUserInfo LoadUserInfo() {
		return this.LoadUserInfo(KDefine.G_DATA_P_USER_INFO);
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

	//! 유저 정보를 저장한다
	public void SaveUserInfo() {
		this.SaveUserInfo(KDefine.G_DATA_P_USER_INFO);
	}

	//! 유저 정보를 저장한다
	public void SaveUserInfo(string a_oFilePath) {
		CFunc.WriteMsgPackObj(a_oFilePath, this.UserInfo);
	}
	#endregion			// 함수

	#region 추가 변수

	#endregion			// 추가 변수

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if NEVER_USE_THIS
