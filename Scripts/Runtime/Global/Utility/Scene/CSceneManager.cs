using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
using UnityEngine.Profiling;
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FILE_BROWSER_ENABLE
using SimpleFileBrowser;
#endif			// #if FILE_BROWSER_ENABLE

//! 씬 관리자
public abstract partial class CSceneManager : CComponent {
	#region 변수
	private Dictionary<string, ObjectPool> m_oObjPoolList = new Dictionary<string, ObjectPool>();
	#endregion			// 변수

	#region 클래스 변수
	private static float m_fGCSkipTime = 0.0f;
	private static Dictionary<string, KeyValuePair<GameObject, Sequence>> m_oTouchResponderInfoList = new Dictionary<string, KeyValuePair<GameObject, Sequence>>();

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	private static float m_fDebugSkipTime = 0.0f;

	private static System.Text.StringBuilder m_oStaticDebugStringBuilder = new System.Text.StringBuilder();
	private static System.Text.StringBuilder m_oDynamicDebugStringBuilder = new System.Text.StringBuilder();

	private static System.Text.StringBuilder m_oExtraStaticDebugStringBuilder = new System.Text.StringBuilder();
	private static System.Text.StringBuilder m_oExtraDynamicDebugStringBuilder = new System.Text.StringBuilder();
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 클래스 변수

	#region 클래스 컴포넌트
	private static Dictionary<string, CSceneManager> m_oSubSceneManagerList = new Dictionary<string, CSceneManager>();
	#endregion			// 클래스 컴포넌트

	#region 프로퍼티
	public abstract string SceneName { get; }

	// 카메라
	public Camera SubUICamera { get; private set; } = null;
	public Camera SubMainCamera { get; private set; } = null;

	// 캔버스
	public Canvas SubUICanvas { get; private set; } = null;
	public Canvas SubObjectCanvas { get; private set; } = null;

	// UI 루트
	public GameObject SubUITop { get; private set; } = null;
	public GameObject SubUIBase { get; private set; } = null;
	public GameObject SubUIRoot { get; private set; } = null;
	public GameObject SubFixUIRoot { get; private set; } = null;

	// 고정 UI 루트
	public GameObject SubLeftUIRoot { get; private set; } = null;
	public GameObject SubRightUIRoot { get; private set; } = null;
	public GameObject SubTopUIRoot { get; private set; } = null;
	public GameObject SubBottomUIRoot { get; private set; } = null;

	// 팝업 UI 루트
	public GameObject SubPopupUIRoot { get; private set; } = null;
	public GameObject SubTopmostUIRoot { get; private set; } = null;

	// 객체 루트
	public GameObject SubBase { get; private set; } = null;
	public GameObject SubObjBase { get; private set; } = null;
	public GameObject SubObjRoot { get; private set; } = null;
	public GameObject SubFixObjRoot { get; private set; } = null;

	// 고정 객체 루트
	public GameObject SubLeftObjRoot { get; private set; } = null;
	public GameObject SubRightObjRoot { get; private set; } = null;
	public GameObject SubTopObjRoot { get; private set; } = null;
	public GameObject SubBottomObjRoot { get; private set; } = null;

	// 객체 캔버스 루트
	public GameObject SubObjectCanvasTop { get; private set; } = null;
	public GameObject SubObjectCanvasBase { get; private set; } = null;
	public GameObject SubObjectCanvasRoot { get; private set; } = null;
	
	public bool IsRootScene => CSceneManager.RootSceneName.ExIsEquals(this.SceneName);
	public virtual float PlaneDistance => KDefine.U_DEF_DISTANCE_CAMERA_PLANE;

	public virtual float UICameraDepth => KDefine.U_DEPTH_UI_CAMERA;
	public virtual float MainCameraDepth => KDefine.U_DEPTH_MAIN_CAMERA;

	public virtual Color ClearColor => KAppDefine.G_DEF_COLOR_CAMERA_BG;

	public virtual KeyValuePair<string, int> UICanvasSortingOrderInfo => KAppDefine.G_SORTING_ORDER_INFO_UI_CANVAS;
	public virtual KeyValuePair<string, int> ObjCanvasSortingOrderInfo => KAppDefine.G_SORTING_ORDER_INFO_OBJ_CANVAS;
	
#if UNITY_EDITOR
	public virtual int ScriptOrder => KDefine.U_SCRIPT_ORDER_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 클래스 프로퍼티
	public static bool IsInit { get; set; } = false;
	public static bool IsSetup { get; set; } = false;
	public static bool IsAutoGC { get; set; } = false;

	public static bool IsAwake { get; private set; } = false;
	public static bool IsAppQuit { get; private set; } = false;

	// 고정 UI 간격
	public static float LeftUIOffset { get; private set; } = 0.0f;
	public static float RightUIOffset { get; private set; } = 0.0f;
	public static float TopUIOffset { get; private set; } = 0.0f;
	public static float BottomUIOffset { get; private set; } = 0.0f;

	// 고정 객체 간격
	public static float LeftObjectOffset { get; private set; } = 0.0f;
	public static float RightObjectOffset { get; private set; } = 0.0f;
	public static float TopObjectOffset { get; private set; } = 0.0f;
	public static float BottomObjectOffset { get; private set; } = 0.0f;

	// 고정 UI 루트 간격
	public static float LeftUIRootOffset { get; private set; } = 0.0f;
	public static float RightUIRootOffset { get; private set; } = 0.0f;
	public static float TopUIRootOffset { get; private set; } = 0.0f;
	public static float BottomUIRootOffset { get; private set; } = 0.0f;

	// 고정 객체 루트 간격
	public static float LeftObjectRootOffset { get; private set; } = 0.0f;
	public static float RightObjectRootOffset { get; private set; } = 0.0f;
	public static float TopObjectRootOffset { get; private set; } = 0.0f;
	public static float BottomObjectRootOffset { get; private set; } = 0.0f;

	// 캔버스 크기
	public static Vector2 CanvasSize { get; protected set; } = Vector2.zero;
	public static Vector3 CanvasScale { get; protected set; } = Vector3.zero;

	// 카메라
	public static Camera UICamera { get; private set; } = null;
	public static Camera MainCamera { get; private set; } = null;

	// UI 루트
	public static GameObject UITop { get; private set; } = null;
	public static GameObject UIBase { get; private set; } = null;
	public static GameObject UIRoot { get; private set; } = null;
	public static GameObject FixUIRoot { get; private set; } = null;

	// 고정 UI 루트
	public static GameObject LeftUIRoot { get; private set; } = null;
	public static GameObject RightUIRoot { get; private set; } = null;
	public static GameObject TopUIRoot { get; private set; } = null;
	public static GameObject BottomUIRoot { get; private set; } = null;

	// 팝업 UI 루트
	public static GameObject PopupUIRoot { get; private set; } = null;
	public static GameObject TopmostUIRoot { get; private set; } = null;

	// 객체 루트
	public static GameObject Base { get; private set; } = null;
	public static GameObject ObjectBase { get; private set; } = null;
	public static GameObject ObjectRoot { get; private set; } = null;
	public static GameObject FixObjectRoot { get; private set; } = null;

	// 고정 객체 루트
	public static GameObject LeftObjectRoot { get; private set; } = null;
	public static GameObject RightObjectRoot { get; private set; } = null;
	public static GameObject TopObjectRoot { get; private set; } = null;
	public static GameObject BottomObjectRoot { get; private set; } = null;

	// 객체 캔버스 루트
	public static GameObject ObjectCanvasTop { get; private set; } = null;
	public static GameObject ObjectCanvasBase { get; private set; } = null;
	public static GameObject ObjectCanvasRoot { get; private set; } = null;

	// 화면 UI 루트
	public static GameObject ScreenBlindUIRoot { get; protected set; } = null;
	public static GameObject ScreenPopupUIRoot { get; protected set; } = null;
	public static GameObject ScreenTopmostUIRoot { get; protected set; } = null;
	public static GameObject ScreenAbsoluteUIRoot { get; protected set; } = null;

	public static Canvas UICanvas { get; private set; } = null;
	public static Canvas ObjectCanvas { get; private set; } = null;
	public static CSceneManager RootSceneManager { get; private set; } = null;

	public static string AwakeSceneName { get; private set; } = string.Empty;
	public static string RootSceneName => SceneManager.GetActiveScene().name;

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	public static Text ScreenStaticDebugText { get; protected set; } = null;
	public static Text ScreenDynamicDebugText { get; protected set; } = null;

	public static Button ScreenFPSBtn { get; protected set; } = null;
	public static Button ScreenDebugBtn { get; protected set; } = null;

	public static GameObject ScreenDebugUIRoot { get; protected set; } = null;
	public static GameObject ScreenDebugTextRoot { get; protected set; } = null;
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	public static Text ScreenStaticFPSText { get; protected set; } = null;
	public static Text ScreenDynamicFPSText { get; protected set; } = null;
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	#endregion			// 클래스 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		CSceneManager.IsAutoGC = true;
		this.IsIgnoreNavigationEvent = true;

		// 처음 씬을 생성했을 경우
		if(!CSceneManager.IsAwake) {
#if DEBUG || DEVELOPMENT_BUILD
			Func.ShowLog("Platform: {0}", KDefine.B_LOG_COLOR_PLATFORM_INFO, Application.platform);
			Func.ShowLog("Data Path: {0}", KDefine.B_LOG_COLOR_PLATFORM_INFO, Application.dataPath);
			Func.ShowLog("Persistent Data Path: {0}", KDefine.B_LOG_COLOR_PLATFORM_INFO, Application.persistentDataPath);
#endif			// #if DEBUG || DEVELOPMENT_BUILD

			CSceneManager.IsInit = false;
			CSceneManager.IsSetup = false;

			CSceneManager.IsAwake = true;
			CSceneManager.AwakeSceneName = CSceneManager.RootSceneName;

			// 초기화 씬이 아닐 경우
			if(!CSceneManager.AwakeSceneName.ExIsEquals(KDefine.B_SCENE_NAME_INIT)) {
				CSceneLoader.Instance.LoadScene(KDefine.B_SCENE_NAME_INIT, false, false);
			}
		}

		this.SetupScene();
		
#if UNITY_EDITOR
		if(this.IsRootScene) {
			Func.SelectObj(this.gameObject, true);
		}
#endif			// #if UNITY_EDITOR
	}

	//! 초기화
	public override void Start() {
		base.Start();
		CActivityIndicatorManager.Instance.StopActivityIndicator();

#if UNIVERSAL_RENDER_PIPELINE_ENABLE && CAMERA_STACK_ENABLE
		CSceneManager.SortCameraStack();
#endif			// #if UNIVERSAL_RENDER_PIPELINE_ENABLE && CAMERA_STACK_ENABLE
	}

	//! 상태를 갱신한다
	public override void OnUpdate(float a_fDeltaTime) {
#if !ROBO_TEST_ENABLE
		if(this.IsRootScene) {
			if(CSceneManager.IsAutoGC) {
				CSceneManager.m_fGCSkipTime += CScheduleManager.Instance.UnscaleDeltaTime;

				if(CSceneManager.m_fGCSkipTime >= KDefine.U_DELTA_TIME_GC) {
					CSceneManager.m_fGCSkipTime = 0.0f;
					System.GC.Collect();
				}
			}

			if(Input.GetKeyDown(KeyCode.Escape)) {
				CSndManager.Instance.PlayFXSnd(KDefine.U_SND_PATH_G_TOUCH_ENDED);
				CNavigationManager.Instance.SendNavigationEvent(ENavigationEventType.BACK_KEY_DOWN);
			}

#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
			if(CSceneManager.ScreenDebugUIRoot != null) {
				if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1)) {
					CSceneManager.OnTouchDebugBtn();
				}

				if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2)) {
					CSceneManager.m_oExtraDynamicDebugStringBuilder.Clear();
				}
			}

			CSceneManager.m_fDebugSkipTime += CScheduleManager.Instance.UnscaleDeltaTime;

			if(CSceneManager.m_fDebugSkipTime >= KDefine.U_DELTA_TIME_DYNAMIC_DEBUG) {
				CSceneManager.m_fDebugSkipTime = 0.0f;

				if(CSceneManager.ScreenStaticDebugText != null) {
					CSceneManager.ScreenStaticDebugText.text = string.Format(KDefine.U_FORMAT_SCENE_M_STATIC_DEBUG_MSG, 
						CSceneManager.m_oStaticDebugStringBuilder.ToString(), CSceneManager.m_oExtraStaticDebugStringBuilder.ToString());
				}

				if(CSceneManager.ScreenDynamicDebugText != null) {
					double dblGCMemory = System.GC.GetTotalMemory(false).ExToMegaByte();
					double dblGPUMemory = Profiler.GetAllocatedMemoryForGraphicsDriver().ExToMegaByte();
					double dblUsedHeapMemory = Profiler.usedHeapSizeLong.ExToMegaByte();

					double dblMonoHeapMemory = Profiler.GetMonoHeapSizeLong().ExToMegaByte();
					double dblMonoUsedMemory = Profiler.GetMonoUsedSizeLong().ExToMegaByte();

					double dblTempAllocMemory = Profiler.GetTempAllocatorSize().ExToMegaByte();
					double dblTotalAllocMemory = Profiler.GetTotalAllocatedMemoryLong().ExToMegaByte();

					double dblTotalReservedMemory = Profiler.GetTotalReservedMemoryLong().ExToMegaByte();
					double dblTotalUnusedReservedMemory = Profiler.GetTotalUnusedReservedMemoryLong().ExToMegaByte();

					double dblGPUAllocMemory = Profiler.GetAllocatedMemoryForGraphicsDriver().ExToMegaByte();

					CSceneManager.m_oDynamicDebugStringBuilder.Clear();
					CSceneManager.m_oDynamicDebugStringBuilder.AppendFormat(KDefine.U_FORMAT_SCENE_M_DYNAMIC_DEBUG_INFO_A, dblGCMemory, dblUsedHeapMemory);
					CSceneManager.m_oDynamicDebugStringBuilder.AppendFormat(KDefine.U_FORMAT_SCENE_M_DYNAMIC_DEBUG_INFO_B, dblMonoHeapMemory, dblMonoUsedMemory);
					CSceneManager.m_oDynamicDebugStringBuilder.AppendFormat(KDefine.U_FORMAT_SCENE_M_DYNAMIC_DEBUG_INFO_C, dblTempAllocMemory, dblTotalAllocMemory);
					CSceneManager.m_oDynamicDebugStringBuilder.AppendFormat(KDefine.U_FORMAT_SCENE_M_DYNAMIC_DEBUG_INFO_D, dblTotalReservedMemory, dblTotalUnusedReservedMemory);
					CSceneManager.m_oDynamicDebugStringBuilder.AppendFormat(KDefine.U_FORMAT_SCENE_M_DYNAMIC_DEBUG_INFO_E, dblGPUAllocMemory);

					CSceneManager.ScreenDynamicDebugText.text = string.Format(KDefine.U_FORMAT_SCENE_M_DYNAMIC_DEBUG_MSG, 
						CSceneManager.m_oDynamicDebugStringBuilder.ToString(), CSceneManager.m_oExtraDynamicDebugStringBuilder.ToString());
				}
			}
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		}
#endif			// #if !ROBO_TEST_ENABLE
	}

	//! 제거 되었을 경우
	public override void OnDestroy() {
		base.OnDestroy();

		if(!CSceneManager.IsAppQuit) {
			System.GC.Collect();
			Resources.UnloadUnusedAssets();

			if(this.IsRootScene) {
				CScheduleManager.Instance.RemoveComponent(this);
				CNavigationManager.Instance.RemoveComponent(this);
			}

			CSceneManager.m_oSubSceneManagerList.ExRemoveValue(this.SceneName);
		}
	}

	//! 어플리메이션이 종료 되었을 경우
	public virtual void OnApplicationQuit() {
		DOTween.KillAll(true);
		CSceneManager.IsAppQuit = true;
	}

	//! 객체 풀을 반환한다
	public ObjectPool GetObjPool(string a_oKey) {
		Func.Assert(a_oKey.ExIsValid());
		return m_oObjPoolList.ExGetValue(a_oKey, null);
	}

	//! 객체 풀을 추가한다
	public void AddObjPool(string a_oKey, ObjectPool a_oObjPool) {
		Func.Assert(a_oObjPool != null && a_oKey.ExIsValid());
		m_oObjPoolList.ExAddValue(a_oKey, a_oObjPool);
	}

	//! 객체 풀을 제거한다
	public void RemoveObjPool(string a_oKey, bool a_bIsDestroy = true) {
		Func.Assert(a_oKey.ExIsValid());

		if(m_oObjPoolList.ContainsKey(a_oKey)) {
			var oObjPool = m_oObjPoolList.ExGetValue(a_oKey, null);
			oObjPool.DeAllocate(oObjPool.CountInactive);
			oObjPool.DespawnAllActiveObjects(a_bIsDestroy);

			m_oObjPoolList.Remove(a_oKey);
		}
	}

	//! 객체를 활성화한다
	public GameObject SpawnObj(string a_oKey, 
		Vector3 a_stPos, Vector3 a_stScale, Vector3 a_stRotation, string a_oName = KDefine.B_EMPTY_STRING, bool a_bIsWorld = false) {
		Func.Assert(a_oKey.ExIsValid() && m_oObjPoolList.ContainsKey(a_oKey));
		
		var oObj = m_oObjPoolList[a_oKey].Spawn();
		oObj.transform.localScale = a_stScale;

		if(a_oName.ExIsValid()) {
			oObj.name = a_oName;
		}

		if(a_bIsWorld) {
			oObj.transform.position = a_stPos;
			oObj.transform.eulerAngles = a_stRotation;
		} else {
			oObj.transform.localPosition = a_stPos;
			oObj.transform.localEulerAngles = a_stRotation;
		}

		return oObj;
	}

	//! 객체를 비활성화한다
	public void DespawnObj(string a_oKey, GameObject a_oObj, bool a_bIsDestroy = false) {
		Func.Assert(a_oKey.ExIsValid() && m_oObjPoolList.ContainsKey(a_oKey));

		if(m_oObjPoolList.ContainsKey(a_oKey)) {
			m_oObjPoolList[a_oKey].Despawn(a_oObj, a_bIsDestroy);
		}
	}

	//! 화면 페이드 인 애니메이션을 시작한다
	public virtual void StartScreenFadeInAnimation(System.Action<GameObject> a_oCallback) {
		CSceneManager.ShowTouchResponder(KDefine.U_OBJ_NAME_SCREEN_F_TOUCH_RESPONDER,
			CSceneManager.ScreenAbsoluteUIRoot, KDefine.U_DEF_COLOR_SCREEN_FADE, a_oCallback, true, false, KDefine.U_DEF_DURATION_SCREEN_FADE_OUT_ANIMATION);
	}

	//! 화면 페이드 아웃 애니메이션을 시작한다
	public virtual void StartScreenFadeOutAnimation() {
		CSceneManager.CloseTouchResponder(KDefine.U_OBJ_NAME_SCREEN_F_TOUCH_RESPONDER,
			KDefine.U_DEF_COLOR_TRANSPARENT, true, KDefine.U_DEF_DURATION_SCREEN_FADE_OUT_ANIMATION);
	}
	#endregion			// 함수

	#region 클래스 함수
	//! 터치 응답자를 반환한다
	public static GameObject GetTouchResponder(string a_oKey) {
		Func.Assert(a_oKey.ExIsValid());
		return CSceneManager.m_oTouchResponderInfoList.ContainsKey(a_oKey) ? CSceneManager.m_oTouchResponderInfoList[a_oKey].Key : null;
	}

	//! 터치 응답자를 출력한다
	public static void ShowTouchResponder(string a_oKey,
		GameObject a_oParent, Color a_stColor, System.Action<GameObject> a_oCallback, bool a_bIsAnimation = true, bool a_bIsEnableNavigation = false, float a_fDuration = 0.0f) {
		Func.Assert(a_oKey.ExIsValid());

		if(!CSceneManager.m_oTouchResponderInfoList.ContainsKey(a_oKey)) {
			var oTouchResponder = Func.CreateTouchResponder(string.Format(KDefine.U_KEY_FORMAT_SCENE_M_TOUCH_RESPONDER, a_oKey),
				CResManager.Instance.GetPrefab(KDefine.U_OBJ_PATH_TOUCH_RESPONDER),
				a_oParent,
				CSceneManager.CanvasSize,
				KDefine.B_POS_MIDDLE_CENTER,
				KDefine.U_DEF_COLOR_TRANSPARENT);

			// 배경색을 설정한다 {
			var oImg = oTouchResponder.GetComponentInChildren<Image>();
			Sequence oSequence = null;

			if(!a_bIsAnimation) {
				oImg.color = a_stColor;
				a_oCallback?.Invoke(oTouchResponder);
			} else {
				a_fDuration = a_fDuration.ExIsLessEquals(0.0f) ? KDefine.U_DEF_DURATION_ANIMATION 
					: a_fDuration;

				oSequence = DOTween.Sequence().SetAutoKill().SetEase(Ease.Linear).SetUpdate(true);
				oSequence.Append(oImg.DOColor(a_stColor, a_fDuration));

				oSequence.AppendCallback(() => {
					oSequence.Kill();
					a_oCallback?.Invoke(oTouchResponder);
				});
			}
			// 배경색을 설정한다 }

			// 터치 전달자를 설정한다 {
			var oTouchDispatcher = oTouchResponder.GetComponentInChildren<CTouchDispatcher>();

			oTouchDispatcher.DestroyCallback = (a_oSender) => {
				CSceneManager.m_oTouchResponderInfoList.Remove(a_oKey);
			};

			if(a_bIsEnableNavigation) {
				CNavigationManager.Instance.AddComponent(oTouchDispatcher);
			}
			// 터치 전달자를 설정한다 }

			oTouchDispatcher.IsIgnoreNavigationEvent = true;
			CSceneManager.m_oTouchResponderInfoList.Add(a_oKey, new KeyValuePair<GameObject, Sequence>(oTouchResponder, oSequence));
		}
	}

	//! 터치 응답자를 닫는다
	public static void CloseTouchResponder(string a_oKey, Color a_stColor, bool a_bIsAnimation = false, float a_fDuration = 0.0f) {
		Func.Assert(a_oKey.ExIsValid());

		if(CSceneManager.m_oTouchResponderInfoList.ContainsKey(a_oKey)) {
			var oTouchResponderInfo = CSceneManager.m_oTouchResponderInfoList[a_oKey];
			CSceneManager.m_oTouchResponderInfoList.Remove(a_oKey);

			var oTouchDispatcher = oTouchResponderInfo.Key.GetComponentInChildren<CTouchDispatcher>();
			oTouchDispatcher.DestroyCallback = null;

			// 내비게이션 관리자에 추가되어있을 경우
			if(oTouchDispatcher.NavigationCallback != null) {
				CNavigationManager.Instance.RemoveComponent(oTouchDispatcher);
			}

			// 배경색을 설정한다 {
			var oImg = oTouchResponderInfo.Key.GetComponentInChildren<Image>();
			oTouchResponderInfo.Value?.Kill();

			if(!a_bIsAnimation) {
				Destroy(oTouchResponderInfo.Key);
			} else {
				a_fDuration = a_fDuration.ExIsLessEquals(0.0f) ? KDefine.U_DEF_DURATION_ANIMATION 
					: a_fDuration;

				var oSequence = DOTween.Sequence().SetAutoKill().SetEase(Ease.Linear).SetUpdate(true);
				oSequence.Append(oImg.DOColor(a_stColor, a_fDuration));

				oSequence.AppendCallback(() => {
					oSequence.Kill();
					Destroy(oTouchResponderInfo.Key);
				});
			}
			// 배경색을 설정한다 }
		}
	}
	#endregion			// 클래스 함수

	#region 제네릭 함수
	//! 객체를 활성화한다
	public T SpawnObj<T>(string a_oKey,
		Vector3 a_stPos, Vector3 a_stScale, Vector3 a_stRotation, string a_oName = KDefine.B_EMPTY_STRING, bool a_bIsWorld = false) where T : Component {
		var oObj = this.SpawnObj(a_oKey, a_stPos, a_stScale, a_stRotation, a_oName, a_bIsWorld);
		return oObj?.GetComponentInChildren<T>();
	}
	#endregion			// 제네릭 함수

	#region 제네릭 클래스 함수
	//! 서브 씬 관리자를 반환한다
	public static T GetSubSceneManager<T>(string a_oKey) where T : CSceneManager {
		return CSceneManager.m_oSubSceneManagerList.ExGetValue(a_oKey, null) as T;
	}
	#endregion			// 제네릭 클래스 함수
	
	#region 조건부 클래스 함수
#if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	//! 디버그 버튼을 눌렀을 경우
	public static void OnTouchDebugBtn() {
		Func.Assert(CSceneManager.ScreenDebugTextRoot != null);
		CSceneManager.ScreenDebugTextRoot.SetActive(!CSceneManager.ScreenDebugTextRoot.activeSelf);
	}

	//! 정적 문자열을 변경한다
	public static void SetStaticString(string a_oString) {
		Func.Assert(CSceneManager.m_oExtraStaticDebugStringBuilder != null && a_oString != null);

		CSceneManager.m_oExtraStaticDebugStringBuilder.Clear();
		CSceneManager.m_oExtraStaticDebugStringBuilder.Append(a_oString);
	}

	//! 동적 문자열을 변경한다
	public static void SetDynamicString(string a_oString) {
		Func.Assert(CSceneManager.m_oExtraDynamicDebugStringBuilder != null && a_oString != null);

		CSceneManager.m_oExtraDynamicDebugStringBuilder.Clear();
		CSceneManager.m_oExtraDynamicDebugStringBuilder.Append(a_oString);
	}
#endif			// #if LOGIC_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
	//! FPS 버튼을 눌렀을 경우
	public static void OnTouchFPSBtn() {
		Func.Assert(CSceneManager.ScreenStaticFPSText != null);
		Func.Assert(CSceneManager.ScreenDynamicFPSText != null);

		CSceneManager.ScreenStaticFPSText.enabled = !CSceneManager.ScreenStaticFPSText.enabled;
		CSceneManager.ScreenDynamicFPSText.enabled = !CSceneManager.ScreenDynamicFPSText.enabled;
	}
#endif			// #if FPS_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

#if UNIVERSAL_RENDER_PIPELINE_ENABLE && CAMERA_STACK_ENABLE
	//! 카메라 스택을 정렬한다
	public static void SortCameraStack() {
		if(CSceneManager.MainCamera != null) {
			var oCameraData = CSceneManager.MainCamera.GetComponentInChildren<UniversalAdditionalCameraData>();

			if(oCameraData != null && oCameraData.cameraStack.ExIsValid()) {
				Func.StableSort(oCameraData.cameraStack, (a_oLhs, a_oRhs) => {
					float fDepthA = a_oLhs.depth;
					float fDepthB = a_oRhs.depth;

					return (fDepthA.ExIsEquals(fDepthB) || fDepthA < fDepthB) ? -1 : 1;
				});
			}
		}
	}
#endif			// #if UNIVERSAL_RENDER_PIPELINE_ENABLE && CAMERA_STACK_ENABLE

#if FILE_BROWSER_ENABLE
	//! 저장 경로 창을 출력한다
	public static void ShowSaveDialog(FileBrowser.OnSuccess a_oSuccessCallback,
		FileBrowser.OnCancel a_oCancelCallback, string a_oDefaultFilepath, bool a_bIsDirectoryMode = false) {
		CSceneManager.ShowTouchResponder(KDefine.U_NAME_DIALOG_TOUCH_RESPONDER,
			CSceneManager.ScreenTopmostUIRoot, KDefine.U_DEF_COLOR_POPUP_BG, null);

		FileBrowser.ShowSaveDialog((a_oFilepath) => {
			a_oSuccessCallback?.Invoke(a_oFilepath);
			CSceneManager.CloseTouchResponder(KDefine.U_NAME_DIALOG_TOUCH_RESPONDER, KDefine.U_DEF_COLOR_TRANSPARENT);
		}, () => {
			a_oCancelCallback?.Invoke();
			CSceneManager.CloseTouchResponder(KDefine.U_NAME_DIALOG_TOUCH_RESPONDER, KDefine.U_DEF_COLOR_TRANSPARENT);
		}, a_bIsDirectoryMode, a_oDefaultFilepath);
	}

	//! 로드 경로 창을 출력한다
	public static void ShowLoadDialog(FileBrowser.OnSuccess a_oSuccessCallback,
		FileBrowser.OnCancel a_oCancelCallback, string a_oDefaultFilepath, bool a_bIsDirectoryMode = false) {
		CSceneManager.ShowTouchResponder(KDefine.U_NAME_DIALOG_TOUCH_RESPONDER,
			CSceneManager.ScreenTopmostUIRoot, KDefine.U_DEF_COLOR_POPUP_BG, null);

		FileBrowser.ShowLoadDialog((a_oFilepath) => {
			a_oSuccessCallback?.Invoke(a_oFilepath);
			CSceneManager.CloseTouchResponder(KDefine.U_NAME_DIALOG_TOUCH_RESPONDER, KDefine.U_DEF_COLOR_TRANSPARENT);
		}, () => {
			a_oCancelCallback?.Invoke();
			CSceneManager.CloseTouchResponder(KDefine.U_NAME_DIALOG_TOUCH_RESPONDER, KDefine.U_DEF_COLOR_TRANSPARENT);
		}, a_bIsDirectoryMode, a_oDefaultFilepath);
	}
#endif			// #if FILE_BROWSER_ENABLE
	#endregion			// 조건부 클래스 함수
}
