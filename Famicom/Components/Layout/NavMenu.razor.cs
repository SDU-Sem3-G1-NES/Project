using System.Runtime.InteropServices;
using Blazored.SessionStorage;
using DataAccess;
using Famicom.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Famicom.Components.Layout
{
    public partial class NavMenu : ComponentBase
    {
        [Parameter] public List<NavItem> NavItems { get; set; } = new List<NavItem>();

        private bool _isInitliased = false;

        private UserModel userModel { get; set; } = null!;
        [Inject] public ISnackbar? Snackbar { get; set; }
        [Inject] private ISessionStorageService SessionStorage { get; set; } = default!;

        protected override Task OnInitializedAsync()
        {
            _isInitliased = true;
            return base.OnInitializedAsync();
        }

        public async Task GetNavItems(string email)
        {
            if(SessionStorage == null) return;
            var isLoggedIn = await SessionStorage.GetItemAsync<bool>("IsLoggedIn");
            if(!_isInitliased || !isLoggedIn) return;
            userModel = new UserModel();
            if(email == null) {
                Snackbar!.Add("Email not found", Severity.Error);
            }

            var user = userModel.GetUser(email!);
            if(user == null) {
                Snackbar!.Add("User not found", Severity.Error);
            }

            var userType = user!.GetType().Name;
            NavItems = new List<NavItem>();

            switch (userType)
            {
                case "Admin":
                    NavItems.Add(new NavItem("Dashboard", Icons.Material.Filled.Dashboard, "/Dashboard"));
                    NavItems.Add(new NavItem("Health", Icons.Material.Filled.HealthAndSafety, "health"));
                    NavItems.Add(new NavItem("Cleaning", Icons.Material.Filled.CleaningServices, "cleaning"));
                    NavItems.Add(new NavItem("Administration", Icons.Material.Filled.AdminPanelSettings, "admin"));
                    NavItems.Add(new NavItem("Settings", Icons.Material.Filled.Settings, "settings"));
                    break;
                case "Employee":
                    NavItems.Add(new NavItem("Dashboard", Icons.Material.Filled.Dashboard, "/Dashboard"));
                    NavItems.Add(new NavItem("Health", Icons.Material.Filled.HealthAndSafety, "health"));
                    NavItems.Add(new NavItem("Settings", Icons.Material.Filled.Settings, "settings"));
                    break;
                case "Cleaner":
                    NavItems.Add(new NavItem("Cleaning", Icons.Material.Filled.CleaningServices, "cleaning"));
                    NavItems.Add(new NavItem("Settings", Icons.Material.Filled.Settings, "settings"));
                    break;
                default:
                    Snackbar!.Add("Invalid user type", Severity.Error);
                    break;
            }
            StateHasChanged();
            await Task.CompletedTask;
        }
    }

    public class NavItem
    {
        public string Name { get; private set; }
        public string Icon { get; private set; }
        public string Url { get; private set; }

        public NavItem(string name, string icon, string url)
        {
            Name = name;
            Icon = icon;
            Url = url;
        }
    }
}