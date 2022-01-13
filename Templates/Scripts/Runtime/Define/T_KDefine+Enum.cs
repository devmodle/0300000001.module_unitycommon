using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if RUNTIME_TEMPLATES_MODULE_ENABLE
#region 기본
/** 플레이 모드 */
public enum EPlayMode {
	NONE = -1,
	NORM,
	TUTORIAL,
	TEST,
	[HideInInspector] MAX_VAL
}

/** 난이도 */
public enum EDifficulty {
	NONE = -1,
	EASY,
	NORM,
	HARD,
	VERY_HARD,
	SUPER_HARD,
	[HideInInspector] MAX_VAL
}

/** 가격 타입 */
public enum EPriceType {
	NONE = -1,
	ADS,
	GOODS,
	PURCHASE,
	[HideInInspector] MAX_VAL
}

/** 가격 종류 */
public enum EPriceKinds {
	NONE = -1,

	#region 광고
	[HideInInspector] ADS_PRICE = EEnumVal.TYPE * 0,

	// 보상 광고 0
	ADS_REWARD = EPriceKinds.ADS_PRICE + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 광고

	#region 재화
	[HideInInspector] GOODS_PRICE = EEnumVal.TYPE * 1,

	// 코인 10,000,000
	GOODS_COINS = EPriceKinds.GOODS_PRICE + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 재화

	#region 결제
	[HideInInspector] PURCHASE_PRICE = EEnumVal.TYPE * 2,

	// 인앱 결제 20,000,000
	IN_APP_PURCHASE = EPriceKinds.PURCHASE_PRICE + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 결제

	[HideInInspector] MAX_VAL
}

/** 아이템 타입 */
public enum EItemType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	[HideInInspector] MAX_VAL
}

/** 아이템 종류 */
public enum EItemKinds {
	NONE = -1,

	#region 재화
	[HideInInspector] GOODS_ITEM = EEnumVal.TYPE * 0,

	// 코인 0
	GOODS_COINS = EItemKinds.GOODS_ITEM + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 재화

	#region 소모
	[HideInInspector] CONSUMABLE_ITEM = EEnumVal.TYPE * 1,

	// 부스터 10,000,000
	[HideInInspector] CONSUMABLE_BOOSTER = EItemKinds.CONSUMABLE_ITEM + (EEnumVal.KINDS_TYPE * 0),

	// 샘플 10,000,100
	CONSUMABLE_BOOSTER_SAMPLE = EItemKinds.CONSUMABLE_BOOSTER + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 게임 아이템 10,010,000
	[HideInInspector] CONSUMABLE_GAME_ITEM = EItemKinds.CONSUMABLE_ITEM + (EEnumVal.KINDS_TYPE * 1),

	// 샘플 10,010,100
	CONSUMABLE_GAME_ITEM_SAMPLE = EItemKinds.CONSUMABLE_GAME_ITEM + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion			// 소모

	#region 비소모
	[HideInInspector] NON_CONSUMABLEITEM = EEnumVal.TYPE * 2,

	// 광고 제거 20,000,000
	NON_CONSUMABLE_REMOVE_ADS = EItemKinds.NON_CONSUMABLEITEM + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 비소모

	[HideInInspector] MAX_VAL
}

/** 판매 아이템 타입 */
public enum ESaleItemType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	[HideInInspector] MAX_VAL
}

/** 판매 아이템 종류 */
public enum ESaleItemKinds {
	NONE = -1,

	#region 재화
	[HideInInspector] GOODS_SALE_ITEM = EEnumVal.TYPE * 0,

	// 샘플 0
	GOODS_SALE_ITEM_SAMPLE = ESaleItemKinds.GOODS_SALE_ITEM + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 재화

	#region 소모
	[HideInInspector] CONSUMABLE_SALE_ITEM = EEnumVal.TYPE * 1,

	// 힌트 10,000,000
	CONSUMABLE_HINT = ESaleItemKinds.GOODS_SALE_ITEM + (EEnumVal.KINDS_TYPE * 0),

	// 이어하기 10,010,000
	CONSUMABLE_CONTINUE = ESaleItemKinds.GOODS_SALE_ITEM + (EEnumVal.KINDS_TYPE * 1),
	#endregion			// 소모

	#region 비소모
	[HideInInspector] NON_CONSUMABLE_SALE_ITEM = EEnumVal.TYPE * 2,

	// 샘플 20,000,000
	NON_CONSUMABLE_SALE_ITEM_SAMPLE = ESaleItemKinds.NON_CONSUMABLE_SALE_ITEM + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 비소모

	[HideInInspector] MAX_VAL
}

/** 판매 상품 타입 */
public enum ESaleProductType {
	NONE = -1,
	PKGS,
	SINGLE,
	[HideInInspector] MAX_VAL
}

/** 판매 상품 종류 */
public enum ESaleProductKinds {
	NONE = -1,

	#region 패키지
	[HideInInspector] PKGS_SALE_PRODUCT = EEnumVal.TYPE * 0,

	// 초보자 0
	PKGS_START = ESaleProductKinds.PKGS_SALE_PRODUCT + (EEnumVal.KINDS_TYPE * 0),

	// 숙련자 10,000
	PKGS_EXPERT = ESaleProductKinds.PKGS_SALE_PRODUCT + (EEnumVal.KINDS_TYPE * 1),

	// 전문가 20,000
	PKGS_PRO = ESaleProductKinds.PKGS_SALE_PRODUCT + (EEnumVal.KINDS_TYPE * 2),
	#endregion			// 패키지

	#region 단일
	[HideInInspector] SINGLE_SALE_PRODUCT = EEnumVal.TYPE * 1,

	// 판매 코인 10,000,000
	SINGLE_SALE_COINS = ESaleProductKinds.SINGLE_SALE_PRODUCT + (EEnumVal.KINDS_TYPE * 0),

	// 광고 제거 10,010,000
	SINGLE_REMOVE_ADS = ESaleProductKinds.SINGLE_SALE_PRODUCT + (EEnumVal.KINDS_TYPE * 1),
	#endregion			// 단일

	[HideInInspector] MAX_VAL
}

/** 미션 타입 */
public enum EMissionType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	[HideInInspector] MAX_VAL
}

/** 미션 종류 */
public enum EMissionKinds {
	NONE = -1,

	#region 자유
	[HideInInspector] FREE_MISSION = EEnumVal.TYPE * 0,

	// 샘플 0
	FREE_MISSION_SAMPLE = EMissionKinds.FREE_MISSION + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 자유

	#region 일일
	[HideInInspector] DAILY_MISSION = EEnumVal.TYPE * 1,

	// 샘플 10,000,000
	DAILY_MISSION_SAMPLE = EMissionKinds.DAILY_MISSION + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 일일

	#region 이벤트
	[HideInInspector] EVENT_MISSION = EEnumVal.TYPE * 2,

	// 샘플 20,000,000
	EVENT_MISSION_SAMPLE = EMissionKinds.EVENT_MISSION + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 이벤트

	[HideInInspector] MAX_VAL
}

/** 보상 타입 */
public enum ERewardType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	CLEAR,
	MISSION,
	TUTORIAL,
	[HideInInspector] MAX_VAL
}

/** 보상 종류 */
public enum ERewardKinds {
	NONE = -1,

	#region 무료
	[HideInInspector] FREE_REWARD = EEnumVal.TYPE * 0,

	// 코인 0
	FREE_COINS = ERewardKinds.FREE_REWARD + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 무료

	#region 일일
	[HideInInspector] DAILY_REWARD = EEnumVal.TYPE * 1,

	// 샘플 10,000,000
	DAILY_REWARD_SAMPLE = ERewardKinds.DAILY_REWARD + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 일일

	#region 이벤트
	[HideInInspector] EVENT_REWARD = EEnumVal.TYPE * 2,

	// 샘플 20,000,000
	EVENT_REWARD_SAMPLE = ERewardKinds.EVENT_REWARD + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 이벤트

	#region 클리어
	[HideInInspector] CLEAR_REWARD = EEnumVal.TYPE * 3,

	// 샘플 30,000,000
	CLEAR_REWARD_SAMPLE = ERewardKinds.CLEAR_REWARD + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 클리어

	#region 미션
	[HideInInspector] MISSION_REWARD = EEnumVal.TYPE * 4,

	// 샘플 40,000,000
	MISSION_REWARD_SAMPLE = ERewardKinds.MISSION_REWARD + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 미션

	#region 튜토리얼
	[HideInInspector] TUTORIAL_REWARD = EEnumVal.TYPE * 5,

	// 샘플 50,000,000
	TUTORIAL_REWARD_SAMPLE = ERewardKinds.TUTORIAL_REWARD + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 튜토리얼

	[HideInInspector] MAX_VAL
}

/** 보상 수준 */
public enum ERewardQuality {
	NONE = -1,
	NORM,
	HIGH,
	ULTRA,
	[HideInInspector] MAX_VAL
}

/** 보상 획득 팝업 타입 */
public enum ERewardAcquirePopupType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	CLEAR,
	[HideInInspector] MAX_VAL
}

/** 레벨 타입 */
public enum ELevelType {
	NONE = -1,
	NORM,
	TUTORIAL,
	[HideInInspector] MAX_VAL
}

/** 레벨 종류 */
public enum ELevelKinds {
	NONE = -1,

	#region 일반
	[HideInInspector] NORM_LEVEL = EEnumVal.TYPE * 0,

	// 샘플 0
	NORM_LEVEL_SAMPLE = ELevelKinds.NORM_LEVEL + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 일반

	#region 튜토리얼
	[HideInInspector] TUTORIAL_LEVEL = EEnumVal.TYPE * 1,

	// 샘플 10,000,000
	TUTORIAL_LEVEL_SAMPLE = ELevelKinds.TUTORIAL_LEVEL + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 튜토리얼

	[HideInInspector] MAX_VAL
}

/** 스테이지 타입 */
public enum EStageType {
	NONE = -1,
	NORM,
	[HideInInspector] MAX_VAL
}

/** 스테이지 타입 */
public enum EStageKinds {
	NONE = -1,

	#region 일반
	[HideInInspector] NORM_STAGE = EEnumVal.TYPE * 0,

	// 샘플 0
	NORM_STAGE_SAMPLE = EStageKinds.NORM_STAGE + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 일반

	[HideInInspector] MAX_VAL
}

/** 챕터 타입 */
public enum EChapterType {
	NONE = -1,
	NORM,
	[HideInInspector] MAX_VAL
}

/** 챕터 타입 */
public enum EChapterKinds {
	NONE = -1,

	#region 일반
	[HideInInspector] NORM_CHAPTER = EEnumVal.TYPE * 0,

	// 샘플 0
	NORM_CHAPTER_SAMPLE = EChapterKinds.NORM_CHAPTER + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 일반

	[HideInInspector] MAX_VAL
}

/** 튜토리얼 타입 */
public enum ETutorialType {
	NONE = -1,
	PLAY,
	HELP,
	[HideInInspector] MAX_VAL
}

/** 튜토리얼 종류 */
public enum ETutorialKinds {
	NONE = -1,

	#region 플레이
	[HideInInspector] PLAY_TUTORIAL = EEnumVal.TYPE * 0,

	// 샘플 0
	PLAY_TUTORIAL_SAMPLE = ETutorialKinds.PLAY_TUTORIAL + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 플레이

	#region 도움말
	[HideInInspector] HELP_TUTORIAL = EEnumVal.TYPE * 1,

	// 샘플 10,000,000
	HELP_TUTORIAL_SAMPLE = ETutorialKinds.HELP_TUTORIAL + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 도움말

	[HideInInspector] MAX_VAL
}

/** 타겟 타입 */
public enum ETargetType {
	NONE = -1,
	BLOCK,
	RECORD,
	[HideInInspector] MAX_VAL
}

/** 타겟 종류 */
public enum ETargetKinds {
	NONE = -1,

	#region 블럭
	[HideInInspector] BLOCK_TARGET = EEnumVal.TYPE * 0,

	// 샘플 0
	BLOCK_TARGET_SAMPLE = ETargetKinds.BLOCK_TARGET + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 블럭

	#region 기록
	[HideInInspector] RECORD_TARGET = EEnumVal.TYPE * 1,

	// 클리어 마크 10,000,000
	RECORD_CLEAR_MARK = ETargetKinds.RECORD_TARGET + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 기록

	[HideInInspector] MAX_VAL
}

/** 블럭 타입 */
public enum EBlockType {
	NONE = -1,
	BG,
	NORM,
	OVERLAY,
	[HideInInspector] MAX_VAL
}

/** 블럭 종류 */
public enum EBlockKinds {
	NONE = -1,

	#region 배경
	[HideInInspector] BG_BLOCK = EEnumVal.TYPE * 0,

	// 빈 블럭 0
	BG_EMPTY = EBlockKinds.BG_BLOCK + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 배경

	#region 일반
	[HideInInspector] NORM_BLOCK = EEnumVal.TYPE * 1,

	// 샘플 10,000,000
	NORM_BLOCK_SAMPLE = EBlockKinds.NORM_BLOCK + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 일반

	#region 중첩
	[HideInInspector] OVERLAY_BLOCK = EEnumVal.TYPE * 2,

	// 샘플 20,000,000
	OVERLAY_BLOCK_SAMPLE = EBlockKinds.OVERLAY_BLOCK + (EEnumVal.KINDS_TYPE * 0),
	#endregion			// 중첩

	[HideInInspector] MAX_VAL
}
#endregion			// 기본

#region 추가 상수

#endregion			// 추가 상수
#endif			// #if RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if NEVER_USE_THIS
