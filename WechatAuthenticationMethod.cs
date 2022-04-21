using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.ExternalAuth.Wechat
{
    internal class WechatAuthenticationMethod : BasePlugin, IExternalAuthenticationMethod
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public WechatAuthenticationMethod(ILocalizationService localizationService,
            ISettingService settingService,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _settingService = settingService;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods
        /// <summary>
        /// 获取设定页面路径
        /// </summary>
        /// <returns></returns>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/WechatAuthenticationMethod/Configure";
        }
        /// <summary>
        /// 再公共商城获取组件视图名称
        /// </summary>
        /// <returns></returns>
        public string GetPublicViewComponentName()
        {
            return WechatAuthenticationDefaults.VIEW_COMPONENT_NAME;
        }
        /// <summary>
        /// 安装
        /// </summary>
        /// <returns></returns>
        public override async Task InstallAsync()
        {
            //settings
            await _settingService.SaveSettingAsync(new WechatExternalAuthSettings());

            //locales
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.ExternalAuth.Wechat.AppID"] = "App ID",
                ["Plugins.ExternalAuth.Wechat.AppID.Hint"] = "输入App ID.可以在微信开放平台中登陆开发者帐号查询.",
                ["Plugins.ExternalAuth.Wechat.AppSecret"] = "App Secret",
                ["Plugins.ExternalAuth.Wechat.AppSecret.Hint"] = "输入App Secret. 可以在微信开放平台中登陆开发者帐号查询.",
                ["Plugins.ExternalAuth.Wechat.Instructions"] = "<p>设置微信第三方登录，具体步骤请<br/><br/><ol><li>访问 <a href=\"https://developers.weixin.qq.com/\" target =\"_blank\" > 微信开发者</a> 请注意 <b>需要登录</b> </li><li> <b>+ 增加一个App</b> 创建新的App ID. <b></b>.)</li><li></li><li></li><li></li><li><b></b> .</li><li>拷贝AppID 和AppSecret</li></ol><br/><br/></p>"
            });

            await base.InstallAsync();
        }
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<WechatExternalAuthSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Wechat.Facebook");

            await base.UninstallAsync();
        }
        #endregion
    }
}
