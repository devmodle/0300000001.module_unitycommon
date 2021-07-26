using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

#if NEVER_USE_THIS
//! 셀 정보
[MessagePackObject]
[System.Serializable]
public sealed class CCellInfo : CBaseInfo, System.ICloneable {
	#region 변수
	[Key(41)] public List<SampleEngineName.EBlockKinds> m_oBlockKindsList = new List<SampleEngineName.EBlockKinds>();
	#endregion			// 변수ㄴ

	#region 인터페이스
	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		m_oBlockKindsList = m_oBlockKindsList ?? new List<SampleEngineName.EBlockKinds>();
	}
	#endregion			// 인터페이스

	#region 함수
	//! 생성자
	public CCellInfo() : base(KDefine.G_VER_CELL_INFO) {
		// Do Nothing
	}

	//! 사본 객체를 생성한다
	public object Clone() {
		var oCellInfo = new CCellInfo();
		oCellInfo.OnAfterDeserialize();

		return oCellInfo;
	}
	#endregion			// 함수
}

//! 레벨 정보
[MessagePackObject]
[System.Serializable]
public sealed class CLevelInfo : CBaseInfo, System.ICloneable {
	#region 변수
	[Key(5)] public STIDInfo m_stIDInfo;
	[Key(201)] public Dictionary<int, Dictionary<int, CCellInfo>> m_oCellInfoDictContainer = new Dictionary<int, Dictionary<int, CCellInfo>>();
	#endregion			// 변수
	
	#region 프로퍼티
	[IgnoreMember] public Vector3Int NumCells { get; set; } = Vector3Int.zero;
	[IgnoreMember] public long LevelID => CFactory.MakeUniqueLevelID(m_stIDInfo.m_nID, m_stIDInfo.m_nStageID, m_stIDInfo.m_nChapterID);
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		m_oCellInfoDictContainer = m_oCellInfoDictContainer ?? new Dictionary<int, Dictionary<int, CCellInfo>>();

		// 셀 개수를 설정한다 {
		var stNumCells = new Vector3Int(KCDefine.B_VAL_0_INT, m_oCellInfoDictContainer.Count, KCDefine.B_VAL_0_INT);

		foreach(var stKeyVal in m_oCellInfoDictContainer) {
			stNumCells.x = Mathf.Max(stNumCells.x, stKeyVal.Value.Count);
		}

		this.NumCells = stNumCells;
		// 셀 개수를 설정한다 }
	}
	#endregion			// 인터페이스

	#region 함수
	//! 생성자
	public CLevelInfo() : base(KDefine.G_VER_LEVEL_INFO) {
		// Do Nothing
	}

	//! 사본 객체를 생성한다
	public object Clone() {
		var oLevelInfo = new CLevelInfo();
		oLevelInfo.m_stIDInfo = m_stIDInfo;

		foreach(var stKeyVal in m_oCellInfoDictContainer) {
			var oCellInfoDict = new Dictionary<int, CCellInfo>();

			foreach(var stCellInfoKeyVal in oCellInfoDict) {
				var oCellInfo = stCellInfoKeyVal.Value.Clone() as CCellInfo;
				oCellInfoDict.Add(stCellInfoKeyVal.Key, oCellInfo);
			}

			oLevelInfo.m_oCellInfoDictContainer.Add(stKeyVal.Key, oCellInfoDict);
		}

		oLevelInfo.OnAfterDeserialize();
		return oLevelInfo;
	}
	#endregion			// 함수
}

//! 레벨 정보 테이블
public class CLevelInfoTable : CSingleton<CLevelInfoTable> {
	#region 프로퍼티
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LevelInfoDictContainer = new Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>>();
	public int NumChapterInfos => this.LevelInfoDictContainer.Count;
	#endregion			// 프로퍼티

	#region 함수
	//! 레벨 정보 개수를 반환한다
	public int GetNumLevelInfos(int a_nID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nChapterID) && this.LevelInfoDictContainer[a_nChapterID].ContainsKey(a_nID));
		return this.LevelInfoDictContainer[a_nChapterID][a_nID].Count;
	}

	//! 스테이지 정보 개수를 반화한다
	public int GetNumStageInfos(int a_nChapterID) {
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nChapterID));
		return this.LevelInfoDictContainer[a_nChapterID].Count;
	}
	
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
		bool bIsValid = this.TryGetChapterLevelInfos(a_nID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDict);
		CAccess.Assert(bIsValid);

		return oChapterLevelInfoDict;
	}

	//! 레벨 정보를 반환한다
	public bool TryGetLevelInfo(int a_nID, out CLevelInfo a_oOutLevelInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);
		a_oOutLevelInfo = oStageLevelInfoDict?.ExGetVal(a_nID, null);

		return a_oOutLevelInfo != null;
	}

	//! 스테이지 레벨 정보를 반환한다
	public bool TryGetStageLevelInfos(int a_nID, out Dictionary<int, CLevelInfo> a_oOutStageLevelInfoDict, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDict);
		a_oOutStageLevelInfoDict = oChapterLevelInfoDict?.ExGetVal(a_nID, null);

		return a_oOutStageLevelInfoDict != null;
	}

	//! 챕터 레벨 정보를 반환한다
	public bool TryGetChapterLevelInfos(int a_nID, out Dictionary<int, Dictionary<int, CLevelInfo>> a_oOutChapterLevelInfoDict) {
		a_oOutChapterLevelInfoDict = this.LevelInfoDictContainer.ExGetVal(a_nID, null);
		return a_oOutChapterLevelInfoDict != null;
	}

	//! 레벨 정보를 로드한다
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LoadLevelInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadLevelInfos(KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO);
#else
		return this.LoadLevelInfos(KCDefine.U_TABLE_P_G_LEVEL_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 레벨 정보를 로드한다
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LoadLevelInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_EDITOR || UNITY_STANDALONE
		string oFilePath = a_oFilePath.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON);
		var oLevelIDList = CFunc.ReadMsgPackJSONObj<List<long>>(oFilePath, false);

		CAccess.Assert(oLevelIDList != null);

		for(int i = 0; i < oLevelIDList.Count; ++i) {
			string oLevelInfoFilePath = string.Format(KDefine.G_RUNTIME_DATA_P_FMT_LEVEL_INFO, oLevelIDList[i] + KCDefine.B_VAL_1_INT);			
			var oLevelInfo = this.LoadLevelInfo(oLevelInfoFilePath);

			this.AddLevelInfo(oLevelInfo);
		}
#else
		try {
			this.LevelInfoDictContainer = CFunc.ReadMsgPackObjFromRes<Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>>>(a_oFilePath, false);
			CAccess.Assert(this.LevelInfoDictContainer != null);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE

		return this.LevelInfoDictContainer;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	//! 레벨 정보를 추가한다
	public void AddLevelInfo(CLevelInfo a_oLevelInfo) {
		CAccess.Assert(a_oLevelInfo != null);

		var oChapterLevelInfoDict = this.LevelInfoDictContainer.ExGetVal(a_oLevelInfo.m_stIDInfo.m_nChapterID, null);
		oChapterLevelInfoDict = oChapterLevelInfoDict ?? new Dictionary<int, Dictionary<int, CLevelInfo>>();

		var oStageLevelInfoDict = oChapterLevelInfoDict?.ExGetVal(a_oLevelInfo.m_stIDInfo.m_nStageID, null);
		oStageLevelInfoDict = oStageLevelInfoDict ?? new Dictionary<int, CLevelInfo>();

		oStageLevelInfoDict.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nID, a_oLevelInfo);
		oChapterLevelInfoDict.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nStageID, oStageLevelInfoDict);
		this.LevelInfoDictContainer.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nChapterID, oChapterLevelInfoDict);
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
		bool bIsValid = this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDict);
		CAccess.Assert(bIsValid && oChapterLevelInfoDict.ExIsValid());

		for(int i = a_nID + KCDefine.B_VAL_1_INT; i < oChapterLevelInfoDict.Count; ++i) {
			foreach(var stKeyVal in oChapterLevelInfoDict[i]) {
				stKeyVal.Value.m_stIDInfo.m_nStageID -= KCDefine.B_VAL_1_INT;
			}

			oChapterLevelInfoDict.ExReplaceVal(i - KCDefine.B_VAL_1_INT, oChapterLevelInfoDict[i]);
		}

		oChapterLevelInfoDict.Remove(oChapterLevelInfoDict.Count - KCDefine.B_VAL_1_INT);
	}

	//! 챕터 레벨 정보를 제거한다
	public void RemoveChapterLevelInfos(int a_nID) {
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nID));

		for(int i = a_nID + KCDefine.B_VAL_1_INT; i < this.LevelInfoDictContainer.Count; ++i) {
			foreach(var stKeyVal in this.LevelInfoDictContainer[i]) {
				foreach(var stLevelInfoKeyVal in stKeyVal.Value) {
					stLevelInfoKeyVal.Value.m_stIDInfo.m_nChapterID -= KCDefine.B_VAL_1_INT;
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
		bool bIsValid = this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDict);

		CAccess.Assert(bIsValid && oChapterLevelInfoDict.ExIsValid());
		CAccess.Assert(oChapterLevelInfoDict.ContainsKey(a_nFromID) && oChapterLevelInfoDict.ContainsKey(a_nToID));

		var oFromStageLevelInfoDict = oChapterLevelInfoDict[a_nFromID];
		int nOffset = (a_nFromID <= a_nToID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;

		oChapterLevelInfoDict.Remove(a_nFromID);

		for(int i = a_nFromID + nOffset; i != a_nToID + nOffset; i += nOffset) {
			foreach(var stKeyVal in oChapterLevelInfoDict[i]) {
				stKeyVal.Value.m_stIDInfo.m_nStageID -= nOffset;
			}

			oChapterLevelInfoDict.ExReplaceVal(i - nOffset, oChapterLevelInfoDict[i]);
		}

		foreach(var stKeyVal in oFromStageLevelInfoDict) {
			stKeyVal.Value.m_stIDInfo.m_nStageID = a_nToID;
		}

		oChapterLevelInfoDict.ExReplaceVal(a_nToID, oFromStageLevelInfoDict);
	}

	//! 챕터 레벨 정보를 이동한다
	public void MoveChapterLevelInfos(int a_nFromID, int a_nToID) {
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nFromID) && this.LevelInfoDictContainer.ContainsKey(a_nToID));

		var oFromChapterLevelInfoDict = this.LevelInfoDictContainer[a_nFromID];
		int nOffset = (a_nFromID <= a_nToID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;

		for(int i = a_nFromID + nOffset; i != a_nToID + nOffset; i += nOffset) {
			foreach(var stKeyVal in this.LevelInfoDictContainer[i]) {
				foreach(var stLevelInfoKeyVal in stKeyVal.Value) {
					stLevelInfoKeyVal.Value.m_stIDInfo.m_nChapterID -= nOffset;
				}
			}

			this.LevelInfoDictContainer.ExReplaceVal(i - nOffset, this.LevelInfoDictContainer[i]);
		}

		foreach(var stKeyVal in oFromChapterLevelInfoDict) {
			foreach(var stLevelInfoKeyVal in stKeyVal.Value) {
				stLevelInfoKeyVal.Value.m_stIDInfo.m_nChapterID = a_nToID;
			}
		}

		this.LevelInfoDictContainer.ExReplaceVal(a_nToID, oFromChapterLevelInfoDict);
	}

	//! 레벨 정보를 저장한다
	public void SaveLevelInfos() {
		var oLevelIDList = new List<long>();
		string oFilePath = KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON);

		foreach(var stKeyVal in this.LevelInfoDictContainer) {
			foreach(var stStageInfoKeyVal in stKeyVal.Value) {
				foreach(var stLevelInfoKeyVal in stStageInfoKeyVal.Value) {
					this.SaveLevelInfo(stLevelInfoKeyVal.Value, oLevelIDList);
				}
			}
		}

		CFunc.WriteMsgPackJSONObj(oFilePath, oLevelIDList, false, false);
		CFunc.WriteMsgPackObj(KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO, this.LevelInfoDictContainer, false, false);
	}

	//! 레벨 정보를 저장한다
	private void SaveLevelInfo(CLevelInfo a_oLevelInfo, List<long> a_oOutLevelIDList) {
		CAccess.Assert(a_oLevelInfo != null);
		string oFilePath = string.Format(KDefine.G_RUNTIME_DATA_P_FMT_LEVEL_INFO, a_oLevelInfo.LevelID + KCDefine.B_VAL_1_INT);

		a_oOutLevelIDList.Add(a_oLevelInfo.LevelID);
		CFunc.WriteMsgPackObj(oFilePath, a_oLevelInfo, false, false);
	}

	//! 레벨 정보를 로드한다
	private CLevelInfo LoadLevelInfo(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		CFunc.ShowLog($"CLevelInfoTable.LoadLevelInfo: {a_oFilePath}");

		return CFunc.ReadMsgPackObj<CLevelInfo>(a_oFilePath, false);
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
