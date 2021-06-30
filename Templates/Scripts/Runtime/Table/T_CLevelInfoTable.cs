using System.Collections;
using System.Collections.Generic;
using System.IO;
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
	private const string KEY_LEVEL_MODE = "LevelMode";
	#endregion			// 상수

	#region 프로퍼티
	[IgnoreMember] public int ID {
		get { return m_oIntList.ExGetVal(CLevelInfo.KEY_ID, KCDefine.B_VAL_0_INT); }
		set { m_oIntList.ExReplaceVal(CLevelInfo.KEY_ID, value); }
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
		bool bIsValid = this.TryGetLevelInfo(a_nID, out CLevelInfo oLevelInfo);
		CAccess.Assert(bIsValid);

		return oLevelInfo;
	}

	//! 레벨 정보를 반환한다
	public bool TryGetLevelInfo(int a_nID, out CLevelInfo a_oOutLevelInfo) {
		a_oOutLevelInfo = this.LevelInfoList.ExGetVal(a_nID, null);
		return this.LevelInfoList.ContainsKey(a_nID);
	}

	//! 레벨 정보를 로드한다
	public CLevelInfo LoadLevelInfo(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid() && File.Exists(a_oFilePath));
		return CFunc.ReadMsgPackObj<CLevelInfo>(a_oFilePath, false);
	}

	//! 레벨 정보를 로드한다
	public CLevelInfo LoadLevelInfoFromRes(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return CFunc.ReadMsgPackObjFromRes<CLevelInfo>(a_oFilePath, false);
	}

	//! 레벨 정보를 로드한다
	public Dictionary<int, CLevelInfo> LoadLevelInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadLevelInfos(KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO);
#else
		return this.LoadLevelInfosFromRes(KCDefine.U_TABLE_P_G_LEVEL_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 레벨 정보를 로드한다
	public Dictionary<int, CLevelInfo> LoadLevelInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		string oStr = CFunc.ReadStr(a_oFilePath);

		return this.DoLoadLevelInfos(oStr);
	}

	//! 레벨 정보를 로드한다
	public Dictionary<int, CLevelInfo> LoadLevelInfosFromRes(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadLevelInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
	}

	//! 레벨 정보를 로드한다
	private Dictionary<int, CLevelInfo> DoLoadLevelInfos(string a_oStr) {
		bool bIsValid = int.TryParse(a_oStr, out int nNumLevelInfos);
		CAccess.Assert(bIsValid);

		for(int i = 0; i < nNumLevelInfos; ++i) {
			string oFilePath = string.Format(KDefine.G_RUNTIME_DATA_P_FMT_LEVEL_INFO, i + KCDefine.B_VAL_1_INT);
			CFunc.ShowLog($"CLevelInfoTable.DoLoadLevelInfos: {oFilePath}");

#if UNITY_EDITOR
			var oLevelInfo = this.LoadLevelInfo(oFilePath);
#else
			var oLevelInfo = this.LoadLevelInfoFromRes(oFilePath);
#endif			// #if UNITY_EDITOR

			oLevelInfo.ID = i;
			this.LevelInfoList.ExAddVal(oLevelInfo.ID, oLevelInfo);
		}

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
		CFunc.WriteStr(KDefine.G_RUNTIME_TABLE_P_LEVEL_INFO, oStr);

		foreach(var stKeyVal in this.LevelInfoList) {
			this.SaveLevelInfo(stKeyVal.Value);
		}
	}

	//! 레벨 정보를 생성한다
	public CLevelInfo MakeLevelInfo(ELevelMode a_eLevelMode) {
		return new CLevelInfo() {
			ID = this.LevelInfoList.Count,
			LevelMode = a_eLevelMode
		};
	}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
