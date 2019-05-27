﻿namespace NK.API.Enum
{
    public enum  FlagsAndAttributes:uint
    {
         FILE_FLAG_WRITE_THROUGH = 0x80000000,
         FILE_FLAG_OVERLAPPED = 0x40000000,
         FILE_FLAG_NO_BUFFERING = 0x20000000,
         FILE_FLAG_RANDOM_ACCESS = 0x10000000,
         FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000,
         FILE_FLAG_DELETE_ON_CLOSE = 0x04000000,
         FILE_FLAG_BACKUP_SEMANTICS = 0x02000000,
         FILE_FLAG_POSIX_SEMANTICS = 0x01000000,
         FILE_FLAG_OPEN_REPARSE_POINT = 0x00200000,
         FILE_FLAG_OPEN_NO_RECALL = 0x00100000,
         FILE_FLAG_FIRST_PIPE_INSTANCE = 0x00080000,
    }
}
