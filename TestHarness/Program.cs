using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jones.FileLogger;
using Jones.Logger;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new FileLogger("myPrecious.txt",LogLevel.Warning);

            logger.Log(LogLevel.Info, "Info!!");
            logger.Log(LogLevel.Warning, "Warning!!");

            try
            {
                try
                {
                    throw new AccessViolationException("uh oh");
                }
                catch (Exception ex)
                {
                    throw new Exception("yooo", ex);
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, "Exception!!", ex);
            }

        }
    }
}
