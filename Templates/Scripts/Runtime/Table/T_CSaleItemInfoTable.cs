using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 판매 아이템 정보
[System.Serializable]
public struct STSaleItemInfo {
	public string m_oName;
	public string m_oDesc;

	public int m_nPrice;

	public ESaleItemType m_eSaleItemType;
	public ESaleItemKinds m_eSaleItemKinds;

	public EPriceType m_ePriceType;
	public EPriceKinds m_ePriceKinds;

	public STItemInfo m_stItemInfo;

	#region 함수
	//! 생성자
	public STSaleItemInfo(SimpleJSON.JSONNode a_oSaleItemInfo) {
		m_oName = a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_NAME];
		m_oDesc = a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_DESC];

		m_nPrice = a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_PRICE].AsInt;

		m_eSaleItemType = (ESaleItemType)a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_SALE_ITEM_TYPE].AsInt;
		m_eSaleItemKinds = (ESaleItemKinds)a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_SALE_ITEM_KINDS].AsInt;

		m_ePriceType = (EPriceType)a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_PRICE_TYPE].AsInt;
		m_ePriceKinds = (EPriceKinds)a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_PRICE_KINDS].AsInt;

		m_stItemInfo = new STItemInfo() {
			m_nNumItems = a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_NUM_ITEMS].AsInt,
			m_eItemKinds = (EItemKinds)a_oSaleItemInfo[KDefine.G_KEY_SALE_IIT_ITEM_KINDS].AsInt
		};
	}
	#endregion			// 함수
}

//! 판매 아이템 정보 테이블
public class CSaleItemInfoTable : CScriptableObj<CSaleItemInfoTable> {
	#region 변수
	[Header("Sale Item Info")]
	[SerializeField] private List<STSaleItemInfo> m_oSaleItemInfoList = new List<STSaleItemInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public Dictionary<ESaleItemKinds, STSaleItemInfo> SaleItemInfoList { get; private set; } = new Dictionary<ESaleItemKinds, STSaleItemInfo>();
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.SetupSaleItemInfos(m_oSaleItemInfoList, this.SaleItemInfoList);
	}

	//! 판매 아이템 정보를 반환한다
	public STSaleItemInfo GetSaleItemInfo(ESaleItemKinds a_eSaleItemKinds) {
		bool bIsValid = this.TryGetSaleItemInfo(a_eSaleItemKinds, out STSaleItemInfo stSaleItemInfo);
		CAccess.Assert(bIsValid);

		return stSaleItemInfo;
	}
	
	//! 판매 아이템 정보를 반환한다
	public bool TryGetSaleItemInfo(ESaleItemKinds a_eSaleItemKinds, out STSaleItemInfo a_stOutSaleItemInfo) {
		a_stOutSaleItemInfo = this.SaleItemInfoList.ExGetVal(a_eSaleItemKinds, KDefine.G_INVALID_SALE_ITEM_INFO);
		return this.SaleItemInfoList.ContainsKey(a_eSaleItemKinds);
	}

	//! 판매 아이템 정보를 로드한다
	public Dictionary<ESaleItemKinds, STSaleItemInfo> LoadSaleItemInfos() {
#if UNITY_EDITOR || UNITY_STANDALONE
		return this.LoadSaleItemInfos(KDefine.G_RUNTIME_TABLE_P_SALE_ITEM_INFO);
#else
		return this.LoadSaleItemInfos(KCDefine.U_TABLE_P_G_SALE_ITEM_INFO);
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 판매 아이템 정보를 로드한다
	public Dictionary<ESaleItemKinds, STSaleItemInfo> LoadSaleItemInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if UNITY_EDITOR || UNITY_STANDALONE
		string oJSONStr = CFunc.ReadStr(a_oFilePath);
		return this.DoLoadSaleItemInfos(oJSONStr);
#else
		try {
			var oTextAsset = CResManager.Inst.GetRes<TextAsset>(a_oFilePath);
			return this.DoLoadSaleItemInfos(oTextAsset.text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
	}

	//! 판매 아이템 정보를 설정한다
	private void SetupSaleItemInfos(List<STSaleItemInfo> a_oSaleItemInfoList, Dictionary<ESaleItemKinds, STSaleItemInfo> a_oOutSaleItemInfoList) {
		CAccess.Assert(a_oSaleItemInfoList != null && a_oOutSaleItemInfoList != null);

		for(int i = 0; i < a_oSaleItemInfoList.Count; ++i) {
			var stSaleItemInfo = a_oSaleItemInfoList[i];
			a_oOutSaleItemInfoList.ExAddVal(stSaleItemInfo.m_eSaleItemKinds, stSaleItemInfo);
		}
	}

	//! 판매 아이템 정보를 로드한다
	private Dictionary<ESaleItemKinds, STSaleItemInfo> DoLoadSaleItemInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());

		var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr) as SimpleJSON.JSONClass;
		var oSaleItemInfos = oJSONNode[KCDefine.B_KEY_JSON_COMMON_DATA];

		for(int i = 0; i < oSaleItemInfos.Count; ++i) {
			var stSaleItemInfo = new STSaleItemInfo(oSaleItemInfos[i]);
			bool bIsReplace = oSaleItemInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT;

			// 판매 아이템 정보가 추가 가능 할 경우
			if(bIsReplace || !this.SaleItemInfoList.ContainsKey(stSaleItemInfo.m_eSaleItemKinds)) {
				this.SaleItemInfoList.ExReplaceVal(stSaleItemInfo.m_eSaleItemKinds, stSaleItemInfo);
			}
		}

#if UNITY_EDITOR
		this.SetupSaleItemInfoList(this.SaleItemInfoList, m_oSaleItemInfoList);
#endif			// #if UNITY_EDITOR

		return this.SaleItemInfoList;
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
	// 판매 아이템 정보를 설정한다
	private void SetupSaleItemInfoList(Dictionary<ESaleItemKinds, STSaleItemInfo> a_oSaleItemInfoList, List<STSaleItemInfo> a_oOutSaleItemInfoList) {
		CAccess.Assert(a_oSaleItemInfoList != null && a_oOutSaleItemInfoList != null);
		a_oOutSaleItemInfoList.Clear();

		foreach(var stKeyVal in a_oSaleItemInfoList) {
			a_oOutSaleItemInfoList.ExAddVal(stKeyVal.Value);
		}

		a_oOutSaleItemInfoList.Sort((a_stLhs, a_stRhs) => (int)a_stLhs.m_eSaleItemKinds - (int)a_stRhs.m_eSaleItemKinds);
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
#endif			// #if NEVER_USE_THIS
