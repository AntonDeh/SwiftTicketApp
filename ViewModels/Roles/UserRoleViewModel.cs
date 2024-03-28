﻿namespace SwiftTicketApp.ViewModels.Roles
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string CurrentRoleId { get; set; } = string.Empty;
        public string CurrentRoleName { get; set; } = "Not Assigned";
        public IEnumerable<RoleViewModel> AvailableRoles { get; set; } = new List<RoleViewModel>();

    }

}
