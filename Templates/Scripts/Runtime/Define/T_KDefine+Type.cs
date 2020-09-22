using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#if MSG_PACK_ENABLE
using MessagePack;
#endif			// #if MSG_PACK_ENABLE

#region 기본
//! 게임 속성
[System.Serializable]
public struct STGameConfig {

}
#endregion			// 기본

#region 조건부 타입
#if MSG_PACK_ENABLE
//! 커스터 타입 랩퍼
[MessagePackObject]
public struct STCustomTypeWrapper {

}
#endif			// #if MSG_PACK_ENABLE
#endregion			// 조건부 타입
#endif			// #if NEVER_USE_THIS
