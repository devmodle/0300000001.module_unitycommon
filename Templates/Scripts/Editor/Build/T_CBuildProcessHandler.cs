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
public static partial class CBuildProcessHandler {
	//! 빌드가 완료 되었을 경우
	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget a_eTarget, string a_oPath) {
		bool bIsWindows = a_eTarget == BuildTarget.StandaloneWindows || a_eTarget == BuildTarget.StandaloneWindows64;

		// 테스크 탑 플랫폼 일 경우
		if(bIsWindows || a_eTarget == BuildTarget.StandaloneOSX) {
			CBuildProcessHandler.OnPostProcessStandaloneBuild(a_eTarget, a_oPath);
		}
		// iOS 플랫폼 일 경우
		else if(a_eTarget == BuildTarget.iOS) {
			CBuildProcessHandler.OnPostProcessiOSBuild(a_eTarget, a_oPath);
		}
		// 안드로이드 플랫폼 일 경우
		else if(a_eTarget == BuildTarget.Android) {
			CBuildProcessHandler.OnPostProcessAndroidBuild(a_eTarget, a_oPath);
		}
	}

	//! 독립 플랫폼 빌드가 완료 되었을 경우
	private static void OnPostProcessStandaloneBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_STANDALONE

#endif			// #if UNITY_STANDALONE
	}

	//! iOS 플랫폼 빌드가 완료 되었을 경우
	private static void OnPostProcessiOSBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_IOS
		string oPlistFilepath = string.Format(KCEditorDefine.B_IOS_INFO_PLIST_PATH_FORMAT, a_oPath);
		string oProjFilepath = PBXProject.GetPBXProjectPath(a_oPath);

		// Plist 옵션을 설정한다 {
		var oDocument = new PlistDocument();
		oDocument.ReadFromFile(oPlistFilepath);

		// Plist 가 존재 할 경우
		if(oDocument.ExIsValid()) {
			oDocument.root.SetBoolean(KCEditorDefine.B_IOS_ENCRYPTION_ENABLE_KEY, false);
			oDocument.WriteToFile(oPlistFilepath);
		}
		// Plist 옵션을 설정한다 }

		// 프로젝트 옵션을 설정한다 {
		var oProj = new PBXProject();
		oProj.ReadFromFile(oProjFilepath);

		// 프로젝트가 존재 할 경우
		if(oProj != null) {
			string oMainGUID = oProj.GetUnityMainTargetGuid();
			string oFrameworkGUID = oProj.GetUnityFrameworkTargetGuid();

			oProj.SetBuildProperty(oMainGUID, KCEditorDefine.B_PROPERTY_NAME_ENABLE_BITCODE, KCEditorDefine.B_PROPERTY_VALUE_ENABLE_BITCODE);
			oProj.SetBuildProperty(oFrameworkGUID, KCEditorDefine.B_PROPERTY_NAME_ENABLE_BITCODE, KCEditorDefine.B_PROPERTY_VALUE_ENABLE_BITCODE);

			for(int i = 0; i < KEditorDefine.B_EXTRA_FRAMEWORKS_IOS.Length; ++i) {
				oProj.AddFrameworkToProject(oMainGUID, KEditorDefine.B_EXTRA_FRAMEWORKS_IOS[i], false);
			}

			for(int i = 0; i < KEditorDefine.B_EXTRA_CAPABILITY_TYPES_IOS.Length; ++i) {
				oProj.AddCapability(oMainGUID, KEditorDefine.B_EXTRA_CAPABILITY_TYPES_IOS[i]);
			}
			
			oProj.WriteToFile(oProjFilepath);

			var oCapability = new ProjectCapabilityManager(oProjFilepath,
				KCEditorDefine.B_PATH_CAPABILITY_ENTITLEMENTS_IOS, null, oMainGUID);
			
			for(int i = 0; i < KEditorDefine.B_EXTRA_CAPABILITY_TYPES_IOS.Length; ++i) {
				var oCapabilityType = KEditorDefine.B_EXTRA_CAPABILITY_TYPES_IOS[i];

				// 푸시 알림 추가가 가능 할 경우
				if(oCapabilityType.Equals(PBXCapabilityType.PushNotifications)) {
					oCapability.AddPushNotifications(!CCommonPlatformBuilder.IsDistributionBuild);
				}
				// 게임 센터 추가가 가능 할 경우
				else if(oCapabilityType.Equals(PBXCapabilityType.GameCenter)) {
					oCapability.AddGameCenter();
				}
				// 결제 추가가 가능 할 경우
				else if(oCapabilityType.Equals(PBXCapabilityType.InAppPurchase)) {
					oCapability.AddInAppPurchase();
				}
			}

			oCapability.WriteToFile();
		}
		// 프로젝트 옵션을 설정한다 }
#endif			// #if UNITY_IOS
	}

	//! 안드로이드 플랫폼 빌드가 완료 되었을 경우
	private static void OnPostProcessAndroidBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_ANDROID

#endif			// #if UNITY_ANDROID
	}
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
