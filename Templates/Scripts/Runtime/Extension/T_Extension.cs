using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 기본 확장 클래스
public static partial class Extension {
	#region 클래스 함수
	//! JSON 문자열 => 유저 정보로 변환한다
	public static KeyValuePair<CUserInfo, CCommonUserInfo> ExJSONStrToUserInfos(this string a_oSender) {
		CAccess.Assert(a_oSender.ExIsValid());
		
#if FIREBASE_MODULE_ENABLE
		var oJSONNode = SimpleJSON.JSON.Parse(a_oSender) as SimpleJSON.JSONClass;

		string oUserInfoStr = oJSONNode[KCDefine.B_KEY_JSON_USER_INFO_DATA];
		string oCommonUserInfoStr = oJSONNode[KCDefine.B_KEY_JSON_COMMON_USER_INFO_DATA];

		var oUserInfo = oUserInfoStr.ExMsgPackJSONStrToObj<CUserInfo>();
		return new KeyValuePair<CUserInfo, CCommonUserInfo>(oUserInfo, oCommonUserInfoStr.ExMsgPackJSONStrToObj<CCommonUserInfo>());
#else
		return new KeyValuePair<CUserInfo, CCommonUserInfo>(null, null);
#endif			// #if FIREBASE_MODULE_ENABLE
	}
	
	//! JSON 문자열 => 지급 아이템 정보로 변환한다
	public static List<STPostItemInfo> ExJSONStrToPostItemInfos(this string a_oSender) {
		CAccess.Assert(a_oSender.ExIsValid());
		var oPostItemInfoList = new List<STPostItemInfo>();

#if FIREBASE_MODULE_ENABLE
		var oJSONNode = SimpleJSON.JSON.Parse(a_oSender) as SimpleJSON.JSONClass;
		var oPostItemInfos = oJSONNode[KCDefine.B_KEY_JSON_ROOT_DATA];

		for(int i = 0; i < oPostItemInfos.Count; ++i) {
			var oPostItemInfoStr = oPostItemInfos[i].ToString();
			var stPostItemInfo = oPostItemInfoStr.ExJSONStrToObj<STPostItemInfo>();

			oPostItemInfoList.Add(stPostItemInfo);
		}
#endif			// #if FIREBASE_MODULE_ENABLE

		return oPostItemInfoList;
	}
	#endregion			// 클래스 함수
}
#endif			// #if NEVER_USE_THIS
