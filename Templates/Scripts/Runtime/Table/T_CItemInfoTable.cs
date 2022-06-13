using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 아이템 정보 */
[System.Serializable]
public partial struct STItemInfo {
	public STDescInfo m_stDescInfo;

	public EItemKinds m_eItemKinds;
	public EItemKinds m_ePrevItemKinds;
	public EItemKinds m_eNextItemKinds;

	public List<EItemKinds> m_oAttachmentsItemKindsList;
	public List<ESkillKinds> m_oSkillKindsList;
	public List<STAbilityValInfo> m_oAbilityValInfoList;

	#region 프로퍼티
	public EItemType ItemType => (EItemType)((int)m_eItemKinds).ExKindsToType();
	public EItemKinds BaseItemKinds => (EItemKinds)((int)m_eItemKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STItemInfo(SimpleJSON.JSONNode a_oItemInfo) {
		m_stDescInfo = new STDescInfo(a_oItemInfo);
		
		m_eItemKinds = a_oItemInfo[KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_ePrevItemKinds = a_oItemInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_eNextItemKinds = a_oItemInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].AsInt : EItemKinds.NONE;

		m_oAttachmentsItemKindsList = new List<EItemKinds>();
		m_oSkillKindsList = new List<ESkillKinds>();
		m_oAbilityValInfoList = new List<STAbilityValInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_ATTACHMENTS_ITEM_KINDS; ++i) {
			string oAttachmentsItemKindsKey = string.Format(KCDefine.U_KEY_FMT_ATTACHMENTS_ITEM_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oAttachmentsItemKindsList.Add(a_oItemInfo[oAttachmentsItemKindsKey].ExIsValid() ? (EItemKinds)a_oItemInfo[oAttachmentsItemKindsKey].AsInt : EItemKinds.NONE);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_SKILL_KINDS; ++i) {
			string oSkillKindsKey = string.Format(KCDefine.U_KEY_FMT_SKILL_KINDS, i + KCDefine.B_VAL_1_INT);
			m_oSkillKindsList.Add(a_oItemInfo[oSkillKindsKey].ExIsValid() ? (ESkillKinds)a_oItemInfo[oSkillKindsKey].AsInt : ESkillKinds.NONE);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_ABILITY_VAL_INFOS; ++i) {
			string oAbilityLVKey = string.Format(KCDefine.U_KEY_FMT_ABILITY_LV, i + KCDefine.B_VAL_1_INT);
			string oAbilityKindsKey = string.Format(KCDefine.U_KEY_FMT_ABILITY_KINDS, i + KCDefine.B_VAL_1_INT);

			m_oAbilityValInfoList.Add(new STAbilityValInfo() {
				m_nLV = long.TryParse(a_oItemInfo[oAbilityLVKey], out long nLV) ? nLV : KCDefine.B_VAL_0_LONG, m_eAbilityKinds = a_oItemInfo[oAbilityKindsKey].ExIsValid() ? (EAbilityKinds)a_oItemInfo[oAbilityKindsKey].AsInt : EAbilityKinds.NONE
			});
		}
	}
	#endregion			// 함수
}

/** 아이템 정보 테이블 */
public partial class CItemInfoTable : CScriptableObj<CItemInfoTable> {
	#region 변수
	[Header("=====> Goods Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oGoodsItemInfoList = new List<STItemInfo>();

	[Header("=====> Consumable Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oConsumableItemInfoList = new List<STItemInfo>();

	[Header("=====> Non Consumable Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oNonConsumableItemInfoList = new List<STItemInfo>();

	[Header("=====> Weapon Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oWeaponItemInfoList = new List<STItemInfo>();

	[Header("=====> Armor Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oArmorItemInfoList = new List<STItemInfo>();

	[Header("=====> Accessory Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oAccessoryItemInfoList = new List<STItemInfo>();

	[Header("=====> Attachments Item Info <=====")]
	[SerializeField] private List<STItemInfo> m_oAttachmentsItemInfoList = new List<STItemInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EItemKinds, STItemInfo> ItemInfoDict { get; private set; } = new Dictionary<EItemKinds, STItemInfo>();

	private string ItemInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_ITEM_INFO;
#else
			return KCDefine.U_TABLE_P_G_ITEM_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		var oItemInfoList = new List<STItemInfo>(m_oGoodsItemInfoList);
		oItemInfoList.ExAddVals(m_oConsumableItemInfoList);
		oItemInfoList.ExAddVals(m_oNonConsumableItemInfoList);
		oItemInfoList.ExAddVals(m_oWeaponItemInfoList);
		oItemInfoList.ExAddVals(m_oArmorItemInfoList);
		oItemInfoList.ExAddVals(m_oAccessoryItemInfoList);
		oItemInfoList.ExAddVals(m_oAttachmentsItemInfoList);

		for(int i = 0; i < oItemInfoList.Count; ++i) {
			this.ItemInfoDict.TryAdd(oItemInfoList[i].m_eItemKinds, oItemInfoList[i]);
		}
	}

	/** 아이템 정보를 반환한다 */
	public STItemInfo GetItemInfo(EItemKinds a_EItemKinds) {
		bool bIsValid = this.TryGetItemInfo(a_EItemKinds, out STItemInfo stItemInfo);
		CAccess.Assert(bIsValid);

		return stItemInfo;
	}

	/** 아이템 정보를 반환한다 */
	public bool TryGetItemInfo(EItemKinds a_EItemKinds, out STItemInfo a_stOutItemInfo) {
		a_stOutItemInfo = this.ItemInfoDict.GetValueOrDefault(a_EItemKinds, default(STItemInfo));
		return this.ItemInfoDict.ContainsKey(a_EItemKinds);
	}

	/** 아이템 정보를 로드한다 */
	public Dictionary<EItemKinds, STItemInfo> LoadItemInfos() {
		return this.LoadItemInfos(this.ItemInfoTablePath);
	}

	/** 아이템 정보를 로드한다 */
	private Dictionary<EItemKinds, STItemInfo> LoadItemInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadItemInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadItemInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 아이템 정보를 로드한다 */
	private Dictionary<EItemKinds, STItemInfo> DoLoadItemInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSONNode.Parse(a_oJSONStr);

		var oItemInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_GOODS], oJSONNode[KCDefine.U_KEY_CONSUMABLE], oJSONNode[KCDefine.U_KEY_NON_CONSUMABLE], oJSONNode[KCDefine.U_KEY_WEAPON], oJSONNode[KCDefine.U_KEY_ARMOR], oJSONNode[KCDefine.U_KEY_ACCESSORY], oJSONNode[KCDefine.U_KEY_ATTACHMENTS]
		};

		for(int i = 0; i < oItemInfosList.Count; ++i) {
			for(int j = 0; j < oItemInfosList[i].Count; ++j) {
				var stItemInfo = new STItemInfo(oItemInfosList[i][j]);

				// 아이템 정보가 추가 가능 할 경우
				if(stItemInfo.m_eItemKinds.ExIsValid() && (!this.ItemInfoDict.ContainsKey(stItemInfo.m_eItemKinds) || oItemInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ItemInfoDict.ExReplaceVal(stItemInfo.m_eItemKinds, stItemInfo);
				}
			}
		}

		return this.ItemInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
