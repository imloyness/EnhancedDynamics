using System.Collections.Generic;
using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.UI;
using TMPro;
using UnityEngine;

namespace EnhancedDynamics.BBP
{
    public class DefaultGeneralSettings : Singleton<DefaultGeneralSettings>
    {
        public bool fovToggle;
        public bool cameraBobbingToggle;
        public bool idleInhaleToggle;
        public bool verticalCameraToggle;
        public bool postProcessingToggle;
        public int bloomValue;
        public int vignetteValue;
        public int motionblurValue;

        public int fovValue;
        public int cameraBobbingIntensity;
        public bool debugModeToggle;

        public void RefreshFromConfig()
        {
            fovToggle = BasePlugin.FOVToggle.Value;
            cameraBobbingToggle = BasePlugin.CameraBobbingToggle.Value;
            idleInhaleToggle = BasePlugin.idleinhaleToggle.Value;
            verticalCameraToggle = BasePlugin.VerticalCameraToggle.Value;
            fovValue = BasePlugin.FOVValue.Value;
            cameraBobbingIntensity = BasePlugin.CameraBobbingIntensity.Value;
            debugModeToggle = BasePlugin.DebugModeToggle.Value;
            postProcessingToggle = BasePlugin.PostProcessingToggle.Value;
            bloomValue = BasePlugin.BloomIntensity.Value;
            vignetteValue = BasePlugin.VignetteIntensity.Value;
            motionblurValue = BasePlugin.MotionBlurIntensity.Value;
        }
    }

    public class PostProcessingSettings : CustomOptionsCategory
    {
        private readonly List<GameObject> pages = new();
        private int currentPageIndex = 0;
        private TextMeshProUGUI pageIndicatorText;

        private MenuToggle PostProcessing;
        private AdjustmentBars BloomBar;
        private AdjustmentBars VignetteBar;
        private AdjustmentBars MotionBlurBar;

        public override void Build()
        {
            TextMeshProUGUI textMeshProUGUI = CreateText("VersionText", $"\n<color=white>ED v.{PluginInfo.PLUGIN_VERSION}</color>",
                Vector3.zero, BaldiFonts.BoldComicSans12, TextAlignmentOptions.Center, new Vector2(300f, 30f), Color.white, false);
            (textMeshProUGUI.transform as RectTransform).anchoredPosition = new Vector2(-149f, -165f);
            textMeshProUGUI.gameObject.AddComponent<Modules.RainbowText>();

            GameObject page = new("PostProcessingPage");
            page.transform.SetParent(transform, false);
            page.transform.localPosition = Vector3.zero;
            pages.Add(page);

            PostProcessing = CreateNewToggle("postProcessingToggle", "Toggle", "DISCLAIMER: This setting will cause lags on low-end computers, be sure to have a good computer for that.", 6);
            PostProcessing.transform.SetParent(page.transform, true);

            MotionBlurBar = CreateNewBar("MotionBlurIntensity", "Motion Blur", "Adjust how smooth frames are", 2, 5, Color.black);
            MotionBlurBar.transform.SetParent(page.transform, true);

            BloomBar = CreateNewBar("bloomIntensity", "Bloom", "Adjust the glow intensity", 3, 5, Color.black);
            BloomBar.transform.SetParent(page.transform, true);

            VignetteBar = CreateNewBar("vignetteIntensity", "Vignette", "Adjust edge darkening", 4, 5, Color.black);
            VignetteBar.transform.SetParent(page.transform, true);


            PostProcessing.Set(DefaultGeneralSettings.Instance.postProcessingToggle);
            BloomBar.Adjust(DefaultGeneralSettings.Instance.bloomValue);
            VignetteBar.Adjust(DefaultGeneralSettings.Instance.vignetteValue);
            MotionBlurBar.Adjust(DefaultGeneralSettings.Instance.motionblurValue);

            page.SetActive(true);

            CreateButton(() => SwitchPage(-1), menuArrowLeft, menuArrowLeftHighlight, "PrevPage", new Vector3(-75f, -150f, 0f));
            CreateButton(() => SwitchPage(1), menuArrowRight, menuArrowRightHighlight, "NextPage", new Vector3(75f, -150f, 0f));

            pageIndicatorText = CreateText("PageIndicator", $"Page {currentPageIndex + 1}/{pages.Count}", new Vector3(0f, -150f, 0f),
                BaldiFonts.ComicSans24, TextAlignmentOptions.Center, new Vector2(200f, 32f), Color.black, false);
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
            BasePlugin.PostProcessingToggle.Value = PostProcessing.Value;
            BasePlugin.VignetteIntensity.Value = VignetteBar.GetRaw();
            BasePlugin.BloomIntensity.Value = BloomBar.GetRaw();
            BasePlugin.MotionBlurIntensity.Value = MotionBlurBar.GetRaw();

            PostProcessing_BBP.RefreshEffects();
        }

        public AdjustmentBars CreateNewBar(string id, string name, string tooltipKey, int order, int length, Color textColor)
        {
            Vector3 originVec = new(90f, 70f, 0f);
            var bar = CreateBars(SendData, id, originVec + new Vector3(-30, order * -29.5f, 0), length);

            TextMeshProUGUI textObj = CreateText(id + "_text", name, originVec + new Vector3(-175, order * -28, 0),
                BaldiFonts.ComicSans24, TextAlignmentOptions.Right, new Vector2(250, 10), textColor, false);
            textObj.transform.SetParent(bar.transform, true);

            AddTooltip(bar, tooltipKey);

            Transform region = toolTipHotspot?.Find(bar.name + "Region");
            if (region != null) region.SetParent(bar.transform, true);

            return bar;
        }

        public MenuToggle CreateNewToggle(string id, string name, string tooltipKey, int order)
        {
            Vector3 originVec = new(90f, 85f, 0f);
            var toggle = CreateToggle(id, name, false, originVec + new Vector3(0, order * -29.5f, 0), 250f);
            AddTooltip(toggle, tooltipKey);

            var hotspotButton = toggle.transform.Find("HotSpot")?.GetComponent<StandardMenuButton>();
            hotspotButton?.OnPress.AddListener(SendData);

            return toggle;
        }
    }

    public class GeneralSettings : CustomOptionsCategory
    {
        private readonly List<GameObject> pages = new();
        private int currentPageIndex = 0;
        private TextMeshProUGUI pageIndicatorText;

        private MenuToggle FOVToggle;
        private AdjustmentBars FOVValue;
        private MenuToggle CameraBobbing;
        private AdjustmentBars CameraBobbingIntensity;
        private MenuToggle IdleInhale;
        private MenuToggle VerticalCamera;
        private MenuToggle DebugMode;
        
        

        public override void Build()
        {
            TextMeshProUGUI textMeshProUGUI = CreateText("VersionText", $"\n<color=white>ED v.{PluginInfo.PLUGIN_VERSION}</color>",
                Vector3.zero, BaldiFonts.BoldComicSans12, TextAlignmentOptions.Center, new Vector2(300f, 30f), Color.white, false);
            (textMeshProUGUI.transform as RectTransform).anchoredPosition = new Vector2(-149f, -165f);
            textMeshProUGUI.gameObject.AddComponent<Modules.RainbowText>();

            GameObject page1 = new("Page1");
            page1.transform.SetParent(transform, false);
            page1.transform.localPosition = Vector3.zero;
            pages.Add(page1);

            GameObject page2 = new("Page2");
            page2.transform.SetParent(transform, false);
            page2.transform.localPosition = Vector3.zero;
            pages.Add(page2);

            GameObject page3 = new("Page3");
            page3.transform.SetParent(transform, false);
            page3.transform.localPosition = Vector3.zero;
            pages.Add(page3);

            // Page 1: FOV-related
            FOVValue = CreateNewBar("fovValue", "Base FOV", "Base field of view value (default: 60)", 2, 3, Color.black);
            FOVValue.transform.SetParent(page1.transform, true);

            FOVToggle = CreateNewToggle("fovToggle", "FOV Transitions", "Enables FOV transitions when running", 4);
            FOVToggle.transform.SetParent(page1.transform, true);

            // Page 2: Camera-related
            CameraBobbingIntensity = CreateNewBar("cameraBobbingIntensity", "Bobbing Intensity",
                "Adjusts the intensity of the camera bobbing effect", 1, 3, Color.black);
            CameraBobbingIntensity.transform.SetParent(page2.transform, true);

            CameraBobbing = CreateNewToggle("cameraBobbingToggle", "Camera Bobbing", "Enables camera bobbing when walking/running", 4);
            CameraBobbing.transform.SetParent(page2.transform, true);

            IdleInhale = CreateNewToggle("idleInhaleToggle", "Idle Inhale Animation",
                "Enables a subtle camera bobbing effect when idle (Requires Camera Bobbing)", 5);
            IdleInhale.transform.SetParent(page2.transform, true);

            VerticalCamera = CreateNewToggle("verticalCameraToggle", "Vertical Camera", "Your neck is finally gonna be free :fire:", 6);
            VerticalCamera.transform.SetParent(page2.transform, true);

            // Page 4: Debug-related
            DebugMode = CreateNewToggle("debugModeToggle", "Debug Mode", "(Does nothing for now)", 1);
            DebugMode.transform.SetParent(page3.transform, true);

            DefaultGeneralSettings.Instance.RefreshFromConfig();

            FOVToggle.Set(DefaultGeneralSettings.Instance.fovToggle);
            CameraBobbing.Set(DefaultGeneralSettings.Instance.cameraBobbingToggle);
            IdleInhale.Set(DefaultGeneralSettings.Instance.idleInhaleToggle);
            VerticalCamera.Set(DefaultGeneralSettings.Instance.verticalCameraToggle);
            FOVValue.Adjust(DefaultGeneralSettings.Instance.fovValue);
            CameraBobbingIntensity.Adjust(DefaultGeneralSettings.Instance.cameraBobbingIntensity);

            page1.SetActive(true);
            page2.SetActive(false);
            page3.SetActive(false);

            CreateButton(() => SwitchPage(-1), menuArrowLeft, menuArrowLeftHighlight, "PrevPage", new Vector3(-75f, -150f, 0f));
            CreateButton(() => SwitchPage(1), menuArrowRight, menuArrowRightHighlight, "NextPage", new Vector3(75f, -150f, 0f));

            pageIndicatorText = CreateText("PageIndicator", $"Page {currentPageIndex + 1}/{pages.Count}", new Vector3(0f, -150f, 0f),
                BaldiFonts.ComicSans24, TextAlignmentOptions.Center, new Vector2(200f, 32f), Color.black, false);
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
            Vector3 originVec = new(90f, 70f, 0f);
            var bar = CreateBars(SendData, id, originVec + new Vector3(-30, order * -29.5f, 0), length);

            TextMeshProUGUI textObj = CreateText(id + "_text", name, originVec + new Vector3(-175, order * -28, 0),
                BaldiFonts.ComicSans24, TextAlignmentOptions.Right, new Vector2(250, 10), textColor, false);
            textObj.transform.SetParent(bar.transform, true);

            AddTooltip(bar, tooltipKey);

            Transform region = toolTipHotspot?.Find(bar.name + "Region");
            if (region != null) region.SetParent(bar.transform, true);

            return bar;
        }

        public MenuToggle CreateNewToggle(string id, string name, string tooltipKey, int order)
        {
            Vector3 originVec = new(90f, 85f, 0f);
            var toggle = CreateToggle(id, name, false, originVec + new Vector3(0, order * -29.5f, 0), 250f);
            AddTooltip(toggle, tooltipKey);

            var hotspotButton = toggle.transform.Find("HotSpot")?.GetComponent<StandardMenuButton>();
            hotspotButton?.OnPress.AddListener(SendData);

            return toggle;
        }
    }
}