using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerScience.Server.Web.Data.ProblemSet
{
    public class ProblemSet
    {
        public DbConnection Connection { get; }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            Connection.Dispose();
        }

        private void Handle(CancellationToken cancellationToken)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(ProblemSet));
        }

        public async Task FetchProblem(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
        }
    }
}
