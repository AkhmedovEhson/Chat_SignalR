using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Filters
{
    public class HubExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
        public HubExceptionFilter()
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(Exception),HandleErrorException }
            };
        }
        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }
        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();

            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }
        }

        private void HandleErrorException(ExceptionContext context)
        {
            var exception = context.Exception;
            var details = exception.Message;
            context.Result = new BadRequestObjectResult(details);
            context.ExceptionHandled = true;
        }
    }

}
