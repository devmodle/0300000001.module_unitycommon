using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if RUNTIME_TEMPLATES_MODULE_ENABLE
/** 효과 정보 */
[System.Serializable]
public struct STFXInfo {
	public string m_oName;
	public string m_oDesc;

	public EFXKinds m_eFXKinds;
	public EFXKinds m_eNextFXKinds;

	#region 함수
	/** 생성자 */
	public STFXInfo(SimpleJSON.JSONNode a_oFXInfo) {
		m_oName = a_oFXInfo[KCDefine.U_KEY_NAME];
		m_oDesc = a_oFXInfo[KCDefine.U_KEY_DESC];

		m_eFXKinds = (EFXKinds)a_oFXInfo[KCDefine.U_KEY_FX_KINDS].AsInt;
		m_eNextFXKinds = (EFXKinds)a_oFXInfo[KCDefine.U_KEY_NEXT_FX_KINDS].AsInt;
	}
	#endregion			// 함수
}

/** 효과 정보 테이블 */
public class CFXInfoTable : CScriptableObj<CFXInfoTable> {
	#region 변수
	[Header("=====> FX Info <=====")]
	[SerializeField] private List<STFXInfo> m_oFXInfoInfoList = new List<STFXInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EFXKinds, STFXInfo> FXInfoDict { get; private set; } = new Dictionary<EFXKinds, STFXInfo>();

	private string FXInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_FX_INFO;
#else
			return KCDefine.U_TABLE_P_G_FX_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 추가 프로퍼티

	#endregion			// 추가 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		for(int i = 0; i < m_oFXInfoInfoList.Count; ++i) {
			this.FXInfoDict.TryAdd(m_oFXInfoInfoList[i].m_eFXKinds, m_oFXInfoInfoList[i]);
		}
	}

	/** 효과 정보를 반환한다 */
	public STFXInfo GetFXInfo(EFXKinds a_eFXKinds) {
		bool bIsValid = this.TryGetFXInfo(a_eFXKinds, out STFXInfo stFXInfo);
		CAccess.Assert(bIsValid);

		return stFXInfo;
	}

	/** 효과 정보를 반환한다 */
	public bool TryGetFXInfo(EFXKinds a_eFXKinds, out STFXInfo a_stOutFXInfo) {
		a_stOutFXInfo = this.FXInfoDict.GetValueOrDefault(a_eFXKinds, KDefine.G_INVALID_FX_INFO);
		return this.FXInfoDict.ContainsKey(a_eFXKinds);
	}

	/** 효과 정보를 로드한다 */
	public Dictionary<EFXKinds, STFXInfo> LoadFXInfos() {
		return this.LoadFXInfos(this.FXInfoTablePath);
	}

	/** 효과 정보를 로드한다 */
	private Dictionary<EFXKinds, STFXInfo> LoadFXInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadFXInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadFXInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 효과 정보를 로드한다 */
	private Dictionary<EFXKinds, STFXInfo> DoLoadFXInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);
		var oFXInfos = oJSONNode[KCDefine.B_KEY_JSON_COMMON_DATA];

		for(int i = 0; i < oFXInfos.Count; ++i) {
			var stFXInfo = new STFXInfo(oFXInfos[i]);
			bool bIsReplace = oFXInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

			// 효과 정보가 추가 가능 할 경우
			if(bIsReplace || !this.FXInfoDict.ContainsKey(stFXInfo.m_eFXKinds)) {
				this.FXInfoDict.ExReplaceVal(stFXInfo.m_eFXKinds, stFXInfo);
			}
		}

		return this.FXInfoDict;
	}
	#endregion			// 함수

	#region 추가 함수

	#endregion			// 추가 함수
}
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
