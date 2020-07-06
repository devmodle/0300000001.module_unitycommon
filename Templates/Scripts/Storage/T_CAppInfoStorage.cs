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

		this.InstallTime = this.InstallTimeString.ExToTime(KDefine.B_DATE_TIME_FORMAT_YYYY_MM_DD_HH_MM_SS);
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
		this.PlatformName = KDefine.B_PLATFORM_NAME_IOS;
#elif UNITY_ANDROID
#if ONE_STORE_PLATFORM
		this.PlatformName = KDefine.B_PLATFORM_NAME_STORE;
#elif GALAXY_STORE_PLATFORM
		this.PlatformName = KDefine.B_PLATFORM_NAME_GALAXY_STORE;
#else
		this.PlatformName = KDefine.B_PLATFORM_NAME_GOOGLE;
#endif			// #if ONE_STORE_PLATFORM
#else
#if UNITY_STANDALONE_WIN
		this.PlatformName = KDefine.B_PLATFORM_NAME_WINDOWS;
#else
		this.PlatformName = KDefine.B_PLATFORM_NAME_MAC;
#endif			// #if UNITY_STANDALONE_WIN
#endif			// #if UNITY_IOS
	}

	//! 상태를 리셋한다
	public virtual void Reset() {
		this.AppInfo = new CAppInfo();
	}

	//! 디바이스 메세지를 수신했을 경우
	public void OnReceiveDeviceMsg(string a_oCmd, string a_oMsg) {
		Func.Assert(!Func.IsMobilePlatform() || a_oMsg.ExIsValid());

		if(Func.IsMobilePlatform()) {
			var oDataList = a_oMsg.ExJSONStringToObject<Dictionary<string, string>>();
			this.StoreVersion = oDataList[KDefine.U_KEY_DEVICE_MR_VERSION];

			this.IsLoadStoreVersion = true;
			this.IsValidStoreVersion = bool.Parse(oDataList[KDefine.U_KEY_DEVICE_MR_RESULT]);
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
			oVersion, KDefine.U_DEF_TIMEOUT_NETWORK_CONNECTION, this.OnReceiveDeviceMsg);
	}

	//! 어플리케이션 정보를 저장한다
	public void SaveAppInfo(string a_oFilepath) {
		var oBytes = MessagePackSerializer.Serialize<CAppInfo>(this.AppInfo);

#if SECURITY_ENABLE
		Func.WriteSecurityBytes(a_oFilepath, oBytes);
#else
		Func.WriteBytes(a_oFilepath, oBytes);
#endif			// #if SECURITY_ENABLE
	}

	//! 어플리케이션 정보를 로드한다
	public void LoadAppInfo(string a_oFilepath) {
		if(File.Exists(a_oFilepath)) {
#if SECURITY_ENABLE
			var oBytes = Func.ReadSecurityBytes(a_oFilepath);
#else
			var oBytes = Func.ReadBytes(a_oFilepath);
#endif			// #if SECURITY_ENABLE

			try {
				this.AppInfo = MessagePackSerializer.Deserialize<CAppInfo>(oBytes);
			} catch(System.Exception oException) {
				Func.ShowLog("CAppInfoStorage.LoadAppInfo Exception: {0}", oException);

				this.Reset();
				this.SaveAppInfo(a_oFilepath);
			}
		}
	}
	#endregion			// 함수
}
#endif			// #if MESSAGE_PACK_ENABLE
#endif			// #if !USE_CUSTOM_PROJECT_OPTION
