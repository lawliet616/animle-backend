using Microsoft.AspNetCore.Mvc;

namespace Animle.interfaces
{
    public  class SimpleResponse
    {
       public  string Response { get; set; }

       public bool IsSuccess { get; set; }
    }
}
