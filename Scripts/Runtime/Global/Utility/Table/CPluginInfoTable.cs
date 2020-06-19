using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ADS_ENABLE
#if ADMOB_ENABLE
//! 애드몹 플러그인 정보
[System.Serializable]
public struct STAdmobPluginInfo {
	public string m_oBannerAdsID;
	public string m_oRewardAdsID;
	public string m_oNativeAdsID;
	public string m_oFullscreenAdsID;

	public List<string> m_oTemplateIDList;
}
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
//! 유니티 애즈 플러그인 정보
[System.Serializable]
public struct STUnityAdsPluginInfo {
	public string m_oGameID;

	public string m_oBannerAdsPlacement;
	public string m_oRewardAdsPlacement;
	public string m_oFullscreenAdsPlacement;
}
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
//! 아이언 소스 플러그인 정보
[System.Serializable]
public struct STIronSourcePluginInfo {
	public string m_oAppKey;

	public string m_oBannerAdsPlacement;
	public string m_oRewardAdsPlacement;
	public string m_oFullscreenAdsPlacement;
}
#endif			// #if IRON_SOURCE_ENABLE
#endif			// #if ADS_ENABLE

#if TENJIN_ENABLE
//! 텐진 플러그인 정보
[System.Serializable]
public struct STTenjinPluginInfo {
	public string m_oAPIKey;
}
#endif			// #if TENJIN_ENABLE

#if FLURRY_ENABLE
//! 플러리 플러그인 정보
[System.Serializable]
public struct STFlurryPluginInfo {
	public string m_oAPIKey;
}
#endif			// #if FLURRY_ENABLE

#if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE
//! 파이어 베이스 플러그인 정보
[System.Serializable]
public struct STFirebasePluginInfo {
	public string m_oDatabaseURL;
}
#endif			// #if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE

//! 플러그인 정보 테이블
public class CPluginInfoTable : CScriptableObject<CPluginInfoTable> {
	#region 변수
#if ADS_ENABLE
	[Header("Ads Plugin Info")]
	public EAdsType m_eBannerAdsType;

#if ADMOB_ENABLE
	[SerializeField] private STAdmobPluginInfo m_stiOSAdmobPluginInfo;
	[SerializeField] private STAdmobPluginInfo m_stAndroidAdmobPluginInfo;
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
	[SerializeField] private STUnityAdsPluginInfo m_stiOSUnityAdsPluginInfo;
	[SerializeField] private STUnityAdsPluginInfo m_stAndroidUnityAdsPluginInfo;
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
	[SerializeField] private STIronSourcePluginInfo m_stiOSIronSourcePluginInfo;
	[SerializeField] private STIronSourcePluginInfo m_stAndroidIronSourcePluginInfo;
#endif			// #if IRON_SOURCE_ENABLE
#endif			// #if ADS_ENABLE

#if TENJIN_ENABLE
	[Header("Tenjin Plugin Info")]
	[SerializeField] private STTenjinPluginInfo m_stTenjinPluginInfo;
#endif			// #if TENJIN_ENABLE

#if FLURRY_ENABLE
	[Header("Flurry Plugin Info")]
	[SerializeField] private STFlurryPluginInfo m_stiOSFlurryPluginInfo;
	[SerializeField] private STFlurryPluginInfo m_stAndroidFlurryPluginInfo;
#endif			// #if FLURRY_ENABLE

#if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE
	[Header("Firebase Plugin Info")]
	[SerializeField] private STFirebasePluginInfo m_stFirebasePluginInfo;
#endif			// #if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE
	#endregion			// 변수

	#region 프로퍼티
#if ADS_ENABLE
#if ADMOB_ENABLE
	public STAdmobPluginInfo AdmobPluginInfo { get; private set; }
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
	public STUnityAdsPluginInfo UnityAdsPluginInfo { get; private set; }
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
	public STIronSourcePluginInfo IronSourcePluginInfo { get; private set; }
#endif			// #if IRON_SOURCE_ENABLE
#endif			// #if ADS_ENABLE

#if TENJIN_ENABLE
	public STTenjinPluginInfo TenjinPluginInfo { get; private set; }
#endif			// #if TENJIN_ENABLE

#if FLURRY_ENABLE
	public STFlurryPluginInfo FlurryPluginInfo { get; private set; }
#endif			// #if FLURRY_ENABLE

#if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE
	public STFirebasePluginInfo FirebasePluginInfo { get; private set; }
#endif			// #if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE

#if UNITY_EDITOR
#if ADS_ENABLE
#if ADMOB_ENABLE
	public STAdmobPluginInfo iOSAdmobPluginInfo => m_stiOSAdmobPluginInfo;
	public STAdmobPluginInfo AndroidAdmobPluginInfo => m_stAndroidAdmobPluginInfo;
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
	public STUnityAdsPluginInfo iOSUnityAdsPluginInfo => m_stiOSUnityAdsPluginInfo;
	public STUnityAdsPluginInfo AndroidUnityAdsPluginInfo => m_stAndroidUnityAdsPluginInfo;
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
	public STIronSourcePluginInfo iOSIronSourcePluginInfo => m_stiOSIronSourcePluginInfo;
	public STIronSourcePluginInfo AndroidIronSourcePluginInfo => m_stAndroidIronSourcePluginInfo;
#endif			// #if IRON_SOURCE_ENABLE
#endif			// #if ADS_ENABLE

#if FLURRY_ENABLE
	public STFlurryPluginInfo iOSFlurryPluginInfo => m_stiOSFlurryPluginInfo;
	public STFlurryPluginInfo AndroidFlurryPluginInfo => m_stAndroidFlurryPluginInfo;
#endif			// #if FLURRY_ENABLE
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

#if ADS_ENABLE
#if ADMOB_ENABLE
		STAdmobPluginInfo stAdmobPluginInfo;

#if UNITY_IOS
		stAdmobPluginInfo = m_stiOSAdmobPluginInfo;
#elif UNITY_ANDROID
		stAdmobPluginInfo = m_stAndroidAdmobPluginInfo;
#else
		stAdmobPluginInfo = new STAdmobPluginInfo() {
			m_oBannerAdsID = string.Empty,
			m_oRewardAdsID = string.Empty,
			m_oNativeAdsID = string.Empty,
			m_oFullscreenAdsID = string.Empty,

			m_oTemplateIDList = new List<string>()
		};
#endif			// #if UNITY_IOS

#if ADS_TEST_ENABLE
		stAdmobPluginInfo.m_oBannerAdsID = KDefine.U_TEST_ADS_ID_ADMOB_BANNER_ADS;
		stAdmobPluginInfo.m_oRewardAdsID = KDefine.U_TEST_ADS_ID_ADMOB_REWARD_ADS;
		stAdmobPluginInfo.m_oNativeAdsID = KDefine.U_TEST_ADS_ID_ADMOB_NATIVE_ADS;
		stAdmobPluginInfo.m_oFullscreenAdsID = KDefine.U_TEST_ADS_ID_ADMOB_FULLSCREEN_ADS;
#endif			// #if ADS_TEST_ENABLE

		this.AdmobPluginInfo = stAdmobPluginInfo;
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
#if UNITY_IOS
		this.UnityAdsPluginInfo = m_stiOSUnityAdsPluginInfo;
#elif UNITY_ANDROID
		this.UnityAdsPluginInfo = m_stAndroidUnityAdsPluginInfo;
#else
		this.UnityAdsPluginInfo = new STUnityAdsPluginInfo() {
			m_oGameID = string.Empty,

			m_oBannerAdsPlacement = string.Empty,
			m_oRewardAdsPlacement = string.Empty,
			m_oFullscreenAdsPlacement = string.Empty,
		};
#endif			// #if UNITY_IOS
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
#if UNITY_IOS
		this.IronSourcePluginInfo = m_stiOSIronSourcePluginInfo;
#elif UNITY_ANDROID
		this.IronSourcePluginInfo = m_stAndroidIronSourcePluginInfo;
#else
		this.IronSourcePluginInfo = new STIronSourcePluginInfo() {
			m_oAppKey = string.Empty,

			m_oBannerAdsPlacement = string.Empty,
			m_oRewardAdsPlacement = string.Empty,
			m_oFullscreenAdsPlacement = string.Empty,
		};
#endif			// #if UNITY_IOS
#endif			// #if IRON_SOURCE_ENABLE
#endif			// #if ADS_ENABLE

#if TENJIN_ENABLE
		this.TenjinPluginInfo = m_stTenjinPluginInfo;
#endif			// #if TENJIN_ENABLE

#if FLURRY_ENABLE
#if UNITY_IOS
		this.FlurryPluginInfo = m_stiOSFlurryPluginInfo;
#elif UNITY_ANDROID
		this.FlurryPluginInfo = m_stAndroidFlurryPluginInfo;
#else
		this.FlurryPluginInfo = new STFlurryPluginInfo() {
			m_oAPIKey = string.Empty
		};
#endif			// #if UNITY_IOS
#endif			// #if FLURRY_ENABLE

#if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE
		this.FirebasePluginInfo = new STFirebasePluginInfo() {
			m_oDatabaseURL = m_stFirebasePluginInfo.m_oDatabaseURL
		};
#endif			// #if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
#if ADS_ENABLE
#if ADMOB_ENABLE
	//! iOS 애드몹 플러그인 정보를 변경한다
	public void SetiOSAdmobPluginInfo(STAdmobPluginInfo a_stPluginInfo) {
		m_stiOSAdmobPluginInfo = a_stPluginInfo;
	}

	//! 안드로이드 애드몹 플러그인 정보를 변경한다
	public void SetAndroidAdmobPluginInfo(STAdmobPluginInfo a_stPluginInfo) {
		m_stAndroidAdmobPluginInfo = a_stPluginInfo;
	}
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
	//! iOS 유니티 애즈 플러그인 정보를 변경한다
	public void SetiOSUnityAdsPluginInfo(STUnityAdsPluginInfo a_stPluginInfo) {
		m_stiOSUnityAdsPluginInfo = a_stPluginInfo;
	}

	//! 안드로이드 유니티 애즈 플러그인 정보를 변경한다
	public void SetAndroidUnityAdsPluginInfo(STUnityAdsPluginInfo a_stPluginInfo) {
		m_stAndroidUnityAdsPluginInfo = a_stPluginInfo;
	}
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
	//! iOS 아이언 소스 플러그인 정보를 변경한다
	public void SetiOSIronSourcePluginInfo(STIronSourcePluginInfo a_stPluginInfo) {
		m_stiOSIronSourcePluginInfo = a_stPluginInfo;
	}

	//! 안드로이드 아이언 소스 플러그인 정보를 변경한다
	public void SetAndroidIronSourcePluginInfo(STIronSourcePluginInfo a_stPluginInfo) {
		m_stAndroidIronSourcePluginInfo = a_stPluginInfo;
	}
#endif			// #if IRON_SOURCE_ENABLE
#endif			// #if ADS_ENABLE

#if TENJIN_ENABLE
	//! 텐진 플러그인 정보를 변경한다
	public void SetTenjinPluginInfo(STTenjinPluginInfo a_stPluginInfo) {
		m_stTenjinPluginInfo = a_stPluginInfo;
	}
#endif			// #if TENJIN_ENABLE

#if FLURRY_ENABLE
	//! iOS 플러리 플러그인 정보를 변경한다
	public void SetiOSFlurryPluginInfo(STFlurryPluginInfo a_stPluginInfo) {
		m_stiOSFlurryPluginInfo = a_stPluginInfo;
	}

	//! 안드로이드 플러리 플러그인 정보를 변경한다
	public void SetAndroidFlurryPluginInfo(STFlurryPluginInfo a_stPluginInfo) {
		m_stAndroidFlurryPluginInfo = a_stPluginInfo;
	}
#endif			// #if FLURRY_ENABLE

#if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE
	//! 파이어 베이스 플러그인 정보를 변경한다
	public void SetFirebasePluginInfo(STFirebasePluginInfo a_stPluginInfo) {
		m_stFirebasePluginInfo = a_stPluginInfo;
	}
#endif			// #if FIREBASE_ENABLE && FIREBASE_DATABASE_ENABLE
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
