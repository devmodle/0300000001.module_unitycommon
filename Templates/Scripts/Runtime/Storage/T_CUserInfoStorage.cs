using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
using Newtonsoft.Json;

/** 유저 정보 */
[MessagePackObject][System.Serializable]
public class CUserInfo : CBaseInfo {
	#region 상수
	private const string KEY_NUM_COINS = "NumCoins";
	private const string KEY_NUM_SALE_COINS = "NumSaleCoins";
	#endregion			// 상수

	#region 변수
	[Key(111)] public Dictionary<EItemKinds, long> m_oNumItemsDict = new Dictionary<EItemKinds, long>();
	#endregion			// 변수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public long NumCoins {
		get { return long.Parse(m_oStrDict.GetValueOrDefault(CUserInfo.KEY_NUM_COINS, $"{KCDefine.B_VAL_0_INT}")); }
		set { m_oStrDict.ExReplaceVal(CUserInfo.KEY_NUM_COINS, $"{value}"); }
	}

	[JsonIgnore][IgnoreMember] public long NumSaleCoins {
		get { return long.Parse(m_oStrDict.GetValueOrDefault(CUserInfo.KEY_NUM_SALE_COINS, $"{KCDefine.B_VAL_0_INT}")); }
		set { m_oStrDict.ExReplaceVal(CUserInfo.KEY_NUM_SALE_COINS, $"{value}"); }
	}
	#endregion			// 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();

		// 버전이 다를 경우
		if(this.Ver.CompareTo(KDefine.G_VER_USER_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CUserInfo() : base(KDefine.G_VER_USER_INFO) {
		// Do Something
	}
	#endregion			// 함수
}

/** 유저 정보 저장소 */
public class CUserInfoStorage : CSingleton<CUserInfoStorage> {
	#region 프로퍼티
	public CUserInfo UserInfo { get; private set; } = new CUserInfo();
	#endregion            // 프로퍼티

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 유저 정보를 리셋한다 */
	public virtual void ResetUserInfo(string a_oJSONStr) {
		CFunc.ShowLog($"CUserInfoStorage.ResetUserInfo: {a_oJSONStr}");
		CAccess.Assert(a_oJSONStr.ExIsValid());

		this.UserInfo = a_oJSONStr.ExMsgPackJSONStrToObj<CUserInfo>();
		CAccess.Assert(this.UserInfo != null);
	}

	/** 아이템 개수를 반환한다 */
	public long GetNumItems(EItemKinds a_eItemKinds) {
		return this.UserInfo.m_oNumItemsDict.GetValueOrDefault(a_eItemKinds, KCDefine.B_VAL_0_INT);
	}

	/** 코인 개수를 추가한다 */
	public void AddNumCoins(long a_nNumCoins) {
		this.UserInfo.NumCoins = System.Math.Clamp(this.UserInfo.NumCoins + a_nNumCoins, KCDefine.B_VAL_0_INT, int.MaxValue);
	}

	/** 잔돈 개수를 추가한다 */
	public void AddNumSaleCoins(long a_nNumSaleCoins) {
		this.UserInfo.NumSaleCoins = System.Math.Clamp(this.UserInfo.NumSaleCoins + a_nNumSaleCoins, KCDefine.B_VAL_0_INT, KDefine.G_MAX_NUM_SALE_COINS);
	}
	
	/** 아이템 개수를 추가한다 */
	public void AddNumItems(EItemKinds a_eItemKinds, long a_nNumItems) {
		long nNumItems = this.GetNumItems(a_eItemKinds) + a_nNumItems;
		this.UserInfo.m_oNumItemsDict.ExReplaceVal(a_eItemKinds, System.Math.Clamp(nNumItems, KCDefine.B_VAL_0_INT, int.MaxValue));
	}

	/** 유저 정보를 로드한다 */
	public CUserInfo LoadUserInfo() {
		return this.LoadUserInfo(KDefine.G_DATA_P_USER_INFO);
	}

	/** 유저 정보를 로드한다 */
	public CUserInfo LoadUserInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
#if MSG_PACK_ENABLE
			this.UserInfo = CFunc.ReadMsgPackObj<CUserInfo>(a_oFilePath);
#else
			this.UserInfo = CFunc.ReadJSONObj<CUserInfo>(a_oFilePath);
#endif			// #if MSG_PACK_ENABLE

			CAccess.Assert(this.UserInfo != null);
		}

		return this.UserInfo;
	}

	/** 유저 정보를 저장한다 */
	public void SaveUserInfo() {
		this.SaveUserInfo(KDefine.G_DATA_P_USER_INFO);
	}

	/** 유저 정보를 저장한다 */
	public void SaveUserInfo(string a_oFilePath) {
#if MSG_PACK_ENABLE
		CFunc.WriteMsgPackObj(a_oFilePath, this.UserInfo);
#else
		CFunc.WriteJSONObj(a_oFilePath, this.UserInfo);
#endif			// #if MSG_PACK_ENABLE
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
