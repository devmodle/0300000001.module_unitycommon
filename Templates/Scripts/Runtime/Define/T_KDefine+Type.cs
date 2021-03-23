using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

#if NEVER_USE_THIS
/*
MsgPack 키 인덱스 지정 방식
:
- 5 ~ 29 : 복합 형식 데이터
- 31 ~ 69 : 리스트 형식 데이터
- 71 ~ 109 : 딕셔너리 형식 데이터

복합 형식 데이터 세부 범위
:
- 5 ~ 19 : 값 형식 데이터
- 21 ~ 29 : 참조 형식 데이터

리스트 형식 데이터 세부 범위
:
- 31 ~ 39 : 값 (기본) 형식 데이터 (1 ~ 4 : 기본, 5 ~ 9 : 열거형)
- 41 ~ 49 : 값 (복합) 형식 데이터
- 51 ~ 59 : 참조 (기본) 형식 데이터
- 61 ~ 69 : 참조 (복합) 형식 데이터

딕셔너리 형식 데이터 세부 범위
:
- 71 ~ 79 : 값 (기본) 형식 데이터 (1 ~ 4 : 기본, 5 ~ 9 : 열거형)
- 81 ~ 89 : 값 (복합) 형식 데이터
- 91 ~ 99 : 참조 (기본) 형식 데이터
- 101 ~ 109 : 참조 (복합) 형식 데이터
*/
#region 기본
//! 게임 속성
[System.Serializable]
public struct STGameConfig {

}

//! 판매 아이템 정보
[System.Serializable]
public struct STSaleItemInfo {
	public int m_nNumItems;
	public EItemKinds m_eItemKinds;
}

//! 커스터 타입 랩퍼
[MessagePackObject]
public struct STCustomTypeWrapper {

}
#endregion			// 기본
#endif			// #if NEVER_USE_THIS
