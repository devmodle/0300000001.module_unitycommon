using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
namespace SampleEngineName {
	//! 엔진 확장 클래스
	public static partial class Extension {
		#region 클래스 함수
		//! 종류 => 타입으로 변환한다
		public static EBlockType ExKindsToType(this EBlockKinds a_eSender) {
			return (EBlockType)((int)a_eSender).ExKindsToType();
		}

		//! 종류 => 기본 종류로 변환한다
		public static EBlockKinds ExKindsToBaseKinds(this EBlockKinds a_eSender) {
			return (EBlockKinds)((int)a_eSender).ExKindsToBaseKinds();
		}

		//! 종류 => 종류 타입으로 변환한다
		public static EBlockKinds ExKindsToKindsType(this EBlockKinds a_eSender) {
			return (EBlockKinds)((int)a_eSender).ExKindsToKindsType();
		}

		//! 종류 => 서브 종류 타입으로 변환한다
		public static EBlockKinds ExKindsToSubKindsType(this EBlockKinds a_eSender) {
			return (EBlockKinds)((int)a_eSender).ExKindsToSubKindsType();
		}
		#endregion			// 클래스 함수

		#region 추가 클래스 변수

		#endregion			// 추가 클래스 변수

		#region 추가 클래스 프로퍼티

		#endregion			// 추가 클래스 프로퍼티

		#region 추가 클래스 함수

		#endregion			// 추가 클래스 함수
	}
}
#endif			// #if NEVER_USE_THIS
