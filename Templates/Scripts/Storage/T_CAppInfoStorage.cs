using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if !USE_CUSTOM_PROJECT_OPTION
#if MESSAGE_PACK_ENABLE
using MessagePack;

//! 어플리케이션 정보
[MessagePackObject]
[System.Serializable]
public class CAppInfo : CBaseInfo {
	#region 변수
	[IgnoreMember] public System.DateTime InstallTime { get; set; } = System.DateTime.Now;
	[IgnoreMember] public System.DateTime UTCInstallTime { get; set; } = System.DateTime.UtcNow;
	#endregion			// 변수
	
	#region 상수
	private const string KEY_DEVICE_ID = "DeviceID";
	private const string KEY_INSTALL_TIME = "InstallTime";
	#endregion			// 상수

	#region 프로퍼티
	[IgnoreMember] public string DeviceID {
		get { return m_oStringList.ExGetValue(CAppInfo.KEY_DEVICE_ID, string.Empty); } 
		set { m_oStringList.ExReplaceValue(CAppInfo.KEY_DEVICE_ID, value); }
	}

	[IgnoreMember] private string InstallTimeString => m_oStringList.ExGetValue(CAppInfo.KEY_INSTALL_TIME, string.Empty);
	#endregion			// 프로퍼티

	#region 인터페이스
	//! 직렬화 될 경우
	public override void OnBeforeSerialize() {
		m_oStringList.ExReplaceValue(CAppInfo.KEY_INSTALL_TIME, this.InstallTime.ExToLongString());
	}

	//! 역직렬화 되었을 경우
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();

		this.InstallTime = this.InstallTimeString.ExToTime(KCDefine.DATE_TIME_FORMAT_YYYY_MM_DD_HH_MM_SS);
		this.UTCInstallTime = this.InstallTime.ToUniversalTime();
	}
	#endregion			// 인터페이스

	#region 함수
	//! 생성자
	public CAppInfo() : base(KAppDefine.G_VERSION_APP_INFO) {
		// Do Nothing
	}
	#endregion			// 함수
}

//! 어플리케이션 정보 저장소
public class CAppInfoStorage : CSingleton<CAppInfoStorage> {
	#region 프로퍼티
	public bool IsLoadStoreVersion { get; private set; } = false;
	public bool IsValidStoreVersion { get; private set; } = false;

	public string CountryCode { get; set; } = string.Empty;
	public string PlatformName { get; private set; } = string.Empty;
	public string StoreVersion { get; private set; } = string.Empty;

	public CAppInfo AppInfo { get; private set; } = null;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.Reset();

#if UNITY_IOS
		this.PlatformName = KCDefine.PLATFORM_NAME_IOS;
#elif UNITY_ANDROID
#if ONE_STORE_PLATFORM
		this.PlatformName = KCDefine.PLATFORM_NAME_STORE;
#elif GALAXY_STORE_PLATFORM
		this.PlatformName = KCDefine.PLATFORM_NAME_GALAXY_STORE;
#else
		this.PlatformName = KCDefine.PLATFORM_NAME_GOOGLE;
#endif			// #if ONE_STORE_PLATFORM
#else
#if UNITY_STANDALONE_WIN
		this.PlatformName = KCDefine.PLATFORM_NAME_WINDOWS;
#else
		this.PlatformName = KCDefine.PLATFORM_NAME_MAC;
#endif			// #if UNITY_STANDALONE_WIN
#endif			// #if UNITY_IOS
	}

	//! 상태를 리셋한다
	public virtual void Reset() {
		this.AppInfo = new CAppInfo();
	}

	//! 약관 동의 필요 여부를 검사한다
	public static bool IsNeedAgreement(string a_oCountryCode) {
		string oCountryCode = a_oCountryCode.ToUpper();
		return oCountryCode.ExIsEuropeanUnion() || oCountryCode.ExIsEquals(KCDefine.KOREA_COUNTRY_CODE);
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdate(string a_oLatestVersion) {
#if UNITY_ANDROID
		return CFunc.IsNeedUpdateByBuildNumber(a_oLatestVersion);
#else
		return CFunc.IsNeedUpdateByBuildVersion(a_oLatestVersion);
#endif			// #if UNITY_ANDROID
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdate(string a_oLatestBuildNumber, string a_oLatestBuildVersion) {
		bool bIsNeedUpdate = CFunc.IsNeedUpdateByBuildNumber(a_oLatestBuildNumber);
		return bIsNeedUpdate || CFunc.IsNeedUpdateByBuildVersion(a_oLatestBuildVersion);
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdateByBuildNumber(string a_oLatestNumber) {
		CAccess.Assert(a_oLatestNumber.ExIsValid());

		bool bIsValidNumberA = int.TryParse(a_oLatestNumber, out int nBuildNumberA);
		bool bIsValidNumberB = int.TryParse(CProjectInfoTable.Instance.ProjectInfo.m_oBuildNumber, out int nBuildNumberB);

		CAccess.Assert(bIsValidNumberA && bIsValidNumberB);
		return nBuildNumberA > nBuildNumberB;
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdateByBuildVersion(string a_oLatestVersion) {
		CAccess.Assert(a_oLatestVersion.ExIsValid());

		var bIsValidVersionA = System.Version.TryParse(a_oLatestVersion, out System.Version oVersionA);
		var bIsValidVersionB = System.Version.TryParse(CProjectInfoTable.Instance.ProjectInfo.m_oBuildVersion, out System.Version oVersionB);

		CAccess.Assert(bIsValidVersionA && bIsValidVersionB);
		return oVersionA.CompareTo(oVersionB) >= KCDefine.COMPARE_RESULT_GREATE;
	}

	//! 디바이스 메세지를 수신했을 경우
	public void OnReceiveDeviceMsg(string a_oCmd, string a_oMsg) {
		CAccess.Assert(!CAccess.IsMobilePlatform() || a_oMsg.ExIsValid());

		if(CAccess.IsMobilePlatform()) {
			var oDataList = a_oMsg.ExJSONStringToObj<Dictionary<string, string>>();
			this.StoreVersion = oDataList[KCDefine.KEY_DEVICE_MR_VERSION];

			this.IsLoadStoreVersion = true;
			this.IsValidStoreVersion = bool.Parse(oDataList[KCDefine.KEY_DEVICE_MR_RESULT]);
		}
	}

	//! 스토어 버전을 설정한다
	public void SetupStoreVersion() {
#if UNITY_ANDROID
		string oVersion = CProjectInfoTable.Instance.ProjectInfo.m_oBuildNumber;
#else
		string oVersion = CProjectInfoTable.Instance.ProjectInfo.m_oBuildVersion;
#endif			// #if UNITY_ANDROID

		CUnityMsgSender.Instance.SendGetStoreVersionMessage(CProjectInfoTable.Instance.ProjectInfo.m_oAppID,
			oVersion, KCDefine.DEF_TIMEOUT_NETWORK_CONNECTION, this.OnReceiveDeviceMsg);
	}

	//! 어플리케이션 정보를 저장한다
	public void SaveAppInfo(string a_oFilepath) {
		var oBytes = MessagePackSerializer.Serialize<CAppInfo>(this.AppInfo);

#if SECURITY_ENABLE
		CFunc.WriteSecurityBytes(a_oFilepath, oBytes);
#else
		CAccess.WriteBytes(a_oFilepath, oBytes);
#endif			// #if SECURITY_ENABLE
	}

	//! 어플리케이션 정보를 로드한다
	public void LoadAppInfo(string a_oFilepath) {
		if(File.Exists(a_oFilepath)) {
#if SECURITY_ENABLE
			var oBytes = CFunc.ReadSecurityBytes(a_oFilepath);
#else
			var oBytes = CAccess.ReadBytes(a_oFilepath);
#endif			// #if SECURITY_ENABLE

			try {
				this.AppInfo = MessagePackSerializer.Deserialize<CAppInfo>(oBytes);
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning("CAppInfoStorage.LoadAppInfo Exception: {0}", oException.Message);

				this.Reset();
				this.SaveAppInfo(a_oFilepath);
			}
		}
	}
	#endregion			// 함수
}
#endif			// #if MESSAGE_PACK_ENABLE
#endif			// #if !USE_CUSTOM_PROJECT_OPTION
