///////////////////////////////////////////////////////////////////////////////
// THIS CODE AND INFORMATION ARE PROVIdED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////////

using Microsoft.Practices.Composite.Logging;

namespace OpenPOS
{
    public class EnterpriseLibraryLogger : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
            //Logger.Write(message, category.ToString(), (int)priority);
        }
    }
}
