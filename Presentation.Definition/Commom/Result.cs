using Microsoft.AspNetCore.Mvc;

namespace Presentation.Definition.Commom
{
    public class Result
    {
        public object Value { get; set; }
        public ErrorDto Error { get; set; }
        public bool Success { get; set; }

        public ObjectResult Return()
        {
            if (Success)
                return new OkObjectResult(Value);
            else
                return  new BadRequestObjectResult(Error);
        }
    }
}