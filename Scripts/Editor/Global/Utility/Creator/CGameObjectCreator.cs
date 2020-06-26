using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

//! 기본 게임 객체 생성자
public static partial class CGameObjectCreator {
	#region 클래스 함수
	//! 텍스트를 생성한다
	[MenuItem("GameObject/Create Other/Text/Text")]
	public static void CreateText() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TEXT, KDefine.U_OBJ_PATH_TEXT);
	}

	//! 텍스트를 생성한다
	[MenuItem("GameObject/Create Other/Text/LocalizeText")]
	public static void CreateLocalizeText() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT, KDefine.U_OBJ_PATH_LOCALIZE_TEXT);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Text + Button/TextButton")]
	public static void CreateTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TEXT_BUTTON, KDefine.U_OBJ_PATH_TEXT_BUTTON);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Text + Button/TextScaleButton")]
	public static void CreateTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_TEXT_SCALE_BUTTON);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/LocalizeText + Button/LocalizeTextButton")]
	public static void CreateLocalizeTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_BUTTON, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_BUTTON);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/LocalizeText + Button/LocalizeTextScaleButton")]
	public static void CreateLocalizeTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_SCALE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Button/ImageButton")]
	public static void CreateImageButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Button/ImageScaleButton")]
	public static void CreateImageScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_SCALE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Text + Button/ImageTextButton")]
	public static void CreateImageTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_TEXT_BUTTON, KDefine.U_OBJ_PATH_IMAGE_TEXT_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Text + Button/ImageTextScaleButton")]
	public static void CreateImageTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_TEXT_SCALE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + LocalizeText + Button/ImageLocalizeTextButton")]
	public static void CreateImageLocalizeTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_LOCALIZE_TEXT_BUTTON, KDefine.U_OBJ_PATH_IMAGE_LOCALIZE_TEXT_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + LocalizeText + Button/ImageLocalizeTextScaleButton")]
	public static void CreateImageLocalizeTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_LOCALIZE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_LOCALIZE_TEXT_SCALE_BUTTON);
	}

	//! 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create Other/ScrollView/ScrollView")]
	public static void CreateScrollView() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_SCROLL_VIEW, KDefine.U_OBJ_PATH_SCROLL_VIEW);
	}

	//! 페이지 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create Other/ScrollView/PageScrollView")]
	public static void CreatePageScrollView() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_PAGE_SCROLL_VIEW, KDefine.U_OBJ_PATH_PAGE_SCROLL_VIEW);
	}

	//! 터치 응답자를 생성한다
	[MenuItem("GameObject/Create Other/Responder/TouchResponder")]
	public static void CreateTouchResponder() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TOUCH_RESPONDER, KDefine.U_OBJ_PATH_TOUCH_RESPONDER);
	}

	//! 드래그 응답자를 생성한다
	[MenuItem("GameObject/Create Other/Responder/DragResponder")]
	public static void CreateDragResponder() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_DRAG_RESPONDER, KDefine.U_OBJ_PATH_DRAG_RESPONDER);
	}

	//! 원본 텍스트를 생성한다
	[MenuItem("GameObject/Create Other/Original/Text/Text")]
	public static void CreateOriginText() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TEXT, KDefine.U_OBJ_PATH_TEXT, true);
	}

	//! 원본 텍스트를 생성한다
	[MenuItem("GameObject/Create Other/Original/Text/LocalizeText")]
	public static void CreateOriginLocalizeText() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT, KDefine.U_OBJ_PATH_LOCALIZE_TEXT, true);
	}
	
	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Text + Button/TextButton")]
	public static void CreateOriginTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TEXT_BUTTON, KDefine.U_OBJ_PATH_TEXT_BUTTON, true);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Text + Button/TextScaleButton")]
	public static void CreateOriginTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_TEXT_SCALE_BUTTON, true);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/LocalizeText + Button/LocalizeTextButton")]
	public static void CreateOriginLocalizeTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_BUTTON, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_BUTTON, true);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/LocalizeText + Button/LocalizeTextScaleButton")]
	public static void CreateOriginLocalizeTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_SCALE_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Button/ImageButton")]
	public static void CreateOriginImageButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Button/ImageScaleButton")]
	public static void CreateOriginImageScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_SCALE_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Text + Button/ImageTextButton")]
	public static void CreateOriginImageTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_TEXT_BUTTON, KDefine.U_OBJ_PATH_IMAGE_TEXT_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Text + Button/ImageTextScaleButton")]
	public static void CreateOriginImageTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_TEXT_SCALE_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + LocalizeText + Button/ImageLocalizeTextButton")]
	public static void CreateOriginImageLocalizeTextButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_LOCALIZE_TEXT_BUTTON, KDefine.U_OBJ_PATH_IMAGE_LOCALIZE_TEXT_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + LocalizeText + Button/ImageLocalizeTextScaleButton")]
	public static void CreateOriginImageLocalizeTextScaleButton() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_IMAGE_LOCALIZE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_LOCALIZE_TEXT_SCALE_BUTTON, true);
	}

	//! 원본 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create Other/Original/ScrollView/ScrollView")]
	public static void CreateOriginScrollView() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_SCROLL_VIEW, KDefine.U_OBJ_PATH_SCROLL_VIEW, true);
	}

	//! 원본 페이지 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create Other/Original/ScrollView/PageScrollView")]
	public static void CreateOriginPageScrollView() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_PAGE_SCROLL_VIEW, KDefine.U_OBJ_PATH_PAGE_SCROLL_VIEW, true);
	}

	//! 원본 터치 응답자를 생성한다
	[MenuItem("GameObject/Create Other/Original/Responder/TouchResponder")]
	public static void CreateOriginTouchResponder() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_TOUCH_RESPONDER, KDefine.U_OBJ_PATH_TOUCH_RESPONDER, true);
	}

	//! 원본 드래그 응답자를 생성한다
	[MenuItem("GameObject/Create Other/Original/Responder/DragResponder")]
	public static void CreateOriginDragResponder() {
		CGameObjectCreator.CreateGameObject(KEditorDefine.B_OBJ_NAME_DRAG_RESPONDER, KDefine.U_OBJ_PATH_DRAG_RESPONDER, true);
	}

	//! 게임 객체를 생성한다
	private static GameObject CreateGameObject(string a_oName, string a_oFilepath, bool a_bIsOriginal = true) {
		GameObject oGameObject = null;
		GameObject oOriginObject = Resources.Load<GameObject>(a_oFilepath);
		
		if(a_bIsOriginal) {
			oGameObject = Func.CreateCloneGameObject(a_oName, oOriginObject, EditorFunc.GetActiveGameObject());
		} else {
			oGameObject = EditorFunc.CreatePrefabInstance(a_oName, oOriginObject, EditorFunc.GetActiveGameObject());
		}

		Func.SelectGameObject(oGameObject);
		EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

		return oGameObject;
	}
	#endregion			// 클래스 함수
}
