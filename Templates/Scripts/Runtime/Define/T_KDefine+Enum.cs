using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
	PURCHASE,
	ITEM,
	SKILL,
	OBJ,
	[HideInInspector] MAX_VAL
}

/** 소유자 타입 */
public enum EOwnerType {
	NONE = -1,
	ITEM,
	SKILL,
	OBJ,
	[HideInInspector] MAX_VAL
}

/** 획득 타입 */
public enum EAcquireType {
	NONE = -1,
	ITEM,
	SKILL,
	OBJ,
	[HideInInspector] MAX_VAL
}
#endregion			// 기본
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
