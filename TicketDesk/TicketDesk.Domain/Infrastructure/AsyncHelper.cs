// TicketDesk - Attribution notice
// 
// Current Code provided adapted from internal implementations within the asp.net identity framework
//
// Previous version by:
//       John Melville - http://stackoverflow.com/questions/5095183/how-would-i-run-an-async-taskt-method-synchronously
//    
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.


using System;
using System.Threading;
using System.Threading.Tasks;

namespace TicketDesk.Domain
{

    public static class AsyncHelper
    {
        private static readonly TaskFactory TaskContext = new
            TaskFactory(CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return TaskContext
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            TaskContext
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }
    }
}
