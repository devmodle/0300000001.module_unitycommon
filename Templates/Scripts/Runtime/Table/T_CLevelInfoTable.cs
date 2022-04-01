using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
using Newtonsoft.Json;

/** 셀 정보 */
[MessagePackObject][System.Serializable]
public partial class CCellInfo : CBaseInfo, System.ICloneable {
	#region 변수
	[JsonIgnore][IgnoreMember][System.NonSerialized] public Vector3Int m_stIdx;
	[Key(161)] public Dictionary<EBlockType, List<EBlockKinds>> m_oBlockKindsDictContainer = new Dictionary<EBlockType, List<EBlockKinds>>();
	#endregion			// 변수

	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public override bool IsIgnoreVer => true;
	[JsonIgnore][IgnoreMember] public override bool IsIgnoreSaveTime => true;
	#endregion			// 프로퍼티
	
	#region ICloneable
	/** 사본 객체를 생성한다 */
	public virtual object Clone() {
		var oCellInfo = new CCellInfo();
		this.SetupCloneInst(oCellInfo);

		oCellInfo.OnAfterDeserialize();
		return oCellInfo;
	}
	#endregion			// ICloneable

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		m_oBlockKindsDictContainer = m_oBlockKindsDictContainer ?? new Dictionary<EBlockType, List<EBlockKinds>>();
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CCellInfo() : base(KDefine.G_VER_CELL_INFO) {
		// Do Something
	}

	/** 사본 객체를 설정한다 */
	protected virtual void SetupCloneInst(CCellInfo a_oCellInfo) {
		a_oCellInfo.m_stIdx = m_stIdx;
		m_oBlockKindsDictContainer.ExCopyTo(a_oCellInfo.m_oBlockKindsDictContainer, this.GetCloneBlockKinds);
	}

	/** 사본 블럭 종류를 반환한다 */
	protected List<EBlockKinds> GetCloneBlockKinds(List<EBlockKinds> a_oBlockKindsList) {
		var oBlockKindsList = new List<EBlockKinds>();
		a_oBlockKindsList.ExCopyTo(oBlockKindsList, (a_eBlockKinds) => a_eBlockKinds);

		return oBlockKindsList;
	}
	#endregion			// 함수
}

/** 레벨 정보 */
[MessagePackObject][System.Serializable]
public partial class CLevelInfo : CBaseInfo, System.ICloneable {
	#region 상수
	private const string KEY_DIFFICULTY = "Difficulty";
	private const string KEY_LEVEL_KINDS = "LevelKinds";
	private const string KEY_REWARD_KINDS = "RewardKinds";
	private const string KEY_TUTORIAL_KINDS = "TutorialKinds";
	private const string KEY_CELL_VER = "CellVer";
	#endregion			// 상수

	#region 변수
	[JsonIgnore][IgnoreMember][System.NonSerialized] public STIDInfo m_stIDInfo;
	[Key(165)] public Dictionary<int, Dictionary<int, CCellInfo>> m_oCellInfoDictContainer = new Dictionary<int, Dictionary<int, CCellInfo>>();
	#endregion			// 변수
	
	#region 프로퍼티
	[JsonIgnore][IgnoreMember] public Vector3Int NumCells { get; private set; } = Vector3Int.zero;
	[JsonIgnore][IgnoreMember] public Dictionary<ETargetKinds, int> NumTargetsDict = new Dictionary<ETargetKinds, int>();
	[JsonIgnore][IgnoreMember] public Dictionary<ETargetKinds, int> NumUnlockTargetsDict = new Dictionary<ETargetKinds, int>();

	[JsonIgnore][IgnoreMember] public EDifficulty Difficulty {
		get { return (EDifficulty)int.Parse(m_oStrDict.GetValueOrDefault(KEY_DIFFICULTY, $"{(int)EDifficulty.NONE}")); }
		set { m_oStrDict.ExReplaceVal(KEY_DIFFICULTY, $"{(int)value}"); }
	}
	
	[JsonIgnore][IgnoreMember] public ELevelKinds LevelKinds {
		get { return (ELevelKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_LEVEL_KINDS, $"{(int)ELevelKinds.NONE}")); }
		set { m_oStrDict.ExReplaceVal(KEY_LEVEL_KINDS, $"{(int)value}"); }
	}

	[JsonIgnore][IgnoreMember] public ERewardKinds RewardKinds {
		get { return (ERewardKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_REWARD_KINDS, $"{(int)ERewardKinds.NONE}")); }
		set { m_oStrDict.ExReplaceVal(KEY_REWARD_KINDS, $"{(int)value}"); }
	}

	[JsonIgnore][IgnoreMember] public ETutorialKinds TutorialKinds {
		get { return (ETutorialKinds)int.Parse(m_oStrDict.GetValueOrDefault(KEY_TUTORIAL_KINDS, $"{(int)ETutorialKinds.NONE}")); }
		set { m_oStrDict.ExReplaceVal(KEY_TUTORIAL_KINDS, $"{(int)value}"); }
	}

	[JsonIgnore][IgnoreMember] public System.Version CellVer {
		get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_CELL_VER, KCDefine.B_DEF_VER)); }
		set { m_oStrDict.ExReplaceVal(KEY_CELL_VER, value.ToString(KCDefine.B_VAL_3_INT)); }
	}

	[JsonIgnore][IgnoreMember] public long UniqueLevelID => CFactory.MakeUniqueLevelID(m_stIDInfo.m_nID, m_stIDInfo.m_nStageID, m_stIDInfo.m_nChapterID);
	#endregion			// 프로퍼티

	#region ICloneable
	/** 사본 객체를 생성한다 */
	public virtual object Clone() {
		var oLevelInfo = new CLevelInfo();
		this.SetupCloneInst(oLevelInfo);

		oLevelInfo.OnAfterDeserialize();
		return oLevelInfo;
	}
	#endregion			// ICloneable

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		m_oCellInfoDictContainer = m_oCellInfoDictContainer ?? new Dictionary<int, Dictionary<int, CCellInfo>>();

		// 셀 개수를 설정한다 {
		var stNumCells = new Vector3Int(KCDefine.B_VAL_0_INT, m_oCellInfoDictContainer.Count, KCDefine.B_VAL_0_INT);

		for(int i = 0; i < m_oCellInfoDictContainer.Count; ++i) {
			stNumCells.x = Mathf.Max(stNumCells.x, m_oCellInfoDictContainer[i].Count);
		}

		this.NumCells = stNumCells;
		// 셀 개수를 설정한다 }

		// 셀을 설정한다 {
		this.NumTargetsDict = this.NumTargetsDict ?? new Dictionary<ETargetKinds, int>();
		this.NumUnlockTargetsDict = this.NumUnlockTargetsDict ?? new Dictionary<ETargetKinds, int>();

		for(int i = 0; i < m_oCellInfoDictContainer.Count; ++i) {
			for(int j = 0; j < m_oCellInfoDictContainer[i].Count; ++j) {
				m_oCellInfoDictContainer[i][j].Ver = this.CellVer;
				m_oCellInfoDictContainer[i][j].m_stIdx = new Vector3Int(j, i, KCDefine.B_IDX_INVALID);

				this.SetupCellInfo(m_oCellInfoDictContainer[i][j]);
			}
		}
		// 셀을 설정한다 }

		// 버전이 다를 경우
		if(this.Ver.CompareTo(KDefine.G_VER_LEVEL_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}

	/** 셀 정보를 설정한다 */
	protected virtual void SetupCellInfo(CCellInfo a_oCellInfo) {
		foreach(var stKeyVal in a_oCellInfo.m_oBlockKindsDictContainer) {
			for(int i = 0; i < stKeyVal.Value.Count; ++i) {
				// Do Something
			}
		}

		// 버전이 다를 경우
		if(this.CellVer.CompareTo(KDefine.G_VER_CELL_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
	#endregion			// IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CLevelInfo() : base(KDefine.G_VER_LEVEL_INFO) {
		// Do Something
	}

	/** 셀 정보를 반환한다 */
	public CCellInfo GetCellInfo(Vector3Int a_stIdx) {
		bool bIsValid = this.TryGetCellInfo(a_stIdx, out CCellInfo oCellInfo);
		CAccess.Assert(bIsValid);

		return oCellInfo;
	}

	/** 셀 정보를 반환한다 */
	public bool TryGetCellInfo(Vector3Int a_stIdx, out CCellInfo a_oOutCellInfo) {
		a_oOutCellInfo = m_oCellInfoDictContainer.ContainsKey(a_stIdx.y) ? m_oCellInfoDictContainer[a_stIdx.y].GetValueOrDefault(a_stIdx.x, null) : null;
		return a_oOutCellInfo != null;
	}

	/** 사본 객체를 설정한다 */
	protected virtual void SetupCloneInst(CLevelInfo a_oLevelInfo) {
		a_oLevelInfo.m_stIDInfo = m_stIDInfo;

		// 셀 정보를 설정한다
		for(int i = 0; i < m_oCellInfoDictContainer.Count; ++i) {
			var oCellInfoDict = new Dictionary<int, CCellInfo>();

			for(int j = 0; j < m_oCellInfoDictContainer[i].Count; ++j) {
				var oCellInfo = m_oCellInfoDictContainer[i][j].Clone() as CCellInfo;
				oCellInfoDict.TryAdd(j, oCellInfo);
			}

			a_oLevelInfo.m_oCellInfoDictContainer.TryAdd(i, oCellInfoDict);
		}
	}
	#endregion			// 함수
}

/** 레벨 정보 테이블 */
public partial class CLevelInfoTable : CSingleton<CLevelInfoTable> {
	#region 프로퍼티
	public Dictionary<int, Dictionary<int, int>> NumLevelInfosDictContainer = new Dictionary<int, Dictionary<int, int>>();
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LevelInfoDictContainer = new Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>>();

	public string LevelInfoTablePath {
		get {
#if AB_TEST_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_A : KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_B;
#else
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_A : KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_B;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#else
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO;
#else
			return KCDefine.U_TABLE_P_G_LEVEL_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if AB_TEST_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}
	
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	public int NumChapterInfos => this.LevelInfoDictContainer.Count;
#else
	public int NumChapterInfos => this.NumLevelInfosDictContainer.Count;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 프로퍼티

	#region 함수
	/** 레벨 정보 개수를 반환한다 */
	public int GetNumLevelInfos(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nChapterID) && this.LevelInfoDictContainer[a_nChapterID].ContainsKey(a_nID));
		return this.LevelInfoDictContainer[a_nChapterID][a_nID].Count;
#else
		CAccess.Assert(this.NumLevelInfosDictContainer.ContainsKey(a_nChapterID) && this.NumLevelInfosDictContainer[a_nChapterID].ContainsKey(a_nID));
		return this.NumLevelInfosDictContainer[a_nChapterID][a_nID];
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 스테이지 정보 개수를 반화한다 */
	public int GetNumStageInfos(int a_nID) {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		bool bIsValid = this.LevelInfoDictContainer.TryGetValue(a_nID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		CAccess.Assert(bIsValid);

		return oChapterLevelInfoDictContainer.Count;
#else
		bool bIsValid = this.NumLevelInfosDictContainer.TryGetValue(a_nID, out Dictionary<int, int> oNumChapterLevelInfosDict);
		CAccess.Assert(bIsValid);
		
		return oNumChapterLevelInfosDict.Count;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}
	
	/** 레벨 정보를 로드한다 */
	public CLevelInfo LoadLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return this.LoadLevelInfo(this.GetLevelInfoPath(a_nID, a_nStageID, a_nChapterID), a_nID, a_nStageID, a_nChapterID);
	}

	/** 레벨 정보를 로드한다 */
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LoadLevelInfos() {
		return this.LoadLevelInfos(this.LevelInfoTablePath);
	}

	/** 레벨 정보 경로를 반환한다 */
	private string GetLevelInfoPath(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		long nUniqueLevelID = CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID);

#if MSG_PACK_ENABLE || NEWTON_SOFT_JSON_MODULE_ENABLE
#if AB_TEST_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return string.Format((CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? KCDefine.U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO_SET_A : KCDefine.U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO_SET_B, nUniqueLevelID + KCDefine.B_VAL_1_INT);
#else
		return string.Format((CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_A : KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_B, nUniqueLevelID + KCDefine.B_VAL_1_INT);
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#else
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return string.Format(KCDefine.U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO, nUniqueLevelID + KCDefine.B_VAL_1_INT);
#else
		return string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO, nUniqueLevelID + KCDefine.B_VAL_1_INT);
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if AB_TEST_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
#else
		return null;
#endif			// #if MSG_PACK_ENABLE || NEWTON_SOFT_JSON_MODULE_ENABLE
	}

	/** 레벨 정보를 로드한다 */
	private CLevelInfo LoadLevelInfo(string a_oFilePath, int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CFunc.ShowLog($"CLevelInfoTable.LoadLevelInfo: {a_oFilePath}");
		CLevelInfo oLevelInfo = null;

#if MSG_PACK_ENABLE
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		oLevelInfo = CFunc.ReadMsgPackObj<CLevelInfo>(a_oFilePath, null, false);
#else
		oLevelInfo = CFunc.ReadMsgPackObjFromRes<CLevelInfo>(a_oFilePath, null, false);
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#elif NEWTON_SOFT_JSON_MODULE_ENABLE
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		oLevelInfo = CFunc.ReadJSONObj<CLevelInfo>(a_oFilePath, null, false);
#else
		oLevelInfo = CFunc.ReadJSONObjFromRes<CLevelInfo>(a_oFilePath, null, false);
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if MSG_PACK_ENABLE

		oLevelInfo.m_stIDInfo = CFactory.MakeIDInfo(a_nID, a_nStageID, a_nChapterID);
		return oLevelInfo;
	}

	/** 레벨 정보를 로드한다 */
	private Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LoadLevelInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		List<long> oLevelIDList = null;

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		CFunc.ShowLog($"CLevelInfoTable.LoadLevelInfos: {a_oFilePath.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON)}");
		oLevelIDList = CFunc.ReadMsgPackJSONObj<List<long>>(a_oFilePath.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON), null, false);
#else
		CFunc.ShowLog($"CLevelInfoTable.LoadLevelInfos: {a_oFilePath}");

		try {
			oLevelIDList = CFunc.ReadMsgPackJSONObjFromRes<List<long>>(a_oFilePath, null, false);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)

		CAccess.Assert(oLevelIDList != null);
		this.NumLevelInfosDictContainer.Clear();

		for(int i = 0; i < oLevelIDList.Count; ++i) {
			int nID = oLevelIDList[i].ExUniqueLevelIDToID();
			int nStageID = oLevelIDList[i].ExUniqueLevelIDToStageID();
			int nChapterID = oLevelIDList[i].ExUniqueLevelIDToChapterID();

			var oNumChapterLevelInfosDict = this.NumLevelInfosDictContainer.GetValueOrDefault(nChapterID, null);
			oNumChapterLevelInfosDict = oNumChapterLevelInfosDict ?? new Dictionary<int, int>();

			int nNumLevelInfos = oNumChapterLevelInfosDict.GetValueOrDefault(nStageID, KCDefine.B_VAL_0_INT);
			oNumChapterLevelInfosDict.ExReplaceVal(nStageID, nNumLevelInfos + KCDefine.B_VAL_1_INT);

			this.NumLevelInfosDictContainer.ExReplaceVal(nChapterID, oNumChapterLevelInfosDict);

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			var oLevelInfo = this.LoadLevelInfo(nID, nStageID, nChapterID);
			oLevelInfo.m_stIDInfo = CFactory.MakeIDInfo(nID, nStageID, nChapterID);

			this.AddLevelInfo(oLevelInfo);
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}

		return this.LevelInfoDictContainer;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	/** 레벨 정보를 반환한다 */
	public CLevelInfo GetLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetLevelInfo(a_nID, out CLevelInfo oLevelInfo, a_nStageID, a_nChapterID);
		CAccess.Assert(bIsValid);

		return oLevelInfo;
	}
	
	/** 스테이지 레벨 정보를 반환한다 */
	public Dictionary<int, CLevelInfo> GetStageLevelInfos(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);
		CAccess.Assert(bIsValid);

		return oStageLevelInfoDict;
	}

	/** 챕터 레벨 정보를 반환한다 */
	public Dictionary<int, Dictionary<int, CLevelInfo>> GetChapterLevelInfos(int a_nID) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		CAccess.Assert(bIsValid);

		return oChapterLevelInfoDictContainer;
	}

	/** 레벨 정보를 반환한다 */
	public bool TryGetLevelInfo(int a_nID, out CLevelInfo a_oOutLevelInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);
		a_oOutLevelInfo = oStageLevelInfoDict?.GetValueOrDefault(a_nID, null);

		return a_oOutLevelInfo != null;
	}

	/** 스테이지 레벨 정보를 반환한다 */
	public bool TryGetStageLevelInfos(int a_nID, out Dictionary<int, CLevelInfo> a_oOutStageLevelInfoDict, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		a_oOutStageLevelInfoDict = oChapterLevelInfoDictContainer?.GetValueOrDefault(a_nID, null);

		return a_oOutStageLevelInfoDict != null;
	}

	/** 챕터 레벨 정보를 반환한다 */
	public bool TryGetChapterLevelInfos(int a_nID, out Dictionary<int, Dictionary<int, CLevelInfo>> a_oOutChapterLevelInfoDictContainer) {
		a_oOutChapterLevelInfoDictContainer = this.LevelInfoDictContainer.GetValueOrDefault(a_nID, null);
		return a_oOutChapterLevelInfoDictContainer != null;
	}

	/** 레벨 정보를 추가한다 */
	public void AddLevelInfo(CLevelInfo a_oLevelInfo, bool a_bIsReplace = false) {
		CAccess.Assert(a_oLevelInfo != null);

		var oChapterLevelInfoDictContainer = this.LevelInfoDictContainer.GetValueOrDefault(a_oLevelInfo.m_stIDInfo.m_nChapterID, null);
		oChapterLevelInfoDictContainer = oChapterLevelInfoDictContainer ?? new Dictionary<int, Dictionary<int, CLevelInfo>>();

		var oStageLevelInfoDict = oChapterLevelInfoDictContainer.GetValueOrDefault(a_oLevelInfo.m_stIDInfo.m_nStageID, null);
		oStageLevelInfoDict = oStageLevelInfoDict ?? new Dictionary<int, CLevelInfo>();

		// 레벨 정보 추가가 가능 할 경우
		if(a_bIsReplace || !oStageLevelInfoDict.ContainsKey(a_oLevelInfo.m_stIDInfo.m_nID)) {
			oStageLevelInfoDict.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nID, a_oLevelInfo);
			oChapterLevelInfoDictContainer.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nStageID, oStageLevelInfoDict);
			this.LevelInfoDictContainer.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nChapterID, oChapterLevelInfoDictContainer);
		}
	}

	/** 스테이지 레벨 정보를 추가한다 */
	public void AddStageLevelInfos(Dictionary<int, CLevelInfo> a_oStageLevelInfoDict, bool a_bIsReplace = false) {
		CAccess.Assert(a_oStageLevelInfoDict != null);
		var oStageLevelInfoList = a_oStageLevelInfoDict.OrderBy((a_stKeyVal) => a_stKeyVal.Key).ToList();

		for(int i = 0; i < oStageLevelInfoList.Count; ++i) {
			int nNumLevelInfos = this.GetNumLevelInfos(oStageLevelInfoList[i].Value.m_stIDInfo.m_nStageID, oStageLevelInfoList[i].Value.m_stIDInfo.m_nChapterID);
			oStageLevelInfoList[i].Value.m_stIDInfo = CFactory.MakeIDInfo(nNumLevelInfos, oStageLevelInfoList[i].Value.m_stIDInfo.m_nStageID, oStageLevelInfoList[i].Value.m_stIDInfo.m_nChapterID);

			this.AddLevelInfo(oStageLevelInfoList[i].Value, a_bIsReplace);
		}
	}

	/** 챕터 레벨 정보를 추가한다 */
	public void AddChapterLevelInfos(Dictionary<int, Dictionary<int, CLevelInfo>> a_oChapterLevelInfoDict, bool a_bIsReplace = false) {
		CAccess.Assert(a_oChapterLevelInfoDict != null);
		var oChapterLevelInfoList = a_oChapterLevelInfoDict.OrderBy((a_stKeyVal) => a_stKeyVal.Key).ToList();

		for(int i = 0; i < oChapterLevelInfoList.Count; ++i) {
			for(int j = 0; j < oChapterLevelInfoList[i].Value.Count; ++i) {
				int nNumStageInfos = this.GetNumStageInfos(oChapterLevelInfoList[i].Value[j].m_stIDInfo.m_nChapterID);
				oChapterLevelInfoList[i].Value[j].m_stIDInfo = CFactory.MakeIDInfo(oChapterLevelInfoList[i].Value[j].m_stIDInfo.m_nID, nNumStageInfos, oChapterLevelInfoList[i].Value[j].m_stIDInfo.m_nChapterID);
			}

			this.AddStageLevelInfos(oChapterLevelInfoList[i].Value, a_bIsReplace);
		}
	}

	/** 레벨 정보를 제거한다 */
	public void RemoveLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);
		CAccess.Assert(bIsValid && oStageLevelInfoDict.ExIsValid());

		for(int i = a_nID + KCDefine.B_VAL_1_INT; i < oStageLevelInfoDict.Count; ++i) {
			oStageLevelInfoDict[i].m_stIDInfo.m_nID -= KCDefine.B_VAL_1_INT;
			oStageLevelInfoDict.ExReplaceVal(i - KCDefine.B_VAL_1_INT, oStageLevelInfoDict[i]);
		}

		oStageLevelInfoDict.ExRemoveVal(oStageLevelInfoDict.Count - KCDefine.B_VAL_1_INT);

		// 스테이지 레벨 정보가 없을 경우
		if(!oStageLevelInfoDict.ExIsValid()) {
			this.RemoveStageLevelInfos(a_nStageID, a_nChapterID);
		}
	}

	/** 스테이지 레벨 정보를 제거한다 */
	public void RemoveStageLevelInfos(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		CAccess.Assert(bIsValid && oChapterLevelInfoDictContainer.ExIsValid());

		for(int i = a_nID + KCDefine.B_VAL_1_INT; i < oChapterLevelInfoDictContainer.Count; ++i) {
			for(int j = 0; j < oChapterLevelInfoDictContainer[i].Count; ++j) {
				oChapterLevelInfoDictContainer[i][j].m_stIDInfo.m_nStageID -= KCDefine.B_VAL_1_INT;
			}

			oChapterLevelInfoDictContainer.ExReplaceVal(i - KCDefine.B_VAL_1_INT, oChapterLevelInfoDictContainer[i]);
		}

		oChapterLevelInfoDictContainer.ExRemoveVal(oChapterLevelInfoDictContainer.Count - KCDefine.B_VAL_1_INT);

		// 챕터 레벨 정보가 없을 경우
		if(!oChapterLevelInfoDictContainer.ExIsValid()) {
			this.RemoveChapterLevelInfos(a_nChapterID);
		}
	}

	/** 챕터 레벨 정보를 제거한다 */
	public void RemoveChapterLevelInfos(int a_nID) {
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nID));

		for(int i = a_nID + KCDefine.B_VAL_1_INT; i < this.LevelInfoDictContainer.Count; ++i) {
			for(int j = 0; j < this.LevelInfoDictContainer[i].Count; ++j) {
				for(int k = 0; k < this.LevelInfoDictContainer[i][j].Count; ++k) {
					this.LevelInfoDictContainer[i][j][k].m_stIDInfo.m_nChapterID -= KCDefine.B_VAL_1_INT;
				}
			}

			this.LevelInfoDictContainer.ExReplaceVal(i - KCDefine.B_VAL_1_INT, this.LevelInfoDictContainer[i]);
		}

		this.LevelInfoDictContainer.ExRemoveVal(this.LevelInfoDictContainer.Count - KCDefine.B_VAL_1_INT);
	}

	/** 레벨 정보를 이동한다 */
	public void MoveLevelInfo(int a_nSrcID, int a_nDestID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);
		CAccess.Assert(bIsValid && oStageLevelInfoDict.ExIsValid() && oStageLevelInfoDict.ContainsKey(a_nSrcID) && oStageLevelInfoDict.ContainsKey(a_nDestID));

		int nOffset = (a_nSrcID <= a_nDestID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;
		var oSrcLevelInfo = oStageLevelInfoDict[a_nSrcID];

		oStageLevelInfoDict.ExRemoveVal(a_nSrcID);

		for(int i = a_nSrcID + nOffset; i != a_nDestID + nOffset; i += nOffset) {
			oStageLevelInfoDict[i].m_stIDInfo.m_nID -= nOffset;
			oStageLevelInfoDict.ExReplaceVal(i - nOffset, oStageLevelInfoDict[i]);
		}

		oSrcLevelInfo.m_stIDInfo.m_nID = a_nDestID;
		oStageLevelInfoDict.ExReplaceVal(a_nDestID, oSrcLevelInfo);
	}

	/** 스테이지 레벨 정보를 이동한다 */
	public void MoveStageLevelInfos(int a_nSrcID, int a_nDestID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		CAccess.Assert(bIsValid && oChapterLevelInfoDictContainer.ExIsValid() && oChapterLevelInfoDictContainer.ContainsKey(a_nSrcID) && oChapterLevelInfoDictContainer.ContainsKey(a_nDestID));

		int nOffset = (a_nSrcID <= a_nDestID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;
		var oSrcStageLevelInfoDict = oChapterLevelInfoDictContainer[a_nSrcID];

		oChapterLevelInfoDictContainer.ExRemoveVal(a_nSrcID);

		for(int i = a_nSrcID + nOffset; i != a_nDestID + nOffset; i += nOffset) {
			for(int j = 0; j < oChapterLevelInfoDictContainer[i].Count; ++j) {
				oChapterLevelInfoDictContainer[i][j].m_stIDInfo.m_nStageID -= nOffset;
			}

			oChapterLevelInfoDictContainer.ExReplaceVal(i - nOffset, oChapterLevelInfoDictContainer[i]);
		}

		for(int i = 0; i < oSrcStageLevelInfoDict.Count; ++i) {
			oSrcStageLevelInfoDict[i].m_stIDInfo.m_nStageID = a_nDestID;
		}

		oChapterLevelInfoDictContainer.ExReplaceVal(a_nDestID, oSrcStageLevelInfoDict);
	}

	/** 챕터 레벨 정보를 이동한다 */
	public void MoveChapterLevelInfos(int a_nSrcID, int a_nDestID) {
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nSrcID) && this.LevelInfoDictContainer.ContainsKey(a_nDestID));

		int nOffset = (a_nSrcID <= a_nDestID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;
		var oSrcChapterLevelInfoDict = this.LevelInfoDictContainer[a_nSrcID];

		for(int i = a_nSrcID + nOffset; i != a_nDestID + nOffset; i += nOffset) {
			for(int j = 0; j < this.LevelInfoDictContainer[i].Count; ++j) {
				for(int k = 0; k < this.LevelInfoDictContainer[i][j].Count; ++k) {
					this.LevelInfoDictContainer[i][j][k].m_stIDInfo.m_nChapterID -= nOffset;
				}
			}

			this.LevelInfoDictContainer.ExReplaceVal(i - nOffset, this.LevelInfoDictContainer[i]);
		}

		for(int i = 0; i < oSrcChapterLevelInfoDict.Count; ++i) {
			for(int j = 0; j < oSrcChapterLevelInfoDict[i].Count; ++j) {
				oSrcChapterLevelInfoDict[i][j].m_stIDInfo.m_nChapterID = a_nDestID;
			}
		}

		this.LevelInfoDictContainer.ExReplaceVal(a_nDestID, oSrcChapterLevelInfoDict);
	}

	/** 레벨 정보를 저장한다 */
	public void SaveLevelInfos() {
		var oLevelIDList = new List<long>();
		string oFilePath = this.LevelInfoTablePath.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON);

		for(int i = 0; i < this.LevelInfoDictContainer.Count; ++i) {
			for(int j = 0; j < this.LevelInfoDictContainer[i].Count; ++j) {
				for(int k = 0; k < this.LevelInfoDictContainer[i][j].Count; ++k) {
					this.LevelInfoDictContainer[i][j][k].m_stIDInfo = CFactory.MakeIDInfo(k, j, i);
					this.SaveLevelInfo(this.LevelInfoDictContainer[i][j][k], oLevelIDList);
				}
			}
		}
		
		CEpisodeInfoTable.Inst.SaveEpisodeInfos();
		CFunc.WriteMsgPackJSONObj(oFilePath, oLevelIDList, null, false, false);
	}

	/** 레벨 정보를 저장한다 */
	private void SaveLevelInfo(CLevelInfo a_oLevelInfo, List<long> a_oOutLevelIDList) {
		CAccess.Assert(a_oLevelInfo != null && a_oOutLevelIDList != null);

		a_oOutLevelIDList.Add(a_oLevelInfo.UniqueLevelID);
		CEpisodeInfoTable.Inst.TryGetLevelInfo(a_oLevelInfo.m_stIDInfo.m_nID, out STLevelInfo stLevelInfo, a_oLevelInfo.m_stIDInfo.m_nStageID, a_oLevelInfo.m_stIDInfo.m_nChapterID);

		var stReplaceLevelInfo = new STLevelInfo() {
			m_nID = a_oLevelInfo.m_stIDInfo.m_nID,
			m_nStageID = a_oLevelInfo.m_stIDInfo.m_nStageID,
			m_nChapterID = a_oLevelInfo.m_stIDInfo.m_nChapterID,
			m_eLevelKinds = stLevelInfo.m_eLevelKinds,

			m_stEpisodeInfo = new STEpisodeInfo() {
				m_oName = stLevelInfo.m_stEpisodeInfo.m_oName ?? string.Empty,
				m_oDesc = stLevelInfo.m_stEpisodeInfo.m_oDesc ?? string.Empty,

				m_eDifficulty = stLevelInfo.m_stEpisodeInfo.m_eDifficulty,
				m_eRewardKinds = stLevelInfo.m_stEpisodeInfo.m_eRewardKinds,
				m_eTutorialKinds = stLevelInfo.m_stEpisodeInfo.m_eTutorialKinds,

				m_oRecordList = new List<int>(),
				m_oNumTargetsDict = new Dictionary<ETargetKinds, int>(),
				m_oNumUnlockTargetsDict = new Dictionary<ETargetKinds, int>()
			}
		};

		stLevelInfo.m_stEpisodeInfo.m_oNumTargetsDict?.ExCopyTo(stReplaceLevelInfo.m_stEpisodeInfo.m_oNumTargetsDict, (a_nNumTargets) => a_nNumTargets);
		stLevelInfo.m_stEpisodeInfo.m_oNumUnlockTargetsDict?.ExCopyTo(stReplaceLevelInfo.m_stEpisodeInfo.m_oNumUnlockTargetsDict, (a_nNumUnlockTargets) => a_nNumUnlockTargets);

		CEpisodeInfoTable.Inst.LevelInfoDict.ExReplaceVal(a_oLevelInfo.UniqueLevelID, stReplaceLevelInfo);

#if MSG_PACK_ENABLE
		CFunc.WriteMsgPackObj(this.GetLevelInfoPath(a_oLevelInfo.m_stIDInfo.m_nID, a_oLevelInfo.m_stIDInfo.m_nStageID, a_oLevelInfo.m_stIDInfo.m_nChapterID), a_oLevelInfo, null, false, false);
#elif NEWTON_SOFT_JSON_MODULE_ENABLE
		CFunc.WriteJSONObj(this.GetLevelInfoPath(a_oLevelInfo.m_stIDInfo.m_nID, a_oLevelInfo.m_stIDInfo.m_nStageID, a_oLevelInfo.m_stIDInfo.m_nChapterID), a_oLevelInfo, null, false, false, false, false);
#endif			// #if MSG_PACK_ENABLE
	}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 조건부 함수
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
