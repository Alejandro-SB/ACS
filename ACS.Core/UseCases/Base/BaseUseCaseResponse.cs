using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.UseCases.Base
{
    public abstract class BaseUseCaseResponse
    {
        public bool Success { get; }
        public string? Message { get; }

        protected BaseUseCaseResponse(bool success = false, string? message = null)
        {
            Success = success;
            Message = message;
        }
    }
}