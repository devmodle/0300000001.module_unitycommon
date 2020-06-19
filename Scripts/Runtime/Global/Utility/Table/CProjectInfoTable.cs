using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 프로젝트 정보
[System.Serializable]
public struct STProjectInfo {
	public string m_oAppID;

	public string m_oBuildNumber;
	public string m_oBuildVersion;

	public string m_oStoreURL;
	public string m_oSupportMail;
}

//! 프로젝터 정보 테이블
public class CProjectInfoTable : CScriptableObject<CProjectInfoTable> {
	#region 변수
	[Header("Common Project Info")]
	[SerializeField] private string m_oCompanyName = string.Empty;
	[SerializeField] private string m_oProjectName = string.Empty;
	[SerializeField] private string m_oProductName = string.Empty;

	[Header("Standalone Project Info")]
	[SerializeField] private STProjectInfo m_stMacProjectInfo;
	[SerializeField] private STProjectInfo m_stWindowsProjectInfo;

	[Header("iOS Project Info")]
	[SerializeField] private STProjectInfo m_stiOSProjectInfo;

	[Header("Android Project Info")]
	[SerializeField] private STProjectInfo m_stGoogleProjectInfo;
	[SerializeField] private STProjectInfo m_stOneStoreProjectInfo;
	[SerializeField] private STProjectInfo m_stGalaxyStoreProjectInfo;
	#endregion			// 변수

	#region 프로퍼티
	public STProjectInfo ProjectInfo { get; private set; }

	public string CompanyName => m_oCompanyName;
	public string ProjectName => m_oProjectName;
	public string ProductName => m_oProductName;

	public STProjectInfo MacProjectInfo => m_stMacProjectInfo;
	public STProjectInfo WindowsProjectInfo => m_stWindowsProjectInfo;

	public STProjectInfo iOSProjectInfo => m_stiOSProjectInfo;

	public STProjectInfo GoogleProjectInfo => m_stGoogleProjectInfo;
	public STProjectInfo OneStoreProjectInfo => m_stOneStoreProjectInfo;
	public STProjectInfo GalaxyStoreProjectInfo => m_stGalaxyStoreProjectInfo;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

#if UNITY_IOS
		this.ProjectInfo = m_stiOSProjectInfo;
#elif UNITY_ANDROID
#if ONE_STORE_PLATFORM
		this.ProjectInfo = m_stOneStoreProjectInfo;
#elif GALAXY_STORE_PLATFORM
		this.ProjectInfo = m_stGalaxyStoreProjectInfo;
#else
		this.ProjectInfo = m_stGoogleProjectInfo;
#endif			// #if ONE_STORE_PLATFORM
#else
#if UNITY_STANDALONE_WIN
		this.ProjectInfo = m_stWindowsProjectInfo;
#else
		this.ProjectInfo = m_stMacProjectInfo;
#endif			// #if UNITY_STANDALONE_WIN
#endif			// #if UNITY_IOS
	}
	#endregion			// 함수

	#region 조건부 함수
	//! 회사 이름을 변경한다
	public void SetCompanyName(string a_oName) {
		m_oCompanyName = a_oName;
	}

	//! 프로젝트 이름을 변경한다
	public void SetProjectName(string a_oProjectName) {
		m_oProjectName = a_oProjectName;
	}

	//! 제품 이름을 변경한다
	public void SetProductName(string a_oName) {
		m_oProductName = a_oName;
	}

	//! 맥 프로젝트 정보를 변경한다
	public void SetMacProjectInfo(STProjectInfo a_stProjectInfo) {
		m_stMacProjectInfo = a_stProjectInfo;
	}

	//! 윈도우즈 프로젝트 정보를 변경한다
	public void SetWindowsProjectInfo(STProjectInfo a_stProjectInfo) {
		m_stWindowsProjectInfo = a_stProjectInfo;
	}

	//! iOS 프로젝트 정보를 변경한다
	public void SetiOSProjectInfo(STProjectInfo a_stProjectInfo) {
		m_stiOSProjectInfo = a_stProjectInfo;
	}

	//! 구글 프로젝트 정보를 변경한다
	public void SetGoogleProjectInfo(STProjectInfo a_stProjectInfo) {
		m_stGoogleProjectInfo = a_stProjectInfo;
	}

	//! 원 스토어 프로젝트 정보를 변경한다
	public void SetOneStoreProjectInfo(STProjectInfo a_stProjectInfo) {
		m_stOneStoreProjectInfo = a_stProjectInfo;
	}

	//! 갤럭시 스토어 프로젝트 정보를 변경한다
	public void SetGalaxyStoreProjectInfo(STProjectInfo a_stProjectInfo) {
		m_stGalaxyStoreProjectInfo = a_stProjectInfo;
	}
	#endregion			// 조건부 함수
}
