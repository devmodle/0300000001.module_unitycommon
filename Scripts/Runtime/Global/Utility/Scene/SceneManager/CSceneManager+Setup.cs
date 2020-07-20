using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

#if UNIVERSAL_RENDER_PIPELINE_ENABLE
using UnityEngine.Rendering.Universal;
#endif			// #if UNIVERSAL_RENDER_PIPELINE_ENABLE

//! 씬 관리자 - 설정
public abstract partial class CSceneManager : CComponent {
	#region 함수
	//! 씬을 설정한다
	protected virtual void SetupScene() {
		// 씬을 설정한다 {
		this.SetupLights();
		this.SetupDefaultObjs();

		if(!this.IsRootScene) {
			this.SetupSubScene();

			if(Application.isPlaying) {
				Func.ShowLog("CSceneManager.SubSceneAwake: {0}", KBDefine.LOG_COLOR_SETUP, this.SceneName);
			}
		} else {
			this.SetupRootScene();

			if(Application.isPlaying) {
				Func.ShowLog("CSceneManager.RootSceneAwake: {0}", KBDefine.LOG_COLOR_SETUP, this.SceneName);
			}
		}
		// 씬을 설정한다 }

		// 카메라를 설정한다 {
		if(this.SubUICamera != null) {
			this.SubUICamera.gameObject.ExRemoveComponent<AudioListener>();

			this.SubUICamera.backgroundColor = this.ClearColor;
			this.SubUICamera.ExSetCullingMask(KUDefine.DEF_LAYER_MASK_UI_CAMERA.ToList());

#if CAMERA_STACK_ENABLE
			this.SubUICamera.gameObject.SetActive(true);
#else
			this.SubUICamera.gameObject.SetActive(false);
#endif			// #if CAMERA_STACK_ENABLE

#if UNIVERSAL_RENDER_PIPELINE_ENABLE
			this.SubUICamera.clearFlags = CameraClearFlags.Nothing;
			var oCameraData = this.SubUICamera.gameObject.ExAddComponent<UniversalAdditionalCameraData>();

			if(oCameraData != null) {
				oCameraData.renderType = CameraRenderType.Overlay;

#if LIGHT_ENABLE && SHADOW_ENABLE
				oCameraData.renderShadows = true;
#else
				oCameraData.renderShadows = false;
#endif			// #if LIGHT_ENABLE && SHADOW_ENABLE

#if UNITY_POST_PROCESSING_STACK_V2
				oCameraData.renderPostProcessing = true;
#else
				oCameraData.renderPostProcessing = false;
#endif			// #if UNITY_POST_PROCESSING_STACK_V2
			}
#else
			this.SubUICamera.clearFlags = CameraClearFlags.Depth;
#endif			// #if UNIVERSAL_RENDER_PIPELINE_ENABLE
		}

		if(this.SubMainCamera != null && CSceneManager.MainCamera != null) {
			var oAudioListener = this.SubMainCamera.gameObject.ExAddComponent<AudioListener>();
			oAudioListener.enabled = this.IsRootScene;

			this.SubMainCamera.backgroundColor = this.ClearColor;
			this.SubMainCamera.clearFlags = this.IsRootScene ? CameraClearFlags.SolidColor : CameraClearFlags.Nothing;

			this.SubMainCamera.gameObject.SetActive(this.IsRootScene);
			this.SubMainCamera.ExSetCullingMask(KUDefine.DEF_LAYER_MASK_MAIN_CAMERA.ToList());
			
#if PHYSICS_RAYCASTER_ENABLE
#if MODE_2D_ENABLE
			var oRaycaster = this.SubMainCamera.gameObject.ExAddComponent<Physics2DRaycaster>();
#else
			var oRaycaster = this.SubMainCamera.gameObject.ExAddComponent<PhysicsRaycaster>();
#endif			// #if MODE_2D_ENABLE

			Func.SetEventMask(oRaycaster, KUDefine.DEF_CULLING_MASK_MAIN_CAMERA.ToList());
#endif			// #if PHYSICS_RAYCASTER_ENABLE
			
#if UNIVERSAL_RENDER_PIPELINE_ENABLE
			var oCameraData = CSceneManager.MainCamera.gameObject.ExAddComponent<UniversalAdditionalCameraData>();
			var oUICameraData = this.SubUICamera?.GetComponentInChildren<UniversalAdditionalCameraData>();

			if(oCameraData != null && oCameraData.cameraStack != null) {
				oCameraData.renderType = CameraRenderType.Base;
				this.SubMainCamera.clearFlags = this.IsRootScene ? CameraClearFlags.SolidColor : CameraClearFlags.Nothing;

				if(this.IsRootScene) {
					oCameraData.cameraStack.Clear();
				} else {
					var oMainCameraData = this.SubMainCamera.gameObject.ExAddComponent<UniversalAdditionalCameraData>();
					oMainCameraData?.cameraStack.Clear();
				}

#if CAMERA_STACK_ENABLE
				if(oUICameraData != null && oUICameraData.renderType == CameraRenderType.Overlay) {
					int nID = this.SubUICamera.GetInstanceID();

					int nIndex = Func.FindValue<Camera>(oCameraData.cameraStack, (a_oCamera) => {
						return nID == a_oCamera.GetInstanceID();
					});

					if(nIndex <= KBDefine.INDEX_INVALID) {					
						oCameraData.cameraStack.ExReplaceValue(this.SubUICamera);
					}
				}
#endif			// #if CAMERA_STACK_ENABLE

#if LIGHT_ENABLE && SHADOW_ENABLE
				oCameraData.renderShadows = true;
#else
				oCameraData.renderShadows = false;
#endif			// #if LIGHT_ENABLE && SHADOW_ENABLE

#if UNITY_POST_PROCESSING_STACK_V2
				oCameraData.renderPostProcessing = true;
#else
				oCameraData.renderPostProcessing = false;
#endif			// #if UNITY_POST_PROCESSING_STACK_V2
			}
#endif			// #if UNIVERSAL_RENDER_PIPELINE_ENABLE
		}
		// 카메라를 설정한다 }

		// 캔버스 순서를 설정한다 {
		this.SubUICanvas.ExSetSortingOrder(this.UICanvasSortingOrderInfo.Key, 
			this.UICanvasSortingOrderInfo.Value);

		if(this.SubObjCanvas != null) {
			this.SubObjCanvas.ExSetSortingOrder(this.ObjCanvasSortingOrderInfo.Key, 
				this.ObjCanvasSortingOrderInfo.Value);
		}
		// 캔버스 순서를 설정한다 }
	}

	//! 루트 씬을 설정한다
	protected virtual void SetupRootScene() {
		this.SetupCamera();
		this.SetupRootObjs();

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		if(CSceneManager.ScreenDebugBtn != null) {
			CSceneManager.ScreenDebugBtn.gameObject.SetActive(true);

			CSceneManager.ScreenDebugBtn.onClick.RemoveAllListeners();
			CSceneManager.ScreenDebugBtn.onClick.AddListener(CSceneManager.OnTouchDebugBtn);
		}
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		if(CSceneManager.ScreenFPSBtn != null) {
			CSceneManager.ScreenFPSBtn.gameObject.SetActive(true);

			CSceneManager.ScreenFPSBtn.onClick.RemoveAllListeners();
			CSceneManager.ScreenFPSBtn.onClick.AddListener(CSceneManager.OnTouchFPSBtn);
		}
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

		if(Application.isPlaying) {
			this.StartScreenFadeOutAni(this.FadeOutAniDuration);
			
			CScheduleManager.Instance.AddComponent(this);
			CNavStackManager.Instance.AddComponent(this);

			// 캔버스를 설정한다 {
			this.SetupCanvas(CSceneManager.ScreenBlindUIRoot?.GetComponentInParent<Canvas>());
			this.SetupCanvas(CSceneManager.ScreenPopupUIRoot?.GetComponentInParent<Canvas>());
			this.SetupCanvas(CSceneManager.ScreenTopmostUIRoot?.GetComponentInParent<Canvas>());
			this.SetupCanvas(CSceneManager.ScreenAbsoluteUIRoot?.GetComponentInParent<Canvas>());

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
			this.SetupCanvas(CSceneManager.ScreenDebugUIRoot?.GetComponentInParent<Canvas>());
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
			// 캔버스를 설정한다 }
		}
	}

	//! 서브 씬을 설정한다
	protected virtual void SetupSubScene() {
		// 서브 객체를 설정한다 {
		var stScene = this.gameObject.scene;

		this.SubUITop = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_UI_TOP);
		this.SubUIBase = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_UI_BASE);
		this.SubUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_UI_ROOT);
		this.SubFixUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_FIX_UI_ROOT);

		this.SubLeftUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_LEFT_UI_ROOT);
		this.SubRightUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_RIGHT_UI_ROOT);
		this.SubTopUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_TOP_UI_ROOT);
		this.SubBottomUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_BOTTOM_UI_ROOT);

		this.SubPopupUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_POPUP_UI_ROOT);
		this.SubTopmostUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_TOPMOST_UI_ROOT);

		this.SubBase = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_BASE);
		this.SubObjBase = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_OBJ_BASE);
		this.SubObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_OBJ_ROOT);
		this.SubFixObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_FIX_OBJ_ROOT);

		this.SubLeftObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_LEFT_OBJ_ROOT);
		this.SubRightObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_RIGHT_OBJ_ROOT);
		this.SubTopObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_TOP_OBJ_ROOT);
		this.SubBottomObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_BOTTOM_OBJ_ROOT);

		this.SubObjCanvasTop = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_OBJ_CANVAS_TOP);
		this.SubObjCanvasBase = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_OBJ_CANVAS_BASE);
		this.SubCanvasObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_CANVAS_OBJ_ROOT);

		this.SubUICamera = stScene.ExFindComponent<Camera>(KUDefine.OBJ_NAME_SCENE_UI_CAMERA);
		this.SubMainCamera = stScene.ExFindComponent<Camera>(KUDefine.OBJ_NAME_SCENE_MAIN_CAMERA);

		this.SubUICanvas = this.SubUIBase.GetComponentInChildren<Canvas>();
		this.SubObjCanvas = this.SubObjCanvasBase?.GetComponentInChildren<Canvas>();
		// 서브 객체를 설정한다 }

		// 루트 객체를 설정한다
		this.SetupCamera();
		this.SetupRootObjs();

		// 사운드 객체를 설정한다 {
		var oObjs = this.gameObject.scene.GetRootGameObjects();

		for(int i = 0; i < oObjs.Length; ++i) {
			var oAudioListeners = oObjs[i].GetComponentsInChildren<AudioListener>(true);

			for(int j = 0; j < oAudioListeners.Length; ++j) {
				if(Application.isPlaying) {
					Destroy(oAudioListeners[j]);
				} else {
					oAudioListeners[j].enabled = false;
				}
			}
		}
		// 사운드 객체를 설정한다 }
	}

	//! 광원을 설정한다
	protected virtual void SetupLights() {
		var oObjs = this.gameObject.scene.GetRootGameObjects();

		for(int i = 0; i < oObjs.Length; ++i) {
			var oLights = oObjs[i].GetComponentsInChildren<Light>(true);

			for(int j = 0; j < oLights.Length; ++j) {
				bool bIsDirectional = oLights[j].type == LightType.Directional;
				oLights[j].shadows = KAppDefine.G_DEF_LIGHT_SHADOW_TYPE;

				if(bIsDirectional && oLights[j].name.ExIsEquals(KUDefine.OBJ_NAME_SCENE_MAIN_LIGHT)) {
					oLights[j].transform.localScale = KBDefine.SCALE_NORMAL;
					oLights[j].transform.localPosition = new Vector3(0.0f, 0.0f, -this.PlaneDistance);
					oLights[j].transform.localEulerAngles = KAppDefine.G_ROTATION_MAIN_LIGHT;
				}

#if LIGHT_ENABLE
				oLights[j].gameObject.SetActive(true);
#else
				oLights[j].gameObject.SetActive(false);
#endif			// #if LIGHT_ENABLE
			}
		}
	}

	//! 간격을 설정한다
	protected virtual void SetupOffsets() {
		// 캔버스 크기를 설정한다
		if(CSceneManager.UICanvas != null && CSceneManager.UICanvas.renderMode == RenderMode.ScreenSpaceCamera) {
			var oTransform = CSceneManager.UICanvas.transform as RectTransform;
			
			CSceneManager.CanvasSize = oTransform.sizeDelta;
			CSceneManager.CanvasScale = oTransform.localScale;
		}

		float fLeftScale = CUAccess.GetLeftScreenScale(Application.isPlaying);
		float fRightScale = CUAccess.GetRightScreenScale(Application.isPlaying);
		float fTopScale = CUAccess.GetTopScreenScale(Application.isPlaying);
		float fBottomScale = CUAccess.GetBottomScreenScale(Application.isPlaying);

		// 간격을 설정한다 {
		CSceneManager.LeftUIOffset = CSceneManager.CanvasSize.x * fLeftScale;
		CSceneManager.LeftObjOffset = CSceneManager.CanvasSize.x * fLeftScale;

		CSceneManager.RightUIOffset = CSceneManager.CanvasSize.x * -fRightScale;
		CSceneManager.RightObjOffset = CSceneManager.CanvasSize.x * -fRightScale;

		CSceneManager.TopUIOffset = CSceneManager.CanvasSize.y * -fTopScale;
		CSceneManager.TopObjOffset = CSceneManager.CanvasSize.y * -fTopScale;

		CSceneManager.BottomUIOffset = CSceneManager.CanvasSize.y * fBottomScale;
		CSceneManager.BottomObjOffset = CSceneManager.CanvasSize.y * fBottomScale;
		// 간격을 설정한다 }
	}

	//! 루트 객체를 설정한다
	protected virtual void SetupRootObjs() {
		this.transform.localScale = KBDefine.SCALE_NORMAL;
		this.transform.localPosition = Vector3.zero;
		this.transform.localEulerAngles = Vector3.zero;

		this.SubBase.transform.localScale = KBDefine.SCALE_NORMAL;
		this.SubBase.transform.localPosition = Vector3.zero;
		this.SubBase.transform.localEulerAngles = Vector3.zero;

		this.SubUITop.transform.localScale = KBDefine.SCALE_NORMAL;
		this.SubUITop.transform.localPosition = Vector3.zero;
		this.SubUITop.transform.localEulerAngles = Vector3.zero;

		this.SubObjBase.transform.localScale = CSceneManager.CanvasScale;
		this.SubObjBase.transform.localPosition = Vector3.zero;
		this.SubObjBase.transform.localEulerAngles = Vector3.zero;

		this.SubObjRoot.transform.localScale = KBDefine.SCALE_NORMAL;
		this.SubObjRoot.transform.localPosition = new Vector3(KBDefine.SCREEN_WIDTH / -2.0f, KBDefine.SCREEN_HEIGHT / -2.0f, 0.0f);
		this.SubObjRoot.transform.localEulerAngles = Vector3.zero;

		this.SubFixObjRoot.transform.localScale = KBDefine.SCALE_NORMAL;
		this.SubFixObjRoot.transform.localPosition = Vector3.zero;
		this.SubFixObjRoot.transform.localEulerAngles = Vector3.zero;

		this.SubLeftObjRoot.transform.localScale = KBDefine.SCALE_NORMAL;
		this.SubLeftObjRoot.transform.localPosition = new Vector3((CSceneManager.CanvasSize.x / -2.0f) + CSceneManager.LeftObjOffset, KBDefine.SCREEN_HEIGHT / -2.0f, 0.0f);
		this.SubLeftObjRoot.transform.localEulerAngles = Vector3.zero;

		this.SubRightObjRoot.transform.localScale = KBDefine.SCALE_NORMAL;
		this.SubRightObjRoot.transform.localPosition = new Vector3((CSceneManager.CanvasSize.x / 2.0f) + CSceneManager.RightObjOffset, KBDefine.SCREEN_HEIGHT / -2.0f, 0.0f);
		this.SubRightObjRoot.transform.localEulerAngles = Vector3.zero;

		this.SubTopObjRoot.transform.localScale = KBDefine.SCALE_NORMAL;
		this.SubTopObjRoot.transform.localPosition = new Vector3(KBDefine.SCREEN_WIDTH / -2.0f, (CSceneManager.CanvasSize.y / 2.0f) + CSceneManager.TopObjOffset, 0.0f);
		this.SubTopObjRoot.transform.localEulerAngles = Vector3.zero;

		this.SubBottomObjRoot.transform.localScale = KBDefine.SCALE_NORMAL;
		this.SubBottomObjRoot.transform.localPosition = new Vector3(KBDefine.SCREEN_WIDTH / -2.0f, (CSceneManager.CanvasSize.y / -2.0f) + CSceneManager.BottomObjOffset, 0.0f);
		this.SubBottomObjRoot.transform.localEulerAngles = Vector3.zero;

		if(this.SubObjCanvasTop != null) {
			this.SubObjCanvasTop.transform.localScale = KBDefine.SCALE_NORMAL;
			this.SubObjCanvasTop.transform.localPosition = Vector3.zero;
			this.SubObjCanvasTop.transform.localEulerAngles = Vector3.zero;
		}

#if MODE_CENTER_ENABLE
		this.SubObjRoot.transform.localPosition = Vector3.zero;

		this.SubLeftObjRoot.transform.ExSetPosY(0.0f);
		this.SubRightObjRoot.transform.ExSetPosY(0.0f);

		this.SubTopObjRoot.transform.ExSetPosX(0.0f);
		this.SubBottomObjRoot.transform.ExSetPosX(0.0f);
#endif			// #if MODE_CENTER_ENABLE

		// 루트 객체 간격을 설정한다 {
		var oUITransform = CSceneManager.UIRoot.transform as RectTransform;
		var oLeftUITransform = CSceneManager.LeftUIRoot.transform as RectTransform;
		var oRightUITransform = CSceneManager.RightUIRoot.transform as RectTransform;
		var oTopUITransform = CSceneManager.TopUIRoot.transform as RectTransform;
		var oBottomUITransform = CSceneManager.BottomUIRoot.transform as RectTransform;

		CSceneManager.LeftUIRootOffset = oUITransform.anchoredPosition.x - oLeftUITransform.anchoredPosition.x;
		CSceneManager.RightUIRootOffset = (oUITransform.anchoredPosition.x + KBDefine.SCREEN_WIDTH) - oRightUITransform.anchoredPosition.x;
		CSceneManager.TopUIRootOffset = (oUITransform.anchoredPosition.y + KBDefine.SCREEN_HEIGHT) - oTopUITransform.anchoredPosition.y;
		CSceneManager.BottomUIRootOffset = oUITransform.anchoredPosition.y - oBottomUITransform.anchoredPosition.y;

		CSceneManager.LeftObjRootOffset = CSceneManager.ObjRoot.transform.localPosition.x - CSceneManager.LeftObjRoot.transform.localPosition.x;
		CSceneManager.RightObjRootOffset = (CSceneManager.ObjRoot.transform.localPosition.x + KBDefine.SCREEN_WIDTH) - CSceneManager.RightObjRoot.transform.localPosition.x;
		CSceneManager.TopObjRootOffset = (CSceneManager.ObjRoot.transform.localPosition.y + KBDefine.SCREEN_HEIGHT) - CSceneManager.TopObjRoot.transform.localPosition.y;
		CSceneManager.BottomObjRootOffset = CSceneManager.ObjRoot.transform.localPosition.y - CSceneManager.BottomObjRoot.transform.localPosition.y;
		// 루트 객체 간격을 설정한다 }
	}

	//! 기본 객체를 설정한다
	protected virtual void SetupDefaultObjs() {
		// 씬 관리자를 설정한다
		CSceneManager.m_oSubSceneManagerList.ExAddValue(this.SceneName, this);
		CSceneManager.RootSceneManager = Func.FindComponent<CSceneManager>(KUDefine.OBJ_NAME_SCENE_SCENE_MANAGER);

		// 기본 객체를 설정한다 {
		var stScene = CSceneManager.RootSceneManager.gameObject.scene;

		this.SubUITop = CSceneManager.UITop = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_UI_TOP);
		this.SubUIBase = CSceneManager.UIBase = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_UI_BASE);
		this.SubUIRoot = CSceneManager.UIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_UI_ROOT);
		this.SubFixUIRoot = CSceneManager.FixUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_FIX_UI_ROOT);

		this.SubLeftUIRoot = CSceneManager.LeftUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_LEFT_UI_ROOT);
		this.SubRightUIRoot = CSceneManager.RightUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_RIGHT_UI_ROOT);
		this.SubTopUIRoot = CSceneManager.TopUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_TOP_UI_ROOT);
		this.SubBottomUIRoot = CSceneManager.BottomUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_BOTTOM_UI_ROOT);

		this.SubPopupUIRoot = CSceneManager.PopupUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_POPUP_UI_ROOT);
		this.SubTopmostUIRoot = CSceneManager.TopmostUIRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_TOPMOST_UI_ROOT);

		this.SubBase = CSceneManager.Base = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_BASE);
		this.SubObjBase = CSceneManager.ObjBase = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_OBJ_BASE);
		this.SubObjRoot = CSceneManager.ObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_OBJ_ROOT);
		this.SubFixObjRoot = CSceneManager.FixObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_FIX_OBJ_ROOT);

		this.SubLeftObjRoot = CSceneManager.LeftObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_LEFT_OBJ_ROOT);
		this.SubRightObjRoot = CSceneManager.RightObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_RIGHT_OBJ_ROOT);
		this.SubTopObjRoot = CSceneManager.TopObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_TOP_OBJ_ROOT);
		this.SubBottomObjRoot = CSceneManager.BottomObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_BOTTOM_OBJ_ROOT);

		this.SubObjCanvasTop = CSceneManager.ObjCanvasTop = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_OBJ_CANVAS_TOP);
		this.SubObjCanvasBase = CSceneManager.ObjCanvasBase = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_OBJ_CANVAS_BASE);
		this.SubCanvasObjRoot = CSceneManager.CanvasObjRoot = stScene.ExFindChild(KUDefine.OBJ_NAME_SCENE_CANVAS_OBJ_ROOT);

		this.SubUICamera = CSceneManager.UICamera = stScene.ExFindComponent<Camera>(KUDefine.OBJ_NAME_SCENE_UI_CAMERA);
		this.SubMainCamera = CSceneManager.MainCamera = stScene.ExFindComponent<Camera>(KUDefine.OBJ_NAME_SCENE_MAIN_CAMERA);

		this.SubUICanvas = CSceneManager.UICanvas = CSceneManager.UIBase.GetComponentInChildren<Canvas>();
		this.SubObjCanvas = CSceneManager.ObjCanvas = CSceneManager.ObjCanvasBase?.GetComponentInChildren<Canvas>();
		// 기본 객체를 설정한다 }
	}

	//! 캔버스를 설정한다
	protected virtual void SetupCanvas(Canvas a_oCanvas) {
		if(a_oCanvas != null) {
			var oObj = a_oCanvas.gameObject;

			// UI 객체를 설정한다 {
			var oUIObjs = new GameObject[] {
				oObj.ExFindChild(KUDefine.OBJ_NAME_SCENE_UI_ROOT),
				oObj.ExFindChild(KUDefine.OBJ_NAME_SCENE_FIX_UI_ROOT),

				oObj.ExFindChild(KUDefine.OBJ_NAME_SCENE_LEFT_UI_ROOT),
				oObj.ExFindChild(KUDefine.OBJ_NAME_SCENE_RIGHT_UI_ROOT),
				oObj.ExFindChild(KUDefine.OBJ_NAME_SCENE_TOP_UI_ROOT),
				oObj.ExFindChild(KUDefine.OBJ_NAME_SCENE_BOTTOM_UI_ROOT),

				oObj.ExFindChild(KUDefine.OBJ_NAME_SCENE_POPUP_UI_ROOT),
				oObj.ExFindChild(KUDefine.OBJ_NAME_SCENE_TOPMOST_UI_ROOT),

				oObj.ExFindChild(KUDefine.OBJ_NAME_SCENE_CANVAS_OBJ_ROOT),

				oObj.ExFindChild(KUDefine.OBJ_NAME_SCREEN_BLIND_UI_ROOT),
				oObj.ExFindChild(KUDefine.OBJ_NAME_SCREEN_POPUP_UI_ROOT),
				oObj.ExFindChild(KUDefine.OBJ_NAME_SCREEN_TOPMOST_UI_ROOT),
				oObj.ExFindChild(KUDefine.OBJ_NAME_SCREEN_ABSOLUTE_UI_ROOT),

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
				oObj.ExFindChild(KUDefine.NAME_SCREEN_DEBUG_UI_ROOT)
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
			};

			for(int i = 0; i < oUIObjs.Length; ++i) {
				if(oUIObjs[i] != null) {
					string oName = oUIObjs[i].name;

					var stPos = Vector2.zero;
					var stSize = Vector2.zero;

					var stPivot = KBDefine.ANCHOR_MIDDLE_CENTER;

					bool bIsLeftUIRoot = oName.ExIsEquals(KUDefine.OBJ_NAME_SCENE_LEFT_UI_ROOT);
					bool bIsRightUIRoot = oName.ExIsEquals(KUDefine.OBJ_NAME_SCENE_RIGHT_UI_ROOT);
					bool bIsTopUIRoot = oName.ExIsEquals(KUDefine.OBJ_NAME_SCENE_TOP_UI_ROOT);
					bool bIsBottomUIRoot = oName.ExIsEquals(KUDefine.OBJ_NAME_SCENE_BOTTOM_UI_ROOT);

					if(bIsLeftUIRoot || bIsRightUIRoot) {
						stSize = new Vector2(0.0f, KBDefine.SCREEN_HEIGHT);

						if(bIsLeftUIRoot) {
							stPos = new Vector2((CSceneManager.CanvasSize.x / -2.0f) + CSceneManager.LeftUIOffset, KBDefine.SCREEN_HEIGHT / -2.0f);
							stPivot = KBDefine.ANCHOR_BOTTOM_LEFT;
						} else {
							stPos = new Vector2((CSceneManager.CanvasSize.x / 2.0f) + CSceneManager.RightUIOffset, KBDefine.SCREEN_HEIGHT / -2.0f);
							stPivot = KBDefine.ANCHOR_BOTTOM_RIGHT;
						}
					} else if(bIsTopUIRoot || bIsBottomUIRoot) {
						stSize = new Vector2(KBDefine.SCREEN_WIDTH, 0.0f);

						if(bIsTopUIRoot) {
							stPos = new Vector2(KBDefine.SCREEN_WIDTH / -2.0f, (CSceneManager.CanvasSize.y / 2.0f) + CSceneManager.TopUIOffset);
							stPivot = KBDefine.ANCHOR_TOP_LEFT;
						} else {
							stPos = new Vector2(KBDefine.SCREEN_WIDTH / -2.0f, (CSceneManager.CanvasSize.y / -2.0f) + CSceneManager.BottomUIOffset);
							stPivot = KBDefine.ANCHOR_BOTTOM_LEFT;
						}
					} else {
						bool bIsUIRoot = oName.ExIsEquals(KUDefine.OBJ_NAME_SCENE_UI_ROOT);
						bool bIsFixUIRoot = oName.ExIsEquals(KUDefine.OBJ_NAME_SCENE_FIX_UI_ROOT);
						bool bIsBlindUIRoot = oName.ExIsEquals(KUDefine.OBJ_NAME_SCREEN_BLIND_UI_ROOT);
						bool bIsCanvasObjRoot = oName.ExIsEquals(KUDefine.OBJ_NAME_SCENE_CANVAS_OBJ_ROOT);

						bool bIsDebugUIRoot = false;

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
						bIsDebugUIRoot = oName.ExIsEquals(KUDefine.NAME_SCREEN_DEBUG_UI_ROOT);
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

						if(bIsUIRoot || bIsFixUIRoot || bIsBlindUIRoot || bIsCanvasObjRoot || bIsDebugUIRoot) {
							if(bIsUIRoot || bIsCanvasObjRoot) {
								stSize = new Vector2(KBDefine.SCREEN_WIDTH, KBDefine.SCREEN_HEIGHT);

								if(bIsCanvasObjRoot) {
									stPos = new Vector2(KBDefine.SCREEN_WIDTH / -2.0f, KBDefine.SCREEN_HEIGHT / -2.0f);
									stPivot = KBDefine.ANCHOR_BOTTOM_LEFT;
								}
							} else {
								stSize = CSceneManager.CanvasSize;
							}
						} else {
							stPos = new Vector2(KBDefine.SCREEN_WIDTH / -2.0f, KBDefine.SCREEN_HEIGHT / -2.0f);
						}
					}

					var oTransform = oUIObjs[i].transform as RectTransform;
					oTransform.anchorMin = KBDefine.ANCHOR_MIDDLE_CENTER;
					oTransform.anchorMax = KBDefine.ANCHOR_MIDDLE_CENTER;

					oTransform.pivot = stPivot;
					oTransform.sizeDelta = stSize;
					oTransform.anchoredPosition = stPos;
					oTransform.localEulerAngles = Vector3.zero;
				}

				// 블라인드 이미지를 설정한다
				if(oUIObjs[i] != null && oUIObjs[i].name.ExIsEquals(KUDefine.OBJ_NAME_SCREEN_BLIND_UI_ROOT)) {
					var oPivots = new Vector2[] {
						KBDefine.ANCHOR_MIDDLE_RIGHT,
						KBDefine.ANCHOR_MIDDLE_LEFT,
						KBDefine.ANCHOR_BOTTOM_CENTER,
						KBDefine.ANCHOR_TOP_CENTER
					};

					var oAnchors = new Vector2[] {
						KBDefine.ANCHOR_MIDDLE_LEFT,
						KBDefine.ANCHOR_MIDDLE_RIGHT,
						KBDefine.ANCHOR_TOP_CENTER,
						KBDefine.ANCHOR_BOTTOM_CENTER
					};

#if PORTRAIT_ENABLE
					var oPositions = new Vector2[] {
						new Vector2(CSceneManager.LeftUIOffset, 0.0f),
						new Vector2(CSceneManager.RightUIOffset, 0.0f),
						new Vector2(0.0f, 0.0f),
						new Vector2(0.0f, CSceneManager.BottomUIOffset)
					};
#else
					var oPositions = new Vector2[] {
						Vector2.zero,
						Vector2.zero,
						Vector2.zero,
						Vector2.zero
					};
#endif			// #if PORTRAIT_ENABLE

					var oImgs = new Image[] {
						oUIObjs[i].ExFindComponent<Image>(KUDefine.OBJ_NAME_LEFT_BLIND_IMG),
						oUIObjs[i].ExFindComponent<Image>(KUDefine.OBJ_NAME_RIGHT_BLIND_IMG),
						oUIObjs[i].ExFindComponent<Image>(KUDefine.OBJ_NAME_TOP_BLIND_IMG),
						oUIObjs[i].ExFindComponent<Image>(KUDefine.OBJ_NAME_BOTTOM_BLIND_IMG)
					};

					for(int j = 0; j < oImgs.Length; ++j) {
						var oImg = oImgs[j];
						oImg.color = CBAccess.IsEditorPlatform() ? KUDefine.DEF_COLOR_TRANSPARENT : KUDefine.DEF_COLOR_BLIND_UI;

						oImg.rectTransform.pivot = oPivots[j];
						oImg.rectTransform.anchorMin = oAnchors[j];
						oImg.rectTransform.anchorMax = oAnchors[j];
						oImg.rectTransform.sizeDelta = CSceneManager.CanvasSize;
						oImg.rectTransform.anchoredPosition = oPositions[j];
					}
				}

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
				if(oUIObjs[i] != null && oUIObjs[i].name.ExIsEquals(KUDefine.NAME_SCREEN_DEBUG_UI_ROOT)) {
					var stSafeArea = CUAccess.GetSafeArea(Application.isPlaying);
					var stScreenSize = CUAccess.GetDeviceScreenSize(true);

					// 정적 텍스트를 설정한다
					if(CSceneManager.ScreenStaticDebugText != null) {
						var oTransform = CSceneManager.ScreenStaticDebugText.rectTransform;
						oTransform.pivot = KBDefine.ANCHOR_TOP_LEFT;
						oTransform.anchorMin = KBDefine.ANCHOR_TOP_LEFT;
						oTransform.anchorMax = KBDefine.ANCHOR_TOP_LEFT;
						oTransform.sizeDelta = new Vector2(CSceneManager.CanvasSize.x, CSceneManager.CanvasSize.y / 2.0f);
						oTransform.anchoredPosition = Vector2.zero;

						CSceneManager.m_oStaticDebugStringBuilder.Clear();
						CSceneManager.m_oStaticDebugStringBuilder.AppendFormat(KUDefine.FORMAT_SCENE_M_STATIC_DEBUG_INFO_A, stScreenSize.x, stScreenSize.y);
						CSceneManager.m_oStaticDebugStringBuilder.AppendFormat(KUDefine.FORMAT_SCENE_M_STATIC_DEBUG_INFO_B, KBDefine.SCREEN_WIDTH, KBDefine.SCREEN_HEIGHT);
						CSceneManager.m_oStaticDebugStringBuilder.AppendFormat(KUDefine.FORMAT_SCENE_M_STATIC_DEBUG_INFO_C, CSceneManager.CanvasSize.x, CSceneManager.CanvasSize.y);
						CSceneManager.m_oStaticDebugStringBuilder.AppendFormat(KUDefine.FORMAT_SCENE_M_STATIC_DEBUG_INFO_D, CSceneManager.LeftUIOffset, CSceneManager.RightUIOffset, CSceneManager.TopUIOffset, CSceneManager.BottomUIOffset);
						CSceneManager.m_oStaticDebugStringBuilder.AppendFormat(KUDefine.FORMAT_SCENE_M_STATIC_DEBUG_INFO_E, CSceneManager.LeftObjOffset, CSceneManager.RightObjOffset, CSceneManager.TopObjOffset, CSceneManager.BottomObjOffset);
						CSceneManager.m_oStaticDebugStringBuilder.AppendFormat(KUDefine.FORMAT_SCENE_M_STATIC_DEBUG_INFO_F, CSceneManager.LeftUIRootOffset, CSceneManager.RightUIRootOffset, CSceneManager.TopUIRootOffset, CSceneManager.BottomUIRootOffset);
						CSceneManager.m_oStaticDebugStringBuilder.AppendFormat(KUDefine.FORMAT_SCENE_M_STATIC_DEBUG_INFO_G, CSceneManager.LeftObjRootOffset, CSceneManager.RightObjRootOffset, CSceneManager.TopObjRootOffset, CSceneManager.BottomObjRootOffset);
						CSceneManager.m_oStaticDebugStringBuilder.AppendFormat(KUDefine.FORMAT_SCENE_M_STATIC_DEBUG_INFO_H, stSafeArea.x, stSafeArea.y, stSafeArea.width, stSafeArea.height);
					}

					// 동적 텍스트를 설정한다
					if(CSceneManager.ScreenDynamicDebugText != null) {
						var oTransform = CSceneManager.ScreenDynamicDebugText.rectTransform;
						oTransform.pivot = KBDefine.ANCHOR_BOTTOM_LEFT;
						oTransform.anchorMin = KBDefine.ANCHOR_BOTTOM_LEFT;
						oTransform.anchorMax = KBDefine.ANCHOR_BOTTOM_LEFT;
						oTransform.sizeDelta = new Vector2(CSceneManager.CanvasSize.x, CSceneManager.CanvasSize.y / 2.0f);
						oTransform.anchoredPosition = Vector2.zero;

						CSceneManager.m_oDynamicDebugStringBuilder.Clear();
					}
				}
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
			}
			// UI 객체를 설정한다 }

			// 캔버스를 설정한다 {
#if CAMERA_STACK_ENABLE
			if(this.SubUICamera != null && !a_oCanvas.name.ExIsEquals(KUDefine.OBJ_NAME_SCENE_OBJ_CANVAS_BASE)) {
				a_oCanvas.worldCamera = this.SubUICamera;
			} else {
				a_oCanvas.worldCamera = CSceneManager.MainCamera;
			}
#else
			a_oCanvas.worldCamera = CSceneManager.MainCamera;
#endif			// #if CAMERA_STACK_ENABLE

			if(a_oCanvas.renderMode == RenderMode.ScreenSpaceCamera) {
				a_oCanvas.planeDistance = this.PlaneDistance;
				a_oCanvas.sortingLayerName = KUDefine.SORTING_LAYER_DEF;
			}

#if PIXEL_PERFECT_ENABLE
			a_oCanvas.pixelPerfect = true;
#else
			a_oCanvas.pixelPerfect = false;
#endif			// #if PIXEL_PERFECT_ENABLE
			// 캔버스를 설정한다 }

			// 캔버스 비율 처리자를 설정한다 {
			var oCanvasScaler = a_oCanvas.GetComponentInChildren<CanvasScaler>();
			bool bIsUICanvas = a_oCanvas.name.ExIsEquals(KUDefine.OBJ_NAME_SCENE_UI_BASE);

			if(bIsUICanvas || a_oCanvas.name.ExIsEquals(KUDefine.OBJ_NAME_SCENE_OBJ_CANVAS_BASE)) {
				oCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			}

			if(oCanvasScaler != null && oCanvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) {
				oCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
				oCanvasScaler.referenceResolution = new Vector2(KBDefine.SCREEN_WIDTH, KBDefine.SCREEN_HEIGHT);
				oCanvasScaler.referencePixelsPerUnit = KBDefine.REF_PIXELS_UNIT;
			}
			// 캔버스 비율 처리자를 설정한다 }
		}
	}

	//! 카메라를 설정한다
	protected virtual void SetupCamera() {
		// 캔버스를 설정한다 {
		this.SubUICanvas.renderMode = RenderMode.ScreenSpaceCamera;
		this.SetupCanvas(this.SubUICanvas);

		if(this.SubObjCanvas != null) {
			this.SubObjCanvas.renderMode = RenderMode.ScreenSpaceCamera;
			this.SetupCanvas(this.SubObjCanvas);
		}
		// 캔버스를 설정한다 }

		this.SetupUICamera(this.SubUICamera);
		this.SetupMainCamera(this.SubMainCamera);
	}

	//! UI 카메라를 설정한다
	protected virtual void SetupUICamera(Camera a_oCamera, bool a_bIsResetPos = true) {
		if(a_oCamera != null) {
#if !CUSTOM_CAMERA_POS_ENABLE
			if(a_bIsResetPos) {
				a_oCamera.transform.position = new Vector3(0.0f, 0.0f, -this.PlaneDistance);
			}
#endif			// #if !CUSTOM_CAMERA_POS_ENABLE

			// 트랜스 폼을 설정한다
			a_oCamera.transform.localScale = KBDefine.SCALE_NORMAL;
			a_oCamera.transform.localEulerAngles = Vector3.zero;

			// 카메라 영역을 설정한다
			a_oCamera.farClipPlane = KUDefine.DISTANCE_CAMERA_FAR_PLANE;
			a_oCamera.nearClipPlane = KUDefine.DISTANCE_CAMERA_NEAR_PLANE;

			// 카메라 투영을 설정한다 {
			a_oCamera.rect = KUDefine.RECT_UI_CAMERA;
			a_oCamera.depth = KUDefine.DEPTH_UI_CAMERA;

			a_oCamera.ExSetup2D(KBDefine.SCREEN_HEIGHT * KBDefine.UNIT_SCALE);
			// 카메라 투영을 설정한다 }
		}
	}

	//! 메인 카메라를 설정한다
	protected virtual void SetupMainCamera(Camera a_oCamera, bool a_bIsResetPos = true) {
		if(a_oCamera != null) {
#if !CUSTOM_CAMERA_POS_ENABLE
			if(a_bIsResetPos) {
				a_oCamera.transform.position = new Vector3(0.0f, 0.0f, -this.PlaneDistance);
			}
#endif			// #if !CUSTOM_CAMERA_POS_ENABLE

			// 트랜스 폼을 설정한다 {
			a_oCamera.transform.localScale = KBDefine.SCALE_NORMAL;

#if MODE_2D_ENABLE
			a_oCamera.transform.localEulerAngles = Vector3.zero;
#endif			// #if MODE_2D_ENABLE
			// 트랜스 폼을 설정한다 }

			// 카메라 영역을 설정한다
			a_oCamera.farClipPlane = KUDefine.DISTANCE_CAMERA_FAR_PLANE;
			a_oCamera.nearClipPlane = KUDefine.DISTANCE_CAMERA_NEAR_PLANE;

			// 카메라 투영을 설정한다 {
			a_oCamera.rect = KUDefine.RECT_MAIN_CAMERA;
			a_oCamera.depth = KUDefine.DEPTH_MAIN_CAMERA;
			
#if MODE_2D_ENABLE
			a_oCamera.ExSetup2D(KBDefine.SCREEN_HEIGHT * KBDefine.UNIT_SCALE);
#else
			a_oCamera.ExSetup3D(KBDefine.SCREEN_HEIGHT * KBDefine.UNIT_SCALE, this.PlaneDistance);
#endif			// #if MODE_2D_ENABLE
			// 카메라 투영을 설정한다 }
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
	//! 에디터 씬을 설정한다
	public void EditorSetupScene() {
#if UNIVERSAL_RENDER_PIPELINE_ENABLE
		if(QualitySettings.renderPipeline == null || GraphicsSettings.renderPipelineAsset == null) {
			Func.SetupQuality(Screen.currentResolution.refreshRate, 
				KAppDefine.G_MULTI_TOUCH_ENABLE, KAppDefine.G_DEF_QUALITY_LEVEL, true);
		}
#endif			// #if UNIVERSAL_RENDER_PIPELINE_ENABLE

		this.SetupScene();

		if(Camera.main != null) {
			this.SetupOffsets();
		}
	}

	//! 스크립트 순서를 설정한다
	protected sealed override void SetupScriptOrder() {
		this.ExSetScriptOrder(this.ScriptOrder);
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
