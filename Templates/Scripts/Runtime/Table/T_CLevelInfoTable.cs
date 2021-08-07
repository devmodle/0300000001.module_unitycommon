using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if NEVER_USE_THIS
//! 셀 정보
[MessagePackObject]
[System.Serializable]
public class CCellInfo : CBaseInfo, System.ICloneable {
	#region 변수
	[Key(3)] public STIdxInfo m_stIdxInfo;
	[Key(61)] public List<SampleEngineName.EBlockKinds> m_oBlockKindsList = new List<SampleEngineName.EBlockKinds>();
	#endregion			// 변수

	#region 인터페이스
	//! 사본 객체를 생성한다
	public virtual object Clone() {
		var oCellInfo = new CCellInfo();
		this.SetupCloneInst(oCellInfo);

		return oCellInfo;
	}

	//! 직렬화 될 경우
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
	}
	#endregion			// 인터페이스

	#region 함수
	//! 사본 객체를 설정한다
	protected virtual void SetupCloneInst(CCellInfo a_oCellInfo) {
		a_oCellInfo.m_stIdxInfo = m_stIdxInfo;
		m_oBlockKindsList.ExCopyTo(a_oCellInfo.m_oBlockKindsList, (a_eBlockKinds) => a_eBlockKinds);
	}
	#endregion			// 함수
}

//! 레벨 정보
[MessagePackObject]
[System.Serializable]
public class CLevelInfo : CBaseInfo, System.ICloneable {
	#region 상수
	private const string KEY_LEVEL_MODE = "LevelMode";
	private const string KEY_LEVEL_KINDS = "LevelKinds";
	private const string KEY_REWARD_KINDS = "RewardKinds";
	private const string KEY_TUTORIAL_KINDS = "TutorialKinds";
	#endregion			// 상수

	#region 변수
	[Key(3)] public STIDInfo m_stIDInfo;
	[Key(161)] public Dictionary<int, Dictionary<int, CCellInfo>> m_oCellInfoDictContainer = new Dictionary<int, Dictionary<int, CCellInfo>>();
	#endregion			// 변수
	
	#region 프로퍼티
	[IgnoreMember] public Vector3Int NumCells { get; private set; } = Vector3Int.zero;

	[IgnoreMember] public ELevelMode LevelMode {
		get { return (ELevelMode)m_oIntDict.ExGetVal(CLevelInfo.KEY_LEVEL_MODE, (int)ELevelMode.NONE); }
		set { m_oIntDict.ExReplaceVal(CLevelInfo.KEY_LEVEL_MODE, (int)value); }
	}
	
	[IgnoreMember] public ELevelKinds LevelKinds {
		get { return (ELevelKinds)m_oIntDict.ExGetVal(CLevelInfo.KEY_LEVEL_KINDS, (int)ELevelKinds.NONE); }
		set { m_oIntDict.ExReplaceVal(CLevelInfo.KEY_LEVEL_KINDS, (int)value); }
	}

	[IgnoreMember] public ERewardKinds RewardKinds {
		get { return (ERewardKinds)m_oIntDict.ExGetVal(CLevelInfo.KEY_REWARD_KINDS, (int)ERewardKinds.NONE); }
		set { m_oIntDict.ExReplaceVal(CLevelInfo.KEY_REWARD_KINDS, (int)value); }
	}

	[IgnoreMember] public ETutorialKinds TutorialKinds {
		get { return (ETutorialKinds)m_oIntDict.ExGetVal(CLevelInfo.KEY_TUTORIAL_KINDS, (int)ETutorialKinds.NONE); }
		set { m_oIntDict.ExReplaceVal(CLevelInfo.KEY_TUTORIAL_KINDS, (int)value); }
	}
	
	[IgnoreMember] public long LevelID => CFactory.MakeUniqueLevelID(m_stIDInfo.m_nID, m_stIDInfo.m_nStageID, m_stIDInfo.m_nChapterID);
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 사본 객체를 생성한다
	public virtual object Clone() {
		var oLevelInfo = new CLevelInfo();
		this.SetupCloneInst(oLevelInfo);

		oLevelInfo.OnAfterDeserialize();
		return oLevelInfo;
	}

	//! 직렬화 될 경우
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();

		// 셀 개수를 설정한다 {
		var stNumCells = new Vector3Int(KCDefine.B_VAL_0_INT, m_oCellInfoDictContainer.Count, KCDefine.B_VAL_0_INT);

		for(int i = 0; i < m_oCellInfoDictContainer.Count; ++i) {
			stNumCells.x = Mathf.Max(stNumCells.x, m_oCellInfoDictContainer[i].Count);
		}

		this.NumCells = stNumCells;
		// 셀 개수를 설정한다 }
	}
	#endregion			// 인터페이스

	#region 함수
	//! 셀 정보를 반환한다
	public CCellInfo GetCellInfo(Vector3Int a_stIdx) {
		bool bIsValid = this.TryGetCellInfo(a_stIdx, out CCellInfo oCellInfo);
		CAccess.Assert(bIsValid);

		return oCellInfo;
	}

	//! 셀 정보를 반환한다
	public bool TryGetCellInfo(Vector3Int a_stIdx, out CCellInfo a_oOutCellInfo) {
		a_oOutCellInfo = m_oCellInfoDictContainer.ContainsKey(a_stIdx.y) ? m_oCellInfoDictContainer[a_stIdx.y].ExGetVal(a_stIdx.x, null) : null;
		return a_oOutCellInfo != null;
	}

	//! 사본 객체를 설정한다
	protected virtual void SetupCloneInst(CLevelInfo a_oLevelInfo) {
		a_oLevelInfo.m_stIDInfo = m_stIDInfo;

		// 셀 정보를 설정한다
		for(int i = 0; i < m_oCellInfoDictContainer.Count; ++i) {
			var oCellInfoDict = new Dictionary<int, CCellInfo>();

			for(int j = 0; j < m_oCellInfoDictContainer[i].Count; ++j) {
				var oCellInfo = m_oCellInfoDictContainer[i][j].Clone() as CCellInfo;
				oCellInfoDict.Add(j, oCellInfo);
			}

			a_oLevelInfo.m_oCellInfoDictContainer.Add(i, oCellInfoDict);
		}
	}
	#endregion			// 함수
}

//! 레벨 정보 테이블
public class CLevelInfoTable : CSingleton<CLevelInfoTable> {
	#region 프로퍼티
	public Dictionary<int, Dictionary<int, int>> NumLevelInfosDictContainer = new Dictionary<int, Dictionary<int, int>>();
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LevelInfoDictContainer = new Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>>();
	
#if UNITY_STANDALONE
	public int NumChapterInfos => this.LevelInfoDictContainer.Count;
#else
	public int NumChapterInfos => this.NumLevelInfosDictContainer.Count;
#endif			// #if UNITY_STANDALONE
	#endregion			// 프로퍼티

	#region 함수
	//! 레벨 정보 개수를 반환한다
	public int GetNumLevelInfos(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
#if UNITY_STANDALONE
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nChapterID) && this.LevelInfoDictContainer[a_nChapterID].ContainsKey(a_nID));
		return this.LevelInfoDictContainer[a_nChapterID][a_nID].Count;
#else
		CAccess.Assert(this.NumLevelInfosDictContainer.ContainsKey(a_nChapterID) && this.NumLevelInfosDictContainer[a_nChapterID].ContainsKey(a_nID));
		return this.NumLevelInfosDictContainer[a_nChapterID][a_nID];
#endif			// #if UNITY_STANDALONE
	}

	//! 스테이지 정보 개수를 반화한다
	public int GetNumStageInfos(int a_nChapterID) {
#if UNITY_STANDALONE
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nChapterID));
		return this.LevelInfoDictContainer[a_nChapterID].Count;
#else
		CAccess.Assert(this.NumLevelInfosDictContainer.ContainsKey(a_nChapterID));
		return this.NumLevelInfosDictContainer[a_nChapterID].Count;
#endif			// #if UNITY_STANDALONE
	}

	//! 레벨 정보를 로드한다
	public CLevelInfo LoadLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		long nLevelID = CFactory.MakeUniqueLevelID(a_nID, a_nStageID, a_nChapterID);

#if UNITY_STANDALONE
		string oFilePath = string.Format(KDefine.G_RUNTIME_DATA_P_FMT_LEVEL_INFO, nLevelID + KCDefine.B_VAL_1_INT);
#else
		string oFilePath = string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO, nLevelID + KCDefine.B_VAL_1_INT);
#endif			// #if UNITY_STANDALONE

		return CFunc.ReadMsgPackObj<CLevelInfo>(oFilePath, false);
	}

	//! 레벨 정보를 로드한다
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LoadLevelInfos() {
#if UNITY_STANDALONE
		return this.LoadLevelInfos(KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO);
#else
		return this.LoadLevelInfos(KCDefine.U_TABLE_P_G_LEVEL_INFO);
#endif			// #if UNITY_STANDALONE
	}

	//! 레벨 정보를 로드한다
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LoadLevelInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		List<long> oLevelIDList = null;

#if UNITY_STANDALONE
		string oFilePath = a_oFilePath.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON);
		oLevelIDList = CFunc.ReadMsgPackJSONObj<List<long>>(oFilePath, false);
#else
		try {
			oLevelIDList = CFunc.ReadMsgPackJSONObjFromRes<List<long>>(a_oFilePath, false);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE

		CAccess.Assert(oLevelIDList != null);
		this.NumLevelInfosDictContainer.Clear();

		for(int i = 0; i < oLevelIDList.Count; ++i) {
			int nID = oLevelIDList[i].ExUniqueLevelIDToID();
			int nStageID = oLevelIDList[i].ExUniqueLevelIDToStageID();
			int nChapterID = oLevelIDList[i].ExUniqueLevelIDToChapterID();

			var oNumChapterLevelInfosDict = this.NumLevelInfosDictContainer.ExGetVal(nChapterID, null);
			oNumChapterLevelInfosDict = oNumChapterLevelInfosDict ?? new Dictionary<int, int>();

			int nNumLevelInfos = oNumChapterLevelInfosDict.ExGetVal(nStageID, KCDefine.B_VAL_0_INT);
			oNumChapterLevelInfosDict.ExReplaceVal(nStageID, nNumLevelInfos + KCDefine.B_VAL_1_INT);

			this.NumLevelInfosDictContainer.ExReplaceVal(nChapterID, oNumChapterLevelInfosDict);

#if UNITY_STANDALONE
			var oLevelInfo = this.LoadLevelInfo(nID, nStageID, nChapterID);
			oLevelInfo.m_stIDInfo = CFactory.MakeIDInfo(nID, nStageID, nChapterID);

			this.AddLevelInfo(oLevelInfo);
#endif			// #if UNITY_STANDALONE
		}

		return this.LevelInfoDictContainer;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_STANDALONE
	//! 레벨 정보를 반환한다
	public CLevelInfo GetLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetLevelInfo(a_nID, out CLevelInfo oLevelInfo, a_nStageID, a_nChapterID);
		CAccess.Assert(bIsValid);

		return oLevelInfo;
	}
	
	//! 스테이지 레벨 정보를 반환한다
	public Dictionary<int, CLevelInfo> GetStageLevelInfos(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);
		CAccess.Assert(bIsValid);

		return oStageLevelInfoDict;
	}

	//! 챕터 레벨 정보를 반환한다
	public Dictionary<int, Dictionary<int, CLevelInfo>> GetChapterLevelInfos(int a_nID) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		CAccess.Assert(bIsValid);

		return oChapterLevelInfoDictContainer;
	}

	//! 레벨 정보를 반환한다
	public bool TryGetLevelInfo(int a_nID, out CLevelInfo a_oOutLevelInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);
		a_oOutLevelInfo = oStageLevelInfoDict?.ExGetVal(a_nID, null);

		return a_oOutLevelInfo != null;
	}

	//! 스테이지 레벨 정보를 반환한다
	public bool TryGetStageLevelInfos(int a_nID, out Dictionary<int, CLevelInfo> a_oOutStageLevelInfoDict, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		a_oOutStageLevelInfoDict = oChapterLevelInfoDictContainer?.ExGetVal(a_nID, null);

		return a_oOutStageLevelInfoDict != null;
	}

	//! 챕터 레벨 정보를 반환한다
	public bool TryGetChapterLevelInfos(int a_nID, out Dictionary<int, Dictionary<int, CLevelInfo>> a_oOutChapterLevelInfoDictContainer) {
		a_oOutChapterLevelInfoDictContainer = this.LevelInfoDictContainer.ExGetVal(a_nID, null);
		return a_oOutChapterLevelInfoDictContainer != null;
	}

	//! 레벨 정보를 추가한다
	public void AddLevelInfo(CLevelInfo a_oLevelInfo) {
		CAccess.Assert(a_oLevelInfo != null);

		var oChapterLevelInfoDictContainer = this.LevelInfoDictContainer.ExGetVal(a_oLevelInfo.m_stIDInfo.m_nChapterID, null);
		oChapterLevelInfoDictContainer = oChapterLevelInfoDictContainer ?? new Dictionary<int, Dictionary<int, CLevelInfo>>();

		var oStageLevelInfoDict = oChapterLevelInfoDictContainer.ExGetVal(a_oLevelInfo.m_stIDInfo.m_nStageID, null);
		oStageLevelInfoDict = oStageLevelInfoDict ?? new Dictionary<int, CLevelInfo>();

		oStageLevelInfoDict.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nID, a_oLevelInfo);
		oChapterLevelInfoDictContainer.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nStageID, oStageLevelInfoDict);
		this.LevelInfoDictContainer.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nChapterID, oChapterLevelInfoDictContainer);
	}

	//! 레벨 정보를 제거한다
	public void RemoveLevelInfo(int a_nID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);
		CAccess.Assert(bIsValid && oStageLevelInfoDict.ExIsValid());

		for(int i = a_nID + KCDefine.B_VAL_1_INT; i < oStageLevelInfoDict.Count; ++i) {
			oStageLevelInfoDict[i].m_stIDInfo.m_nID -= KCDefine.B_VAL_1_INT;
			oStageLevelInfoDict.ExReplaceVal(i - KCDefine.B_VAL_1_INT, oStageLevelInfoDict[i]);
		}

		oStageLevelInfoDict.Remove(oStageLevelInfoDict.Count - KCDefine.B_VAL_1_INT);
	}

	//! 스테이지 레벨 정보를 제거한다
	public void RemoveStageLevelInfos(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		CAccess.Assert(bIsValid && oChapterLevelInfoDictContainer.ExIsValid());

		for(int i = a_nID + KCDefine.B_VAL_1_INT; i < oChapterLevelInfoDictContainer.Count; ++i) {
			for(int j = 0; j < oChapterLevelInfoDictContainer[i].Count; ++j) {
				oChapterLevelInfoDictContainer[i][j].m_stIDInfo.m_nStageID -= KCDefine.B_VAL_1_INT;
			}

			oChapterLevelInfoDictContainer.ExReplaceVal(i - KCDefine.B_VAL_1_INT, oChapterLevelInfoDictContainer[i]);
		}

		oChapterLevelInfoDictContainer.Remove(oChapterLevelInfoDictContainer.Count - KCDefine.B_VAL_1_INT);
	}

	//! 챕터 레벨 정보를 제거한다
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

		this.LevelInfoDictContainer.Remove(this.LevelInfoDictContainer.Count - KCDefine.B_VAL_1_INT);
	}

	//! 레벨 정보를 이동한다
	public void MoveLevelInfo(int a_nFromID, int a_nToID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);

		CAccess.Assert(bIsValid && oStageLevelInfoDict.ExIsValid());
		CAccess.Assert(oStageLevelInfoDict.ContainsKey(a_nFromID) && oStageLevelInfoDict.ContainsKey(a_nToID));

		var oFromLevelInfo = oStageLevelInfoDict[a_nFromID];
		int nOffset = (a_nFromID <= a_nToID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;

		oStageLevelInfoDict.Remove(a_nFromID);

		for(int i = a_nFromID + nOffset; i != a_nToID + nOffset; i += nOffset) {
			oStageLevelInfoDict[i].m_stIDInfo.m_nID -= nOffset;
			oStageLevelInfoDict.ExReplaceVal(i - nOffset, oStageLevelInfoDict[i]);
		}

		oFromLevelInfo.m_stIDInfo.m_nID = a_nToID;
		oStageLevelInfoDict.ExReplaceVal(a_nToID, oFromLevelInfo);
	}

	//! 스테이지 레벨 정보를 이동한다
	public void MoveStageLevelInfos(int a_nFromID, int a_nToID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);

		CAccess.Assert(bIsValid && oChapterLevelInfoDictContainer.ExIsValid());
		CAccess.Assert(oChapterLevelInfoDictContainer.ContainsKey(a_nFromID) && oChapterLevelInfoDictContainer.ContainsKey(a_nToID));

		var oFromStageLevelInfoDict = oChapterLevelInfoDictContainer[a_nFromID];
		int nOffset = (a_nFromID <= a_nToID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;

		oChapterLevelInfoDictContainer.Remove(a_nFromID);

		for(int i = a_nFromID + nOffset; i != a_nToID + nOffset; i += nOffset) {
			for(int j = 0; j < oChapterLevelInfoDictContainer[i].Count; ++j) {
				oChapterLevelInfoDictContainer[i][j].m_stIDInfo.m_nStageID -= nOffset;
			}

			oChapterLevelInfoDictContainer.ExReplaceVal(i - nOffset, oChapterLevelInfoDictContainer[i]);
		}

		for(int i = 0; i < oFromStageLevelInfoDict.Count; ++i) {
			oFromStageLevelInfoDict[i].m_stIDInfo.m_nStageID = a_nToID;
		}

		oChapterLevelInfoDictContainer.ExReplaceVal(a_nToID, oFromStageLevelInfoDict);
	}

	//! 챕터 레벨 정보를 이동한다
	public void MoveChapterLevelInfos(int a_nFromID, int a_nToID) {
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nFromID) && this.LevelInfoDictContainer.ContainsKey(a_nToID));

		var oFromChapterLevelInfoDict = this.LevelInfoDictContainer[a_nFromID];
		int nOffset = (a_nFromID <= a_nToID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;

		for(int i = a_nFromID + nOffset; i != a_nToID + nOffset; i += nOffset) {
			for(int j = 0; j < this.LevelInfoDictContainer[i].Count; ++j) {
				for(int k = 0; k < this.LevelInfoDictContainer[i][j].Count; ++k) {
					this.LevelInfoDictContainer[i][j][k].m_stIDInfo.m_nChapterID -= nOffset;
				}
			}

			this.LevelInfoDictContainer.ExReplaceVal(i - nOffset, this.LevelInfoDictContainer[i]);
		}

		for(int i = 0; i < oFromChapterLevelInfoDict.Count; ++i) {
			for(int j = 0; j < oFromChapterLevelInfoDict[i].Count; ++j) {
				oFromChapterLevelInfoDict[i][j].m_stIDInfo.m_nChapterID = a_nToID;
			}
		}

		this.LevelInfoDictContainer.ExReplaceVal(a_nToID, oFromChapterLevelInfoDict);
	}

	//! 레벨 정보를 저장한다
	public void SaveLevelInfos() {
		var oLevelIDList = new List<long>();
		string oFilePath = KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON);

		for(int i = 0; i < this.LevelInfoDictContainer.Count; ++i) {
			for(int j = 0; j < this.LevelInfoDictContainer[i].Count; ++j) {
				for(int k = 0; k < this.LevelInfoDictContainer[i][j].Count; ++k) {
					this.LevelInfoDictContainer[i][j][k].m_stIDInfo = CFactory.MakeIDInfo(k, j, i);
					this.SaveLevelInfo(this.LevelInfoDictContainer[i][j][k], oLevelIDList);
				}
			}
		}

		CEpisodeInfoTable.Inst.SaveEpisodeInfos();
		CFunc.WriteMsgPackJSONObj(oFilePath, oLevelIDList, false, false);
	}

	//! 레벨 정보를 저장한다
	private void SaveLevelInfo(CLevelInfo a_oLevelInfo, List<long> a_oOutLevelIDList) {
		CAccess.Assert(a_oLevelInfo != null);
		
		a_oOutLevelIDList.Add(a_oLevelInfo.LevelID);
		CEpisodeInfoTable.Inst.TryGetLevelInfo(a_oLevelInfo.m_stIDInfo.m_nID, out STLevelInfo stLevelInfo, a_oLevelInfo.m_stIDInfo.m_nStageID, a_oLevelInfo.m_stIDInfo.m_nChapterID);

		CEpisodeInfoTable.Inst.LevelInfoDict.ExReplaceVal(a_oLevelInfo.LevelID, new STLevelInfo() {
			m_nID = stLevelInfo.m_nID,
			m_nStageID = stLevelInfo.m_nStageID,
			m_nChapterID = stLevelInfo.m_nChapterID,

			m_oName = stLevelInfo.m_oName ?? string.Empty,
			m_oDesc = stLevelInfo.m_oDesc ?? string.Empty,
			
			m_eLevelMode = a_oLevelInfo.LevelMode,
			m_eLevelKinds = a_oLevelInfo.LevelKinds,
			m_eRewardKinds = a_oLevelInfo.RewardKinds,
			m_eTutorialKinds = a_oLevelInfo.TutorialKinds
		});

		string oFilePath = string.Format(KDefine.G_RUNTIME_DATA_P_FMT_LEVEL_INFO, a_oLevelInfo.LevelID + KCDefine.B_VAL_1_INT);
		CFunc.WriteMsgPackObj(oFilePath, a_oLevelInfo, false, false);
	}
#endif			// #if UNITY_STANDALONE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
