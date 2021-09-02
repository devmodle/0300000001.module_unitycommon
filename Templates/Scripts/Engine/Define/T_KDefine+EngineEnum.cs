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
		OVERLAY,
		MAX_VAL
	}

	//! 블럭 종류
	public enum EBlockKinds {
		NONE = -1,

		#region 배경
		// 빈 블럭 0
		BG_EMPTY,
		#endregion			// 배경

		#region 일반
		// 샘플 10,000,000
		NORM_SAMPLE = EBlockKinds.BG_EMPTY + KCDefine.B_UNIT_KINDS_PER_TYPE,
		#endregion			// 일반

		#region 중첩
		// 샘플 20,000,000
		OVERLAY_SAMPLE = EBlockKinds.NORM_SAMPLE + KCDefine.B_UNIT_KINDS_PER_TYPE,
		#endregion			// 중첩

		MAX_VAL
	}
	#endregion			// 기본

	#region 추가 상수

	#endregion			// 추가 상수
}
#endif			// #if NEVER_USE_THIS
