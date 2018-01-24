using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    interface IMember
    {
        void Join(Client client);
        void Exit();
        void Notify();
    }
}
