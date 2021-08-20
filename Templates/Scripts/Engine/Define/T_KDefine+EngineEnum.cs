using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
namespace SampleEngineName {
	#region 기본
	//! 블럭 타입
	public enum EBlockType {
		NONE = -1,
		BG,
		NORM,
		MAX_VAL
	}

	//! 블럭 종류
	public enum EBlockKinds {
		NONE = -1,

		#region 배경
		// 배경 블럭 0
		[HideInInspector] BG_BLOCK,

		// 빈 블럭 0
		BG_EMPTY = EBlockKinds.BG_BLOCK,
		#endregion			// 배경

		#region 일반
		// 일반 블럭 10,000,000
		[HideInInspector] NORM_BLOCK = EBlockKinds.BG_BLOCK + KCDefine.B_UNIT_KINDS_PER_TYPE,
		#endregion			// 일반

		MAX_VAL
	}
	#endregion			// 기본

	#region 추가 상수

	#endregion			// 추가 상수
}
#endif			// #if NEVER_USE_THIS
