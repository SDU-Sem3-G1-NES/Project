using Blazored.SessionStorage;
using Famicom.Components.Pages;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Famicom.Components.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        protected bool isLoggedIn;

        public string? email { get; set; }
        public int? userId { get; set; }

        [Inject]
        private ISessionStorageService? SessionStorage { get; set; }

        [Inject]
        private NavigationManager? Navigation { get; set; }
        
        [Inject] 
        private LoginStateService LoginStateService { get; set; } = default!;
        
        public NavMenu _navMenu { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            LoginStateService.OnLoginStateChanged += HandleLoginStateChanged;

            _theme = new()
            {
                PaletteLight = _lightPalette,
                PaletteDark = _darkPalette,
                LayoutProperties = new LayoutProperties()
            };
            _navMenu = new NavMenu();
            await base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CheckLogin();
            }
            else {
                await _navMenu.GetNavItems(email!);
            }
            await base.OnInitializedAsync();
        }
        

        private async Task CheckLogin()
        {
            if (SessionStorage == null || Navigation == null)
            {
                throw new Exception("Required services are missing.");
            }

            try
            {
                email = await SessionStorage.GetItemAsync<string>("Email");
                userId = await SessionStorage.GetItemAsync<int>("UserId");

                if (email == null || userId == null)
                {
                    Navigation.NavigateTo("/Login");
                    return;
                }


                isLoggedIn = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception during initialization: {ex.Message}");
                Navigation.NavigateTo("/Error");
            }
        }

        private bool _drawerOpen = true;
        private bool _isDarkMode = true;
        private MudTheme? _theme = null;

        private async Task Logout()
        {
            if (SessionStorage != null)
            {
                await SessionStorage.ClearAsync();
                LoginStateService.IsLoggedIn = false;
                isLoggedIn = false;
                Navigation?.NavigateTo("/Login");
                StateHasChanged();
            }
        }

        private async void HandleLoginStateChanged()
        {
            await CheckLogin();
        }

        private void DrawerToggle()

        {
            _drawerOpen = !_drawerOpen;
        }



        private void DarkModeToggle()
        {
            _isDarkMode = !_isDarkMode;
        }



        private readonly PaletteLight _lightPalette = new()
            {
                Black = "#110e2d",
                AppbarText = "#424242",
                AppbarBackground = "rgba(255,255,255,0.8)",
                DrawerBackground = "#ffffff",
                GrayLight = "#e8e8e8",
                GrayLighter = "#f9f9f9",
            };



        private readonly PaletteDark _darkPalette = new()
            {
                Primary = "#7e6fff",
                Surface = "#1e1e2d",
                Background = "#1a1a27",
                BackgroundGray = "#151521",
                AppbarText = "#92929f",
                AppbarBackground = "rgba(26,26,39,0.8)",
                DrawerBackground = "#1a1a27",
                ActionDefault = "#74718e",
                ActionDisabled = "#9999994d",
                ActionDisabledBackground = "#605f6d4d",
                TextPrimary = "#b2b0bf",
                TextSecondary = "#92929f",
                TextDisabled = "#ffffff33",
                DrawerIcon = "#92929f",
                DrawerText = "#92929f",
                GrayLight = "#2a2833",
                GrayLighter = "#1e1e2d",
                Info = "#4a86ff",
                Success = "#3dcb6c",
                Warning = "#ffb545",
                Error = "#ff3f5f",
                LinesDefault = "#33323e",
                TableLines = "#33323e",
                Divider = "#292838",
                OverlayLight = "#1e1e2d80",
            };



        public string DarkLightModeButtonIcon => _isDarkMode switch
        {
            true => Icons.Material.Rounded.AutoMode,
            false => Icons.Material.Outlined.DarkMode,

        };
    }
}