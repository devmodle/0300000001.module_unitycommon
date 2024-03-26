#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif // #if UNITY_IOS

/** 빌드 처리자 */
[InitializeOnLoad]
public static partial class CProcessorBuild
{
	#region 변수
	private static Dictionary<BuildTarget, System.Action<BuildTarget, string>> m_oDictHandlerProcessPost = new Dictionary<BuildTarget, System.Action<BuildTarget, string>>()
	{
		[BuildTarget.iOS] = CProcessorBuild.HandleOnBuildProcessPostiOS,
		[BuildTarget.Android] = CProcessorBuild.HandleOnBuildProcessPostAndroid,
		[BuildTarget.StandaloneOSX] = CProcessorBuild.HandleOnBuildProcessPostStandalone,
		[BuildTarget.StandaloneWindows] = CProcessorBuild.HandleOnBuildProcessPostStandalone,
		[BuildTarget.StandaloneWindows64] = CProcessorBuild.HandleOnBuildProcessPostStandalone
	};

	private static Dictionary<BuildTarget, System.Action<BuildTarget, string>> m_oDictHandlerProcessPostLate = new Dictionary<BuildTarget, System.Action<BuildTarget, string>>()
	{
		[BuildTarget.iOS] = CProcessorBuild.HandleOnBuildProcessPostLateiOS,
		[BuildTarget.Android] = CProcessorBuild.HandleOnBuildProcessPostLateAndroid,
		[BuildTarget.StandaloneOSX] = CProcessorBuild.HandleOnBuildProcessPostLateStandalone,
		[BuildTarget.StandaloneWindows] = CProcessorBuild.HandleOnBuildProcessPostLateStandalone,
		[BuildTarget.StandaloneWindows64] = CProcessorBuild.HandleOnBuildProcessPostLateStandalone
	};
	#endregion // 변수

	#region 클래스 함수
	/** 빌드가 완료되었을 경우 */
	[PostProcessBuild(byte.MaxValue * 10)]
	public static void OnBuildProcessPost(BuildTarget a_eTarget, string a_oPath)
	{
		CProcessorBuild.m_oDictHandlerProcessPostLate.ExGetVal(a_eTarget)?.Invoke(a_eTarget, a_oPath);
	}

	/** 빌드가 완료되었을 경우 */
	[PostProcessBuild(byte.MaxValue * 20)]
	public static void OnBuildProcessPostLate(BuildTarget a_eTarget, string a_oPath)
	{
		// 배치 모드가 아닐 경우
		if(!Application.isBatchMode)
		{
			EditorUtility.RevealInFinder(a_oPath);
		}

		CProcessorBuild.m_oDictHandlerProcessPostLate.ExGetVal(a_eTarget)?.Invoke(a_eTarget, a_oPath);
	}
	#endregion // 클래스 함수
}

/** 빌드 처리자 - iOS */
public static partial class CProcessorBuild
{
	#region 클래스 함수
	/** iOS 빌드 완료를 처리한다 */
	private static void HandleOnBuildProcessPostiOS(BuildTarget a_eTarget, string a_oPath)
	{
#if UNITY_IOS
		string oPlistPath = string.Format(KCDefineEditor.B_DATA_P_FMT_INFO_IOS, a_oPath);
		string oPBXProjPath = PBXProject.GetPBXProjectPath(a_oPath);

		// Plist 파일이 존재 할 경우
		if(File.Exists(oPlistPath))
		{
			var oDoc = new PlistDocument();
			oDoc.ReadFromFile(oPlistPath);
			oDoc.root.ExAddVal(KCDefineEditor.B_KEY_IOS_FIREBASE_APP_STORE_RECEIPT_URL_CHECK_ENABLE, false);
			
			oDoc.root.ExAddVal(KCDefineEditor.B_KEY_IOS_APP_USES_NON_EXEMPT_ENCRYPTION_ENABLE, KCDefineEditor.B_TEXT_IOS_FALSE);
			oDoc.root.ExAddVal(KCDefineEditor.B_KEY_IOS_USER_TRACKING_USAGE_DESC, KEditorDefine.B_IOS_USER_TRACKING_USAGE_DESC);

			var oDeviceCapabilityList = oDoc.ExGetArray(KCDefineEditor.B_KEY_IOS_REQUIRED_DEVICE_CAPABILITIES);
			oDeviceCapabilityList.values.Clear();

			var oAppTransportSecurityDict = oDoc.ExGetDict(KCDefineEditor.B_KEY_IOS_APP_TRANSPORT_SECURITY);
			oAppTransportSecurityDict.values.Clear();
			
			for(int i = 0; i < KEditorDefine.B_IOS_ADS_NETWORK_ID_LIST.Count; ++i)
			{
				var oAdsNetworkItemList = oDoc.ExGetArray(KCDefineEditor.B_KEY_IOS_ADS_NETWORK_ITEMS);

				// 광고 네트워크 식별자가 없을 경우
				if(!oAdsNetworkItemList.ExIsContainsAdsNetworkID(KEditorDefine.B_IOS_ADS_NETWORK_ID_LIST[i]))
				{
					var oAdsNetworkIDInfoDict = oAdsNetworkItemList.AddDict();
					oAdsNetworkIDInfoDict.SetString(KCDefineEditor.B_KEY_IOS_ADS_NETWORK_ID, KEditorDefine.B_IOS_ADS_NETWORK_ID_LIST[i]);
				}
			}
			
			oDoc.WriteToFile(oPlistPath);
		}

		// 프로젝트 파일이 존재 할 경우
		if(File.Exists(oPBXProjPath))
		{
			var oPBXProj = new PBXProject();
			oPBXProj.ReadFromFile(oPBXProjPath);

			string oMainGUID = oPBXProj.GetUnityMainTargetGuid();
			string oFrameworkGUID = oPBXProj.GetUnityFrameworkTargetGuid();

			oPBXProj.SetBuildProperty(oMainGUID, KCDefineEditor.B_PROPERTY_N_IOS_ENABLE_BITCODE, KCDefineEditor.B_TEXT_IOS_FALSE);
			oPBXProj.SetBuildProperty(oFrameworkGUID, KCDefineEditor.B_PROPERTY_N_IOS_ENABLE_BITCODE, KCDefineEditor.B_TEXT_IOS_FALSE);

			oPBXProj.SetBuildProperty(oMainGUID, KCDefineEditor.B_PROPERTY_N_IOS_ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES, KCDefineEditor.B_TEXT_IOS_FALSE);
			oPBXProj.SetBuildProperty(oFrameworkGUID, KCDefineEditor.B_PROPERTY_N_IOS_ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES, KCDefineEditor.B_TEXT_IOS_FALSE);

			for(int i = 0; i < KEditorDefine.B_IOS_ADD_FRAMEWORK_LIST.Count; ++i)
			{
				oPBXProj.AddFrameworkToProject(oMainGUID, KEditorDefine.B_IOS_ADD_FRAMEWORK_LIST[i], false);
				oPBXProj.AddFrameworkToProject(oFrameworkGUID, KEditorDefine.B_IOS_ADD_FRAMEWORK_LIST[i], false);
			}

			// 전처리기 심볼 정보 테이블이 존재 할 경우
			if(CSetterOptsPlatform.DefineSymbolDictContainer != null && CSetterOptsPlatform.DefineSymbolDictContainer.TryGetValue(BuildTargetGroup.iOS, out List<string> oDefineSymbolList))
			{
				for(int i = 0; i < oDefineSymbolList.Count; ++i)
				{
					oPBXProj.AddBuildProperty(oMainGUID, KCDefineEditor.B_PROPERTY_N_IOS_PREPROCESSOR_DEFINITIONS, oDefineSymbolList[i]);
					oPBXProj.AddBuildProperty(oFrameworkGUID, KCDefineEditor.B_PROPERTY_N_IOS_PREPROCESSOR_DEFINITIONS, oDefineSymbolList[i]);
				}
			}

			oPBXProj.WriteToFile(oPBXProjPath);
		}
#endif // #if UNITY_IOS
	}

	/** iOS 빌드 완료를 처리한다 */
	private static void HandleOnBuildProcessPostLateiOS(BuildTarget a_eTarget, string a_oPath)
	{
#if UNITY_IOS
		string oPodsPath = string.Format(KCDefineEditor.B_DATA_P_FMT_COCOA_PODS, a_oPath);
		string oPlistPath = string.Format(KCDefineEditor.B_DATA_P_FMT_INFO_IOS, a_oPath);
		string oPBXProjPath = string.Format(KCDefineEditor.B_PROJ_P_FMT_COCOA_PODS, a_oPath);

		// 코코아 포드 파일이 존재 할 경우
		if(File.Exists(oPodsPath))
		{
			CFuncEditor.ExecuteCmdLine(string.Format(KCDefineEditor.B_BUILD_CMD_FMT_IOS_COCOA_PODS, a_oPath), false);
		}

		// Plist 파일이 존재 할 경우
		if(File.Exists(oPlistPath))
		{
			var oDoc = new PlistDocument();
			oDoc.ReadFromFile(oPlistPath);

			var oDeviceCapabilityList = oDoc.ExGetArray(KCDefineEditor.B_KEY_IOS_REQUIRED_DEVICE_CAPABILITIES);
			oDeviceCapabilityList.ExAddVal(KCDefineEditor.B_TEXT_IOS_METAL);
			oDeviceCapabilityList.ExAddVal(KCDefineEditor.B_TEXT_IOS_ARM_64);

			var oAppTransportSecurityDict = oDoc.ExGetDict(KCDefineEditor.B_KEY_IOS_APP_TRANSPORT_SECURITY);
			oAppTransportSecurityDict.ExAddVal(KCDefineEditor.B_KEY_IOS_ALLOWS_ARBITRARY_LOADS, true);

			oDoc.WriteToFile(oPlistPath);
		}

		// 프로젝트 파일이 존재 할 경우
		if(File.Exists(oPBXProjPath))
		{
			var oPBXProj = new PBXProject();
			oPBXProj.ReadFromFile(oPBXProjPath);

			string oMainGUID = oPBXProj.GetUnityMainTargetGuid();
			string oFrameworkGUID = oPBXProj.GetUnityFrameworkTargetGuid();

			oPBXProj.SetBuildProperty(oPBXProj.ProjectGuid(), 
				KCDefineEditor.B_PROPERTY_N_IOS_ENABLE_BITCODE, KCDefineEditor.B_TEXT_IOS_FALSE);

			oPBXProj.SetBuildProperty(oPBXProj.ProjectGuid(), 
				KCDefineEditor.B_PROPERTY_N_IOS_ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES, KCDefineEditor.B_TEXT_IOS_FALSE);

			oPBXProj.AddBuildProperty(oPBXProj.ProjectGuid(), 
				KCDefineEditor.B_PROPERTY_N_IOS_USER_HEADER_SEARCH_PATHS, KCDefineEditor.B_SEARCH_P_IOS_PODS);

			for(int i = 0; i < KEditorDefine.B_IOS_REMOVE_FRAMEWORK_LIST.Count; ++i)
			{
				oPBXProj.RemoveFrameworkFromProject(oMainGUID, KEditorDefine.B_IOS_REMOVE_FRAMEWORK_LIST[i]);
				oPBXProj.RemoveFrameworkFromProject(oFrameworkGUID, KEditorDefine.B_IOS_REMOVE_FRAMEWORK_LIST[i]);
			}

			oPBXProj.WriteToFile(oPBXProjPath);
		}
#endif // #if UNITY_IOS
	}
	#endregion // 클래스 함수
}

/** 빌드 처리자 - 안드로이드 */
public static partial class CProcessorBuild
{
	#region 클래스 함수
	/** 안드로이드 빌드 완료를 처리한다 */
	private static void HandleOnBuildProcessPostAndroid(BuildTarget a_eTarget, string a_oPath)
	{
#if UNITY_ANDROID
		// Do Something
#endif // #if UNITY_ANDROID
	}

	/** 안드로이드 빌드 완료를 처리한다 */
	private static void HandleOnBuildProcessPostLateAndroid(BuildTarget a_eTarget, string a_oPath)
	{
#if UNITY_ANDROID
		// Do Something
#endif // #if UNITY_ANDROID
	}
	#endregion // 클래스 함수
}

/** 빌드 처리자 - 독립 플랫폼 */
public static partial class CProcessorBuild
{
	#region 클래스 함수
	/** 독립 플랫폼 빌드 완료를 처리한다 */
	private static void HandleOnBuildProcessPostStandalone(BuildTarget a_eTarget, string a_oPath)
	{
#if UNITY_STANDALONE
		string oPath = Path.GetDirectoryName(a_oPath).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH);
		string oDestPath = string.Format(KCDefineEditor.B_DIR_P_FMT_EXTERNAL_DATAS_STANDALONE, oPath);

		CFunc.CopyDir(KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS, oDestPath);
#endif // #if UNITY_STANDALONE
	}

	/** 독립 플랫폼 빌드 완료를 처리한다 */
	private static void HandleOnBuildProcessPostLateStandalone(BuildTarget a_eTarget, string a_oPath)
	{
#if UNITY_STANDALONE
		// Do Something
#endif // #if UNITY_STANDALONE
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
#endif // #if SCRIPT_TEMPLATE_ONLY
