using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 기본 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수
	/** JSON 문자열 => 획득 정보로 변환한다 */
	public static List<STAcquireInfo> ExJSONStrToAcquireInfos(this string a_oSender) {
		CAccess.Assert(a_oSender.ExIsValid());
		var oAcquireInfoList = new List<STAcquireInfo>();
		
#if FIREBASE_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
		var oJSONNode = SimpleJSON.JSON.Parse(a_oSender) as SimpleJSON.JSONClass;
		var oAcquireInfos = oJSONNode[KCDefine.B_KEY_JSON_ROOT_DATA];

		for(int i = 0; i < oAcquireInfos.Count; ++i) {
			oAcquireInfoList.Add(oAcquireInfos[i].ToString().ExJSONStrToObj<STAcquireInfo>());
		}
#endif			// #if FIREBASE_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE

		return oAcquireInfoList;
	}
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
