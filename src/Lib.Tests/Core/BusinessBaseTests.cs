using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Core;
using Lib.Entities;

namespace Lib.Tests.Core
{
    public class Business : BusinessBase<Contact>
    {
        public Business(ILocalPluginContext context) : base(context)
        {
        }

        public override void HandleExecute()
        {
        }
    }

    public class BusinessBaseTests
    {
    }
}
