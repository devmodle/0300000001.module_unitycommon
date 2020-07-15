using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR && UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif			// #if UNITY_EDITOR && UNITY_IOS

#if !USE_CUSTOM_PROJECT_OPTION
#if MESSAGE_PACK_ENABLE
using MessagePack;

//! 기본 정보
[Union(0, typeof(CAppInfo))]
[Union(1, typeof(CUserInfo))]
[MessagePackObject]
[System.Serializable]
public abstract class CBaseInfo : IMessagePackSerializationCallbackReceiver {
	#region 변수
	[Key(0)] public STVersionInfo m_stVersionInfo;

	[Key(1)] public Dictionary<string, bool> m_oBoolList = new Dictionary<string, bool>();
	[Key(2)] public Dictionary<string, int> m_oIntList = new Dictionary<string, int>();
	[Key(3)] public Dictionary<string, float> m_oFloatList = new Dictionary<string, float>();
	[Key(4)] public Dictionary<string, string> m_oStringList = new Dictionary<string, string>();
	#endregion			// 변수

	#region 인터페이스
	//! 직렬화 될 경우
	public virtual void OnBeforeSerialize() {
		// Do Nothing
	}

	//! 역직렬화 되었을 경우
	public virtual void OnAfterDeserialize() {
		m_oBoolList = m_oBoolList ?? new Dictionary<string, bool>();
		m_oIntList = m_oIntList ?? new Dictionary<string, int>();
		m_oFloatList = m_oFloatList ?? new Dictionary<string, float>();
		m_oStringList = m_oStringList ?? new Dictionary<string, string>();
	}
	#endregion			// 인터페이스

	#region 함수
	//! 생성자
	public CBaseInfo(string a_oVersion) {
		m_stVersionInfo = Func.MakeDefVersionInfo(a_oVersion);
	}
	#endregion			// 함수
}

//! 어플리케이션 커스텀 타입 래퍼
[MessagePackObject]
public struct STAppCustomTypeWrapper {

}
#endif			// #if MESSAGE_PACK_ENABLE

//! 전역 어플리케이션 상수
public static partial class KAppDefine {
	#region 기본
	// 디바이스 {
	public const bool G_MULTI_TOUCH_ENABLE = false;

	public const int G_MOBILE_TARGET_FRAME_RATE = 60;
	public const int G_DESKTOP_TARGET_FRAME_RATE = 60;
	public const int G_CONSOLE_TARGET_FRAME_RATE = 60;
	public const int G_HANDHELD_CONSOLE_TARGET_FRAME_RATE = 60;

	public const EQualityLevel G_DEF_QUALITY_LEVEL = EQualityLevel.AUTO;
	// 디바이스 }

	// 간격 {
	public const float G_V_OFFSET_ALERT_P_TITLE = 70.0f;

	public const float G_V_OFFSET_ALERT_P_BTN = 45.0f;
	public const float G_H_OFFSET_ALERT_P_BTN = 45.0f;

	public const float G_V_EXTRA_OFFSET_ALERT_P_TITLE = 0.0f;
	public const float G_V_EXTRA_OFFSET_ALERT_P_MSG = 15.0f;
	public const float G_V_EXTRA_OFFSET_ALERT_P_BTN = 0.0f;

	public const float G_V_OFFSET_ALERT_P_BG = 25.0f;
	public const float G_H_OFFSET_ALERT_P_BG = 165.0f;
	// 간격 }

	// 이름
	public const string G_NAME_PROJECT_ROOT = "Library";
	public const string G_NAME_UNITY_PROJECT_ROOT = KAppDefine.G_NAME_PROJECT_ROOT;
	#endregion			// 기본

	#region 런타임 상수
	// 색상 {
	public static readonly Color G_DEF_COLOR_CAMERA_BG = Color.black;

	public static readonly Color G_DEF_COLOR_POPUP_BG = new Color(0.0f, 0.0f, 0.0f, 0.785f);
	public static readonly Color G_DEF_COLOR_ACTIVITY_INDICATOR_BG = new Color(0.0f, 0.0f, 0.0f, 0.785f);
	// 색상 }

	// 회전
	public static readonly Vector3 G_ROTATION_MAIN_LIGHT = new Vector3(45.0f, 45.0f, 0.0f);

	// 정렬 순서 정보 {
	public static readonly KeyValuePair<string, int> G_SORTING_ORDER_INFO_OBJ_CANVAS = new KeyValuePair<string, int>(KDefine.U_SORTING_LAYER_DEF, 0);

#if CAMERA_STACK_ENABLE
	public static readonly KeyValuePair<string, int> G_SORTING_ORDER_INFO_UI_CANVAS = new KeyValuePair<string, int>(KDefine.U_SORTING_LAYER_DEF, 0);
#else
	public static readonly KeyValuePair<string, int> G_SORTING_ORDER_INFO_UI_CANVAS = new KeyValuePair<string, int>(KDefine.U_SORTING_LAYER_DEF_UI, 0);
#endif			// #if CAMERA_STACK_ENABLE
	// 정렬 순서 정보 }
	#endregion			// 런타임 상수

	#region 조건부 상수
#if UNITY_EDITOR
	// 광원
	public const LightmapBakeType G_DEF_LIGHTMAP_BAKE_TYPE_DIRECTIONAL = LightmapBakeType.Realtime;
#endif			// #if UNITY_EDITOR

#if MESSAGE_PACK_ENABLE
	// 버전
	public const string G_VERSION_APP_INFO = "1.0.0";
	public const string G_VERSION_USER_INFO = "1.0.0";
#endif			// #if MESSAGE_PACK_ENABLE

	// 광원 {
#if LIGHT_ENABLE && SHADOW_ENABLE
#if SOFT_SHADOW_ENABLE
	public const LightShadows G_DEF_LIGHT_SHADOW_TYPE = LightShadows.Soft;
#else
	public const LightShadows G_DEF_LIGHT_SHADOW_TYPE = LightShadows.Hard;
#endif			// #if SOFT_SHADOW_ENABLE
#else
	public const LightShadows G_DEF_LIGHT_SHADOW_TYPE = LightShadows.None;
#endif			// #if LIGHT_ENABLE && SHADOW_ENABLE
	// 광원 }
	#endregion			// 조건부 상수

	#region 조건부 런타임 상수
#if UNITY_EDITOR && UNITY_IOS
	// 프레임워크
	public static readonly string[] G_EXTRA_FRAMEWORKS_IOS = new string[] {
		"AuthenticationServices.framework"
	};

	// 호환성 타입
	public static readonly PBXCapabilityType[] G_EXTRA_CAPABILITY_TYPES_IOS = new PBXCapabilityType[] {
		PBXCapabilityType.GameCenter,
		PBXCapabilityType.SignInWithApple,
		PBXCapabilityType.PushNotifications
	};
#endif			// #if UNITY_EDITOR && UNITY_IOS
	#endregion			// 조건부 런타임 상수
}
#endif			// #if !USE_CUSTOM_PROJECT_OPTION
