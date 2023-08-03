using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class LogInTest
    {
        private bool sceneLoaded;
        private LogInDemoViewModel logInDemoViewModel;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("Scene_Demo", LoadSceneMode.Single);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            sceneLoaded = true;
        }

        private void ChangeViewToLogInViewModel()
        {
            ScreenManager.instance.ChangeView(ViewID.LogInDemoViewModel);
            logInDemoViewModel = (LogInDemoViewModel)ScreenManager.instance.GetView(ViewID.LogInDemoViewModel);
        }

        [UnityTest]
        public IEnumerator ObjectComesWithViewModel_ComponentAdded_NotNull()
        {
            yield return new WaitWhile(() => sceneLoaded == false);

            ChangeViewToLogInViewModel();

            yield return new WaitForFixedUpdate();

            Assert.NotNull(logInDemoViewModel.GetComponent<LogInDemoViewModel>(), "Object has LogInViewModel attached");
        }

        [UnityTest]
        public IEnumerator ObjectComesWithPresenter_ComponentAdded_NotNull()
        {
            yield return new WaitWhile(() => sceneLoaded == false);

            ChangeViewToLogInViewModel();

            yield return new WaitForFixedUpdate();

            Assert.NotNull(logInDemoViewModel.GetComponent<LogInDemoPresenter>(), "Object has LogInDemoPresenter attached");
        }

        [UnityTest]
        public IEnumerator ObjectComesWithInteractor_ComponentAdded_NotNull()
        {
            yield return new WaitWhile(() => sceneLoaded == false);

            ChangeViewToLogInViewModel();

            yield return new WaitForFixedUpdate();

            Assert.NotNull(logInDemoViewModel.GetComponent<LogInDemoInteractor>(), "Object has LogInDemoInteractor attached");
        }

        [UnityTest]
        public IEnumerator OnClick_LogIn_InputEmpty_WarningPopsUp()
        {
            yield return new WaitWhile(() => sceneLoaded == false);

            ChangeViewToLogInViewModel();

            yield return new WaitForFixedUpdate();

            logInDemoViewModel.emailInput.text = "";
            logInDemoViewModel.passwordInput.text = "";

            logInDemoViewModel.OnClick_LogIn();

            Assert.IsTrue(ScreenManager.instance.GetView(ViewID.PopUpViewModel).gameObject.activeSelf, "Warning Pop Up has appeard due to text  inputs being empty");
        }
    }
}
