using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#if PURCHASE_ENABLE
using UnityEngine.Purchasing;

//! 상품 정보
[System.Serializable]
public struct STProductInfo {
	public string m_oID;
	public ProductType m_eProductType;
}
#endif			// #if PURCHASE_ENABLE

//! 상품 정보 테이블
public class CProductInfoTable : CScriptableObject<CProductInfoTable> {
#if PURCHASE_ENABLE
	#region 변수
	[Header("Common Product Info")]
	[SerializeField] private List<STProductInfo> m_oCommonProductInfoList = new List<STProductInfo>();

	[Header("iOS Product Info")]
	[SerializeField] private List<STProductInfo> m_oiOSProductInfoList = new List<STProductInfo>();

	[Header("Android Product Info")]
	[SerializeField] private List<STProductInfo> m_oGoogleProductInfoList = new List<STProductInfo>();
	[SerializeField] private List<STProductInfo> m_oOneStoreProductInfoList = new List<STProductInfo>();
	[SerializeField] private List<STProductInfo> m_oGalaxyStoreProductInfoList = new List<STProductInfo>();
	#endregion			// 변수

	#region 프로퍼티
	public List<STProductInfo> ProductInfoList { get; private set; } = new List<STProductInfo>();

#if UNITY_EDITOR
	public List<STProductInfo> GoogleProductInfoList => m_oGoogleProductInfoList;
	public List<STProductInfo> OneStoreProductInfoList => m_oOneStoreProductInfoList;
	public List<STProductInfo> GalaxyStoreProductInfoList => m_oGalaxyStoreProductInfoList;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.ProductInfoList.ExAddValues(m_oCommonProductInfoList);

#if UNITY_IOS
		this.ProductInfoList.ExAddValues(m_oiOSProductInfoList);
#elif UNITY_ANDROID
#if ONE_STORE_PLATFORM
		this.ProductInfoList.ExAddValues(m_oOneStoreProductInfoList);
#elif GALAXY_STORE_PLATFORM
		this.ProductInfoList.ExAddValues(m_oGalaxyStoreProductInfoList);
#else
		this.ProductInfoList.ExAddValues(m_oGoogleProductInfoList);
#endif			// #if ONE_STORE_PLATFORM
#endif			// #if UNITY_IOS
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
	//! 공용 상품 정보를 변경한다
	public void SetCommonProductInfoList(List<STProductInfo> a_oProductInfoList) {
		Func.Assert(a_oProductInfoList != null);
		m_oCommonProductInfoList = a_oProductInfoList;
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
#endif			// #if PURCHASE_ENABLE
}
