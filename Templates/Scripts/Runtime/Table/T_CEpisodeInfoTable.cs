using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 스테이지 정보
[System.Serializable]
public struct STStageInfo {
	public string m_oName;
	public string m_oDesc;

	public EStageType m_eStageType;
	public EStageKinds m_eStageKinds;
	public EStageMode m_eStageMode;

	#region 함수
	//! 생성자
	public STStageInfo(SimpleJSON.JSONNode a_oStageInfo) {
		m_oName = a_oStageInfo[KDefine.G_KEY_EPISODE_IT_NAME];
		m_oDesc = a_oStageInfo[KDefine.G_KEY_EPISODE_IT_DESC];

		m_eStageType = (EStageType)a_oStageInfo[KDefine.G_KEY_EPISODE_IT_STAGE_TYPE].AsInt;
		m_eStageKinds = (EStageKinds)a_oStageInfo[KDefine.G_KEY_EPISODE_IT_STAGE_KINDS].AsInt;
		m_eStageMode = (EStageMode)a_oStageInfo[KDefine.G_KEY_EPISODE_IT_STAGE_MODE].AsInt;
	}
	#endregion			// 함수
}

//! 챕터 정보
[System.Serializable]
public struct STChapterInfo {
	public string m_oName;
	public string m_oDesc;

	public EChapterType m_eChapterType;
	public EChapterKinds m_eChapterKinds;
	public EChapterMode m_eChapterMode;

	#region 함수
	//! 생성자
	public STChapterInfo(SimpleJSON.JSONNode a_oChapterInfo) {
		m_oName = a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_NAME];
		m_oDesc = a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_DESC];

		m_eChapterType = (EChapterType)a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_CHAPTER_TYPE].AsInt;
		m_eChapterKinds = (EChapterKinds)a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_CHAPTER_KINDS].AsInt;
		m_eChapterMode = (EChapterMode)a_oChapterInfo[KDefine.G_KEY_EPISODE_IT_CHAPTER_MODE].AsInt;
	}
	#endregion			// 함수
}

//! 에피소드 정보 테이블
public class CEpisodeInfoTable : CScriptableObj<CEpisodeInfoTable> {
	#region 변수
	[Header("Stage Info")]
	[SerializeField] private List<STStageInfo> m_oStageInfoList = new List<STStageInfo>();

	[Header("Chapter Info")]
	[SerializeField] private List<STChapterInfo> m_oChapterInfoList = new List<STChapterInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EStageKinds, STStageInfo> StageInfoList { get; private set; } = new Dictionary<EStageKinds, STStageInfo>();
	public Dictionary<EChapterKinds, STChapterInfo> ChapterInfoList { get; private set; } = new Dictionary<EChapterKinds, STChapterInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		this.SetupStageInfos();
		this.SetupChapterInfos();
	}

	//! 스테이지 정보를 반환한다
	public STStageInfo GetStageInfo(EStageKinds a_eStageKinds) {
		bool bIsValid = this.TryGetStageInfo(a_eStageKinds, out STStageInfo stStageInfo);
		CAccess.Assert(bIsValid);

		return stStageInfo;
	}

	//! 챕터 정보를 반환한다
	public STChapterInfo GetChapterInfo(EChapterKinds a_eChapterKinds) {
		bool bIsValid = this.TryGetChapterInfo(a_eChapterKinds, out STChapterInfo stChapterInfo);
		CAccess.Assert(bIsValid);

		return stChapterInfo;
	}

	//! 스테이지 정보를 반환한다
	public bool TryGetStageInfo(EStageKinds a_eStageKinds, out STStageInfo a_stOutStageInfo) {
		a_stOutStageInfo = this.StageInfoList.ExGetVal(a_eStageKinds, KDefine.G_INVALID_STAGE_INFO);
		return this.StageInfoList.ContainsKey(a_eStageKinds);
	}

	//! 챕터 정보를 반환한다
	public bool TryGetChapterInfo(EChapterKinds a_eChapterKinds, out STChapterInfo a_stOutChapterInfo) {
		a_stOutChapterInfo = this.ChapterInfoList.ExGetVal(a_eChapterKinds, KDefine.G_INVALID_CHAPTER_INFO);
		return this.ChapterInfoList.ContainsKey(a_eChapterKinds);
	}

	//! 에피소드 정보를 로드한다	
	public List<object> LoadEpisodeInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadEpisodeInfos(KDefine.G_RUNTIME_TABLE_P_EPISODE_INFO);
#else
		return this.LoadEpisodeInfos(KCDefine.U_TABLE_P_G_EPISODE_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 에피소드 정보를 로드한다
	public List<object> LoadEpisodeInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_EDITOR || UNITY_STANDALONE
		string oJSONStr = CFunc.ReadStr(a_oFilePath);
		return this.DoLoadEpisodeInfos(oJSONStr);
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadEpisodeInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 스테이지 정보를 설정한다
	private void SetupStageInfos() {
		CAccess.Assert(m_oStageInfoList != null && this.StageInfoList != null);

		for(int i = 0; i < m_oStageInfoList.Count; ++i) {
			var stStageInfo = m_oStageInfoList[i];
			this.StageInfoList.ExAddVal(stStageInfo.m_eStageKinds, stStageInfo);
		}
	}

	//! 챕터 정보를 설정한다
	private void SetupChapterInfos() {
		CAccess.Assert(m_oChapterInfoList != null && this.ChapterInfoList != null);

		for(int i = 0; i < m_oChapterInfoList.Count; ++i) {
			var stChapterInfo = m_oChapterInfoList[i];
			this.ChapterInfoList.ExAddVal(stChapterInfo.m_eChapterKinds, stChapterInfo);
		}
	}

	//! 에피소드 정보를 로드한다
	private List<object> DoLoadEpisodeInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;
		var oStageInfos = oJSONNode[KDefine.G_KEY_EPISODE_IT_STAGE];
		var oChapterInfos = oJSONNode[KDefine.G_KEY_EPISODE_IT_CHAPTER];

		for(int i = 0; i < oStageInfos.Count; ++i) {
			var stStageInfo = new STStageInfo(oStageInfos[i]);
			bool bIsReplace = oStageInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

			// 스테이지 정보가 추가 가능 할 경우
			if(bIsReplace || !this.StageInfoList.ContainsKey(stStageInfo.m_eStageKinds)) {
				this.StageInfoList.ExReplaceVal(stStageInfo.m_eStageKinds, stStageInfo);
			}
		}

		for(int i = 0; i < oChapterInfos.Count; ++i) {
			var stChapterInfo = new STChapterInfo(oChapterInfos[i]);
			bool bIsReplace = oChapterInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

			// 챕터 정보가 추가 가능 할 경우
			if(bIsReplace || !this.ChapterInfoList.ContainsKey(stChapterInfo.m_eChapterKinds)) {
				this.ChapterInfoList.ExReplaceVal(stChapterInfo.m_eChapterKinds, stChapterInfo);
			}
		}

#if UNITY_EDITOR
		this.SetupStageInfoList();
		this.SetupChapterInfoList();
#endif			// #if UNITY_EDITOR

		return new List<object>() {
			this.StageInfoList, this.ChapterInfoList
		};
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
	//! 스테이지 정보를 설정한다
	private void SetupStageInfoList() {
		m_oStageInfoList.Clear();

		foreach(var stKeyVal in this.StageInfoList) {
			m_oStageInfoList.ExAddVal(stKeyVal.Value);
		}

		m_oStageInfoList.Sort((a_stLhs, a_stRhs) => (int)a_stLhs.m_eStageKinds - (int)a_stRhs.m_eStageKinds);
	}

	//! 챕터 정보를 설정한다
	private void SetupChapterInfoList() {
		m_oChapterInfoList.Clear();

		foreach(var stKeyVal in this.ChapterInfoList) {
			m_oChapterInfoList.ExAddVal(stKeyVal.Value);
		}
		
		m_oChapterInfoList.Sort((a_stLhs, a_stRhs) => (int)a_stLhs.m_eChapterKinds - (int)a_stRhs.m_eChapterKinds);
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
