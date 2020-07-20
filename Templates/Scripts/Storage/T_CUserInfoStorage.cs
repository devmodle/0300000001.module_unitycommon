using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if !USE_CUSTOM_PROJECT_OPTION
#if MESSAGE_PACK_ENABLE
using MessagePack;

//! 유저 정보
[MessagePackObject]
[System.Serializable]
public class CUserInfo : CBaseInfo {
	#region 상수
	private const string KEY_IS_AGREE = "IsAgree";
	private const string KEY_IS_MUTE_BG_SND = "IsMuteBGSnd";
	private const string KEY_IS_MUTE_FX_SNDS = "IsMuteFXSnds";
	private const string KEY_IS_DISABLE_VIBRATE = "IsDisableVibrate";
	private const string KEY_IS_DISABLE_NOTIFICATION = "IsDisableNotification";
	private const string KEY_IS_ENABLE_TUTORIAL = "IsEnableTutorial";
	private const string KEY_IS_REMOVE_ADS = "IsRemoveAds";

	private const string KEY_USER_TYPE = "UserType";
	#endregion			// 상수

	#region 프로퍼티
	[IgnoreMember] public bool IsAgree {
		get { return m_oBoolList.ExGetValue(CUserInfo.KEY_IS_AGREE, false); } 
		set { m_oBoolList.ExReplaceValue(CUserInfo.KEY_IS_AGREE, value); }
	}

	[IgnoreMember] public bool IsMuteBGSnd {
		get { return m_oBoolList.ExGetValue(CUserInfo.KEY_IS_MUTE_BG_SND, false); } 
		set { m_oBoolList.ExReplaceValue(CUserInfo.KEY_IS_MUTE_BG_SND, value); }
	}

	[IgnoreMember] public bool IsMuteFXSnds {
		get { return m_oBoolList.ExGetValue(CUserInfo.KEY_IS_MUTE_FX_SNDS, false); } 
		set { m_oBoolList.ExReplaceValue(CUserInfo.KEY_IS_MUTE_FX_SNDS, value); }
	}

	[IgnoreMember] public bool IsDisableVibrate {
		get { return m_oBoolList.ExGetValue(CUserInfo.KEY_IS_DISABLE_VIBRATE, false); } 
		set { m_oBoolList.ExReplaceValue(CUserInfo.KEY_IS_DISABLE_VIBRATE, value); }
	}

	[IgnoreMember] public bool IsDisableNotification {
		get { return m_oBoolList.ExGetValue(CUserInfo.KEY_IS_DISABLE_NOTIFICATION, false); } 
		set { m_oBoolList.ExReplaceValue(CUserInfo.KEY_IS_DISABLE_NOTIFICATION, value); }
	}

	[IgnoreMember] public bool IsEnableTutorial {
		get { return m_oBoolList.ExGetValue(CUserInfo.KEY_IS_ENABLE_TUTORIAL, false); } 
		set { m_oBoolList.ExReplaceValue(CUserInfo.KEY_IS_ENABLE_TUTORIAL, value); }
	}

	[IgnoreMember] public bool IsRemoveAds {
		get { return m_oBoolList.ExGetValue(CUserInfo.KEY_IS_REMOVE_ADS, false); } 
		set { m_oBoolList.ExReplaceValue(CUserInfo.KEY_IS_REMOVE_ADS, value); }
	}

	[IgnoreMember] public EUserType UserType {
		get { return (EUserType)m_oIntList.ExGetValue(CUserInfo.KEY_USER_TYPE, (int)EUserType.NONE); } 
		set { m_oIntList.ExReplaceValue(CUserInfo.KEY_USER_TYPE, (int)value); }
	}
	#endregion			// 프로퍼티
	
	#region 함수
	//! 생성자
	public CUserInfo() : base(KAppDefine.G_VERSION_USER_INFO) {
		// Do Nothing
	}
	#endregion			// 함수
}

//! 유저 정보 저장소
public class CUserInfoStorage : CSingleton<CUserInfoStorage> {
	#region 프로퍼티
	public CUserInfo UserInfo { get; private set; } = null;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		this.Reset();
	}

	//! 상태를 리셋한다
	public virtual void Reset() {
		this.UserInfo = new CUserInfo();
		this.UserInfo.IsDisableVibrate = true;
		this.UserInfo.IsDisableNotification = true;
		this.UserInfo.IsEnableTutorial = true;
		
#if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		this.UserInfo.UserType = EUserType.NONE;
#else
		this.UserInfo.UserType = EUserType.USER_A;
#endif			// #if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	}

	//! 유저 정보를 저장한다
	public void SaveUserInfo(string a_oFilepath) {
		var oBytes = MessagePackSerializer.Serialize<CUserInfo>(this.UserInfo);

#if SECURITY_ENABLE
		CFunc.WriteSecurityBytes(a_oFilepath, oBytes);
#else
		CAccess.WriteBytes(a_oFilepath, oBytes);
#endif			// #if SECURITY_ENABLE
	}

	//! 유저 정보를 로드한다
	public void LoadUserInfo(string a_oFilepath) {
		if(File.Exists(a_oFilepath)) {
#if SECURITY_ENABLE
			var oBytes = CFunc.ReadSecurityBytes(a_oFilepath);
#else
			var oBytes = CAccess.ReadBytes(a_oFilepath);
#endif			// #if SECURITY_ENABLE

			try {
				this.UserInfo = MessagePackSerializer.Deserialize<CUserInfo>(oBytes);
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning("CUserInfoStorage.LoadUserInfo Exception: {0}", oException.Message);

				this.Reset();
				this.SaveUserInfo(a_oFilepath);
			}
		}
	}
	#endregion			// 함수
}
#endif			// #if MESSAGE_PACK_ENABLE
#endif			// #if !USE_CUSTOM_PROJECT_OPTION
