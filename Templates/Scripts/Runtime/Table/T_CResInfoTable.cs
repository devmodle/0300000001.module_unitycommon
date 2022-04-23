using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 리소스 정보 */
[System.Serializable]
public struct STResInfo {
	public float m_fRate;
	public string m_oResPath;
	public EResKinds m_eResKinds;

	#region 프로퍼티
	public EResType ResType => (EResType)((int)m_eResKinds).ExKindsToType();
	public EResKinds BaseResKinds => (EResKinds)((int)m_eResKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티
	
	#region 함수
	/** 생성자 */
	public STResInfo(SimpleJSON.JSONNode a_oResInfo) {
		m_fRate = a_oResInfo[KCDefine.U_KEY_RATE].AsFloat;
		m_oResPath = a_oResInfo[KCDefine.U_KEY_RES_PATH];
		m_eResKinds = a_oResInfo[KCDefine.U_KEY_RES_KINDS].ExIsValid() ? (EResKinds)a_oResInfo[KCDefine.U_KEY_RES_KINDS].AsInt : EResKinds.NONE;
	}
	#endregion			// 함수
}

/** 리소스 정보 테이블 */
public partial class CResInfoTable : CScriptableObj<CResInfoTable> {
	#region 변수
	[Header("=====> Snd Res Info <=====")]
	[SerializeField] private List<STResInfo> m_oSndResInfoList = new List<STResInfo>();

	[Header("=====> Font Res Info <=====")]
	[SerializeField] private List<STResInfo> m_oFontResInfoList = new List<STResInfo>();

	[Header("=====> Sprite Res Info <=====")]
	[SerializeField] private List<STResInfo> m_oSpriteResInfoList = new List<STResInfo>();

	[Header("=====> Texture Res Info <=====")]
	[SerializeField] private List<STResInfo> m_oTextureResInfoList = new List<STResInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EResKinds, STResInfo> ResInfoDict { get; private set; } = new Dictionary<EResKinds, STResInfo>();

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

		var oResInfoList = new List<STResInfo>(m_oSndResInfoList);
		oResInfoList.AddRange(m_oFontResInfoList);
		oResInfoList.AddRange(m_oSpriteResInfoList);
		oResInfoList.AddRange(m_oTextureResInfoList);

		for(int i = 0; i < oResInfoList.Count; ++i) {
			this.ResInfoDict.TryAdd(oResInfoList[i].m_eResKinds, oResInfoList[i]);
		}
	}

	/** 리소스 정보를 반환한다 */
	public STResInfo GetResInfo(EResKinds a_eResKinds) {
		bool bIsValid = this.TryGetResInfo(a_eResKinds, out STResInfo stResInfo);
		CAccess.Assert(bIsValid);

		return stResInfo;
	}

	/** 리소스 정보를 반환한다 */
	public bool TryGetResInfo(EResKinds a_eResKinds, out STResInfo a_stOutResInfo) {
		a_stOutResInfo = this.ResInfoDict.GetValueOrDefault(a_eResKinds, default(STResInfo));
		return this.ResInfoDict.ContainsKey(a_eResKinds);
	}

	/** 리소스 정보를 로드한다 */
	public Dictionary<EResKinds, STResInfo> LoadResInfos() {
		return this.LoadResInfos(this.ResInfoTablePath);
	}

	/** 리소스 정보를 로드한다 */
	private Dictionary<EResKinds, STResInfo> LoadResInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadResInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadResInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 리소스 정보를 로드한다 */
	private Dictionary<EResKinds, STResInfo> DoLoadResInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);

		var oResInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_SND], oJSONNode[KCDefine.U_KEY_FONT], oJSONNode[KCDefine.U_KEY_SPRITE], oJSONNode[KCDefine.U_KEY_TEXTURE]
		};

		for(int i = 0; i < oResInfosList.Count; ++i) {
			for(int j = 0; j < oResInfosList[i].Count; ++j) {
				var stResInfo = new STResInfo(oResInfosList[i][j]);

				// 리소스 정보가 추가 가능 할 경우
				if(!this.ResInfoDict.ContainsKey(stResInfo.m_eResKinds) || oResInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT) {
					this.ResInfoDict.ExReplaceVal(stResInfo.m_eResKinds, stResInfo);
				}
			}
		}

		return this.ResInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
