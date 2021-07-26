using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEVER_USE_THIS
#region 기본
//! 플레이 모드
public enum EPlayMode {
	NONE = -1,
	NORM,
	TEST,
	TUTORIAL,
	MAX_VAL
}

//! 레벨 모드
public enum ELevelMode {
	NONE = -1,
	EASY,
	NORM,
	HARD,
	MAX_VAL
}

//! 스테이지 모드
public enum EStageMode {
	NONE = -1,
	MAX_VAL
}

//! 챕터 모드
public enum EChapterMode {
	NONE = -1,
	MAX_VAL
}

//! 가격 타입
public enum EPriceType {
	NONE = -1,
	ADS,
	GOODS,
	PURCHASE,
	MAX_VAL
}

//! 가격 종류
public enum EPriceKinds {
	NONE = -1,

	#region 광고
	// 광고 가격 0
	[HideInInspector] ADS_PRICE,

	// 보상 광고 0
	ADS_REWARD = EPriceKinds.ADS_PRICE,
	#endregion			// 광고

	#region 재화
	// 재화 가격 10,000,000
	[HideInInspector] GOODS_PRICE = EPriceKinds.ADS_PRICE + KCDefine.B_UNIT_KINDS_PER_TYPE,

	// 코인 10,000,000
	GOODS_COIN = EPriceKinds.GOODS_PRICE,
	#endregion			// 재화

	MAX_VAL
}

//! 아이템 타입
public enum EItemType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	MAX_VAL
}

//! 아이템 종류
public enum EItemKinds {
	NONE = -1,

	#region 재화
	// 재화 아이템 0
	[HideInInspector] GOODS_ITEM,

	// 코인 0
	GOODS_COIN = EItemKinds.GOODS_ITEM,
	#endregion			// 재화

	#region 소모
	// 소모 아이템 10,000,000
	[HideInInspector] CONSUMABLE_ITEM =  EItemKinds.GOODS_ITEM + KCDefine.B_UNIT_KINDS_PER_TYPE,

	// 부스터 10,000,000
	[HideInInspector] CONSUMABLE_BOOSTER = EItemKinds.CONSUMABLE_ITEM,

	// 게임 아이템 10,010,000
	[HideInInspector] CONSUMABLE_GAME_ITEM = EItemKinds.CONSUMABLE_BOOSTER + KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE,

	// 힌트 10,010,100
	CONSUMABLE_GAME_ITEM_HINT = EItemKinds.CONSUMABLE_GAME_ITEM + KCDefine.B_UNIT_KINDS_PER_SUB_KINDS_TYPE,
	#endregion			// 소모

	#region 비소모
	// 비소모 아이템 20,000,000
	[HideInInspector] NON_CONSUMABLE_ITEM = EItemKinds.CONSUMABLE_ITEM + KCDefine.B_UNIT_KINDS_PER_TYPE,

	// 광고 제거 20,000,000
	NON_CONSUMABLE_REMOVE_ADS = EItemKinds.NON_CONSUMABLE_ITEM,
	#endregion			// 비소모

	MAX_VAL
}

//! 판매 아이템 타입
public enum ESaleItemType {
	NONE = -1,
	BOOSTER,
	GAME_ITEM,
	MAX_VAL
}

//! 판매 아이템 종류
public enum ESaleItemKinds {
	NONE = -1,

	#region 부스터
	// 부스터 판매 아이템 0
	[HideInInspector] BOOSTER_SALE_ITEM,
	#endregion			// 부스터

	#region 게임 아이템
	// 게임 아이템 판매 아이템 10,000,000
	[HideInInspector] GAME_ITEM_SALE_ITEM = ESaleItemKinds.BOOSTER_SALE_ITEM + KCDefine.B_UNIT_KINDS_PER_TYPE,

	// 힌트 10,000,000
	GAME_ITEM_HINT = ESaleItemKinds.GAME_ITEM_SALE_ITEM,
	#endregion			// 게임 아이템

	MAX_VAL
}

//! 판매 상품 타입
public enum ESaleProductType {
	NONE = -1,
	PKGS,
	SINGLE,
	MAX_VAL
}

//! 판매 상품 종류
public enum ESaleProductKinds {
	NONE = -1,

	#region 패키지
	// 패키지 판매 상품 0
	[HideInInspector] PKGS_SALE_PRODUCT,
	#endregion			// 패키지

	#region 단일
	// 단일 판매 상품 10,000,000
	[HideInInspector] SINGLE_SALE_PRODUCT = ESaleProductKinds.PKGS_SALE_PRODUCT + KCDefine.B_UNIT_KINDS_PER_TYPE,

	// 잔돈 10,000,000
	SINGLE_CHANGES = ESaleProductKinds.SINGLE_SALE_PRODUCT,

	// 광고 제거 10,010,000
	SINGLE_REMOVE_ADS = ESaleProductKinds.SINGLE_CHANGES + KCDefine.B_UNIT_KINDS_PER_KINDS_TYPE,
	#endregion			// 단일

	MAX_VAL
}

//! 미션 타입
public enum EMissionType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	MAX_VAL
}

//! 미션 종류
public enum EMissionKinds {
	NONE = -1,

	#region 자유
	// 자유 미션 0
	[HideInInspector] FREE_MISSION,
	#endregion			// 자유

	#region 일일
	// 일일 미션 10,000,000
	[HideInInspector] DAILY_MISSION = EMissionKinds.FREE_MISSION + KCDefine.B_UNIT_KINDS_PER_TYPE,
	#endregion			// 일일

	#region 이벤트
	// 이벤트 미션 20,000,000
	[HideInInspector] EVENT_MISSION = EMissionKinds.DAILY_MISSION + KCDefine.B_UNIT_KINDS_PER_TYPE,
	#endregion			// 이벤트

	MAX_VAL
}

//! 보상 타입
public enum ERewardType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	CLEAR,
	MAX_VAL
}

//! 보상 종류
public enum ERewardKinds {
	NONE = -1,

	#region 무료
	// 무료 보상
	[HideInInspector] FREE_REWARD,
	#endregion			// 무료

	#region 일일
	// 일일 보상 10,000,000
	[HideInInspector] DAILY_REWARD = ERewardKinds.FREE_REWARD + KCDefine.B_UNIT_KINDS_PER_TYPE,
	#endregion			// 일일

	#region 이벤트
	// 이벤트 보상 20,000,000
	[HideInInspector] EVENT_REWARD = ERewardKinds.DAILY_REWARD + KCDefine.B_UNIT_KINDS_PER_TYPE,
	#endregion			// 이벤트

	#region 클리어
	// 클리어 보상 30,000,000
	[HideInInspector] CLEAR_REWARD = ERewardKinds.EVENT_REWARD + KCDefine.B_UNIT_KINDS_PER_TYPE,
	#endregion			// 클리어

	MAX_VAL
}

//! 보상 수준
public enum ERewardQuality {
	NONE = -1,
	NORM,
	HIGH,
	SPECIAL,
	MAX_VAL
}

//! 보상 획득 팝업 타입
public enum ERewardAcquirePopupType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	CLEAR,
	MAX_VAL
}

//! 튜토리얼 타입
public enum ETutorialType {
	NONE = -1,
	PLAY,
	HELP,
	MAX_VAL
}

//! 튜토리얼 종류
public enum ETutorialKinds {
	NONE = -1,
	MAX_VAL
}
#endregion			// 기본
#endif			// #if NEVER_USE_THIS
