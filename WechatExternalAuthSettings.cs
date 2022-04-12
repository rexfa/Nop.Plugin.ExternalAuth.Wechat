using Nop.Core.Configuration;


namespace Nop.Plugin.ExternalAuth.Wechat
{
    /// <summary>
    /// 微信的外部授权设置
    /// </summary>
    public class WechatExternalAuthSettings : ISettings
    {
        /// <summary>
        /// 微信端获取的APPID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string AppSecret { get; set; }
    }
}