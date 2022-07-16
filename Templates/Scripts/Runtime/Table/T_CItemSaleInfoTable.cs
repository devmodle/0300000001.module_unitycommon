using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 아이템 판매 정보 */
[System.Serializable]
public partial struct STItemSaleInfo {
	public STCommonInfo m_stCommonInfo;

	public EItemKinds m_eItemKinds;
	public EItemKinds m_ePrevItemKinds;
	public EItemKinds m_eNextItemKinds;
	
	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STItemSaleInfo INVALID = new STItemSaleInfo() {
		m_eItemKinds = EItemKinds.NONE, m_ePrevItemKinds = EItemKinds.NONE, m_eNextItemKinds = EItemKinds.NONE
	};
	#endregion			// 상수

	#region 프로퍼티
	public EItemType ItemType => (EItemType)((int)m_eItemKinds).ExKindsToType();
	public EItemKinds BaseItemKinds => (EItemKinds)((int)m_eItemKinds).ExKindsToSubKindsType();
	#endregion			// 프로퍼티

	#region 함수
	/** 생성자 */
	public STItemSaleInfo(SimpleJSON.JSONNode a_oItemSaleInfo) {
		m_stCommonInfo = new STCommonInfo(a_oItemSaleInfo);

		m_eItemKinds = a_oItemSaleInfo[KCDefine.U_KEY_ITEM_SALE_KINDS].ExIsValid() ? (EItemKinds)a_oItemSaleInfo[KCDefine.U_KEY_ITEM_SALE_KINDS].AsInt : EItemKinds.NONE;
		m_ePrevItemKinds = a_oItemSaleInfo[KCDefine.U_KEY_PREV_ITEM_SALE_KINDS].ExIsValid() ? (EItemKinds)a_oItemSaleInfo[KCDefine.U_KEY_PREV_ITEM_SALE_KINDS].AsInt : EItemKinds.NONE;
		m_eNextItemKinds = a_oItemSaleInfo[KCDefine.U_KEY_NEXT_ITEM_SALE_KINDS].ExIsValid() ? (EItemKinds)a_oItemSaleInfo[KCDefine.U_KEY_NEXT_ITEM_SALE_KINDS].AsInt : EItemKinds.NONE;

		m_oPayTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		m_oAcquireTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		for(int i = 0; i < KDefine.G_MAX_NUM_TARGET_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemSaleInfo[string.Format(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oPayTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}

		for(int i = 0; i < KDefine.G_MAX_NUM_ABILITY_VAL_INFOS; ++i) {
			var stTargetInfo = new STTargetInfo(a_oItemSaleInfo[string.Format(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, i + KCDefine.B_VAL_1_INT)]);
			m_oAcquireTargetInfoDict.TryAdd(Factory.MakeUniqueTargetInfoID(stTargetInfo.m_eTargetKinds, stTargetInfo.m_nKinds), stTargetInfo);
		}
	}
	#endregion			// 함수
}

/** 아이템 판매 정보 테이블 */
public partial class CItemSaleInfoTable : CScriptableObj<CItemSaleInfoTable> {
	#region 변수
	[Header("=====> Goods Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oGoodsItemSaleInfoList = new List<STItemSaleInfo>();

	[Header("=====> Consumable Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oConsumableItemSaleInfoList = new List<STItemSaleInfo>();

	[Header("=====> Non Consumable Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oNonConsumableItemSaleInfoList = new List<STItemSaleInfo>();

	[Header("=====> Weapon Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oWeaponItemSaleInfoList = new List<STItemSaleInfo>();

	[Header("=====> Armor Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oArmorItemSaleInfoList = new List<STItemSaleInfo>();

	[Header("=====> Accessory Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oAccessoryItemSaleInfoList = new List<STItemSaleInfo>();

	[Header("=====> Attachments Item Sale Info <=====")]
	[SerializeField] private List<STItemSaleInfo> m_oAttachmentsItemSaleInfoList = new List<STItemSaleInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<EItemKinds, STItemSaleInfo> ItemSaleInfoDict { get; private set; } = new Dictionary<EItemKinds, STItemSaleInfo>();

	private string ItemSaleInfoTablePath {
		get {
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_ITEM_SALE_INFO;
#else
			return KCDefine.U_TABLE_P_G_ITEM_SALE_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetItemSaleInfos();
	}

	/** 아이템 판매 정보를 리셋한다 */
	public void ResetItemSaleInfos() {
		this.ItemSaleInfoDict.Clear();

		var oItemSaleInfoList = new List<STItemSaleInfo>(m_oGoodsItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oConsumableItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oNonConsumableItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oWeaponItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oArmorItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oAccessoryItemSaleInfoList);
		oItemSaleInfoList.ExAddVals(m_oAttachmentsItemSaleInfoList);

		for(int i = 0; i < oItemSaleInfoList.Count; ++i) {
			this.ItemSaleInfoDict.TryAdd(oItemSaleInfoList[i].m_eItemKinds, oItemSaleInfoList[i]);
		}
	}

	/** 아이템 판매 정보를 리셋한다 */
	public void ResetItemSaleInfos(string a_oJSONStr) {
		this.ResetItemSaleInfos();
		this.DoLoadItemSaleInfos(a_oJSONStr);
	}

	/** 아이템 판매 정보를 반환한다 */
	public STItemSaleInfo GetItemSaleInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetItemSaleInfo(a_eItemKinds, out STItemSaleInfo stItemSaleInfo);
		CAccess.Assert(bIsValid);

		return stItemSaleInfo;
	}

	/** 지불 타겟 정보를 반환한다 */
	public STTargetInfo GetPayTargetInfo(EItemKinds a_eItemKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetPayTargetInfo(a_eItemKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stPayTargetInfo);
		CAccess.Assert(bIsValid);

		return stPayTargetInfo;
	}

	/** 획득 타겟 정보를 반환한다 */
	public STTargetInfo GetAcquireTargetInfo(EItemKinds a_eItemKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetAcquireTargetInfo(a_eItemKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stAcquireTargetInfo);
		CAccess.Assert(bIsValid);

		return stAcquireTargetInfo;
	}
	
	/** 아이템 판매 정보를 반환한다 */
	public bool TryGetItemSaleInfo(EItemKinds a_eItemKinds, out STItemSaleInfo a_stOutItemSaleInfo) {
		a_stOutItemSaleInfo = this.ItemSaleInfoDict.GetValueOrDefault(a_eItemKinds, STItemSaleInfo.INVALID);
		return this.ItemSaleInfoDict.ContainsKey(a_eItemKinds);
	}

	/** 지불 타겟 정보를 반환한다 */
	public bool TryGetPayTargetInfo(EItemKinds a_eItemKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutPayTargetInfo) {
		a_stOutPayTargetInfo = this.TryGetItemSaleInfo(a_eItemKinds, out STItemSaleInfo stItemSaleInfo) ? stItemSaleInfo.m_oPayTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID) : STTargetInfo.INVALID;
		return !a_stOutPayTargetInfo.Equals(STTargetInfo.INVALID);
	}

	/** 획득 타겟 정보를 반환한다 */
	public bool TryGetAcquireTargetInfo(EItemKinds a_eItemKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutAcquireTargetInfo) {
		a_stOutAcquireTargetInfo = this.TryGetItemSaleInfo(a_eItemKinds, out STItemSaleInfo stItemSaleInfo) ? stItemSaleInfo.m_oAcquireTargetInfoDict.GetValueOrDefault(Factory.MakeUniqueTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID) : STTargetInfo.INVALID;
		return !a_stOutAcquireTargetInfo.Equals(STTargetInfo.INVALID);
	}

	/** 아이템 판매 정보를 로드한다 */
	public Dictionary<EItemKinds, STItemSaleInfo> LoadItemSaleInfos() {
		this.ResetItemSaleInfos();
		return this.LoadItemSaleInfos(this.ItemSaleInfoTablePath);
	}

	/** 아이템 판매 정보를 로드한다 */
	private Dictionary<EItemKinds, STItemSaleInfo> LoadItemSaleInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
		return this.DoLoadItemSaleInfos(CFunc.ReadStr(a_oFilePath));
#else
		try {
			return this.DoLoadItemSaleInfos(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 아이템 판매 정보를 로드한다 */
	private Dictionary<EItemKinds, STItemSaleInfo> DoLoadItemSaleInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;

		var oItemSaleInfosList = new List<SimpleJSON.JSONNode>() {
			oJSONNode[KCDefine.U_KEY_GOODS], oJSONNode[KCDefine.U_KEY_CONSUMABLE], oJSONNode[KCDefine.U_KEY_NON_CONSUMABLE], oJSONNode[KCDefine.U_KEY_WEAPON], oJSONNode[KCDefine.U_KEY_ARMOR], oJSONNode[KCDefine.U_KEY_ACCESSORY]
		};

		for(int i = 0; i < oItemSaleInfosList.Count; ++i) {
			for(int j = 0; j < oItemSaleInfosList[i].Count; ++j) {
				var stItemSaleInfo = new STItemSaleInfo(oItemSaleInfosList[i][j]);

				// 아이템 판매 정보가 추가 가능 할 경우
				if(stItemSaleInfo.m_eItemKinds.ExIsValid() && (!this.ItemSaleInfoDict.ContainsKey(stItemSaleInfo.m_eItemKinds) || oItemSaleInfosList[i][j][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
					this.ItemSaleInfoDict.ExReplaceVal(stItemSaleInfo.m_eItemKinds, stItemSaleInfo);
				}
			}
		}

		return this.ItemSaleInfoDict;
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
