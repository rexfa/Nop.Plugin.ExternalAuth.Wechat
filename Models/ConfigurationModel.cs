using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.ExternalAuth.Wechat.Models
{
    /// <summary>
    /// 装载插件配置模型
    /// </summary>
    public record ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.ExternalAuth.Wechat.AppID")]
        public string AppID { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.Wechat.AppSecret")]
        public string AppSecret { get; set; }
    }
}
