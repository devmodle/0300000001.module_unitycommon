using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE
/** 서브 열거형 값 */
public static partial class KEnumVal {
	#region 기본

	#endregion			// 기본
}

#region 기본
/** 아이템 타입 */
public enum EItemType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	WEAPON,
	ARMOR,
	ACCESSORY,
	ATTACHMENTS,
	[HideInInspector] MAX_VAL
}

/** 아이템 종류 */
public enum EItemKinds {
	NONE = -1,

	#region 재화
	// 0
	GOODS_COINS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,000
	GOODS_COINS_BOX_COINS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 재화

	#region 소모
	// 100,000,000
	CONSUMABLE_BOOSTER_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,100,000
	CONSUMABLE_GAME_ITEM_HINT = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,100,100
	CONSUMABLE_GAME_ITEM_CONTINUE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 100,100,200
	CONSUMABLE_GAME_ITEM_SHUFFLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 2),
	#endregion			// 소모

	#region 비소모
	// 200,000,000
	NON_CONSUMABLE_REMOVE_ADS = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 비소모

	#region 무기
	// 300,000,000
	WEAPON_ITEM_SAMPLE = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 무기

	#region 방어구
	// 400,000,000
	ARMOR_ITEM_SAMPLE = (EEnumVal.TYPE * 4) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 방어구

	#region 악세서리
	// 50,000,000
	ACCESSORY_ITEM_SAMPLE = (EEnumVal.TYPE * 5) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 악세서리

	#region 장식
	// 60,000,000
	ATTACHMENTS_ITEM_SAMPLE = (EEnumVal.TYPE * 5) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 장식

	[HideInInspector] MAX_VAL
}

/** 미션 타입 */
public enum EMissionType {
	NONE = -1,
	MAIN,
	FREE,
	DAILY,
	EVENT,
	[HideInInspector] MAX_VAL
}

/** 미션 종류 */
public enum EMissionKinds {
	NONE = -1,

	#region 메인
	// 0
	MAIN_MISSION_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 메인

	#region 자유
	// 100,000,000
	FREE_MISSION_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 자유

	#region 일일
	// 200,000,000
	DAILY_MISSION_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 일일

	#region 이벤트
	// 300,000,000
	EVENT_MISSION_SAMPLE = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	// 0
	FREE_COINS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 무료

	#region 일일
	// 100,000,000
	DAILY_REWARD_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 일일

	#region 이벤트
	// 200,000,000
	EVENT_REWARD_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 이벤트

	#region 클리어
	// 300,000,000
	CLEAR_REWARD_SAMPLE = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 클리어

	#region 미션
	// 400,000,000
	MISSION_REWARD_SAMPLE = (EEnumVal.TYPE * 4) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 미션

	#region 튜토리얼
	// 50,000,000
	TUTORIAL_REWARD_SAMPLE = (EEnumVal.TYPE * 5) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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

/** 에피소드 타입 */
public enum EEpisodeType {
	NONE = -1,
	LEVEL,
	STAGE,
	CHAPTER,
	[HideInInspector] MAX_VAL
}

/** 에피소드 종류 */
public enum EEpisodeKinds {
	NONE = -1,

	#region 레벨
	// 0
	LEVEL_NORM = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100
	LEVEL_NORM_BOSS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 200,000
	LEVEL_TUTORIAL = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 200,100
	LEVEL_TUTORIAL_BOSS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion			// 레벨

	#region 스테이지
	// 100,000,000
	STAGE_NORM = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,100,000
	STAGE_TUTORIAL = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 스테이지

	#region 챕터
	// 200,000,000
	CHAPTER_NORM = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 200,100,000
	CHAPTER_TUTORIAL = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 챕터

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
	// 0
	PLAY_TUTORIAL_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 플레이

	#region 도움말
	// 100,000,000
	HELP_TUTORIAL_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 도움말

	[HideInInspector] MAX_VAL
}

/** 효과 타입 */
public enum EFXType {
	NONE = -1,
	HIT,
	BUFF,
	DEBUFF,
	[HideInInspector] MAX_VAL
}

/** 효과 종류 */
public enum EFXKinds {
	NONE = -1,

	#region 타격
	// 0
	HIT_FX_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 타격

	#region 버프
	// 100,000,000
	BUFF_FX_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 버프

	#region 디버프
	// 200,000,000
	DEBUFF_FX_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 디버프

	[HideInInspector] MAX_VAL
}

/** 스킬 타입 */
public enum ESkillType {
	NONE = -1,
	ACTIVE,
	PASSIVE,
	[HideInInspector] MAX_VAL
}

/** 스킬 종류 */
public enum ESkillKinds {
	NONE = -1,

	#region 액티브
	// 0
	ACTIVE_SKILL_SAMPLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 액티브

	#region 패시브
	// 100,000,000
	PASSIVE_SKILL_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 패시브

	[HideInInspector] MAX_VAL
}

/** 어빌리티 타입 */
public enum EAbilityType {
	NONE = -1,
	STAT,
	BUFF,
	DEBUFF,
	UPGRADE,
	[HideInInspector] MAX_VAL
}

/** 어빌리티 종류 */
public enum EAbilityKinds {
	NONE = -1,

	#region 스탯
	// 0
	STAT_LV = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100
	STAT_EXP = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 200
	STAT_HP = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),

	// 300
	STAT_MP = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 3),
	#endregion			// 스탯

	#region 버프
	// 100,000,000
	BUFF_ABILITY_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 버프

	#region 디버프
	// 200,000,000
	DEBUFF_ABILITY_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 디버프

	#region 업그레이드
	// 300,000,000
	UPGRADE_ABILITY_SAMPLE = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 업그레이드

	[HideInInspector] MAX_VAL
}

/** 객체 타입 */
public enum EObjType {
	NONE = -1,
	BG,
	NORM,
	OVERLAY,
	PLAYABLE,
	NON_PLAYABLE,
	ENEMY,
	[HideInInspector] MAX_VAL
}

/** 객체 종류 */
public enum EObjKinds {
	NONE = -1,

	#region 배경
	// 0
	BG_EMPTY = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 배경

	#region 일반
	// 100,000,000
	NORM_OBJ_SAMPLE = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 일반

	#region 중첩
	// 200,000,000
	OVERLAY_OBJ_SAMPLE = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 중첩

	#region 플레이 가능
	// 300,000,000
	PLAYABLE_OBJ_SAMPLE = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 플레이 가능

	#region 플레이 불가능
	// 400,000,000
	NON_PLAYABLE_OBJ_SAMPLE = (EEnumVal.TYPE * 4) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 플레이 불가능

	#region 적
	// 500,000,000
	ENEMY_OBJ_SALE_SAMPLE = (EEnumVal.TYPE * 5) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 적

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
	// 0
	SND_BG_SCENE_TITLE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100
	SND_BG_SCENE_MAIN = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 200
	SND_BG_SCENE_GAME = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),

	// 100,000
	SND_FX_TOUCH_BEGIN = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,100
	SND_FX_TOUCH_END = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 200,000
	SND_FX_POPUP_SHOW = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 200,100
	SND_FX_POPUP_CLOSE = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion			// 사운드

	#region 폰트
	// 100,000,000
	FONT_KOREAN = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,100,000
	FONT_ENGLISH = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 폰트

	#region 스프라이트
	// 200,000,000
	[HideInInspector] SPRITE_DEF = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SPRITE_WHITE,
	SPRITE_SPLASH,
	SPRITE_INDICATOR,
	#endregion			// 스프라이트

	#region 텍스처
	// 300,000,000
	[HideInInspector] TEXTURE_DEF = (EEnumVal.TYPE * 3) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	TEXTURE_WHITE,
	TEXTURE_SPLASH,
	TEXTURE_INDICATOR,
	#endregion			// 텍스처

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
	// 0
	PKGS_SPECIAL_BEGINNER = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100
	PKGS_SPECIAL_EXPERT = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 200
	PKGS_SPECIAL_PRO = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	#endregion			// 패키지

	#region 단일
	// 100,000,000
	SINGLE_COINS_BOX = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,100,000
	SINGLE_REMOVE_ADS = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 단일

	[HideInInspector] MAX_VAL
}

/** 타겟 타입 */
public enum ETargetType {
	NONE = -1,
	ITEM,
	SKILL,
	OBJ,
	[HideInInspector] MAX_VAL
}

/** 타겟 종류 */
public enum ETargetKinds {
	NONE = -1,

	#region 아이템
	// 0
	ITEM = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,000
	ITEM_LV = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 200,000
	ITEM_EXP = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 300,000
	ITEM_NUMS = (EEnumVal.TYPE * 0) + (EEnumVal.KINDS_TYPE * 3) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 아이템

	#region 스킬
	// 100,000,000
	SKILL = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,100,000
	SKILL_LV = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,200,000
	SKILL_EXP = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,300,000
	SKILL_NUMS = (EEnumVal.TYPE * 1) + (EEnumVal.KINDS_TYPE * 3) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 스킬

	#region 객체
	// 200,000,000
	OBJ = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 200,100,000
	OBJ_LV = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 200,200,000
	OBJ_EXP = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 200,300,000
	OBJ_NUMS = (EEnumVal.TYPE * 2) + (EEnumVal.KINDS_TYPE * 3) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion			// 객체

	[HideInInspector] MAX_VAL
}
#endregion			// 기본
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
