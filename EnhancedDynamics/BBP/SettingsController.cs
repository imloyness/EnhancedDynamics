using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EnhancedDynamics
{
    public class DefaultCategorySettings : Singleton<DefaultCategorySettings>
    {
        public bool fovToggle;
        public bool cameraBobbingToggle;
        public bool idleInhaleToggle;
        public bool verticalCameraToggle;
        public int fovValue;
        public int cameraBobbingIntensity;

        public void RefreshFromConfig()
        {
            fovToggle = BasePlugin.FOVToggle.Value;
            cameraBobbingToggle = BasePlugin.CameraBobbingToggle.Value;
            idleInhaleToggle = BasePlugin.idleinhaleToggle.Value;
            verticalCameraToggle = BasePlugin.VerticalCameraToggle.Value;
            fovValue = BasePlugin.FOVValue.Value;
            cameraBobbingIntensity = BasePlugin.CameraBobbingIntensity.Value;
        }
    }

    public class CategorySettings : CustomOptionsCategory
    {
        private List<GameObject> pages = new List<GameObject>();
        private int currentPageIndex = 0;
        private TextMeshProUGUI pageIndicatorText;

        MenuToggle FOVToggle;
        AdjustmentBars FOVValue;
        MenuToggle CameraBobbing;
        AdjustmentBars CameraBobbingIntensity;
        MenuToggle IdleInhale;
        MenuToggle VerticalCamera;

        public override void Build()
        {
            TextMeshProUGUI textMeshProUGUI = CreateText("VersionText", "\n<color=white>v1.2.0</color>", Vector3.zero, BaldiFonts.BoldComicSans12, TextAlignmentOptions.Center, new Vector2(300f, 30f), Color.white, false);
            (textMeshProUGUI.transform as RectTransform).anchoredPosition = new Vector2(-160f, -165f);
            textMeshProUGUI.gameObject.AddComponent<RainbowText>();

            GameObject page1 = new GameObject("Page1");
            page1.transform.SetParent(transform, false);
            page1.transform.localPosition = Vector3.zero;
            pages.Add(page1);

            GameObject page2 = new GameObject("Page2");
            page2.transform.SetParent(transform, false);
            page2.transform.localPosition = Vector3.zero;
            pages.Add(page2);

            // Page 1 FOV-related
            FOVValue = CreateNewBar("fovValue", "Base FOV", "Base field of view value (default: 60)", 2, 3, Color.black);
            FOVValue.transform.SetParent(page1.transform, true);

            FOVToggle = CreateNewToggle("fovToggle", "FOV Transitions", "Enables FOV transitions when running, making the game feels more intensive", 4);
            FOVToggle.transform.SetParent(page1.transform, true);

            // Page 2 camera-related
            CameraBobbingIntensity = CreateNewBar("cameraBobbingIntensity", "Bobbing Intensity", "Adjusts the intensity of the camera bobbing effect", 1, 3, Color.black);
            CameraBobbingIntensity.transform.SetParent(page2.transform, true);

            CameraBobbing = CreateNewToggle("cameraBobbingToggle", "Camera Bobbing", "Enables camera bobbing when walking/running", 4);
            CameraBobbing.transform.SetParent(page2.transform, true);

            IdleInhale = CreateNewToggle("idleInhaleToggle", "Idle Inhale Animation", "Enables a subtle camera bobbing effect when idle to simulate breathing", 5);
            IdleInhale.transform.SetParent(page2.transform, true);

            VerticalCamera = CreateNewToggle("verticalCameraToggle", "Vertical Camera", "Your neck is finally gonna be free :fire:", 6);
            VerticalCamera.transform.SetParent(page2.transform, true);

            DefaultCategorySettings.Instance.RefreshFromConfig();
            FOVToggle.Set(DefaultCategorySettings.Instance.fovToggle);
            CameraBobbing.Set(DefaultCategorySettings.Instance.cameraBobbingToggle);
            IdleInhale.Set(DefaultCategorySettings.Instance.idleInhaleToggle);
            VerticalCamera.Set(DefaultCategorySettings.Instance.verticalCameraToggle);
            FOVValue.Adjust(DefaultCategorySettings.Instance.fovValue);
            CameraBobbingIntensity.Adjust(DefaultCategorySettings.Instance.cameraBobbingIntensity);

            page1.SetActive(true);
            page2.SetActive(false);

            StandardMenuButton leftBtn = CreateButton(() => SwitchPage(-1), menuArrowLeft, menuArrowLeftHighlight, "PrevPage", new Vector3(-75f, -150f, 0f));
            StandardMenuButton rightBtn = CreateButton(() => SwitchPage(1), menuArrowRight, menuArrowRightHighlight, "NextPage", new Vector3(75f, -150f, 0f));

            pageIndicatorText = CreateText("PageIndicator", $"Page {currentPageIndex + 1}/{pages.Count}", new Vector3(0f, -150f, 0f), BaldiFonts.ComicSans24, TextAlignmentOptions.Center, new Vector2(200f, 32f), Color.black, false);
        }

        private void SwitchPage(int delta)
        {
            pages[currentPageIndex].SetActive(false);
            currentPageIndex = (currentPageIndex + delta + pages.Count) % pages.Count;
            pages[currentPageIndex].SetActive(true);

            if (pageIndicatorText != null)
            {
                pageIndicatorText.text = $"Page {currentPageIndex + 1}/{pages.Count}";
            }
        }

        public void SendData()
        {
            BasePlugin.FOVToggle.Value = FOVToggle.Value;
            BasePlugin.CameraBobbingToggle.Value = CameraBobbing.Value;
            BasePlugin.idleinhaleToggle.Value = IdleInhale.Value;
            BasePlugin.VerticalCameraToggle.Value = VerticalCamera.Value;
            BasePlugin.FOVValue.Value = FOVValue.GetRaw();
            BasePlugin.CameraBobbingIntensity.Value = CameraBobbingIntensity.GetRaw();
        }

        public AdjustmentBars CreateNewBar(string id, string name, string tooltipKey, int order, int length, Color textColor)
        {
            Vector3 originVec = new Vector3(90f, 70f, 0f);
            var bar = CreateBars(() =>
            {
                SendData();
            }, id, originVec + new Vector3(-30, order * -29.5f, 0), length);

            TextMeshProUGUI textObj = CreateText(id + "_text", name, originVec + new Vector3(-175, order * -28, 0), MTM101BaldAPI.UI.BaldiFonts.ComicSans24, TextAlignmentOptions.Right, new Vector2(250, 10), textColor, false);
            textObj.transform.SetParent(bar.transform, true);

            AddTooltip(bar, tooltipKey);

            Transform region = toolTipHotspot?.Find(bar.name + "Region");
            region.SetParent(bar.transform, true);

            return bar;
        }


        public MenuToggle CreateNewToggle(string id, string name, string tooltipKey, int order)
        {
            Vector3 originVec = new Vector3(90f, 85f, 0f);
            var toggle = CreateToggle(id, name, false, originVec + new Vector3(0, order * -29.5f, 0), 250f);
            AddTooltip(toggle, tooltipKey);

            var hotspotButton = toggle.transform.Find("HotSpot")?.GetComponent<StandardMenuButton>();
            hotspotButton.OnPress.AddListener(() => SendData());

            return toggle;
        }
    }
}