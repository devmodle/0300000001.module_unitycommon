using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 객체 정보 */
[System.Serializable]
public partial struct STObjInfo {
	public STCommonInfo m_stCommonInfo;
	public Vector3 m_stSize;

	public EObjKinds m_eObjKinds;
	public EObjKinds m_ePrevObjKinds;
	public EObjKinds m_eNextObjKinds;

	List<EResKinds> m_oResKindsList;
	List<STTargetInfo> m_oEquipableItemTargetInfoList;
	List<STTargetInfo> m_oSkillTargetInfoList;
	List<STAbilityValInfo> m_oAbilityValInfoList;

	#region 상수
	public static STObjInfo INVALID = new STObjInfo() {
		m_eObjKinds = EObjKinds.NONE, m_ePrevObjKinds = EObjKinds.NONE, m_eNextObjKinds = EObjKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EObjType ObjType => (EObjType)((int)m_eObjKinds).ExKindsToType();
	public EObjKinds BaseObjKinds => (EObjKinds)((int)m_eObjKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티
	
	#region 함수
	/** 생성자 */
	public STObjInfo(SimpleJSON.JSONNode a_oObjInfo) {
		m_stCommonInfo = new STCommonInfo(a_oObjInfo);
		m_stSize = new Vector3(a_oObjInfo[KCDefine.U_KEY_SIZE_X].AsFloat, a_oObjInfo[KCDefine.U_KEY_SIZE_Y].AsFloat, KCDefine.B_VAL_0_REAL);

		m_eObjKinds = a_oObjInfo[KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_ePrevObjKinds = a_oObjInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_eNextObjKinds = a_oObjInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].AsInt : EObjKinds.NONE;

		m_oResKindsList = new List<EResKinds>();
		m_oEquipableItemTargetInfoList = new List<STTargetInfo>();
		m_oSkillTargetInfoList = new List<STTargetInfo>();
		m_oAbilityValInfoList = new List<STAbilityValInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_RES_KINDS; ++i) {
			string oResKindsKey = string.Format(KCDefine.U_KEY_FMT_RES_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oResKindsList.Add(a_oObjInfo[oResKindsKey].ExIsValid() ? (EResKinds)a_oObjInfo[oResKindsKey].AsInt : EResKinds.NONE);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			m_oEquipableItemTargetInfoList.Add(new STTargetInfo(a_oObjInfo, KCDefine.U_PREFIX_EQUIPABLE_ITEM, i));
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			m_oSkillTargetInfoList.Add(new STTargetInfo(a_oObjInfo, KCDefine.U_PREFIX_SKILL, i));
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_ABILITY_VAL_INFOS; ++i) {
			m_oAbilityValInfoList.Add(new STAbilityValInfo(a_oObjInfo, i));
		}
	}
	#endregion			// 함수
}

/** 객체 정보 테이블 */
public partial class CObjInfoTable : CScriptableObj<CObjInfoTable> {
	#region 변수
	[Header("=====> BG Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oBGObjInfoList = new List<STObjInfo>();

	[Header("=====> Norm Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oNormObjInfoList = new List<STObjInfo>();

	[Header("=====> Overlay Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oOverlayObjInfoList = new List<STObjInfo>();

	[Header("=====> Playable Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oPlayableObjInfoList = new List<STObjInfo>();

	[Header("=====> Non Playable Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oNonPlayableObjInfoList = new List<STObjInfo>();

	[Header("=====> Enemy Obj Info <=====")]
	[SerializeField] private List<STObjInfo> m_oEnemyObjInfoList = new List<STObjInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EObjKinds, STObjInfo> ObjInfoDict { get; private set; } = new Dictionary<EObjKinds, STObjInfo>();

	private string ObjInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_OBJ_INFO;
#else
			return KCDefine.U_TABLE_P_G_OBJ_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetObjInfos();
	}

	/** 객체 정보를 리셋한다 */
	public void ResetObjInfos() {
		this.ObjInfoDict.Clear();

		var oObjInfoList = new List<STObjInfo>(m_oBGObjInfoList);
		oObjInfoList.AddRange(m_oNormObjInfoList);
		oObjInfoList.AddRange(m_oOverlayObjInfoList);
		oObjInfoList.AddRange(m_oPlayableObjInfoList);
		oObjInfoList.AddRange(m_oNonPlayableObjInfoList);
		oObjInfoList.AddRange(m_oEnemyObjInfoList);

		for(int i = 0; i < oObjInfoList.Count; ++i) {
			this.ObjInfoDict.TryAdd(oObjInfoList[i].m_eObjKinds, oObjInfoList[i]);
		}
	}

	/** 객체 정보를 리셋한다 */
	public void ResetObjInfos(string a_oJSONStr) {
		this.ResetObjInfos();
		this.DoLoadObjInfos(a_oJSONStr);
	}

	/** 객체 정보를 반환한다 */
	public STObjInfo GetObjInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetObjInfo(a_eObjKinds, out STObjInfo stObjInfo);
		CAccess.Assert(bIsValid);

		return stObjInfo;
	}

	/** 객체 정보를 반환한다 */
	public bool TryGetObjInfo(EObjKinds a_eObjKinds, out STObjInfo a_stOutObjInfo) {
		a_stOutObjInfo = this.ObjInfoDict.GetValueOrDefault(a_eObjKinds, default(STObjInfo));
		return this.ObjInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 객체 정보를 로드한다 */
	public Dictionary<EObjKinds, STObjInfo> LoadObjInfos() {
		this.ResetObjInfos();
		return this.LoadObjInfos(this.ObjInfoTablePath);
	}

	/** 객체 정보를 로드한다 */
	private Dictionary<EObjKinds, STObjInfo> LoadObjInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadObjInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadObjInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 객체 정보를 로드한다 */
	private Dictionary<EObjKinds, STObjInfo> DoLoadObjInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);

		var oObjInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_BG], oJSONNode[KCDefine.U_KEY_NORM], oJSONNode[KCDefine.U_KEY_OVERLAY], oJSONNode[KCDefine.U_KEY_PLAYABLE], oJSONNode[KCDefine.U_KEY_NON_PLAYABLE], oJSONNode[KCDefine.U_KEY_ENEMY]
		};

		for(int i = 0; i < oObjInfosList.Count; ++i) {
			for(int j = 0; j < oObjInfosList[i].Count; ++j) {
				var stObjInfo = new STObjInfo(oObjInfosList[i][j]);

				// 객체 정보가 추가 가능 할 경우
				if(stObjInfo.m_eObjKinds.ExIsValid() && (!this.ObjInfoDict.ContainsKey(stObjInfo.m_eObjKinds) || oObjInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ObjInfoDict.ExReplaceVal(stObjInfo.m_eObjKinds, stObjInfo);
				}
			}
		}

		return this.ObjInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
