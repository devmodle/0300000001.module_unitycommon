using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if TENJIN_ENABLE
#if PURCHASE_ENABLE
using UnityEngine.Purchasing;
#endif			// #if PURCHASE_ENABLE

//! 텐진 관리자
public class CTenjinManager : CSingleton<CTenjinManager> {
	#region 변수
	private string m_oAPIKey = string.Empty;
	#endregion			// 변수

	#region 프로퍼티
	public bool IsInit { get; private set; } = false;

	private BaseTenjin TenjinInstance {
		get {
			var oInstance = Tenjin.getInstance(m_oAPIKey);
			Func.Assert(oInstance != null);

			return oInstance;
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public virtual void Init(string a_oAPIKey, System.Action<CTenjinManager, bool> a_oCallback) {
		Func.ShowLog("CTenjinManager.Init: {0}", Color.yellow, a_oAPIKey);

		if(!this.IsInit && Func.IsMobilePlatform()) {
			Func.Assert(a_oAPIKey.ExIsValid());

			this.IsInit = true;
			m_oAPIKey = a_oAPIKey;

			this.OnApplicationPause(false);
		}

		a_oCallback?.Invoke(this, this.IsInit);
	}

	//! 어플리케이션이 정지 되었을 경우
	public void OnApplicationPause(bool a_bIsPause) {
		if(this.IsInit && !a_bIsPause) {
#if MESSAGE_PACK_ENABLE
			if(CUserInfoStorage.Instance.UserInfo.IsAgree) {
				this.TenjinInstance.OptOut();
			} else {
				this.TenjinInstance.OptIn();
			}
#else
			this.TenjinInstance.OptIn();
#endif			// #if MESSAGE_PACK_ENABLE

			this.TenjinInstance.Connect();
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if TENJIN_ANALYTICS_ENABLE && PURCHASE_ENABLE
	//! 결제 로그를 전송한다
	public void SendPurchaseLog(Product a_oProduct) {
		Func.ShowLog("CTenjinManager.SendPurchaseLog: {0}", Color.yellow, a_oProduct);
		
#if ANALYTICS_TEST_ENABLE || (ADHOC_BUILD || STORE_BUILD)
		if(this.IsInit) {
			var oDataList = MiniJson.JsonDecode(a_oProduct.receipt) as Dictionary<string, object>;

			if(oDataList != null) {
				var oPayload = oDataList[KDefine.U_KEY_TENJIN_M_PAYLOAD] as string;
				double dblPrice = decimal.ToDouble(a_oProduct.metadata.localizedPrice);

#if UNITY_IOS
			this.TenjinInstance.Transaction(a_oProduct.definition.id,
				a_oProduct.metadata.isoCurrencyCode, 1, dblPrice, a_oProduct.transactionID, oPayload, null);
#elif UNITY_ANDROID
				var oAndroidDataList = MiniJson.JsonDecode(oPayload) as Dictionary<string, object>;

				var oReceipt = oAndroidDataList[KDefine.KEY_TENJIN_M_RECEIPT] as string;
				var oSignature = oAndroidDataList[KDefine.KEY_TENJIN_M_SIGNATURE] as string;

				this.TenjinInstance.Transaction(a_oProduct.definition.id,
					a_oProduct.metadata.isoCurrencyCode, 1, dblPrice, null, oReceipt, oSignature);
#endif			// #if UNITY_IOS
			}
		}
#endif			// #if ANALYTICS_TEST_ENABLE || (ADHOC_BUILD || STORE_BUILD)
	}
#endif			// #if TENJIN_ANALYTICS_ENABLE && PURCHASE_ENABLE
	#endregion			// 조건부 함수
}
#endif			// #if TENJIN_ENABLE
