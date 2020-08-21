using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

// 에디터 함수 - 설정
public static partial class EditorFunc {
	#region 클래스 함수
	//! 씬을 설정한다
	public static void SetupScene(Scene a_stScene, Dictionary<string, System.Type> a_oSceneManagerTypeList) {
		foreach(var stKeyValue in a_oSceneManagerTypeList) {
			if(a_stScene.name.ExIsEquals(stKeyValue.Key)) {
				var oSceneManager = a_stScene.ExFindChild(KDefine.U_OBJ_NAME_SCENE_SCENE_MANAGER);

				if(oSceneManager != null && oSceneManager.GetComponentInChildren(stKeyValue.Value) == null) {
					oSceneManager.AddComponent(stKeyValue.Value);
					EditorSceneManager.MarkSceneDirty(a_stScene);
				}
			}
		}
	}

	//! 전처리기 심볼을 설정한다
	public static void SetupDefineSymbols(Dictionary<BuildTargetGroup, List<string>> a_oDefineSymbolListContainer) {
		Func.Assert(a_oDefineSymbolListContainer.ExIsValid());

		foreach(var stKeyValue in a_oDefineSymbolListContainer) {
			var oDefineSymbolList = new List<string>();

			for(int i = 0; i < stKeyValue.Value.Count; ++i) {
				oDefineSymbolList.ExAddValue(stKeyValue.Value[i]);
			}

			PlayerSettings.SetScriptingDefineSymbolsForGroup(stKeyValue.Key,
				oDefineSymbolList.ExToString(KEditorDefine.B_TOKEN_DEFINE_SYMBOL));
		}
	}

	//! 플러그인 프로젝트를 설정한다
	[MenuItem("Utility/Setup/PluginProjects")]
	public static void SetupPluginProjects() {
		// iOS 플러그인을 복사한다
		Func.CopyDirectory(KEditorDefine.B_IOS_SRC_PLUGIN_PATH, KEditorDefine.B_IOS_DEST_PLUGIN_PATH);

		// 안드로이드 플러그인을 복사한다 {
		Func.CopyFile(KEditorDefine.B_ANDROID_SRC_PLUGIN_PATH, KEditorDefine.B_ANDROID_DEST_PLUGIN_PATH);
		Func.CopyFile(KEditorDefine.B_ANDROID_SRC_UNITY_PLUGIN_PATH, KEditorDefine.B_ANDROID_DEST_UNITY_PLUGIN_PATH);

		Func.CopyFile(KEditorDefine.B_ANDROID_SRC_MANIFEST_PATH, KEditorDefine.B_ANDROID_DEST_MANIFEST_PATH, false);
		Func.CopyFile(KEditorDefine.B_ANDROID_SRC_MAIN_TEMPLATE_PATH, KEditorDefine.B_ANDROID_DEST_MAIN_TEMPLATE_PATH, false);
		Func.CopyFile(KEditorDefine.B_ANDROID_SRC_PROGUARD_PATH, KEditorDefine.B_ANDROID_DEST_PROGUARD_PATH, false);
		// 안드로이드 플러그인을 복사한다 }

		EditorFunc.UpdateAssetDatabaseState();
	}

	//! 플레이어 옵션을 설정한다
	[MenuItem("Utility/Setup/PlayerOptions")]
	public static void SetupPlayerOptions() {
		EditorFunc.SetupPlatformOptions();
		EditorFunc.SetupBuildOptions();
	}

	//! 에디터 옵션을 설정한다
	[MenuItem("Utility/Setup/EditorOptions")]
	public static void SetupEditorOptions() {
		CPlatformBuilder.EditorInitialize();

		// 에디터 옵션을 설정한다 {
		EditorSettings.enterPlayModeOptionsEnabled = false;
		
		EditorSettings.spritePackerMode = SpritePackerMode.AlwaysOnAtlas;
		EditorSettings.serializationMode = SerializationMode.ForceText;
		EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.None;

		EditorSettings.unityRemoteResolution = KEditorDefine.B_EDITOR_OPTION_REMOTE_RESOLUTION;
		EditorSettings.unityRemoteCompression = KEditorDefine.B_EDITOR_OPTION_REMOTE_COMPRESSION;
		EditorSettings.externalVersionControl = KEditorDefine.B_EDITOR_OPTION_VERSION_CONTROL;
		EditorSettings.unityRemoteJoystickSource = KEditorDefine.B_EDITOR_OPTION_JOYSTIC_SOURCE;
		EditorSettings.projectGenerationRootNamespace = string.Empty;
		EditorSettings.projectGenerationUserExtensions = KEditorDefine.B_EDITOR_OPTION_EXTENSIONS;

		EditorSettings.prefabUIEnvironment = EditorFunc.FindAsset<SceneAsset>(KEditorDefine.B_SCENE_NAME_PATTERN, new string[] {
			KEditorDefine.B_DIR_PATH_SCENES
		});

		EditorSettings.prefabRegularEnvironment = EditorFunc.FindAsset<SceneAsset>(KEditorDefine.B_SCENE_NAME_PATTERN, new string[] {
			KEditorDefine.B_DIR_PATH_SCENES
		});

#if MODE_2D_ENABLE
		EditorSettings.defaultBehaviorMode = EditorBehaviorMode.Mode2D;
#else
		EditorSettings.defaultBehaviorMode = EditorBehaviorMode.Mode3D;
#endif			// #if MODE_2D_ENABLE

#if UNITY_IOS
		EditorSettings.unityRemoteDevice = KEditorDefine.B_EDITOR_OPTION_IOS_REMOTE_DEVICE;
#elif UNITY_ANDROID
		EditorSettings.unityRemoteDevice = KEditorDefine.B_EDITOR_OPTION_ANDROID_REMOTE_DEVICE;
#else
		EditorSettings.unityRemoteDevice = KEditorDefine.B_EDITOR_OPTION_DISABLE_REMOTE_DEVICE;
#endif			// #if UNITY_IOS

		if(CPlatformBuilder.BuildOptionTable != null) {
			EditorSettings.asyncShaderCompilation = CPlatformBuilder.BuildOptionTable.EditorOption.m_bIsAsyncShaderCompile;
			EditorSettings.useLegacyProbeSampleCount = CPlatformBuilder.BuildOptionTable.EditorOption.m_bIsUseLegacyProbeSampleCount;
			EditorSettings.enableTextureStreamingInPlayMode = CPlatformBuilder.BuildOptionTable.EditorOption.m_bIsEnableTextureStreamingInPlayMode;
			EditorSettings.enableTextureStreamingInEditMode = CPlatformBuilder.BuildOptionTable.EditorOption.m_bIsEnableTextureStreamingInEditMode;

			EditorSettings.cacheServerMode = CPlatformBuilder.BuildOptionTable.EditorOption.m_eCacheServerMode;	
			EditorSettings.assetPipelineMode = CPlatformBuilder.BuildOptionTable.EditorOption.m_eAssetPipelineMode;
			EditorSettings.lineEndingsForNewScripts = CPlatformBuilder.BuildOptionTable.EditorOption.m_eLineEndingMode;
			EditorSettings.etcTextureCompressorBehavior = (int)CPlatformBuilder.BuildOptionTable.EditorOption.m_eTextureCompressionType;
		}
		// 에디터 옵션을 설정한다 }

		// 사운드 옵션을 설정한다 {
		var oSndManager = EditorFunc.LoadAsset(KEditorDefine.B_ASSET_PATH_SND_MANAGER);

		if(oSndManager != null && CPlatformBuilder.BuildOptionTable != null) {
			var oConfiguration = AudioSettings.GetConfiguration();
			oConfiguration.sampleRate = CPlatformBuilder.BuildOptionTable.SndOption.m_nSampleRate;
			oConfiguration.numRealVoices = CPlatformBuilder.BuildOptionTable.SndOption.m_nNumRealVoices;
			oConfiguration.numVirtualVoices = CPlatformBuilder.BuildOptionTable.SndOption.m_nNumVirtualVoices;
			oConfiguration.speakerMode = CPlatformBuilder.BuildOptionTable.SndOption.m_eSpeakerMode;
			oConfiguration.dspBufferSize = (int)CPlatformBuilder.BuildOptionTable.SndOption.m_eDSPBufferSize;

			AudioSettings.Reset(oConfiguration);
			var oSerializeObject = new SerializedObject(oSndManager);

			oSerializeObject.ExSetPropertyValue(KEditorDefine.B_PROPERTY_NAME_SND_M_DISABLE_AUDIO, (a_oProperty) => {
				a_oProperty.boolValue = CPlatformBuilder.BuildOptionTable.SndOption.m_bIsDisable;
			});

			oSerializeObject.ExSetPropertyValue(KEditorDefine.B_PROPERTY_NAME_SND_M_VIRTUALIZE_EFFECT, (a_oProperty) => {
				a_oProperty.boolValue = CPlatformBuilder.BuildOptionTable.SndOption.m_bIsVirtualizeEffect;
			});

			oSerializeObject.ExSetPropertyValue(KEditorDefine.B_PROPERTY_NAME_SND_M_GLOBAL_VOLUME, (a_oProperty) => {
				a_oProperty.floatValue = CPlatformBuilder.BuildOptionTable.SndOption.m_fGlobalVolume;
			});

			oSerializeObject.ExSetPropertyValue(KEditorDefine.B_PROPERTY_NAME_SND_M_ROLLOFF_SCALE, (a_oProperty) => {
				a_oProperty.floatValue = CPlatformBuilder.BuildOptionTable.SndOption.m_fRolloffScale;
			});

			oSerializeObject.ExSetPropertyValue(KEditorDefine.B_PROPERTY_NAME_SND_M_DOPPLER_FACTOR, (a_oProperty) => {
				a_oProperty.floatValue = CPlatformBuilder.BuildOptionTable.SndOption.m_fDopplerFactor;
			});
		}
		// 사운드 옵션을 설정한다 }

		// 태그 옵션을 설정한다 {
#if !CUSTOM_TAG_ENABLE || !CUSTOM_SORTING_LAYER_ENABLE
		var oTagManager = EditorFunc.LoadAsset(KEditorDefine.B_ASSET_PATH_TAG_MANAGER);

		if(oTagManager != null) {
			var oSerializeObject = new SerializedObject(oTagManager);

#if !CUSTOM_TAG_ENABLE
			oSerializeObject.ExSetPropertyValue(KEditorDefine.B_PROPERTY_NAME_TAG_M_TAG, (a_oProperty) => {
#if !EXTRA_TAG_ENABLE
				a_oProperty.ClearArray();
#endif			// #if !EXTRA_TAG_ENABLE

				if(a_oProperty.arraySize < KDefine.U_TAGS.Length) {
					for(int i = a_oProperty.arraySize; i < KDefine.U_TAGS.Length; ++i) {
						a_oProperty.InsertArrayElementAtIndex(i);
					}
				}

				Func.Assert(a_oProperty.arraySize >= KDefine.U_TAGS.Length);

				for(int i = 0; i < a_oProperty.arraySize; ++i) {
					var oProperty = a_oProperty.GetArrayElementAtIndex(i);
					oProperty.stringValue = KDefine.U_TAGS[i];
				}
			});
#endif			// #if !CUSTOM_TAG_ENABLE

#if !CUSTOM_SORTING_LAYER_ENABLE
			oSerializeObject.ExSetPropertyValue(KEditorDefine.B_PROPERTY_NAME_TAG_M_SORTING_LAYER, (a_oProperty) => {
#if !EXTRA_SORTING_LAYER_ENABLE
				a_oProperty.ClearArray();
#endif			// #if !EXTRA_TAG_ENABLE

				if(a_oProperty.arraySize < KDefine.U_SORTING_LAYERS.Length) {
					for(int i = a_oProperty.arraySize; i < KDefine.U_SORTING_LAYERS.Length; ++i) {
						a_oProperty.InsertArrayElementAtIndex(i);
					}
				}

				Func.Assert(a_oProperty.arraySize >= KDefine.U_SORTING_LAYERS.Length);

				for(int i = 0; i < a_oProperty.arraySize; ++i) {
					var oEnumerator = a_oProperty.GetArrayElementAtIndex(i).GetEnumerator();

					oEnumerator.ExEnumerate((a_oValue) => {
						var oProperty = (SerializedProperty)a_oValue;
						string oSortingLayer = KDefine.U_SORTING_LAYERS[i];

						if(oProperty.name.ExIsEquals(KEditorDefine.B_PROPERTY_NAME_TAG_M_NAME)) {
							oProperty.stringValue = oSortingLayer;
						} else if(oProperty.name.ExIsEquals(KEditorDefine.B_PROPERTY_NAME_TAG_M_UNIQUE_ID)) {
							oProperty.intValue = oSortingLayer.ExIsEquals(KDefine.U_SORTING_LAYER_DEF) ? 0 : i + 10;
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

		string oSearch = KEditorDefine.DS_DEFINE_SYMBOL_NOT_USE_CUSTOM_PROJECT_OPTION;
		string oReplace = KEditorDefine.DS_DEFINE_SYMBOL_USE_CUSTOM_PROJECT_OPTION;

		for(int i = 0; i < KEditorDefine.B_PATH_SCRIPT_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KEditorDefine.B_PATH_SCRIPT_FILEPATH_INFOS[i];
			Func.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, oSearch, oReplace, oEncoding, false);
		}

		for(int i = 0; i < KEditorDefine.B_PATH_COMPARE_SCRIPT_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KEditorDefine.B_PATH_COMPARE_SCRIPT_FILEPATH_INFOS[i];

			if(!Func.IsEqualsFile(stFilepathInfo.Key, stFilepathInfo.Value, oEncoding, oEncoding)) {
				Func.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value);
			}
		}
		// 스크립트를 복사한다 }

		// 데이터를 복사한다
		for(int i = 0; i < KEditorDefine.B_PATH_DATA_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KEditorDefine.B_PATH_DATA_FILEPATH_INFOS[i];
			Func.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, false);
		}

		// 프리팹을 복사한다
		for(int i = 0; i < KEditorDefine.B_PATH_PREFAB_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KEditorDefine.B_PATH_PREFAB_FILEPATH_INFOS[i];
			Func.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, false);
		}
		
		// 테이블을 복사한다
		for(int i = 0; i < KEditorDefine.B_PATH_TABLE_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KEditorDefine.B_PATH_TABLE_FILEPATH_INFOS[i];
			Func.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, false);
		}

		// 스크립트 객체를 복사한다
		for(int i = 0; i < KEditorDefine.B_PATH_SCRIPTABLE_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KEditorDefine.B_PATH_SCRIPTABLE_FILEPATH_INFOS[i];
			Func.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, false);
		}

		// 씬을 복사한다
		for(int i = 0; i < KEditorDefine.B_PATH_SCENE_FILEPATH_INFOS.Length; ++i) {
			var stFilepathInfo = KEditorDefine.B_PATH_SCENE_FILEPATH_INFOS[i];
			Func.CopyFile(stFilepathInfo.Key, stFilepathInfo.Value, false);
		}
		
		EditorFunc.UpdateAssetDatabaseState();
	}

	//! 전처리기 심볼을 설정한다
	[MenuItem("Utility/Setup/DefineSymbols")]
	public static void SetupDefineSymbols() {
		// 테이블을 제거한다
		Resources.UnloadAsset(CPlatformBuilder.BuildInfoTable);
		Resources.UnloadAsset(CPlatformBuilder.BuildOptionTable);
		Resources.UnloadAsset(CPlatformBuilder.ProjectInfoTable);
		Resources.UnloadAsset(CPlatformBuilder.DefineSymbolTable);

		// 전처리기 심볼을 설정한다 {
		CPlatformBuilder.EditorInitialize();

		if(CPlatformBuilder.DefineSymbolListContainer.ExIsValid()) {
			foreach(var stKeyValue in CPlatformBuilder.DefineSymbolListContainer) {
				CPlatformBuilder.RemoveDefineSymbol(stKeyValue.Key, KEditorDefine.DS_DEFINE_SYMBOL_FPS_ENABLE);
				CPlatformBuilder.RemoveDefineSymbol(stKeyValue.Key, KEditorDefine.DS_DEFINE_SYMBOL_ADS_TEST_ENABLE);
				CPlatformBuilder.RemoveDefineSymbol(stKeyValue.Key, KEditorDefine.DS_DEFINE_SYMBOL_LOGIC_TEST_ENABLE);
				CPlatformBuilder.RemoveDefineSymbol(stKeyValue.Key, KEditorDefine.DS_DEFINE_SYMBOL_RECEIPT_CHECK_ENABLE);

				CPlatformBuilder.RemoveDefineSymbol(stKeyValue.Key, KEditorDefine.DS_DEFINE_SYMBOL_ADHOC_BUILD);
				CPlatformBuilder.RemoveDefineSymbol(stKeyValue.Key, KEditorDefine.DS_DEFINE_SYMBOL_STORE_BUILD);
			}

			EditorFunc.SetupDefineSymbols(CPlatformBuilder.DefineSymbolListContainer);
		}
		// 전처리기 심볼을 설정한다 }
	}

	//! 그래픽 API 를 설정한다
	[MenuItem("Utility/Setup/GraphicAPIs")]
	public static void SetupGraphicAPIs() {
		// 독립 플랫폼 그래픽 API 를 설정한다
		EditorFunc.SetGraphicAPI(BuildTarget.StandaloneOSX, KEditorDefine.B_MAC_GRAPHICS_DEVICE_TYPES);
		EditorFunc.SetGraphicAPI(BuildTarget.StandaloneWindows, KEditorDefine.B_WINDOWS_GRAPHICS_DEVICE_TYPES);
		EditorFunc.SetGraphicAPI(BuildTarget.StandaloneWindows64, KEditorDefine.B_WINDOWS_GRAPHICS_DEVICE_TYPES);

		// iOS 그래픽 API 를 설정한다
		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
		EditorFunc.SetGraphicAPI(BuildTarget.iOS, KEditorDefine.B_IOS_DEVICE_GRAPHICS_DEVICE_TYPES);

		// 안드로이드 그래픽 API 를 설정한다
		EditorFunc.SetGraphicAPI(BuildTarget.Android, KEditorDefine.B_ANDROID_GRAPHICS_DEVICE_TYPES);
	}

		//! 플랫폼 옵션을 설정한다
	private static void SetupPlatformOptions() {
		CPlatformBuilder.EditorInitialize();

		EditorFunc.SetupStandalonePlatform();
		EditorFunc.SetupiOSPlatform();
		EditorFunc.SetupAndroidPlatform();

		// 퀄리티를 설정한다
		Func.SetupQuality(Screen.currentResolution.refreshRate, 
			KAppDefine.G_MULTI_TOUCH_ENABLE, KAppDefine.G_DEF_QUALITY_LEVEL, true);

		// 어플리케이션 식별자를 설정한다
		if(CPlatformBuilder.ProjectInfoTable != null) {
			PlayerSettings.macOS.buildNumber = CPlatformBuilder.ProjectInfoTable.MacProjectInfo.m_oBuildNumber;

			PlayerSettings.iOS.buildNumber = CPlatformBuilder.ProjectInfoTable.iOSProjectInfo.m_oBuildNumber;
			PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, CPlatformBuilder.ProjectInfoTable.iOSProjectInfo.m_oAppID);

			int nBuildNumber = 0;
			bool bIsValidNumber = false;

			if(Application.isBatchMode) {
				if(CPlatformBuilder.StandalonePlatformType == EStandalonePlatformType.WINDOWS) {
					PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, CPlatformBuilder.ProjectInfoTable.WindowsProjectInfo.m_oAppID);
				} else {
					PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, CPlatformBuilder.ProjectInfoTable.MacProjectInfo.m_oAppID);
				}

				if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.ONE_STORE) {
					bIsValidNumber = int.TryParse(CPlatformBuilder.ProjectInfoTable.OneStoreProjectInfo.m_oBuildNumber, out nBuildNumber);
				} else if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.GALAXY_STORE) {
					bIsValidNumber = int.TryParse(CPlatformBuilder.ProjectInfoTable.GalaxyStoreProjectInfo.m_oBuildNumber, out nBuildNumber);
				} else {
					bIsValidNumber = int.TryParse(CPlatformBuilder.ProjectInfoTable.GoogleProjectInfo.m_oBuildNumber, out nBuildNumber);
				}
			} else {
#if WINDOWS_PLATFORM
				PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, CPlatformBuilder.ProjectInfoTable.WindowsProjectInfo.m_oAppID);
#else
				PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, CPlatformBuilder.ProjectInfoTable.MacProjectInfo.m_oAppID);
#endif			// #if WINDOWS_PLATFORM

#if ONE_STORE_PLATFORM
				bIsValidNumber = int.TryParse(CPlatformBuilder.ProjectInfoTable.OneStoreProjectInfo.m_oBuildNumber, out nBuildNumber);
#elif GALAXY_STORE_PLATFORM
				bIsValidNumber = int.TryParse(CPlatformBuilder.ProjectInfoTable.GalaxyStoreProjectInfo.m_oBuildNumber, out nBuildNumber);
#else
				bIsValidNumber = int.TryParse(CPlatformBuilder.ProjectInfoTable.GoogleProjectInfo.m_oBuildNumber, out nBuildNumber);
#endif			// #if ONE_STORE_PLATFORM
			}

			Func.Assert(bIsValidNumber);
			PlayerSettings.Android.bundleVersionCode = nBuildNumber;

			if(Application.isBatchMode) {
				if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.ONE_STORE) {
					PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuilder.ProjectInfoTable.OneStoreProjectInfo.m_oAppID);
				} else if(CPlatformBuilder.AndroidPlatformType == EAndroidPlatformType.GALAXY_STORE) {
					PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuilder.ProjectInfoTable.OneStoreProjectInfo.m_oAppID);
				} else {
					PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuilder.ProjectInfoTable.GoogleProjectInfo.m_oAppID);
				}
			} else {
#if ONE_STORE_PLATFORM
				PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuilder.ProjectInfoTable.OneStoreProjectInfo.m_oAppID);
#elif GALAXY_STORE_PLATFORM
				PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuilder.ProjectInfoTable.GalaxyStoreProjectInfo.m_oAppID);
#else
				PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, CPlatformBuilder.ProjectInfoTable.GoogleProjectInfo.m_oAppID);
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
		if(CPlatformBuilder.BuildInfoTable != null) {
			CExtension.ExSetPropertyValue<PlayerSettings.macOS>(null,
				KEditorDefine.B_PROPERTY_NAME_CATEGORY, BindingFlags.NonPublic | BindingFlags.Static, CPlatformBuilder.BuildInfoTable.StandaloneBuildInfo.m_oCategory);
		}
	}

	//! iOS 플랫폼을 설정한다
	private static void SetupiOSPlatform() {
		if(CPlatformBuilder.BuildInfoTable != null) {
			PlayerSettings.iOS.SetiPadLaunchScreenType(CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_eiPadLaunchScreenType);
			PlayerSettings.iOS.SetiPhoneLaunchScreenType(CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_eiPhoneLaunchScreenType);
		}
	}

	//! 안드로이드 플랫폼을 설정한다
	private static void SetupAndroidPlatform() {
		if(CPlatformBuilder.BuildInfoTable != null) {
			PlayerSettings.Android.useCustomKeystore = true;

			PlayerSettings.Android.keystoreName = CPlatformBuilder.BuildInfoTable.AndroidBuildInfo.m_oKeystorePath;
			PlayerSettings.Android.keyaliasName = CPlatformBuilder.BuildInfoTable.AndroidBuildInfo.m_oKeyaliasName;

			PlayerSettings.Android.keystorePass = CPlatformBuilder.BuildInfoTable.AndroidBuildInfo.m_oKeystorePassword;
			PlayerSettings.Android.keyaliasPass = CPlatformBuilder.BuildInfoTable.AndroidBuildInfo.m_oKeyaliasPassword;
		}
	}

	//! 빌드 옵션을 설정한다
	private static void SetupBuildOptions() {
		CPlatformBuilder.EditorInitialize();

		EditorFunc.SetupStandaloneBuildOptions();
		EditorFunc.SetupiOSBuildOptions();
		EditorFunc.SetupAndroidBuildOptions();
		
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

		if(CPlatformBuilder.ProjectInfoTable != null) {
			PlayerSettings.companyName = CPlatformBuilder.ProjectInfoTable.CompanyName;
			PlayerSettings.productName = CPlatformBuilder.ProjectInfoTable.ProductName;
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
		PlayerSettings.SplashScreen.backgroundColor = KEditorDefine.B_COLOR_UNITY_LOGO_BG;
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
		CExtension.ExCallFunc<EditorUserBuildSettings>(null, KEditorDefine.B_FUNC_NAME_SET_COMPRESSION_TYPE, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
			BuildTargetGroup.Standalone, (int)CompressionType.None
		});

		CExtension.ExCallFunc<EditorUserBuildSettings>(null, KEditorDefine.B_FUNC_NAME_SET_COMPRESSION_TYPE, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
			BuildTargetGroup.iOS, (int)CompressionType.None
		});

		CExtension.ExCallFunc<EditorUserBuildSettings>(null, KEditorDefine.B_FUNC_NAME_SET_COMPRESSION_TYPE, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
			BuildTargetGroup.Android, (int)CompressionType.None
		});
		// 리소스 압축 방식을 변경한다 }

		if(CPlatformBuilder.BuildOptionTable != null) {
			PlayerSettings.gpuSkinning = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsGPUSkinning;
			PlayerSettings.MTRendering = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsMTRendering;
			PlayerSettings.runInBackground = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsRunInBackground;
			PlayerSettings.bakeCollisionMeshes = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsPreBakeCollisionMesh;
			PlayerSettings.use32BitDisplayBuffer = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsUse32BitDisplayBuffer;
			PlayerSettings.muteOtherAudioSources = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsMuteOtherAudioSource;
			PlayerSettings.enableFrameTimingStats = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsEnableFrameTimingStats;
			PlayerSettings.enableInternalProfiler = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsEnableInternalProfiler;
			PlayerSettings.preserveFramebufferAlpha = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsPreserveFrameBufferAlpha;
			PlayerSettings.vulkanEnableSetSRGBWrite = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsEnableVulkanSRGBWrite;

			PlayerSettings.aotOptions = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_oAOTCompileOption;
			PlayerSettings.accelerometerFrequency = (int)CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_eAccelerometerFrequency;
			PlayerSettings.vulkanNumSwapchainBuffers = CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_nNumVulkanSwapChainBuffers;

			// 기타 옵션을 설정한다
			PlayerSettings.SetPreloadedAssets(CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_oPreloadAssetList?.ToArray());

			// 멀티 쓰레드 렌더링을 설정한다 {
			PlayerSettings.SetMobileMTRendering(BuildTargetGroup.iOS, 
				CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsMTRendering);
			
			PlayerSettings.SetMobileMTRendering(BuildTargetGroup.Android, 
				CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsMTRendering);
			// 멀티 쓰레드 렌더링을 설정한다 }

			// 광원 맵 엔코딩 퀄리티를 설정한다 {
			CExtension.ExCallFunc<PlayerSettings>(null, KEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_ENCODING_QUALITY, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
				BuildTargetGroup.Standalone, (int)CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_eLightmapEncodingQuality
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_ENCODING_QUALITY, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
				BuildTargetGroup.iOS, (int)CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_eLightmapEncodingQuality
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_ENCODING_QUALITY, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
				BuildTargetGroup.Android, (int)CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_eLightmapEncodingQuality
			});
			// 광원 맵 엔코딩 퀄리티를 설정한다 }

			// 광원 맵 스트리밍 여부를 설정한다 {
			CExtension.ExCallFunc<PlayerSettings>(null, KEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_ENABLE, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
				BuildTargetGroup.Standalone, CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsEnableLightmapStreaming
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_ENABLE, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
				BuildTargetGroup.iOS, CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsEnableLightmapStreaming
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_ENABLE, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
				BuildTargetGroup.Android, CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_bIsEnableLightmapStreaming
			});
			// 광원 맵 스트리밍 여부를 설정한다 }

			// 광원 맵 스트리밍 우선 순위를 설정한다 {
			CExtension.ExCallFunc<PlayerSettings>(null, KEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_PRIORITY, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
				BuildTargetGroup.Standalone, CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_nLightmapStreamingPriority
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_PRIORITY, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
				BuildTargetGroup.iOS, CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_nLightmapStreamingPriority
			});

			CExtension.ExCallFunc<PlayerSettings>(null, KEditorDefine.B_FUNC_NAME_SET_LIGHTMAP_STREAMING_PRIORITY, BindingFlags.NonPublic | BindingFlags.Static, new object[] {
				BuildTargetGroup.Android, CPlatformBuilder.BuildOptionTable.CommonBuildOption.m_nLightmapStreamingPriority
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

		PlayerSettings.defaultScreenWidth = KDefine.B_DESKTOP_WINDOW_WIDTH;
		PlayerSettings.defaultScreenHeight = KDefine.B_DESKTOP_WINDOW_HEIGHT;

		PlayerSettings.SetAspectRatio(AspectRatio.Aspect4by3, true);
		PlayerSettings.SetAspectRatio(AspectRatio.Aspect5by4, true);
		PlayerSettings.SetAspectRatio(AspectRatio.Aspect16by9, true);
		PlayerSettings.SetAspectRatio(AspectRatio.Aspect16by10, true);
		PlayerSettings.SetAspectRatio(AspectRatio.AspectOthers, true);

		if(CPlatformBuilder.BuildOptionTable != null) {
			PlayerSettings.fullScreenMode = CPlatformBuilder.BuildOptionTable.StandaloneBuildOption.m_eFullscreenMode;
			PlayerSettings.forceSingleInstance = CPlatformBuilder.BuildOptionTable.StandaloneBuildOption.m_bIsForceSingleInstance;
			PlayerSettings.captureSingleScreen = CPlatformBuilder.BuildOptionTable.StandaloneBuildOption.m_bIsCaptureSingleScreen;
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
		PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, (int)AppleMobileArchitecture.ARM64);

		if(CPlatformBuilder.BuildInfoTable != null) {
			PlayerSettings.iOS.appleDeveloperTeamID = CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oTeamID;
			PlayerSettings.iOS.targetOSVersionString = CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oTargetOSVersion;
			PlayerSettings.iOS.iOSUrlSchemes = CPlatformBuilder.BuildInfoTable.iOSBuildInfo.m_oURLSchemeList?.ToArray();
		}

		if(CPlatformBuilder.BuildOptionTable != null) {
			PlayerSettings.iOS.targetDevice = CPlatformBuilder.BuildOptionTable.iOSBuildOption.m_eTargetDevice;
			PlayerSettings.iOS.statusBarStyle = CPlatformBuilder.BuildOptionTable.iOSBuildOption.m_eStatusBarStyle;
			PlayerSettings.iOS.requiresPersistentWiFi = CPlatformBuilder.BuildOptionTable.iOSBuildOption.m_bIsRequirePersistentWIFI;
			PlayerSettings.iOS.appInBackgroundBehavior = CPlatformBuilder.BuildOptionTable.iOSBuildOption.m_eBackgroundBehavior;

			PlayerSettings.iOS.cameraUsageDescription = CPlatformBuilder.BuildOptionTable.iOSBuildOption.m_oCameraDescription;
			PlayerSettings.iOS.locationUsageDescription = CPlatformBuilder.BuildOptionTable.iOSBuildOption.m_oLocationDescription;
			PlayerSettings.iOS.microphoneUsageDescription = CPlatformBuilder.BuildOptionTable.iOSBuildOption.m_oMicrophoneDescription;

			CExtension.ExSetPropertyValue<PlayerSettings.iOS>(null,
				KEditorDefine.B_PROPERTY_NAME_APPLE_ENABLE_PRO_MOTION, BindingFlags.NonPublic | BindingFlags.Static, CPlatformBuilder.BuildOptionTable.iOSBuildOption.m_bIsEnableProMotion);

			CExtension.ExSetPropertyValue<PlayerSettings.iOS>(null,
				KEditorDefine.B_PROPERTY_NAME_REQUIRE_AR_KIT_SUPPORT, BindingFlags.NonPublic | BindingFlags.Static, CPlatformBuilder.BuildOptionTable.iOSBuildOption.m_bIsRequreARKitSupport);

			CExtension.ExSetPropertyValue<PlayerSettings.iOS>(null,
				KEditorDefine.B_PROPERTY_NAME_AUTO_ADD_CAPABILITIES, BindingFlags.NonPublic | BindingFlags.Static, CPlatformBuilder.BuildOptionTable.iOSBuildOption.m_bIsAutoAddCapabilities);
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

		if(CPlatformBuilder.BuildInfoTable != null) {
			PlayerSettings.Android.minSdkVersion = CPlatformBuilder.BuildInfoTable.AndroidBuildInfo.m_eMinSDKVersion;
			PlayerSettings.Android.targetSdkVersion = CPlatformBuilder.BuildInfoTable.AndroidBuildInfo.m_eTargetSDKVersion;
		}

		CExtension.ExSetPropertyValue<PlayerSettings.Android>(null,
			KEditorDefine.B_PROPERTY_NAME_VALIDATE_APP_BUNDLE_SIZE, BindingFlags.NonPublic | BindingFlags.Static, true);

		if(CPlatformBuilder.BuildOptionTable != null) {
			PlayerSettings.Android.blitType = CPlatformBuilder.BuildOptionTable.AndroidBuildOption.m_eBlitType;
			PlayerSettings.Android.androidTVCompatibility = CPlatformBuilder.BuildOptionTable.AndroidBuildOption.m_bIsTVCompatibility;
			PlayerSettings.Android.preferredInstallLocation = CPlatformBuilder.BuildOptionTable.AndroidBuildOption.m_ePreferredInstallLocation;

			CExtension.ExSetPropertyValue<PlayerSettings.Android>(null,
				KEditorDefine.B_PROPERTY_NAME_OPTIMIZE_FRAME_PACING, BindingFlags.NonPublic | BindingFlags.Static, CPlatformBuilder.BuildOptionTable.AndroidBuildOption.m_bIsOptimizeFramePacing);

			CExtension.ExSetPropertyValue<PlayerSettings.Android>(null,
				KEditorDefine.B_PROPERTY_NAME_APP_BUNDLE_SIZE_TO_VALIDATE, BindingFlags.NonPublic | BindingFlags.Static, CPlatformBuilder.BuildOptionTable.AndroidBuildOption.m_nAppBundleSize);

			CExtension.ExSetPropertyValue<PlayerSettings.Android>(null,
				KEditorDefine.B_PROPERTY_NAME_SUPPORTED_ASPECT_RATIO_MODE, BindingFlags.NonPublic | BindingFlags.Static, (int)CPlatformBuilder.BuildOptionTable.AndroidBuildOption.m_eAspectRatioMode);
		}
	}
	#endregion			// 클래스 함수
}
