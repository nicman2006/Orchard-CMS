﻿
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Notify;
using Szmyd.Orchard.Modules.Sharing.Models;

namespace Szmyd.Orchard.Modules.Sharing.Drivers
{
    
    public class ShareBarSettingsPartDriver : ContentPartDriver<ShareBarSettingsPart>
    {

        public ShareBarSettingsPartDriver(
            INotifier notifier,
            IAuthorizationService authorizationService,
            IAuthenticationService authenticationService,
            IOrchardServices services)
        {
            _notifier = notifier;
            _authorizationService = authorizationService;
            _authenticationService = authenticationService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        private const string TemplateName = "Parts/Share.Settings";
        private readonly INotifier _notifier;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthenticationService _authenticationService;

        protected override DriverResult Editor(ShareBarSettingsPart part, dynamic shapeHelper)
        {
            if (!_authorizationService.TryCheckAccess(Permissions.EditSettings, _authenticationService.GetAuthenticatedUser(), part))
                return null;

            return ContentShape("Parts_Share_Settings",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(ShareBarSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (!_authorizationService.TryCheckAccess(Permissions.EditSettings, _authenticationService.GetAuthenticatedUser(), part))
                return null;

            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                _notifier.Information(T("Content sharing settings updated successfully"));
            }
            else
            {
                _notifier.Error(T("Error during content sharing settings update!"));
            }
            return Editor(part, shapeHelper);
        }
    }
}