using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
//! 기본 확장 클래스
public static partial class Extension {
	#region 클래스 함수
	//! JSON 문자열 => 지급 아이템 정보로 변환한다
	public static List<STPostItemInfo> ExJSONStrToPostItemInfos(this string a_oSender) {
		CAccess.Assert(a_oSender.ExIsValid());
		var oPostItemInfoList = new List<STPostItemInfo>();

#if FIREBASE_MODULE_ENABLE
		var oJSONNode = SimpleJSON.JSON.Parse(a_oSender);
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

	#region 제네릭 클래스 함수
	//! 딕셔너리 => JSON 노드로 변환한다
	public static SimpleJSON.JSONNode ExToJSONNode<Key, Val>(this Dictionary<Key, Val> a_oSender) {
		CAccess.Assert(a_oSender != null);
		var oJSONNode = new SimpleJSON.JSONNode();

		foreach(var stKeyVal in a_oSender) {
			oJSONNode.Add(stKeyVal.Key.ToString(), stKeyVal.Value.ToString());
		}

		return oJSONNode;
	}
	#endregion			// 제네릭 클래스 함수
}
#endif			// #if NEVER_USE_THIS
