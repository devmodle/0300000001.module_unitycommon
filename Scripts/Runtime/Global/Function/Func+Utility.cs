using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif			// #if UNITY_EDITOR

#if UNITY_IOS
using UnityEngine.iOS;
#endif			// #if UNITY_IOS

#if UNIVERSAL_RENDER_PIPELINE_ENABLE
using UnityEngine.Rendering.Universal;
#endif			// #if UNIVERSAL_RENDER_PIPELINE_ENABLE

//! 유틸리티 함수
public static partial class Func {
	#region 클래스 함수
	//! 약관 동의 필요 여부를 검사한다
	public static bool IsNeedAgreement(string a_oCountryCode) {
		string oCountryCode = a_oCountryCode.ToUpper();
		return oCountryCode.ExIsEuropeanUnion() || oCountryCode.ExIsEquals(KDefine.B_KOREA_COUNTRY_CODE);
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdate(string a_oLatestVersion) {
#if UNITY_ANDROID
		return Func.IsNeedUpdateByBuildNumber(a_oLatestVersion);
#else
		return Func.IsNeedUpdateByBuildVersion(a_oLatestVersion);
#endif			// #if UNITY_ANDROID
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdate(string a_oLatestBuildNumber, string a_oLatestBuildVersion) {
		bool bIsNeedUpdate = Func.IsNeedUpdateByBuildNumber(a_oLatestBuildNumber);
		return bIsNeedUpdate || Func.IsNeedUpdateByBuildVersion(a_oLatestBuildVersion);
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdateByBuildNumber(string a_oLatestNumber) {
		Func.Assert(a_oLatestNumber.ExIsValid());

		bool bIsValidNumberA = int.TryParse(a_oLatestNumber, out int nBuildNumberA);
		bool bIsValidNumberB = int.TryParse(CProjectInfoTable.Instance.ProjectInfo.m_oBuildNumber, out int nBuildNumberB);

		Func.Assert(bIsValidNumberA && bIsValidNumberB);
		return nBuildNumberA > nBuildNumberB;
	}

	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdateByBuildVersion(string a_oLatestVersion) {
		Func.Assert(a_oLatestVersion.ExIsValid());

		var bIsValidVersionA = System.Version.TryParse(a_oLatestVersion, out System.Version oVersionA);
		var bIsValidVersionB = System.Version.TryParse(CProjectInfoTable.Instance.ProjectInfo.m_oBuildVersion, out System.Version oVersionB);

		Func.Assert(bIsValidVersionA && bIsValidVersionB);
		return oVersionA.CompareTo(oVersionB) >= KDefine.B_COMPARE_RESULT_GREATE;
	}

	//! 안전 영역을 반환한다
	public static Rect GetSafeArea(bool a_bIsRuntime = true) {
		if(a_bIsRuntime) {
			return Screen.safeArea;
		}

		return new Rect(0.0f, 0.0f, Camera.main.pixelWidth, Camera.main.pixelHeight);
	}

	//! 디바이스 화면 크기를 반환한다
	public static Vector2 GetDeviceScreenSize(bool a_bIsRuntime = true) {
		return new Vector2(a_bIsRuntime ? Screen.width : Camera.main.pixelWidth,
			a_bIsRuntime ? Screen.height : Camera.main.pixelHeight);
	}

	//! 해상도를 반환한다
	public static Vector2 GetResolution(bool a_bIsRuntime = true) {
		float fScale = Func.GetResolutionScale(a_bIsRuntime);
		return new Vector2(KDefine.B_SCREEN_WIDTH, KDefine.B_SCREEN_HEIGHT) * fScale;
	}

	//! 해상도 비율을 반환한다
	public static float GetResolutionScale(bool a_bIsRuntime = true) {
		float fScale = 1.0f;
		float fAspect = KDefine.B_SCREEN_WIDTH / (float)KDefine.B_SCREEN_HEIGHT;

		float fScreenWidth = Func.GetDeviceScreenSize(a_bIsRuntime).x;
		float fScreenHeight = Func.GetDeviceScreenSize(a_bIsRuntime).y;

		// 화면을 벗어났을 경우
		if(fScreenWidth.ExIsLess(fScreenHeight * fAspect)) {
			fScale = fScreenWidth / (fScreenHeight * fAspect);
		}
		
		return fScale;
	}

	//! 왼쪽 화면 비율을 반환한다
	public static float GetLeftScreenScale(bool a_bIsRuntime = true) {
		var stSafeArea = Func.GetSafeArea(a_bIsRuntime);
		return stSafeArea.x / Func.GetDeviceScreenSize(a_bIsRuntime).x;
	}

	//! 오른쪽 화면 비율을 반환한다
	public static float GetRightScreenScale(bool a_bIsRuntime = true) {
		var stSafeArea = Func.GetSafeArea(a_bIsRuntime);
		float fScreenWidth = Func.GetDeviceScreenSize(a_bIsRuntime).x;

		return (fScreenWidth - (stSafeArea.x + stSafeArea.width)) / fScreenWidth;
	}

	//! 상단 화면 비율을 반환한다
	public static float GetTopScreenScale(bool a_bIsRuntime = true) {
		var stSafeArea = Func.GetSafeArea(a_bIsRuntime);
		float fScreenHeight = Func.GetDeviceScreenSize(a_bIsRuntime).y;

		return (fScreenHeight - (stSafeArea.y + stSafeArea.height)) / fScreenHeight;
	}

	//! 하단 화면 비율을 반환한다
	public static float GetBottomScreenScale(bool a_bIsRuntime = true) {
		var stSafeArea = Func.GetSafeArea(a_bIsRuntime);
		return stSafeArea.y / Func.GetDeviceScreenSize(a_bIsRuntime).y;
	}
	
	//! 메세지를 전송한다
	public static void SendMessage(string a_oName, string a_oMessage, object a_oParams) {
		var oGameObject = Func.FindGameObject(a_oName);
		oGameObject?.SendMessage(a_oMessage, a_oParams, SendMessageOptions.DontRequireReceiver);
	}

	//! 메세지를 전파한다
	public static void BroadcastMessage(string a_oMessage, object a_oParams) {
		for(int i = 0; i < SceneManager.sceneCount; ++i) {
			var stScene = SceneManager.GetSceneAt(i);
			stScene.ExBroadcastMessage(a_oMessage, a_oParams);
		}
	}

	//! 퀄리티를 설정한다
	public static void SetupQuality(int a_nTargetFrameRate, bool a_bIsEnableMultiTouch, EQualityLevel a_eQualityLevel = EQualityLevel.AUTO, bool a_bIsApplyExpensiveChange = false) {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Input.multiTouchEnabled = a_bIsEnableMultiTouch;
		Application.targetFrameRate = Mathf.Min(a_nTargetFrameRate, Screen.currentResolution.refreshRate);

		// 퀄리티 레벨을 설정한다 {
		var eQualityLevel = a_eQualityLevel;

		if(a_eQualityLevel == EQualityLevel.AUTO) {
#if ULTRA_QUALITY_LEVEL_ENABLE
			eQualityLevel = EQualityLevel.ULTRA;
#else
			eQualityLevel = EQualityLevel.VERY_LOW;
#endif			// #if ULTRA_QUALITY_LEVEL_ENABLE
		}

#if UNITY_EDITOR
		QualitySettings.antiAliasing = KDefine.U_QUALITY_ANTI_ALIASING;
		QualitySettings.maximumLODLevel = KDefine.U_QUALITY_MAX_LOD_LEVEL;
		QualitySettings.asyncUploadTimeSlice = KDefine.U_QUALITY_ASYNC_UPLOAD_TIME_SLICE;
		QualitySettings.asyncUploadBufferSize = KDefine.U_QUALITY_ASYNC_UPLOAD_BUFFER_SIZE;
		QualitySettings.asyncUploadPersistentBuffer = KDefine.U_QUALITY_ASYNC_UPLOAD_PERSISTENT_BUFFER;
		QualitySettings.resolutionScalingFixedDPIFactor = KDefine.U_QUALITY_RESOLUTION_SCALE_FIXED_DPI_FACTOR;

		QualitySettings.vSyncCount = (eQualityLevel >= EQualityLevel.HIGH) ? (int)EVSyncType.EVERY : (int)EVSyncType.NEVER;
		QualitySettings.anisotropicFiltering = (eQualityLevel >= EQualityLevel.HIGH) ? AnisotropicFiltering.Enable : AnisotropicFiltering.Disable;
#endif			// #if UNITY_EDITOR

		QualitySettings.SetQualityLevel((int)eQualityLevel, a_bIsApplyExpensiveChange);
		// 퀄리티 레벨을 설정한다 }

#if UNITY_EDITOR
		// 렌더링 파이프라인을 설정한다 {			
#if UNIVERSAL_RENDER_PIPELINE_ENABLE
		var oRenderPipeline = Resources.Load<UniversalRenderPipelineAsset>(KDefine.U_PIPELINE_PATH_UNIVERSAL_RENDER_PIPELINE);

		if(oRenderPipeline != null) {
			oRenderPipeline.supportsHDR = false;

			oRenderPipeline.shaderVariantLogLevel = ShaderVariantLogLevel.Disabled;
			oRenderPipeline.colorGradingMode = ColorGradingMode.LowDynamicRange;
			oRenderPipeline.shadowCascadeOption = ShadowCascadesOption.NoCascades;

			oRenderPipeline.renderScale = KDefine.U_SCALE_UNIVERSAL_RP_RENDERING;
			oRenderPipeline.colorGradingLutSize = KDefine.U_SIZE_UNIVERSAL_RP_COLOR_GRADING_LUT;

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_ANTI_ALIASING, 
				MsaaQuality.Disabled);

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_DEBUG_LEVEL, 
				PipelineDebugLevel.Disabled);

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_ADDITIONAL_LIGHT_PER_OBJECT_LIMIT, 
				KDefine.U_MAX_NUM_UNIVERSAL_RP_ADDITIONAL_LIGHT_PER_OBJECT);

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_MAIN_LIGHT_SHADOW_MAP_RESOLUTION, 
				UnityEngine.Rendering.Universal.ShadowResolution._2048);

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_ADDITIONAL_LIGHT_SHADOW_MAP_RESOLUTION, 
				UnityEngine.Rendering.Universal.ShadowResolution._512);

#if DYNAMIC_BATCHING_ENABLE
			oRenderPipeline.useSRPBatcher = true;
			oRenderPipeline.supportsDynamicBatching = true;
#else
			oRenderPipeline.useSRPBatcher = false;
			oRenderPipeline.supportsDynamicBatching = false;
#endif			// #if DYNAMIC_BATCHING_ENABLE

#if LIGHT_ENABLE
			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_SUPPORT_MIXED_LIGHTING, 
				true);

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_MAIN_LIGHT_RENDERING_MODE, 
				LightRenderingMode.PerPixel);

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_ADDITIONAL_LIGHT_RENDERING_MODE, 
				LightRenderingMode.PerPixel);
#else
			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_SUPPORT_MIXED_LIGHTING, 
				false);

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_MAIN_LIGHT_RENDERING_MODE, 
				LightRenderingMode.Disabled);

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_ADDITIONAL_LIGHT_RENDERING_MODE, 
				LightRenderingMode.Disabled);
#endif			// #if LIGHT_ENABLE

#if LIGHT_ENABLE && SHADOW_ENABLE
			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_MAIN_LIGHT_SUPPORT_SHADOW, 
				true);

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_ADDITIONAL_LIGHT_SUPPORT_SHADOW, 
				true);

#if SOFT_SHADOW_ENABLE
			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_SUPPORT_SOFT_SHADOW, 
				true);
#else
			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_SUPPORT_SOFT_SHADOW, 
				false);
#endif			// #if SOFT_SHADOW_ENABLE
#else
			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_MAIN_LIGHT_SUPPORT_SHADOW, 
				false);

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_ADDITIONAL_LIGHT_SUPPORT_SHADOW, 
				false);

			oRenderPipeline.ExSetRuntimeFieldValue<UniversalRenderPipelineAsset>(KDefine.U_FIELD_NAME_UNIVERSAL_RP_SUPPORT_SOFT_SHADOW, 
				false);
#endif			// #if LIGHT_ENABLE && SHADOW_ENABLE
		}

		QualitySettings.renderPipeline = oRenderPipeline;
		GraphicsSettings.renderPipelineAsset = oRenderPipeline;
#else
		QualitySettings.renderPipeline = null;
		GraphicsSettings.renderPipelineAsset = null;
#endif			// #if UNIVERSAL_RENDER_PIPELINE_ENABLE
		// 렌더링 파이프라인을 설정한다 }
#endif			// #if UNITY_EDITOR
	}
	
	//! 스크린 UI 를 설정한다
	public static void SetupScreenUI(GameObject a_oScreenUI, int a_nSortingOrder) {
		var oCanvas = a_oScreenUI.GetComponentInChildren<Canvas>();
		oCanvas.sortingOrder = a_nSortingOrder;
		oCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

		var oCanvasScaler = a_oScreenUI.GetComponentInChildren<CanvasScaler>();
		oCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		oCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
		oCanvasScaler.referenceResolution = new Vector2(KDefine.B_SCREEN_WIDTH, KDefine.B_SCREEN_HEIGHT);
		oCanvasScaler.referencePixelsPerUnit = KDefine.B_REF_PIXELS_UNIT;

#if PIXEL_PERFECT_ENABLE
		oCanvas.pixelPerfect = true;
#else
		oCanvas.pixelPerfect = false;
#endif			// #if PIXEL_PERFECT_ENABLE
	}

	//! 객체를 탐색한다
	public static GameObject FindGameObject(string a_oName) {
		Func.Assert(a_oName.ExIsValid());
		return GameObject.Find(a_oName);
	}

	//! 추가 씬을 로드한다
	public static void LoadAdditiveScene(string a_oName, bool a_bIsStartActivityIndicator = false) {
		Func.Assert(a_oName.ExIsValid());
		CSceneLoader.Instance.LoadScene(a_oName, a_bIsStartActivityIndicator, false, false, LoadSceneMode.Additive);
	}

	//! 추가 씬을 로드한다
	public static void LoadAdditiveScenes(string[] a_oNames, bool a_bIsStartActivityIndicator = false) {
		Func.Assert(a_oNames.ExIsValid());

		for(int i = 0; i < a_oNames.Length; ++i) {
			Func.LoadAdditiveScene(a_oNames[i], a_bIsStartActivityIndicator);
		}
	}

	//! 추가 씬을 로드한다
	public static void LoadAdditiveScenes(List<string> a_oNameList, bool a_bIsStartActivityIndicator = false) {
		Func.Assert(a_oNameList.ExIsValid());

		for(int i = 0; i < a_oNameList.Count; ++i) {
			Func.LoadAdditiveScene(a_oNameList[i], a_bIsStartActivityIndicator);
		}
	}

	//! 객체를 생선한다
	public static GameObject CreateGameObject(string a_oName, 
		GameObject a_oParent, bool a_bIsStayWorldState = false) {
		var oGameObject = new GameObject(a_oName);
		oGameObject.transform.SetParent(a_oParent?.transform, a_bIsStayWorldState);

		return oGameObject;
	}

	//! 사본 객체를 생성한다
	public static GameObject CreateCloneGameObject(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, bool a_bIsStayWorldState = false) {
		Func.Assert(a_oOrigin != null);

		var oGameObject = Object.Instantiate(a_oOrigin, a_oOrigin.transform.position, a_oOrigin.transform.rotation);
		oGameObject.name = a_oName;
		oGameObject.transform.localScale = a_oOrigin.transform.localScale;

		oGameObject.transform.SetParent(a_oParent?.transform, a_bIsStayWorldState);
		return oGameObject;
	}

	//! 터치 응답자를 생성한다
	public static GameObject CreateTouchResponder(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, Vector2 a_stSize, Vector2 a_stPos, Color a_stColor) {
		var oGameObject = Func.CreateCloneGameObject(a_oName, a_oOrigin, a_oParent);
		var oImage = oGameObject.GetComponentInChildren<Image>();

		Func.Assert(oImage != null);

		var oTransform = oGameObject.transform as RectTransform;
		oTransform.sizeDelta = a_stSize;
		oTransform.anchoredPosition = a_stPos;

		oImage.color = a_stColor;
		return oGameObject;
	}
	
	//! 객체 풀을 생성한다
	public static ObjectPool CreateObjectPool(GameObject a_oOrigin, GameObject a_oParent, int a_nNumObjects = 0) {
		Func.Assert(a_oOrigin != null);
		return new ObjectPool(a_oOrigin, a_oParent?.transform, a_nNumObjects);
	}
	#endregion			// 클래스 함수

	#region 제네릭 클래스 함수
	//! 컴포넌트를 탐색한다
	public static T FindComponent<T>(string a_oName) where T : Component {
		var oGameObject = Func.FindGameObject(a_oName);
		return oGameObject?.GetComponentInChildren<T>();
	}

	//! 컴포넌트를 탐색한다
	public static T[] FindComponents<T>(string a_oName) where T : Component {
		var oGameObject = Func.FindGameObject(a_oName);
		return oGameObject?.GetComponentsInChildren<T>();
	}

	//! 객체를 생선한다
	public static T CreateGameObject<T>(string a_oName,
		GameObject a_oParent, bool a_bIsStayWorldState = false) where T : Component {
		var oGameObject = Func.CreateGameObject(a_oName, a_oParent, a_bIsStayWorldState);
		return oGameObject.ExAddComponent<T>();
	}

	//! 객체 사본을 생성한다
	public static T CreateCloneGameObject<T>(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, bool a_bIsStayWorldState = false) where T : Component {
		var oGameObject = Func.CreateCloneGameObject(a_oName, a_oOrigin, a_oParent, a_bIsStayWorldState);
		return oGameObject?.GetComponentInChildren<T>();
	}

	//! 팝업을 생성한다
	public static T CreatePopup<T>(string a_oName, 
		GameObject a_oOrigin, GameObject a_oParent, Vector2 a_stPos) where T : CPopup {
		var oPopup = Func.CreateCloneGameObject<T>(a_oName, a_oOrigin, a_oParent);

		if(oPopup != null) {
			oPopup.m_oRectTransform.anchoredPosition = a_stPos;
		}

		return oPopup;
	}

	//! 알림 팝업을 생성한다
	public static T CreateAlertPopup<T>(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, Dictionary<string, string> a_oDataList, System.Action<CAlertPopup, bool> a_oCallback) where T : CAlertPopup {
		var oAlertPopup = Func.CreatePopup<T>(a_oName, a_oOrigin, a_oParent, KDefine.B_POS_MIDDLE_CENTER);
		oAlertPopup?.Init(a_oDataList, a_oCallback);

		return oAlertPopup;
	}

	//! 토스트 팝업을 생성한다
	public static T CreateToastPopup<T>(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, string a_oMessage, float a_fDuration) where T : CToastPopup {
		var oToastPopup = Func.CreatePopup<T>(a_oName, a_oOrigin, a_oParent, KDefine.B_POS_MIDDLE_CENTER);
		oToastPopup?.Init(a_oMessage, a_fDuration);

		return oToastPopup;
	}

	//! 터치 응답자를 생성한다
	public static T CreateTouchResponder<T>(string a_oName,
		GameObject a_oOrigin, GameObject a_oParent, Vector2 a_stSize, Vector2 a_stPos, Color a_stColor) where T : Component {
		var oGameObject = Func.CreateTouchResponder(a_oName, a_oOrigin, a_oParent, a_stSize, a_stPos, a_stColor);
		return oGameObject?.GetComponentInChildren<T>();
	}
	#endregion			// 제네릭 클래스 함수

	#region 조건부 클래스 함수
#if UNITY_EDITOR
	//! 객체를 선택한다
	public static void SelectGameObject(GameObject a_oGameObject, bool a_bIsEnablePing = false) {
		Func.Assert(a_oGameObject != null);
		Selection.activeGameObject = a_oGameObject;

		if(a_bIsEnablePing) {
			EditorGUIUtility.PingObject(a_oGameObject);
		}
	}
#endif			// #if UNITY_EDITOR

#if HAPTIC_FEEDBACK_ENABLE && (UNITY_IOS || UNITY_ANDROID)
	//! 햅틱 피드백 지원 여부를 검사한다
	public static bool IsSupportHapticFeedback() {
#if UNITY_IOS
		string oModel = Device.generation.ToString();
		bool bIsiPhone = oModel.Contains(KDefine.U_MODEL_NAME_IPHONE);

		int nIndex = KDefine.U_HAPTIC_FEEDBACK_SUPPORT_MODELS.ExFindValue((a_eDeviceGeneration) => {
			return bIsiPhone && a_eDeviceGeneration == Device.generation;
		});

		return nIndex > KDefine.B_INDEX_INVALID;
#else
		return true;
#endif			// #if UNITY_IOS
	}
#endif			// #if HAPTIC_FEEDBACK_ENABLE && (UNITY_IOS || UNITY_ANDROID)

#if MESSAGE_PACK_ENABLE
	//! 업데이트 필요 여부를 검사한다
	public static bool IsNeedUpdate() {
		bool bIsEnable = CAppInfoStorage.Instance.IsLoadStoreVersion && 
			CAppInfoStorage.Instance.IsValidStoreVersion;

		return bIsEnable && Func.IsNeedUpdate(CAppInfoStorage.Instance.StoreVersion);
	}

	//! 버전 정보를 생성한다
	public static STVersionInfo MakeDefVersionInfo(string a_oVersion) {
		return new STVersionInfo() {
			m_oVersion = a_oVersion,
			m_oExtraInfoList = new Dictionary<string, string>()
		};
	}
#endif			// #if MESSAGE_PACK_ENABLE
	#endregion			// 조건부 클래스 함수
}
