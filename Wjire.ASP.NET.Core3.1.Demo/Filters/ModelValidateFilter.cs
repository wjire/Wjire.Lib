using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wjire.ASP.NET.Core3._1.Demo
{

    /// <summary>
    /// 模型验证过滤器
    /// </summary>

    public class ModelValidateFilter : IActionFilter
    {

        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext" />.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// 入参验证
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == true) return;
            string result = string.Empty;
            foreach (ModelStateEntry item in context.ModelState.Values)
            {
                foreach (ModelError error in item.Errors)
                {
                    result += error.ErrorMessage + "|";
                }
            }
            result = result.Substring(0, result.Length - 1);
            context.Result = new JsonResult(result);
        }
    }
}
