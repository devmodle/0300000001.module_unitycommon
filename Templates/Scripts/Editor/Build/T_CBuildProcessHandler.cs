using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif			// #if UNITY_IOS

//! 빌드 프로세스 처리자
[InitializeOnLoad]
public static partial class CBuildProcessHandler {
	#region 클래스 변수
	private static Dictionary<BuildTarget, System.Action<BuildTarget, string>> m_oPostProcessBuildList = new Dictionary<BuildTarget, System.Action<BuildTarget, string>>() {
		[BuildTarget.StandaloneOSX] = CBuildProcessHandler.OnPostProcessStandaloneBuild,
		[BuildTarget.StandaloneWindows] = CBuildProcessHandler.OnPostProcessStandaloneBuild,
		[BuildTarget.StandaloneWindows64] = CBuildProcessHandler.OnPostProcessStandaloneBuild,

		[BuildTarget.iOS] = CBuildProcessHandler.OnPostProcessiOSBuild,
		[BuildTarget.Android] = CBuildProcessHandler.OnPostProcessAndroidBuild
	};
	#endregion			// 클래스 변수

	#region 클래스 함수
	//! 빌드가 완료 되었을 경우
	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget a_eTarget, string a_oPath) {
		CAccess.Assert(CBuildProcessHandler.m_oPostProcessBuildList.ContainsKey(a_eTarget));
		CBuildProcessHandler.m_oPostProcessBuildList[a_eTarget](a_eTarget, a_oPath);
	}

	//! 독립 플랫폼 빌드가 완료 되었을 경우
	private static void OnPostProcessStandaloneBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_STANDALONE
		string oDestPath = string.Format(KCEditorDefine.B_DIR_P_FMT_STANDALONE_EXTERNAL_DATAS, a_oPath);
		CFunc.CopyDir(KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS, oDestPath);
#endif			// #if UNITY_STANDALONE
	}

	//! iOS 빌드가 완료 되었을 경우
	private static void OnPostProcessiOSBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_IOS
		string oPlistPath = string.Format(KCEditorDefine.B_PLIST_P_FMT_IOS, a_oPath);
		string oProjPath = PBXProject.GetPBXProjectPath(a_oPath);

		// Plist 옵션을 설정한다 {
		var oDoc = new PlistDocument();
		oDoc.ReadFromFile(oPlistPath);

		// Plist 가 존재 할 경우
		if(oDoc.ExIsValid()) {
			oDoc.root.SetBoolean(KCEditorDefine.B_KEY_IOS_ENCRYPTION_ENABLE, KEditorDefine.B_IOS_ENCRYPTION_ENABLE);
			var oAdsNetworkItemList = oDoc.ExGetArray(KCEditorDefine.B_KEY_IOS_ADS_NETWORK_ITEMS);
			
			for(int i = 0; i < KEditorDefine.B_IOS_ADS_NETWORK_IDS.Length; ++i) {
				var oAdsNetworkIDInfo = oAdsNetworkItemList.AddDict();
				string oID = KEditorDefine.B_IOS_ADS_NETWORK_IDS[i];

				oAdsNetworkIDInfo.SetString(KCEditorDefine.B_KEY_IOS_ADS_NETWORK_ID, oID);
			}

			oDoc.WriteToFile(oPlistPath);
		}
		// Plist 옵션을 설정한다 }

		// 프로젝트 옵션을 설정한다 {
		var oProj = new PBXProject();
		oProj.ReadFromFile(oProjPath);

		string oMainGUID = oProj.GetUnityMainTargetGuid();
		string oFrameworkGUID = oProj.GetUnityFrameworkTargetGuid();

		oProj.SetBuildProperty(oMainGUID, KCEditorDefine.B_PROPERTY_N_IOS_ENABLE_BITCODE, KCEditorDefine.B_TEXT_IOS_YES);
		oProj.SetBuildProperty(oFrameworkGUID, KCEditorDefine.B_PROPERTY_N_IOS_ENABLE_BITCODE, KCEditorDefine.B_TEXT_IOS_YES);

		for(int i = 0; i < KEditorDefine.B_IOS_EXTRA_FRAMEWORKS.Length; ++i) {
			oProj.AddFrameworkToProject(oMainGUID, KEditorDefine.B_IOS_EXTRA_FRAMEWORKS[i], false);
			oProj.AddFrameworkToProject(oFrameworkGUID, KEditorDefine.B_IOS_EXTRA_FRAMEWORKS[i], false);
		}

		for(int i = 0; i < KEditorDefine.B_IOS_EXTRA_CAPABILITY_TYPES.Length; ++i) {
			oProj.AddCapability(oMainGUID, KEditorDefine.B_IOS_EXTRA_CAPABILITY_TYPES[i]);
		}

		// 전처리기 심볼 테이블이 존재 할 경우
		if(CPlatformOptsSetter.DefineSymbolListContainer != null && CPlatformOptsSetter.DefineSymbolListContainer.ContainsKey(BuildTargetGroup.iOS)) {
			var oDefineSymbolList = CPlatformOptsSetter.DefineSymbolListContainer[BuildTargetGroup.iOS];

			for(int i = 0; i < oDefineSymbolList.Count; ++i) {
				oProj.AddBuildProperty(oMainGUID, KCEditorDefine.B_PROPERTY_N_IOS_PREPROCESSOR_DEFINITIONS, oDefineSymbolList[i]);
				oProj.AddBuildProperty(oFrameworkGUID, KCEditorDefine.B_PROPERTY_N_IOS_PREPROCESSOR_DEFINITIONS, oDefineSymbolList[i]);
			}
		}

		oProj.WriteToFile(oProjPath);
		var oCapability = new ProjectCapabilityManager(oProjPath, KCEditorDefine.B_ENTITLEMENTS_P_IOS_CAPABILITY, null, oMainGUID);
		
		for(int i = 0; i < KEditorDefine.B_IOS_EXTRA_CAPABILITY_TYPES.Length; ++i) {
			var oCapabilityType = KEditorDefine.B_IOS_EXTRA_CAPABILITY_TYPES[i];

			// 애플 로그인 타입 일 경우
			if(oCapabilityType.Equals(PBXCapabilityType.SignInWithApple)) {
				oCapability.AddSignInWithApple();
			}
			// 푸시 알림 타입 일 경우
			else if(oCapabilityType.Equals(PBXCapabilityType.PushNotifications)) {
				bool bIsDevBuild = CPlatformBuilder.BuildType != EBuildType.ADHOC && CPlatformBuilder.BuildType != EBuildType.STORE;
				oCapability.AddPushNotifications(bIsDevBuild);

#if FIREBASE_MODULE_ENABLE && FIREBASE_CLOUD_MSG_ENABLE
				oCapability.AddBackgroundModes(KEditorDefine.B_IOS_BACKGROUND_MODES_OPTS);
#endif			// #if FIREBASE_MODULE_ENABLE && FIREBASE_CLOUD_MSG_ENABLE
			}
			// 게임 센터 타입 일 경우
			else if(oCapabilityType.Equals(PBXCapabilityType.GameCenter)) {
				oCapability.AddGameCenter();
			}
			// 결제 타입 일 경우
			else if(oCapabilityType.Equals(PBXCapabilityType.InAppPurchase)) {
				oCapability.AddInAppPurchase();
			}
		}

		oCapability.WriteToFile();
		// 프로젝트 옵션을 설정한다 }
#endif			// #if UNITY_IOS
	}

	//! 안드로이드 빌드가 완료 되었을 경우
	private static void OnPostProcessAndroidBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_ANDROID

#endif			// #if UNITY_ANDROID
	}
	#endregion			// 클래스 함수
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
