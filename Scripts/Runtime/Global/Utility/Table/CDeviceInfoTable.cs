using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 디바이스 정보 테이블
public class CDeviceInfoTable : CScriptableObj<CDeviceInfoTable> {
	#region 변수
#if ADS_ENABLE && (ADMOB_ENABLE || APP_LOVIN_ENABLE)
	[Header("Admob Device Info")]
	[SerializeField] private List<string> m_oiOSAdmobDeviceIDList = new List<string>();
	[SerializeField] private List<string> m_oAndroidAdmobDeviceIDList = new List<string>();
#endif			// #if ADS_ENABLE && (ADMOB_ENABLE || APP_LOVIN_ENABLE)
	#endregion			// 변수

	#region 프로퍼티
	public List<string> AdmobDeviceIDList { get; private set; } = new List<string>();

#if UNITY_EDITOR
#if ADS_ENABLE && (ADMOB_ENABLE || APP_LOVIN_ENABLE)
	public List<string> iOSAdmobDeviceIDList => m_oiOSAdmobDeviceIDList;
	public List<string> AndroidAdmobDeviceIDList => m_oAndroidAdmobDeviceIDList;
#endif			// #if ADS_ENABLE && (ADMOB_ENABLE || APP_LOVIN_ENABLE)
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

#if ADS_ENABLE && (ADMOB_ENABLE || APP_LOVIN_ENABLE)
#if UNITY_IOS
		this.AdmobDeviceIDList = m_oiOSAdmobDeviceIDList;
#elif UNITY_ANDROID
		this.AdmobDeviceIDList = m_oAndroidAdmobDeviceIDList;
#endif			// #if UNITY_IOS
#endif			// #if ADS_ENABLE && (ADMOB_ENABLE || APP_LOVIN_ENABLE)
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
#if ADS_ENABLE && (ADMOB_ENABLE || APP_LOVIN_ENABLE)
	//! iOS 애드몹 디바이스 아이디 리스트를 변경한다
	public void SetiOSAdmobDeviceIDList(List<string> a_oDeviceIDList) {
		m_oiOSAdmobDeviceIDList = a_oDeviceIDList;
	}

	//! 안드로이드 애드몹 디바이스 아이디 리스트를 변경한다
	public void SetAndroidAdmobDeviceIDList(List<string> a_oDeviceIDList) {
		m_oAndroidAdmobDeviceIDList = a_oDeviceIDList;
	}
#endif			// #if ADS_ENABLE && (ADMOB_ENABLE || APP_LOVIN_ENABLE)
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
