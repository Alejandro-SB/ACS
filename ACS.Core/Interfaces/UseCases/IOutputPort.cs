using System;
using System.Collections.Generic;
using System.Text;

namespace ACS.Core.Interfaces.UseCases
{
    public interface IOutputPort<in TUseCaseResponse>
    {
        void Handle(TUseCaseResponse response);
    }
}