using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
using MessagePack;
using Newtonsoft.Json;

/** 유저 타겟 정보 */
[Union(0, typeof(CUserItemInfo))]
[Union(1, typeof(CUserSkillInfo))]
[Union(2, typeof(CUserObjInfo))]
[MessagePackObject][System.Serializable]
public abstract partial class CUserTargetInfo : CBaseInfo {
	#region 변수
	[Key(131)] public Dictionary<EAbilityKinds, STAbilityValInfo> m_oAbilityValInfoDict = new Dictionary<EAbilityKinds, STAbilityValInfo>();
	#endregion			// 변수

	#region 상수
	private const string KEY_NUMS = "Nums";
	private const string KEY_OWNER_KINDS = "OwnerKinds";
	private const string KEY_OWNER_TARGET_KINDS = "OwnerTargetKinds";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public long Nums { get { return long.Parse(m_oStrDict.GetValueOrDefault(KEY_NUMS, KCDefine.B_STR_0_INT)); } set { m_oStrDict.ExReplaceVal(KEY_NUMS, $"{value}"); } }
	[JsonIgnore][IgnoreMember] public int OwnerKinds { get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_OWNER_KINDS, $"{KCDefine.B_IDX_INVALID}")); } set { m_oStrDict.ExReplaceVal(KEY_OWNER_KINDS, $"{value}"); } }
	[JsonIgnore][IgnoreMember] public ETargetKinds OwnerTargetKinds { get { return (ETargetKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_OWNER_TARGET_KINDS, $"{(int)ETargetKinds.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_OWNER_TARGET_KINDS, $"{(int)value}"); } }
	#endregion			// 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		m_oAbilityValInfoDict = m_oAbilityValInfoDict ?? new Dictionary<EAbilityKinds, STAbilityValInfo>();
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CUserTargetInfo(System.Version a_stVer) : base(a_stVer) {
		// Do Something
	}
	#endregion			// 함수
}

/** 유저 아이템 정보 */
[MessagePackObject][System.Serializable]
public partial class CUserItemInfo : CUserTargetInfo {
	#region 상수
	private const string KEY_ITEM_KINDS = "ItemKinds";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public EItemKinds ItemKinds { get { return (EItemKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_ITEM_KINDS, $"{(int)EItemKinds.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_ITEM_KINDS, $"{(int)value}"); } }

	[JsonIgnore][IgnoreMember] public override bool IsIgnoreVer => true;
	[JsonIgnore][IgnoreMember] public override bool IsIgnoreSaveTime => true;
	#endregion			// 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CUserItemInfo() : base(KDefine.G_VER_USER_ITEM_INFO) {
		// Do Something
	}
	#endregion			// 함수
}

/** 유저 스킬 정보 */
[MessagePackObject][System.Serializable]
public partial class CUserSkillInfo : CUserTargetInfo {
	#region 상수
	private const string KEY_SKILL_KINDS = "SkillKinds";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public ESkillKinds SkillKinds { get { return (ESkillKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_SKILL_KINDS, $"{(int)ESkillKinds.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_SKILL_KINDS, $"{(int)value}"); } }

	[JsonIgnore][IgnoreMember] public override bool IsIgnoreVer => true;
	[JsonIgnore][IgnoreMember] public override bool IsIgnoreSaveTime => true;
	#endregion			// 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CUserSkillInfo() : base(KDefine.G_VER_USER_SKILL_INFO) {
		// Do Something
	}
	#endregion			// 함수
}

/** 유저 객체 정보 */
[MessagePackObject][System.Serializable]
public partial class CUserObjInfo : CUserTargetInfo {
	#region 상수
	private const string KEY_OBJ_KINDS = "ObjKinds";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public EObjKinds ObjKinds { get { return (EObjKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_OBJ_KINDS, $"{(int)EObjKinds.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_OBJ_KINDS, $"{(int)value}"); } }

	[JsonIgnore][IgnoreMember] public override bool IsIgnoreVer => true;
	[JsonIgnore][IgnoreMember] public override bool IsIgnoreSaveTime => true;
	#endregion			// 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CUserObjInfo() : base(KDefine.G_VER_USER_OBJ_INFO) {
		// Do Something
	}
	#endregion			// 함수
}

/** 유저 정보 */
[MessagePackObject][System.Serializable]
public partial class CUserInfo : CBaseInfo {
	#region 변수
	[Key(91)] public List<CUserItemInfo> m_oUserItemInfoList = new List<CUserItemInfo>();
	[Key(92)] public List<CUserSkillInfo> m_oUserSkillInfoList = new List<CUserSkillInfo>();
	[Key(93)] public List<CUserObjInfo> m_oUserObjInfoList = new List<CUserObjInfo>();
	#endregion			// 변수

	#region 상수
	private const string KEY_USER_ITEM_INFO_VER = "UserItemInfoVer";
	private const string KEY_USER_SKILL_INFO_VER = "UserSkillInfoVer";
	private const string KEY_USER_OBJ_INFO_VER = "UserObjInfoVer";
	private const string KEY_ABILITY_VAL_INFO_VER = "AbilityValInfoVer";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public System.Version UserItemInfoVer { get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_USER_ITEM_INFO_VER, KCDefine.B_DEF_VER)); } set { m_oStrDict.ExReplaceVal(KEY_USER_ITEM_INFO_VER, value.ToString(KCDefine.B_VAL_3_INT)); } }
	[JsonIgnore][IgnoreMember] public System.Version UserSkillInfoVer { get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_USER_SKILL_INFO_VER, KCDefine.B_DEF_VER)); } set { m_oStrDict.ExReplaceVal(KEY_USER_SKILL_INFO_VER, value.ToString(KCDefine.B_VAL_3_INT)); } }
	[JsonIgnore][IgnoreMember] public System.Version UserObjInfoVer { get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_USER_OBJ_INFO_VER, KCDefine.B_DEF_VER)); } set { m_oStrDict.ExReplaceVal(KEY_USER_OBJ_INFO_VER, value.ToString(KCDefine.B_VAL_3_INT)); } }
	[JsonIgnore][IgnoreMember] public System.Version AbilityValInfoVer { get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_ABILITY_VAL_INFO_VER, KCDefine.B_DEF_VER)); } set { m_oStrDict.ExReplaceVal(KEY_ABILITY_VAL_INFO_VER, value.ToString(KCDefine.B_VAL_3_INT)); } }
	#endregion			// 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();

		m_oUserItemInfoList = m_oUserItemInfoList ?? new List<CUserItemInfo>();
		m_oUserSkillInfoList = m_oUserSkillInfoList ?? new List<CUserSkillInfo>();
		m_oUserObjInfoList = m_oUserObjInfoList ?? new List<CUserObjInfo>();

		for(int i = 0; i < m_oUserItemInfoList.Count; ++i) {
			this.SetupUserItemInfo(m_oUserItemInfoList[i]);
		}

		for(int i = 0; i < m_oUserSkillInfoList.Count; ++i) {
			this.SetupUserSkillInfo(m_oUserSkillInfoList[i]);
		}

		for(int i = 0; i < m_oUserObjInfoList.Count; ++i) {
			this.SetupUserObjInfo(m_oUserObjInfoList[i]);
		}

		// 버전이 다를 경우
		if(this.Ver.CompareTo(KDefine.G_VER_USER_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CUserInfo() : base(KDefine.G_VER_USER_INFO) {
		this.UserItemInfoVer = KDefine.G_VER_USER_ITEM_INFO;
		this.UserSkillInfoVer = KDefine.G_VER_USER_SKILL_INFO;
		this.UserObjInfoVer = KDefine.G_VER_USER_OBJ_INFO;
		this.AbilityValInfoVer = KDefine.G_VER_ABILITY_VER_INFO;
	}

	/** 유저 아이템 정보를 설정한다 */
	protected virtual void SetupUserItemInfo(CUserItemInfo a_oUserItemInfo) {
		// 버전이 다를 경우
		if(this.UserItemInfoVer.CompareTo(KDefine.G_VER_USER_ITEM_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}

	/** 유저 스킬 정보를 설정한다 */
	protected virtual void SetupUserSkillInfo(CUserSkillInfo a_oUserSkillInfo) {
		// 버전이 다를 경우
		if(this.UserSkillInfoVer.CompareTo(KDefine.G_VER_USER_SKILL_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}

	/** 유저 객체 정보를 설정한다 */
	protected virtual void SetupUserObjInfo(CUserObjInfo a_oUserObjInfo) {
		for(int i = 0; i < a_oUserObjInfo.m_oAbilityValInfoDict.Keys.Count; ++i) {
			// 버전이 다를 경우
			if(this.AbilityValInfoVer.CompareTo(KDefine.G_VER_ABILITY_VER_INFO) < KCDefine.B_COMPARE_EQUALS) {
				// Do Something	
			}
		}

		// 버전이 다를 경우
		if(this.UserObjInfoVer.CompareTo(KDefine.G_VER_USER_OBJ_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// 함수
}

/** 유저 정보 저장소 */
public partial class CUserInfoStorage : CSingleton<CUserInfoStorage> {
	#region 프로퍼티
	public CUserInfo UserInfo { get; private set; } = new CUserInfo();
	public bool IsPurchaseRemoveAds => this.TryGetUserItemInfo(EItemKinds.NON_CONSUMABLE_REMOVE_ADS, out CUserItemInfo oUserItemInfo);
	public long NumCoins => this.TryGetUserItemInfo(EItemKinds.GOODS_COINS, out CUserItemInfo oUserItemInfo) ? oUserItemInfo.Nums : KCDefine.B_VAL_0_INT;
	public long NumCoinsBoxCoins => this.TryGetUserItemInfo(EItemKinds.GOODS_COINS_BOX_COINS, out CUserItemInfo oUserItemInfo) ? oUserItemInfo.Nums : KCDefine.B_VAL_0_INT;
	#endregion            // 프로퍼티

	#region 함수
	/** 유저 정보를 리셋한다 */
	public virtual void ResetUserInfo(string a_oJSONStr) {
		CFunc.ShowLog($"CUserInfoStorage.ResetUserInfo: {a_oJSONStr}");
		this.UserInfo = a_oJSONStr.ExMsgPackBase64StrToObj<CUserInfo>();

		CAccess.Assert(this.UserInfo != null);
	}

	/** 유저 아이템 개수를 반환한다 */
	public long GetNumUserItems(EItemKinds a_eItemKinds) {
		return this.UserInfo.m_oUserItemInfoList.Sum((a_oUserItemInfo) => (a_oUserItemInfo.ItemKinds == a_eItemKinds) ? a_oUserItemInfo.Nums : KCDefine.B_VAL_0_INT);
	}

	/** 유저 스킬 개수를 반환한다 */
	public long GetNumUserSkills(ESkillKinds a_eSkillKinds) {
		return this.UserInfo.m_oUserSkillInfoList.Sum((a_oUserSkillInfo) => (a_oUserSkillInfo.SkillKinds == a_eSkillKinds) ? a_oUserSkillInfo.Nums : KCDefine.B_VAL_0_INT);
	}

	/** 유저 객체 개수를 반환한다 */
	public long GetNumUserObjs(EObjKinds a_eObjKinds) {
		return this.UserInfo.m_oUserObjInfoList.Sum((a_oUserObjInfo) => (a_oUserObjInfo.ObjKinds == a_eObjKinds) ? a_oUserObjInfo.Nums : KCDefine.B_VAL_0_INT);
	}

	/** 유저 아이템 정보를 반환한다 */
	public CUserItemInfo GetUserItemInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetUserItemInfo(a_eItemKinds, out CUserItemInfo oUserItemInfo);
		CAccess.Assert(bIsValid);

		return oUserItemInfo;
	}

	/** 유저 스킬 정보를 반환한다 */
	public CUserSkillInfo GetUserSkillInfo(ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetUserSkillInfo(a_eSkillKinds, out CUserSkillInfo oUserSkillInfo);
		CAccess.Assert(bIsValid);

		return oUserSkillInfo;
	}

	/** 유저 객체 정보를 반환한다 */
	public CUserObjInfo GetUserObjInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetUserObjInfo(a_eObjKinds, out CUserObjInfo oUserObjInfo);
		CAccess.Assert(bIsValid);

		return oUserObjInfo;
	}

	/** 유저 아이템 정보를 반환한다 */
	public bool TryGetUserItemInfo(EItemKinds a_eItemKinds, out CUserItemInfo a_oOutUserItemInfo) {
		a_oOutUserItemInfo = this.UserInfo.m_oUserItemInfoList.ExGetVal((a_oUserItemInfo) => a_oUserItemInfo.ItemKinds == a_eItemKinds, null);
		return a_oOutUserItemInfo != null;
	}

	/** 유저 스킬 정보를 반환한다 */
	public bool TryGetUserSkillInfo(ESkillKinds a_eSkillKinds, out CUserSkillInfo a_oOutUserSkillInfo) {
		a_oOutUserSkillInfo = this.UserInfo.m_oUserSkillInfoList.ExGetVal((a_oUserItemInfo) => a_oUserItemInfo.SkillKinds == a_eSkillKinds, null);
		return a_oOutUserSkillInfo != null;
	}

	/** 유저 객체 정보를 반환한다 */
	public bool TryGetUserObjInfo(EObjKinds a_eObjKinds, out CUserObjInfo a_oOutUserObjInfo) {
		a_oOutUserObjInfo = this.UserInfo.m_oUserObjInfoList.ExGetVal((a_oUserItemInfo) => a_oUserItemInfo.ObjKinds == a_eObjKinds, null);
		return a_oOutUserObjInfo != null;
	}
	
	/** 유저 아이템 정보를 추가한다 */
	public void AddUserItemInfo(CUserItemInfo a_oUserItemInfo, bool a_bIsDuplicate = false) {
		// 유저 아이템 정보 추가가 가능 할 경우
		if(a_bIsDuplicate || !this.TryGetUserItemInfo(a_oUserItemInfo.ItemKinds, out CUserItemInfo oUserItemInfo)) {
			this.UserInfo.m_oUserItemInfoList.Add(a_oUserItemInfo);
		}
	}

	/** 유저 스킬 정보를 추가한다 */
	public void AddUserSkillInfo(CUserSkillInfo a_oUserSkillInfo, bool a_bIsDuplicate = false) {
		// 유저 스킬 정보 추가가 가능 할 경우
		if(a_bIsDuplicate || !this.TryGetUserSkillInfo(a_oUserSkillInfo.SkillKinds, out CUserSkillInfo oUserSkillInfo)) {
			this.UserInfo.m_oUserSkillInfoList.Add(a_oUserSkillInfo);
		}
	}

	/** 유저 객체 정보를 추가한다 */
	public void AddUserObjInfo(CUserObjInfo a_oUserObjInfo, bool a_bIsDuplicate = false) {
		// 유저 객체 정보 추가가 가능 할 경우
		if(a_bIsDuplicate || !this.TryGetUserObjInfo(a_oUserObjInfo.ObjKinds, out CUserObjInfo oUserObjInfo)) {
			this.UserInfo.m_oUserObjInfoList.Add(a_oUserObjInfo);
		}
	}

	/** 유저 정보를 로드한다 */
	public CUserInfo LoadUserInfo() {
#if MSG_PACK_ENABLE || NEWTON_SOFT_JSON_MODULE_ENABLE
		return this.LoadUserInfo(KDefine.G_DATA_P_USER_INFO);
#else
		return null;
#endif			// #if MSG_PACK_ENABLE || NEWTON_SOFT_JSON_MODULE_ENABLE
	}

	/** 유저 정보를 로드한다 */
	public CUserInfo LoadUserInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
#if MSG_PACK_ENABLE
			this.UserInfo = CFunc.ReadMsgPackObj<CUserInfo>(a_oFilePath);
#elif NEWTON_SOFT_JSON_MODULE_ENABLE
			this.UserInfo = CFunc.ReadJSONObj<CUserInfo>(a_oFilePath);
#endif			// #if MSG_PACK_ENABLE

			CAccess.Assert(this.UserInfo != null);
		}

		return this.UserInfo;
	}

	/** 유저 정보를 저장한다 */
	public void SaveUserInfo() {
#if MSG_PACK_ENABLE || NEWTON_SOFT_JSON_MODULE_ENABLE
		this.SaveUserInfo(KDefine.G_DATA_P_USER_INFO);
#endif			// #if MSG_PACK_ENABLE || NEWTON_SOFT_JSON_MODULE_ENABLE
	}

	/** 유저 정보를 저장한다 */
	public void SaveUserInfo(string a_oFilePath) {
#if MSG_PACK_ENABLE
		CFunc.WriteMsgPackObj(a_oFilePath, this.UserInfo);
#elif NEWTON_SOFT_JSON_MODULE_ENABLE
		CFunc.WriteJSONObj(a_oFilePath, this.UserInfo);
#endif			// #if MSG_PACK_ENABLE
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
