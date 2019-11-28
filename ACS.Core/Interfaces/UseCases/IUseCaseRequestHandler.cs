using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Core.Interfaces.UseCases
{
    public interface IUseCaseRequestHandler<in TUseCaseRequest, out TUseCaseResponse> 
        where TUseCaseRequest : IUseCaseRequest<TUseCaseResponse>
    {
        Task<bool> Handle(TUseCaseRequest message, IOutputPort<TUseCaseResponse> outputPort);
    }
}