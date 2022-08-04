#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using MessagePack;
using Newtonsoft.Json;

/** 타겟 정보 */
[Union(0, typeof(CItemTargetInfo))]
[Union(1, typeof(CSkillTargetInfo))]
[Union(2, typeof(CObjTargetInfo))]
public abstract partial class CTargetInfo : CBaseInfo {
	#region 변수
	[Key(1)] public STIdxInfo m_stIdxInfo = STIdxInfo.INVALID;
	[Key(131)] public Dictionary<ulong, STTargetInfo> m_oAbilityTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
	[Key(161)] public Dictionary<ETargetType, List<CTargetInfo>> m_oTargetInfoDictContainer = new Dictionary<ETargetType, List<CTargetInfo>>();

	[JsonIgnore][IgnoreMember][System.NonSerialized] public CTargetInfo m_oOwnerTargetInfo = null;
	#endregion			// 변수

	#region 상수
	private const string KEY_NAME = "Name";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public string Name { get { return m_oStrDict.GetValueOrDefault(KEY_NAME, string.Empty); } set { m_oStrDict.ExReplaceVal(KEY_NAME, value); } }

	[JsonIgnore][IgnoreMember] public abstract int Kinds { get; }
	[JsonIgnore][IgnoreMember] public abstract ETargetType TargetType { get; }
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
		m_oTargetInfoDictContainer = m_oTargetInfoDictContainer ?? new Dictionary<ETargetType, List<CTargetInfo>>();

		for(var eTargetType = ETargetType.NONE + KCDefine.B_VAL_1_INT; eTargetType < ETargetType.MAX_VAL; ++eTargetType) {
			// 타겟 정보가 존재 할 경우
			if(m_oTargetInfoDictContainer.TryGetValue(eTargetType, out List<CTargetInfo> oTargetInfoList)) {
				this.SetupTargetInfos(oTargetInfoList);
			}
		}
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CTargetInfo(System.Version a_stVer) : base(a_stVer) {
		// Do Something
	}

	/** 어빌리티 타겟 정보를 설정한다 */
	public void SetupAbilityTargetInfos(System.Version a_stVer) {
		foreach(var stKeyVal in m_oAbilityTargetInfoDict) {
			// 버전이 다를 경우
			if(a_stVer.CompareTo(KDefine.G_VER_ABILITY_TARGET_INFO) < KCDefine.B_COMPARE_EQUALS) {
				// Do Something
			}
		}
	}
	
	/** 타겟 정보를 설정한다 */
	private void SetupTargetInfos(List<CTargetInfo> a_oTargetInfoList) {
		for(int i = 0; i < a_oTargetInfoList.Count; ++i) {
			a_oTargetInfoList[i].m_oOwnerTargetInfo = this;
		}
	}
	#endregion			// 함수
}

/** 아이템 타겟 정보 */
[MessagePackObject][System.Serializable]
public partial class CItemTargetInfo : CTargetInfo {
	#region 상수
	private const string KEY_ITEM_KINDS = "ItemKinds";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public EItemKinds ItemKinds { get { return (EItemKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_ITEM_KINDS, $"{(int)EItemKinds.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_ITEM_KINDS, $"{(int)value}"); } }
	[JsonIgnore][IgnoreMember] public override bool IsIgnoreSaveTime => true;

	[JsonIgnore][IgnoreMember] public override int Kinds => (int)this.ItemKinds;
	[JsonIgnore][IgnoreMember] public override ETargetType TargetType => ETargetType.ITEM;
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
		if(this.Ver.CompareTo(KDefine.G_VER_ITEM_TARGET_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CItemTargetInfo() : this(KDefine.G_VER_ITEM_TARGET_INFO) {
		// Do Something
	}

	/** 생성자 */
	public CItemTargetInfo(System.Version a_stVer) : base(a_stVer) {
		// Do Something
	}
	#endregion			// 함수
}

/** 스킬 타겟 정보 */
[MessagePackObject][System.Serializable]
public partial class CSkillTargetInfo : CTargetInfo {
	#region 상수
	private const string KEY_SKILL_KINDS = "SkillKinds";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public ESkillKinds SkillKinds { get { return (ESkillKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_SKILL_KINDS, $"{(int)ESkillKinds.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_SKILL_KINDS, $"{(int)value}"); } }
	[JsonIgnore][IgnoreMember] public override bool IsIgnoreSaveTime => true;

	[JsonIgnore][IgnoreMember] public override int Kinds => (int)this.SkillKinds;
	[JsonIgnore][IgnoreMember] public override ETargetType TargetType => ETargetType.SKILL;
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
		if(this.Ver.CompareTo(KDefine.G_VER_SKILL_TARGET_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CSkillTargetInfo() : this(KDefine.G_VER_SKILL_TARGET_INFO) {
		// Do Something
	}

	/** 생성자 */
	public CSkillTargetInfo(System.Version a_stVer) : base(a_stVer) {
		// Do Something
	}
	#endregion			// 함수
}

/** 객체 타겟 정보 */
[Union(0, typeof(CCharacterUserInfo))]
[MessagePackObject][System.Serializable]
public partial class CObjTargetInfo : CTargetInfo {
	#region 상수
	private const string KEY_OBJ_KINDS = "ObjKinds";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public EObjKinds ObjKinds { get { return (EObjKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_OBJ_KINDS, $"{(int)EObjKinds.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_OBJ_KINDS, $"{(int)value}"); } }
	[JsonIgnore][IgnoreMember] public override bool IsIgnoreSaveTime => true;

	[JsonIgnore][IgnoreMember] public override int Kinds => (int)this.ObjKinds;
	[JsonIgnore][IgnoreMember] public override ETargetType TargetType => ETargetType.OBJ;
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
		if(this.Ver.CompareTo(KDefine.G_VER_OBJ_TARGET_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CObjTargetInfo() : this(KDefine.G_VER_OBJ_TARGET_INFO) {
		// Do Something
	}

	/** 생성자 */
	public CObjTargetInfo(System.Version a_stVer) : base(a_stVer) {
		// Do Something
	}
	#endregion			// 함수
}

/** 캐릭터 유저 정보 */
[MessagePackObject][System.Serializable]
public partial class CCharacterUserInfo : CObjTargetInfo {
	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();

		// 버전이 다를 경우
		if(this.Ver.CompareTo(KDefine.G_VER_CHARACTER_OBJ_TARGET_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CCharacterUserInfo() : this(KDefine.G_VER_CHARACTER_OBJ_TARGET_INFO) {
		// Do Something
	}

	/** 생성자 */
	public CCharacterUserInfo(System.Version a_stVer) : base(a_stVer) {
		// Do Something
	}
	#endregion			// 함수
}

/** 유저 정보 */
[MessagePackObject][System.Serializable]
public partial class CUserInfo : CBaseInfo {
	#region 변수
	[Key(91)] public Dictionary<int, CCharacterUserInfo> m_oCharacterUserInfoDict = new Dictionary<int, CCharacterUserInfo>();
	#endregion			// 변수

	#region 상수
	private const string KEY_LOGIN_TYPE = "LoginType";
	private const string KEY_ABILITY_TARGET_INFO_VER = "AbilityTargetInfoVer";
	#endregion			// 상수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public ELoginType LoginType { get { return (ELoginType)int.Parse(m_oStrDict.GetValueOrDefault(KEY_LOGIN_TYPE, $"{(int)ELoginType.NONE}")); } set { m_oStrDict.ExReplaceVal(KEY_LOGIN_TYPE, $"{(int)value}"); } }
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
		m_oCharacterUserInfoDict = m_oCharacterUserInfoDict ?? new Dictionary<int, CCharacterUserInfo>();

		foreach(var stKeyVal in m_oCharacterUserInfoDict) {
			stKeyVal.Value.SetupAbilityTargetInfos(this.AbilityTargetInfoVer);
		}

		// 버전이 다를 경우
		if(this.Ver.CompareTo(KDefine.G_VER_USER_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CUserInfo() : this(KDefine.G_VER_USER_INFO) {
		// Do Something
	}

	/** 생성자 */
	public CUserInfo(System.Version a_stVer) : base(a_stVer) {
		// Do Something
	}
	#endregion			// 함수
}

/** 유저 정보 저장소 */
public partial class CUserInfoStorage : CSingleton<CUserInfoStorage> {
	#region 프로퍼티
	public CUserInfo UserInfo { get; private set; } = new CUserInfo();
	#endregion            // 프로퍼티

	#region 함수
	/** 유저 정보를 리셋한다 */
	public virtual void ResetUserInfo(string a_oJSONStr) {
		CFunc.ShowLog($"CUserInfoStorage.ResetUserInfo: {a_oJSONStr}");
		this.UserInfo = a_oJSONStr.ExMsgPackBase64StrToObj<CUserInfo>();

		CAccess.Assert(this.UserInfo != null);
	}

	/** 아이템 타겟 정보를 반환한다 */
	public CItemTargetInfo GetItemTargetInfo(int a_nCharacterID, EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetItemTargetInfo(a_nCharacterID, a_eItemKinds, out CItemTargetInfo oItemTargetInfo);
		CAccess.Assert(bIsValid);

		return oItemTargetInfo;
	}

	/** 스킬 타겟 정보를 반환한다 */
	public CSkillTargetInfo GetSkillTargetInfo(int a_nCharacterID, ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetSkillTargetInfo(a_nCharacterID, a_eSkillKinds, out CSkillTargetInfo oSkillTargetInfo);
		CAccess.Assert(bIsValid);

		return oSkillTargetInfo;
	}

	/** 객체 타겟 정보를 반환한다 */
	public CObjTargetInfo GetObjTargetInfo(int a_nCharacterID, EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetObjTargetInfo(a_nCharacterID, a_eObjKinds, out CObjTargetInfo oObjTargetInfo);
		CAccess.Assert(bIsValid);

		return oObjTargetInfo;
	}

	/** 캐릭터 유저 정보를 반환한다 */
	public CCharacterUserInfo GetCharacterUserInfo(int a_nCharacterID) {
		bool bIsValid = this.TryGetCharacterUserInfo(a_nCharacterID, out CCharacterUserInfo oCharacterUserInfo);
		CAccess.Assert(bIsValid);

		return oCharacterUserInfo;
	}

	/** 아이템 타겟 정보를 반환한다 */
	public bool TryGetItemTargetInfo(int a_nCharacterID, EItemKinds a_eItemKinds, out CItemTargetInfo a_oOutItemTargetInfo) {
		a_oOutItemTargetInfo = this.TryGetTargetInfo(a_nCharacterID, ETargetType.ITEM, (int)a_eItemKinds, out CTargetInfo oTargetInfo) ? oTargetInfo as CItemTargetInfo : null;
		return a_oOutItemTargetInfo != null;
	}

	/** 스킬 타겟 정보를 반환한다 */
	public bool TryGetSkillTargetInfo(int a_nCharacterID, ESkillKinds a_eSkillKinds, out CSkillTargetInfo a_oOutSkillTargetInfo) {
		a_oOutSkillTargetInfo = this.TryGetTargetInfo(a_nCharacterID, ETargetType.SKILL, (int)a_eSkillKinds, out CTargetInfo oTargetInfo) ? oTargetInfo as CSkillTargetInfo : null;
		return a_oOutSkillTargetInfo != null;
	}

	/** 객체 타겟 정보를 반환한다 */
	public bool TryGetObjTargetInfo(int a_nCharacterID, EObjKinds a_eObjKinds, out CObjTargetInfo a_oOutObjTargetInfo) {
		a_oOutObjTargetInfo = this.TryGetTargetInfo(a_nCharacterID, ETargetType.OBJ, (int)a_eObjKinds, out CTargetInfo oTargetInfo) ? oTargetInfo as CObjTargetInfo : null;
		return a_oOutObjTargetInfo != null;
	}

	/** 캐릭터 유저 정보를 반환한다 */
	public bool TryGetCharacterUserInfo(int a_nCharacterID, out CCharacterUserInfo a_oOutCharacterUserInfo) {
		this.UserInfo.m_oCharacterUserInfoDict.TryGetValue(a_nCharacterID, out a_oOutCharacterUserInfo);
		return this.UserInfo.m_oCharacterUserInfoDict.ContainsKey(a_nCharacterID);
	}

	/** 타겟 정보를 추가한다 */
	public void AddTargetInfo(int a_nCharacterID, CTargetInfo a_oTargetInfo, bool a_bIsDuplicate = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || this.UserInfo.m_oCharacterUserInfoDict.ContainsKey(a_nCharacterID));

		// 캐릭터 유저 정보가 존재 할 경우
		if(this.TryGetCharacterUserInfo(a_nCharacterID, out CCharacterUserInfo oCharacterUserInfo)) {
			oCharacterUserInfo.m_oTargetInfoDictContainer.ExAddTargetInfo(a_oTargetInfo, oCharacterUserInfo, a_bIsDuplicate);
		}
	}

	/** 캐릭터 유저 정보를 추가한다 */
	public void AddCharacterUserInfo(int a_nCharacterID, CCharacterUserInfo a_oCharacterUserInfo) {
		this.UserInfo.m_oCharacterUserInfoDict.TryAdd(a_nCharacterID, a_oCharacterUserInfo);
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

	/** 타겟 정보를 반환한다 */
	private bool TryGetTargetInfo(int a_nCharacterID, ETargetType a_eTargetType, int a_nKinds, out CTargetInfo a_oOutTargetInfo) {
		a_oOutTargetInfo = (this.TryGetCharacterUserInfo(a_nCharacterID, out CCharacterUserInfo oCharacterUserInfo) && oCharacterUserInfo.m_oTargetInfoDictContainer.ExTryGetTargetInfo(a_eTargetType, a_nKinds, out CTargetInfo oTargetInfo)) ? oTargetInfo : null;
		return a_oOutTargetInfo != null;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
