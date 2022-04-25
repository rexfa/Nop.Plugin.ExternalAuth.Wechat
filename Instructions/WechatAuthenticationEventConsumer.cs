using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Services.Authentication.External;
using Nop.Services.Common;
using Nop.Services.Events;

namespace Nop.Plugin.ExternalAuth.Wechat.Instructions
{
    /// <summary>
    /// 客户Wechat 身份验证事件（用于在注册时保存客户字段）
    /// </summary>
    public class WechatAuthenticationEventConsumer : IConsumer<CustomerAutoRegisteredByExternalMethodEvent>
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;

        #endregion

        #region Ctor

        public WechatAuthenticationEventConsumer(IGenericAttributeService genericAttributeService)
        {
            _genericAttributeService = genericAttributeService;
        }

        #endregion
        #region Methods
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventMessage"></param>
        /// <returns>异步操作的任务</returns>
        public async Task HandleEventAsync(CustomerAutoRegisteredByExternalMethodEvent eventMessage)
        {
            if (eventMessage?.Customer == null || eventMessage.AuthenticationParameters == null)
                return;

            //handle event only for this authentication method
            if (!eventMessage.AuthenticationParameters.ProviderSystemName.Equals(WechatAuthenticationDefaults.SystemName))
                return;

            //store some of the customer fields
            var firstName = eventMessage.AuthenticationParameters.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.GivenName)?.Value;
            if (!string.IsNullOrEmpty(firstName))
                await _genericAttributeService.SaveAttributeAsync(eventMessage.Customer, NopCustomerDefaults.FirstNameAttribute, firstName);

            var lastName = eventMessage.AuthenticationParameters.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Surname)?.Value;
            if (!string.IsNullOrEmpty(lastName))
                await _genericAttributeService.SaveAttributeAsync(eventMessage.Customer, NopCustomerDefaults.LastNameAttribute, lastName);
        }
        #endregion
    }
}
