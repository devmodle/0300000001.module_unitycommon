#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE
/** 서브 전역 상수 */
public static partial class KDefine {
	#region 기본
	// 이름
	public const string G_OBJ_N_STORE_POPUP = "STORE_POPUP";
	public const string G_OBJ_N_SETTINGS_POPUP = "SETTINGS_POPUP";
	public const string G_OBJ_N_SYNC_POPUP = "SYNC_POPUP";
	public const string G_OBJ_N_DAILY_MISSION_POPUP = "DAILY_MISSION_POPUP";
	public const string G_OBJ_N_FREE_REWARD_POPUP = "FREE_REWARD_POPUP";
	public const string G_OBJ_N_DAILY_REWARD_POPUP = "DAILY_REWARD_POPUP";
	public const string G_OBJ_N_COINS_BOX_POPUP = "COINS_BOX_POPUP";
	public const string G_OBJ_N_REWARD_ACQUIRE_POPUP = "REWARD_ACQUIRE_POPUP";
	public const string G_OBJ_N_COINS_BOX_ACQUIRE_POPUP = "COINS_BOX_ACQUIRE_POPUP";
	public const string G_OBJ_N_CONTINUE_POPUP = "CONTINUE_POPUP";
	public const string G_OBJ_N_RESULT_POPUP = "RESULT_POPUP";
	public const string G_OBJ_N_PAUSE_POPUP = "PAUSE_POPUP";
	public const string G_OBJ_N_RESUME_POPUP = "RESUME_POPUP";
	public const string G_OBJ_N_PRODUCT_TRADE_POPUP = "PRODUCT_TRADE_POPUP";
	public const string G_OBJ_N_FOCUS_POPUP = "FOCUS_POPUP";
	public const string G_OBJ_N_TUTORIAL_POPUP = "TUTORIAL_POPUP";
	
	// 설정 팝업 {
	public const string G_IMG_P_SETTINGS_P_SND_ON = "G_SndOn";
	public const string G_IMG_P_SETTINGS_P_SND_OFF = "G_SndOff";

	public const string G_IMG_P_SETTINGS_P_BG_SND_ON = "G_BGSndOn";
	public const string G_IMG_P_SETTINGS_P_BG_SND_OFF = "G_BGSndOff";

	public const string G_IMG_P_SETTINGS_P_FX_SNDS_ON = "G_FXSndsOn";
	public const string G_IMG_P_SETTINGS_P_FX_SNDS_OFF = "G_FXSndsOff";

	public const string G_IMG_P_SETTINGS_P_VIBRATE_ON = "G_VibrateOn";
	public const string G_IMG_P_SETTINGS_P_VIBRATE_OFF = "G_VibrateOff";

	public const string G_IMG_P_SETTINGS_P_NOTI_ON = "G_NotiOn";
	public const string G_IMG_P_SETTINGS_P_NOTI_OFF = "G_NotiOff";
	// 설정 팝업 }
	#endregion			// 기본

	#region 런타임 상수
	// 일일 보상
	public static readonly List<ERewardKinds> G_REWARDS_KINDS_DAILY_REWARD_LIST = new List<ERewardKinds>() {
		// Do Something
	};

	// 상점 상품 종류
	public static readonly List<EProductKinds> G_PRODUCT_KINDS_STORE_LIST = new List<EProductKinds>() {
		// Do Something
	};

	// 특수 패키지 상품 종류
	public static readonly List<EProductKinds> G_PRODUCT_KINDS_SPECIAL_PKGS_LIST = new List<EProductKinds>() {
		// Do Something
	};
	#endregion			// 런타임 상수
}

/** 서브 타이틀 씬 상수 */
public static partial class KDefine {
	#region 기본
	
	#endregion			// 기본
}

/** 서브 메인 씬 상수 */
public static partial class KDefine {
	#region 기본
	// 개수
	public const int MS_MAX_NUM_LEVELS_IN_ROW = 1;
	public const int MS_MAX_NUM_STAGES_IN_ROW = 1;
	public const int MS_MAX_NUM_CHAPTERS_IN_ROW = 1;
	#endregion			// 기본
}

/** 서브 게임 씬 상수 */
public static partial class KDefine {
	#region 기본
	// 단위
	public const int GS_MIN_LEVEL_ENABLE_REWARD_ADS_WATCH = 0;
	#endregion			// 기본

	#region 런타임 상수
	// 경로
	public static readonly string GS_IMG_P_FMT_BG = $"{KCDefine.B_DIR_P_IMAGES}{KCDefine.B_DIR_P_GAME_SCENE}BG_{"{0:00}"}_{"{0:000}"}_{"{0:0000}"}";
	public static readonly string GS_IMG_P_FMT_TOP_BG = $"{KCDefine.B_DIR_P_IMAGES}{KCDefine.B_DIR_P_GAME_SCENE}TopBG_{"{0:00}"}_{"{0:000}"}_{"{0:0000}"}";
	public static readonly string GS_IMG_P_FMT_BOTTOM_BG = $"{KCDefine.B_DIR_P_IMAGES}{KCDefine.B_DIR_P_GAME_SCENE}BottomBG_{"{0:00}"}_{"{0:000}"}_{"{0:0000}"}";

	// 정렬 순서 {
	public static readonly STSortingOrderInfo GS_SORTING_OI_BG = new STSortingOrderInfo() {
		m_nOrder = sbyte.MaxValue * 0, m_oLayer = KCDefine.U_SORTING_L_UNDERGROUND
	};

	public static readonly STSortingOrderInfo GS_SORTING_OI_TOP_BG = new STSortingOrderInfo() {
		m_nOrder = sbyte.MaxValue * 1, m_oLayer = KCDefine.U_SORTING_L_UNDERGROUND
	};

	public static readonly STSortingOrderInfo GS_SORTING_OI_BOTTOM_BG = new STSortingOrderInfo() {
		m_nOrder = sbyte.MaxValue * 1, m_oLayer = KCDefine.U_SORTING_L_UNDERGROUND
	};
	// 정렬 순서 }
	#endregion			// 런타임 상수
}

/** 서브 로딩 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion			// 기본
}

/** 서브 중첩 씬 상수 */
public static partial class KDefine {
	#region 기본
	
	#endregion			// 기본
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
