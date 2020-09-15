using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 기본 함수
public static partial class Func {
	#region 클래스 함수
	//! 지역화 문자열을 설정한다
	public static void SetupLocalizeStrings() {
		string oFilepath = string.Empty;

		// 언어가 유효 할 경우
		if(Application.systemLanguage.ExIsValid()) {
			oFilepath = CFunc.MakeLocalizeFilepath(KCDefine.U_TABLE_PATH_G_LOCALIZE_COMMON_STRING,
				Application.systemLanguage.ToString());
		} else {
#if MSG_PACK_ENABLE
			oFilepath = CFunc.MakeLocalizeFilepath(KCDefine.U_TABLE_PATH_G_LOCALIZE_COMMON_STRING,
				CCommonAppInfoStorage.Instance.CountryCode);
#endif			// #if MSG_PACK_ENABLE
		}

		oFilepath = CAccess.IsExistsRes<TextAsset>(oFilepath) ? oFilepath
			: KCDefine.U_TABLE_PATH_G_ENGLISH_COMMON_STRING;

		CStringTable.Instance.LoadStringsFromRes(oFilepath);
	}
	#endregion			// 클래스 함수

	#region 조건부 클래스 함수
#if ADS_MODULE_ENABLE && MSG_PACK_ENABLE
	//! 보상 광고를 출력한다
	public static void ShowRewardAds(EAdsType a_eAdsType, 
		System.Action<CAdsManager, STAdsRewardInfo, bool> a_oCallback, System.Action<CAdsManager> a_oCloseCallback = null) 
	{
		// 보상 광고 출력이 가능 할 경우
		if(CAdsManager.Instance.IsLoadRewardAds(a_eAdsType)) {
			bool bIsWatchAds = false;
			STAdsRewardInfo stRewardInfo = default(STAdsRewardInfo);

			CAdsManager.Instance.ShowRewardAds(a_eAdsType, (a_oSender, a_stRewardInfo, a_bIsSuccess) => {
				bIsWatchAds = a_bIsSuccess;
				stRewardInfo = a_stRewardInfo;
			}, (a_oSender) => {
				a_oCallback?.Invoke(a_oSender, stRewardInfo, bIsWatchAds);
				a_oCloseCallback?.Invoke(a_oSender);
			});
		} else {
			a_oCallback?.Invoke(CAdsManager.Instance, default(STAdsRewardInfo), false);
		}
	}

	//! 전면 광고를 출력한다
	public static void ShowFullscreenAds(EAdsType a_eAdsType, 
		System.Action<CAdsManager, bool> a_oCallback, System.Action<CAdsManager> a_oCloseCallback = null) 
	{
		var stDeltaTime = System.DateTime.Now - CGameInfoStorage.Instance.PrevAdsTime;
		float fDelay = CValueTable.Instance.GetFloat(KCDefine.VT_KEY_DELAY_COMMON_GIS_ADS);

		bool bIsEnable = stDeltaTime.TotalSeconds.ExIsGreateEquals(fDelay);

		// 전면 광고 출력이 가능 할 경우
		if(bIsEnable && CAdsManager.Instance.IsLoadFullscreenAds(a_eAdsType)) {
			CGameInfoStorage.Instance.PrevAdsTime = System.DateTime.Now;
			CAdsManager.Instance.ShowFullscreenAds(a_eAdsType, a_oCallback, a_oCloseCallback);
		} else {
			a_oCallback?.Invoke(CAdsManager.Instance, false);
		}
	}
#endif			// #if ADS_MODULE_ENABLE && MSG_PACK_ENABLE
	#endregion			// 조건부 클래스 함수
}
#endif			// #if NEVER_USE_THIS
