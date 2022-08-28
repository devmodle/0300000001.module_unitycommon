#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 효과 정보 */
[System.Serializable]
public struct STFXInfo {
	public STCommonInfo m_stCommonInfo;
	public STTimeInfo m_stTimeInfo;

	public EFXKinds m_eFXKinds;
	public EFXKinds m_ePrevFXKinds;
	public EFXKinds m_eNextFXKinds;

	List<EResKinds> m_oResKindsList;

	#region 상수
	public static STFXInfo INVALID = new STFXInfo() {
		m_eFXKinds = EFXKinds.NONE, m_ePrevFXKinds = EFXKinds.NONE, m_eNextFXKinds = EFXKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EFXType FXType => (EFXType)((int)m_eFXKinds).ExKindsToType();
	public EFXKinds BaseFXKinds => (EFXKinds)((int)m_eFXKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STFXInfo(SimpleJSON.JSONNode a_oFXInfo) {
		m_stCommonInfo = new STCommonInfo(a_oFXInfo);
		m_stTimeInfo = new STTimeInfo(a_oFXInfo[KCDefine.U_KEY_TIME_INFO]);

		m_eFXKinds = a_oFXInfo[KCDefine.U_KEY_FX_KINDS].ExIsValid() ? (EFXKinds)a_oFXInfo[KCDefine.U_KEY_FX_KINDS].AsInt : EFXKinds.NONE;
		m_ePrevFXKinds = a_oFXInfo[KCDefine.U_KEY_PREV_FX_KINDS].ExIsValid() ? (EFXKinds)a_oFXInfo[KCDefine.U_KEY_PREV_FX_KINDS].AsInt : EFXKinds.NONE;
		m_eNextFXKinds = a_oFXInfo[KCDefine.U_KEY_NEXT_FX_KINDS].ExIsValid() ? (EFXKinds)a_oFXInfo[KCDefine.U_KEY_NEXT_FX_KINDS].AsInt : EFXKinds.NONE;

		m_oResKindsList = new List<EResKinds>();

		for(int i = 0; i < KDefine.G_MAX_NUM_RES_KINDS; ++i) {
			string oKey = string.Format(KCDefine.U_KEY_FMT_RES_KINDS, i + KCDefine.B_VAL_1_INT);
			if(a_oFXInfo[oKey].ExIsValid()) { m_oResKindsList.ExAddVal((EResKinds)a_oFXInfo[oKey].AsInt); }
		}
	}
	#endregion			// 함수
}

/** 효과 정보 테이블 */
public partial class CFXInfoTable : CSingleton<CFXInfoTable> {
	#region 프로퍼티
	public Dictionary<EFXKinds, STFXInfo> FXInfoDict { get; } = new Dictionary<EFXKinds, STFXInfo>();
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetFXInfos();
	}

	/** 효과 정보를 리셋한다 */
	public virtual void ResetFXInfos() {
		this.FXInfoDict.Clear();
	}

	/** 효과 정보를 리셋한다 */
	public virtual void ResetFXInfos(string a_oJSONStr) {
		this.ResetFXInfos();
		this.DoLoadFXInfos(a_oJSONStr);
	}

	/** 효과 정보를 반환한다 */
	public STFXInfo GetFXInfo(EFXKinds a_eFXKinds) {
		bool bIsValid = this.TryGetFXInfo(a_eFXKinds, out STFXInfo stFXInfo);
		CAccess.Assert(bIsValid);

		return stFXInfo;
	}

	/** 효과 정보를 반환한다 */
	public bool TryGetFXInfo(EFXKinds a_eFXKinds, out STFXInfo a_stOutFXInfo) {
		a_stOutFXInfo = this.FXInfoDict.GetValueOrDefault(a_eFXKinds, STFXInfo.INVALID);
		return this.FXInfoDict.ContainsKey(a_eFXKinds);
	}

	/** 효과 정보를 로드한다 */
	public Dictionary<EFXKinds, STFXInfo> LoadFXInfos() {
		this.ResetFXInfos();
		return this.LoadFXInfos(Access.FXInfoTableLoadPath);
	}

	/** 효과 정보를 저장한다 */
	public void SaveFXInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetFXInfos(a_oJSONStr);
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out List<SimpleJSON.JSONNode> a_oOutFXInfosList) {
		a_oOutFXInfosList = new List<SimpleJSON.JSONNode>();
		var oTableInfoDictContainer = KDefine.G_TABLE_INFO_DICT_CONTAINER.GetValueOrDefault(Path.GetFileNameWithoutExtension(Access.FXInfoTableLoadPath));

		// 공용 정보가 존재 할 경우
		if(oTableInfoDictContainer.Item2[this.GetType()].ContainsKey(KCDefine.B_KEY_COMMON)) {
			for(int i = 0; i < oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON].Count; ++i) {
				a_oOutFXInfosList.ExAddVal(a_oJSONNode[oTableInfoDictContainer.Item2[this.GetType()][KCDefine.B_KEY_COMMON][i]]);
			}
		}
	}

	/** 효과 정보를 로드한다 */
	private Dictionary<EFXKinds, STFXInfo> LoadFXInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
		return this.DoLoadFXInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false));
#else
		return this.DoLoadFXInfos(File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false));
#endif			// #if UNITY_EDITOR || (UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD))
	}

	/** 효과 정보를 로드한다 */
	private Dictionary<EFXKinds, STFXInfo> DoLoadFXInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out List<SimpleJSON.JSONNode> oFXInfosList);

		for(int i = 0; i < oFXInfosList.Count; ++i) {
			for(int j = 0; j < oFXInfosList[i].Count; ++j) {
				var stFXInfo = new STFXInfo(oFXInfosList[i][j]);

				// 효과 정보 추가 가능 할 경우
				if(stFXInfo.m_eFXKinds.ExIsValid() && (!this.FXInfoDict.ContainsKey(stFXInfo.m_eFXKinds) || oFXInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.FXInfoDict.ExReplaceVal(stFXInfo.m_eFXKinds, stFXInfo);
				}
			}
		}

		return this.FXInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
