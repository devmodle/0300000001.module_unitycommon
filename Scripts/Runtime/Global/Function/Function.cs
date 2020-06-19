using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

//! 기본 함수
public static partial class Function {
	#region 클래스 함수
	//! 에디터 플랫폼 여부를 검사한다
	public static bool IsEditorPlatform() {
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor;
	}

	//! 데스크 탑 플랫폼 여부를 검사한다
	public static bool IsDesktopPlatform() {
		return Function.IsMacPlatform() || Function.IsWindowsPlatform();
	}

	//! 독립 플랫폼 여부를 검사한다
	public static bool IsStandalonePlatform() {
		return Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer;
	}

	//! 맥 플랫폼 여부를 검사한다
	public static bool IsMacPlatform() {
		return Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
	}

	//! 윈도우 플랫폼 여부를 검사한다
	public static bool IsWindowsPlatform() {
		return Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer;
	}

	//! 모바일 플랫폼 여부를 검사한다
	public static bool IsMobilePlatform() {
		return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android;
	}

	//! 콘솔 플랫폼 여부를 검사한다
	public static bool IsConsolePlatform() {
		return Application.platform == RuntimePlatform.PS4 || Application.platform == RuntimePlatform.XboxOne;
	}

	//! 휴대용 콘솔 플랫폼 여부를 검사한다
	public static bool IsHandheldConsolePlatform() {
		return Application.platform == RuntimePlatform.Stadia || Application.platform == RuntimePlatform.Switch;
	}

	//! 동일 파일 여부를 검사한다
	public static bool IsEqualsFile(string a_oFilepathA, 
		string a_oFilepathB, System.Text.Encoding a_oEncodingA, System.Text.Encoding a_oEncodingB) {
		string oStringA = Function.ReadString(a_oFilepathA, a_oEncodingA);
		string oStringB = Function.ReadString(a_oFilepathB, a_oEncodingB);

		return oStringA.ExIsEquals(oStringB);
	}

	//! 읽기용 스트림을 반환한다
	public static FileStream GetReadStream(string a_oFilepath) {
		Function.Assert(a_oFilepath.ExIsValid());
		return File.Exists(a_oFilepath) ? File.Open(a_oFilepath, FileMode.Open, FileAccess.Read) : null;
	}

	//! 쓰기용 스트림을 반환한다
	public static FileStream GetWriteStream(string a_oFilepath,
		bool a_bIsAutoCreateDirectory = true, bool a_bIsAutoBackup = false, string a_oBackupDirectoryName = KDefine.B_EMPTY_STRING) {
		Function.Assert(a_oFilepath.ExIsValid());
		string oDirectoryPath = Path.GetDirectoryName(a_oFilepath);

		if(a_bIsAutoCreateDirectory && !Directory.Exists(oDirectoryPath)) {
			Directory.CreateDirectory(oDirectoryPath);
		}

		// 자동 백업이 가능 할 경우
		if(a_bIsAutoBackup && File.Exists(a_oFilepath)) {
			string oFilename = Path.GetFileName(a_oFilepath);
			string oBackupFilename = string.Format(KDefine.B_FILENAME_FORMAT_BACKUP, Path.GetFileNameWithoutExtension(a_oFilepath), System.DateTime.Now.ToString(KDefine.B_NAME_FORMAT_BACKUP));
			string oBackupDirectoryPath = Path.Combine(oDirectoryPath, (a_oBackupDirectoryName.Length <= 0) ? KDefine.B_DIR_NAME_BACKUP : a_oBackupDirectoryName);

			if(!Directory.Exists(oBackupDirectoryPath)) {
				Directory.CreateDirectory(oBackupDirectoryPath);
			}

			string oBackupFilepath = Path.Combine(oBackupDirectoryPath, 
				oFilename.ExGetReplaceString(Path.GetFileNameWithoutExtension(a_oFilepath), oBackupFilename));

			// 동일한 백업 파일이 없을 경우
			if(!File.Exists(oBackupFilepath)) {
				// 이전 파일을 제거한다 {
				var oFilepaths = Directory.GetFiles(oBackupDirectoryPath);

				if(oFilepaths.Length >= KDefine.B_MAX_NUM_BACKUP_FILES - 1) {
					System.Array.Sort(oFilepaths, (a_oLhs, a_oRhs) => {
						return a_oRhs.CompareTo(a_oLhs);
					});

					for(int i = KDefine.B_MAX_NUM_BACKUP_FILES - 1; i < oFilepaths.Length; ++i) {
						File.Delete(oFilepaths[i]);
					}
				}
				// 이전 파일을 제거한다 }

				// 파일을 복사한다
				Function.CopyFile(a_oFilepath, oBackupFilepath);
			}
		}

		return File.Open(a_oFilepath, FileMode.Create, FileAccess.Write);
	}

	//! 정수 랜덤 값을 반환한다
	public static int[] GetIntRandomValues(int a_nMin, int a_nMax, int a_nNumValues) {
		Function.Assert(a_nMin <= a_nMax);

		return Function.MakeValues<int>(a_nNumValues, (a_nIndex) => {
			return Random.Range(a_nMin, a_nMax + 1);
		});
	}

	//! 실수 랜덤 값을 반환한다
	public static float[] GetFloatRandomValues(float a_fMin, float a_fMax, int a_nNumValues) {
		Function.Assert(a_fMin <= a_fMax);

		return Function.MakeValues<float>(a_nNumValues, (a_nIndex) => {
			return Random.Range(a_fMin, a_fMax);
		});
	}

	//! 정수 랜덤 분할 값을 반환한다
	public static int[] GetIntRandomSplitValues(int a_nValue, int a_nNumValues) {
		Function.Assert(a_nNumValues >= 1);
		int nLeftValue = a_nValue;

		return Function.MakeValues<int>(a_nNumValues, (a_nIndex) => {
			if(a_nNumValues <= 1) {
				return a_nValue;
			} else if(a_nIndex + 1 >= a_nNumValues) {
				return nLeftValue;
			}

			int nNumValues = a_nNumValues - a_nIndex;
			int nValue = Random.Range(1, (nLeftValue / nNumValues) + 1);

			if(a_nIndex + 1 >= a_nNumValues - 1) {
				nValue = nLeftValue / 2;
			} else if(nValue < (nLeftValue / nNumValues) / 2) {
				nValue = nValue * 2;
			}

			return nValue;
		});
	}

	//! URL 을 개방한다
	public static void OpenURL(string a_oURL) {
		Function.Assert(a_oURL.ExIsValid());
		Application.OpenURL(a_oURL);
	}

	//! 메일을 전송한다
	public static void SendMail(string a_oRecipient, string a_oTitle, string a_oMessage) {
		Function.Assert(a_oTitle != null && a_oMessage != null && a_oRecipient.ExIsValid());

		string oURL = string.Format(KDefine.B_MAIL_URL_FORMAT,
			a_oRecipient, System.Uri.EscapeUriString(a_oTitle), System.Uri.EscapeUriString(a_oMessage));

		Function.OpenURL(oURL);
	}

	//! 조건을 검사한다
	[Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
	public static void Assert(bool a_bIsTrue, string a_oMessage = KDefine.B_EMPTY_STRING) {
		if(a_oMessage.ExIsValid()) {
			UnityEngine.Assertions.Assert.IsTrue(a_bIsTrue, a_oMessage);
		} else {
			UnityEngine.Assertions.Assert.IsTrue(a_bIsTrue);
		}
	}

	//! 로그를 출력한다
	[Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
	public static void ShowLog(string a_oFormat, params object[] a_oParams) {
		Function.DoShowLog(string.Format(a_oFormat, a_oParams), LogType.Log);
	}

	//! 로그를 출력한다
	[Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
	public static void ShowLog(string a_oFormat, Color a_stColor, params object[] a_oParams) {
		string oFormat = a_oFormat.ExGetColorFormatString(a_stColor);
		Function.DoShowLog(string.Format(oFormat, a_oParams), LogType.Log);
	}

	//! 경고 로그를 출력한다
	public static void ShowLogWarning(string a_oFormat, params object[] a_oParams) {
		Function.DoShowLog(string.Format(a_oFormat, a_oParams), LogType.Warning);
	}

	//! 경고 로그를 출력한다
	public static void ShowLogWarning(string a_oFormat, Color a_stColor, params object[] a_oParams) {
		string oFormat = a_oFormat.ExGetColorFormatString(a_stColor);
		Function.DoShowLog(string.Format(oFormat, a_oParams), LogType.Warning);
	}

	//! 에러 로그를 출력한다
	public static void ShowLogError(string a_oFormat, params object[] a_oParams) {
		Function.DoShowLog(string.Format(a_oFormat, a_oParams), LogType.Error);
	}

	//! 에러 로그를 출력한다
	public static void ShowLogError(string a_oFormat, Color a_stColor, params object[] a_oParams) {
		string oFormat = a_oFormat.ExGetColorFormatString(a_stColor);
		Function.DoShowLog(string.Format(oFormat, a_oParams), LogType.Error);
	}

	//! 파일을 복사한다
	public static void CopyFile(string a_oSrcFilepath, string a_oDestFilepath, bool a_bIsOverwrite = true) {
		Function.Assert(a_oSrcFilepath.ExIsValid() && a_oDestFilepath.ExIsValid());

		// 파일 복사가 가능 할 경우
		if(File.Exists(a_oSrcFilepath) && (a_bIsOverwrite || !File.Exists(a_oDestFilepath))) {
			var oBytes = Function.ReadBytes(a_oSrcFilepath);
			Function.WriteBytes(a_oDestFilepath, oBytes);
		}
	}

	//! 파일을 복사한다
	public static void CopyFile(string a_oSrcFilepath, 
		string a_oDestFilepath, string a_oIgnore, System.Text.Encoding a_oEncoding, bool a_bIsOverwrite = true) {
		Function.Assert(a_oSrcFilepath.ExIsValid() && a_oDestFilepath.ExIsValid() && a_oIgnore.ExIsValid());

		// 파일 복사가 가능 할 경우
		if(File.Exists(a_oSrcFilepath) && (a_bIsOverwrite || !File.Exists(a_oDestFilepath))) {
			var oStringLines = Function.ReadStringLines(a_oSrcFilepath, a_oEncoding);

			if(oStringLines.ExIsValid()) {
				var oStringBuilder = new System.Text.StringBuilder();

				for(int i = 0; i < oStringLines.Length; ++i) {
					if(!oStringLines[i].Contains(a_oIgnore)) {
						oStringBuilder.AppendLine(oStringLines[i]);
					}
				}

				Function.WriteString(a_oDestFilepath, oStringBuilder.ToString(), a_oEncoding);
			}
		}
	}

	//! 파일을 복사한다
	public static void CopyFile(string a_oSrcFilepath, 
		string a_oDestFilepath, string a_oSearch, string a_oReplace, System.Text.Encoding a_oEncoding, bool a_bIsOverwrite = true) {
		Function.Assert(a_oSrcFilepath.ExIsValid() && a_oDestFilepath.ExIsValid() && a_oSearch.ExIsValid() && a_oReplace.ExIsValid());

		// 파일 복사가 가능 할 경우
		if(File.Exists(a_oSrcFilepath) && (a_bIsOverwrite || !File.Exists(a_oDestFilepath))) {
			var oStringLines = Function.ReadStringLines(a_oSrcFilepath, a_oEncoding);

			if(oStringLines.ExIsValid()) {
				var oStringBuilder = new System.Text.StringBuilder();

				for(int i = 0; i < oStringLines.Length; ++i) {
					string oString = !oStringLines[i].ExIsValid() ? string.Empty
						: oStringLines[i].ExGetReplaceString(a_oSearch, a_oReplace, short.MaxValue);
						
					oStringBuilder.AppendLine(oString);
				}

				Function.WriteString(a_oDestFilepath, oStringBuilder.ToString(), a_oEncoding);
			}
		}
	}

	//! 디렉토리를 복사한다
	public static void CopyDirectory(string a_oSrcPath, string a_oDestPath, bool a_bIsOverwrite = true) {
		Function.Assert(a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid());

		// 디렉토리 복사가 가능 할 경우
		if(Directory.Exists(a_oSrcPath)) {
			// 파일을 복사한다
			if(a_bIsOverwrite || !Directory.Exists(a_oDestPath)) {
				if(Directory.Exists(a_oDestPath)) {
					Directory.Delete(a_oDestPath, true);
				}

				Directory.CreateDirectory(a_oDestPath);
				var oFiles = Directory.GetFiles(a_oSrcPath);

				for(int i = 0; i < oFiles.Length; ++i) {
					string oFilename = Path.GetFileName(oFiles[i]);
					Function.CopyFile(oFiles[i], Path.Combine(a_oDestPath, oFilename), a_bIsOverwrite);
				}
			}

			// 하위 디렉토리를 복사한다 {
			var oDirectories = Directory.GetDirectories(a_oSrcPath);

			for(int i = 0; i < oDirectories.Length; ++i) {
				string oDirectoryName = Path.GetFileNameWithoutExtension(oDirectories[i]);
				Function.CopyDirectory(oDirectories[i], Path.Combine(a_oDestPath, oDirectoryName), a_bIsOverwrite);
			}
			// 하위 디렉토리를 복사한다 }
		}
	}

	//! 바이트를 읽어들인다
	public static byte[] ReadBytes(string a_oFilepath) {
		Function.Assert(a_oFilepath.ExIsValid());
		return File.Exists(a_oFilepath) ? File.ReadAllBytes(a_oFilepath) : null;
	}

	//! 보안 바이트를 읽어들인다
	public static byte[] ReadSecurityBytes(string a_oFilepath) {
		var oBytes = Function.ReadBytes(a_oFilepath);
		return (oBytes != null) ? System.Convert.FromBase64String(System.Text.Encoding.Default.GetString(oBytes)) : null;
	}

	//! 문자열을 읽어들인다
	public static string ReadString(string a_oFilepath, System.Text.Encoding a_oEncoding) {
		Function.Assert(a_oEncoding != null && a_oFilepath.ExIsValid());
		return File.Exists(a_oFilepath) ? File.ReadAllText(a_oFilepath, a_oEncoding) : string.Empty;
	}

	//! 문자열 라인을 읽어들인다
	public static string[] ReadStringLines(string a_oFilepath, System.Text.Encoding a_oEncoding) {
		Function.Assert(a_oEncoding != null && a_oFilepath.ExIsValid());
		return File.ReadAllLines(a_oFilepath, a_oEncoding);
	}

	//! 보안 문자열을 읽어들인다
	public static string ReadSecurityString(string a_oFilepath, System.Text.Encoding a_oEncoding) {
		Function.Assert(a_oEncoding != null && a_oFilepath.ExIsValid());
		var oBytes = Function.ReadSecurityBytes(a_oFilepath);

		return (oBytes != null) ? a_oEncoding.GetString(oBytes) : string.Empty;
	}

	//! 바이트를 기록한다
	public static void WriteBytes(string a_oFilepath,
		byte[] a_oBytes, bool a_bIsAutoCreateDirectory = true, bool a_bIsAutoBackup = false, string a_oBackupDirectoryName = KDefine.B_EMPTY_STRING) {
		using(var oWriteStream = Function.GetWriteStream(a_oFilepath, a_bIsAutoCreateDirectory, a_bIsAutoBackup, a_oBackupDirectoryName)) {
			Function.WriteBytes(oWriteStream, a_oBytes);
		}
	}

	//! 바이트를 기록한다
	public static void WriteBytes(FileStream a_oWriteStream, byte[] a_oBytes) {
		Function.Assert(a_oBytes.ExIsValid());

		a_oWriteStream?.Write(a_oBytes, 0, a_oBytes.Length);
		a_oWriteStream?.Flush(true);
	}

	//! 보안 바이트를 기록한다
	public static void WriteSecurityBytes(string a_oFilepath,
		byte[] a_oBytes, bool a_bIsAutoCreateDirectory = true, bool a_bIsAutoBackup = false, string a_oBackupDirectoryName = KDefine.B_EMPTY_STRING) {
		using(var oWriteStream = Function.GetWriteStream(a_oFilepath, a_bIsAutoCreateDirectory, a_bIsAutoBackup, a_oBackupDirectoryName)) {
			Function.WriteSecurityBytes(oWriteStream, a_oBytes);
		}
	}

	//! 보안 바이트를 기록한다
	public static void WriteSecurityBytes(FileStream a_oWriteStream, byte[] a_oBytes) {
		Function.Assert(a_oBytes.ExIsValid());
		var oString = System.Convert.ToBase64String(a_oBytes, 0, a_oBytes.Length);

		Function.WriteBytes(a_oWriteStream, System.Text.Encoding.Default.GetBytes(oString));
	}

	//! 문자열을 기록한다
	public static void WriteString(string a_oFilepath,
		string a_oString, System.Text.Encoding a_oEncoding, bool a_bIsAutoCreateDirectory = true, bool a_bIsAutoBackup = false, string a_oBackupDirectoryName = KDefine.B_EMPTY_STRING) {
		using(var oWriteStream = Function.GetWriteStream(a_oFilepath, a_bIsAutoCreateDirectory, a_bIsAutoBackup, a_oBackupDirectoryName)) {
			Function.WriteString(oWriteStream, a_oString, a_oEncoding);
		}
	}

	//! 문자열을 기록한다
	public static void WriteString(FileStream a_oWriteStream, string a_oString, System.Text.Encoding a_oEncoding) {
		Function.Assert(a_oEncoding != null && a_oString.ExIsValid());
		Function.WriteBytes(a_oWriteStream, a_oEncoding.GetBytes(a_oString));
	}

	//! 보안 문자열을 기록한다
	public static void WriteSecurityString(string a_oFilepath,
		string a_oString, System.Text.Encoding a_oEncoding, bool a_bIsAutoCreateDirectory = true, bool a_bIsAutoBackup = false, string a_oBackupDirectoryName = KDefine.B_EMPTY_STRING) {
		using(var oWriteStream = Function.GetWriteStream(a_oFilepath, a_bIsAutoCreateDirectory, a_bIsAutoBackup, a_oBackupDirectoryName)) {
			Function.WriteSecurityString(oWriteStream, a_oString, a_oEncoding);
		}
	}

	//! 보안 문자열을 기록한다
	public static void WriteSecurityString(FileStream a_oWriteStream, string a_oString, System.Text.Encoding a_oEncoding) {
		Function.Assert(a_oEncoding != null && a_oString.ExIsValid());
		Function.WriteSecurityBytes(a_oWriteStream, a_oEncoding.GetBytes(a_oString));
	}

	//! 함수를 지연 호출한다
	public static void LateCallFunction(CComponent a_oComponent,
		System.Action<CComponent, object[]> a_oCallback, object[] a_oParams = null) {
		a_oComponent?.StartCoroutine(Function.DoLateCallFunction(a_oComponent, a_oCallback, a_oParams));
	}

	//! 함수를 지연 호출한다
	public static void LateCallFunction(CComponent a_oComponent,
		float a_fDelay, System.Action<CComponent, object[]> a_oCallback, bool a_bIsRealtime = false, object[] a_oParams = null) {
		a_oComponent?.StartCoroutine(Function.DoLateCallFunction(a_oComponent, a_fDelay, a_oCallback, a_bIsRealtime, a_oParams));
	}

	//! 함수를 반복 호출한다
	public static void RepeatCallFunction(CComponent a_oComponent,
		float a_fDeltaTime, float a_fMaxDeltaTime, System.Func<CComponent, object[], bool, bool> a_oCallback, bool a_bIsRealtime = false, object[] a_oParams = null) {
		Function.Assert(a_oCallback != null);
		a_oComponent?.StartCoroutine(Function.DoRepeatCallFunction(a_oComponent, a_fDeltaTime, a_fMaxDeltaTime, a_oCallback, a_bIsRealtime, a_oParams));
	}

	//! 비동기 작업을 대기한다
	public static void WaitAsyncTask(Task a_oTask, System.Action<Task> a_oCallback) {
		Function.Assert(a_oTask != null);

		a_oTask.ContinueWith((a_oContinueTask) => {
			string oKey = string.Format(KDefine.B_KEY_FORMAT_ASYNC_TASK_CALLBACK,
				System.Threading.Thread.CurrentThread.ManagedThreadId);

			CScheduleManager.Instance.AddCallback(oKey, () => {
				a_oCallback?.Invoke(a_oContinueTask);
			});
		});
	}

	//! 비동기 작업을 대기한다
	public static IEnumerator WaitAsyncOperation(AsyncOperation a_oAsyncOperation, System.Action<AsyncOperation, bool> a_oCallback, bool a_bIsRealtime = false) {
		Function.Assert(a_oAsyncOperation != null);

		do {
			yield return Function.CreateWaitForSeconds(KDefine.B_DELTA_TIME_ASYNC_OPERATION, a_bIsRealtime);
			a_oCallback?.Invoke(a_oAsyncOperation, false);
		} while(!a_oAsyncOperation.isDone);

		yield return Function.CreateWaitForSeconds(KDefine.B_DELTA_TIME_ASYNC_OPERATION, a_bIsRealtime);
		a_oCallback?.Invoke(a_oAsyncOperation, true);
	}

	//! 대기 객체를 생성한다
	public static IEnumerator CreateWaitForSeconds(float a_fDeltaTime, bool a_bIsRealtime = false) {
		if(a_bIsRealtime) {
			yield return new WaitForSecondsRealtime(a_fDeltaTime);
		} else {
			yield return new WaitForSeconds(a_fDeltaTime);
		}
	}

	//! 로그를 출력한다
	private static void DoShowLog(string a_oLog, LogType a_eLogType) {
		if(a_eLogType == LogType.Error) {
			UnityEngine.Debug.LogError(a_oLog);
		} else if(a_eLogType == LogType.Warning) {
			UnityEngine.Debug.LogWarning(a_oLog);
		} else {
			UnityEngine.Debug.Log(a_oLog);
		}
	}

	//! 함수를 지연 호출한다
	private static IEnumerator DoLateCallFunction(CComponent a_oComponent,
		System.Action<CComponent, object[]> a_oCallback, object[] a_oParams) {
		yield return new WaitForEndOfFrame();
		a_oCallback?.Invoke(a_oComponent, a_oParams);
	}

	//! 함수를 지연 호출한다
	private static IEnumerator DoLateCallFunction(CComponent a_oComponent,
		float a_fDelay, System.Action<CComponent, object[]> a_oCallback, bool a_bIsRealtime, object[] a_oParams) {
		yield return Function.CreateWaitForSeconds(a_fDelay, a_bIsRealtime);
		a_oCallback?.Invoke(a_oComponent, a_oParams);
	}

	//! 함수를 반복 호출한다
	private static IEnumerator DoRepeatCallFunction(CComponent a_oComponent,
		float a_fDeltaTime, double a_dblMaxDeltaTime, System.Func<CComponent, object[], bool, bool> a_oCallback, bool a_bIsRealtime, object[] a_oParams) {
		var stStartTime = System.DateTime.Now;
		System.TimeSpan stDeltaTime;

		do {
			yield return Function.CreateWaitForSeconds(a_fDeltaTime, a_bIsRealtime);
			stDeltaTime = System.DateTime.Now - stStartTime;
		} while(a_oCallback(a_oComponent, a_oParams, false) && stDeltaTime.TotalSeconds.ExIsLess(a_dblMaxDeltaTime));

		a_oCallback(a_oComponent, a_oParams, true);
	}
	#endregion			// 클래스 함수

	#region 제네릭 클래스 함수
	//! 값을 교환한다
	public static void Swap<T>(ref T a_tLhs, ref T a_tRhs) {
		T tTemp = a_tLhs;
		a_tLhs = a_tRhs;
		a_tRhs = tTemp;
	}

	//! 안전 정렬을 수행한다
	public static void StableSort<T>(T[] a_oValues, T[] oTempValues, int a_nLeft, int a_nRight, System.Comparison<T> a_oCompare) {
		if(a_nLeft < a_nRight) {
			int nCount = 0;
			int nMiddle = (a_nLeft + a_nRight) / 2;

			int nLeftIndex = a_nLeft;
			int nRightIndex = nMiddle + 1;

			// 정렬 범위를 분활한다
			Function.StableSort<T>(a_oValues, oTempValues, a_nLeft, nMiddle, a_oCompare);
			Function.StableSort<T>(a_oValues, oTempValues, nMiddle + 1, a_nRight, a_oCompare);

			// 정렬을 수행한다 {
			while(nLeftIndex <= nMiddle && nRightIndex <= a_nRight) {
				while(nLeftIndex <= nMiddle && a_oCompare?.Invoke(a_oValues[nLeftIndex], a_oValues[nRightIndex]) <= 0) {
					oTempValues[nCount++] = a_oValues[nLeftIndex++];
				}

				while(nRightIndex <= a_nRight && !(a_oCompare?.Invoke(a_oValues[nLeftIndex], a_oValues[nRightIndex]) <= 0)) {
					oTempValues[nCount++] = a_oValues[nRightIndex++];
				}
			}

			while(nLeftIndex <= nMiddle) {
				oTempValues[nCount++] = a_oValues[nLeftIndex++];
			}

			while(nRightIndex <= a_nRight) {
				oTempValues[nCount++] = a_oValues[nRightIndex++];
			}

			for(int i = 0; i < nCount; ++i) {
				a_oValues[a_nLeft + i] = oTempValues[i];
			}
			// 정렬을 수행한다 }
		}
	}

	//! 안전 정렬을 수행한다
	public static void StableSort<T>(List<T> a_oValueList, T[] oTempValues, int a_nLeft, int a_nRight, System.Comparison<T> a_oCompare) {
		if(a_nLeft < a_nRight) {
			int nCount = 0;
			int nMiddle = (a_nLeft + a_nRight) / 2;

			int nLeftIndex = a_nLeft;
			int nRightIndex = nMiddle + 1;

			// 정렬 범위를 분활한다
			Function.StableSort<T>(a_oValueList, oTempValues, a_nLeft, nMiddle, a_oCompare);
			Function.StableSort<T>(a_oValueList, oTempValues, nMiddle + 1, a_nRight, a_oCompare);

			// 정렬을 수행한다 {
			while(nLeftIndex <= nMiddle && nRightIndex <= a_nRight) {
				while(nLeftIndex <= nMiddle && a_oCompare?.Invoke(a_oValueList[nLeftIndex], a_oValueList[nRightIndex]) <= 0) {
					oTempValues[nCount++] = a_oValueList[nLeftIndex++];
				}

				while(nRightIndex <= a_nRight && !(a_oCompare?.Invoke(a_oValueList[nLeftIndex], a_oValueList[nRightIndex]) <= 0)) {
					oTempValues[nCount++] = a_oValueList[nRightIndex++];
				}
			}

			while(nLeftIndex <= nMiddle) {
				oTempValues[nCount++] = a_oValueList[nLeftIndex++];
			}

			while(nRightIndex <= a_nRight) {
				oTempValues[nCount++] = a_oValueList[nRightIndex++];
			}

			for(int i = 0; i < nCount; ++i) {
				a_oValueList[a_nLeft + i] = oTempValues[i];
			}
			// 정렬을 수행한다 }
		}
	}

	//! 값을 생성한다
	public static T[] MakeValues<T>(int a_nNumValues, System.Func<int, T> a_oCallback) {
		Function.Assert(a_oCallback != null && a_nNumValues >= 1);
		var oValues = new T[a_nNumValues];

		for(int i = 0; i < a_nNumValues; ++i) {
			oValues[i] = a_oCallback.Invoke(i);
		}

		return oValues;
	}

	//! 섞인 값을 생성한다
	public static T[] MakeShuffleValues<T>(int a_nNumValues, System.Func<int, T> a_oCallback) {
		var oValues = Function.MakeValues<T>(a_nNumValues, a_oCallback);
		oValues.ExShuffle();

		return oValues;
	}
	
	//! 비동기 작업을 대기한다
	public static void WaitAsyncTask<T>(Task<T> a_oTask, System.Action<Task<T>> a_oCallback) {
		Function.Assert(a_oTask != null);

		a_oTask.ContinueWith((a_oContinueTask) => {
			string oKey = string.Format(KDefine.B_KEY_FORMAT_ASYNC_TASK_CALLBACK,
				System.Threading.Thread.CurrentThread.ManagedThreadId);

			CScheduleManager.Instance.AddCallback(oKey, () => {
				a_oCallback?.Invoke(a_oContinueTask);
			});
		});
	}
	#endregion			// 제네릭 클래스 함수
}
