using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AsyncLogging
{
    public interface IWriteLogData<T>: IComparable<IWriteLogData<T>>, IComparer<IWriteLogData<T>>, IDisposable where T: class, new()
    {
        string Name { get; }

        bool IsActive { get; set; }

        int SortOrder { get; set; }

        bool CanWriteLogData(T logData);

        void WriteLogData(T logData);
    }
}
