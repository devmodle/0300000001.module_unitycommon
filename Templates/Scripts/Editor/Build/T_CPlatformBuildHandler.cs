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

//! 플랫폼 빌드 처리자
public static partial class CPlatformBuildHandler {
	//! 빌드가 완료 되었을 경우
	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget a_eTarget, string a_oPath) {
		bool bIsWindows = a_eTarget == BuildTarget.StandaloneWindows || a_eTarget == BuildTarget.StandaloneWindows64;

		if(bIsWindows || a_eTarget == BuildTarget.StandaloneOSX) {
			CPlatformBuildHandler.OnPostProcessStandaloneBuild(a_eTarget, a_oPath);
		} else if(a_eTarget == BuildTarget.iOS) {
			CPlatformBuildHandler.OnPostProcessiOSBuild(a_eTarget, a_oPath);
		} else if(a_eTarget == BuildTarget.Android) {
			CPlatformBuildHandler.OnPostProcessAndroidBuild(a_eTarget, a_oPath);
		}
	}

	//! 독립 플랫폼 빌드가 완료 되었을 경우
	private static void OnPostProcessStandaloneBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_STANDALONE

#endif			// #if UNITY_STANDALONE
	}

	//! iOS 빌드가 완료 되었을 경우
	private static void OnPostProcessiOSBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_IOS
		string oPlistFilepath = string.Format(KCEditorDefine.B_IOS_INFO_PLIST_PATH_FORMAT, a_oPath);
		string oProjFilepath = PBXProject.GetPBXProjectPath(a_oPath);

		// Plist 옵션을 설정한다 {
		var oDocument = new PlistDocument();
		oDocument.ReadFromFile(oPlistFilepath);

		if(oDocument.ExIsValid()) {
			oDocument.root.SetBoolean(KCEditorDefine.B_IOS_ENCRYPTION_ENABLE_KEY, false);
			oDocument.WriteToFile(oPlistFilepath);
		}
		// Plist 옵션을 설정한다 }

		// 프로젝트 옵션을 설정한다 {
		var oProj = new PBXProject();
		oProj.ReadFromFile(oProjFilepath);

		if(oProj != null) {
			string oGUID = oProj.GetUnityMainTargetGuid();

			var oCapability = new ProjectCapabilityManager(oProjFilepath,
				KCEditorDefine.B_PATH_CAPABILITY_ENTITLEMENTS_IOS, null, oGUID);

			for(int i = 0; i < KACEditorDefine.B_EXTRA_FRAMEWORKS_IOS.Length; ++i) {
				oProj.AddFrameworkToProj(oGUID, KACEditorDefine.B_EXTRA_FRAMEWORKS_IOS[i], false);
			}

			for(int i = 0; i < KACEditorDefine.B_EXTRA_CAPABILITY_TYPES_IOS.Length; ++i) {
				var oCapabilityType = KACEditorDefine.B_EXTRA_CAPABILITY_TYPES_IOS[i];

				if(oCapabilityType.Equals(PBXCapabilityType.GameCenter)) {
					oCapability.AddGameCenter();
				} else if(oCapabilityType.Equals(PBXCapabilityType.SignInWithApple)) {
					oCapability.AddSignInWithApple();
				} else if(oCapabilityType.Equals(PBXCapabilityType.PushNotifications)) {
					oCapability.AddPushNotifications(!CPlatformBuilder.IsDistributionBuild);
				}

				oProj.AddCapability(oGUID, KACEditorDefine.B_EXTRA_CAPABILITY_TYPES_IOS[i], 
					KCEditorDefine.B_PATH_CAPABILITY_ENTITLEMENTS_IOS);
			}

			oCapability.WriteToFile();
			oProj.WriteToFile(oProjFilepath);
		}
		// 프로젝트 옵션을 설정한다 }
#endif			// #if UNITY_IOS
	}

	//! 안드로이드 빌드가 완료 되었을 경우
	private static void OnPostProcessAndroidBuild(BuildTarget a_eTarget, string a_oPath) {
#if UNITY_ANDROID

#endif			// #if UNITY_ANDROID
	}
}
#endif			// #if UNITY_EDITOR
#endif			// #if NEVER_USE_THIS
