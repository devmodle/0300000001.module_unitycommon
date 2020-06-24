using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using Leguar.TotalJSON;

#if MESSAGE_PACK_ENABLE
using MessagePack;
#endif			// #if MESSAGE_PACK_ENABLE

#if YAML_SERIALIZER_ENABLE
using YamlDotNet.Serialization;
#endif			// #if YAML_SERIALIZER_ENABLE

//! 기본 확장 클래스
public static partial class CExtension {
	#region 클래스 함수
	//! 유효 여부를 검사한다
	public static bool ExIsValid(this string a_oSender) {
		return a_oSender != null && a_oSender.Length >= 1;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this TextAsset a_oSender) {
		return a_oSender != null && (a_oSender.text.ExIsValid() || a_oSender.bytes.ExIsValid());
	}

	//! 동일 여부를 검사한다
	public static bool ExIsEquals(this float a_fSender, float a_fRhs) {
		return Mathf.Approximately(a_fSender, a_fRhs);
	}

	//! 동일 여부를 검사한다
	public static bool ExIsEquals(this double a_dblSender, double a_dblRhs) {
		double dblDeltaTime = System.Math.Abs(a_dblSender) - System.Math.Abs(a_dblRhs);
		return dblDeltaTime >= -double.Epsilon && dblDeltaTime <= double.Epsilon;
	}

	//! 동일 여부를 검사한다
	public static bool ExIsEquals(this Vector2 a_stSender, Vector2 a_stRhs) {
		return a_stSender.x.ExIsEquals(a_stRhs.x) && a_stSender.y.ExIsEquals(a_stRhs.y);
	}

	//! 동일 여부를 검사한다
	public static bool ExIsEquals(this Vector3 a_stSender, Vector3 a_stRhs) {
		return a_stSender.x.ExIsEquals(a_stRhs.x) && 
			a_stSender.y.ExIsEquals(a_stRhs.y) && a_stSender.z.ExIsEquals(a_stRhs.z);
	}

	//! 동일 여부를 검사한다
	public static bool ExIsEquals(this Vector4 a_stSender, Vector4 a_stRhs) {
		return a_stSender.x.ExIsEquals(a_stRhs.x) && a_stSender.y.ExIsEquals(a_stRhs.y) &&
			a_stSender.z.ExIsEquals(a_stRhs.z) && a_stSender.w.ExIsEquals(a_stRhs.w);
	}

	//! 동일 여부를 검사한다
	public static bool ExIsEquals(this string a_oSender, string a_oRhs) {
		return a_oSender != null && a_oRhs != null && a_oSender.Equals(a_oRhs);
	}

	//! 작음 여부를 검사한다
	public static bool ExIsLess(this float a_fSender, float a_fRhs) {
		return a_fSender < a_fRhs - float.Epsilon;
	}

	//! 작거나 같음 여부를 검사한다
	public static bool ExIsLessEquals(this float a_fSender, float a_fRhs) {
		return a_fSender.ExIsLess(a_fRhs) || a_fSender.ExIsEquals(a_fRhs);
	}

	//! 큰 여부를 검사한다
	public static bool ExIsGreate(this float a_fSender, float a_fRhs) {
		return a_fSender > a_fRhs + float.Epsilon;
	}

	//! 크거나 같음 여부를 검사한다
	public static bool ExIsGreateEquals(this float a_fSender, float a_fRhs) {
		return a_fSender.ExIsGreate(a_fRhs) || a_fSender.ExIsEquals(a_fRhs);
	}

	//! 작음 여부를 검사한다
	public static bool ExIsLess(this double a_dblSender, double a_dblRhs) {
		return a_dblSender < a_dblRhs - double.Epsilon;
	}

	//! 작거나 같음 여부를 검사한다
	public static bool ExIsLessEquals(this double a_dblSender, double a_dblRhs) {
		return a_dblSender.ExIsLess(a_dblRhs) || a_dblSender.ExIsEquals(a_dblRhs);
	}

	//! 큰 여부를 검사한다
	public static bool ExIsGreate(this double a_dblSender, double a_dblRhs) {
		return a_dblSender > a_dblRhs + double.Epsilon;
	}

	//! 크거나 같음 여부를 검사한다
	public static bool ExIsGreateEquals(this double a_dblSender, double a_dblRhs) {
		return a_dblSender.ExIsGreate(a_dblRhs) || a_dblSender.ExIsEquals(a_dblRhs);
	}

	//! 비동기 작업 완료 여부를 검사한다
	public static bool ExIsComplete(this Task a_oSender) {
		return a_oSender != null && (a_oSender.IsCompleted && !a_oSender.IsFaulted && !a_oSender.IsCanceled);
	}

	//! 유럽 연합 여부를 검사한다
	public static bool ExIsEuropeanUnion(this string a_oSender) {
		Func.Assert(a_oSender.ExIsValid());
		string oCountryCode = a_oSender.ToUpper();

		int nIndex = KDefine.B_EUROPEAN_UNION_COUNTRY_CODES.ExFindValue((a_oString) => {
			return a_oString.ExIsEquals(oCountryCode);
		});

		return nIndex > KDefine.B_INDEX_INVALID;	
	}

	//! 색상을 반환한다
	public static Color ExGetAlphaColor(this Color a_stSender, float a_fAlpha) {
		a_stSender.a = a_fAlpha;
		return a_stSender;
	}

	//! X 축 간격을 반환한다
	public static float ExGetDeltaX(this Vector3 a_stSender, Vector3 a_stRhs) {
		return (a_stSender - a_stRhs).x;
	}

	//! Y 축 간격을 반환한다
	public static float ExGetDeltaY(this Vector3 a_stSender, Vector3 a_stRhs) {
		return (a_stSender - a_stRhs).y;
	}

	//! Z 축 간격을 반환한다
	public static float ExGetDeltaZ(this Vector3 a_stSender, Vector3 a_stRhs) {
		return (a_stSender - a_stRhs).z;
	}

	//! 시간 간격을 반환한다
	public static double ExGetDeltaTime(this System.DateTime a_stSender, System.DateTime a_stRhs) {
		return (a_stSender - a_stRhs).TotalSeconds;
	}

	//! 시간 간격을 반환한다
	public static double ExGetDeltaTimePerMinutes(this System.DateTime a_stSender, System.DateTime a_stRhs) {
		return (a_stSender - a_stRhs).TotalMinutes;
	}

	//! 시간 간격을 반환한다
	public static double ExGetDeltaTimePerHours(this System.DateTime a_stSender, System.DateTime a_stRhs) {
		return (a_stSender - a_stRhs).TotalHours;
	}

	//! 시간 간격을 반환한다
	public static double ExGetDeltaTimePerDays(this System.DateTime a_stSender, System.DateTime a_stRhs) {
		return (a_stSender - a_stRhs).TotalDays;
	}

	//! 크기 형식 문자열을 반환한다
	public static string ExGetSizeFormatString(this string a_oSender, int a_nSize) {
		return string.Format(KDefine.B_SIZE_FORMAT_STRING, a_nSize, a_oSender);
	}

	//! 색상 형식 문자열을 반환한다
	public static string ExGetColorFormatString(this string a_oSender, Color a_stColor) {
		return string.Format(KDefine.B_COLOR_FORMAT_STRING, ColorUtility.ToHtmlStringRGBA(a_stColor), a_oSender);
	}

	//! 변경 된 문자열을 반환한다
	public static string ExGetReplaceString(this string a_oSender, string a_oSearch, string a_oReplace, int a_nReplaceTimes = 1) {
		Func.Assert(a_oSender.ExIsValid());
		Func.Assert(a_nReplaceTimes >= 1 && (a_oSearch != null && a_oReplace != null));

		if(!a_oSearch.ExIsEquals(a_oReplace)) {
			for(int i = 0; i < a_nReplaceTimes && a_oSender.Contains(a_oSearch); ++i) {
				a_oSender = a_oSender.Replace(a_oSearch, a_oReplace);
			}
		}

		return a_oSender;
	}

	//! 위치를 변경한다
	public static void ExSetPos(this Transform a_oSender, Vector3 a_stPos, bool a_bIsWorld = false) {
		Func.Assert(a_oSender != null);

		if(a_bIsWorld) {
			a_oSender.position = a_stPos;
		} else {
			a_oSender.localPosition = a_stPos;
		}
	}

	//! 비율을 변경한다
	public static void ExSetScale(this Transform a_oSender, Vector3 a_stScale) {
		Func.Assert(a_oSender != null);
		a_oSender.localScale = a_stScale;
	}

	//! 회전을 변경한다
	public static void ExSetRotation(this Transform a_oSender, Vector3 a_stRotation, bool a_bIsWorld = false) {
		Func.Assert(a_oSender != null);

		if(a_bIsWorld) {
			a_oSender.eulerAngles = a_stRotation;
		} else {
			a_oSender.localEulerAngles = a_stRotation;
		}
	}

	//! X 축 위치를 변경한다
	public static void ExSetPosX(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		Func.Assert(a_oSender != null);
		var stPos = a_bIsWorld ? a_oSender.position : a_oSender.localPosition;

		a_oSender.ExSetPos(new Vector3(a_fValue, stPos.y, stPos.z), a_bIsWorld);
	}
	
	//! Y 축 위치를 변경한다
	public static void ExSetPosY(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		Func.Assert(a_oSender != null);
		var stPos = a_bIsWorld ? a_oSender.position : a_oSender.localPosition;

		a_oSender.ExSetPos(new Vector3(stPos.x, a_fValue, stPos.z), a_bIsWorld);
	}

	//! Z 축 위치를 변경한다
	public static void ExSetPosZ(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		Func.Assert(a_oSender != null);
		var stPos = a_bIsWorld ? a_oSender.position : a_oSender.localPosition;

		a_oSender.ExSetPos(new Vector3(stPos.x, stPos.y, a_fValue), a_bIsWorld);
	}

	//! X 축 비율을 변경한다
	public static void ExSetScaleX(this Transform a_oSender, float a_fValue) {
		Func.Assert(a_oSender != null);
		a_oSender.ExSetScale(new Vector3(a_fValue, a_oSender.localScale.y, a_oSender.localScale.z));
	}

	//! Y 축 비율을 변경한다
	public static void ExSetScaleY(this Transform a_oSender, float a_fValue) {
		Func.Assert(a_oSender != null);
		a_oSender.ExSetScale(new Vector3(a_oSender.localScale.x, a_fValue, a_oSender.localScale.z));
	}

	//! Z 축 비율을 변경한다
	public static void ExSetScaleZ(this Transform a_oSender, float a_fValue) {
		Func.Assert(a_oSender != null);
		a_oSender.ExSetScale(new Vector3(a_oSender.localScale.x, a_oSender.localScale.y, a_fValue));
	}

	//! X 축 각도를 변경한다
	public static void ExSetRotationX(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		Func.Assert(a_oSender != null);
		var stRotation = a_bIsWorld ? a_oSender.eulerAngles : a_oSender.localEulerAngles;

		a_oSender.ExSetRotation(new Vector3(a_fValue, stRotation.y, stRotation.z), a_bIsWorld);
	}
	
	//! Y 축 각도를 변경한다
	public static void ExSetRotationY(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		Func.Assert(a_oSender != null);
		var stRotation = a_bIsWorld ? a_oSender.eulerAngles : a_oSender.localEulerAngles;

		a_oSender.ExSetRotation(new Vector3(stRotation.x, a_fValue, stRotation.z), a_bIsWorld);
	}

	//! Z 축 각도를 변경한다
	public static void ExSetRotationZ(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		Func.Assert(a_oSender != null);
		var stRotation = a_bIsWorld ? a_oSender.eulerAngles : a_oSender.localEulerAngles;

		a_oSender.ExSetRotation(new Vector3(stRotation.x, stRotation.y, a_fValue), a_bIsWorld);
	}

	//! 앵커 위치를 변경한다
	public static void ExSetAnchorPos(this RectTransform a_oSender, Vector2 a_stPos) {
		Func.Assert(a_oSender != null);
		a_oSender.anchoredPosition = a_stPos;
	}

	//! 크기 간격을 변경한다
	public static void ExSetSizeDelta(this RectTransform a_oSender, Vector2 a_stDelta) {
		Func.Assert(a_oSender != null);
		a_oSender.sizeDelta = a_stDelta;
	}

	//! X 축 앵커 위치를 변경한다
	public static void ExSetAnchorPosX(this RectTransform a_oSender, float a_fValue) {
		Func.Assert(a_oSender != null);
		a_oSender.ExSetAnchorPos(new Vector2(a_fValue, a_oSender.anchoredPosition.y));
	}

	//! Y 축 앵커 위치를 변경한다
	public static void ExSetAnchorPosY(this RectTransform a_oSender, float a_fValue) {
		Func.Assert(a_oSender != null);
		a_oSender.ExSetAnchorPos(new Vector2(a_oSender.anchoredPosition.x, a_fValue));
	}

	//! X 축 크기 간격을 변경한다
	public static void ExSetSizeDeltaX(this RectTransform a_oSender, float a_fValue) {
		Func.Assert(a_oSender != null);
		a_oSender.ExSetSizeDelta(new Vector2(a_fValue, a_oSender.sizeDelta.y));
	}

	//! Y 축 크기 간격을 변경한다
	public static void ExSetSizeDeltaY(this RectTransform a_oSender, float a_fValue) {
		Func.Assert(a_oSender != null);
		a_oSender.ExSetSizeDelta(new Vector2(a_oSender.sizeDelta.x, a_fValue));
	}

	//! 위치를 추가한다
	public static void ExAddPos(this Transform a_oSender, Vector3 a_stPos, bool a_bIsWorld = false) {
		Func.Assert(a_oSender != null);

		if(a_bIsWorld) {
			a_oSender.position += a_stPos;
		} else {
			a_oSender.localPosition += a_stPos;
		}
	}

	//! 비율을 추가한다
	public static void ExAddScale(this Transform a_oSender, Vector3 a_stScale) {
		Func.Assert(a_oSender != null);
		a_oSender.localScale += a_stScale;
	}

	//! 회전을 추가한다
	public static void ExAddRotation(this Transform a_oSender, Vector3 a_stRotation, bool a_bIsWorld = false) {
		Func.Assert(a_oSender != null);

		if(a_bIsWorld) {
			a_oSender.eulerAngles += a_stRotation;
		} else {
			a_oSender.localEulerAngles += a_stRotation;
		}
	}

	//! X 축 위치를 추가한다
	public static void ExAddPosX(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		a_oSender.ExAddPos(new Vector3(a_fValue, 0.0f, 0.0f), a_bIsWorld);
	}
	
	//! Y 축 위치를 추가한다
	public static void ExAddPosY(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		a_oSender.ExAddPos(new Vector3(0.0f, a_fValue, 0.0f), a_bIsWorld);
	}

	//! Z 축 위치를 추가한다
	public static void ExAddPosZ(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		a_oSender.ExAddPos(new Vector3(0.0f, 0.0f, a_fValue), a_bIsWorld);
	}

	//! X 축 비율을 추가한다
	public static void ExAddScaleX(this Transform a_oSender, float a_fValue) {
		a_oSender.ExAddScale(new Vector3(a_fValue, 0.0f, 0.0f));
	}

	//! Y 축 비율을 추가한다
	public static void ExAddScaleY(this Transform a_oSender, float a_fValue) {
		a_oSender.ExAddScale(new Vector3(0.0f, a_fValue, 0.0f));
	}

	//! Z 축 비율을 추가한다
	public static void ExAddScaleZ(this Transform a_oSender, float a_fValue) {
		a_oSender.ExAddScale(new Vector3(0.0f, 0.0f, a_fValue));
	}

	//! X 축 각도를 추가한다
	public static void ExAddRotationX(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		a_oSender.ExAddRotation(new Vector3(a_fValue, 0.0f, 0.0f), a_bIsWorld);
	}
	
	//! Y 축 각도를 추가한다
	public static void ExAddRotationY(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		a_oSender.ExAddRotation(new Vector3(0.0f, a_fValue, 0.0f), a_bIsWorld);
	}

	//! Z 축 각도를 추가한다
	public static void ExAddRotationZ(this Transform a_oSender, float a_fValue, bool a_bIsWorld = false) {
		a_oSender.ExAddRotation(new Vector3(0.0f, 0.0f, a_fValue), a_bIsWorld);
	}

	//! 앵커 위치를 추가한다
	public static void ExAddAnchorPos(this RectTransform a_oSender, Vector2 a_stPos) {
		Func.Assert(a_oSender != null);
		a_oSender.anchoredPosition += a_stPos;
	}

	//! 크기 간격을 추가한다
	public static void ExAddSizeDelta(this RectTransform a_oSender, Vector2 a_stDelta) {
		Func.Assert(a_oSender != null);
		a_oSender.sizeDelta += a_stDelta;
	}

	//! X 축 앵커 위치를 추가한다
	public static void ExAddAnchorPosX(this RectTransform a_oSender, float a_fValue) {
		a_oSender.ExAddAnchorPos(new Vector2(a_fValue, 0.0f));
	}

	//! Y 축 앵커 위치를 추가한다
	public static void ExAddAnchorPosY(this RectTransform a_oSender, float a_fValue) {
		a_oSender.ExAddAnchorPos(new Vector2(0.0f, a_fValue));
	}

	//! X 축 크기 간격을 추가한다
	public static void ExAddSizeDeltaX(this RectTransform a_oSender, float a_fValue) {
		a_oSender.ExAddSizeDelta(new Vector2(a_fValue, 0.0f));
	}

	//! Y 축 크기 간격을 추가한다
	public static void ExAddSizeDeltaY(this RectTransform a_oSender, float a_fValue) {
		a_oSender.ExAddSizeDelta(new Vector2(0.0f, a_fValue));
	}

	//! 시간을 비교한다
	public static int ExCompare(this System.DateTime a_stSender, System.DateTime a_stRhs) {
		var stLhs = new System.TimeSpan(a_stSender.Ticks);
		var stRhs = new System.TimeSpan(a_stRhs.Ticks);

		if(stLhs.TotalSeconds.ExIsEquals(stRhs.TotalSeconds)) {
			return KDefine.B_COMPARE_RESULT_EQUALS;
		}

		return stLhs.TotalSeconds.ExIsLess(stRhs.TotalSeconds) ? KDefine.B_COMPARE_RESULT_LESS 
			: KDefine.B_COMPARE_RESULT_GREATE;
	}

	//! 값을 순회한다
	public static void ExEnumerate(this IEnumerator a_oSender, System.Action<object> a_oCallback) {
		Func.Assert(a_oSender != null && a_oCallback != null);

		while(a_oSender.MoveNext()) {
			a_oCallback?.Invoke(a_oSender.Current);
		}
	}

	//! 리스트 -> 비트로 변환한다
	public static int ExToBits(this List<int> a_oSender) {
		int nValue = 0;

		for(int i = 0; i < a_oSender?.Count; ++i) {
			nValue |= 1 << a_oSender[i];
		}

		return nValue;
	}

	//! 바이트 -> 메가 바이트로 변환한다
	public static double ExToMegaByte(this uint a_oSender) {
		return a_oSender / 1024.0 / 1024.0;
	}

	//! 바이트 -> 메가 바이트로 변환한다
	public static double ExToMegaByte(this long a_oSender) {
		return a_oSender / 1024.0 / 1024.0;
	}

	//! 문자열 -> 시간으로 변환한다
	public static System.DateTime ExToTime(this string a_oSender, string a_oFormat) {
		Func.Assert(a_oSender.ExIsValid() && a_oFormat.ExIsValid());
		return System.DateTime.ParseExact(a_oSender, a_oFormat, CultureInfo.InvariantCulture);
	}

	//! 시간 -> 문자열로 변환한다
	public static string ExToString(this System.DateTime a_stSender, string a_oFormat) {
		Func.Assert(a_oFormat.ExIsValid());
		return a_stSender.ToString(a_oFormat, CultureInfo.InvariantCulture);
	}

	//! 시간 -> 긴 문자열로 변환한다
	public static string ExToLongString(this System.DateTime a_stSender) {
		return a_stSender.ExToString(KDefine.B_DATE_TIME_FORMAT_YYYY_MM_DD_HH_MM_SS);
	}

	//! 시간 -> 짧은 문자열로 변환한다
	public static string ExToShortString(this System.DateTime a_stSender) {
		return a_stSender.ExToString(KDefine.B_DATE_TIME_FORMAT_YYYY_MM_DD);
	}

	//! 지역 시간 -> PST 시간으로 변환한다
	public static System.DateTime ExToPSTTime(this System.DateTime a_stSender) {
		var stUTCTime = a_stSender.ToUniversalTime();
		return stUTCTime.AddHours(KDefine.B_DELTA_TIME_UTC_TO_PST);
	}

	//! 지역 시간 -> 특정 지역 시간으로 변환한다
	public static System.DateTime ExToZoneTime(this System.DateTime a_stSender, string a_oTimeZoneID) {
		Func.Assert(a_oTimeZoneID.ExIsValid());

		var oTimeZoneInfo = System.TimeZoneInfo.FindSystemTimeZoneById(a_oTimeZoneID);
		return System.TimeZoneInfo.ConvertTime(a_stSender, System.TimeZoneInfo.Local, oTimeZoneInfo);
	}

	//! PST 시간 -> 지역 시간으로 변환한다
	public static System.DateTime ExPSTToLocalTime(this System.DateTime a_stSender) {
		var stUTCTime = a_stSender.AddHours(-KDefine.B_DELTA_TIME_UTC_TO_PST);
		return stUTCTime.ToLocalTime();
	}

	//! 특정 지역 시간 -> 지역 시간으로 변환한다
	public static System.DateTime ExToLocalTime(this System.DateTime a_stSender, string a_oTimeZoneID) {
		Func.Assert(a_oTimeZoneID.ExIsValid());

		var oTimeZoneInfo = System.TimeZoneInfo.FindSystemTimeZoneById(a_oTimeZoneID);
		return System.TimeZoneInfo.ConvertTime(a_stSender, oTimeZoneInfo, System.TimeZoneInfo.Local);
	}
	#endregion			// 클래스 함수

	#region 제네릭 클래스 함수
	//! 유효 여부를 검사한다
	public static bool ExIsValid<T>(this T[] a_oSender) {
		return a_oSender != null && a_oSender.Length >= 1;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid<T>(this T[,] a_oSender) {
		return a_oSender != null && (a_oSender.GetLength(0) >= 1 && a_oSender.GetLength(1) >= 1);
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid<T>(this List<T> a_oSender) {
		return a_oSender != null && a_oSender.Count >= 1;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid<Key, Value>(this Dictionary<Key, Value> a_oSender) {
		return a_oSender != null && a_oSender.Count >= 1;
	}

	//! 인덱스 유효 여부를 검사한다
	public static bool ExIsValidIndex<T>(this T[] a_oSender, int a_nIndex) {
		Func.Assert(a_oSender != null);
		return a_nIndex > KDefine.B_INDEX_INVALID && a_nIndex < a_oSender.Length;
	}

	//! 인덱스 유효 여부룰 검사한다
	public static bool ExIsValidIndex<T>(this List<T> a_oSender, int a_nIndex) {
		Func.Assert(a_oSender != null);
		return a_nIndex > KDefine.B_INDEX_INVALID && a_nIndex < a_oSender.Count;
	}

	//! 비동기 작업 완료 여부를 검사한다
	public static bool ExIsComplete<T>(this Task<T> a_oSender) {
		bool bIsComplete = a_oSender != null && (a_oSender.IsCompleted && !a_oSender.IsFaulted && !a_oSender.IsCanceled);
		return bIsComplete && a_oSender.Result != null;
	}

	//! 값을 리셋한다
	public static void ExResetValues<T>(this T[] a_oSender) {
		Func.Assert(a_oSender != null);

		for(int i = 0; i < a_oSender.Length; ++i) {
			a_oSender[i] = default(T);
		}
	}

	//! 값을 리셋한다
	public static void ExResetValues<T>(this T[,] a_oSender) {
		Func.Assert(a_oSender != null);

		for(int i = 0; i < a_oSender.GetLength(0); ++i) {
			for(int j = 0; j < a_oSender.GetLength(1); ++j) {
				a_oSender[i, j] = default(T);
			}
		}
	}

	//! 값을 반환한다
	public static T ExGetValue<T>(this T[] a_oSender, int a_nIndex, T a_tDefValue) {
		Func.Assert(a_oSender != null);
		return (a_nIndex < a_oSender.Length) ? a_oSender[a_nIndex] : a_tDefValue;
	}

	//! 값을 반환한다
	public static T ExGetValue<T>(this List<T> a_oSender, int a_nIndex, T a_tDefValue) {
		Func.Assert(a_oSender != null);
		return (a_nIndex < a_oSender.Count) ? a_oSender[a_nIndex] : a_tDefValue;
	}

	//! 값을 반환한다
	public static Value ExGetValue<Key, Value>(this Dictionary<Key, Value> a_oSender, Key a_tKey, Value a_tDefValue) {
		Func.Assert(a_oSender != null);
		return a_oSender.ContainsKey(a_tKey) ? a_oSender[a_tKey] : a_tDefValue;
	}

	//! 필드 값을 반환한다
	public static object ExGetFieldValue<T>(this object a_oSender, string a_oName, BindingFlags a_eBindingFlags) {
		var oType = typeof(T);
		var oFieldInfo = oType.GetField(a_oName, a_eBindingFlags);

		Func.Assert(oFieldInfo != null);
		return oFieldInfo.GetValue(a_oSender);
	}

	//! 런타임 필드 값을 반환한다
	public static object ExGetRuntimeFieldValue<T>(this object a_oSender, string a_oName) {
		var oType = typeof(T);
		var oFieldInfos = oType.GetRuntimeFields();

		Func.Assert(oFieldInfos != null);

		foreach(var oFieldInfo in oFieldInfos) {
			if(oFieldInfo.Name.ExIsEquals(a_oName)) {
				return oFieldInfo.GetValue(a_oSender);
			}
		}

		return null;
	}

	//! 프로퍼티 값을 반환한다
	public static object ExGetPropertyValue<T>(this object a_oSender, string a_oName, BindingFlags a_eBindingFlags) {
		var oType = typeof(T);
		var oPropertyInfo = oType.GetProperty(a_oName, a_eBindingFlags);

		Func.Assert(oPropertyInfo != null);
		return oPropertyInfo.GetValue(a_oSender);
	}

	//! 런타임 프로퍼티 값을 반환한다
	public static object ExGetRuntimePropertyValue<T>(this object a_oSender, string a_oName) {
		var oType = typeof(T);
		var oPropertyInfos = oType.GetRuntimeProperties();

		Func.Assert(oPropertyInfos != null);

		foreach(var oPropertyInfo in oPropertyInfos) {
			if(oPropertyInfo.Name.ExIsEquals(a_oName)) {
				return oPropertyInfo.GetValue(a_oSender);
			}
		}

		return null;
	}

	//! 값을 변경한다
	public static void ExSetValue<T>(this T[] a_oSender, int a_nIndex, T a_tValue) {
		Func.Assert(a_oSender != null);

		if(a_nIndex >= 0 && a_nIndex < a_oSender.Length) {
			a_oSender[a_nIndex] = a_tValue;
		}
	}

	//! 값을 변경한다
	public static void ExSetValue<T>(this List<T> a_oSender, int a_nIndex, T a_tValue) {
		Func.Assert(a_oSender != null);

		if(a_nIndex >= 0 && a_nIndex < a_oSender.Count) {
			a_oSender[a_nIndex] = a_tValue;
		}
	}

	//! 값을 변경한다
	public static void ExSetValue<Key, Value>(this Dictionary<Key, Value> a_oSender, Key a_tKey, Value a_tValue) {
		Func.Assert(a_oSender != null);

		if(a_oSender.ContainsKey(a_tKey)) {
			a_oSender[a_tKey] = a_tValue;
		}
	}

	//! 필드 값을 변경한다
	public static void ExSetFieldValue<T>(this object a_oSender, string a_oName, BindingFlags a_eBindingFlags, object a_oValue) {
		var oType = typeof(T);
		var oFieldInfo = oType.GetField(a_oName, a_eBindingFlags);

		Func.Assert(oFieldInfo != null);
		oFieldInfo.SetValue(a_oSender, a_oValue);
	}

	//! 런타임 필드 값을 변경한다
	public static void ExSetRuntimeFieldValue<T>(this object a_oSender, string a_oName, object a_oValue) {
		var oType = typeof(T);
		var oFieldInfos = oType.GetRuntimeFields();

		Func.Assert(oFieldInfos != null && a_oName.ExIsValid());

		foreach(var oFieldInfo in oFieldInfos) {
			if(oFieldInfo.Name.ExIsEquals(a_oName)) {
				oFieldInfo.SetValue(a_oSender, a_oValue);
			}
		}
	}

	//! 프로퍼티 값을 변경한다
	public static void ExSetPropertyValue<T>(this object a_oSender, string a_oName, BindingFlags a_eBindingFlags, object a_oValue) {
		var oType = typeof(T);
		var oPropertyInfo = oType.GetProperty(a_oName, a_eBindingFlags);

		Func.Assert(oPropertyInfo != null);
		oPropertyInfo.SetValue(a_oSender, a_oValue);
	}

	//! 런타임 프로퍼티 값을 변경한다
	public static void ExSetRuntimePropertyValue<T>(this object a_oSender, string a_oName, object a_oValue) {
		var oType = typeof(T);
		var oPropertyInfos = oType.GetRuntimeProperties();

		Func.Assert(oPropertyInfos != null && a_oName.ExIsValid());

		foreach(var oPropertyInfo in oPropertyInfos) {
			if(oPropertyInfo.Name.ExIsEquals(a_oName)) {
				oPropertyInfo.SetValue(a_oSender, a_oValue);
			}
		}
	}

	//! 값을 추가한다
	public static void ExAddValue<T>(this List<T> a_oSender, T a_tValue, bool a_bIsReplace = false) {
		Func.Assert(a_oSender != null);
		int nIndex = a_oSender.IndexOf(a_tValue);

		if(nIndex <= KDefine.B_INDEX_INVALID) {
			a_oSender.Add(a_tValue);
		} else if(a_bIsReplace) {
			a_oSender[nIndex] = a_tValue;
		}
	}

	//! 값을 추가한다
	public static void ExAddValue<Key, Value>(this Dictionary<Key, Value> a_oSender, Key a_tKey, Value a_tValue, bool a_bIsReplace = false) {
		Func.Assert(a_oSender != null);

		if(!a_oSender.ContainsKey(a_tKey)) {
			a_oSender.Add(a_tKey, a_tValue);
		} else if(a_bIsReplace) {
			a_oSender[a_tKey] = a_tValue;
		}
	}

	//! 값을 추가한다
	public static void ExAddValues<T>(this List<T> a_oSender, T[] a_oValues, bool a_bIsReplace = false) {
		Func.Assert(a_oValues != null);

		for(int i = 0; i < a_oValues.Length; ++i) {
			a_oSender.ExAddValue(a_oValues[i], a_bIsReplace);
		}
	}

	//! 값을 추가한다
	public static void ExAddValues<T>(this List<T> a_oSender, List<T> a_oValueList, bool a_bIsReplace = false) {
		Func.Assert(a_oValueList != null);

		for(int i = 0; i < a_oValueList.Count; ++i) {
			a_oSender.ExAddValue(a_oValueList[i], a_bIsReplace);
		}
	}

	//! 값을 추가한다
	public static void ExAddValues<Key, Value>(this Dictionary<Key, Value> a_oSender, Dictionary<Key, Value> a_oValueList, bool a_bIsReplace = false) {
		Func.Assert(a_oValueList != null);

		foreach(var stKeyValue in a_oValueList) {
			a_oSender.ExAddValue(stKeyValue.Key, stKeyValue.Value, a_bIsReplace);
		}
	}

	//! 값을 대체한다
	public static void ExReplaceValue<T>(this List<T> a_oSender, T a_tValue) {
		a_oSender.ExAddValue(a_tValue, true);
	}

	//! 값을 대체한다
	public static void ExReplaceValue<Key, Value>(this Dictionary<Key, Value> a_oSender, Key a_tKey, Value a_tValue) {
		a_oSender.ExAddValue(a_tKey, a_tValue, true);
	}

	//! 값을 대체한다
	public static void ExReplaceValues<T>(this List<T> a_oSender, T[] a_oValues) {
		a_oSender.ExAddValues(a_oValues, true);
	}

	//! 값을 대체한다
	public static void ExReplaceValues<T>(this List<T> a_oSender, List<T> a_oValueList) {
		a_oSender.ExAddValues(a_oValueList, true);
	}

	//! 값을 대체한다
	public static void ExReplaceValues<Key, Value>(this Dictionary<Key, Value> a_oSender, Dictionary<Key, Value> a_oValueList) {
		a_oSender.ExAddValues(a_oValueList, true);
	}

	//! 값을 제거한다
	public static void ExRemoveValueAt<T>(this List<T> a_oSender, int a_nIndex) {
		Func.Assert(a_oSender != null);

		if(a_oSender.ExIsValidIndex(a_nIndex)) {
			a_oSender.RemoveAt(a_nIndex);
		}
	}

	//! 값을 제거한다
	public static void ExRemoveValue<T>(this List<T> a_oSender, T a_tValue) {
		Func.Assert(a_oSender != null);

		if(a_oSender.Contains(a_tValue)) {
			int nIndex = a_oSender.IndexOf(a_tValue);
			a_oSender.ExRemoveValueAt(nIndex);
		}
	}

	//! 값을 제거한다
	public static void ExRemoveValue<T>(this List<T> a_oSender, System.Func<T, bool> a_oCompare) {
		int nIndex = a_oSender.ExFindValue(a_oCompare);

		if(nIndex > KDefine.B_INDEX_INVALID) {
			a_oSender.ExRemoveValueAt(nIndex);
		}
	}

	//! 값을 제거한다
	public static void ExRemoveValue<Key, Value>(this Dictionary<Key, Value> a_oSender, Key a_tKey) {
		Func.Assert(a_oSender != null);

		if(a_oSender.ContainsKey(a_tKey)) {
			a_oSender.Remove(a_tKey);
		}
	}

	//! 값을 제거한다
	public static void ExRemoveValue<Key, Value>(this Dictionary<Key, Value> a_oSender, System.Func<Value, bool> a_oCompare) {
		var stResult = a_oSender.ExFindValue(a_oCompare);

		if(stResult.Key) {
			a_oSender.ExRemoveValue(stResult.Value);
		}
	}

	//! 값을 제거한다
	public static void ExRemoveValues<T>(this List<T> a_oSender, T[] a_oValues) {
		Func.Assert(a_oValues != null);

		for(int i = 0; i < a_oValues.Length; ++i) {
			a_oSender.ExRemoveValue(a_oValues[i]);
		}
	}

	//! 값을 제거한다
	public static void ExRemoveValues<T>(this List<T> a_oSender, List<T> a_oValueList) {
		Func.Assert(a_oValueList != null);

		for(int i = 0; i < a_oValueList.Count; ++i) {
			a_oSender.ExRemoveValue(a_oValueList[i]);
		}
	}

	//! 값을 제거한다
	public static void ExRemoveValues<Key, Value>(this Dictionary<Key, Value> a_oSender, Key[] a_oKeys) {
		Func.Assert(a_oKeys != null);

		for(int i = 0; i < a_oKeys.Length; ++i) {
			a_oSender.ExRemoveValue(a_oKeys[i]);
		}
	}

	//! 값을 제거한다
	public static void ExRemoveValues<Key, Value>(this Dictionary<Key, Value> a_oSender, List<Key> a_oKeyList) {
		Func.Assert(a_oKeyList != null);

		for(int i = 0; i < a_oKeyList.Count; ++i) {
			a_oSender.ExRemoveValue(a_oKeyList[i]);
		}
	}

	//! 값을 교환한다
	public static void ExSwap<T>(this T[] a_oSender, int a_nIndexA, int a_nIndexB) {
		Func.Assert(a_oSender.ExIsValid());
		Func.Assert(a_oSender.ExIsValidIndex(a_nIndexA) && a_oSender.ExIsValidIndex(a_nIndexB));

		Func.Swap(ref a_oSender[a_nIndexA], ref a_oSender[a_nIndexB]);
	}

	//! 값을 교환한다
	public static void ExSwap<T>(this List<T> a_oSender, int a_nIndexA, int a_nIndexB) {
		Func.Assert(a_oSender.ExIsValid());
		Func.Assert(a_oSender.ExIsValidIndex(a_nIndexA) && a_oSender.ExIsValidIndex(a_nIndexB));

		T tTemp = a_oSender[a_nIndexA];
		a_oSender[a_nIndexA] = a_oSender[a_nIndexB];
		a_oSender[a_nIndexB] = tTemp;
	}

	//! 값을 섞는다
	public static void ExShuffle<T>(this T[] a_oSender) {
		for(int i = 0; i < a_oSender?.Length; ++i) {
			int nIndex = Random.Range(0, a_oSender.Length);
			a_oSender.ExSwap(i, nIndex);
		}
	}

	//! 값을 섞는다
	public static void ExShuffle<T>(this List<T> a_oSender) {
		for(int i = 0; i < a_oSender?.Count; ++i) {
			int nIndex = Random.Range(0, a_oSender.Count);
			a_oSender.ExSwap(i, nIndex);
		}
	}

	//! 안전 정렬을 수행한다
	public static void ExStableSort<T>(this T[] a_oSender, System.Comparison<T> a_oCompare) {
		Func.Assert(a_oSender.ExIsValid());
		var oTempValues = new T[a_oSender.Length];

		Func.StableSort(a_oSender, oTempValues, 0, a_oSender.Length - 1, a_oCompare);
	}

	//! 안전 정렬을 수행한다
	public static void ExStableSort<T>(this List<T> a_oSender, System.Comparison<T> a_oCompare) {
		Func.Assert(a_oSender.ExIsValid());
		var oTempValues = new T[a_oSender.Count];

		Func.StableSort(a_oSender, oTempValues, 0, a_oSender.Count - 1, a_oCompare);
	}

	//! 1 차원 배열 -> 2 차원 배열로 복사한다
	public static void ExCopyToMultiArray<T>(this T[] a_oSender, T[,] a_oDestValues) {
		bool bIsEnable = a_oSender.ExIsValid() && a_oDestValues.ExIsValid(); 
		Func.Assert(bIsEnable && a_oSender.Length == a_oDestValues.Length);

		for(int i = 0; i < a_oSender.Length; ++i) {
			int nRow = i / a_oDestValues.GetLength(1);
			int nCol = i % a_oDestValues.GetLength(1);

			a_oDestValues[nRow, nCol] = a_oSender[i];
		}
	}

	//! 2 차원 배열 -> 1 차원 배열로 복사한다
	public static void ExCopyToSingleArray<T>(this T[,] a_oSender, T[] a_oDestValues) {
		bool bIsEnable = a_oSender.ExIsValid() && a_oDestValues.ExIsValid(); 
		Func.Assert(bIsEnable && a_oSender.Length == a_oDestValues.Length);

		for(int i = 0; i < a_oSender.GetLength(0); ++i) {
			for(int j = 0; j < a_oSender.GetLength(1); ++j) {
				int nIndex = (i * a_oSender.GetLength(1)) + j;
				a_oDestValues[nIndex] = a_oSender[i, j];
			}
		}
	}

	//! 값을 순회한다
	public static void ExEnumerate<T>(this T[] a_oSender, System.Action<int, T> a_oCallback) {
		Func.Assert(a_oCallback != null && a_oSender.ExIsValid());

		for(int i = 0; i < a_oSender.Length; ++i) {
			a_oCallback(i, a_oSender[i]);
		}
	}

	//! 값을 순회한다
	public static void ExEnumerate<T>(this T[,] a_oSender, System.Action<int, int, T> a_oCallback) {
		Func.Assert(a_oCallback != null && a_oSender.ExIsValid());

		for(int i = 0; i < a_oSender.GetLength(0); ++i) {
			for(int j = 0; j < a_oSender.GetLength(1); ++j) {
				a_oCallback(i, j, a_oSender[i, j]);
			}
		}
	}

	//! 값을 순회한다
	public static void ExEnumerate<T>(this List<T> a_oSender, System.Action<int, T> a_oCallback) {
		Func.Assert(a_oCallback != null && a_oSender.ExIsValid());

		for(int i = 0; i < a_oSender.Count; ++i) {
			a_oCallback(i, a_oSender[i]);
		}
	}

	//! 값을 순회한다
	public static void ExEnumerate<Key, Value>(this Dictionary<Key, Value> a_oSender, System.Action<Key, Value> a_oCallback) {
		Func.Assert(a_oCallback != null && a_oSender.ExIsValid());

		foreach(var stKeyValue in a_oSender) {
			a_oCallback(stKeyValue.Key, stKeyValue.Value);
		}
	}

	//! 값을 탐색한다
	public static int ExFindValue<T>(this T[] a_oSender, System.Func<T, bool> a_oCompare) {
		Func.Assert(a_oSender != null && a_oCompare != null);

		for(int i = 0; i < a_oSender.Length; ++i) {
			if(a_oCompare(a_oSender[i])) {
				return i;
			}
		}

		return KDefine.B_INDEX_INVALID;
	}

	//! 값을 탐색한다
	public static int ExFindValue<T>(this List<T> a_oSender, System.Func<T, bool> a_oCompare) {
		Func.Assert(a_oSender != null && a_oCompare != null);

		for(int i = 0; i < a_oSender.Count; ++i) {
			if(a_oCompare(a_oSender[i])) {
				return i;
			}
		}

		return KDefine.B_INDEX_INVALID;
	}

	//! 값을 탐색한다
	public static KeyValuePair<bool, Key> ExFindValue<Key, Value>(this Dictionary<Key, Value> a_oSender, System.Func<Value, bool> a_oCompare) {
		Func.Assert(a_oSender != null && a_oCompare != null);

		foreach(var stKeyValue in a_oSender) {
			if(a_oCompare(stKeyValue.Value)) {
				return new KeyValuePair<bool, Key>(true, stKeyValue.Key);
			}
		}

		return new KeyValuePair<bool, Key>(false, default(Key));
	}

	//! 1 차원 배열 -> 2 차원 배열로 변환한다
	public static T[,] ExToMultiArray<T>(this T[] a_oSender, int a_nNumRows, int a_nNumCols) {
		Func.Assert(a_oSender.ExIsValid() && (a_nNumRows * a_nNumCols >= a_oSender.Length));

		var oConvertValues = new T[a_nNumRows, a_nNumCols];
		a_oSender.ExCopyToMultiArray(oConvertValues);

		return oConvertValues;
	}

	//! 2 차원 배열 -> 1 차원 배열로 변환한다
	public static T[] ExToSingleArray<T>(this T[,] a_oSender) {
		Func.Assert(a_oSender.ExIsValid());

		var oConvertValues = new T[a_oSender.Length];
		a_oSender.ExCopyToSingleArray(oConvertValues);

		return oConvertValues;
	}

	//! 배열 -> 문자열로 변환한다
	public static string ExToString<T>(this T[] a_oSender, string a_oSeparateToken) {
		Func.Assert(a_oSeparateToken != null && a_oSender.ExIsValid());
		var oStringBuilder = new System.Text.StringBuilder();

		for(int i = 0; i < a_oSender.Length; ++i) {
			oStringBuilder.Append(a_oSender[i]);

			if(i < a_oSender.Length - 1) {
				oStringBuilder.Append(a_oSeparateToken);
			}
		}

		return oStringBuilder.ToString();
	}

	//! 리스트 -> 문자열로 변환한다
	public static string ExToString<T>(this List<T> a_oSender, string a_oSeparateToken) {
		Func.Assert(a_oSeparateToken != null && a_oSender.ExIsValid());
		var oStringBuilder = new System.Text.StringBuilder();

		for(int i = 0; i < a_oSender.Count; ++i) {
			oStringBuilder.Append(a_oSender[i]);

			if(i < a_oSender.Count - 1) {
				oStringBuilder.Append(a_oSeparateToken);
			}
		}

		return oStringBuilder.ToString();
	}

	//! 사전 -> 문자열로 변환한다
	public static string ExToString<Key, Value>(this Dictionary<Key, Value> a_oSender, string a_oSeparateToken) {
		Func.Assert(a_oSeparateToken != null && a_oSender.ExIsValid());

		int i = 0;
		var oStringBuilder = new System.Text.StringBuilder();

		foreach(var stKeyValue in a_oSender) {
			oStringBuilder.AppendFormat(KDefine.B_DICTIONARY_FORMAT_STRING, 
				stKeyValue.Key, stKeyValue.Value);

			if(i < a_oSender.Count - 1) {
				oStringBuilder.Append(a_oSeparateToken);
			}

			i += 1;
		}

		return oStringBuilder.ToString();
	}

	//! 유니티 객체 -> 특정 타입으로 변환한다
	public static List<T> ExToTypes<T>(this Object[] a_oSender) where T : class {
		Func.Assert(a_oSender.ExIsValid());
		var oConvertTypes = new List<T>();

		for(int i = 0; i < a_oSender.Length; ++i) {
			var oConvertType = a_oSender[i] as T;

			if(oConvertType != null) {
				oConvertTypes.Add(oConvertType);
			}
		}

		return oConvertTypes;
	}

	//! 유니티 객체 -> 특정 타입으로 변환한다
	public static List<T> ExToTypes<T>(this List<Object> a_oSender) where T : class {
		Func.Assert(a_oSender.ExIsValid());
		var oConvertTypeList = new List<T>();

		for(int i = 0; i < a_oSender.Count; ++i) {
			var oConvertType = a_oSender[i] as T;

			if(oConvertType != null) {
				oConvertTypeList.Add(oConvertType);
			}
		}

		return oConvertTypeList;
	}

	//! 객체 -> JSON 문자열로 변환한다
	public static string ExToJSONString<T>(this T a_tSender, bool a_bIsNeedRoot = false, bool a_bIsPretty = false) {
		object oObject = !a_bIsNeedRoot ? a_tSender as object : new Dictionary<string, object>() {
			[KDefine.B_KEY_JSON_ROOT_DATA] = a_tSender
		};

		var oJSON = JSON.Serialize(oObject, new SerializeSettings() {
			AllowNonStringDictionaryKeys = true
		});

		return a_bIsPretty ? oJSON.CreatePrettyString() : oJSON.CreateString();
	}

	//! JSON 문자열 -> 객체로 변환한다
	public static T ExJSONStringToObject<T>(this string a_oSender) {
		Func.Assert(a_oSender.ExIsValid());
		return JSON.ParseString(a_oSender).Deserialize<T>();
	}

	//! 함수를 호출한다
	public static object ExCallFunc<T>(this object a_oSender, string a_oName, BindingFlags a_eBindingFlags, object[] a_oParams) {
		var oType = typeof(T);
		var oMethodInfo = oType.GetMethod(a_oName, a_eBindingFlags);

		Func.Assert(oMethodInfo != null);
		return oMethodInfo.Invoke(a_oSender, a_oParams);
	}
	

	//! 런타임 함수를 호출한다
	public static object ExRuntimeCallFunc<T>(this object a_oSender, string a_oName, object[] a_oParams) {
		var oType = typeof(T);
		var oMethodInfos = oType.GetRuntimeMethods();

		Func.Assert(oMethodInfos != null && a_oName.ExIsValid());

		foreach(var oMethodInfo in oMethodInfos) {
			if(oMethodInfo.Name.ExIsEquals(a_oName)) {
				return oMethodInfo.Invoke(a_oSender, a_oParams);
			}
		}

		return null;
	}
	#endregion			// 제네릭 클래스 함수

	#region 조건부 클래스 함수
#if MESSAGE_PACK_ENABLE
	//! 문자열 -> 압축 된 문자열로 변환한다
	public static string ExToCompressString(this string a_oSender, System.Text.Encoding a_oEncoding) {
		Func.Assert(a_oEncoding != null && a_oSender.ExIsValid());

#if UNITY_STANDALONE
		var oBytes = MessagePackSerializer.Serialize<string>(a_oSender);
		return System.Convert.ToBase64String(oBytes, 0, oBytes.Length);
#else
		using(var oMemoryStream = new MemoryStream()) {
			var oBytes = a_oEncoding.GetBytes(a_oSender);

			using(var oGZipStream = new GZipStream(oMemoryStream, CompressionMode.Compress, true)) {
				oGZipStream.Write(oBytes, 0, oBytes.Length);
			}

			oMemoryStream.Seek(0, SeekOrigin.Begin);

			var oCompressBytes = new byte[oMemoryStream.Length];
			oMemoryStream.Read(oCompressBytes, 0, (int)oMemoryStream.Length);

			var oBufferBytes = new byte[oCompressBytes.Length + 4];
			System.Buffer.BlockCopy(oCompressBytes, 0, oBufferBytes, 4, oCompressBytes.Length);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(oBytes.Length), 0, oBufferBytes, 0, 4);

			return System.Convert.ToBase64String(oBufferBytes);
		}
#endif			// #if UNITY_STANDALONE
	}

	//! 압축 된 문자열 -> 문자열로 변환한다
	public static string ExCompressStringToString(this string a_oSender, System.Text.Encoding a_oEncoding) {
		Func.Assert(a_oEncoding != null && a_oSender.ExIsValid());
		var oBytes = System.Convert.FromBase64String(a_oSender);

#if UNITY_STANDALONE
		return MessagePackSerializer.Deserialize<string>(oBytes);
#else
		using(var oMemoryStream = new MemoryStream(oBytes, 4, oBytes.Length - 4)) {
			int nLength = System.BitConverter.ToInt32(oBytes, 0);
			var oDecompressBytes = new byte[nLength];

			using(var oGZipStream = new GZipStream(oMemoryStream, CompressionMode.Decompress, true)) {
				oGZipStream.Read(oDecompressBytes, 0, oDecompressBytes.Length);
			}

			return a_oEncoding.GetString(oDecompressBytes);
		}
#endif			// #if UNITY_STANDALONE
	}
#endif			// #if MESSAGE_PACK_ENABLE
	#endregion			// 조건부 클래스 함수

	#region 조건부 제네릭 클래스 함수
#if MESSAGE_PACK_ENABLE
	//! 객체 -> JSON 문자열로 변환한다
	public static string ExToMessagePackJSONString<T>(this T a_tSender) {
		var oBytes = MessagePackSerializer.Serialize<T>(a_tSender);
		return MessagePackSerializer.ConvertToJson(oBytes);
	}

	//! JSON 문자열 -> 객체로 변환한다
	public static T ExMessagePackJSONStringToObject<T>(string a_oSender) {
		var oBytes = MessagePackSerializer.ConvertFromJson(a_oSender);
		return MessagePackSerializer.Deserialize<T>(oBytes);
	}
#endif			// #if MESSAGE_PACK_ENABLE

#if YAML_SERIALIZER_ENABLE
	//! 객체 -> YAML 문자열로 변환한다
	public static string ExToYAMLString<T>(this T a_tSender) {
		var oBuilder = new SerializerBuilder().Build();
		return oBuilder.Serialize(a_tSender);
	}

	//! YAML -> 객체로 변환한다
	public static T ExYAMLStringToObject<T>(this string a_oSender) {
		Func.Assert(a_oSender.ExIsValid());
		var oBuilder = new DeserializerBuilder().Build();

		return oBuilder.Deserialize<T>(a_oSender);
	}
#endif			// #if YAML_SERIALIZER_ENABLE
	#endregion			// 조건부 제네릭 클래스 함수
}
