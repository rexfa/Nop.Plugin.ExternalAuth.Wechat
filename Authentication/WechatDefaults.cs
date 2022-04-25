
namespace Microsoft.AspNetCore.Authentication.Wechat
{
    /// <summary>
    /// Default values for the Wechat authentication handler.
    /// 暂时完成
    /// </summary>
    public static class WechatDefaults
    {
        /// <summary>
        /// The default scheme for Wechat authentication. The value is <c>Wechat</c>.
        /// </summary>
        public const string AuthenticationScheme = "Wechat";
        /// <summary>
        /// The default display name for Wechat authentication. Defaults to <c>Wechat</c>.
        /// </summary>
        public static readonly string DisplayName = "Wechat";

        // https://developers.weixin.qq.com/doc/oplatform/Website_App/WeChat_Login/Wechat_Login.html


        /// <summary>
        /// 第一步 请求CODE ***
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://open.weixin.qq.com/connect/qrconnect";
        /// <summary>
        /// 第二步 通过code获取access_token ***
        /// </summary>
        public static readonly string TokenEndpoint = "https://api.weixin.qq.com/sns/oauth2/access_token";
        /// <summary>
        /// 第三步 获取用户个人信息 ***
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://api.weixin.qq.com/sns/userinfo";


        /// <summary>
        /// 第三步后 刷新access_token有效期
        /// </summary>
        public static readonly string RefreshTokenEndpoint = "https://api.weixin.qq.com/sns/oauth2/refresh_token";
        /// <summary>
        /// 第三步后 检查access_token有效性
        /// </summary>
        public static readonly string CheckTokenEndpoint = "https://api.weixin.qq.com//sns/auth";
    }
}
