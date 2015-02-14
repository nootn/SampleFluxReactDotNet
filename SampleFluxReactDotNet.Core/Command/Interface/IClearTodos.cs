using System;
using DotNetAppStarterKit.Core.Command;

namespace SampleFluxReactDotNet.Core.Command.Interface
{
    public interface IClearTodos : ICommand<Guid[]>
    {
    }
}