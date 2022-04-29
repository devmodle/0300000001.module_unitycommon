using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#region 기본
/** 플레이 모드 */
public enum EPlayMode {
	NONE = -1,
	NORM,
	TUTORIAL,
	TEST,
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
	ADS_REWARD = EPriceKinds.ADS_PRICE + (EEnumVal.KINDS_TYPE * 0),			//!< 보상 광고 0
	#endregion			// 광고

	#region 재화
	[HideInInspector] GOODS_PRICE = EEnumVal.TYPE * 1,
	GOODS_COINS = EPriceKinds.GOODS_PRICE + (EEnumVal.KINDS_TYPE * 0),			//!< 코인 10,000,000
	#endregion			// 재화

	#region 결제
	[HideInInspector] PURCHASE_PRICE = EEnumVal.TYPE * 2,
	IN_APP_PURCHASE = EPriceKinds.PURCHASE_PRICE + (EEnumVal.KINDS_TYPE * 0),			//!< 인앱 결제 20,000,000
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
	GOODS_COINS = EItemKinds.GOODS_ITEM + (EEnumVal.KINDS_TYPE * 0),			//!< 코인 0
	#endregion			// 재화

	#region 소모
	[HideInInspector] CONSUMABLE_ITEM = EEnumVal.TYPE * 1,

	[HideInInspector] CONSUMABLE_BOOSTER = EItemKinds.CONSUMABLE_ITEM + (EEnumVal.KINDS_TYPE * 0),
	CONSUMABLE_BOOSTER_SAMPLE = EItemKinds.CONSUMABLE_BOOSTER + (EEnumVal.SUB_KINDS_TYPE * 0),			//!< 샘플 10,000,000

	[HideInInspector] CONSUMABLE_GAME_ITEM = EItemKinds.CONSUMABLE_ITEM + (EEnumVal.KINDS_TYPE * 1),
	CONSUMABLE_GAME_ITEM_HINT = EItemKinds.CONSUMABLE_GAME_ITEM + (EEnumVal.SUB_KINDS_TYPE * 0),			//!< 힌트 10,010,000
	CONSUMABLE_GAME_ITEM_CONTINUE = EItemKinds.CONSUMABLE_GAME_ITEM + (EEnumVal.SUB_KINDS_TYPE * 1),			//!< 이어하기 10,010,100
	#endregion			// 소모

	#region 비소모
	[HideInInspector] NON_CONSUMABLEITEM = EEnumVal.TYPE * 2,
	NON_CONSUMABLE_REMOVE_ADS = EItemKinds.NON_CONSUMABLEITEM + (EEnumVal.KINDS_TYPE * 0),			//!< 광고 제거 20,000,000
	#endregion			// 비소모

	[HideInInspector] MAX_VAL
}

/** 아이템 판매 타입 */
public enum EItemSaleType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	[HideInInspector] MAX_VAL
}

/** 아이템 판매 종류 */
public enum EItemSaleKinds {
	NONE = -1,

	#region 재화
	[HideInInspector] GOODS_ITEM_SALE = EEnumVal.TYPE * 0,
	GOODS_ITEM_SALE_SAMPLE = EItemSaleKinds.GOODS_ITEM_SALE + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 0
	#endregion			// 재화

	#region 소모
	[HideInInspector] CONSUMABLE_ITEM_SALE = EEnumVal.TYPE * 1,

	[HideInInspector] CONSUMABLE_BOOSTER = EItemSaleKinds.CONSUMABLE_ITEM_SALE + (EEnumVal.KINDS_TYPE * 0),
	CONSUMABLE_BOOSTER_SAMPLE = EItemSaleKinds.CONSUMABLE_BOOSTER + (EEnumVal.SUB_KINDS_TYPE * 0),			//!< 샘플 10,000,000

	[HideInInspector] CONSUMABLE_GAME_ITEM = EItemSaleKinds.CONSUMABLE_ITEM_SALE + (EEnumVal.KINDS_TYPE * 1),
	CONSUMABLE_GAME_ITEM_HINT = EItemSaleKinds.CONSUMABLE_GAME_ITEM + (EEnumVal.SUB_KINDS_TYPE * 0),			//!< 힌트 10,010,000
	CONSUMABLE_GAME_ITEM_CONTINUE = EItemSaleKinds.CONSUMABLE_GAME_ITEM + (EEnumVal.SUB_KINDS_TYPE * 1),			//!< 이어하기 10,010,100
	#endregion			// 소모

	#region 비소모
	[HideInInspector] NON_CONSUMABLE_ITEM_SALE = EEnumVal.TYPE * 2,
	NON_CONSUMABLE_ITEM_SALE_SAMPLE = EItemSaleKinds.NON_CONSUMABLE_ITEM_SALE + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 20,000,000
	#endregion			// 비소모

	[HideInInspector] MAX_VAL
}

/** 상품 판매 타입 */
public enum EProductSaleType {
	NONE = -1,
	PKGS,
	SINGLE,
	[HideInInspector] MAX_VAL
}

/** 상품 판매 종류 */
public enum EProductSaleKinds {
	NONE = -1,

	#region 패키지
	[HideInInspector] PKGS_PRODUCT_SALE = EEnumVal.TYPE * 0,

	[HideInInspector] PKGS_SPECIAL = EProductSaleKinds.PKGS_PRODUCT_SALE + (EEnumVal.KINDS_TYPE * 0),
	PKGS_SPECIAL_BEGINNER = EProductSaleKinds.PKGS_SPECIAL + (EEnumVal.SUB_KINDS_TYPE * 0),			//!< 초보자 패키지 0
	PKGS_SPECIAL_EXPERT = EProductSaleKinds.PKGS_SPECIAL + (EEnumVal.SUB_KINDS_TYPE * 1),			//!< 숙련자 패키지 100
	PKGS_SPECIAL_PRO = EProductSaleKinds.PKGS_SPECIAL + (EEnumVal.SUB_KINDS_TYPE * 2),			//!< 전문가 패키지 200
	#endregion			// 패키지

	#region 단일
	[HideInInspector] SINGLE_PRODUCT_SALE = EEnumVal.TYPE * 1,
	SINGLE_COINS_BOX = EProductSaleKinds.SINGLE_PRODUCT_SALE + (EEnumVal.KINDS_TYPE * 0),			//!< 코인 상자 10,000,000
	SINGLE_REMOVE_ADS = EProductSaleKinds.SINGLE_PRODUCT_SALE + (EEnumVal.KINDS_TYPE * 1),			//!< 광고 제거 10,010,000
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
	FREE_MISSION_SAMPLE = EMissionKinds.FREE_MISSION + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 0
	#endregion			// 자유

	#region 일일
	[HideInInspector] DAILY_MISSION = EEnumVal.TYPE * 1,
	DAILY_MISSION_SAMPLE = EMissionKinds.DAILY_MISSION + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 10,000,000
	#endregion			// 일일

	#region 이벤트
	[HideInInspector] EVENT_MISSION = EEnumVal.TYPE * 2,
	EVENT_MISSION_SAMPLE = EMissionKinds.EVENT_MISSION + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 20,000,000
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
	FREE_COINS = ERewardKinds.FREE_REWARD + (EEnumVal.KINDS_TYPE * 0),			//!< 코인 0
	#endregion			// 무료

	#region 일일
	[HideInInspector] DAILY_REWARD = EEnumVal.TYPE * 1,
	DAILY_REWARD_SAMPLE = ERewardKinds.DAILY_REWARD + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 10,000,000
	#endregion			// 일일

	#region 이벤트
	[HideInInspector] EVENT_REWARD = EEnumVal.TYPE * 2,
	EVENT_REWARD_SAMPLE = ERewardKinds.EVENT_REWARD + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 20,000,000
	#endregion			// 이벤트

	#region 클리어
	[HideInInspector] CLEAR_REWARD = EEnumVal.TYPE * 3,
	CLEAR_REWARD_SAMPLE = ERewardKinds.CLEAR_REWARD + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 30,000,000
	#endregion			// 클리어

	#region 미션
	[HideInInspector] MISSION_REWARD = EEnumVal.TYPE * 4,
	MISSION_REWARD_SAMPLE = ERewardKinds.MISSION_REWARD + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 40,000,000
	#endregion			// 미션

	#region 튜토리얼
	[HideInInspector] TUTORIAL_REWARD = EEnumVal.TYPE * 5,
	TUTORIAL_REWARD_SAMPLE = ERewardKinds.TUTORIAL_REWARD + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 50,000,000
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
	NORM_LEVEL_SAMPLE = ELevelKinds.NORM_LEVEL + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 0
	#endregion			// 일반

	#region 튜토리얼
	[HideInInspector] TUTORIAL_LEVEL = EEnumVal.TYPE * 1,
	TUTORIAL_LEVEL_SAMPLE = ELevelKinds.TUTORIAL_LEVEL + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 10,000,000
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
	NORM_STAGE_SAMPLE = EStageKinds.NORM_STAGE + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 0
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
	NORM_CHAPTER_SAMPLE = EChapterKinds.NORM_CHAPTER + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 0
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
	PLAY_TUTORIAL_SAMPLE = ETutorialKinds.PLAY_TUTORIAL + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 0
	#endregion			// 플레이

	#region 도움말
	[HideInInspector] HELP_TUTORIAL = EEnumVal.TYPE * 1,
	HELP_TUTORIAL_SAMPLE = ETutorialKinds.HELP_TUTORIAL + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 10,000,000
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
	BLOCK_TARGET_SAMPLE = ETargetKinds.BLOCK_TARGET + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 0
	#endregion			// 블럭

	#region 기록
	[HideInInspector] RECORD_TARGET = EEnumVal.TYPE * 1,
	RECORD_MARK = ETargetKinds.RECORD_TARGET + (EEnumVal.KINDS_TYPE * 0),			//!< 마크 10,000,000
	#endregion			// 기록

	[HideInInspector] MAX_VAL
}

/** 효과 타입 */
public enum EFXType {
	NONE = -1,
	[HideInInspector] MAX_VAL
}

/** 효과 종류 */
public enum EFXKinds {
	NONE = -1,
	[HideInInspector] MAX_VAL
}

/** 스킬 타입 */
public enum ESkillType {
	NONE = -1,
	[HideInInspector] MAX_VAL
}

/** 스킬 종류 */
public enum ESkillKinds {
	NONE = -1,
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
	BG_EMPTY = EBlockKinds.BG_BLOCK + (EEnumVal.KINDS_TYPE * 0),			//!< 빈 블럭 0
	#endregion			// 배경

	#region 일반
	[HideInInspector] NORM_BLOCK = EEnumVal.TYPE * 1,
	NORM_BLOCK_SAMPLE = EBlockKinds.NORM_BLOCK + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 10,000,000
	#endregion			// 일반

	#region 중첩
	[HideInInspector] OVERLAY_BLOCK = EEnumVal.TYPE * 2,
	OVERLAY_BLOCK_SAMPLE = EBlockKinds.OVERLAY_BLOCK + (EEnumVal.KINDS_TYPE * 0),			//!< 샘플 20,000,000
	#endregion			// 중첩

	[HideInInspector] MAX_VAL
}

/** 리소스 타입 */
public enum EResType {
	NONE = -1,
	SND,
	FONT,
	SPRITE,
	TEXTURE,
	[HideInInspector] MAX_VAL
}

/** 리소스 종류 */
public enum EResKinds {
	NONE = -1,

	#region 사운드
	[HideInInspector] SND_RES = EEnumVal.TYPE * 0,

	[HideInInspector] SND_BG_SCENE = EResKinds.SND_RES + (EEnumVal.KINDS_TYPE * 0),
	SND_BG_SCENE_TITLE = EResKinds.SND_BG_SCENE + (EEnumVal.SUB_KINDS_TYPE * 0),			//!< 타이틀 씬 배경음 0
	SND_BG_SCENE_MAIN = EResKinds.SND_BG_SCENE + (EEnumVal.SUB_KINDS_TYPE * 1),			//!< 메인 씬 배경음 100
	SND_BG_SCENE_GAME = EResKinds.SND_BG_SCENE + (EEnumVal.SUB_KINDS_TYPE * 2),			//!< 게임 씬 배경음 200

	[HideInInspector] SND_FX_TOUCH = EResKinds.SND_RES + (EEnumVal.KINDS_TYPE * 1),
	SND_FX_TOUCH_BEGIN = EResKinds.SND_FX_TOUCH + (EEnumVal.SUB_KINDS_TYPE * 0),			//!< 터치 시작 효과음 10,000 
	SND_FX_TOUCH_END = EResKinds.SND_FX_TOUCH + (EEnumVal.SUB_KINDS_TYPE * 1),			//!< 터치 종료 효과음 10,100

	[HideInInspector] SND_FX_POPUP = EResKinds.SND_RES + (EEnumVal.KINDS_TYPE * 2),
	SND_FX_POPUP_SHOW = EResKinds.SND_FX_POPUP + (EEnumVal.SUB_KINDS_TYPE * 0),			//!< 터치 시작 효과음 20,000 
	SND_FX_POPUP_CLOSE = EResKinds.SND_FX_POPUP + (EEnumVal.SUB_KINDS_TYPE * 1),			//!< 터치 종료 효과음 20,100
	#endregion			// 사운드

	#region 폰트
	[HideInInspector] FONT_RES = EEnumVal.TYPE * 1,
	FONT_KOREAN = EResKinds.FONT_RES + (EEnumVal.KINDS_TYPE * 0),			//!< 한국어 10,000,000
	FONT_ENGLISH = EResKinds.FONT_RES + (EEnumVal.KINDS_TYPE * 1),			//!< 영어 10,010,000
	#endregion			// 폰트

	#region 스프라이트
	[HideInInspector] SPRITE_RES = EEnumVal.TYPE * 2,

	[HideInInspector] SPRITE_DEF = EResKinds.SPRITE_RES + (EEnumVal.KINDS_TYPE * 0),			//!< 기본 20,000,000
	SPRITE_WHITE,
	SPRITE_SPLASH,
	SPRITE_INDICATOR,
	#endregion			// 스프라이트

	#region 텍스처
	[HideInInspector] TEXTURE_RES = EEnumVal.TYPE * 3,

	[HideInInspector] TEXTURE_DEF = EResKinds.TEXTURE_RES + (EEnumVal.KINDS_TYPE * 0),			//!< 기본 30,000,000
	TEXTURE_WHITE,
	TEXTURE_SPLASH,
	TEXTURE_INDICATOR,
	#endregion			// 텍스처

	[HideInInspector] MAX_VAL
}
#endregion			// 기본
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
