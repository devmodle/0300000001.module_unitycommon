using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

//! 기본 객체 생성자
public static partial class CObjectCreator {
	#region 클래스 함수
	//! 텍스트를 생성한다
	[MenuItem("GameObject/Create Other/Text/Text")]
	public static void CreateText() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TEXT, KDefine.U_OBJ_PATH_TEXT);
	}

	//! 텍스트를 생성한다
	[MenuItem("GameObject/Create Other/Text/LocalizeText")]
	public static void CreateLocalizeText() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT, KDefine.U_OBJ_PATH_LOCALIZE_TEXT);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Text + Button/TextButton")]
	public static void CreateTextButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TEXT_BUTTON, KDefine.U_OBJ_PATH_TEXT_BUTTON);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Text + Button/TextScaleButton")]
	public static void CreateTextScaleButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_TEXT_SCALE_BUTTON);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/LocalizeText + Button/LocalizeTextButton")]
	public static void CreateLocalizeTextButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_BUTTON, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_BUTTON);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/LocalizeText + Button/LocalizeTextScaleButton")]
	public static void CreateLocalizeTextScaleButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_SCALE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Button/ImageButton")]
	public static void CreateImageButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Button/ImageScaleButton")]
	public static void CreateImageScaleButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_SCALE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Text + Button/ImageTextButton")]
	public static void CreateImageTextButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_TEXT_BUTTON, KDefine.U_OBJ_PATH_IMAGE_TEXT_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Text + Button/ImageTextScaleButton")]
	public static void CreateImageTextScaleButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_TEXT_SCALE_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + LocalizeText + Button/ImageLocalizeTextButton")]
	public static void CreateImageLocalizeTextButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_LOCALIZE_TEXT_BUTTON, KDefine.U_OBJ_PATH_IMAGE_LOCALIZE_TEXT_BUTTON);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + LocalizeText + Button/ImageLocalizeTextScaleButton")]
	public static void CreateImageLocalizeTextScaleButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_LOCALIZE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_LOCALIZE_TEXT_SCALE_BUTTON);
	}

	//! 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create Other/ScrollView/ScrollView")]
	public static void CreateScrollView() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_SCROLL_VIEW, KDefine.U_OBJ_PATH_SCROLL_VIEW);
	}

	//! 페이지 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create Other/ScrollView/PageScrollView")]
	public static void CreatePageScrollView() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_PAGE_SCROLL_VIEW, KDefine.U_OBJ_PATH_PAGE_SCROLL_VIEW);
	}

	//! 터치 응답자를 생성한다
	[MenuItem("GameObject/Create Other/Responder/TouchResponder")]
	public static void CreateTouchResponder() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TOUCH_RESPONDER, KDefine.U_OBJ_PATH_TOUCH_RESPONDER);
	}

	//! 드래그 응답자를 생성한다
	[MenuItem("GameObject/Create Other/Responder/DragResponder")]
	public static void CreateDragResponder() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_DRAG_RESPONDER, KDefine.U_OBJ_PATH_DRAG_RESPONDER);
	}

	//! 원본 텍스트를 생성한다
	[MenuItem("GameObject/Create Other/Original/Text/Text")]
	public static void CreateOriginText() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TEXT, KDefine.U_OBJ_PATH_TEXT, true);
	}

	//! 원본 텍스트를 생성한다
	[MenuItem("GameObject/Create Other/Original/Text/LocalizeText")]
	public static void CreateOriginLocalizeText() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT, KDefine.U_OBJ_PATH_LOCALIZE_TEXT, true);
	}
	
	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Text + Button/TextButton")]
	public static void CreateOriginTextButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TEXT_BUTTON, KDefine.U_OBJ_PATH_TEXT_BUTTON, true);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Text + Button/TextScaleButton")]
	public static void CreateOriginTextScaleButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_TEXT_SCALE_BUTTON, true);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/LocalizeText + Button/LocalizeTextButton")]
	public static void CreateOriginLocalizeTextButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_BUTTON, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_BUTTON, true);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/LocalizeText + Button/LocalizeTextScaleButton")]
	public static void CreateOriginLocalizeTextScaleButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_SCALE_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Button/ImageButton")]
	public static void CreateOriginImageButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Button/ImageScaleButton")]
	public static void CreateOriginImageScaleButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_SCALE_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Text + Button/ImageTextButton")]
	public static void CreateOriginImageTextButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_TEXT_BUTTON, KDefine.U_OBJ_PATH_IMAGE_TEXT_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Text + Button/ImageTextScaleButton")]
	public static void CreateOriginImageTextScaleButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_TEXT_SCALE_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + LocalizeText + Button/ImageLocalizeTextButton")]
	public static void CreateOriginImageLocalizeTextButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_LOCALIZE_TEXT_BUTTON, KDefine.U_OBJ_PATH_IMAGE_LOCALIZE_TEXT_BUTTON, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + LocalizeText + Button/ImageLocalizeTextScaleButton")]
	public static void CreateOriginImageLocalizeTextScaleButton() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMAGE_LOCALIZE_TEXT_SCALE_BUTTON, KDefine.U_OBJ_PATH_IMAGE_LOCALIZE_TEXT_SCALE_BUTTON, true);
	}

	//! 원본 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create Other/Original/ScrollView/ScrollView")]
	public static void CreateOriginScrollView() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_SCROLL_VIEW, KDefine.U_OBJ_PATH_SCROLL_VIEW, true);
	}

	//! 원본 페이지 스크롤 뷰를 생성한다
	[MenuItem("GameObject/Create Other/Original/ScrollView/PageScrollView")]
	public static void CreateOriginPageScrollView() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_PAGE_SCROLL_VIEW, KDefine.U_OBJ_PATH_PAGE_SCROLL_VIEW, true);
	}

	//! 원본 터치 응답자를 생성한다
	[MenuItem("GameObject/Create Other/Original/Responder/TouchResponder")]
	public static void CreateOriginTouchResponder() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TOUCH_RESPONDER, KDefine.U_OBJ_PATH_TOUCH_RESPONDER, true);
	}

	//! 원본 드래그 응답자를 생성한다
	[MenuItem("GameObject/Create Other/Original/Responder/DragResponder")]
	public static void CreateOriginDragResponder() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_DRAG_RESPONDER, KDefine.U_OBJ_PATH_DRAG_RESPONDER, true);
	}

	//! 객체를 생성한다
	private static GameObject CreateObject(string a_oName, string a_oFilepath, bool a_bIsOriginal = true) {
		GameObject oObject = null;
		GameObject oOrigin = Resources.Load<GameObject>(a_oFilepath);
		
		if(a_bIsOriginal) {
			oObject = Func.CreateCloneObject(a_oName, oOrigin, EditorFunc.GetActiveObject());
		} else {
			oObject = EditorFunc.CreatePrefabInstance(a_oName, oOrigin, EditorFunc.GetActiveObject());
		}

		Func.SelectObject(oObject);
		EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

		return oObject;
	}
	#endregion			// 클래스 함수
}
