using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if SINGULAR_ENABLE
#if PURCHASE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_ENABLE

//! 싱귤러 관리자
public class CSingularManager : CSingleton<CSingularManager> {
	#region 컴포넌트
	private SingularSDK m_oSingular = null;
	#endregion			// 컴포넌트

	#region 프로퍼티
	public bool IsInit { get; private set; } = false;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public virtual void Init(System.Action<CSingularManager, bool> a_oCallback) {
		Func.ShowLog("CSingularManager.Init", KDefine.B_LOG_COLOR_PLUGIN);

		// 초기화가 필요 할 경우
		if(!this.IsInit && Func.IsMobilePlatform()) {
			m_oSingular = Func.CreateCloneObj<SingularSDK>(KDefine.U_OBJ_NAME_SINGULAR, 
				CResManager.Instance.GetPrefab(KDefine.U_OBJ_PATH_SINGULAR), this.gameObject);

			this.IsInit = true;
			SingularSDK.InitializeSingularSDK();
		}

		a_oCallback?.Invoke(this, this.IsInit);
	}
	#endregion			// 함수

	#region 조건부 함수
#if SINGULAR_ANALYTICS_ENABLE
	//! 분석 유저 식별자를 변경한다
	public void SetAnalyticsUserID(string a_oID) {
		Func.Assert(a_oID.ExIsValid());
		Func.ShowLog("CSingularManager.SetAnalyticsUserID: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oID);

		if(this.IsInit) {
			SingularSDK.SetCustomUserId(a_oID);
		}
	}

	//! 로그를 전송한다
	public void SendLog(string a_oName) {
		this.SendLog(a_oName, null);
	}

	//! 로그를 전송한다
	public void SendLog(string a_oName, string a_oParameter, List<string> a_oDataList) {
		Func.Assert(a_oParameter.ExIsValid());

		this.SendLog(a_oName, new Dictionary<string, object>() {
			[a_oParameter] = a_oDataList.ExToString(KDefine.U_TOKEN_SINGULAR_ANALYTICS_LOG_DATA)
		});
	}

	//! 로그를 전송한다
	public void SendLog(string a_oName, Dictionary<string, object> a_oDataList) {
		Func.Assert(a_oName.ExIsValid());
		Func.ShowLog("CSingularManager.SendLog: {0}, {1}", KDefine.B_LOG_COLOR_PLUGIN, a_oName, a_oDataList);

#if ANALYTICS_TEST_ENABLE || (ADHOC_BUILD || STORE_BUILD)
		if(this.IsInit) {
			var oDataList = a_oDataList ?? new Dictionary<string, object>();

#if MESSAGE_PACK_ENABLE
			oDataList.ExAddValue(KDefine.U_LOG_KEY_DEVICE_ID, CAppInfoStorage.Instance.AppInfo.DeviceID);

#if AUTO_LOG_PARAMETER_ENABLE
			oDataList.ExAddValue(KDefine.U_LOG_KEY_PLATFORM, CAppInfoStorage.Instance.PlatformName);
			oDataList.ExAddValue(KDefine.U_LOG_KEY_USER_TYPE, CUserInfoStorage.Instance.UserInfo.UserType.ToString());
			
			oDataList.ExAddValue(KDefine.U_LOG_KEY_LOG_TIME, System.DateTime.UtcNow.ExToLongString());
			oDataList.ExAddValue(KDefine.U_LOG_KEY_INSTALL_TIME, CAppInfoStorage.Instance.AppInfo.m_stUTCInstallTime.ExToLongString());
#endif			// #if AUTO_LOG_PARAMETER_ENABLE
#endif			// #if MESSAGE_PACK_ENABLE

			SingularSDK.Event(oDataList, a_oName);
		}
#endif			// #if ANALYTICS_TEST_ENABLE || (ADHOC_BUILD || STORE_BUILD)
	}

#if PURCHASE_ENABLE
	//! 결제 로그를 전송한다
	public void SendPurchaseLog(Product a_oProduct, bool a_bIsRestore = false) {
		Func.ShowLog("CSingularManager.SendPurchaseLog: {0}", KDefine.B_LOG_COLOR_PLUGIN, a_oProduct);

#if ANALYTICS_TEST_ENABLE || (ADHOC_BUILD || STORE_BUILD)
		if(this.IsInit) {
			SingularSDK.InAppPurchase(a_oProduct, null, a_bIsRestore);
		}
#endif			// #if ANALYTICS_TEST_ENABLE || (ADHOC_BUILD || STORE_BUILD)
	}
#endif			// #if PURCHASE_ENABLE
#endif			// #if SINGULAR_ANALYTICS_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if SINGULAR_ENABLE
