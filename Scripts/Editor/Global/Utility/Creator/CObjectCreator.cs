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
	public static void CreateTextBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TEXT_BTN, KDefine.U_OBJ_PATH_TEXT_BTN);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Text + Button/TextScaleButton")]
	public static void CreateTextScaleBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_TEXT_SCALE_BTN);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/LocalizeText + Button/LocalizeTextButton")]
	public static void CreateLocalizeTextBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_BTN, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_BTN);
	}

	//! 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/LocalizeText + Button/LocalizeTextScaleButton")]
	public static void CreateLocalizeTextScaleBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_SCALE_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Button/ImageButton")]
	public static void CreateImgBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_BTN, KDefine.U_OBJ_PATH_IMG_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Button/ImageScaleButton")]
	public static void CreateImgScaleBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_SCALE_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Text + Button/ImageTextButton")]
	public static void CreateImgTextBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_TEXT_BTN, KDefine.U_OBJ_PATH_IMG_TEXT_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + Text + Button/ImageTextScaleButton")]
	public static void CreateImgTextScaleBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_TEXT_SCALE_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + LocalizeText + Button/ImageLocalizeTextButton")]
	public static void CreateImgLocalizeTextBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_LOCALIZE_TEXT_BTN, KDefine.U_OBJ_PATH_IMG_LOCALIZE_TEXT_BTN);
	}

	//! 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Button/Image + LocalizeText + Button/ImageLocalizeTextScaleButton")]
	public static void CreateImgLocalizeTextScaleBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_LOCALIZE_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_LOCALIZE_TEXT_SCALE_BTN);
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
	public static void CreateOriginTextBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TEXT_BTN, KDefine.U_OBJ_PATH_TEXT_BTN, true);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Text + Button/TextScaleButton")]
	public static void CreateOriginTextScaleBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_TEXT_SCALE_BTN, true);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/LocalizeText + Button/LocalizeTextButton")]
	public static void CreateOriginLocalizeTextBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_BTN, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_BTN, true);
	}

	//! 원본 텍스트 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/LocalizeText + Button/LocalizeTextScaleButton")]
	public static void CreateOriginLocalizeTextScaleBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_LOCALIZE_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_LOCALIZE_TEXT_SCALE_BTN, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Button/ImageButton")]
	public static void CreateOriginImgBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_BTN, KDefine.U_OBJ_PATH_IMG_BTN, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Button/ImageScaleButton")]
	public static void CreateOriginImgScaleBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_SCALE_BTN, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Text + Button/ImageTextButton")]
	public static void CreateOriginImgTextBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_TEXT_BTN, KDefine.U_OBJ_PATH_IMG_TEXT_BTN, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + Text + Button/ImageTextScaleButton")]
	public static void CreateOriginImgTextScaleBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_TEXT_SCALE_BTN, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + LocalizeText + Button/ImageLocalizeTextButton")]
	public static void CreateOriginImgLocalizeTextBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_LOCALIZE_TEXT_BTN, KDefine.U_OBJ_PATH_IMG_LOCALIZE_TEXT_BTN, true);
	}

	//! 원본 이미지 버튼을 생성한다
	[MenuItem("GameObject/Create Other/Original/Button/Image + LocalizeText + Button/ImageLocalizeTextScaleButton")]
	public static void CreateOriginImgLocalizeTextScaleBtn() {
		CObjectCreator.CreateObject(KEditorDefine.B_OBJ_NAME_IMG_LOCALIZE_TEXT_SCALE_BTN, KDefine.U_OBJ_PATH_IMG_LOCALIZE_TEXT_SCALE_BTN, true);
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
