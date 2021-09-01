using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessagePack;

#if NEVER_USE_THIS
#if UNITY_EDITOR || UNITY_STANDALONE
#region 기본
//! 서브 에디터 레벨 생성 정보
public class CSubEditorLevelCreateInfo : CEditorLevelCreateInfo {

}

//! 에디터 타입 랩퍼
[MessagePackObject]
public struct STEditorTypeWrapper {

}
#endregion			// 기본
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
#endif			// #if NEVER_USE_THIS
