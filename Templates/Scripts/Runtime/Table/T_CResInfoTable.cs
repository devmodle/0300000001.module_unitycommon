using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 리소스 정보 */
[System.Serializable]
public struct STResInfo {
	public string m_oName;
	public string m_oDesc;

	public long m_nID;
	public string m_oResPath;
	
	#region 함수
	/** 생성자 */
	public STResInfo(SimpleJSON.JSONNode a_oResInfo) {
		m_oName = a_oResInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oResInfo[KCDefine.U_KEY_DESC];
		
		m_nID = long.TryParse(a_oResInfo[KCDefine.U_KEY_ID], out long nID) ? nID : KCDefine.B_VAL_0_LONG;
		m_oResPath = a_oResInfo[KCDefine.U_KEY_RES_PATH];
	}
	#endregion			// 함수
}

/** 리소스 정보 테이블 */
public partial class CResInfoTable : CScriptableObj<CResInfoTable> {
	#region 변수
	[Header("=====> Res Info <=====")]
	[SerializeField] private List<STResInfo> m_oResInfoList = new List<STResInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<long, STResInfo> ResInfoDict { get; private set; } = new Dictionary<long, STResInfo>();

	private string ResInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_RES_INFO;
#else
			return KCDefine.U_TABLE_P_G_RES_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		for(int i = 0; i < m_oResInfoList.Count; ++i) {
			this.ResInfoDict.TryAdd(m_oResInfoList[i].m_nID, m_oResInfoList[i]);
		}
	}

	/** 리소스 정보를 반환한다 */
	public STResInfo GetResInfo(long a_nID) {
		bool bIsValid = this.TryGetResInfo(a_nID, out STResInfo stResInfo);
		CAccess.Assert(bIsValid);

		return stResInfo;
	}

	/** 리소스 정보를 반환한다 */
	public bool TryGetResInfo(long a_nID, out STResInfo a_stOutResInfo) {
		a_stOutResInfo = this.ResInfoDict.GetValueOrDefault(a_nID, KDefine.G_INVALID_RES_INFO);
		return this.ResInfoDict.ContainsKey(a_nID);
	}

	/** 리소스 정보를 로드한다 */
	public Dictionary<long, STResInfo> LoadResInfos() {
		return this.LoadResInfos(this.ResInfoTablePath);
	}

	/** 리소스 정보를 로드한다 */
	private Dictionary<long, STResInfo> LoadResInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadResInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadResInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 리소스 정보를 로드한다 */
	private Dictionary<long, STResInfo> DoLoadResInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oResInfos = oJSONNode[KCDefine.B_KEY_JSON_COMMON_DATA];

		for(int i = 0; i < oResInfos.Count; ++i) {
			var stResInfo = new STResInfo(oResInfos[i]);
			bool bIsReplace = oResInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

			// 리소스 정보가 추가 가능 할 경우
			if(bIsReplace || !this.ResInfoDict.ContainsKey(stResInfo.m_nID)) {
				this.ResInfoDict.ExReplaceVal(stResInfo.m_nID, stResInfo);
			}
		}

		return this.ResInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
