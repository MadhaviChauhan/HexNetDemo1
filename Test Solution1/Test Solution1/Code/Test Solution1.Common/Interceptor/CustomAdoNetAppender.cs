using log4net.Appender;
using System.Configuration;

namespace Test_Solution1.Common.Interceptor
{
    public class CustomAdoNetAppender : AdoNetAppender
    {
        public override void ActivateOptions()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["APPNAME"].ConnectionString;
            base.ActivateOptions();
        }
    }

}

public class CustomAdoNetAppender : AdoNetAppender
{
    public override void ActivateOptions()
    {
        ConnectionString = ConfigurationManager.AppSettings["IPC"];
        base.ActivateOptions();
    }
}

