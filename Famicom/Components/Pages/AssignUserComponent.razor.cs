using Microsoft.AspNetCore.Components;

namespace Famicom.Components.Pages
{
    public partial class AssignUserComponent : ComponentBase
    {
        private bool AddUserVisible { get; set; }

        public AssignUserComponent()
        {
            AddUserVisible = false;
        }


    }
}
