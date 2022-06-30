using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
using MessagePack;
using Newtonsoft.Json;

/** 유저 아이템 정보 */
[MessagePackObject][System.Serializable]
public partial class CUserItemInfo : CBaseInfo {
	#region 상수
	private const string KEY_LV = "LV";
	private const string KEY_NUM_ITEMS = "NumItems";
	private const string KEY_ITEM_KINDS = "ItemKinds";
	private const string KEY_OWNER_TYPE = "OwnerType";
	private const string KEY_OWNER_KINDS = "OwnerKinds";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public long LV { get { return long.Parse(m_oStrDict.GetValueOrDefault(KEY_LV, KCDefine.B_STR_0_INT)); } set { m_oStrDict.ExReplaceVal(KEY_LV, $"{value}"); } }
	[JsonIgnore][IgnoreMember] public long NumItems { get { return long.Parse(m_oStrDict.GetValueOrDefault(KEY_NUM_ITEMS, KCDefine.B_STR_0_INT)); } set { m_oStrDict.ExReplaceVal(KEY_NUM_ITEMS, $"{value}"); } }
	[JsonIgnore][IgnoreMember] public EItemKinds ItemKinds { get { return (EItemKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_ITEM_KINDS, $"{(int)EItemKinds.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_ITEM_KINDS, $"{(int)value}"); } }
	[JsonIgnore][IgnoreMember] public EOwnerType OwnerType { get { return (EOwnerType)int.Parse(m_oStrDict.GetValueOrDefault(KEY_OWNER_TYPE, $"{(int)EOwnerType.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_OWNER_TYPE, $"{(int)value}"); } }
	[JsonIgnore][IgnoreMember] public int OwnerKinds { get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_OWNER_KINDS, $"{KCDefine.B_IDX_INVALID}")); } set { m_oStrDict.ExReplaceVal(KEY_OWNER_KINDS, $"{value}"); } }

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
public partial class CUserSkillInfo : CBaseInfo {
	#region 상수
	private const string KEY_LV = "LV";
	private const string KEY_SKILL_KINDS = "SkillKinds";
	private const string KEY_OWNER_TYPE = "OwnerType";
	private const string KEY_OWNER_KINDS = "OwnerKinds";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public long LV { get { return long.Parse(m_oStrDict.GetValueOrDefault(KEY_LV, KCDefine.B_STR_0_INT)); } set { m_oStrDict.ExReplaceVal(KEY_LV, $"{value}"); } }
	[JsonIgnore][IgnoreMember] public ESkillKinds SkillKinds { get { return (ESkillKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_SKILL_KINDS, $"{(int)ESkillKinds.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_SKILL_KINDS, $"{(int)value}"); } }
	[JsonIgnore][IgnoreMember] public EOwnerType OwnerType { get { return (EOwnerType)int.Parse(m_oStrDict.GetValueOrDefault(KEY_OWNER_TYPE, $"{(int)EOwnerType.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_OWNER_TYPE, $"{(int)value}"); } }
	[JsonIgnore][IgnoreMember] public int OwnerKinds { get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_OWNER_KINDS, $"{KCDefine.B_IDX_INVALID}")); } set { m_oStrDict.ExReplaceVal(KEY_OWNER_KINDS, $"{value}"); } }

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
public partial class CUserObjInfo : CBaseInfo {
	#region 상수
	private const string KEY_LV = "LV";
	private const string KEY_OBJ_KINDS = "ObjKinds";
	private const string KEY_OWNER_TYPE = "OwnerType";
	private const string KEY_OWNER_KINDS = "OwnerKinds";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public long LV { get { return long.Parse(m_oStrDict.GetValueOrDefault(KEY_LV, KCDefine.B_STR_0_INT)); } set { m_oStrDict.ExReplaceVal(KEY_LV, $"{value}"); } }
	[JsonIgnore][IgnoreMember] public EObjKinds ObjKinds { get { return (EObjKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_OBJ_KINDS, $"{(int)EObjKinds.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_OBJ_KINDS, $"{(int)value}"); } }
	[JsonIgnore][IgnoreMember] public EOwnerType OwnerType { get { return (EOwnerType)int.Parse(m_oStrDict.GetValueOrDefault(KEY_OWNER_TYPE, $"{(int)EOwnerType.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_OWNER_TYPE, $"{(int)value}"); } }
	[JsonIgnore][IgnoreMember] public int OwnerKinds { get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_OWNER_KINDS, $"{KCDefine.B_IDX_INVALID}")); } set { m_oStrDict.ExReplaceVal(KEY_OWNER_KINDS, $"{value}"); } }

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
public partial class CUserInfoStorage : CSingleton<CUserInfoStorage> {
	#region 프로퍼티
	public CUserInfo UserInfo { get; private set; } = new CUserInfo();
	public bool IsPurchaseRemoveAds => this.TryGetUserItemInfo(EItemKinds.NON_CONSUMABLE_REMOVE_ADS, out CUserItemInfo oUserItemInfo);
	public long NumCoins => this.TryGetUserItemInfo(EItemKinds.GOODS_COINS, out CUserItemInfo oUserItemInfo) ? oUserItemInfo.NumItems : KCDefine.B_VAL_0_INT;
	public long NumCoinsBoxCoins => this.TryGetUserItemInfo(EItemKinds.GOODS_COINS_BOX_COINS, out CUserItemInfo oUserItemInfo) ? oUserItemInfo.NumItems : KCDefine.B_VAL_0_INT;
	#endregion            // 프로퍼티

	#region 함수
	/** 유저 정보를 리셋한다 */
	public virtual void ResetUserInfo(string a_oJSONStr) {
		CFunc.ShowLog($"CUserInfoStorage.ResetUserInfo: {a_oJSONStr}");
		CAccess.Assert(a_oJSONStr.ExIsValid());

		this.UserInfo = a_oJSONStr.ExMsgPackBase64StrToObj<CUserInfo>();
		CAccess.Assert(this.UserInfo != null);
	}

	/** 유저 아이템 개수를 반환한다 */
	public long GetNumUserItems(EItemKinds a_eItemKinds) {
		return this.UserInfo.m_oUserItemInfoList.Sum((a_oUserItemInfo) => (a_oUserItemInfo.ItemKinds == a_eItemKinds) ? a_oUserItemInfo.NumItems : KCDefine.B_VAL_0_INT);
	}

	/** 유저 아이템 정보를 반환한다 */
	public CUserItemInfo GetUserItemInfo(EItemKinds a_eItemKinds, bool a_bIsAutoCreate = false) {
		bool bIsValid = this.TryGetUserItemInfo(a_eItemKinds, out CUserItemInfo oUserItemInfo, a_bIsAutoCreate);
		CAccess.Assert(bIsValid);

		return oUserItemInfo;
	}

	/** 유저 스킬 정보를 반환한다 */
	public CUserSkillInfo GetUserSkillInfo(ESkillKinds a_eSkillKinds, bool a_bIsAutoCreate = false) {
		bool bIsValid = this.TryGetUserSkillInfo(a_eSkillKinds, out CUserSkillInfo oUserSkillInfo, a_bIsAutoCreate);
		CAccess.Assert(bIsValid);

		return oUserSkillInfo;
	}

	/** 유저 객체 정보를 반환한다 */
	public CUserObjInfo GetUserObjInfo(EObjKinds a_eObjKinds, bool a_bIsAutoCreate = false) {
		bool bIsValid = this.TryGetUserObjInfo(a_eObjKinds, out CUserObjInfo oUserObjInfo, a_bIsAutoCreate);
		CAccess.Assert(bIsValid);

		return oUserObjInfo;
	}

	/** 유저 아이템 정보를 반환한다 */
	public bool TryGetUserItemInfo(EItemKinds a_eItemKinds, out CUserItemInfo a_oOutUserItemInfo, bool a_bIsAutoCreate = false) {
		a_oOutUserItemInfo = this.UserInfo.m_oUserItemInfoList.ExGetVal((a_oUserItemInfo) => a_oUserItemInfo.ItemKinds == a_eItemKinds, null);

		// 유저 아이템 정보가 없을 경우
		if(a_bIsAutoCreate && a_oOutUserItemInfo == null) {
			a_oOutUserItemInfo = Factory.MakeUserItemInfo(a_eItemKinds);
			this.AddUserItemInfo(a_oOutUserItemInfo, CItemInfoTable.Inst.GetItemInfo(a_eItemKinds).m_stCommonInfo.m_bIsDuplicate);
		}

		return a_oOutUserItemInfo != null;
	}

	/** 유저 스킬 정보를 반환한다 */
	public bool TryGetUserSkillInfo(ESkillKinds a_eSkillKinds, out CUserSkillInfo a_oOutUserSkillInfo, bool a_bIsAutoCreate = false) {
		a_oOutUserSkillInfo = this.UserInfo.m_oUserSkillInfoList.ExGetVal((a_oUserItemInfo) => a_oUserItemInfo.SkillKinds == a_eSkillKinds, null);

		// 유저 스킬 정보가 없을 경우
		if(a_bIsAutoCreate && a_oOutUserSkillInfo == null) {
			a_oOutUserSkillInfo = Factory.MakeUserSkillInfo(a_eSkillKinds);
			this.AddUserSkillInfo(a_oOutUserSkillInfo, CSkillInfoTable.Inst.GetSkillInfo(a_eSkillKinds).m_stCommonInfo.m_bIsDuplicate);
		}

		return a_oOutUserSkillInfo != null;
	}

	/** 유저 객체 정보를 반환한다 */
	public bool TryGetUserObjInfo(EObjKinds a_eObjKinds, out CUserObjInfo a_oOutUserObjInfo, bool a_bIsAutoCreate = false) {
		a_oOutUserObjInfo = this.UserInfo.m_oUserObjInfoList.ExGetVal((a_oUserItemInfo) => a_oUserItemInfo.ObjKinds == a_eObjKinds, null);

		// 유저 객체 정보가 없을 경우
		if(a_bIsAutoCreate && a_oOutUserObjInfo == null) {
			a_oOutUserObjInfo = Factory.MakeUserObjInfo(a_eObjKinds);
			this.AddUserObjInfo(a_oOutUserObjInfo, CObjInfoTable.Inst.GetObjInfo(a_eObjKinds).m_stCommonInfo.m_bIsDuplicate);
		}

		return a_oOutUserObjInfo != null;
	}

	/** 유저 아이템 레벨을 추가한다 */
	public void AddUserItemLV(EItemKinds a_eItemKinds, long a_nUserItemLV, bool a_bIsAutoCreate = false) {
		var oUserItemInfo = this.GetUserItemInfo(a_eItemKinds, a_bIsAutoCreate);
		oUserItemInfo.LV = System.Math.Clamp(oUserItemInfo.LV + a_nUserItemLV, KCDefine.B_VAL_0_INT, long.MaxValue);
	}

	/** 유저 스킬 레벨을 추가한다 */
	public void AddUserSkillLV(ESkillKinds a_eSkillKinds, long a_nUserSkillLV, bool a_bIsAutoCreate = false) {
		var oUserSkillInfo = this.GetUserSkillInfo(a_eSkillKinds, a_bIsAutoCreate);
		oUserSkillInfo.LV = System.Math.Clamp(oUserSkillInfo.LV + a_nUserSkillLV, KCDefine.B_VAL_0_INT, long.MaxValue);
	}

	/** 유저 객체 레벨을 추가한다 */
	public void AddUserObjLV(EObjKinds a_eObjKinds, long a_nUserObjLV, bool a_bIsAutoCreate = false) {
		var oUserObjInfo = this.GetUserObjInfo(a_eObjKinds, a_bIsAutoCreate);
		oUserObjInfo.LV = System.Math.Clamp(oUserObjInfo.LV + a_nUserObjLV, KCDefine.B_VAL_0_INT, long.MaxValue);
	}

	/** 유저 아이템 개수를 추가한다 */
	public void AddNumUserItems(EItemKinds a_eItemKinds, long a_nNumUserItems, bool a_bIsAutoCreate = false) {
		var oUserItemInfo = this.GetUserItemInfo(a_eItemKinds, a_bIsAutoCreate);
		oUserItemInfo.NumItems = System.Math.Clamp(oUserItemInfo.NumItems + a_nNumUserItems, KCDefine.B_VAL_0_INT, long.MaxValue);
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
