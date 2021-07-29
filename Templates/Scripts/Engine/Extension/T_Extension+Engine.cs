using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
namespace SampleEngineName {
	//! 엔진 확장 클래스
	public static partial class Extension {
		#region 클래스 함수
		//! 블럭 종류 => 기본 블럭 종류로 변환한다
		public static EBlockKinds ExKindsToBaseKinds(this EBlockKinds a_eSender) {
			return (EBlockKinds)(((int)a_eSender).ExKindsToBaseKinds());
		}

		//! 블럭 종류 => 블럭 종류 타입으로 변환한다
		public static EBlockKinds ExKindsToKindsType(this EBlockKinds a_eSender) {
			return (EBlockKinds)(((int)a_eSender).ExKindsToKindsType());
		}
		#endregion			// 클래스 함수
	}
}
#endif			// #if NEVER_USE_THIS
