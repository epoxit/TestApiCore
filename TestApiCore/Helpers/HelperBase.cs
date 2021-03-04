using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PublicApi
{
    public class HelperBase
    {
        protected AppManager app;
        public HelperBase(AppManager Manager)
        {
            this.app = Manager;
        }
    }
}

