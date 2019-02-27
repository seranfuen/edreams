using System;

namespace eDream.libs
{
    public class FinishedPersistingEventArgs : EventArgs
    {
        public PersistenceOperationResult Result { get; }

        public FinishedPersistingEventArgs(PersistenceOperationResult result)
        {
            Result = result;
        }
    }
}