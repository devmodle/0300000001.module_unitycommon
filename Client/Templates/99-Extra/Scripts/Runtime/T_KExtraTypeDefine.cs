#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE
using MessagePack;

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
using Newtonsoft.Json;
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE

#region 자료형
/** 추가 타입 랩퍼 */
[MessagePackObject]
public struct STExtraTypeWrapper {
	// Do Something
}
#endregion // 자료형

namespace NSEngine {
	#region 자료형
	/** 엔진 추가 타입 랩퍼 */
	[MessagePackObject]
	public struct STEngineExtraTypeWrapper {
		// Do Something
	}
	#endregion // 자료형
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY
