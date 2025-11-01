using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibraryManagementAPI.Extensions;

public static class ControllerExtensions
{
    public static ActionResult? ValidateModelState(this ControllerBase controller)
    {
        if (!controller.ModelState.IsValid)
            return controller.BadRequest(controller.ModelState);
        
        return null;
    }
}
