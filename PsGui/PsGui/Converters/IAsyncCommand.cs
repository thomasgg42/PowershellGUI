﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsGui.Converters
{
    /// <summary>
    /// An Async implmentation of ICommand
    /// </summary>
    public interface IAsyncCommand<T> : System.Windows.Input.ICommand
    {
        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The executed Task</returns>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        System.Threading.Tasks.Task ExecuteAsync(T parameter);
    }

    /// <summary>
    /// An Async implmentation of ICommand
    /// </summary>
    public interface IAsyncCommand : System.Windows.Input.ICommand
    {
        /// <summary>
        /// Executes the Command as a Task
        /// </summary>
        /// <returns>The executed Task</returns>
        System.Threading.Tasks.Task ExecuteAsync();
    }
}
