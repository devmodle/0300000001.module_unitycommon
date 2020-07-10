using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif			// #if UNITY_EDITOR

//! 유틸리티 확장 클래스
public static partial class CExtension {
	#region 클래스 함수
	//! 비율 벡터를 반환한다
	public static Vector2 ExGetScaleVector(this Vector2 a_stSender, Vector2 a_stScale) {
		a_stSender.x *= a_stScale.x;
		a_stSender.y *= a_stScale.y;

		return a_stSender;
	}

	//! 비율 벡터를 반환한다
	public static Vector3 ExGetScaleVector(this Vector3 a_stSender, Vector3 a_stScale) {
		a_stSender.x *= a_stScale.x;
		a_stSender.y *= a_stScale.y;
		a_stSender.z *= a_stScale.z;

		return a_stSender;
	}

	//! 비율 벡터를 반환한다
	public static Vector4 ExGetScaleVector(this Vector4 a_stSender, Vector4 a_stScale) {
		a_stSender.x *= a_stScale.x;
		a_stSender.y *= a_stScale.y;
		a_stSender.z *= a_stScale.z;
		a_stSender.w *= a_stScale.w;

		return a_stSender;
	}

	//! 캔버스 월드 위치를 반환한다
	public static Vector3 ExGetWorldPos(this PointerEventData a_oSender) {
		Func.Assert(a_oSender != null);
		var stScreenSize = Func.GetDeviceScreenSize();

		float fAspect = stScreenSize.x / stScreenSize.y;
		float fScreenWidth = KDefine.B_SCREEN_HEIGHT * fAspect;

		var stNormalPos = new Vector3(((a_oSender.position.x * 2.0f) / stScreenSize.x) - 1.0f,
			((a_oSender.position.y * 2.0f) / stScreenSize.y) - 1.0f, 0.0f);

		stNormalPos.x *= (fScreenWidth / 2.0f) * KDefine.B_UNIT_SCALE;
		stNormalPos.y *= (KDefine.B_SCREEN_HEIGHT / 2.0f) * KDefine.B_UNIT_SCALE;

		return stNormalPos;
	}

	//! 캔버스 로컬 위치를 반환한다
	public static Vector3 ExGetLocalPos(this PointerEventData a_oSender, GameObject a_oObj) {
		var stPos = a_oSender.ExGetWorldPos();
		return stPos.ExToLocal(a_oObj);
	}

	//! 캔버스 월드 비율 위치를 반환한다
	public static Vector3 ExGetWorldScalePos(this PointerEventData a_oSender, Vector3 a_stScale) {
		var stPos = a_oSender.ExGetWorldPos();
		return stPos.ExGetScaleVector(a_stScale);
	}

	//! 캔버스 로컬 비율 위치를 반환한다
	public static Vector3 ExGetLocalScalePos(this PointerEventData a_oSender, GameObject a_oObj, Vector3 a_stScale) {
		var stPos = a_oSender.ExGetWorldScalePos(a_stScale);
		return stPos.ExToLocal(a_oObj);
	}

	//! 보정된 캔버스 월드 위치를 반환한다
	public static Vector3 ExGetCorrectWorldPos(this Vector3 a_stSender) {
		var stResolution = Func.GetResolution();

		float fPosX = Mathf.Clamp(a_stSender.x, (stResolution.x / -2.0f) * KDefine.B_UNIT_SCALE, (stResolution.x / 2.0f) * KDefine.B_UNIT_SCALE);
		float fPosY = Mathf.Clamp(a_stSender.y, (stResolution.y / -2.0f) * KDefine.B_UNIT_SCALE, (stResolution.y / 2.0f) * KDefine.B_UNIT_SCALE);

		return new Vector3(fPosX, fPosY, a_stSender.z);
	}

	//! 보정된 캔버스 월드 위치를 반환한다
	public static Vector3 ExGetCorrectWorldPos(this PointerEventData a_oSender) {
		var stPos = a_oSender.ExGetWorldPos();
		return stPos.ExGetCorrectWorldPos();
	}

	//! 보정된 캔버스 로컬 위치를 반환한다
	public static Vector3 ExGetCorrectLocalPos(this PointerEventData a_oSender, GameObject a_oObj) {
		var stPos = a_oSender.ExGetCorrectWorldPos();
		return stPos.ExToLocal(a_oObj);
	}

	//! 보정된 캔버스 월드 비율 위치를 반환한다
	public static Vector3 ExGetCorrectWorldScalePos(this Vector3 a_oSender, Vector3 a_stScale) {
		var stPos = a_oSender.ExGetScaleVector(a_stScale);
		return stPos.ExGetCorrectWorldPos();
	}

	//! 보정된 캔버스 월드 비율 위치를 반환한다
	public static Vector3 ExGetCorrectWorldScalePos(this PointerEventData a_oSender, Vector3 a_stScale) {
		var stPos = a_oSender.ExGetWorldScalePos(a_stScale);
		return stPos.ExGetCorrectWorldPos();
	}

	//! 보정된 캔버스 로컬 비율 위치를 반환한다
	public static Vector3 ExGetCorrectLocalScalePos(this PointerEventData a_oSender, GameObject a_oObj, Vector3 a_stScale) {
		var stPos = a_oSender.ExGetCorrectWorldScalePos(a_stScale);
		return stPos.ExToLocal(a_oObj);
	}

	//! 스크롤 뷰 정규 위치를 반환한다
	public static Vector2 ExGetNormalPos(this ScrollRect a_oSender, Vector3 a_stPos, GameObject a_oViewport, GameObject a_oContent) {
		return new Vector2(a_oSender.ExGetHNormalPos(a_stPos, a_oViewport, a_oContent),
			a_oSender.ExGetVNormalPos(a_stPos, a_oViewport, a_oContent));
	}

	//! 스크롤 뷰 수직 정규 위치를 반환한다
	public static float ExGetVNormalPos(this ScrollRect a_oSender, Vector3 a_stPos, GameObject a_oViewport, GameObject a_oContent) {
		var oTransform = a_oViewport?.transform as RectTransform;
		var oContentTransform = a_oContent?.transform as RectTransform;

		Func.Assert(oTransform != null && oContentTransform != null);

		float fPosY = oContentTransform.rect.height - a_stPos.y;
		return Mathf.Clamp01((fPosY - oTransform.rect.height) / (oContentTransform.rect.height - oTransform.rect.height));
	}

	//! 스크롤 뷰 수평 정규 위치를 반환한다
	public static float ExGetHNormalPos(this ScrollRect a_oSender, Vector3 a_stPos, GameObject a_oViewport, GameObject a_oContent) {
		var oTransform = a_oViewport?.transform as RectTransform;
		var oContentTransform = a_oContent?.transform as RectTransform;

		Func.Assert(oTransform != null && oContentTransform != null);
		return Mathf.Clamp01((a_stPos.x - oTransform.rect.width) / (oContentTransform.rect.width - oTransform.rect.width));
	}

	//! 스크롤 뷰 수직 정규 범위를 반환한다
	public static KeyValuePair<float, float> ExGetVNormalRange(this ScrollRect a_oSender, GameObject a_oViewport, GameObject a_oContent) {
		var oTransform = a_oViewport?.transform as RectTransform;
		var oContentTransform = a_oContent?.transform as RectTransform;

		Func.Assert(oTransform != null && oContentTransform != null);

		float fMaxPosY = oContentTransform.rect.height - oContentTransform.anchoredPosition.y;
		float fMinPosY = oContentTransform.rect.height - (oContentTransform.anchoredPosition.y + oTransform.rect.height);

		return new KeyValuePair<float, float>(Mathf.Clamp01((fMinPosY - oTransform.rect.height) / (oContentTransform.rect.height - oTransform.rect.height)),
			Mathf.Clamp01((fMaxPosY - oTransform.rect.height) / (oContentTransform.rect.height - oTransform.rect.height)));
	}

	//! 스크롤 뷰 수평 정규 범위를 반환한다
	public static KeyValuePair<float, float> ExGetHNormalRange(this ScrollRect a_oSender, GameObject a_oViewport, GameObject a_oContent) {
		var oTransform = a_oViewport?.transform as RectTransform;
		var oContentTransform = a_oContent?.transform as RectTransform;

		Func.Assert(oTransform != null && oContentTransform != null);

		return new KeyValuePair<float, float>(Mathf.Clamp01((oContentTransform.anchoredPosition.x - oTransform.rect.width) / (oContentTransform.rect.width - oTransform.rect.width)),
			Mathf.Clamp01(((oContentTransform.anchoredPosition.x + oTransform.rect.width) - oTransform.rect.width) / (oContentTransform.rect.width - oTransform.rect.width)));
	}

	//! 자식을 반환한다
	public static List<GameObject> ExGetChildren(this Scene a_stSender) {
		var oObjs = a_stSender.GetRootGameObjects();
		var oObjList = new List<GameObject>();

		for(int i = 0; i < oObjs?.Length; ++i) {
			var oChildObjList = oObjs[i].ExGetChildren();
			oObjList.AddRange(oChildObjList);
		}

		return oObjList;
	}

	//! 자식을 반환한다
	public static List<GameObject> ExGetChildren(this GameObject a_oSender, bool a_bIsIncludeSelf = true) {
		var oObjList = new List<GameObject>();
		var oEnumerator = a_bIsIncludeSelf ? a_oSender.DescendantsAndSelf() : a_oSender.Descendants();

		foreach(var oObj in oEnumerator) {
			oObjList.Add(oObj);
		}

		return oObjList;
	}

	//! 부모를 반환한다
	public static List<GameObject> ExGetParents(this GameObject a_oSender, bool a_bIsIncludeSelf = true) {
		var oObjList = new List<GameObject>();
		var oEnumerator = a_bIsIncludeSelf ? a_oSender.AncestorsAndSelf() : a_oSender.Ancestors();

		foreach(var oObj in oEnumerator) {
			oObjList.Add(oObj);
		}

		return oObjList;
	}

	//! 활성화 여부를 변경한다
	public static void ExSetEnable(this LayoutGroup a_oSender, bool a_bIsEnable) {
		Func.Assert(a_oSender != null);

		a_oSender.enabled = false;
		a_oSender.gameObject.ExSetEnableComponent<ContentSizeFitter>(a_bIsEnable);
	}

	//! 상호 작용 여부를 변경한다
	public static void ExSetInteractable(this Button a_oSender, bool a_bIsEnable) {
		Func.Assert(a_oSender != null);
		
		a_oSender.interactable = a_bIsEnable;
		a_oSender.gameObject.ExSetEnableComponent<CTouchFader>(a_bIsEnable);
		a_oSender.gameObject.ExSetEnableComponent<CTouchScaler>(a_bIsEnable);
		a_oSender.gameObject.ExSetEnableComponent<CTouchSndPlayer>(a_bIsEnable);
	}

	//! 스프라이트를 변경한다
	public static void ExSetSprite(this Button a_oSender, Sprite a_oSprite) {
		var oImg = a_oSender?.GetComponentInChildren<Image>();
		Func.Assert(oImg != null);
		
		oImg.sprite = a_oSprite;
	}

	//! 컬링 마스크를 변경한다
	public static void ExSetCullingMask(this Camera a_oSender, List<int> a_oLayerList, bool a_bIsResetCullingMask = true) {
		Func.Assert(a_oSender != null);
		a_oSender.cullingMask = a_bIsResetCullingMask ? 0 : a_oSender.cullingMask;

		if(a_oLayerList != null) {
			a_oSender.cullingMask |= a_oLayerList.ExToBits();
		}
	}

	//! 컬링 마스크를 변경한다
	public static void ExSetCullingMask(this Light a_oSender, List<int> a_oLayerList, bool a_bIsResetCullingMask = true) {
		Func.Assert(a_oSender != null);
		a_oSender.cullingMask = a_bIsResetCullingMask ? 0 : a_oSender.cullingMask;

		for(int i = 0; i < a_oLayerList?.Count; ++i) {
			a_oSender.cullingMask |= a_oLayerList.ExToBits();
		}
	}

	//! 이벤트 마스크를 변경한다
	public static void ExSetEventMask(this PhysicsRaycaster a_oSender, List<int> a_oLayerList, bool a_bIsResetEventMask = true) {
		Func.Assert(a_oSender != null);

		var stLayerMask = a_oSender.eventMask;
		stLayerMask.value = a_bIsResetEventMask ? 0 : a_oSender.eventMask.value;

		if(a_oLayerList != null) {
			stLayerMask.value |= a_oLayerList.ExToBits();
		}

		a_oSender.eventMask = stLayerMask;
	}

	//! 캔버스 정렬 순서를 변경한다
	public static void ExSetSortingOrder(this Canvas a_oSender, string a_oSortingLayer, int a_nSortingOrder) {
		Func.Assert(a_oSender != null && a_oSortingLayer.ExIsValid());

		a_oSender.sortingLayerName = a_oSortingLayer;
		a_oSender.sortingOrder = a_nSortingOrder;
	}

	//! 2 차원 카메라를 설정한다
	public static void ExSetup2D(this Camera a_oSender, float a_fPlaneHeight) {
		Func.Assert(a_oSender != null);

		a_oSender.orthographic = true;
		a_oSender.orthographicSize = a_fPlaneHeight * 0.5f;
	}

	//! 3 차원 카메라를 설정한다
	public static void ExSetup3D(this Camera a_oSender, float a_fPlaneHeight, float a_fPlaneDistance) {
		Func.Assert(a_oSender != null && a_fPlaneDistance.ExIsGreate(0.0f));
		float fFieldOfView = Mathf.Atan((a_fPlaneHeight * 0.5f) / a_fPlaneDistance);

		a_oSender.orthographic = false;
		a_oSender.fieldOfView = (fFieldOfView * 2.0f) * Mathf.Rad2Deg;
	}

	//! 파티클을 재생한다
	public static void ExPlay(this ParticleSystem a_oSender, bool a_bIsReset = true, bool a_bIsRemoveChildren = false) {
		Func.Assert(a_oSender != null);

		if(a_bIsReset) {
			a_oSender.Stop(a_bIsRemoveChildren);
		}

		a_oSender.Play();
	}

	//! 자식 객체를 탐색한다
	public static GameObject ExFindChild(this Scene a_stSender, string a_oName) {
		var oObjs = a_stSender.GetRootGameObjects();

		for(int i = 0; i < oObjs?.Length; ++i) {
			var oObj = oObjs[i].ExFindChild(a_oName);

			if(oObj != null) {
				return oObj;
			}
		}

		return null;
	}

	//! 자식 객체를 탐색한다
	public static GameObject ExFindChild(this GameObject a_oSender, string a_oName, bool a_bIsIncludeSelf = true) {
		Func.Assert(a_oSender != null && a_oName.ExIsValid());
		var oEnumerator = a_bIsIncludeSelf ? a_oSender.DescendantsAndSelf() : a_oSender.Descendants();

		foreach(var oObj in oEnumerator) {
			if(oObj.name.ExIsEquals(a_oName)) {
				return oObj;
			}
		}

		return null;
	}

	//! 자식 객체를 탐색한다
	public static List<GameObject> ExFindChildren(this Scene a_stSender, string a_oName) {
		var oObjs = a_stSender.GetRootGameObjects();
		var oObjList = new List<GameObject>();

		for(int i = 0; i < oObjs?.Length; ++i) {
			var oChildObjectList = oObjs[i].ExFindChildren(a_oName);

			if(oChildObjectList != null) {
				oObjList.AddRange(oChildObjectList);
			}
		}

		return oObjList;
	}

	//! 자식 객체를 탐색한다
	public static List<GameObject> ExFindChildren(this GameObject a_oSender, string a_oName, bool a_bIsIncludeSelf = true) {
		Func.Assert(a_oSender != null && a_oName.ExIsValid());

		var oObjList = new List<GameObject>();
		var oEnumerator = a_bIsIncludeSelf ? a_oSender.DescendantsAndSelf() : a_oSender.Descendants();

		foreach(var oObj in oEnumerator) {
			if(oObj.name.ExIsEquals(a_oName)) {
				oObjList.Add(oObj);
			}
		}

		return oObjList;
	}

	//! 부모 객체를 탐색한다
	public static GameObject ExFindParent(this GameObject a_oSender, string a_oName, bool a_bIsIncludeSelf = true) {
		Func.Assert(a_oSender != null && a_oName.ExIsValid());
		var oEnumerator = a_bIsIncludeSelf ? a_oSender.AncestorsAndSelf() : a_oSender.Ancestors();

		foreach(var oObj in oEnumerator) {
			if(oObj.name.ExIsEquals(a_oName)) {
				return oObj;
			}
		}

		return null;
	}

	//! 부모 객체를 탐색한다
	public static List<GameObject> ExFindParents(this GameObject a_oSender, string a_oName, bool a_bIsIncludeSelf = true) {
		Func.Assert(a_oSender != null && a_oName.ExIsValid());

		var oObjList = new List<GameObject>();
		var oEnumerator = a_bIsIncludeSelf ? a_oSender.AncestorsAndSelf() : a_oSender.Ancestors();

		foreach(var oObj in oEnumerator) {
			if(oObj.name.ExIsEquals(a_oName)) {
				oObjList.Add(oObj);
			}
		}

		return oObjList;
	}

	//! 메세지를 전송한다
	public static void ExSendMsg(this Scene a_stSender, string a_oName, string a_oMsg, object a_oParams) {
		var oObj = a_stSender.ExFindChild(a_oName);
		oObj?.SendMessage(a_oMsg, a_oParams, SendMessageOptions.DontRequireReceiver);
	}

	//! 메세지를 전송한다
	public static void ExSendMsg(this GameObject a_oSender, string a_oName, string a_oMsg, object a_oParams) {
		var oObj = a_oSender.ExFindChild(a_oName);
		oObj?.SendMessage(a_oMsg, a_oParams, SendMessageOptions.DontRequireReceiver);
	}

	//! 메세지를 전파한다
	public static void ExBroadcastMsg(this Scene a_stSender, string a_oMsg, object a_oParams) {
		var oObjs = a_stSender.GetRootGameObjects();

		for(int i = 0; i < oObjs?.Length; ++i) {
			oObjs[i].ExBroadcastMsg(a_oMsg, a_oParams);
		}
	}

	//! 메세지를 전파한다
	public static void ExBroadcastMsg(this GameObject a_oSender, string a_oMsg, object a_oParams) {
		Func.Assert(a_oSender != null && a_oMsg.ExIsValid());
		a_oSender.BroadcastMessage(a_oMsg, a_oParams, SendMessageOptions.DontRequireReceiver);
	}

	//! 로컬 -> 월드로 변환한다
	public static Vector3 ExToWorld(this Vector3 a_stSender, GameObject a_oObj, bool a_bIsCoordinate = true) {
		Func.Assert(a_oObj != null);
		var stVector = new Vector4(a_stSender.x, a_stSender.y, a_stSender.z, a_bIsCoordinate ? 1.0f : 0.0f);

		return a_oObj.transform.localToWorldMatrix * stVector;
	}

	//! 월드 -> 로컬로 변환한다
	public static Vector3 ExToLocal(this Vector3 a_stSender, GameObject a_oObj, bool a_bIsCoordinate = true) {
		Func.Assert(a_oObj != null);
		var stVector = new Vector4(a_stSender.x, a_stSender.y, a_stSender.z, a_bIsCoordinate ? 1.0f : 0.0f);

		return a_oObj.transform.worldToLocalMatrix * stVector;
	}
	#endregion			// 클래스 함수

	#region 제네릭 클래스 함수
	//! 컴포넌트 활성 여부를 변경한다
	public static void ExSetEnableComponent<T>(this GameObject a_oSender, bool a_bIsEnable) where T : Component {
		Func.Assert(a_oSender != null);
		var oComponent = a_oSender.GetComponentInChildren<T>() as MonoBehaviour;

		if(oComponent != null) {
			oComponent.enabled = a_bIsEnable;
		}
	}

	//! 컴포넌트 활성 여부를 변경한다
	public static void ExSetEnableComponent<T>(this Scene a_stSender, string a_oName, bool a_bIsEnable) where T : Component {
		var oObj = a_stSender.ExFindChild(a_oName);
		oObj.ExSetEnableComponent<T>(a_bIsEnable);
	}

	//! 컴포넌트 활성 여부를 변경한다
	public static void ExSetEnableComponent<T>(this GameObject a_oSender, string a_oName, bool a_bIsEnable, bool a_bIsIncludeSelf = true) where T : Component {
		var oObj = a_oSender.ExFindChild(a_oName, a_bIsIncludeSelf);
		oObj.ExSetEnableComponent<T>(a_bIsEnable);
	}

	//! 컴포넌트를 추가한다
	public static T ExAddComponent<T>(this GameObject a_oSender) where T : Component {
		Func.Assert(a_oSender != null);
		var oComponent = a_oSender.GetComponent<T>();

		return (oComponent != null) ? oComponent : a_oSender.AddComponent<T>();
	}

	//! 컴포넌트를 제거한다
	public static void ExRemoveComponent<T>(this GameObject a_oSender, bool a_bIsFindChildren = false) where T : Component {
		Func.Assert(a_oSender != null);
		var oComponent = a_bIsFindChildren ? a_oSender.GetComponentInChildren<T>() : a_oSender.GetComponent<T>();

		if(oComponent != null) {
			if(Application.isPlaying) {
				GameObject.Destroy(oComponent);
			} else {
				GameObject.DestroyImmediate(oComponent);
			}
		}
	}

	//! 컴포넌트를 탐색한다
	public static T ExFindComponent<T>(this Scene a_stSender, string a_oName) where T : Component {
		var oObj = a_stSender.ExFindChild(a_oName);
		return oObj?.GetComponentInChildren<T>();
	}

	//! 컴포넌트를 탐색한다
	public static T ExFindComponent<T>(this GameObject a_oSender, string a_oName, bool a_bIsIncludeSelf = true) where T : Component {
		var oObj = a_oSender.ExFindChild(a_oName, a_bIsIncludeSelf);
		return oObj?.GetComponentInChildren<T>();
	}

	//! 컴포넌트를 탐색한다
	public static T[] ExFindComponents<T>(this Scene a_stSender, string a_oName) where T : Component {
		var oObj = a_stSender.ExFindChild(a_oName);
		return oObj?.GetComponentsInChildren<T>();
	}

	//! 컴포넌트를 탐색한다
	public static T[] ExFindComponents<T>(this GameObject a_oSender, string a_oName, bool a_bIsIncludeSelf = true) where T : Component {
		var oObj = a_oSender.ExFindChild(a_oName, a_bIsIncludeSelf);
		return oObj?.GetComponentsInChildren<T>();
	}

	//! 부모 컴포넌트를 탐색한다
	public static T ExFindComponentInParent<T>(this GameObject a_oSender, string a_oName, bool a_bIsIncludeSelf = true) where T : Component {
		var oObj = a_oSender.ExFindParent(a_oName, a_bIsIncludeSelf);
		return oObj?.GetComponentInParent<T>();
	}

	//! 부모 컴포넌트를 탐색한다
	public static T[] ExFindComponentsInParent<T>(this GameObject a_oSender, string a_oName, bool a_bIsIncludeSelf = true) where T : Component {
		var oObj = a_oSender.ExFindParent(a_oName, a_bIsIncludeSelf);
		return oObj?.GetComponentsInParent<T>();
	}
	#endregion			// 제네릭 클래스 함수

	#region 조건부 클래스 함수
#if UNITY_EDITOR
	//! 스크립트 순서를 변경한다
	public static void ExSetScriptOrder(this MonoBehaviour a_oSender, int a_nOrder) {
		Func.Assert(a_oSender != null && (a_nOrder >= short.MinValue && a_nOrder <= short.MaxValue));
		
		var oMonoScript = MonoScript.FromMonoBehaviour(a_oSender);
		int nCurrentOrder = MonoImporter.GetExecutionOrder(oMonoScript);

		if(nCurrentOrder != a_nOrder) {
			MonoImporter.SetExecutionOrder(oMonoScript, a_nOrder);
		}
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 클래스 함수
}
