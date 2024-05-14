using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberSystemTest
{
    [CollectionDefinition (nameof (DisableParallelizationCollection), DisableParallelization = true)]
    public class DisableParallelizationCollection
    {
    }
}
