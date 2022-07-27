#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
using MessagePack;
using Newtonsoft.Json;

/** 타겟 정보 */
[Union(0, typeof(CItemInfo))]
[Union(1, typeof(CSkillInfo))]
[Union(2, typeof(CObjInfo))]
[MessagePackObject][System.Serializable]
public abstract partial class CTargetInfo : CBaseInfo {
	#region 변수
	[Key(131)] public Dictionary<ulong, STTargetInfo> m_oAbilityTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
	#endregion			// 변수

	#region 상수
	private const string KEY_OWNER_KINDS = "OwnerKinds";
	private const string KEY_OWNER_TARGET_KINDS = "OwnerTargetKinds";
	#endregion			// 상수

	#region 프로퍼티
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
		m_oAbilityTargetInfoDict = m_oAbilityTargetInfoDict ?? new Dictionary<ulong, STTargetInfo>();
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CTargetInfo(System.Version a_stVer) : base(a_stVer) {
		// Do Something
	}
	#endregion			// 함수
}

/** 아이템 정보 */
[MessagePackObject][System.Serializable]
public partial class CItemInfo : CTargetInfo {
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
	public CItemInfo() : base(KDefine.G_VER_ITEM_INFO) {
		// Do Something
	}
	#endregion			// 함수
}

/** 스킬 정보 */
[MessagePackObject][System.Serializable]
public partial class CSkillInfo : CTargetInfo {
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
	public CSkillInfo() : base(KDefine.G_VER_SKILL_INFO) {
		// Do Something
	}
	#endregion			// 함수
}

/** 객체 정보 */
[MessagePackObject][System.Serializable]
public partial class CObjInfo : CTargetInfo {
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
	public CObjInfo() : base(KDefine.G_VER_OBJ_INFO) {
		// Do Something
	}
	#endregion			// 함수
}

/** 유저 정보 */
[MessagePackObject][System.Serializable]
public partial class CUserInfo : CBaseInfo {
	#region 변수
	[Key(91)] public List<CItemInfo> m_oItemInfoList = new List<CItemInfo>();
	[Key(92)] public List<CSkillInfo> m_oSkillInfoList = new List<CSkillInfo>();
	[Key(93)] public List<CObjInfo> m_oObjInfoList = new List<CObjInfo>();
	#endregion			// 변수

	#region 상수
	private const string KEY_LOGIN_TYPE = "LoginType";

	private const string KEY_ITEM_INFO_VER = "ItemInfoVer";
	private const string KEY_SKILL_INFO_VER = "SkillInfoVer";
	private const string KEY_OBJ_INFO_VER = "ObjInfoVer";
	private const string KEY_ABILITY_TARGET_INFO_VER = "AbilityTargetInfoVer";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public ELoginType LoginType { get { return (ELoginType)int.Parse(m_oStrDict.GetValueOrDefault(KEY_LOGIN_TYPE, $"{(int)ELoginType.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_LOGIN_TYPE, $"{(int)value}"); } }

	[JsonIgnore][IgnoreMember] public System.Version ItemInfoVer { get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_ITEM_INFO_VER, KCDefine.B_DEF_VER)); } set { m_oStrDict.ExReplaceVal(KEY_ITEM_INFO_VER, value.ToString(KCDefine.B_VAL_3_INT)); } }
	[JsonIgnore][IgnoreMember] public System.Version SkillInfoVer { get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_SKILL_INFO_VER, KCDefine.B_DEF_VER)); } set { m_oStrDict.ExReplaceVal(KEY_SKILL_INFO_VER, value.ToString(KCDefine.B_VAL_3_INT)); } }
	[JsonIgnore][IgnoreMember] public System.Version ObjInfoVer { get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_OBJ_INFO_VER, KCDefine.B_DEF_VER)); } set { m_oStrDict.ExReplaceVal(KEY_OBJ_INFO_VER, value.ToString(KCDefine.B_VAL_3_INT)); } }
	[JsonIgnore][IgnoreMember] public System.Version AbilityTargetInfoVer { get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_ABILITY_TARGET_INFO_VER, KCDefine.B_DEF_VER)); } set { m_oStrDict.ExReplaceVal(KEY_ABILITY_TARGET_INFO_VER, value.ToString(KCDefine.B_VAL_3_INT)); } }
	#endregion			// 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();

		m_oItemInfoList = m_oItemInfoList ?? new List<CItemInfo>();
		m_oSkillInfoList = m_oSkillInfoList ?? new List<CSkillInfo>();
		m_oObjInfoList = m_oObjInfoList ?? new List<CObjInfo>();

		for(int i = 0; i < m_oItemInfoList.Count; ++i) {
			this.SetupItemInfo(m_oItemInfoList[i]);
		}

		for(int i = 0; i < m_oSkillInfoList.Count; ++i) {
			this.SetupSkillInfo(m_oSkillInfoList[i]);
		}

		for(int i = 0; i < m_oObjInfoList.Count; ++i) {
			this.SetupObjInfo(m_oObjInfoList[i]);
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
		this.ItemInfoVer = KDefine.G_VER_ITEM_INFO;
		this.SkillInfoVer = KDefine.G_VER_SKILL_INFO;
		this.ObjInfoVer = KDefine.G_VER_OBJ_INFO;
		this.AbilityTargetInfoVer = KDefine.G_VER_ABILITY_TARGET_INFO;
	}

	/** 아이템 정보를 설정한다 */
	protected virtual void SetupItemInfo(CItemInfo a_oItemInfo) {
		// 버전이 다를 경우
		if(this.ItemInfoVer.CompareTo(KDefine.G_VER_ITEM_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}

	/** 스킬 정보를 설정한다 */
	protected virtual void SetupSkillInfo(CSkillInfo a_oSkillInfo) {
		// 버전이 다를 경우
		if(this.SkillInfoVer.CompareTo(KDefine.G_VER_SKILL_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}

	/** 객체 정보를 설정한다 */
	protected virtual void SetupObjInfo(CObjInfo a_oObjInfo) {
		for(int i = 0; i < a_oObjInfo.m_oAbilityTargetInfoDict.Keys.Count; ++i) {
			// 버전이 다를 경우
			if(this.AbilityTargetInfoVer.CompareTo(KDefine.G_VER_ABILITY_TARGET_INFO) < KCDefine.B_COMPARE_EQUALS) {
				// Do Something	
			}
		}

		// 버전이 다를 경우
		if(this.ObjInfoVer.CompareTo(KDefine.G_VER_OBJ_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// 함수
}

/** 유저 정보 저장소 */
public partial class CUserInfoStorage : CSingleton<CUserInfoStorage> {
	#region 프로퍼티
	public CUserInfo UserInfo { get; private set; } = new CUserInfo();
	public bool IsPurchaseRemoveAds => this.TryGetItemInfo(EItemKinds.NON_CONSUMABLE_REMOVE_ADS, out CItemInfo oItemInfo);
	public long NumCoins => (this.TryGetItemInfo(EItemKinds.GOODS_COINS, out CItemInfo oItemInfo) && oItemInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, out STTargetInfo stTargetInfo)) ? stTargetInfo.m_stValInfo01.m_nVal : KCDefine.B_VAL_0_INT;
	public long NumCoinsBoxCoins => (this.TryGetItemInfo(EItemKinds.GOODS_COINS_BOX_COINS, out CItemInfo oItemInfo) && oItemInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, out STTargetInfo stTargetInfo)) ? stTargetInfo.m_stValInfo01.m_nVal : KCDefine.B_VAL_0_INT;
	#endregion            // 프로퍼티

	#region 함수
	/** 유저 정보를 리셋한다 */
	public virtual void ResetUserInfo(string a_oJSONStr) {
		CFunc.ShowLog($"CUserInfoStorage.ResetUserInfo: {a_oJSONStr}");
		this.UserInfo = a_oJSONStr.ExMsgPackBase64StrToObj<CUserInfo>();

		CAccess.Assert(this.UserInfo != null);
	}

	/** 아이템 개수를 반환한다 */
	public long GetNumItems(EItemKinds a_eItemKinds) {
		return this.UserInfo.m_oItemInfoList.Sum((a_oItemInfo) => (a_oItemInfo.ItemKinds == a_eItemKinds && a_oItemInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, out STTargetInfo stTargetInfo)) ? stTargetInfo.m_stValInfo01.m_nVal : KCDefine.B_VAL_0_INT);
	}

	/** 스킬 개수를 반환한다 */
	public long GetNumSkills(ESkillKinds a_eSkillKinds) {
		return this.UserInfo.m_oSkillInfoList.Sum((a_oSkillInfo) => (a_oSkillInfo.SkillKinds == a_eSkillKinds && a_oSkillInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, out STTargetInfo stTargetInfo)) ? stTargetInfo.m_stValInfo01.m_nVal : KCDefine.B_VAL_0_INT);
	}

	/** 객체 개수를 반환한다 */
	public long GetNumObjs(EObjKinds a_eObjKinds) {
		return this.UserInfo.m_oObjInfoList.Sum((a_oObjInfo) => (a_oObjInfo.ObjKinds == a_eObjKinds && a_oObjInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, out STTargetInfo stTargetInfo)) ? stTargetInfo.m_stValInfo01.m_nVal : KCDefine.B_VAL_0_INT);
	}

	/** 아이템 정보를 반환한다 */
	public CItemInfo GetItemInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetItemInfo(a_eItemKinds, out CItemInfo oItemInfo);
		CAccess.Assert(bIsValid);

		return oItemInfo;
	}

	/** 아이템 어빌리티 타겟 정보를 반환한다 */
	public STTargetInfo GetItemAbilityTargetInfo(EItemKinds a_eItemKinds, EAbilityKinds a_eAbilityKinds) {
		bool bIsValid = this.TryGetItemAbilityTargetInfo(a_eItemKinds, a_eAbilityKinds, out STTargetInfo stTargetInfo);
		CAccess.Assert(bIsValid);

		return stTargetInfo;
	}

	/** 스킬 정보를 반환한다 */
	public CSkillInfo GetSkillInfo(ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetSkillInfo(a_eSkillKinds, out CSkillInfo oSkillInfo);
		CAccess.Assert(bIsValid);

		return oSkillInfo;
	}

	/** 스킬 어빌리티 타겟 정보를 반환한다 */
	public STTargetInfo GetSkillAbilityTargetInfo(ESkillKinds a_eSkillKinds, EAbilityKinds a_eAbilityKinds) {
		bool bIsValid = this.TryGetSkillAbilityTargetInfo(a_eSkillKinds, a_eAbilityKinds, out STTargetInfo stTargetInfo);
		CAccess.Assert(bIsValid);

		return stTargetInfo;
	}

	/** 객체 정보를 반환한다 */
	public CObjInfo GetObjInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetObjInfo(a_eObjKinds, out CObjInfo oObjInfo);
		CAccess.Assert(bIsValid);

		return oObjInfo;
	}

	/** 객체 어빌리티 타겟 정보를 반환한다 */
	public STTargetInfo GetObjAbilityTargetInfo(EObjKinds a_eObjKinds, EAbilityKinds a_eAbilityKinds) {
		bool bIsValid = this.TryGetObjAbilityTargetInfo(a_eObjKinds, a_eAbilityKinds, out STTargetInfo stTargetInfo);
		CAccess.Assert(bIsValid);

		return stTargetInfo;
	}

	/** 아이템 정보를 반환한다 */
	public bool TryGetItemInfo(EItemKinds a_eItemKinds, out CItemInfo a_oOutItemInfo) {
		a_oOutItemInfo = this.UserInfo.m_oItemInfoList.ExGetVal((a_oItemInfo) => a_oItemInfo.ItemKinds == a_eItemKinds, null);
		return a_oOutItemInfo != null;
	}

	/** 아이템 어빌리티 타겟 정보를 반환한다 */
	public bool TryGetItemAbilityTargetInfo(EItemKinds a_eItemKinds, EAbilityKinds a_eAbilityKinds, out STTargetInfo a_stOutTargetInfo) {
		a_stOutTargetInfo = (this.TryGetItemInfo(a_eItemKinds, out CItemInfo oItemInfo) && oItemInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)a_eAbilityKinds, out STTargetInfo stTargetInfo)) ? stTargetInfo : STTargetInfo.INVALID;
		return !a_stOutTargetInfo.Equals(STTargetInfo.INVALID);
	}

	/** 스킬 정보를 반환한다 */
	public bool TryGetSkillInfo(ESkillKinds a_eSkillKinds, out CSkillInfo a_oOutSkillInfo) {
		a_oOutSkillInfo = this.UserInfo.m_oSkillInfoList.ExGetVal((a_oItemInfo) => a_oItemInfo.SkillKinds == a_eSkillKinds, null);
		return a_oOutSkillInfo != null;
	}

	/** 스킬 어빌리티 타겟 정보를 반환한다 */
	public bool TryGetSkillAbilityTargetInfo(ESkillKinds a_eSkillKinds, EAbilityKinds a_eAbilityKinds, out STTargetInfo a_stOutTargetInfo) {
		a_stOutTargetInfo = (this.TryGetSkillInfo(a_eSkillKinds, out CSkillInfo oSkillInfo) && oSkillInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)a_eAbilityKinds, out STTargetInfo stTargetInfo)) ? stTargetInfo : STTargetInfo.INVALID;
		return !a_stOutTargetInfo.Equals(STTargetInfo.INVALID);
	}

	/** 객체 정보를 반환한다 */
	public bool TryGetObjInfo(EObjKinds a_eObjKinds, out CObjInfo a_oOutObjInfo) {
		a_oOutObjInfo = this.UserInfo.m_oObjInfoList.ExGetVal((a_oItemInfo) => a_oItemInfo.ObjKinds == a_eObjKinds, null);
		return a_oOutObjInfo != null;
	}

	/** 객체 어빌리티 타겟 정보를 반환한다 */
	public bool TryGetObjAbilityTargetInfo(EObjKinds a_eObjKinds, EAbilityKinds a_eAbilityKinds, out STTargetInfo a_stOutTargetInfo) {
		a_stOutTargetInfo = (this.TryGetObjInfo(a_eObjKinds, out CObjInfo oObjInfo) && oObjInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)a_eAbilityKinds, out STTargetInfo stTargetInfo)) ? stTargetInfo : STTargetInfo.INVALID;
		return !a_stOutTargetInfo.Equals(STTargetInfo.INVALID);
	}
	
	/** 아이템 정보를 추가한다 */
	public void AddItemInfo(CItemInfo a_oItemInfo, bool a_bIsDuplicate = false) {
		// 아이템 정보 추가가 가능 할 경우
		if(a_bIsDuplicate || !this.TryGetItemInfo(a_oItemInfo.ItemKinds, out CItemInfo oItemInfo)) {
			this.UserInfo.m_oItemInfoList.Add(a_oItemInfo);
		}
	}

	/** 스킬 정보를 추가한다 */
	public void AddSkillInfo(CSkillInfo a_oSkillInfo, bool a_bIsDuplicate = false) {
		// 스킬 정보 추가가 가능 할 경우
		if(a_bIsDuplicate || !this.TryGetSkillInfo(a_oSkillInfo.SkillKinds, out CSkillInfo oSkillInfo)) {
			this.UserInfo.m_oSkillInfoList.Add(a_oSkillInfo);
		}
	}

	/** 객체 정보를 추가한다 */
	public void AddObjInfo(CObjInfo a_oObjInfo, bool a_bIsDuplicate = false) {
		// 객체 정보 추가가 가능 할 경우
		if(a_bIsDuplicate || !this.TryGetObjInfo(a_oObjInfo.ObjKinds, out CObjInfo oObjInfo)) {
			this.UserInfo.m_oObjInfoList.Add(a_oObjInfo);
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
