using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 기본 상수
public static partial class KDefine {
	#region 기본
	// 버전
	public const string B_VERSION_APP_INFO = "1.0.0";
	public const string B_VERSION_USER_INFO = "1.0.0";
	public const string B_VERSION_GAME_INFO = "1.0.0";
	#endregion			// 기본

	#region 런타임 상수
	// 경로
	public static readonly string B_DATA_PATH_APP_INFO = string.Format("{0}AppInfo.bytes", KCDefine.B_DIR_PATH_WRITABLE);
	public static readonly string B_DATA_PATH_USER_INFO = string.Format("{0}UserInfo.bytes", KCDefine.B_DIR_PATH_WRITABLE);
	public static readonly string B_DATA_PATH_GAME_INFO = string.Format("{0}GameInfo.bytes", KCDefine.B_DIR_PATH_WRITABLE);

	// 디바이스 {
	public static readonly SystemLanguage[] B_SUPPORT_LANGUAGES = new SystemLanguage[] {
		SystemLanguage.Korean, SystemLanguage.English
	};
	
	public static readonly string[] B_SUPPORT_COUNTRY_CODES = new string[] {
		KCDefine.B_KOREA_COUNTRY_CODE
	};
	// 디바이스 }
	#endregion			// 런타임 상수
}
#endif			// #if NEVER_USE_THIS
