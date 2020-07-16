using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
#endif			// #if UNITY_IOS

#if ADS_ENABLE && ADMOB_ENABLE
using GoogleMobileAds.Api;
#endif			// #if ADS_ENABLE && ADMOB_ENABLE

//! 유틸리티 상수
public static partial class KDefine {
	#region 기본
	// 개수
	public const int U_MAX_NUM_LAYERS = 32;
	public const int U_MAX_NUM_DUPLICATE_FX_SNDS = 5;

	// 길이
	public const int U_MAX_LENGTH_LOG = 100000000;
	public const float U_MAX_PERCENT_ASYNC_SCENE_LOAD = 0.9f;

	// 레이어
	public const int U_LAYER_DEF = 0;
	public const int U_LAYER_TRANSPARENT_FX = 1;
	public const int U_LAYER_IGNORE_RAYCAST = 2;
	public const int U_LAYER_WATER = 4;
	public const int U_LAYER_UI = 5;
	public const int U_LAYER_CUSTOM = 8;

	// 정렬 순서 {
	public const int U_SORTING_ORDER_SCREEN_POPUP_UI = 0;
	public const int U_SORTING_ORDER_SCREEN_TOPMOST_UI = 1;
	public const int U_SORTING_ORDER_SCREEN_ABSOLUTE_UI = 2;
	public const int U_SORTING_ORDER_SCREEN_BLIND_UI = 3;
	public const int U_SORTING_ORDER_SCREEN_DEBUG_UI = 4;

	public const int U_SORTING_ORDER_FPS_COUNTER = 5;
	public const int U_SORTING_ORDER_FILE_BROWSER_UI = short.MaxValue;
	// 정렬 순서 }

	// 세기
	public const float U_DEF_INTENSITY_VIBRATE = 1.0f;

	// 깊이
	public const float U_DEPTH_UI_CAMERA = 50.0f;
	public const float U_DEPTH_MAIN_CAMERA = -50.0f;

	// 거리 {
	public const float U_DEF_DISTANCE_PAGE = 25.0f;
	public const float U_DEF_DISTANCE_CAMERA_PLANE = 5.0f;

	public const float U_DISTANCE_CAMERA_FAR_PLANE = 5000.0f;
	public const float U_DISTANCE_CAMERA_NEAR_PLANE = 0.1f;
	// 거리 }

	// 비율 {
	public const float U_MIN_SCALE_POPUP = 0.001f;
	public const float U_DEF_SCALE_POPUP = 1.0f;

	public const float U_DEF_SCALE_TOUCH = 1.05f;
	public const float U_DEF_SCALE_PAGE_SCROLL = 0.35f;
	// 비율 }

	// 시간 {
	public const float U_DEF_TIME_SCALE = 1.0f;
	public const float U_ZERO_TIME_SCALE = 0.0f;

	public const float U_DELTA_TIME_GC = 10.0f;

	public const float U_DELTA_TIME_PERMISSION_CHECK = 0.15f;
	public const float U_MAX_DELTA_TIME_PERMISSION_CHECK = 15.0f;

	public const float U_DEF_DURATION_ANI = 0.25f;
	public const float U_DEF_DURATION_POPUP_ANI = 0.15f;
	public const float U_DEF_DURATION_SCROLL_ANI = 0.25f;
	public const float U_DEF_DURATION_SCREEN_FADE_IN_ANI = 0.15f;
	public const float U_DEF_DURATION_SCREEN_FADE_OUT_ANI = 0.15f;
	public const float U_DEF_DURATION_TOAST_POPUP = 2.0f;

	public const float U_DEF_DURATION_LIGHT_VIBRATE = 0.05f;
	public const float U_DEF_DURATION_MEDIUM_VIBRATE = 0.1f;
	public const float U_DEF_DURATION_HEAVY_VIBRATE = 0.15f;

	public const float U_DELAY_INIT = 0.15f;
	public const float U_DEF_DELAY_POPUP_SHOW_ANI = KDefine.B_DELTA_TIME_INTERMEDIATE;

	public const float U_DEF_TIMEOUT_NETWORK_CONNECTION = 30.0f;
	// 시간 }

	// 형식
	public const string U_FORMAT_LOG_M_LOG = "[{0}]\nLogType: {1}\nCondition: {2}\nStackTrace:\n{3}==============================\n\n";

	// 태그 {
	public const string U_TAG_PLAYER = "Player";
	public const string U_TAG_FINISH = "Finish";
	public const string U_TAG_RESPAWN = "Respawn";
	public const string U_TAG_EDITOR_ONLY = "EditorOnly";
	public const string U_TAG_MAIN_CAMERA = "MainCamera";
	public const string U_TAG_GAME_CONTROLLER = "GameController";

	public const string U_TAG_ENEMY = "Enemy";
	public const string U_TAG_OBSTACLE = "Obstacle";
	public const string U_TAG_UI_CAMERA = "UICamera";
	public const string U_TAG_MAIN_LIGHT = "MainLight";
	public const string U_TAG_SCENE_MANAGER = "SceneManager";
	// 태그 }

	// 정렬 레이어 {
	public const string U_SORTING_LAYER_UNDERGROUND = "Underground";
	public const string U_SORTING_LAYER_BACKGROUND = "Background";
	public const string U_SORTING_LAYER_DEF = "Default";
	public const string U_SORTING_LAYER_FOREGROUND = "Foreground";
	public const string U_SORTING_LAYER_OVERGROUND = "Overground";
	public const string U_SORTING_LAYER_TOP = "Top";
	public const string U_SORTING_LAYER_TOPMOST = "Topmost";
	public const string U_SORTING_LAYER_ABSOLUTE = "Absolute";

#if !CAMERA_STACK_ENABLE
	public const string U_SORTING_LAYER_UNDERGROUND_UI = "UndergroundUI";
	public const string U_SORTING_LAYER_BACKGROUND_UI = "BackgroundUI";
	public const string U_SORTING_LAYER_DEF_UI = "DefaultUI";
	public const string U_SORTING_LAYER_FOREGROUND_UI = "ForegroundUI";
	public const string U_SORTING_LAYER_OVERGROUND_UI = "OvergroundUI";
	public const string U_SORTING_LAYER_TOP_UI = "TopUI";
	public const string U_SORTING_LAYER_TOPMOST_UI = "TopmostUI";
	public const string U_SORTING_LAYER_ABSOLUTE_UI = "AbsoluteUI";
#endif			// #if !CAMERA_STACK_ENABLE
	// 정렬 레이어 }

	// 키 {
	public const string U_KEY_DEVICE_CMD = "Cmd";
	public const string U_KEY_DEVICE_MSG = "Msg";

	public const string U_KEY_STRING_T_ID = "ID";
	public const string U_KEY_STRING_T_STRING = "String";

	public const string U_KEY_VALUE_T_ID = "ID";
	public const string U_KEY_VALUE_T_VALUE = "Value";
	public const string U_KEY_VALUE_T_VALUE_TYPE = "ValueType";

	public const string U_KEY_ALERT_P_TITLE = "Title";
	public const string U_KEY_ALERT_P_MSG = "Msg";
	public const string U_KEY_ALERT_P_OK_BTN_TEXT = "OKBtnText";
	public const string U_KEY_ALERT_P_CANCEL_BTN_TEXT = "CancelBtnText";

	public const string U_KEY_UNITY_MS_APP_ID = "AppID";
	public const string U_KEY_UNITY_MS_VERSION = "Version";
	public const string U_KEY_UNITY_MS_TIMEOUT = "Timeout";

	public const string U_KEY_UNITY_MS_ALERT_TITLE = KDefine.U_KEY_ALERT_P_TITLE;
	public const string U_KEY_UNITY_MS_ALERT_MSG = KDefine.U_KEY_ALERT_P_MSG;
	public const string U_KEY_UNITY_MS_ALERT_OK_BTN_TEXT = KDefine.U_KEY_ALERT_P_OK_BTN_TEXT;
	public const string U_KEY_UNITY_MS_ALERT_CANCEL_BTN_TEXT = KDefine.U_KEY_ALERT_P_CANCEL_BTN_TEXT;

	public const string U_KEY_UNITY_MS_VIBRATE_TYPE = "Type";
	public const string U_KEY_UNITY_MS_VIBRATE_STYLE = "Style";
	public const string U_KEY_UNITY_MS_VIBRATE_DURATION = "Duration";
	public const string U_KEY_UNITY_MS_VIBRATE_INTENSITY = "Intensity";

	public const string U_KEY_DEVICE_MR_RESULT = "Result";
	public const string U_KEY_DEVICE_MR_VERSION = KDefine.U_KEY_UNITY_MS_VERSION;

	public const string U_KEY_FORMAT_SCENE_M_TOUCH_RESPONDER = "SceneMTouchResponder_{0}";
	// 키 }

	// 이름 {
	public const string U_OBJ_NAME_SCENE_UI_TOP = "UIRoot";
	public const string U_OBJ_NAME_SCENE_UI_BASE = "Canvas";
	public const string U_OBJ_NAME_SCENE_UI_ROOT = "UIs";
	public const string U_OBJ_NAME_SCENE_FIX_UI_ROOT = "FixUIs";

	public const string U_OBJ_NAME_SCENE_LEFT_UI_ROOT = "LeftUIs";
	public const string U_OBJ_NAME_SCENE_RIGHT_UI_ROOT = "RightUIs";
	public const string U_OBJ_NAME_SCENE_TOP_UI_ROOT = "TopUIs";
	public const string U_OBJ_NAME_SCENE_BOTTOM_UI_ROOT = "BottomUIs";

	public const string U_OBJ_NAME_SCENE_POPUP_UI_ROOT = "PopupUIs";
	public const string U_OBJ_NAME_SCENE_TOPMOST_UI_ROOT = "TopmostUIs";

	public const string U_OBJ_NAME_SCENE_BASE = "Base";
	public const string U_OBJ_NAME_SCENE_OBJ_BASE = "ObjRoot";
	public const string U_OBJ_NAME_SCENE_OBJ_ROOT = "Objs";
	public const string U_OBJ_NAME_SCENE_FIX_OBJ_ROOT = "FixObjs";

	public const string U_OBJ_NAME_SCENE_LEFT_OBJ_ROOT = "LeftObjs";
	public const string U_OBJ_NAME_SCENE_RIGHT_OBJ_ROOT = "RightObjs";
	public const string U_OBJ_NAME_SCENE_TOP_OBJ_ROOT = "TopObjs";
	public const string U_OBJ_NAME_SCENE_BOTTOM_OBJ_ROOT = "BottomObjs";

	public const string U_OBJ_NAME_SCENE_OBJECT_CANVAS_TOP = "ObjCanvasRoot";
	public const string U_OBJ_NAME_SCENE_OBJECT_CANVAS_BASE = "ObjCanvas";
	public const string U_OBJ_NAME_SCENE_OBJECT_CANVAS_ROOT = "CanvasObjs";

	public const string U_OBJ_NAME_SCENE_UI_CAMERA = "UI Camera";
	public const string U_OBJ_NAME_SCENE_MAIN_CAMERA = "Main Camera";
	public const string U_OBJ_NAME_SCENE_MAIN_LIGHT = "Directional Light";
	public const string U_OBJ_NAME_SCENE_SCENE_MANAGER = "SceneManager";

	public const string U_OBJ_NAME_SCREEN_BLIND_UI_ROOT = "ScreenBlindUIs";
	public const string U_OBJ_NAME_SCREEN_POPUP_UI_ROOT = "ScreenPopupUIs";
	public const string U_OBJ_NAME_SCREEN_TOPMOST_UI_ROOT = "ScreenTopmostUIs";
	public const string U_OBJ_NAME_SCREEN_ABSOLUTE_UI_ROOT = "ScreenAbsoluteUIs";

	public const string U_OBJ_NAME_LEFT_BLIND_IMG = "LeftBlindImg";
	public const string U_OBJ_NAME_RIGHT_BLIND_IMG = "RightBlindImg";
	public const string U_OBJ_NAME_TOP_BLIND_IMG = "TopBlindImg";
	public const string U_OBJ_NAME_BOTTOM_BLIND_IMG = "BottomBlindImg";

	public const string U_OBJ_NAME_POPUP_CONTENT_ROOT = "Contents";
	public const string U_OBJ_NAME_POPUP_CLOSE_BTN = "CloseBtn";
	public const string U_OBJ_NAME_FORMAT_POPUP_TOUCH_RESPONDER = "PopupTouchResponder_{0}";

	public const string U_OBJ_NAME_ALERT_POPUP = "AlertPopup";
	public const string U_OBJ_NAME_TOAST_POPUP = "ToastPopup";

	public const string U_OBJ_NAME_ALERT_P_TITLE_TEXT = "TitleText";
	public const string U_OBJ_NAME_ALERT_P_MSG_TEXT = "MsgText";
	public const string U_OBJ_NAME_ALERT_P_BTN_TEXT = "Text";

	public const string U_OBJ_NAME_ALERT_P_BG_IMG = "BGImg";

	public const string U_OBJ_NAME_ALERT_P_OK_BTN = "OKBtn";
	public const string U_OBJ_NAME_ALERT_P_CANCEL_BTN = "CancelBtn";

	public const string U_OBJ_NAME_TOAST_P_MSG_TEXT = "MsgText";
	public const string U_OBJ_NAME_TOAST_P_TOAST_POPUP = "ToastPopup";

	public const string U_OBJ_NAME_SND_M_BG_SND = "BGSnd";
	public const string U_OBJ_NAME_SND_M_FX_SND = "FXSnd";

	public const string U_OBJ_NAME_SCROLL_V_CONTENT = "Content";
	public const string U_OBJ_NAME_SCROLL_V_VIEWPORT = "Viewport";

	public const string U_OBJ_NAME_DIALOG_TOUCH_RESPONDER = "DialogTouchResponder";
	public const string U_OBJ_NAME_SCREEN_F_TOUCH_RESPONDER = "ScreenFadeTouchResponder";
	public const string U_OBJ_NAME_ACTIVITY_I_TOUCH_RESPONDER = "ActivityIndicatorTouchResponder";

	public const string U_IMG_NAME_DEF_SPRITE = "DefaultSprite";
	public const string U_IMG_NAME_SPRITE_CLONE = "(Clone)";

	public const string U_MESH_NAME_DEF_MESH = "DefaultMesh";

	public const string U_CLS_NAME_UNITY_MS_MSG_RECEIVER = "dante.distribution.plugin.CAndroidPlugin";
	public const string U_FUNC_NAME_UNITY_MS_MSG_HANDLER = "handleUnityMsg";

	public const string U_FUNC_NAME_ON_DRAG = "OnDrag";
	public const string U_FUNC_NAME_ON_POINTER_UP = "OnPointerUp";
	public const string U_FUNC_NAME_ON_POINTER_DOWN = "OnPointerDown";
	public const string U_FUNC_NAME_ON_POINTER_ENTER = "OnPointerEnter";
	public const string U_FUNC_NAME_ON_POINTER_EXIT = "OnPointerExit";

	public const string U_FUNC_NAME_RESET_LOCALIZE = "ResetLocalize";
	// 이름 }
	#endregion			// 기본

	#region 런타임 상수
	// 영역
	public static readonly Rect U_RECT_UI_CAMERA = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
	public static readonly Rect U_RECT_MAIN_CAMERA = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

	// 색상 {
	public static readonly Color U_DEF_COLOR_NORMAL = Color.white;
	public static readonly Color U_DEF_COLOR_SELECT = new Color(0.5f, 0.5f, 0.5f, 1.0f);
	public static readonly Color U_DEF_COLOR_DISABLE = new Color(0.5f, 0.5f, 0.5f, 1.0f);
	public static readonly Color U_DEF_COLOR_TRANSPARENT = new Color(0.0f, 0.0f, 0.0f, 0.0f);

	public static readonly Color U_DEF_COLOR_BLIND_UI = Color.black;
	public static readonly Color U_DEF_COLOR_SCREEN_FADE = Color.black;
	// 색상 }

	// 동기화 객체
	public static readonly object U_LOCK_OBJECT = new object();
	public static readonly object U_LOCK_OBJECT_UPDATE_M_UPDATE = new object();

	// 레이어 마스크 {
	public static readonly int[] U_DEF_LAYER_MASK_UI_CAMERA = new int[] {
		KDefine.U_LAYER_UI
	};

	public static readonly int[] U_DEF_LAYER_MASK_MAIN_CAMERA = new int[] {
		KDefine.U_LAYER_DEF,
		KDefine.U_LAYER_TRANSPARENT_FX,
		KDefine.U_LAYER_IGNORE_RAYCAST,
		KDefine.U_LAYER_WATER,

#if !CAMERA_STACK_ENABLE
		KDefine.U_LAYER_UI
#endif			// #if !CAMERA_STACK_ENABLE	
	};
	// 레이어 마스크 }

	// 태그
	public static readonly string[] U_TAGS = new string[] {
		KDefine.U_TAG_ENEMY,
		KDefine.U_TAG_OBSTACLE,
		KDefine.U_TAG_UI_CAMERA,
		KDefine.U_TAG_MAIN_LIGHT,
		KDefine.U_TAG_SCENE_MANAGER
	};

	// 정렬 레이어
	public static readonly string[] U_SORTING_LAYERS = new string[] {
		KDefine.U_SORTING_LAYER_UNDERGROUND,
		KDefine.U_SORTING_LAYER_BACKGROUND,
		KDefine.U_SORTING_LAYER_DEF,
		KDefine.U_SORTING_LAYER_FOREGROUND,
		KDefine.U_SORTING_LAYER_OVERGROUND,
		KDefine.U_SORTING_LAYER_TOP,
		KDefine.U_SORTING_LAYER_TOPMOST,
		KDefine.U_SORTING_LAYER_ABSOLUTE,

#if !CAMERA_STACK_ENABLE
		KDefine.U_SORTING_LAYER_UNDERGROUND_UI,
		KDefine.U_SORTING_LAYER_BACKGROUND_UI,
		KDefine.U_SORTING_LAYER_DEF_UI,
		KDefine.U_SORTING_LAYER_FOREGROUND_UI,
		KDefine.U_SORTING_LAYER_OVERGROUND_UI,
		KDefine.U_SORTING_LAYER_TOP_UI,
		KDefine.U_SORTING_LAYER_TOPMOST_UI,
		KDefine.U_SORTING_LAYER_ABSOLUTE_UI
#endif			// #if !CAMERA_STACK_ENABLE
	};

	// 경로 {
	public static readonly string U_OBJ_PATH_TEXT = string.Format("{0}{1}{2}U_Text", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_TEXT_BASE);
	public static readonly string U_OBJ_PATH_TEXT_BTN = string.Format("{0}{1}{2}U_TextBtn", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_BUTTON_BASE);
	public static readonly string U_OBJ_PATH_TEXT_SCALE_BTN = string.Format("{0}{1}{2}U_TextScaleBtn", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_BUTTON_BASE);

	public static readonly string U_OBJ_PATH_LOCALIZE_TEXT = string.Format("{0}{1}{2}U_LocalizeText", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_TEXT_BASE);
	public static readonly string U_OBJ_PATH_LOCALIZE_TEXT_BTN = string.Format("{0}{1}{2}U_LocalizeTextBtn", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_BUTTON_BASE);
	public static readonly string U_OBJ_PATH_LOCALIZE_TEXT_SCALE_BTN = string.Format("{0}{1}{2}U_LocalizeTextScaleBtn", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_BUTTON_BASE);

	public static readonly string U_OBJ_PATH_IMG_BTN = string.Format("{0}{1}{2}U_ImgBtn", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_BUTTON_BASE);
	public static readonly string U_OBJ_PATH_IMG_SCALE_BTN = string.Format("{0}{1}{2}U_ImgScaleBtn", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_BUTTON_BASE);

	public static readonly string U_OBJ_PATH_IMG_TEXT_BTN = string.Format("{0}{1}{2}U_ImgTextBtn", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_BUTTON_BASE);
	public static readonly string U_OBJ_PATH_IMG_TEXT_SCALE_BTN = string.Format("{0}{1}{2}U_ImgTextScaleBtn", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_BUTTON_BASE);

	public static readonly string U_OBJ_PATH_IMG_LOCALIZE_TEXT_BTN = string.Format("{0}{1}{2}U_ImgLocalizeTextBtn", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_BUTTON_BASE);
	public static readonly string U_OBJ_PATH_IMG_LOCALIZE_TEXT_SCALE_BTN = string.Format("{0}{1}{2}U_ImgLocalizeTextScaleBtn", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_BUTTON_BASE);

	public static readonly string U_OBJ_PATH_SCROLL_VIEW = string.Format("{0}{1}{2}U_ScrollView", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_SCROLL_VIEW_BASE);
	public static readonly string U_OBJ_PATH_PAGE_SCROLL_VIEW = string.Format("{0}{1}{2}U_PageScrollView", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_SCROLL_VIEW_BASE);

	public static readonly string U_OBJ_PATH_ALERT_POPUP = string.Format("{0}{1}{2}U_AlertPopup", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_POPUP_BASE);
	public static readonly string U_OBJ_PATH_TOAST_POPUP = string.Format("{0}{1}{2}U_ToastPopup", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_POPUP_BASE);

	public static readonly string U_OBJ_PATH_DRAG_RESPONDER = string.Format("{0}{1}{2}U_DragResponder", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_RESPONDER_BASE);
	public static readonly string U_OBJ_PATH_TOUCH_RESPONDER = string.Format("{0}{1}{2}U_TouchResponder", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_RESPONDER_BASE);

	public static readonly string U_OBJ_PATH_BG_SND = string.Format("{0}{1}{2}U_BGSnd", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_SOUND_BASE);
	public static readonly string U_OBJ_PATH_FX_SND = string.Format("{0}{1}{2}U_FXSnd", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE, KDefine.B_DIR_PATH_SOUND_BASE);

	public static readonly string U_OBJ_PATH_SS_FPS_COUNTER = string.Format("{0}{1}SS_FPSCounter", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_SETUP_SCENE_BASE);
	public static readonly string U_OBJ_PATH_SS_TIMER_MANAGER = string.Format("{0}{1}SS_TimerManager", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_SETUP_SCENE_BASE);

	public static readonly string U_SCRIPTABLE_PATH_G_BUILD_INFO_TABLE = string.Format("{0}{1}G_BuildInfoTable", KDefine.B_DIR_PATH_SCRIPTABLES, KDefine.B_DIR_PATH_GLOBAL_BASE);
	public static readonly string U_SCRIPTABLE_PATH_G_BUILD_OPTION_TABLE = string.Format("{0}{1}G_BuildOptionTable", KDefine.B_DIR_PATH_SCRIPTABLES, KDefine.B_DIR_PATH_GLOBAL_BASE);
	public static readonly string U_SCRIPTABLE_PATH_G_DEFINE_SYMBOL_TABLE = string.Format("{0}{1}G_DefineSymbolTable", KDefine.B_DIR_PATH_SCRIPTABLES, KDefine.B_DIR_PATH_GLOBAL_BASE);
	public static readonly string U_SCRIPTABLE_PATH_G_PROJECT_INFO_TABLE = string.Format("{0}{1}G_ProjectInfoTable", KDefine.B_DIR_PATH_SCRIPTABLES, KDefine.B_DIR_PATH_GLOBAL_BASE);
	public static readonly string U_SCRIPTABLE_PATH_G_DEVICE_INFO_TABLE = string.Format("{0}{1}G_DeviceInfoTable", KDefine.B_DIR_PATH_SCRIPTABLES, KDefine.B_DIR_PATH_GLOBAL_BASE);

	public static readonly string U_TABLE_PATH_G_COMMON_VALUE_TABLE = string.Format("{0}{1}{2}G_ValueTable_Common", KDefine.B_DIR_PATH_TABLES, KDefine.B_DIR_PATH_GLOBAL_BASE, KDefine.B_DIR_PATH_VALUE_INFO_BASE);
	public static readonly string U_TABLE_PATH_G_COMMON_STRING_TABLE = string.Format("{0}{1}{2}G_StringTable_Common", KDefine.B_DIR_PATH_TABLES, KDefine.B_DIR_PATH_GLOBAL_BASE, KDefine.B_DIR_PATH_STRING_INFO_BASE);

	public static readonly string U_TABLE_PATH_G_KOREAN_COMMON_STRING_TABLE = string.Format("{0}{1}{2}G_StringTable_Common_KO", KDefine.B_DIR_PATH_TABLES, KDefine.B_DIR_PATH_GLOBAL_BASE, KDefine.B_DIR_PATH_STRING_INFO_BASE);
	public static readonly string U_TABLE_PATH_G_ENGLISH_COMMON_STRING_TABLE = string.Format("{0}{1}{2}G_StringTable_Common_EN", KDefine.B_DIR_PATH_TABLES, KDefine.B_DIR_PATH_GLOBAL_BASE, KDefine.B_DIR_PATH_STRING_INFO_BASE);

	public static readonly string U_SND_PATH_G_TOUCH_BEGAN = string.Format("{0}{1}G_TouchBegan", KDefine.B_DIR_PATH_SOUNDS, KDefine.B_DIR_PATH_GLOBAL_BASE);
	public static readonly string U_SND_PATH_G_TOUCH_ENDED = string.Format("{0}{1}G_TouchEnded", KDefine.B_DIR_PATH_SOUNDS, KDefine.B_DIR_PATH_GLOBAL_BASE);

	public static readonly string U_PIPELINE_PATH_UNIVERSAL_RENDER_PIPELINE = string.Format("{0}{1}U_UniversalRPAsset", KDefine.B_DIR_PATH_PIPELINES, KDefine.B_DIR_PATH_UTILITY_BASE);
	public static readonly string U_PIPELINE_PATH_UNIVERSAL_RENDER_PIPELINE_RENDERER = string.Format("{0}{1}U_UniversalRPRenderer", KDefine.B_DIR_PATH_PIPELINES, KDefine.B_DIR_PATH_UTILITY_BASE);

	public static readonly string U_IMG_PATH_SCREENSHOT = string.Format("{0}Screenshot.png", KDefine.B_DIR_PATH_WRITABLE);

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
	public static readonly string U_IMG_RELATIVE_PATH_SCREENSHOT = string.Format("{0}/Screenshot.png", KAppDefine.G_NAME_PROJECT_ROOT, KDefine.B_DIR_PATH_WRITABLE);
#else
	public static readonly string U_IMG_RELATIVE_PATH_SCREENSHOT = string.Format("{0}Screenshot.png", KDefine.B_DIR_PATH_WRITABLE);
#endif			// #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
	// 경로 }
	#endregion			// 런타임 상수

	#region 조건부 상수
#if UNITY_EDITOR
	// 퀄리티 {
	public const bool U_QUALITY_ASYNC_UPLOAD_PERSISTENT_BUFFER = true;
	public const float U_QUALITY_RESOLUTION_SCALE_FIXED_DPI_FACTOR = 1.0f;

	public const int U_QUALITY_ANTI_ALIASING = 0;
	public const int U_QUALITY_MAX_LOD_LEVEL = 0;
	public const int U_QUALITY_ASYNC_UPLOAD_TIME_SLICE = 2;
	public const int U_QUALITY_ASYNC_UPLOAD_BUFFER_SIZE = 16;
	// 퀄리티 }

	// 스크립트 순서 {
	public const int U_SCRIPT_ORDER_SINGLETON = -10;
	public const int U_SCRIPT_ORDER_BANNER_ADS_CORRECTOR = 5;

	public const int U_SCRIPT_ORDER_INIT_SCENE_MANAGER = -20;
	public const int U_SCRIPT_ORDER_SETUP_SCENE_MANAGER = KDefine.U_SCRIPT_ORDER_INIT_SCENE_MANAGER + 1;
	public const int U_SCRIPT_ORDER_START_SCENE_MANAGER = KDefine.U_SCRIPT_ORDER_INIT_SCENE_MANAGER + 1;
	public const int U_SCRIPT_ORDER_LOADING_SCENE_MANAGER = KDefine.U_SCRIPT_ORDER_INIT_SCENE_MANAGER + 1;
	public const int U_SCRIPT_ORDER_SPLASH_SCENE_MANAGER = KDefine.U_SCRIPT_ORDER_INIT_SCENE_MANAGER + 1;
	public const int U_SCRIPT_ORDER_AGREE_SCENE_MANAGER = KDefine.U_SCRIPT_ORDER_INIT_SCENE_MANAGER + 1;
	public const int U_SCRIPT_ORDER_SCENE_MANAGER = KDefine.U_SCRIPT_ORDER_INIT_SCENE_MANAGER + 2;
	// 스크립트 순서 }

#if UNIVERSAL_RENDER_PIPELINE_ENABLE
	// 개수
	public const int U_MAX_NUM_UNIVERSAL_RP_ADDITIONAL_LIGHT_PER_OBJ = 4;

	// 크기
	public const int U_SIZE_UNIVERSAL_RP_COLOR_GRADING_LUT = 32;

	// 비율
	public const float U_SCALE_UNIVERSAL_RP_RENDERING = 1.0f;

	// 이름 {
	public const string U_FIELD_NAME_UNIVERSAL_RP_DEBUG_LEVEL = "m_DebugLevel";
	public const string U_FIELD_NAME_UNIVERSAL_RP_ANTI_ALIASING = "m_MSAA";
	public const string U_FIELD_NAME_UNIVERSAL_RP_SUPPORT_SOFT_SHADOW = "m_SoftShadowsSupported";
	public const string U_FIELD_NAME_UNIVERSAL_RP_SUPPORT_MIXED_LIGHTING = "m_MixedLightingSupported";
	
	public const string U_FIELD_NAME_UNIVERSAL_RP_MAIN_LIGHT_RENDERING_MODE = "m_MainLightRenderingMode";
	public const string U_FIELD_NAME_UNIVERSAL_RP_MAIN_LIGHT_SUPPORT_SHADOW = "m_MainLightShadowsSupported";
	public const string U_FIELD_NAME_UNIVERSAL_RP_MAIN_LIGHT_SHADOW_MAP_RESOLUTION = "m_MainLightShadowmapResolution";

	public const string U_FIELD_NAME_UNIVERSAL_RP_ADDITIONAL_LIGHT_RENDERING_MODE = "m_AdditionalLightsRenderingMode";
	public const string U_FIELD_NAME_UNIVERSAL_RP_ADDITIONAL_LIGHT_SUPPORT_SHADOW = "m_AdditionalLightShadowsSupported";
	public const string U_FIELD_NAME_UNIVERSAL_RP_ADDITIONAL_LIGHT_PER_OBJ_LIMIT = "m_AdditionalLightsPerObjectLimit";
	public const string U_FIELD_NAME_UNIVERSAL_RP_ADDITIONAL_LIGHT_SHADOW_MAP_RESOLUTION = "m_AdditionalLightShadowmapResolution";
	// 이름 }
#endif			// #if UNIVERSAL_RENDER_PIPELINE_ENABLE
#endif			// #if UNITY_EDITOR

#if UNITY_IOS
	// 키
	public const string U_KEY_UNITY_MS_LOGIN_WITH_APPLE_CALLBACK = "UnityMSLoginWithAppleCallback";
	public const string U_KEY_UNITY_MS_GET_CREDENTIAL_STATE_CALLBACK = "UnityMSGetCredentialStateCallback";

	// 이름 {
	public const string U_OBJ_NAME_LOGIN_WITH_APPLE = "LoginWithApple";

#if HAPTIC_FEEDBACK_ENABLE
	public const string U_MODEL_NAME_IPHONE = "iPhone";
#endif			// #if HAPTIC_FEEDBACK_ENABLE
	// 이름 }
#endif			// #if UNITY_IOS

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	// 시간
	public const float U_DELTA_TIME_DYNAMIC_DEBUG = 1.0f;

	// 형식 {
	public const string U_FORMAT_SCENE_M_STATIC_DEBUG_MSG = "{0}\n\n{1}";
	public const string U_FORMAT_SCENE_M_DYNAMIC_DEBUG_MSG = "{0}\n\n{1}";

	public const string U_FORMAT_SCENE_M_STATIC_DEBUG_INFO_A = "Resolution: <color=orange>{0:0.0}</color>, <color=orange>{1:0.0}</color>\n";
	public const string U_FORMAT_SCENE_M_STATIC_DEBUG_INFO_B = "Design Resolution: <color=orange>{0:0.0}</color>, <color=orange>{1:0.0}</color>\n";
	public const string U_FORMAT_SCENE_M_STATIC_DEBUG_INFO_C = "Canvas Size: <color=orange>{0:0.0}</color>, <color=orange>{1:0.0}</color>\n";
	public const string U_FORMAT_SCENE_M_STATIC_DEBUG_INFO_D = "UI Offset: <color=orange>{0:0.0}</color>, <color=orange>{1:0.0}</color>, <color=orange>{2:0.0}</color>, <color=orange>{3:0.0}</color>\n";
	public const string U_FORMAT_SCENE_M_STATIC_DEBUG_INFO_E = "Object Offset: <color=orange>{0:0.0}</color>, <color=orange>{1:0.0}</color>, <color=orange>{2:0.0}</color>, <color=orange>{3:0.0}</color>\n";
	public const string U_FORMAT_SCENE_M_STATIC_DEBUG_INFO_F = "UI Root Offset: <color=orange>{0:0.0}</color>, <color=orange>{1:0.0}</color>, <color=orange>{2:0.0}</color>, <color=orange>{3:0.0}</color>\n";
	public const string U_FORMAT_SCENE_M_STATIC_DEBUG_INFO_G = "Object Root Offset: <color=orange>{0:0.0}</color>, <color=orange>{1:0.0}</color>, <color=orange>{2:0.0}</color>, <color=orange>{3:0.0}</color>\n";
	public const string U_FORMAT_SCENE_M_STATIC_DEBUG_INFO_H = "Safe Area: <color=orange>{0:0.0}</color>, <color=orange>{1:0.0}</color>, <color=orange>{2:0.0}</color>, <color=orange>{3:0.0}</color>";

	public const string U_FORMAT_SCENE_M_DYNAMIC_DEBUG_INFO_A = "GC: <color=orange>{0:0.0}</color> MB, Used Heap: <color=orange>{1:0.0}</color> MB\n";
	public const string U_FORMAT_SCENE_M_DYNAMIC_DEBUG_INFO_B = "Mono Heap: <color=orange>{0:0.0}</color> MB, Mono Used: <color=orange>{1:0.0}</color> MB\n";
	public const string U_FORMAT_SCENE_M_DYNAMIC_DEBUG_INFO_C = "Temp Alloc: <color=orange>{0:0.0}</color> MB, Total Alloc: <color=orange>{1:0.0}</color> MB\n";
	public const string U_FORMAT_SCENE_M_DYNAMIC_DEBUG_INFO_D = "Reserved: <color=orange>{0:0.0}</color> MB, Unused Reserved: <color=orange>{1:0.0}</color> MB\n";
	public const string U_FORMAT_SCENE_M_DYNAMIC_DEBUG_INFO_E = "GPU Alloc: <color=orange>{0:0.0}</color> MB";
	// 형식 }

	// 이름 {
	public const string U_NAME_SCREEN_DEBUG_UI_ROOT = "ScreenDebugUIs";
	public const string U_NAME_SCREEN_DEBUG_TEXT_ROOT = "DebugTexts";

	public const string U_NAME_SCREEN_STATIC_FPS_TEXT = "StaticInfoText";
	public const string U_NAME_SCREEN_DYNAMIC_FPS_TEXT = "DynamicInfoText";

	public const string U_NAME_SCREEN_STATIC_DEBUG_TEXT = "StaticDebugText";
	public const string U_NAME_SCREEN_DYNAMIC_DEBUG_TEXT = "DynamicDebugText";

	public const string U_NAME_SCREEN_FPS_BTN = "FPSBtn";
	public const string U_NAME_SCREEN_DEBUG_BTN = "DebugBtn";
	// 이름 }
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if TENJIN_ENABLE
	// 키
	public const string U_KEY_TENJIN_M_RECEIPT = "json";
	public const string U_KEY_TENJIN_M_PAYLOAD = "Payload";
	public const string U_KEY_TENJIN_M_SIGNATURE = "signature";
#endif			// #if TENJIN_ENABLE

#if ADS_ENABLE
	// 횟수
	public const int U_MAX_TIMES_ADS_LOAD_TRY = 15;

	// 간격
	public const float U_OFFSET_BANNER_ADS = 110.0f;

	// 키 {
	public const string U_KEY_ADS_M_BANNER_ADS_ID = "AdsMBannerAdsID";
	public const string U_KEY_ADS_M_REWARD_ADS_ID = "AdsMRewardAdsID";
	public const string U_KEY_ADS_M_FULLSCREEN_ADS_ID = "AdsMFullscreenAdsID";
	public const string U_KEY_ADS_M_NATIVE_ADS_ID = "AdsMNativeAdsID";

	public const string U_KEY_ADS_M_BANNER_ADS_PLACEMENT = "AdsMBannerAdsPlacement";
	public const string U_KEY_ADS_M_REWARD_ADS_PLACEMENT = "AdsMRewardAdsPlacement";
	public const string U_KEY_ADS_M_FULLSCREEN_ADS_PLACEMENT = "AdsMFullscreenAdsPlacement";

#if ADMOB_ENABLE
	public const float U_OFFSET_ADMOB_BANNER_ADS = 5.0f;

	public const string U_KEY_ADS_M_ADMOB_BANNER_ADS_LOAD_CALLBACK = "AdsMAdmobBannerAdsLoadCallback";
	public const string U_KEY_ADS_M_ADMOB_BANNER_ADS_LOAD_FAIL_CALLBACK = "AdsMAdmobBannerAdsLoadFailCallback";
	public const string U_KEY_ADS_M_ADMOB_BANNER_ADS_CLOSE_CALLBACK = "AdsMAdmobBannerAdsCloseCallback";

	public const string U_KEY_ADS_M_ADMOB_REWARD_ADS_LOAD_FAIL_CALLBACK = "AdsMAdmobRewardAdsLoadFailCallback";
	public const string U_KEY_ADS_M_ADMOB_REWARD_ADS_CLOSE_CALLBACK = "AdsMAdmobRewardAdsCloseCallback";
	public const string U_KEY_ADS_M_ADMOB_REWARD_ADS_RECEIVE_REWARD_CALLBACK = "AdsMAdmobRewardAdsReceiveRewardCallback";

	public const string U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK = "AdsMAdmobFullscreenAdsLoadFailCallback";
	public const string U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_CLOSE_CALLBACK = "AdsMAdmobFullscreenAdsCloseCallback";

	public const string U_KEY_ADS_M_ADMOB_NATIVE_ADS_LOAD_CALLBACK = "AdsMAdmobNativeAdsLoadCallback";
	public const string U_KEY_ADS_M_ADMOB_NATIVE_ADS_LOAD_FAIL_CALLBACK = "AdsMAdmobNativeAdsLoadFailCallback";
#endif			// #if ADMOB_ENABLE

#if UNITY_ADS_ENABLE
	public const string U_KEY_ADS_M_UNITY_ADS_BANNER_ADS_LOAD_CALLBACK = "AdsMUnityAdsBannerAdsLoadCallback";
	public const string U_KEY_ADS_M_UNITY_ADS_BANNER_ADS_LOAD_FAIL_CALLBACK = "AdsMUnityAdsBannerAdsLoadFailCallback";

	public const string U_KEY_ADS_M_UNITY_ADS_LOAD_CALLBACK = "AdsMUnityAdsLoadCallback";
	public const string U_KEY_ADS_M_UNITY_ADS_LOAD_FAIL_CALLBACK = "AdsMUnityAdsLoadFailCallback";
	public const string U_KEY_ADS_M_UNITY_ADS_CLOSE_CALLBACK = "AdsMUnityAdsCloseCallback";
#endif			// #if UNITY_ADS_ENABLE

#if IRON_SOURCE_ENABLE
	public const string U_KEY_ADS_M_IRON_SOURCE_BANNER_ADS_LOAD_CALLBACK = "AdsMIronSourceBannerAdsLoadCallback";
	public const string U_KEY_ADS_M_IRON_SOURCE_BANNER_ADS_LOAD_FAIL_CALLBACK = "AdsMIronSourceBannerAdsLoadFailCallback";

	public const string U_KEY_ADS_M_IRON_SOURCE_REWARD_ADS_LOAD_FAIL_CALLBACK = "AdsMIronSourceRewardAdsLoadFailCallback";
	public const string U_KEY_ADS_M_IRON_SOURCE_REWARD_ADS_CLOSE_CALLBACK = "AdsMIronSourceRewardAdsCloseCallback";
	public const string U_KEY_ADS_M_IRON_SOURCE_REWARD_ADS_RECEIVE_REWARD_CALLBACK = "AdsMIronSourceRewardAdsReceiveRewardCallback";
	public const string U_KEY_ADS_M_IRON_SOURCE_REWARD_ADS_CHANGE_STATE_CALLBACK = "AdsMIronSourceRewardAdsChangeStateCallback";

	public const string U_KEY_ADS_M_IRON_SOURCE_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK = "AdsMIronSourceFullscreenAdsLoadFailCallback";
	public const string U_KEY_ADS_M_IRON_SOURCE_FULLSCREEN_ADS_CLOSE_CALLBACK = "AdsMIronSourceFullscreenAdsCloseCallback";
#endif			// #if IRON_SOURCE_ENABLE

#if APP_LOVIN_ENABLE
	public const string U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_LOAD_FAIL_CALLBACK = "AdsMAppLovinRewardAdsLoadFailCallback";
	public const string U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_CLOSE_CALLBACK = "AdsMAppLovinRewardAdsCloseCallback";
	public const string U_KEY_ADS_M_APP_LOVIN_REWARD_ADS_RECEIVE_REWARD_CALLBACK = "AdsMAppLovinRewardAdsReceiveRewardCallback";

	public const string U_KEY_ADS_M_APP_LOVIN_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK = "AdsMAppLovinFullscreenAdsLoadFailCallback";
	public const string U_KEY_ADS_M_APP_LOVIN_FULLSCREEN_ADS_CLOSE_CALLBACK = "AdsMAppLovinFullscreenAdsCloseCallback";
#endif			// #if APP_LOVIN_ENABLE
	// 키 }

	// 식별자 {
#if ADMOB_ENABLE
	public const string U_TEST_ADS_ID_ADMOB_BANNER_ADS = "ca-app-pub-3940256099942544/6300978111";
	public const string U_TEST_ADS_ID_ADMOB_REWARD_ADS = "ca-app-pub-3940256099942544/5224354917";
	public const string U_TEST_ADS_ID_ADMOB_NATIVE_ADS = "ca-app-pub-3940256099942544/2247696110";
	public const string U_TEST_ADS_ID_ADMOB_FULLSCREEN_ADS = "ca-app-pub-3940256099942544/1033173712";
#endif			// #if ADMOB_ENABLE
	// 식별자 }
#endif			// #if ADS_ENABLE

#if FLURRY_ENABLE || FACEBOOK_ENABLE || FIREBASE_ENABLE || UNITY_SERVICE_ENABLE
	// 시간
	public const float U_TIMEOUT_FLURRY_NETWORK_CONNECTION = 60.0f;

	// 이름
	public const string U_LOG_NAME_APP_LAUNCH = "AppLaunch";

	// 키 {
	public const string U_LOG_KEY_USER_ID = "UserID";
	public const string U_LOG_KEY_PLATFORM = "Platform";
	public const string U_LOG_KEY_LOG_TIME = "LogTime";
	public const string U_LOG_KEY_INSTALL_TIME = "InstallTime";

	public const string U_LOG_KEY_USER_INFO = "UserInfo";
	public const string U_LOG_KEY_USER_TYPE = "UserType";
	public const string U_LOG_KEY_DEVICE_ID = "DeviceID";
	public const string U_LOG_KEY_COUNTRY_CODE = "CountryCode";

	public const string U_LOG_KEY_PARAMETER_A = "ParameterA";
	public const string U_LOG_KEY_PARAMETER_B = "ParameterB";
	public const string U_LOG_KEY_PARAMETER_C = "ParameterC";
	public const string U_LOG_KEY_PARAMETER_D = "ParameterD";
	public const string U_LOG_KEY_PARAMETER_E = "ParameterE";
	public const string U_LOG_KEY_PARAMETER_F = "ParameterF";
	// 키 }

#if FLURRY_ENABLE && FLURRY_ANALYTICS_ENABLE
	// 토큰
	public const string U_TOKEN_FLURRY_ANALYTICS_LOG_DATA = ",";
#endif			// #if FLURRY_ENABLE && FLURRY_ANALYTICS_ENABLE

#if FACEBOOK_ENABLE
	// 키
	public const string U_KEY_FACEBOOK_M_INIT_CALLBACK = "FacebookMInitCallback";
	public const string U_KEY_FACEBOOK_M_LOGIN_CALLBACK = "FacebookMLoginCallback";
	public const string U_KEY_FACEBOOK_M_VIEW_STATE_SHOW_CALLBACK = "FacebookMViewStateShowCallback";
	public const string U_KEY_FACEBOOK_M_VIEW_STATE_CLOSE_CALLBACK = "FacebookMViewStateCloseCallback";

#if FACEBOOK_ANALYTICS_ENABLE
	// 토큰
	public const string U_TOKEN_FACEBOOK_ANALYTICS_LOG_DATA = ",";
#endif			// #if FACEBOOK_ANALYTICS_ENABLE
#endif			// #if FACEBOOK_ENABLE

#if FIREBASE_ENABLE
#if FIREBASE_REMOTE_CONFIG_ENABLE
	// 시간
	public const System.TimeSpan U_DEF_TIMEOUT_FIREBASE_FETCH_CONFIG_DATA = new System.TimeSpan(0, 0, 30);
#endif			// #if FIREBASE_REMOTE_CONFIG_ENABLE

#if FIREBASE_DATABASE_ENABLE
	// 노드 이름
	public const string U_NODE_NAME_FIREBASE_USER_INFO_LIST = "UserInfoList";
	public const string U_NODE_NAME_FIREBASE_PURCHASE_INFO_LIST = "PurchaseInfoList";
#endif			// #if FIREBASE_DATABASE_ENABLE

#if FIREBASE_REMOTE_CONFIG_ENABLE
	// 키 {
	public const string U_CONFIG_KEY_FIREBASE_MAC_VERSION_INFO = "MacVersionInfo";
	public const string U_CONFIG_KEY_FIREBASE_WINDOWS_VERSION_INFO = "WindowsVersionInfo";

	public const string U_CONFIG_KEY_FIREBASE_IOS_VERSION_INFO = "iOSVersionInfo";

	public const string U_CONFIG_KEY_FIREBASE_GOOGLE_VERSION_INFO = "AndroidVersionInfo";
	public const string U_CONFIG_KEY_FIREBASE_ONE_STORE_VERSION_INFO = "OneStoreVersionInfo";
	public const string U_CONFIG_KEY_FIREBASE_GALAXY_STORE_VERSION_INFO = "GalaxyStoreVersionInfo";

	public const string U_CONFIG_KEY_FIREBASE_BUILD_NUMBER = "BuildNumber";
	public const string U_CONFIG_KEY_FIREBASE_BUILD_VERSION = "BuildVersion";

#if UNITY_IOS
	public const string U_CONFIG_KEY_FIREBASE_VERSION_INFO = KDefine.U_CONFIG_KEY_FIREBASE_IOS_VERSION_INFO;
#elif UNITY_ANDROID
#if ONE_STORE_PLATFORM
	public const string U_CONFIG_KEY_FIREBASE_VERSION_INFO = KDefine.U_CONFIG_KEY_FIREBASE_ONE_STORE_VERSION_INFO;
#elif GALAXY_STORE_PLATFORM
	public const string U_CONFIG_KEY_FIREBASE_VERSION_INFO = KDefine.U_CONFIG_KEY_FIREBASE_GALAXY_STORE_VERSION_INFO;
#else
	public const string U_CONFIG_KEY_FIREBASE_VERSION_INFO = KDefine.U_CONFIG_KEY_FIREBASE_GOOGLE_VERSION_INFO;
#endif			// #if ONE_STORE_PLATFORM
#else
#if UNITY_STANDALONE_WIN
	public const string U_CONFIG_KEY_FIREBASE_VERSION_INFO = KDefine.U_CONFIG_KEY_FIREBASE_WINDOWS_VERSION_INFO;
#else
	public const string U_CONFIG_KEY_FIREBASE_VERSION_INFO = KDefine.U_CONFIG_KEY_FIREBASE_MAC_VERSION_INFO;
#endif			// #if UNITY_STANDALONE_WIN
#endif			// #if UNITY_IOS
	// 키 }
#endif			// #if FIREBASE_REMOTE_CONFIG_ENABLE

#if FIREBASE_ANALYTICS_ENABLE
	// 토큰
	public const string U_TOKEN_FIREBASE_ANALYTICS_LOG_DATA = ",";
#endif			// #if FIREBASE_ANALYTICS_ENABLE
#endif			// #if FIREBASE_ENABLE

#if UNITY_SERVICE_ENABLE
#if UNITY_SERVICE_CRASHLYTICS_ENABLE
	// 크기
	public const uint U_SIZE_UNITY_SERVICE_CRASH_LOG_BUFFER = 50;
#endif			// #if UNITY_SERVICE_CRASHLYTICS_ENABLE

#if UNITY_SERVICE_ANALYTICS_ENABLE
	// 토큰
	public const string U_TOKEN_UNITY_SERVICE_ANALYTICS_LOG_DATA = ",";
#endif			// #if UNITY_SERVICE_ANALYTICS_ENABLE
#endif			// #if UNITY_SERVICE_ENABLE
#endif			// #if FLURRY_ENABLE || FACEBOOK_ENABLE || FIREBASE_ENABLE || UNITY_SERVICE_ENABLE

#if PURCHASE_ENABLE
	// 메세지
	public const string U_MSG_INVALID_PRODUCT_TYPE = "해당 상품 타입을 지원하지않습니다.";

	// 키
	public const string U_KEY_PURCHASE_M_RESTORE_CALLBACK = "PurchaseMRestoreCallback";
#endif			// #if PURCHASE_ENABLE

#if GAME_CENTER_ENABLE
	// 키
	public const string U_KEY_GAME_CM_LOGIN_CALLBACK = "GameCMLoginCallback";
	public const string U_KEY_GAME_CM_LOAD_SCORES_CALLBACK = "GameCMLoadScoresCallback";
	public const string U_KEY_GAME_CM_UPDATE_SCORE_CALLBACK = "GameCMUpdateScoreCallback";
	public const string U_KEY_GAME_CM_UPDATE_ACHIEVEMENT_CALLBACK = "GameCMUpdateAchievementCallback";

	// 코드
	public const string U_AUTH_CODE_GAME_CM_UNKNOWN = "Unknown";
#endif			// #if GAME_CENTER_ENABLE

#if !MESSAGE_PACK_ENABLE
	// 메세지
	public const string U_MSG_NEED_MESSAGE_PACK = "MessagePack 라이브러리가 필요합니다.";
#endif			// #if !MESSAGE_PACK_ENABLE
	#endregion			// 조건부 상수

	#region 조건부 런타임 상수
#if UNITY_IOS
	// 버전
	public static readonly System.Version U_MIN_VERSION_LOGIN_WITH_APPLE = new System.Version(13, 0, 0);

	// 경로
	public static readonly string U_OBJ_PATH_LOGIN_WITH_APPLE = string.Format("{0}{1}U_LogInWithApple", KDefine.B_DIR_PATH_PREFABS, KDefine.B_DIR_PATH_UTILITY_BASE);

#if HAPTIC_FEEDBACK_ENABLE
	// 햅틱 피드백 지원 모델
	public static readonly DeviceGeneration[] U_HAPTIC_FEEDBACK_SUPPORT_MODELS = new DeviceGeneration[] {
		DeviceGeneration.iPhone7, DeviceGeneration.iPhone7Plus, DeviceGeneration.iPhone8, DeviceGeneration.iPhone8Plus,
		DeviceGeneration.iPhoneX, DeviceGeneration.iPhoneXR, DeviceGeneration.iPhoneXS, DeviceGeneration.iPhoneXSMax,
		DeviceGeneration.iPhone11, DeviceGeneration.iPhone11Pro, DeviceGeneration.iPhone11ProMax, DeviceGeneration.iPhoneUnknown
	};
#endif			// #if HAPTIC_FEEDBACK_ENABLE
#endif			// #if UNITY_IOS

#if ADS_ENABLE
#if ADMOB_ENABLE
	// 크기
	public static readonly AdSize U_SIZE_ADMOB_BANNER = new AdSize(AdSize.FullWidth, 50);
#endif			// #if ADMOB_ENABLE

#if IRON_SOURCE_ENABLE
	// 크기
	public static readonly IronSourceBannerSize U_SIZE_IRON_SOURCE_BANNER = new IronSourceBannerSize(320, 50);
#endif			// #if IRON_SOURCE_ENABLE
#endif			// #if ADS_ENABLE

#if ADS_ENABLE || TENJIN_ENABLE || FLURRY_ENABLE || FIREBASE_ENABLE
	// 경로
	public static readonly string U_SCRIPTABLE_PATH_G_PLUGIN_INFO_TABLE = string.Format("{0}{1}G_PluginInfoTable", KDefine.B_DIR_PATH_SCRIPTABLES, KDefine.B_DIR_PATH_GLOBAL_BASE);
#endif			// #if ADS_ENABLE || TENJIN_ENABLE || FLURRY_ENABLE || FIREBASE_ENABLE

#if FACEBOOK_ENABLE
	// 권한
	public static readonly string[] U_DEF_PERMISSIONS_FACEBOOK = new string[] {
		"public_profile", "email"
	};
#endif			// #if FACEBOOK_ENABLE

#if PURCHASE_ENABLE
	// 경로
	public static readonly string U_DATA_PATH_PURCHASE_M_PRODUCT_ID_LIST = string.Format("{0}PurchaseProductIDList.bytes", KDefine.B_DIR_PATH_WRITABLE);
	public static readonly string U_SCRIPTABLE_PATH_G_PRODUCT_INFO_TABLE = string.Format("{0}{1}G_ProductInfoTable", KDefine.B_DIR_PATH_SCRIPTABLES, KDefine.B_DIR_PATH_GLOBAL_BASE);
#endif			// #if PURCHASE_ENABLE
	#endregion			// 조건부 런타임 상수
}
