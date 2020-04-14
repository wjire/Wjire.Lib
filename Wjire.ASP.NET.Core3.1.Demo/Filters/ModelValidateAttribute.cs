using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Wjire.Common;

namespace Wjire.ASP.NET.Core3._1.Demo
{

    /// <summary>
    /// 模型验证过滤器
    /// </summary>

    public class ModelValidateAttribute : IActionFilter
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
            if (context.ModelState.IsValid == false)
            {
                string result = string.Empty;
                var dic = context.ModelState.ToDictionary(t => t.Key, t => string.Join(",", t.Value.Errors.Select(s=>s.ErrorMessage)));
                foreach (Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry item in context.ModelState.Values)
                {
                    foreach (Microsoft.AspNetCore.Mvc.ModelBinding.ModelError error in item.Errors)
                    {
                        result += error.ErrorMessage + "|";
                    }
                }
                result = result.Substring(0, result.Length - 1);
                throw new CustomException(dic.ToJson());
            }
        }
    }
}
