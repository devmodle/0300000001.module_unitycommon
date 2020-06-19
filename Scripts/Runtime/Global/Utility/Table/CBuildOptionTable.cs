using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif			// #if UNITY_EDITOR

//! 에디터 옵션
[System.Serializable]
public struct STEditorOption {
	public bool m_bIsAsyncShaderCompile;
	public bool m_bIsUseLegacyProbeSampleCount;
	public bool m_bIsEnableTextureStreamingInPlayMode;
	public bool m_bIsEnableTextureStreamingInEditMode;

#if UNITY_EDITOR
	public CacheServerMode m_eCacheServerMode;
	public AssetPipelineMode m_eAssetPipelineMode;
	public LineEndingsMode m_eLineEndingMode;
	public ETextureCompressionType m_eTextureCompressionType;
#endif			// #if UNITY_EDITOR
}

//! 사운드 옵션
[System.Serializable]
public struct STSoundOption {
	public bool m_bIsDisable;
	public bool m_bIsVirtualizeEffect;

	public int m_nSampleRate;
	public int m_nNumRealVoices;
	public int m_nNumVirtualVoices;

	public float m_fGlobalVolume;
	public float m_fRolloffScale;
	public float m_fDopplerFactor;

	public AudioSpeakerMode m_eSpeakerMode;

#if UNITY_EDITOR
	public EDSPBufferSize m_eDSPBufferSize;
#endif			// #if UNITY_EDITOR
}

//! 공용 빌드 옵션
[System.Serializable]
public struct STCommonBuildOption {
	public bool m_bIsGPUSkinning;
	public bool m_bIsMTRendering;
	public bool m_bIsRunInBackground;
	public bool m_bIsPreBakeCollisionMesh;
	public bool m_bIsUse32BitDisplayBuffer;
	public bool m_bIsMuteOtherAudioSource;
	public bool m_bIsEnableFrameTimingStats;
	public bool m_bIsEnableInternalProfiler;
	public bool m_bIsPreserveFrameBufferAlpha;
	public bool m_bIsEnableVulkanSRGBWrite;
	public bool m_bIsEnableLightmapStreaming;
	
	public int m_nLightmapStreamingPriority;
	public uint m_nNumVulkanSwapChainBuffers;

	public string m_oAOTCompileOption;
	public List<Object> m_oPreloadAssetList;

#if UNITY_EDITOR
	public EAccelerometerFrequency m_eAccelerometerFrequency;
	public ELightmapEncodingQuality m_eLightmapEncodingQuality;
#endif			// #if UNITY_EDITOR
}

//! 독립 플랫폼 빌드 옵션
[System.Serializable]
public struct STStandaloneBuildOption {
	public bool m_bIsForceSingleInstance;
	public bool m_bIsCaptureSingleScreen;

	public FullScreenMode m_eFullscreenMode;
}

//! iOS 빌드 옵션
[System.Serializable]
public struct STiOSBuildOption {
	public bool m_bIsEnableProMotion;
	public bool m_bIsRequreARKitSupport;
	public bool m_bIsRequirePersistentWIFI;
	public bool m_bIsAutoAddCapabilities;

	public string m_oCameraDescription;
	public string m_oLocationDescription;
	public string m_oMicrophoneDescription;
	
#if UNITY_EDITOR
	public iOSTargetDevice m_eTargetDevice;
	public iOSStatusBarStyle m_eStatusBarStyle;
	public iOSAppInBackgroundBehavior m_eBackgroundBehavior;
#endif			// #if UNITY_EDITOR
}

//! 안드로이드 빌드 옵션
[System.Serializable]
public struct STAndroidBuildOption {
	public bool m_bIsTVCompatibility;
	public bool m_bIsOptimizeFramePacing;

	public int m_nAppBundleSize;

#if UNITY_EDITOR
	public AndroidBlitType m_eBlitType;
	public EAspectRatioMode m_eAspectRatioMode;
	public AndroidPreferredInstallLocation m_ePreferredInstallLocation;
#endif			// #if UNITY_EDITOR
}

//! 빌드 옵션 테이블
public class CBuildOptionTable : CScriptableObject<CBuildOptionTable> {
	#region 변수
	[Header("Editor Option")]
	[SerializeField] private STEditorOption m_stEditorOption;

	[Header("Sound Option")]
	[SerializeField] private STSoundOption m_stSoundOption;

	[Header("Build Option")]
	[SerializeField] private STCommonBuildOption m_stCommonBuildOption;
	[SerializeField] private STStandaloneBuildOption m_stStandaloneBuildOption;
	[SerializeField] private STiOSBuildOption m_stiOSBuildOption;
	[SerializeField] private STAndroidBuildOption m_stAndroidBuildOption;
	#endregion			// 변수

	#region 프로퍼티
	public STEditorOption EditorOption => m_stEditorOption;
	public STSoundOption SoundOption => m_stSoundOption;

	public STCommonBuildOption CommonBuildOption => m_stCommonBuildOption;
	public STStandaloneBuildOption StandaloneBuildOption => m_stStandaloneBuildOption;
	public STiOSBuildOption iOSBuildOption => m_stiOSBuildOption;
	public STAndroidBuildOption AndroidBuildOption => m_stAndroidBuildOption;
	#endregion			// 프로퍼티

	#region 조건부 함수
#if UNITY_EDITOR
	//! 에디터 옵션을 변경한다
	public void SetEditorOption(STEditorOption a_stEditorOption) {
		m_stEditorOption = a_stEditorOption;
	}

	//! 사운드 옵션을 변경한다
	public void SetSoundOption(STSoundOption a_stSoundOption) {
		m_stSoundOption = a_stSoundOption;
	}

	//! 공용 빌드 옵션 변경한다
	public void SetCommonBuildOption(STCommonBuildOption a_stBuildOption) {
		m_stCommonBuildOption = a_stBuildOption;
	}

	//! 독립 플랫폼 빌드 옵션을 변경한다
	public void SetStandaloneBuildOption(STStandaloneBuildOption a_stBuildOption) {
		m_stStandaloneBuildOption = a_stBuildOption;
	}

	//! iOS 빌드 옵션을 변경한다
	public void SetiOSBuildOption(STiOSBuildOption a_stBuildOption) {
		m_stiOSBuildOption = a_stBuildOption;
	}

	//! 안드로이드 빌드 옵션을 변경한다
	public void SetAndroidBuildOption(STAndroidBuildOption a_stBuildOption) {
		m_stAndroidBuildOption = a_stBuildOption;
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
