using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 기본 상수
public static partial class KDefine {
	#region 기본
	
	#endregion			// 기본

	#region 런타임 상수
	// 경로
	public static readonly string B_DATA_PATH_GAME_INFO = string.Format("{0}GameInfo.bytes", KCDefine.B_DIR_PATH_WRITABLE);
	#endregion			// 런타임 상수

	#region 조건부 상수
#if MSG_PACK_ENABLE
	public const string B_VERSION_GAME_INFO = "1.0.0";
#endif			// #if MSG_PACK_ENABLE
	#endregion			// 조건부 상수
}
#endif			// #if NEVER_USE_THIS
