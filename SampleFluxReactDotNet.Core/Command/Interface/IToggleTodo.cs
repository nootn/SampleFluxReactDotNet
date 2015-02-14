using System;
using DotNetAppStarterKit.Core.Command;

namespace SampleFluxReactDotNet.Core.Command.Interface
{
    public interface IToggleTodo : ICommand<Tuple<Guid, bool>>
    {
    }
}