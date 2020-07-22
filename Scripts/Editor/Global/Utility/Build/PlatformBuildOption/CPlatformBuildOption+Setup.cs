using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//! 플랫폼 빌드 옵션 - 설정
public static partial class CPlatformBuildOption {
	#region 클래스 함수
	//! 플러그인 프로젝트를 설정한다
	[MenuItem("Utility/Setup/PluginProjects")]
	public static void SetupPluginProjects() {
		// iOS 플러그인을 복사한다
		CFunc.CopyDirectory(KCEditorDefine.B_IOS_SRC_PLUGIN_PATH, KCEditorDefine.B_IOS_DEST_PLUGIN_PATH);

		// 안드로이드 플러그인을 복사한다 {
		CFunc.CopyFile(KCEditorDefine.B_ANDROID_SRC_PLUGIN_PATH, KCEditorDefine.B_ANDROID_DEST_PLUGIN_PATH);
		CFunc.CopyFile(KCEditorDefine.B_ANDROID_SRC_UNITY_PLUGIN_PATH, KCEditorDefine.B_ANDROID_DEST_UNITY_PLUGIN_PATH);

		CFunc.CopyFile(KCEditorDefine.B_ANDROID_SRC_MANIFEST_PATH, KCEditorDefine.B_ANDROID_DEST_MANIFEST_PATH, false);
		CFunc.CopyFile(KCEditorDefine.B_ANDROID_SRC_MAIN_TEMPLATE_PATH, KCEditorDefine.B_ANDROID_DEST_MAIN_TEMPLATE_PATH, false);
		CFunc.CopyFile(KCEditorDefine.B_ANDROID_SRC_PROGUARD_PATH, KCEditorDefine.B_ANDROID_DEST_PROGUARD_PATH, false);
		// 안드로이드 플러그인을 복사한다 }

		CEditorFunc.UpdateAssetDatabaseState();
	}

	//! 플레이어 옵션을 설정한다
	[MenuItem("Utility/Setup/PlayerOptions")]
	public static void SetupPlayerOptions() {
		CPlatformBuildOption.SetupPlatformOptions();
		CPlatformBuildOption.SetupBuildOptions();
	}

	//! 에디터 옵션을 설정한다
	[MenuItem("Utility/Setup/EditorOptions")]
	public static void SetupEditorOptions() {
		CPlatformBuildOption.EditorInitialize();

		// 에디터 옵션을 설정한다 {
		EditorSettings.enterPlayModeOptionsEnabled = false;
		
		EditorSettings.spritePackerMode = SpritePackerMode.AlwaysOnAtlas;
		EditorSettings.serializationMode = SerializationMode.ForceText;
		EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.None;

		EditorSettings.unityRemoteResolution = KCEditorDefine.B_EDITOR_OPTION_REMOTE_RESOLUTION;
		EditorSettings.unityRemoteCompression = KCEditorDefine.B_EDITOR_OPTION_REMOTE_COMPRESSION;
		EditorSettings.externalVersionControl = KCEditorDefine.B_EDITOR_OPTION_VERSION_CONTROL;
		EditorSettings.unityRemoteJoystickSource = KCEditorDefine.B_EDITOR_OPTION_JOYSTIC_SOURCE;
		EditorSettings.projectGenerationRootNamespace = string.Empty;
		EditorSettings.projectGenerationUserExtensions = KCEditorDefine.B_EDITOR_OPTION_EXTENSIONS;

		EditorSettings.prefabUIEnvironment = CEditorFunc.FindAsset<SceneAsset>(KCEditorDefine.B_SCENE_NAME_PATTERN, new string[] {
			KCEditorDefine.B_DIR_PATH_AUTO_SCENES,
			KCEditorDefine.B_DIR_PATH_SCENES
		});

		EditorSettings.prefabRegularEnvironment = CEditorFunc.FindAsset<SceneAsset>(KCEditorDefine.B_SCENE_NAME_PATTERN, new string[] {
			KCEditorDefine.B_DIR_PATH_AUTO_SCENES,
			KCEditorDefine.B_DIR_PATH_SCENES
		});

#if MODE_2D_ENABLE
		EditorSettings.defaultBehaviorMode = EditorBehaviorMode.Mode2D;
#else
		EditorSettings.defaultBehaviorMode = EditorBehaviorMode.Mode3D;
#endif			// #if MODE_2D_ENABLE

#if UNITY_IOS
		EditorSettings.unityRemoteDevice = KCEditorDefine.B_EDITOR_OPTION_IOS_REMOTE_DEVICE;
#elif UNITY_ANDROID
		EditorSettings.unityRemoteDevice = KCEditorDefine.B_EDITOR_OPTION_ANDROID_REMOTE_DEVICE;
#else
		EditorSettings.unityRemoteDevice = KCEditorDefine.B_EDITOR_OPTION_DISABLE_REMOTE_DEVICE;
#endif			// #if UNITY_IOS

		if(CPlatformBuildOption.BuildOptionTable != null) {
			EditorSettings.asyncShaderCompilation = CPlatformBuildOption.BuildOptionTable.EditorOption.m_bIsAsyncShaderCompile;
			EditorSettings.useLegacyProbeSampleCount = CPlatformBuildOption.BuildOptionTable.EditorOption.m_bIsUseLegacyProbeSampleCount;
			EditorSettings.enableTextureStreamingInPlayMode = CPlatformBuildOption.BuildOptionTable.EditorOption.m_bIsEnableTextureStreamingInPlayMode;
			EditorSettings.enableTextureStreamingInEditMode = CPlatformBuildOption.BuildOptionTable.EditorOption.m_bIsEnableTextureStreamingInEditMode;

			EditorSettings.cacheServerMode = CPlatformBuildOption.BuildOptionTable.EditorOption.m_eCacheServerMode;	
			EditorSettings.assetPipelineMode = CPlatformBuildOption.BuildOptionTable.EditorOption.m_eAssetPipelineMode;
			EditorSettings.lineEndingsForNewScripts = CPlatformBuildOption.BuildOptionTable.EditorOption.m_eLineEndingMode;
			EditorSettings.etcTextureCompressorBehavior = (int)CPlatformBuildOption.BuildOptionTable.EditorOption.m_eTextureCompressionType;
		}
		// 에디터 옵션을 설정한다 }

		// 사운드 옵션을 설정한다 {
		var oSndManager = CEditorFunc.LoadAsset(KCEditorDefine.B_ASSET_PATH_SND_MANAGER);

		if(oSndManager != null && CPlatformBuildOption.BuildOptionTable != null) {
			var oConfiguration = AudioSettings.GetConfiguration();
			oConfiguration.sampleRate = CPlatformBuildOption.BuildOptionTable.SndOption.m_nSampleRate;
			oConfiguration.numRealVoices = CPlatformBuildOption.BuildOptionTable.SndOption.m_nNumRealVoices;
			oConfiguration.numVirtualVoices = CPlatformBuildOption.BuildOptionTable.SndOption.m_nNumVirtualVoices;
			oConfiguration.speakerMode = CPlatformBuildOption.BuildOptionTable.SndOption.m_eSpeakerMode;
			oConfiguration.dspBufferSize = (int)CPlatformBuildOption.BuildOptionTable.SndOption.m_eDSPBufferSize;

			AudioSettings.Reset(oConfiguration);
			var oSerializeObj = new SerializedObject(oSndManager);

			oSerializeObj.ExSetPropertyValue(KCEditorDefine.B_PROPERTY_NAME_SND_M_DISABLE_AUDIO, (a_oProperty) => {
				a_oProperty.boolValue = CPlatformBuildOption.BuildOptionTable.SndOption.m_bIsDisable;
			});

			oSerializeObj.ExSetPropertyValue(KCEditorDefine.B_PROPERTY_NAME_SND_M_VIRTUALIZE_EFFECT, (a_oProperty) => {
				a_oProperty.boolValue = CPlatformBuildOption.BuildOptionTable.SndOption.m_bIsVirtualizeEffect;
			});

			oSerializeObj.ExSetPropertyValue(KCEditorDefine.B_PROPERTY_NAME_SND_M_GLOBAL_VOLUME, (a_oProperty) => {
				a_oProperty.floatValue = CPlatformBuildOption.BuildOptionTable.SndOption.m_fGlobalVolume;
			});

			oSerializeObj.ExSetPropertyValue(KCEditorDefine.B_PROPERTY_NAME_SND_M_ROLLOFF_SCALE, (a_oProperty) => {
				a_oProperty.floatValue = CPlatformBuildOption.BuildOptionTable.SndOption.m_fRolloffScale;
			});

			oSerializeObj.ExSetPropertyValue(KCEditorDefine.B_PROPERTY_NAME_SND_M_DOPPLER_FACTOR, (a_oProperty) => {
				a_oProperty.floatValue = CPlatformBuildOption.BuildOptionTable.SndOption.m_fDopplerFactor;
			});
		}
		// 사운드 옵션을 설정한다 }

		// 태그 옵션을 설정한다 {
#if !CUSTOM_TAG_ENABLE || !CUSTOM_SORTING_LAYER_ENABLE
		var oTagManager = CEditorFunc.LoadAsset(KCEditorDefine.B_ASSET_PATH_TAG_MANAGER);

		if(oTagManager != null) {
			var oSerializeObj = new SerializedObject(oTagManager);

#if !CUSTOM_TAG_ENABLE
			oSerializeObj.ExSetPropertyValue(KCEditorDefine.B_PROPERTY_NAME_TAG_M_TAG, (a_oProperty) => {
#if !EXTRA_TAG_ENABLE
				a_oProperty.ClearArray();
#endif			// #if !EXTRA_TAG_ENABLE

				if(a_oProperty.arraySize < KCDefine.U_TAGS.Length) {
					for(int i = a_oProperty.arraySize; i < KCDefine.U_TAGS.Length; ++i) {
						a_oProperty.InsertArrayElementAtIndex(i);
					}
				}

				CAccess.Assert(a_oProperty.arraySize >= KCDefine.U_TAGS.Length);

				for(int i = 0; i < a_oProperty.arraySize; ++i) {
					var oProperty = a_oProperty.GetArrayElementAtIndex(i);
					oProperty.stringValue = KCDefine.U_TAGS[i];
				}
			});
#endif			// #if !CUSTOM_TAG_ENABLE

#if !CUSTOM_SORTING_LAYER_ENABLE
			oSerializeObj.ExSetPropertyValue(KCEditorDefine.B_PROPERTY_NAME_TAG_M_SORTING_LAYER, (a_oProperty) => {
#if !EXTRA_SORTING_LAYER_ENABLE
				a_oProperty.ClearArray();
#endif			// #if !EXTRA_TAG_ENABLE

				if(a_oProperty.arraySize < KCDefine.U_SORTING_LAYERS.Length) {
					for(int i = a_oProperty.arraySize; i < KCDefine.U_SORTING_LAYERS.Length; ++i) {
						a_oProperty.InsertArrayElementAtIndex(i);
					}
				}

				CAccess.Assert(a_oProperty.arraySize >= KCDefine.U_SORTING_LAYERS.Length);

				for(int i = 0; i < a_oProperty.arraySize; ++i) {
					var oEnumerator = a_oProperty.GetArrayElementAtIndex(i).GetEnumerator();

					oEnumerator.ExEnumerate((a_oValue) => {
						var oProperty = (SerializedProperty)a_oValue;
						string oSortingLayer = KCDefine.U_SORTING_LAYERS[i];

						if(oProperty.name.ExIsEquals(KCEditorDefine.B_PROPERTY_NAME_TAG_M_NAME)) {
							oProperty.stringValue = oSortingLayer;
						} else if(oProperty.name.ExIsEquals(KCEditorDefine.B_PROPERTY_NAME_TAG_M_UNIQUE_ID)) {
							oProperty.intValue = oSortingLayer.ExIsEquals(KCDefine.U_SORTING_LAYER_DEF) ? 0 : i + 10;
						}
					});
				}
			});
#endif			// #if !CUSTOM_SORTING_LAYER_ENABLE
#endif			// #if !CUSTOM_TAG_ENABLE || !CUSTOM_SORTING_LAYER_ENABLE
			// 태그 옵션을 설정한다 }
		}
	}

	//! 프로젝트 옵션을 설정한다
	[MenuItem("Utility/Setup/ProjectOptions")]
	public static void SetupProjectOptions() {
		// 스크립트를 복사한다 {
		var oEncoding = System.Text.Encoding.Default;

		string oSearch = KCEditorDefine.DS_DEFINE_SYMBOL_NEVER_USE_THIS;
		string oReplace = KCEditorDefine.DS_DEFINE_SYMBOL_USE_CUSTOM_PROJECT_OPTION;

		for(int i = 0; i < KCEditorDefine.B_PATH_SCRIPT_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KCEditorDefine.B_PATH_SCRIPT_FILEPATH_INFOS[i];
			CFunc.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, oSearch, oReplace, oEncoding, false);
		}
		// 스크립트를 복사한다 }

		// 데이터를 복사한다
		for(int i = 0; i < KCEditorDefine.B_PATH_DATA_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KCEditorDefine.B_PATH_DATA_FILEPATH_INFOS[i];
			CFunc.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, false);
		}

		// 프리팹을 복사한다
		for(int i = 0; i < KCEditorDefine.B_PATH_PREFAB_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KCEditorDefine.B_PATH_PREFAB_FILEPATH_INFOS[i];
			CFunc.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, false);
		}
		
		// 테이블을 복사한다
		for(int i = 0; i < KCEditorDefine.B_PATH_TABLE_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KCEditorDefine.B_PATH_TABLE_FILEPATH_INFOS[i];
			CFunc.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, false);
		}

		// 스크립트 객체를 복사한다
		for(int i = 0; i < KCEditorDefine.B_PATH_SCRIPTABLE_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KCEditorDefine.B_PATH_SCRIPTABLE_FILEPATH_INFOS[i];
			CFunc.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, false);
		}

		// 씬을 복사한다
		for(int i = 0; i < KCEditorDefine.B_PATH_SCENE_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KCEditorDefine.B_PATH_SCENE_FILEPATH_INFOS[i];
			CFunc.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, false);
		}
		
		CEditorFunc.UpdateAssetDatabaseState();
	}

	//! 전처리기 심볼을 설정한다
	[MenuItem("Utility/Setup/DefineSymbols")]
	public static void SetupDefineSymbols() {
		// 테이블을 제거한다
		Resources.UnloadAsset(CPlatformBuildOption.BuildInfoTable);
		Resources.UnloadAsset(CPlatformBuildOption.BuildOptionTable);
		Resources.UnloadAsset(CPlatformBuildOption.ProjectInfoTable);
		Resources.UnloadAsset(CPlatformBuildOption.DefineSymbolTable);

		// 전처리기 심볼을 설정한다 {
		CPlatformBuildOption.EditorInitialize();

		if(CPlatformBuildOption.DefineSymbolListContainer.ExIsValid()) {
			foreach(var stKeyValue in CPlatformBuildOption.DefineSymbolListContainer) {
				CPlatformBuildOption.RemoveDefineSymbol(stKeyValue.Key, KCEditorDefine.DS_DEFINE_SYMBOL_FPS_ENABLE);
				CPlatformBuildOption.RemoveDefineSymbol(stKeyValue.Key, KCEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
				CPlatformBuildOption.RemoveDefineSymbol(stKeyValue.Key, KCEditorDefine.DS_DEFINE_SYMBOL_LOGIC_TEST_ENABLE);
				CPlatformBuildOption.RemoveDefineSymbol(stKeyValue.Key, KCEditorDefine.DS_DEFINE_SYMBOL_RECEIPT_CHECK_ENABLE);

				CPlatformBuildOption.RemoveDefineSymbol(stKeyValue.Key, KCEditorDefine.DS_DEFINE_SYMBOL_ADHOC_BUILD);
				CPlatformBuildOption.RemoveDefineSymbol(stKeyValue.Key, KCEditorDefine.DS_DEFINE_SYMBOL_STORE_BUILD);
			}

			CEditorFunc.SetupDefineSymbols(CPlatformBuildOption.DefineSymbolListContainer);
		}
		// 전처리기 심볼을 설정한다 }
	}

	//! 그래픽 API 를 설정한다
	[MenuItem("Utility/Setup/GraphicAPIs")]
	public static void SetupGraphicAPIs() {
		// 독립 플랫폼 그래픽 API 를 설정한다
		CEditorAccess.SetGraphicAPI(BuildTarget.StandaloneOSX, KCEditorDefine.B_MAC_GRAPHICS_DEVICE_TYPES);
		CEditorAccess.SetGraphicAPI(BuildTarget.StandaloneWindows, KCEditorDefine.B_WINDOWS_GRAPHICS_DEVICE_TYPES);
		CEditorAccess.SetGraphicAPI(BuildTarget.StandaloneWindows64, KCEditorDefine.B_WINDOWS_GRAPHICS_DEVICE_TYPES);

		// iOS 그래픽 API 를 설정한다
		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
		CEditorAccess.SetGraphicAPI(BuildTarget.iOS, KCEditorDefine.B_IOS_DEVICE_GRAPHICS_DEVICE_TYPES);

		// 안드로이드 그래픽 API 를 설정한다
		CEditorAccess.SetGraphicAPI(BuildTarget.Android, KCEditorDefine.B_ANDROID_GRAPHICS_DEVICE_TYPES);
	}

		//! 플랫폼 옵션을 설정한다
	private static void SetupPlatformOptions() {
		CPlatformBuildOption.EditorInitialize();

		CPlatformBuildOption.SetupStandalonePlatform();
		CPlatformBuildOption.SetupiOSPlatform();
		CPlatformBuildOption.SetupAndroidPlatform();

		// 퀄리티를 설정한다
		CFunc.SetupQuality(Screen.currentResolution.refreshRate, 
			KCDefine.U_DEF_MULTI_TOUCH_ENABLE, KCDefine.U_DEF_QUALITY_LEVEL, true);

		// 어플리케이션 식별자를 설정한다
		if(CPlatformBuildOption.ProjectInfoTable != null) {
			PlayerSettings.macOS.buildNumber = CPlatformBuildOption.ProjectInfoTable.MacProjectInfo.m_oBuildNumber;

			PlayerSettings.iOS.buildNumber = CPlatformBuildOption.ProjectInfoTable.iOSProjectInfo.m_oBuildNumber;
			PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, CPlatformBuildOption.ProjectInfoTable.iOSProjectInfo.m_oAppID);

			int nBuildNumber = 0;
			bool bIsValidNumber = false;

			if(Application.isBatchMode) {
				if(CPlatformBuilder.StandalonePlatformType == EStandalonePlatformType.WINDOWS) {
					PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, CPlatformBuildOption.ProjectInfoTable.WindowsProjectInfo.m_oAppID);
				} else {
					PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, CPlatformBuildOption.ProjectInfoTable.MacProjectInfo.m_oAppID);
				}

				if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.ONE_STORE) {
					bIsValidNumber = int.TryParse(CPlatformBuildOption.ProjectInfoTable.OneStoreProjectInfo.m_oBuildNumber, out nBuildNumber);
				} else if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.GALAXY_STORE) {
					bIsValidNumber = int.TryParse(CPlatformBuildOption.ProjectInfoTable.GalaxyStoreProjectInfo.m_oBuildNumber, out nBuildNumber);
				} else {
					bIsValidNumber = int.TryParse(CPlatformBuildOption.ProjectInfoTable.GoogleProjectInfo.m_oBuildNumber, out nBuildNumber);
				}
			} else {
#if WINDOWS_PLATFORM
				PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, CPlatformBuildOption.ProjectInfoTable.WindowsProjectInfo.m_oAppID);
#else
				PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, CPlatformBuildOption.ProjectInfoTable.MacProjectInfo.m_oAppID);
#endif			// #if WINDOWS_PLATFORM

#if ONE_STORE_PLATFORM
				bIsValidNumber = int.TryParse(CPlatformBuildOption.ProjectInfoTable.OneStoreProjectInfo.m_oBuildNumber, out nBuildNumber);
#elif GALAXY_STORE_PLATFORM
				bIsValidNumber = int.TryParse(CPlatformBuildOption.ProjectInfoTable.GalaxyStoreProjectInfo.m_oBuildNumber, out nBuildNumber);
#else
				bIsValidNumber = int.TryParse(CPlatformBuildOption.ProjectInfoTable.GoogleProjectInfo.m_oBuildNumber, out nBuildNumber);
#endif			// #if ONE_STORE_PLATFORM
			}

			CAccess.Assert(bIsValidNumber);
			PlayerSettings.Android.bundleVersionCode = nBuildNumber;

			if(Application.isBatchMode) {
				if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.ONE_STORE) {
					PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuildOption.ProjectInfoTable.OneStoreProjectInfo.m_oAppID);
				} else if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.GALAXY_STORE) {
					PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuildOption.ProjectInfoTable.OneStoreProjectInfo.m_oAppID);
				} else {
					PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuildOption.ProjectInfoTable.GoogleProjectInfo.m_oAppID);
				}
			} else {
#if ONE_STORE_PLATFORM
				PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuildOption.ProjectInfoTable.OneStoreProjectInfo.m_oAppID);
#elif GALAXY_STORE_PLATFORM
				PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuildOption.ProjectInfoTable.GalaxyStoreProjectInfo.m_oAppID);
#else
				PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuildOption.ProjectInfoTable.GoogleProjectInfo.m_oAppID);
#endif			// #if ONE_STORE_PLATFORM
			}
		}

		// 스크립트 API 를 설정한다 {
		PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x);
		PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);
		PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Standalone, Il2CppCompilerConfiguration.Release);

		PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
		PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_4_6);
		PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.iOS, Il2CppCompilerConfiguration.Release);

		PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
		PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
		PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, Il2CppCompilerConfiguration.Release);
		// 스크립트 API 를 설정한다 }
	}

	//! 독립 플랫폼을 설정한다
	private static void SetupStandalonePlatform() {
		if(CPlatformBuildOption.BuildInfoTable != null) {
			CAccessExtension.ExSetPropertyValue<PlayerSettings.macOS>(null,
				KCEditorDefine.B_PROPERTY_NAME_CATEGORY, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, CPlatformBuildOption.BuildInfoTable.StandaloneBuildInfo.m_oCategory);
		}
	}

	//! iOS 플랫폼을 설정한다
	private static void SetupiOSPlatform() {
		if(CPlatformBuildOption.BuildInfoTable != null) {
			PlayerSettings.iOS.SetiPadLaunchScreenType(CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_eiPadLaunchScreenType);
			PlayerSettings.iOS.SetiPhoneLaunchScreenType(CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_eiPhoneLaunchScreenType);
		}
	}

	//! 안드로이드 플랫폼을 설정한다
	private static void SetupAndroidPlatform() {
		if(CPlatformBuildOption.BuildInfoTable != null) {
			PlayerSettings.Android.useCustomKeystore = true;

			PlayerSettings.Android.keystoreName = CPlatformBuildOption.BuildInfoTable.AndroidBuildInfo.m_oKeystorePath;
			PlayerSettings.Android.keyaliasName = CPlatformBuildOption.BuildInfoTable.AndroidBuildInfo.m_oKeyaliasName;

			PlayerSettings.Android.keystorePass = CPlatformBuildOption.BuildInfoTable.AndroidBuildInfo.m_oKeystorePassword;
			PlayerSettings.Android.keyaliasPass = CPlatformBuildOption.BuildInfoTable.AndroidBuildInfo.m_oKeyaliasPassword;
		}
	}

	//! 빌드 옵션을 설정한다
	private static void SetupBuildOptions() {
		CPlatformBuildOption.EditorInitialize();

		CPlatformBuildOption.SetupStandaloneBuildOptions();
		CPlatformBuildOption.SetupiOSBuildOptions();
		CPlatformBuildOption.SetupAndroidBuildOptions();
		
		// 기본 옵션을 설정한다 {
		PlayerSettings.usePlayerLog = true;
		PlayerSettings.graphicsJobs = false;
		PlayerSettings.gcIncremental = false;
		PlayerSettings.statusBarHidden = true;
		PlayerSettings.stripEngineCode = true;
		PlayerSettings.allowUnsafeCode = false;
		PlayerSettings.enableCrashReportAPI = true;
		PlayerSettings.enableMetalAPIValidation = true;
		PlayerSettings.stripUnusedMeshComponents = true;
		PlayerSettings.defaultIsNativeResolution = true;
		PlayerSettings.logObjCUncaughtExceptions = true;
		PlayerSettings.legacyClampBlendShapeWeights = false;

		PlayerSettings.actionOnDotNetUnhandledException = ActionOnDotNetUnhandledException.Crash;

		if(CPlatformBuildOption.ProjectInfoTable != null) {
			PlayerSettings.companyName = CPlatformBuildOption.ProjectInfoTable.CompanyName;
			PlayerSettings.productName = CPlatformBuildOption.ProjectInfoTable.ProductName;
		}

#if LINEAR_PIPELINE_ENABLE
		PlayerSettings.colorSpace = ColorSpace.Linear;
#else
		PlayerSettings.colorSpace = ColorSpace.Gamma;
#endif			// #if LINEAR_PIPELINE_ENABLE
		// 기본 옵션을 설정한다 }

		// 디바이스 방향을 설정한다 {
		PlayerSettings.useAnimatedAutorotation = false;

#if PORTRAIT_ENABLE
		PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
#else
		PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
#endif			// #if PORTRAIT_ENABLE
		// 디바이스 방향을 설정한다 }

		// 스플래시 옵션을 설정한다 {
		PlayerSettings.SplashScreen.show = false;
		PlayerSettings.SplashScreen.showUnityLogo = false;
		PlayerSettings.SplashScreen.blurBackgroundImage = true;

		PlayerSettings.SplashScreen.drawMode = PlayerSettings.SplashScreen.DrawMode.UnityLogoBelow;
		PlayerSettings.SplashScreen.animationMode = PlayerSettings.SplashScreen.AnimationMode.Static;
		PlayerSettings.SplashScreen.unityLogoStyle = PlayerSettings.SplashScreen.UnityLogoStyle.LightOnDark;
		PlayerSettings.SplashScreen.backgroundColor = KCEditorDefine.B_COLOR_UNITY_LOGO_BG;
		// 스플래시 옵션을 설정한다 }

		// 로그 타입을 설정한다
		PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
		PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
		PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
		PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.ScriptOnly);
		PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);

		// 스크립트 관리 수준을 설정한다
		PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Standalone, ManagedStrippingLevel.Low);
		PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel.Low);
		PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Low);

		// 리소스 압축 방식을 변경한다 {
		CExtension.ExCallFunc<EditorUserBuildSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_COMPRESSION_TYPE, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
			BuildTargetGroup.Standalone, (int)CompressionType.None
		});

		CExtension.ExCallFunc<EditorUserBuildSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_COMPRESSION_TYPE, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
			BuildTargetGroup.iOS, (int)CompressionType.None
		});

		CExtension.ExCallFunc<EditorUserBuildSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_COMPRESSION_TYPE, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
			BuildTargetGroup.Android, (int)CompressionType.None
		});
		// 리소스 압축 방식을 변경한다 }

		if(CPlatformBuildOption.BuildOptionTable != null) {
			PlayerSettings.gpuSkinning = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsGPUSkinning;
			PlayerSettings.MTRendering = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsMTRendering;
			PlayerSettings.runInBackground = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsRunInBackground;
			PlayerSettings.bakeCollisionMeshes = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsPreBakeCollisionMesh;
			PlayerSettings.use32BitDisplayBuffer = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsUse32BitDisplayBuffer;
			PlayerSettings.muteOtherAudioSources = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsMuteOtherAudioSource;
			PlayerSettings.enableFrameTimingStats = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsEnableFrameTimingStats;
			PlayerSettings.enableInternalProfiler = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsEnableInternalProfiler;
			PlayerSettings.preserveFramebufferAlpha = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsPreserveFrameBufferAlpha;
			PlayerSettings.vulkanEnableSetSRGBWrite = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsEnableVulkanSRGBWrite;

			PlayerSettings.aotOptions = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_oAOTCompileOption;
			PlayerSettings.accelerometerFrequency = (int)CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_eAccelerometerFrequency;
			PlayerSettings.vulkanNumSwapchainBuffers = CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_nNumVulkanSwapChainBuffers;

			// 기타 옵션을 설정한다
			PlayerSettings.SetPreloadedAssets(CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_oPreloadAssetList?.ToArray());

			// 멀티 쓰레드 렌더링을 설정한다 {
			PlayerSettings.SetMobileMTRendering(BuildTargetGroup.iOS, 
				CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsMTRendering);
			
			PlayerSettings.SetMobileMTRendering(BuildTargetGroup.Android, 
				CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsMTRendering);
			// 멀티 쓰레드 렌더링을 설정한다 }

			// 광원 맵 엔코딩 퀄리티를 설정한다 {
			CExtension.ExCallFunc<PlayerSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_ENCODING_QUALITY, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
				BuildTargetGroup.Standalone, (int)CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_eLightmapEncodingQuality
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_ENCODING_QUALITY, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
				BuildTargetGroup.iOS, (int)CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_eLightmapEncodingQuality
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_ENCODING_QUALITY, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
				BuildTargetGroup.Android, (int)CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_eLightmapEncodingQuality
			});
			// 광원 맵 엔코딩 퀄리티를 설정한다 }

			// 광원 맵 스트리밍 여부를 설정한다 {
			CExtension.ExCallFunc<PlayerSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_ENABLE, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
				BuildTargetGroup.Standalone, CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsEnableLightmapStreaming
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_ENABLE, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
				BuildTargetGroup.iOS, CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsEnableLightmapStreaming
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_ENABLE, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
				BuildTargetGroup.Android, CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_bIsEnableLightmapStreaming
			});
			// 광원 맵 스트리밍 여부를 설정한다 }

			// 광원 맵 스트리밍 우선 순위를 설정한다 {
			CExtension.ExCallFunc<PlayerSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_PRIORITY, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
				BuildTargetGroup.Standalone, CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_nLightmapStreamingPriority
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_PRIORITY, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
				BuildTargetGroup.iOS, CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_nLightmapStreamingPriority
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KCEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_PRIORITY, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, new object[] {
				BuildTargetGroup.Android, CPlatformBuildOption.BuildOptionTable.CommonBuildOption.m_nLightmapStreamingPriority
			});
			// 광원 맵 스트리밍 우선 순위를 설정한다 }
		}
	}

	//! 독립 플랫폼 빌드 옵션을 설정한다
	private static void SetupStandaloneBuildOptions() {
		PlayerSettings.resizableWindow = false;
		PlayerSettings.macRetinaSupport = true;
		PlayerSettings.visibleInBackground = false;
		PlayerSettings.useFlipModelSwapchain = true;
		PlayerSettings.allowFullscreenSwitch = false;
		PlayerSettings.useMacAppStoreValidation = CPlatformBuilder.IsDistributionBuild;

		PlayerSettings.defaultScreenWidth = KCDefine.B_DESKTOP_WINDOW_WIDTH;
		PlayerSettings.defaultScreenHeight = KCDefine.B_DESKTOP_WINDOW_HEIGHT;

		PlayerSettings.SetAspectRatio(AspectRatio.Aspect4by3, true);
		PlayerSettings.SetAspectRatio(AspectRatio.Aspect5by4, true);
		PlayerSettings.SetAspectRatio(AspectRatio.Aspect16by9, true);
		PlayerSettings.SetAspectRatio(AspectRatio.Aspect16by10, true);
		PlayerSettings.SetAspectRatio(AspectRatio.AspectOthers, true);

		if(CPlatformBuildOption.BuildOptionTable != null) {
			PlayerSettings.fullScreenMode = CPlatformBuildOption.BuildOptionTable.StandaloneBuildOption.m_eFullscreenMode;
			PlayerSettings.forceSingleInstance = CPlatformBuildOption.BuildOptionTable.StandaloneBuildOption.m_bIsForceSingleInstance;
			PlayerSettings.captureSingleScreen = CPlatformBuildOption.BuildOptionTable.StandaloneBuildOption.m_bIsCaptureSingleScreen;
		}
	}

	//! iOS 빌드 옵션을 설정한다
	private static void SetupiOSBuildOptions() {
		PlayerSettings.iOS.hideHomeButton = false;
		PlayerSettings.iOS.allowHTTPDownload = true;
		PlayerSettings.iOS.requiresFullScreen = true;
		PlayerSettings.iOS.useOnDemandResources = false;
		PlayerSettings.iOS.forceHardShadowsOnMetal = false;
		PlayerSettings.iOS.appleEnableAutomaticSigning = false;
		PlayerSettings.iOS.disableDepthAndStencilBuffers = false;

		PlayerSettings.iOS.backgroundModes = iOSBackgroundMode.None;
		PlayerSettings.iOS.scriptCallOptimization = ScriptCallOptimizationLevel.SlowAndSafe;
		PlayerSettings.iOS.showActivityIndicatorOnLoading = iOSShowActivityIndicatorOnLoading.DontShow;

		EditorUserBuildSettings.symlinkLibraries = false;
		PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, (int)AppleMobileArchitecture.Universal);

		if(CPlatformBuildOption.BuildInfoTable != null) {
			PlayerSettings.iOS.appleDeveloperTeamID = CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_oTeamID;
			PlayerSettings.iOS.targetOSVersionString = CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_oTargetOSVersion;
			PlayerSettings.iOS.iOSUrlSchemes = CPlatformBuildOption.BuildInfoTable.iOSBuildInfo.m_oURLSchemeList?.ToArray();
		}

		if(CPlatformBuildOption.BuildOptionTable != null) {
			PlayerSettings.iOS.targetDevice = CPlatformBuildOption.BuildOptionTable.iOSBuildOption.m_eTargetDevice;
			PlayerSettings.iOS.statusBarStyle = CPlatformBuildOption.BuildOptionTable.iOSBuildOption.m_eStatusBarStyle;
			PlayerSettings.iOS.requiresPersistentWiFi = CPlatformBuildOption.BuildOptionTable.iOSBuildOption.m_bIsRequirePersistentWIFI;
			PlayerSettings.iOS.appInBackgroundBehavior = CPlatformBuildOption.BuildOptionTable.iOSBuildOption.m_eBackgroundBehavior;

			PlayerSettings.iOS.cameraUsageDescription = CPlatformBuildOption.BuildOptionTable.iOSBuildOption.m_oCameraDescription;
			PlayerSettings.iOS.locationUsageDescription = CPlatformBuildOption.BuildOptionTable.iOSBuildOption.m_oLocationDescription;
			PlayerSettings.iOS.microphoneUsageDescription = CPlatformBuildOption.BuildOptionTable.iOSBuildOption.m_oMicrophoneDescription;

			CAccessExtension.ExSetPropertyValue<PlayerSettings.iOS>(null,
				KCEditorDefine.B_PROPERTY_NAME_APPLE_ENABLE_PRO_MOTION, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, CPlatformBuildOption.BuildOptionTable.iOSBuildOption.m_bIsEnableProMotion);

			CAccessExtension.ExSetPropertyValue<PlayerSettings.iOS>(null,
				KCEditorDefine.B_PROPERTY_NAME_REQUIRE_AR_KIT_SUPPORT, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, CPlatformBuildOption.BuildOptionTable.iOSBuildOption.m_bIsRequreARKitSupport);

			CAccessExtension.ExSetPropertyValue<PlayerSettings.iOS>(null,
				KCEditorDefine.B_PROPERTY_NAME_AUTO_ADD_CAPABILITIES, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, CPlatformBuildOption.BuildOptionTable.iOSBuildOption.m_bIsAutoAddCapabilities);
		}
	}

	//! 안드로이드 빌드 옵션을 설정한다
	private static void SetupAndroidBuildOptions() {
		PlayerSettings.Android.androidIsGame = true;
		PlayerSettings.Android.startInFullscreen = true;
		PlayerSettings.Android.useAPKExpansionFiles = false;
		PlayerSettings.Android.renderOutsideSafeArea = true;
		PlayerSettings.Android.forceSDCardPermission = false;
		PlayerSettings.Android.forceInternetPermission = true;
		PlayerSettings.Android.buildApkPerCpuArchitecture = false;
		PlayerSettings.Android.disableDepthAndStencilBuffers = false;

		PlayerSettings.Android.splashScreenScale = AndroidSplashScreenScale.ScaleToFit;
		PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
		PlayerSettings.Android.showActivityIndicatorOnLoading = AndroidShowActivityIndicatorOnLoading.DontShow;

		EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
		EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
		EditorUserBuildSettings.androidDebugMinification = AndroidMinification.Proguard;
		EditorUserBuildSettings.androidReleaseMinification = AndroidMinification.Proguard;

		if(CPlatformBuildOption.BuildInfoTable != null) {
			PlayerSettings.Android.minSdkVersion = CPlatformBuildOption.BuildInfoTable.AndroidBuildInfo.m_eMinSDKVersion;
			PlayerSettings.Android.targetSdkVersion = CPlatformBuildOption.BuildInfoTable.AndroidBuildInfo.m_eTargetSDKVersion;
		}

		CAccessExtension.ExSetPropertyValue<PlayerSettings.Android>(null,
			KCEditorDefine.B_PROPERTY_NAME_VALIDATE_APP_BUNDLE_SIZE, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, true);

		if(CPlatformBuildOption.BuildOptionTable != null) {
			PlayerSettings.Android.blitType = CPlatformBuildOption.BuildOptionTable.AndroidBuildOption.m_eBlitType;
			PlayerSettings.Android.androidTVCompatibility = CPlatformBuildOption.BuildOptionTable.AndroidBuildOption.m_bIsTVCompatibility;
			PlayerSettings.Android.preferredInstallLocation = CPlatformBuildOption.BuildOptionTable.AndroidBuildOption.m_ePreferredInstallLocation;

			CAccessExtension.ExSetPropertyValue<PlayerSettings.Android>(null,
				KCEditorDefine.B_PROPERTY_NAME_OPTIMIZE_FRAME_PACING, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, CPlatformBuildOption.BuildOptionTable.AndroidBuildOption.m_bIsOptimizeFramePacing);

			CAccessExtension.ExSetPropertyValue<PlayerSettings.Android>(null,
				KCEditorDefine.B_PROPERTY_NAME_APP_BUNDLE_SIZE_TO_VALIDATE, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, CPlatformBuildOption.BuildOptionTable.AndroidBuildOption.m_nAppBundleSize);

			CAccessExtension.ExSetPropertyValue<PlayerSettings.Android>(null,
				KCEditorDefine.B_PROPERTY_NAME_SUPPORTED_ASPECT_RATIO_MODE, KCDefine.B_BINDING_FLAG_NON_PUBLIC_STATIC, (int)CPlatformBuildOption.BuildOptionTable.AndroidBuildOption.m_eAspectRatioMode);
		}
	}
	#endregion			// 클래스 함수
}
