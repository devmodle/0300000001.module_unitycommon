using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

#if NEVER_USE_THIS
using SampleEngineName;

//! 레벨 정보
[MessagePackObject]
[System.Serializable]
public sealed class CLevelInfo : CBaseInfo {
	#region 상수
	private const string KEY_ID = "ID";
	private const string KEY_STAGE_ID = "StageID";
	private const string KEY_CHAPTER_ID = "ChapterID";
	private const string KEY_LEVEL_MODE = "LevelMode";
	#endregion			// 상수
	
	#region 프로퍼티
	[IgnoreMember] public int ID {
		get { return m_oIntList.ExGetVal(CLevelInfo.KEY_ID, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CLevelInfo.KEY_ID, value); }
	}

	[IgnoreMember] public int StageID {
		get { return m_oIntList.ExGetVal(CLevelInfo.KEY_STAGE_ID, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CLevelInfo.KEY_STAGE_ID, value); }
	}

	[IgnoreMember] public int ChapterID {
		get { return m_oIntList.ExGetVal(CLevelInfo.KEY_CHAPTER_ID, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CLevelInfo.KEY_CHAPTER_ID, value); }
	}

	[IgnoreMember] public ELevelMode LevelMode {
		get { return (ELevelMode)m_oIntList.ExGetVal(CLevelInfo.KEY_LEVEL_MODE, (int)ELevelMode.NONE); }
		set { m_oIntList.ExReplaceVal(CLevelInfo.KEY_LEVEL_MODE, (int)value); }
	}
	#endregion			// 프로퍼티

	#region 함수
	//! 생성자
	public CLevelInfo() : base(KDefine.B_VER_LEVEL_INFO) {
		// Do Nothing
	}
	#endregion			// 함수
}

//! 레벨 정보 테이블
public class CLevelInfoTable : CSingleton<CLevelInfoTable> {
	#region 프로퍼티
	public Dictionary<int, CLevelInfo> LevelInfoList { get; private set; } = new Dictionary<int, CLevelInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 레벨 정보를 반환한다
	public CLevelInfo GetLevelInfo(int a_nID) {
		return this.GetLevelInfo(a_nID, KCDefine.B_VAL_0_INT);
	}

	//! 레벨 정보를 반환한다
	public CLevelInfo GetLevelInfo(int a_nID, int a_nStageID) {
		return this.GetLevelInfo(a_nID, a_nStageID, KCDefine.B_VAL_0_INT);
	}

	//! 레벨 정보를 반환한다
	public CLevelInfo GetLevelInfo(int a_nID, int a_nStageID, int a_nChapterID) {
		bool bIsValid = this.TryGetLevelInfo(a_nID, a_nStageID, a_nChapterID, out CLevelInfo oLevelInfo);
		CAccess.Assert(bIsValid);

		return oLevelInfo;
	}

	//! 레벨 정보를 반환한다
	public bool TryGetLevelInfo(int a_nID, out CLevelInfo a_oOutLevelInfo) {
		return this.TryGetLevelInfo(a_nID, KCDefine.B_VAL_0_INT, out a_oOutLevelInfo);
	}

	//! 레벨 정보를 반환한다
	public bool TryGetLevelInfo(int a_nID, int a_nStageID, out CLevelInfo a_oOutLevelInfo) {
		return this.TryGetLevelInfo(a_nID, a_nStageID, KCDefine.B_VAL_0_INT, out a_oOutLevelInfo);
	}

	//! 레벨 정보를 반환한다
	public bool TryGetLevelInfo(int a_nID, int a_nStageID, int a_nChapterID, out CLevelInfo a_oOutLevelInfo) {
		int nID = Factory.MakeLevelID(a_nID, a_nStageID, a_nChapterID);
		a_oOutLevelInfo = this.LevelInfoList.ExGetVal(nID, null);

		return this.LevelInfoList.ContainsKey(a_nID);
	}

	//! 레벨 정보를 로드한다
	public Dictionary<int, CLevelInfo> LoadLevelInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadLevelInfos(KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO);
#else
		return this.LoadLevelInfos(KCDefine.U_TABLE_P_G_LEVEL_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 레벨 정보를 로드한다
	public Dictionary<int, CLevelInfo> LoadLevelInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_EDITOR || UNITY_STANDALONE
		string oFilePath = a_oFilePath.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_TXT);
		string oStr = CFunc.ReadStr(oFilePath);

		bool bIsValid = int.TryParse(oStr, out int nNumLevelInfos);
		CAccess.Assert(bIsValid);

		for(int i = 0; i < nNumLevelInfos; ++i) {
			string oPathFmt =  Application.isEditor ? KDefine.G_RUNTIME_DATA_P_FMT_LEVEL_INFO : KCDefine.U_DATA_P_FMT_G_LEVEL_INFO;
			string oLevelInfoFilePath = string.Format(oPathFmt, i + KCDefine.B_VAL_1_INT);

			CFunc.ShowLog($"CLevelInfoTable.LoadLevelInfos: {oLevelInfoFilePath}");
			
			var oLevelInfo = this.LoadLevelInfo(oLevelInfoFilePath);
			this.LevelInfoList.ExAddVal(oLevelInfo.ID, oLevelInfo);
		}
#else
		try {
			this.LevelInfoList = CFunc.ReadMsgPackObjFromRes<Dictionary<int, CLevelInfo>>(a_oFilePath, false);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}		
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE

		return this.LevelInfoList;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR || UNITY_STANDALONE
	//! 레벨 정보를 추가한다
	public void AddLevelInfo(CLevelInfo a_oLevelInfo) {
		CAccess.Assert(a_oLevelInfo != null && !this.LevelInfoList.ContainsKey(a_oLevelInfo.ID));
		this.LevelInfoList.Add(a_oLevelInfo.ID, a_oLevelInfo);
	}

	//! 레벨 정보를 제거한다
	public void RemoveLevelInfo(CLevelInfo a_oLevelInfo) {
		CAccess.Assert(a_oLevelInfo != null && this.LevelInfoList.ContainsKey(a_oLevelInfo.ID));
		this.LevelInfoList.Remove(a_oLevelInfo.ID);
		
		for(int i = a_oLevelInfo.ID; i <= this.LevelInfoList.Count; ++i) {
			int nID = i + KCDefine.B_VAL_1_INT;

			this.LevelInfoList[nID].ID = i;
			this.LevelInfoList.ExReplaceVal(i, this.LevelInfoList[nID]);
		}
	}

	//! 레벨 정보를 저장한다
	public void SaveLevelInfo(CLevelInfo a_oLevelInfo) {
		CAccess.Assert(a_oLevelInfo != null);
		string oFilePath = string.Format(KDefine.G_RUNTIME_DATA_P_FMT_LEVEL_INFO, a_oLevelInfo.ID + KCDefine.B_VAL_1_INT);

		CFunc.WriteMsgPackObj(oFilePath, a_oLevelInfo, false, false);
	}

	//! 레벨 정보를 저장한다
	public void SaveLevelInfos() {
		string oStr = string.Format(KCDefine.B_TEXT_FMT_1_DIGITS, this.LevelInfoList.Count);		
		string oFilePath = KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO.ExGetReplaceStr(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_TXT);

		foreach(var stKeyVal in this.LevelInfoList) {
			this.SaveLevelInfo(stKeyVal.Value);
		}

		CFunc.WriteStr(oFilePath, oStr);
		CFunc.WriteMsgPackObj(KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO, this.LevelInfoList, false, false);
	}

	//! 레벨 정보를 로드한다
	private CLevelInfo LoadLevelInfo(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return CFunc.ReadMsgPackObj<CLevelInfo>(a_oFilePath, false);
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
